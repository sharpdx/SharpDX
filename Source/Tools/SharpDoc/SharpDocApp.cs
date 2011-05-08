// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Options;
using RazorEngine;
using SharpDoc.Model;
using SharpDoc.RazorExtensions;

namespace SharpDoc
{
    /// <summary>
    /// SharpDoc application.
    /// </summary>
    public class SharpDocApp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDocApp"/> class.
        /// </summary>
        public SharpDocApp()
        {
            Config = new Config();
        }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>The config.</value>
        public Config Config { get; set; }

        /// <summary>
        /// Print usages the error.
        /// </summary>
        /// <param name="error">The error.</param>
        private static void UsageError(string error)
        {
            var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            Console.Write("{0}: ", exeName);
            Console.WriteLine(error);
            Console.WriteLine("Use {0} --help' for more information.", exeName);
            Environment.Exit(1);
        }

        /// <summary>
        /// Parses the command line arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void ParseArguments(string[] args)
        {
            var showHelp = false;

            var options = new OptionSet()
                              {
                                  "Copyright (c) 2010-2011 SharpDX - Alexandre Mutel",
                                  "Usage: SharpDoc [options]* [--config file.xml | Assembly1.dll Assembly1.xml...]*",
                                  "Documentation generator for .Net languages",
                                  "",
                                  "options:",
                                  {"c|config=", "Configuration file", opt => Config = Config.Load(opt)},

                                  {
                                      "D=", "Define a template parameter with an (optional) value.",
                                      (param, value) =>
                                          {
                                              if (param == null)
                                                  throw new OptionException("Missing parameter name for option -D.", "-D");
                                              Config.Parameters.Add(new ConfigParam(param, value));
                                          }
                                      },
                                  {
                                      "S=", "Define a style parameter with a (optional) value.",
                                      (style, value) =>
                                          {
                                              if (style == null)
                                                  throw new OptionException("Missing parameter name/value for option -S.", "-S");
                                              Config.StyleParameters.Add(new ConfigParam(style, value));
                                          }
                                      },
                                  {"s|style=", "Specify the style to use [default: Standard]", opt => Config.StyleName = opt},
                                  {"o|output=", "Specify the output directory [default: Output]", opt => Config.OutputDirectory = opt},
                                  {"r|references=", "Add reference assemblies in order to load source assemblies", opt => Config.References.Add(opt)},
                                  "",
                                  {"h|help", "Show this message and exit", opt => showHelp = opt != null},
                                  "",
                                  "[Assembly1.dll Assembly1.xml...] Source files, if a config file is not specified, load source assembly and xml from the specified list of files",
                                  // default
                                  {"<>", opt => Config.Sources.AddRange(opt.Split(' ', '\t')) },
                              };           
            try
            {
                options.Parse(args);
            }
            catch (OptionException e)
            {
                UsageError(e.Message);
            }

            if (showHelp)
            {
                options.WriteOptionDescriptions(Console.Out);
                Environment.Exit(0);
            }

            if (Config.Sources.Count == 0)
                UsageError("At least one option is missing. Either a valid config file (-config) or a direct list of assembly/xml files must be specified");

            foreach (var m in Config.Parameters)
            {
                Console.WriteLine("\t{0}={1}", m.Name, m.value);
            }
            Console.WriteLine("Options:");
            Console.WriteLine("\tOuptut File: {0}", Config.OutputDirectory);
            foreach (var source in Config.Sources)
            {
                Console.WriteLine("\tSource File: {0}", source);                
            }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            // Force loading of dynamics
            bool loaded = typeof(Microsoft.CSharp.RuntimeBinder.Binder).Assembly != null;

            // args = GetFiles(args);

            var assemblyManager = new AssemblyManager();            
            foreach (var arg in Config.Sources)
                assemblyManager.Sources.Add(arg);

            foreach (var arg in Config.References)
                assemblyManager.References.Add(arg);

            var modelProcessor = new ModelProcessor();
            modelProcessor.Run(assemblyManager.Load());

            var context = new TemplateContext
            {
                Assemblies = new List<NAssembly>(modelProcessor.Assemblies),
                ClassLibraryName = "SharpDX Class Library",
                Registry = modelProcessor.Registry
            };

            // Add parameters
            if (Config.Parameters.Count > 0)
            {
                var dictionary = (DynamicParam) context.Param;
                foreach (var configParam in Config.Parameters)
                {                   
                    dictionary.Properties.Remove(configParam.Name);
                    dictionary.Properties.Add(configParam.Name, configParam.value);
                }
            }

            // Add styles
            if (Config.StyleParameters.Count > 0)
            {
                var dictionary = (IDictionary<string, object>)context.Style;
                foreach (var configParam in Config.StyleParameters)
                {
                    dictionary.Remove(configParam.Name);
                    dictionary.Add(configParam.Name, configParam.value);
                }
            }

            Razor.SetTemplateBase(typeof(TemplateHelperBase));
            Razor.AddResolver(context);

            context.UseStyle(Config.StyleName, false);

            context.RemoveOutputDirectory();
            context.Parse("Main");            
        }

        static public string[] GetFiles(string[] patterns)
        {
            List<string> filelist = new List<string>();
            foreach (string pattern in patterns)
                filelist.AddRange(GetFiles(pattern));
            string[] files = new string[filelist.Count];
            filelist.CopyTo(files, 0);
            return files;
        }

        static public string[] GetFiles(string patternlist)
        {
            List<string> filelist = new List<string>();
            foreach (string pattern in
                patternlist.Split(Path.GetInvalidPathChars()))
            {
                string dir = Path.GetDirectoryName(pattern);
                if (String.IsNullOrEmpty(dir)) dir =
                     Directory.GetCurrentDirectory();
                filelist.AddRange(Directory.GetFiles(
                    Path.GetFullPath(dir),
                    Path.GetFileName(pattern)));
            }
            string[] files = new string[filelist.Count];
            filelist.CopyTo(files, 0);
            return files;
        }
    }
}