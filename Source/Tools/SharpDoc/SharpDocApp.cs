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
using SharpCore.Logging;
using SharpDoc.Model;
using SharpDocPak;

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
            StyleManager = new StyleManager();
        }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>The config.</value>
        public Config Config { get; set; }

        /// <summary>
        /// Gets or sets the style manager.
        /// </summary>
        /// <value>The style manager.</value>
        public StyleManager StyleManager { get; set; }

        /// <summary>
        /// Print usages the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="parameters">The parameters.</param>
        private static void UsageError(string error, params object[] parameters)
        {
            var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            Console.Write("{0}: ", exeName);
            Console.WriteLine(error, parameters);
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
                                  {"d|style-dir=", "Add a style directory", opt => Config.StyleDirectories.Add(opt) },
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

                StyleManager.Init(Config);
            }
            catch (OptionException e)
            {
                UsageError(e.Message);
            }

            if (showHelp)
            {
                options.WriteOptionDescriptions(Console.Out);
                StyleManager.WriteAvailaibleStyles(Console.Out);
                Environment.Exit(0);
            }

            if (Config.Sources.Count == 0)
                UsageError("At least one option is missing. Either a valid config file (-config) or a direct list of assembly/xml files must be specified");

            // Verify the validity of the style
            if (!StyleManager.StyleExist(Config.StyleName))
                UsageError("Style [{0}] does not exist. Use --help to have a list of available styles.", Config.StyleName);
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            // Force loading of dynamics for RazorEngine
            bool loaded = typeof(Microsoft.CSharp.RuntimeBinder.Binder).Assembly != null;

            // Process the assemblies
            var modelProcessor = new ModelProcessor {AssemblyManager = new MonoCecilAssemblyManager(), ModelBuilder = new MonoCecilModelBuilder()};
            modelProcessor.Run(Config);

            if (Logger.HasErrors)
                Logger.Fatal("Too many errors in config file. Check previous message.");

            // Build the topics
            var topicBuilder = new TopicBuilder() {Assemblies = modelProcessor.Assemblies, Registry = modelProcessor.Registry, RootTopic = Config.RootTopic};
            topicBuilder.Run();

            // New instance of a tempalte context used by the RazorEngine
            var context = new TemplateContext
            {
                Assemblies = new List<NAssembly>(modelProcessor.Assemblies),
                Registry = modelProcessor.Registry,
                RootTopic = topicBuilder.RootTopic,
                SearchTopic = topicBuilder.SearchTopic,
                StyleManager = StyleManager,
                OutputDirectory =  Config.AbsoluteOutputDirectory
            };

            if (Logger.HasErrors)
                Logger.Fatal("Too many errors in config file. Check previous message.");

            // Set title
            context.Param.DocumentationTitle = Config.Title;

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

            // Delete output directory first
            Directory.Delete(context.OutputDirectory, true);

            Razor.SetTemplateBase(typeof(TemplateHelperBase));
            Razor.AddResolver(context);

            context.UseStyle(Config.StyleName);
          
            context.Parse(StyleDefinition.DefaultBootableTemplateName);
        
            if ((Config.OutputType & OutputType.DocPak) != 0 )
                GenerateDocPak();
        }

        private void GenerateDocPak()
        {
            if (Config.DocPak == null)
            {
                Logger.Error("Docpak config not found from the config file. Cannot generate docpak");
                return;
            }

            var sharpDocPak = new SharpDocPakApp
                                  {
                                      Output = Config.DocPak.Name,
                                      CommandType = CommandType.Pak,
                                      DirectoryLocation = Config.AbsoluteOutputDirectory,
                                      Tags = Config.DocPak.Tags,
                                      Title = Config.Title
                                  };

            sharpDocPak.Run();
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