// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Linq;
using System.Reflection;
using Mono.Options;
using SharpGen.Logging;
using SharpGen.Config;
using SharpGen.Generator;
using SharpGen.Parser;

namespace SharpGen
{
    /// <summary>
    /// CodeGen Application.
    /// </summary>
    public class CodeGenApp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenApp"/> class.
        /// </summary>
        public CodeGenApp()
        {
            Macros = new HashSet<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generating doc.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is generating doc; otherwise, <c>false</c>.
        /// </value>
        public bool IsGeneratingDoc { get; set; }

        /// <summary>
        /// Gets or sets the path to a C++ document provider assembly.
        /// </summary>
        /// <value>The path to a C++ document provider assembly.</value>
        public string DocProviderAssemblyPath { get; set; }

        /// <summary>
        /// Gets or sets the CastXML executable path.
        /// </summary>
        /// <value>The CastXML executable path.</value>
        public string CastXmlExecutablePath { get; set; }

        /// <summary>
        /// Gets or sets the path to the Visual C++ toolset
        /// </summary>
        /// <value>The Visual C++ toolset path</value>
        public string VcToolsPath { get; set; }

        /// <summary>
        /// Gets or sets the app configuration.
        /// </summary>
        /// <value>The application configuration</value>
        public string AppType { get; set; }

        /// <summary>
        /// Gets or sets the macros.
        /// </summary>
        /// <value>
        /// The macros.
        /// </value>
        public HashSet<string> Macros { get; set; }

        private ConfigFile Config { get; set; }

        private string _thisAssemblyPath;
        private bool _isAssemblyNew;
        private DateTime _assemblyDatetime;
        private string _assemblyCheckFile;
        private string _generatedPath;
        private string _allConfigCheck;
        private string _configRootPath;

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
                                  "Copyright (c) 2010-2014 SharpDX - Alexandre Mutel",
                                  "Usage: SharpGen [options] config_file.xml",
                                  "Code generator from C++ to C# for .Net languages",
                                  "",
                                  {"c|castxml=", "Specify the path to castxml.exe", opt => CastXmlExecutablePath = opt},
                                  {"d|doc", "Specify to generate the documentation [default: false]", opt => IsGeneratingDoc = true},
                                  {"p|docpath=", "Specify the path to the assembly doc provider [default: null]", opt => DocProviderAssemblyPath = opt},
                                  {"v|vctools=", "Specify the path to the Visual C++ Toolset", opt => VcToolsPath = opt },
                                  {"a|apptype=", "Specify what app type to generate code for (i.e. DESKTOP_APP or STORE_APP)", opt => AppType = opt },
                                  "",
                                  {"h|help", "Show this message and exit", opt => showHelp = opt != null},
                                  // default
                                  {"<>", opt => _configRootPath = opt },
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

            if (_configRootPath == null)
                UsageError("Missing config.xml. A config.xml must be specified");
        }

        /// <summary>
        /// Initializes the specified instance with a config root file.
        /// </summary>
        /// <returns>true if the config or assembly changed from the last run; otherwise returns false</returns>
        public bool Init()
        {
            _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            _assemblyCheckFile = Path.ChangeExtension(_thisAssemblyPath, ".check-" + AppType);
            _assemblyDatetime = File.GetLastWriteTime(_thisAssemblyPath);
            _isAssemblyNew = (File.GetLastWriteTime(_thisAssemblyPath) != File.GetLastWriteTime(_assemblyCheckFile));
            _generatedPath = Path.GetDirectoryName(_configRootPath);

            Logger.Message("Loading config files...");

            Macros.Add("DIRECTX11_1");
            Macros.Add(AppType);

            Config = ConfigFile.Load(_configRootPath, Macros.ToArray(), new KeyValue("VC_TOOLS_PATH", VcToolsPath));
            var latestConfigTime = ConfigFile.GetLatestTimestamp(Config.ConfigFilesLoaded);

            _allConfigCheck = Config.Id + "-" + AppType + "-CodeGen.check";

            var isConfigFileChanged = !File.Exists(_allConfigCheck) || latestConfigTime > File.GetLastWriteTime(_allConfigCheck);

            if(_isAssemblyNew)
            {
                Logger.Message("Assembly [{0}] changed. All files will be generated", _thisAssemblyPath);
            }
            else if(isConfigFileChanged)
            {
                Logger.Message("Config files [{0}] changed", string.Join(",", Config.ConfigFilesLoaded.Select(file => Path.GetFileName(file.AbsoluteFilePath))));
            }

            // Return true if a config file changed or the assembly changed
            return isConfigFileChanged || _isAssemblyNew;
        }

        /// <summary>
        /// Run CodeGenerator
        /// </summary>
        public void Run()
        {
            Logger.Progress(0, "Starting code generation...");

            try
            {
                // Run the parser
                var parser = new Parser.CppParser
                                 {
                                     IsGeneratingDoc = IsGeneratingDoc,
                                     DocProviderAssembly = DocProviderAssemblyPath,
                                     // @"..\..\..\DocProviderFromMsdn\bin\debug\DocProviderFromMsdn.exe",
                                     ForceParsing = _isAssemblyNew,
                                     CastXmlExecutablePath = CastXmlExecutablePath
                                 };

                // Init the parser
                parser.Init(Config);

                if (Logger.HasErrors)
                    Logger.Fatal("Initializing parser failed");

                // Run the parser
                var group = parser.Run();

                if (Logger.HasErrors)
                    Logger.Fatal("C++ compiler failed to parse header files");

                // Run the main mapping process
                var transformer = new TransformManager
                {
                    GeneratedPath = _generatedPath,
                    ForceGenerator = _isAssemblyNew,
                    AppType = AppType
                };

                transformer.Init(group, Config);

                if (Logger.HasErrors)
                    Logger.Fatal("Mapping rules initialization failed");

                transformer.Generate();

                if (Logger.HasErrors)
                    Logger.Fatal("Code generation failed");


                // Print statistics
                parser.PrintStatistics();
                transformer.PrintStatistics();

                // Output all elements
                var fileWriter = new StreamWriter("SharpGen_rename.log");
                transformer.NamingRules.DumpRenames(fileWriter);
                fileWriter.Close();

                // Update Checkfile for assembly
                File.WriteAllText(_assemblyCheckFile, "");
                File.SetLastWriteTime(_assemblyCheckFile, _assemblyDatetime);

                // Update Checkfile for all config files
                File.WriteAllText(_allConfigCheck, "");
                File.SetLastWriteTime(_allConfigCheck, DateTime.Now);
            }
            finally
            {
                Logger.Progress(100, "Finished");
            }
        }
    }
}