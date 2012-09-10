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
using SharpDX.Direct3D;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This is the command line front-end for <see cref="SharpDX.Toolkit.Graphics.EffectCompiler"/>.
    /// </summary>
    class Program : ConsoleProgram
    {
        [Option("Effect *.fx Files", Required = true)]
        public List<string> FxFiles = new List<string>();

        [Option("D", Description = "Define macro", Value = "<id>=<text>")]
        public readonly List<string> Defines = new List<string>();

        [Option("I", Description = "Additional include path\n", Value = "<include>")]
        public readonly List<string> IncludeDirs = new List<string>();

        [Option("Fo", Description = "Output object file. default is [output.tkfxo]\n", Value = "<file>")]
        public string OutputFile = "output.tkfxo";

        [Option("Od", Description = "Output shader with debug information and no optimization")]
        public bool Debug;

        [Option("O", Description = "Optimization level 0..3.  1 is default", Value = "<0,1,2,3>")]
        public int OptimizationLevel = 1;

        [Option("Zpr", Description = "Pack matrices in row-major order")]
        public bool PackRowMajor;

        [Option("Zpc", Description = "Pack matrices in column-major order")]
        public bool PackColumnMajor;

        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        void Run(string[] args)
        {
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

            // ----------------------------------------------------------------
            // Process each fx files
            // ----------------------------------------------------------------
            var archiveBytecode = new EffectBytecode();
            bool hasErrors = false;

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

                // If there is no errors, merge the result to the final archive
                if (!hasErrors)
                {
                    var logger = new Logger();
                    archiveBytecode.MergeFrom(effectBytecode.Bytecode, logger);

                    // If there is any errors from 
                    if (logger.HasErrors)
                    {
                        hasErrors = true;
                        ErrorColor();
                        foreach (var message in logger.Messages)
                            Console.Error.WriteLine(message);
                        ResetColor();
                    }
                }
            }

            if (hasErrors)
            {
                ErrorColor();
                Console.Error.WriteLine("Compilation has errors. Process aborted.");
                ResetColor();
                Environment.Exit(-1);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Save output to [{0}]", options.OutputFile);
                // Save the result
                archiveBytecode.Save(options.OutputFile);
            }
        }
    }
}
