// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Mono.Options;

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

            var files = new List<string>();

            var options = new OptionSet()
                              {
                                  "Copyright (c) 2010-2013 SharpDX - Alexandre Mutel",
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
                                  {"s|style=", "Specify the style to use [default: Standard]", opt => Config.StyleNames.Add(opt)},
                                  {"o|output=", "Specify the output directory [default: Output]", opt => Config.OutputDirectory = opt},
                                  {"r|references=", "Add reference assemblies in order to load source assemblies", opt => Config.References.Add(opt)},
                                  "",
                                  {"h|help", "Show this message and exit", opt => showHelp = opt != null},
                                  "",
                                  "[Assembly1.dll Assembly1.xml...] Source files, if a config file is not specified, load source assembly and xml from the specified list of files",
                                  // default
                                  {"<>", opt => files.AddRange(opt.Split(' ', '\t')) },
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

            // Add files from command line
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    var configSource = new ConfigSource();
                    var ext = Path.GetExtension(file);
                    if (ext != null && ext.ToLower() == ".xml")
                    {
                        configSource.DocumentationPath = file;
                    }
                    else
                    {
                        configSource.AssemblyPath = file;
                    }

                    Config.Sources.Add(configSource);
                }
            }

            if (Config.Sources.Count == 0)
                UsageError("At least one option is missing. Either a valid config file (-config) or a direct list of assembly/xml files must be specified");

            // Add default style Standard if none is defined
            if (Config.StyleNames.Count == 0)
                Config.StyleNames.Add("Standard");

            // Verify the validity of the style
            foreach (var styleName in Config.StyleNames)
            {
                if (!StyleManager.StyleExist(styleName))
                    UsageError("Style [{0}] does not exist. Use --help to have a list of available styles.", styleName);
            }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            // Force loading of dynamics for RazorEngine
            bool loaded = typeof(Microsoft.CSharp.RuntimeBinder.Binder).Assembly != null;
            
            var clock = Stopwatch.StartNew();

            // New instance of a template context used by the RazorEngine
            var context = new TemplateContext
            {
                Config = Config,
                StyleManager = StyleManager,
            };

            // Setup the context based on the config and StyleManager
            context.Initialize();

            // Delete output directory first
            try
            {
                if (Directory.Exists(context.OutputDirectory))
                    Directory.Delete(context.OutputDirectory, true);
            } catch
            {
            }

            // Verify the validity of the style
            foreach (var styleName in Config.StyleNames)
            {
                Logger.Message("-------------------------------------------------------------------------------");
                Logger.Message("Generating documentation using [{0}] style", styleName);
                Logger.Message("-------------------------------------------------------------------------------");
                context.UseStyle(styleName);
                context.Parse(StyleDefinition.DefaultBootableTemplateName);
            }

            Logger.Message("Total time: {0:F1}s", clock.ElapsedMilliseconds / 1000.0f);
            //Logger.Message("Time for assembly processing: {0:F1}s", timeForModelProcessor/1000.0f);
            //Logger.Message("Time for writing content: {0:F1}s", timeForWriting/1000.0f);

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