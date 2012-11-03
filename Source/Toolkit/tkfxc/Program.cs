// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
        [Option("Effect *.fx Files | PreCompiled Effect *.tkfxo files", Required = true)]
        public List<string> FxFiles = new List<string>();

        [Option("D", Description = "Define macro", Value = "<id>=<text>")]
        public readonly List<string> Defines = new List<string>();

        [Option("I", Description = "Additional include path\n", Value = "<include>")]
        public readonly List<string> IncludeDirs = new List<string>();

        [Option("Fo", Description = "Output object file. default is [output.tkfxo]\n", Value = "<file>")]
        public string OutputFile = "output.tkfxo";

        [Option("Fc", Description = "Output class file.", Value = "<file>")]
        public string OutputClassFile;

        [Option("ON", Description = "Name of the namespace to output in the .cs when using Fc option. Default: SharpDX.Toolkit.Graphics.", Value = "<namepsace>")]
        public string OutputNamespace = "SharpDX.Toolkit.Graphics";

        [Option("OC", Description = "Name of the classname to output in the .cs when using Fc option. Default: name of Fc file without the extension.", Value = "<classname>")]
        public string OutputClassname;

        [Option("OF", Description = "Name of the fieldname to output in the .cs when using Fc option. Default: effectBytecode.\n", Value = "<fieldname>")]
        public string OutputFieldName = "effectBytecode";

        [Option("Fv", Description = "Output disassemble of fx files and tkfxo files only. No output file generated")]
        public bool ViewOnly;

        [Option("Od", Description = "Output shader with debug information and no optimization")]
        public bool Debug;

        [Option("O", Description = "Optimization level 0..3.  1 is default", Value = "<0,1,2,3>")]
        public int OptimizationLevel = 1;

        [Option("Zpr", Description = "Pack matrices in row-major order")]
        public bool PackRowMajor;

        [Option("Zpc", Description = "Pack matrices in column-major order")]
        public bool PackColumnMajor;

        [Option("nodis", Description = "Suppress output of disassembly on the standard output.")]
        public bool NoDisassembly;

        static void Main(string[] args)
        {
            new Program().Run(args);
        }

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
            var macros = new List<ShaderMacro>();
            foreach (var define in options.Defines)
            {
                var nameValue = define.Split('=');
                string name = nameValue[0];
                string value = null;
                if (nameValue.Length > 1)
                {
                    value = nameValue[1];
                }
                macros.Add(new ShaderMacro(name, value));
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

            var archiveBytecode = new EffectData();
            bool hasErrors = false;

            // ----------------------------------------------------------------
            // Process each fx files / tkfxo files
            // ----------------------------------------------------------------
            foreach (var fxFile in options.FxFiles)
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, fxFile);

                if (!File.Exists(filePath))
                {
                    ErrorColor();
                    Console.Error.WriteLine("File [{0}] does not exist", fxFile);
                    ResetColor();
                    hasErrors = true;
                    continue;
                }

                EffectData effectData = null;

                // Try to load this file as a precompiled file
                effectData = EffectData.Load(fxFile);
                if (effectData != null)
                {
                    Console.WriteLine("Load Compiled File [{0}]", fxFile);                    
                }
                else
                {
                    // Compile the fx file
                    Console.WriteLine("Compile Effect File [{0}]", filePath);
                    var effectBytecode = EffectCompiler.Compile(File.ReadAllText(filePath), filePath, flags, macros, options.IncludeDirs);

                    // If there is any warning, errors, turn Error color on
                    if (effectBytecode.Logger.Messages.Count > 0)
                    {
                        ErrorColor();
                    }

                    // Show a message error for the current file
                    if (effectBytecode.HasErrors)
                    {
                        Console.Error.WriteLine("Error when compiling file [{0}]:", fxFile);
                        hasErrors = true;
                    }

                    // Print all messages (warning and errors)
                    foreach (var logMessage in effectBytecode.Logger.Messages)
                    {
                        Console.WriteLine(logMessage);
                    }

                    // If we have some messages, reset the color back
                    if (effectBytecode.Logger.Messages.Count > 0)
                    {
                        ResetColor();
                    }

                    effectData = effectBytecode.EffectData;
                }

                // If there is no errors, merge the result to the final archive
                if (!hasErrors)
                {
                    if (ProcessBytecode(effectData, archiveBytecode))
                        hasErrors = true;
                }
            }

            if (hasErrors)
            {
                ErrorColor();
                Console.Error.WriteLine("Compilation has errors. Process aborted.");
                ResetColor();
                Environment.Exit(-1);
            }
            else if (!ViewOnly)
            {
                Console.WriteLine();

                if (OutputClassFile != null)
                {
                    var codeWriter = new EffectDataCodeWriter
                                         {
                                             Namespace = OutputNamespace, 
                                             ClassName = OutputClassname ?? Path.GetFileNameWithoutExtension(OutputClassFile),
                                             FieldName = OutputFieldName,
                                         };

                    Console.WriteLine("Save C# code output to [{0}]", OutputClassFile);
                    using (var stream = new NativeFileStream(OutputClassFile, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write))
                        codeWriter.Write(archiveBytecode, new StreamWriter(stream, Encoding.UTF8));
                }
                else
                {
                    Console.WriteLine("Save output to [{0}]", options.OutputFile);
                    // Save the result
                    archiveBytecode.Save(options.OutputFile);
                }
            }
        }

        private bool ProcessBytecode(EffectData input, EffectData merged)
        {
            var hasErrors = false;
            if (!NoDisassembly)
                DumpBytecode(input);

            var logger = new Logger();
            merged.MergeFrom(input, logger);

            // If there is any errors from 
            if (logger.HasErrors)
            {
                hasErrors = true;
                ErrorColor();
                foreach (var message in logger.Messages)
                    Console.Error.WriteLine(message);
                ResetColor();
            }

            return hasErrors;
        }

        private void DumpBytecode(EffectData effectData)
        {
            Console.WriteLine();

            for (int i = 0; i < effectData.Shaders.Count; i++)
            {
                var shader = effectData.Shaders[i];

                Color(ConsoleColor.White);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Shader[{0}] {1}Type: {2} Level: {3} Visibility: {4}", i, shader.Name == null ? string.Empty : string.Format("{0} ", shader.Name), shader.Type, shader.Level, shader.Name != null ? "public" : "private");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine();
                ResetColor();

                DumpHtmlToConsole(EffectCompiler.DisassembleShader(shader));
                Console.WriteLine();
            }

            Color(ConsoleColor.White);
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Effects");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine();
            ResetColor();

            const string tab = "    ";

            foreach (var effect in effectData.Effects)
            {
                Console.Write("effect");
                Color(ConsoleColor.LightGreen);
                Console.WriteLine(" {0}", effect.Name);
                ResetColor();

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
                                              link.IsImport ? link.ImportName : string.Format("Shader[{0}] => {1}", link.Index, effectData.Shaders[link.Index].Name == null ? "private" : "public " + effectData.Shaders[link.Index].Name));
                            ResetColor();
                        }

                        if (pass.Attributes.Count > 0)
                        {
                            Console.WriteLine();

                            foreach (var attribute in pass.Attributes)
                            {
                                var typeName = attribute.Value != null ? attribute.Value.GetType().FullName.StartsWith("SharpDX") ? attribute.Value.GetType().FullName : null : null;
                                Console.WriteLine(passtab + tab + "{0} = {1}", attribute.Name, typeName == null ? attribute.Value : string.Format("{0}({1})", typeName, attribute.Value));
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
