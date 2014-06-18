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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using SharpDX.Direct3D;
using SharpDX.IO;
using SharpDX.Text;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This is the command line front-end for <see cref="SharpDX.Toolkit.Graphics.EffectCompiler"/>.
    /// </summary>
    class Program : ConsoleProgram
    {
        [Option("Effect File", Required = true)]
        public string FxFile = null;

        [Option("D", Description = "Define macro", Value = "<id>=<text>")]
        public readonly List<string> Defines = new List<string>();

        [Option("I", Description = "Additional include path\n", Value = "<include>")]
        public readonly List<string> IncludeDirs = new List<string>();

        [Option("Fo", Description = "Output object file. default is [InputFileName.tkb]\n", Value = "<file>")]
        public string OutputFile = null;

        [Option("Fc", Description = "Output class file.", Value = "<file>")]
        public string OutputClassFile;

        [Option("ON", Description = "Name of the namespace to output in the .cs when using Fc option. Default: SharpDX.Toolkit.Graphics.", Value = "<namepsace>")]
        public string OutputNamespace = "SharpDX.Toolkit.Graphics";

        [Option("OC", Description = "Name of the classname to output in the .cs when using Fc option. Default: name of Fc file without the extension.", Value = "<classname>")]
        public string OutputClassname;

        [Option("OF", Description = "Name of the fieldname to output in the .cs when using Fc option. Default: effectBytecode.\n", Value = "<fieldname>")]
        public string OutputFieldName = "effectBytecode";

        [Option("Ti", Description = "Compile the file only if the source file is newer (working with indirect include).\n")]
        public bool CompileOnlyIfNewer;

        [Option("Re", Description = "Allow effect dynamic compiling at runtime.\n")]
        public bool AllowDynamicCompiling;

        [Option("To", Description = "Output directory for the dependency file (default '.' in the same directory than file to compile\n")]
        public string OutputDependencyDirectory = ".";

        [Option("Od", Description = "Output shader with debug information and no optimization")]
        public bool Debug;

        [Option("O", Description = "Optimization level 0..3.  1 is default\n", Value = "<0,1,2,3>")]
        public int OptimizationLevel = 1;

        [Option("Zpr", Description = "Pack matrices in row-major order")]
        public bool PackRowMajor;

        [Option("Zpc", Description = "Pack matrices in column-major order")]
        public bool PackColumnMajor;

        [Option("Gpp", Description = "Force partial precision")]
        public bool ForcePartialPrecision;

        [Option("Gfa", Description = "Avoid flow control constructs")]
        public bool AvoidFlowControl;

        [Option("Gfp", Description = "Prefer flow control constructs")]
        public bool PreferFlowControl;

        [Option("Ges", Description = "Enable strict mode")]
        public bool EnableStrictness;

        [Option("Gec", Description = "Enable backwards compatibility mode")]
        public bool EnableBackwardsCompatibility;

        [Option("Gis", Description = "Force IEEE strictness\n")]
        public bool IeeeStrictness;

        [Option("nodis", Description = "Suppress output of disassembly on the standard output.")]
        public bool NoDisassembly;

        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        private bool hasErrors;

        void Run(string[] args)
        {
            var assemblyUri = new Uri(Assembly.GetEntryAssembly().CodeBase);
            var assemblyPath = Path.GetDirectoryName(assemblyUri.LocalPath);

            var newPath = Path.GetFullPath(Path.Combine(assemblyPath, @"..\Redist\D3D\" + (IntPtr.Size == 4 ? "x86" : "x64"))) + ";" + Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", newPath);

            // Print the exe header
            PrintHeader();

            // Parse the command line
            if (!ParseCommandLine(args))
                Environment.Exit(-1);

            var options = this;

            // ----------------------------------------------------------------
            // Process macros
            // ----------------------------------------------------------------
            var macros = new List<EffectData.ShaderMacro>();
            foreach (var define in options.Defines)
            {
                var nameValue = define.Split('=');
                string name = nameValue[0];
                string value = null;
                if (nameValue.Length > 1)
                {
                    value = nameValue[1];
                }
                macros.Add(new EffectData.ShaderMacro(name, value));
            }

            // ----------------------------------------------------------------
            // Setup compiler flags
            // ----------------------------------------------------------------
            var flags = EffectCompilerFlags.None;
            if (options.Debug)
                flags |= EffectCompilerFlags.Debug;

            switch (options.OptimizationLevel)
            {
                case 0:
                    flags |= EffectCompilerFlags.OptimizationLevel0;
                    break;
                case 1:
                    flags |= EffectCompilerFlags.OptimizationLevel1;
                    break;
                case 2:
                    flags |= EffectCompilerFlags.OptimizationLevel2;
                    break;
                case 3:
                    flags |= EffectCompilerFlags.OptimizationLevel3;
                    break;
            }

            if (options.PackRowMajor)
                flags |= EffectCompilerFlags.PackMatrixRowMajor;

            if (options.PackColumnMajor)
                flags |= EffectCompilerFlags.PackMatrixColumnMajor;

            if (options.AvoidFlowControl)
                flags |= EffectCompilerFlags.AvoidFlowControl;

            if (options.PreferFlowControl)
                flags |= EffectCompilerFlags.PreferFlowControl;

            if (options.EnableStrictness)
                flags |= EffectCompilerFlags.EnableStrictness;

            if (options.EnableBackwardsCompatibility)
                flags |= EffectCompilerFlags.EnableBackwardsCompatibility;

            if (options.IeeeStrictness)
                flags |= EffectCompilerFlags.IeeeStrictness;

            hasErrors = false;

            // ----------------------------------------------------------------
            // Process each fx files / tkfxo files
            // ----------------------------------------------------------------
            var fxFile = options.FxFile;
            var filePath = Path.Combine(Environment.CurrentDirectory, fxFile);

            // Check that input file exists
            if (!File.Exists(filePath))
            {
                ErrorColor();
                Console.Error.WriteLine("File [{0}] does not exist", fxFile);
                ResetColor();
                Abort();
            }

            // ----------------------------------------------------------------
            // Pre check files
            // ----------------------------------------------------------------
            if (options.OutputClassFile == null && options.OutputFile == null)
            {
                options.OutputFile = Path.GetFileNameWithoutExtension(options.FxFile) + ".tkb";
            }

            // Check for output files
            bool outputFileExist = options.OutputClassFile != null && File.Exists(options.OutputClassFile);
            if (options.OutputFile != null && !File.Exists(options.OutputFile))
            {
                outputFileExist = false;
            }

            // New Compiler
            var compiler = new EffectCompiler();

            string outputDependencyDirPath = Path.Combine(Environment.CurrentDirectory, OutputDependencyDirectory);
            string outputDependencyFilePath = Path.Combine(outputDependencyDirPath,  compiler.GetDependencyFileNameFromSourcePath(options.FxFile));

            if (AllowDynamicCompiling)
            {
                CompileOnlyIfNewer = true;
            }

            if (CompileOnlyIfNewer)
            {
                if (!compiler.CheckForChanges(outputDependencyFilePath) && outputFileExist)
                {
                    Console.Error.WriteLine("Nothing to compile. Output file [{0}] is up-to-date", options.OutputFile);
                    Environment.Exit(0);
                }
            }

            var viewOnly = false;
            // Try to load this file as a precompiled file
            var effectData = EffectData.Load(fxFile);
            EffectCompilerResult compilerResult = null;

            if (effectData != null)
            {
                Console.WriteLine("Load Compiled File [{0}]", fxFile);
                viewOnly = true;
            }
            else
            {
                // Compile the fx file
                Console.WriteLine("Compile Effect File [{0}]", filePath);
                compilerResult = compiler.Compile(File.ReadAllText(filePath), filePath, flags, macros, options.IncludeDirs, AllowDynamicCompiling, CompileOnlyIfNewer ? outputDependencyFilePath : null);

                // If there is any warning, errors, turn Error color on
                if (compilerResult.Logger.Messages.Count > 0)
                {
                    ErrorColor();
                }

                // Show a message error for the current file
                if (compilerResult.HasErrors)
                {
                    Console.Error.WriteLine("Error when compiling file [{0}]:", fxFile);
                    hasErrors = true;
                }

                // Print all messages (warning and errors)
                foreach (var logMessage in compilerResult.Logger.Messages)
                {
                    Console.WriteLine(logMessage);
                }

                // If we have some messages, reset the color back
                if (compilerResult.Logger.Messages.Count > 0)
                {
                    ResetColor();
                }

                effectData = compilerResult.EffectData;
            }

            if (!NoDisassembly && effectData != null)
            {
                DumpBytecode(compiler, effectData);
            }

            if (hasErrors)
            {
                Abort();
            }

            if (!viewOnly)
            {
                Console.WriteLine();

                if (CompileOnlyIfNewer && compilerResult.DependencyFilePath != null)
                {
                    // Dependency file save to 
                    Console.WriteLine("Save dependency list to [{0}]", outputDependencyFilePath);
                }

                if (OutputClassFile != null)
                {
                    var codeWriter = new EffectDataCodeWriter
                                         {
                                             Namespace = OutputNamespace,
                                             ClassName = OutputClassname ?? Path.GetFileNameWithoutExtension(OutputClassFile),
                                             FieldName = OutputFieldName,
                                         };

                    Console.WriteLine("Save C# code output to [{0}]", OutputClassFile);
                    using (var stream = new NativeFileStream(OutputClassFile, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write)) codeWriter.Write(effectData, new StreamWriter(stream, Encoding.UTF8));
                }

                if (options.OutputFile != null)
                {
                    Console.WriteLine("Save binary output to [{0}]", options.OutputFile);
                    // Save the result
                    effectData.Save(options.OutputFile);
                }
            }
        }

        private void Abort()
        {
            ErrorColor();
            Console.Error.WriteLine("Compilation has errors. Process aborted.");
            ResetColor();
            Environment.Exit(-1);
        }

        private void DumpBytecode(EffectCompiler compiler, EffectData effectData)
        {
            Console.WriteLine();

            for (int i = 0; i < effectData.Shaders.Count; i++)
            {
                var shader = effectData.Shaders[i];

                Color(ConsoleColor.White);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Shader[{0}] {1}Type: {2} Level: {3} Visibility: {4} ByteCode Size: {5}", i, shader.Name == null ? string.Empty : string.Format("{0} ", shader.Name), shader.Type, shader.Level, shader.Name != null ? "public" : "private", shader.Bytecode.Length);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine();
                ResetColor();

                DumpHtmlToConsole(compiler.DisassembleShader(shader));
                Console.WriteLine();
            }

            Color(ConsoleColor.White);
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("effect {0}", effectData.Description.Name);
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine();
            ResetColor();

            const string tab = "    ";

            var effect = effectData.Description;
            {
                Console.WriteLine("{");
                foreach (var technique in effect.Techniques)
                {
                    Console.Write(tab + "technique");
                    Color(ConsoleColor.LightGreen);
                    Console.WriteLine(" {0}", technique.Name);
                    ResetColor();

                    Console.WriteLine(tab + "{");
                    for (int passIndex = 0; passIndex < technique.Passes.Count; passIndex++)
                    {
                        var pass = technique.Passes[passIndex];
                        var passtab = pass.IsSubPass ? tab + tab + tab : tab + tab;

                        Console.Write("{0}{1} #{2}", passtab, ((pass.IsSubPass) ? "subpass" : "pass"), passIndex);
                        Color(ConsoleColor.LightGreen);
                        Console.WriteLine(" {0}", pass.Name);
                        ResetColor();

                        Console.WriteLine(passtab + "{");

                        for (int i = 0; i < pass.Pipeline.Links.Length; i++)
                        {
                            var shaderType = (EffectShaderType) i;
                            var link = pass.Pipeline.Links[i];
                            if (link == null)
                                continue;

                            Color(ConsoleColor.LightYellow);
                            Console.Write(passtab + tab + "{0}", shaderType);
                            ResetColor();
                            Console.Write(" = ");
                            Color(ConsoleColor.White);
                            Console.WriteLine("{0}",
                                link.IsImport ? link.ImportName : link.IsNullShader ? "null" : string.Format("Shader[{0}] => {1}", link.Index, effectData.Shaders[link.Index].Name == null ? "private" : "public " + effectData.Shaders[link.Index].Name));
                            ResetColor();
                        }

                        if (pass.Properties.Count > 0)
                        {
                            Console.WriteLine();

                            foreach (var attribute in pass.Properties)
                            {
                                var typeName = attribute.Value != null ? attribute.Value.GetType().FullName.StartsWith("SharpDX") ? attribute.Value.GetType().FullName : null : null;
                                Console.WriteLine(passtab + tab + "{0} = {1}", attribute.Key, typeName == null ? attribute.Value : string.Format("{0}({1})", typeName, attribute.Value));
                            }
                        }
                        Console.WriteLine(passtab + "}");
                    }
                    Console.WriteLine(tab + "}");
                }
                Console.WriteLine("}");
            }
        }

        private void DumpHtmlToConsole(string html)
        {
            // Remove body tag that is unclosed?
            html = Regex.Replace(html, @"\<body.*?>", string.Empty);

            // Fix <\d+ and \d+> tags
            html = Regex.Replace(html, @"\<(\d+)", "&lt;$1");
            html = Regex.Replace(html, @"(\d+)\>", "$1&gt;");
           
            using (var reader = XmlReader.Create(new StringReader(html)))
            {
                while (true)
                {
                    try
                    {
                        if (!reader.Read())
                        {
                            break;
                        }
                    } catch (Exception ex)
                    {
                        ErrorColor();
                        Console.Out.WriteLine();
                        Console.Error.WriteLine("Warning, cannot print dissassemble: {0}", ex.Message);
                        ResetColor();
                        break;
                    }

                    if (reader.NodeType == XmlNodeType.Comment)
                        continue;

                    //  Here we check the type of the node, in this case we are looking for element
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "font")
                            {
                                var color = reader.GetAttribute("color");
                                switch (color)
                                {
                                    case "#ffff40":
                                        Color(ConsoleColor.LightYellow);
                                        break;
                                    case "#e0e0e0":
                                        ResetColor();
                                        break;
                                    case "#a0a0a0":
                                        Color(ConsoleColor.LightGrey);
                                        break;
                                    case "#00ffff":
                                        Color(ConsoleColor.LightCyan);
                                        break;
                                }
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name == "font")
                            {
                                ResetColor();
                            }
                            break;
                    }
                    if (!string.IsNullOrEmpty(reader.Value))
                        Console.Write(reader.Value);
                }
            }
            ResetColor();
        }
    }
}
