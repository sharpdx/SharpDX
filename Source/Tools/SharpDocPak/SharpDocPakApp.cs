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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Mono.Options;

namespace SharpDocPak
{
    /// <summary>
    /// SharpDocPak application.
    /// </summary>
    public class SharpDocPakApp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDocPakApp"/> class.
        /// </summary>
        public SharpDocPakApp()
        {
            Tags = new List<TagIndex>();
            CommandType = CommandType.Show;
        }

        /// <summary>
        /// Gets or sets the type of the command to run.
        /// </summary>
        /// <value>The type of the command.</value>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the output of the docpak exe.
        /// </summary>
        /// <value>The output.</value>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the directory location used for Pack or Unpack commands.
        /// </summary>
        /// <value>The directory location.</value>
        public string DirectoryLocation { get; set; }

        /// <summary>
        /// Gets or sets the tags to index.
        /// </summary>
        /// <value>The tags.</value>
        public List<TagIndex> Tags{ get; set; }

        /// <summary>
        /// Gets or sets the title of the browser form.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Default title of the browser form.
        /// </summary>
        public const string DefaultTitle = "SharpDoc Viewer";

        /// <summary>
        /// Print usages the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="parameters">The parameters.</param>
        private static void UsageError(string error, params string[] parameters)
        {
            ConsoleHelper.UseConsole();
            var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            Console.Write("{0}: ", exeName);
            Console.WriteLine(error, parameters);
            Console.WriteLine("Use {0} --help' for more information.", exeName);
            Environment.Exit(1);
        }

        /// <summary>
        /// Parses and validates the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void ParseArguments(string[] args)
        {
            var showHelp = false;

            var options = new OptionSet()
                              {
                                  "Copyright (c) 2010-2013 SharpDX - Alexandre Mutel",
                                  "Usage: SharpDocPak [-p|-u|-l] [options]*",
                                  "Html Documentation exe packer/viewer",
                                  "",
                                  "commands:", 
                                  { 
                                      "p|pack", "Pack a documentation from a directory. The value contains the name of the resulting exe",                                                   
                                      opt => CommandType = opt != null ? CommandType.Pak : CommandType.Show
                                      },
                                  {
                                      "u|unpack", "Unpack the documentation from the current executable to the specified directory",
                                      opt => CommandType = opt != null ? CommandType.Unpak : CommandType.Show
                                      },
                                  {
                                      "l|list", "List all packed files from this docpak executable",
                                      opt => CommandType = opt != null ? CommandType.List : CommandType.Show
                                      },
                                  "",
                                  "options:",
                                  {"d|dir=", "Output or Input directory, mandatory for pack/unpack options", opt => DirectoryLocation = opt},
                                  {"o|output=", "Filepath of the generated exe, mandatory for --pack option", opt => Output = opt},
                                  {"t|title=", "Documentation Window Title [default: " + DefaultTitle + "]", opt => Title = opt ?? DefaultTitle},
                                  {
                                      "D=", "Define a tag to index. The tagid must be unique." +
                                            "The value is composed on an xpath and an optional name separated by a semicolon ';'." +
                                            "Example: -D content=//html/body;Content",
                                      (param, value) =>
                                          {
                                              if (param == null)
                                                  throw new OptionException("Missing parameter tag id for option -D.", "-D");

                                              if (value == null)
                                                  throw new OptionException("Missing parameter xpath and name for option -D.", "-D");

                                              TagIndex.ParseAndAddTagIndex(Tags, param, value);
                                          }
                                      },
                                  "",
                                  {"h|help", "Show this message and exit", opt => showHelp = opt != null},
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
                ConsoleHelper.UseConsole();
                options.WriteOptionDescriptions(Console.Out);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            switch (CommandType)
            {
                case CommandType.Pak:
                    if (string.IsNullOrEmpty(Output))
                        UsageError("option --output is mandatory with --pack option");

                    if (string.IsNullOrEmpty(DirectoryLocation))
                        UsageError("option --dir is mandatory with --pack option");

                    if (!Directory.Exists(DirectoryLocation))
                        UsageError("Directory [{0}] doesn't exist", DirectoryLocation);

                    // Run the packer
                    Pack();
                    break;
                case CommandType.Unpak:
                    if (string.IsNullOrEmpty(DirectoryLocation))
                        UsageError("option --dir is mandatory with --unpack option");

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(DirectoryLocation))
                        Directory.CreateDirectory(DirectoryLocation);

                    // Unpack this exe
                    Unpack();
                    break;

                case CommandType.List:
                    List();
                    break;

                default:
                    Show();
                    break;
            }
        }

        /// <summary>
        /// Packs a set of files into a docpak exe.
        /// </summary>
        private void Pack()
        {
            ConsoleHelper.UseConsole();
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, DirectoryLocation);

            // Check that input directory contains at least an index.htm / index.html file
            if (!File.Exists(Path.Combine(rootDirectory, Archive.DefaultHtmlRoot)) &&
                File.Exists(Path.Combine(rootDirectory, Archive.DefaultHtmlRootAlternate)))
            {
                UsageError("Directory [{0}] must contain at least an {1} or {2} file", rootDirectory, Archive.DefaultHtmlRoot, Archive.DefaultHtmlRootAlternate);
            }

            // Initialize the indexer
            var indexer = new DocumentIndexer {Tags = Tags};
            indexer.Init();

            // Initialize the archive
            var archive = new Archive { Title = Title, Tags = indexer.Tags, Index = indexer.Index };

            // Iterate on all files
            var files = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileName = file.Substring(rootDirectory.Length).Replace('\\', '/');
                byte[] buffer = File.ReadAllBytes(file);
                archive.Files.Add(fileName, buffer);
                indexer.AddFile(fileName, buffer);
            }

            // Close the indexer
            indexer.Close();

            // Create a docpak exe
            Output = Path.ChangeExtension(Output, ".exe");
            var input = new FileStream(typeof(SharpDocPakApp).Assembly.Location, FileMode.Open, FileAccess.Read);
            var output = new FileStream(Output, FileMode.Create, FileAccess.Write);

            // Appends the archive
            archive.Append(input, output);

            input.Close();
            output.Close();

            Console.WriteLine("Docpak exe successfully generated to [{0}]", Output);
        }

        /// <summary>
        /// Loads the current archive from this executable. If no archive is available, display a message box error.
        /// </summary>
        /// <returns>Archive if successfully loaded</returns>
        private Archive LoadCurrentArchive()
        {
            var archive = Archive.GetFromCurrentExecutable();
            if (archive == null)
            {
                MessageBox.Show("This executable is not packed with any documentation. Use --help for more information", "SharpDocPak", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(1);
            }
            return archive;
        }

        /// <summary>
        /// Unpacks the archive stored in this docpak exe.
        /// </summary>
        private void Unpack()
        {
            ConsoleHelper.UseConsole();
            var archive = LoadCurrentArchive();
            foreach(var fileKeyValue in  archive.Files)
            {
                var filePath = Path.Combine(DirectoryLocation, fileKeyValue.Key);
                Console.WriteLine("Unpack file [{0}]", filePath);
                File.WriteAllBytes(filePath, fileKeyValue.Value);
            }
            Console.WriteLine("Unpack successful.");
        }

        /// <summary>
        /// Lists the archive stored in this docpak exe.
        /// </summary>
        private void List()
        {
            ConsoleHelper.UseConsole();
            var archive = LoadCurrentArchive();
            foreach (var file in archive.Files.Keys)
                Console.WriteLine("File [{0}]", file);
            Console.WriteLine("SharpDocPak list successfully returned.");
        }

        /// <summary>
        /// Shows a browser with documentation stored in the archive of this docpak exe.
        /// </summary>
        private void Show()
        {
            var app = new DocumentServer {Content = LoadCurrentArchive()};
            app.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var webForm = new WebBrowserForm { Text = app.Content.Title };
            webForm.Browser.Navigate(app.Url + app.Content.DefaultHtmlFile);
            Application.Run(webForm);            
        }
    }
}