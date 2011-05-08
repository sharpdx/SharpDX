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
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using SharpCore.Logging;

namespace SharpGen
{
    /// <summary>
    /// Main program for CodeGen
    /// </summary>
    internal static class Program
    {
        private static CodeGenApp _codeGenApp;
        private static ProgressForm _progressForm;

        private class CommandArgs
        {
            public string GccXmlExecutablePath;
            public string DocProviderAssemblyPath;
            public string ConfigRootPath;
        }

        /// <summary>
        /// Print usage and exit
        /// </summary>
        /// <param name="exitCode">exit code to return</param>
        /// <param name="message">error message to print</param>
        /// <param name="messageArgs">error message arguments</param>
        private static void Usage(int exitCode, string message = null, params string[] messageArgs)
        {
            Logger.Error("Usage: {0} [/d:path_to_docprovider default = null] /gccxml:path_to_gcc_xml_exe path_to_root_config_file.xml", Path.GetFileName(Assembly.GetEntryAssembly().Location));
            if (message != null)
                Logger.Error(message, messageArgs);

            Logger.Fatal("Invalid command args");
        }

        /// <summary>
        /// Parse arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static CommandArgs ParseArgs(string[] args)
        {
            var cmdArgs = new CommandArgs();

            foreach (var arg in args)
            {
                if (arg.StartsWith("/d:"))
                    cmdArgs.DocProviderAssemblyPath = arg.Substring("/d:".Length);
                else if (arg.StartsWith("/gccxml:"))
                    cmdArgs.GccXmlExecutablePath = arg.Substring("/gccxml:".Length);
                else if (cmdArgs.ConfigRootPath != null)
                {
                    Usage(1, "Unexpected argument: {0}", arg);
                }
                else
                {
                    cmdArgs.ConfigRootPath = arg;
                }
            }

            if (cmdArgs.GccXmlExecutablePath == null)
                Usage(1, "expecting arg: /gccxml option");

            if (cmdArgs.ConfigRootPath == null)
                Usage(1, "expecting arg: path_to_config_root_file.xml");

            return cmdArgs;
        }

        /// <summary>
        /// Runs code generation asynchronously.
        /// </summary>
        public static void RunAsync()
        {
            try
            {
                Logger.Progress(0, "Starting code generation...");

                _codeGenApp.Run();
            }
            catch (Exception ex)
            {
                Logger.Fatal("Unexpected exception", ex);
            }
            finally
            {
                MethodInvoker methodInvoker = delegate() { _progressForm.Shutdown(); };
                _progressForm.Invoke(methodInvoker);                
            }
        }

        /// <summary>
        /// Main SharpGen
        /// </summary>
        /// <param name="args">Command line args.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            _progressForm = null;
            try
            {
                // Optimize XmlSerialization by generating XmlSerializers assembly
                Utilities.SGenThisAssembly();

                var commandArgs = ParseArgs(args);

                _codeGenApp = new CodeGenApp
                {
                    DocProviderAssemblyPath = commandArgs.DocProviderAssemblyPath,
                    GccXmlExecutablePath = commandArgs.GccXmlExecutablePath
                };

                if (_codeGenApp.Init(commandArgs.ConfigRootPath))
                {
                    Application.EnableVisualStyles();
                    _progressForm = new ProgressForm();
                    Logger.ProgressReport = _progressForm;
                    _progressForm.Show();

                    var runningThread = new Thread(RunAsync) { IsBackground = true };
                    runningThread.Start();

                    Application.Run(_progressForm);
                }
                else
                {
                    Logger.Message("Latest code generation is up to date. No need to run code generation");
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal("Unexpected exception", ex);
            }
            Environment.Exit(0);
        }
    }
}