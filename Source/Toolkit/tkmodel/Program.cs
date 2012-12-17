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
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit.Compiler
{
    /// <summary>
    /// This is the command line front-end for <see cref="SharpDX.Toolkit.Graphics.EffectCompiler"/>.
    /// </summary>
    class Program : ConsoleProgram
    {
        [Option("Model Files", Required = true)]
        public List<string> ModelFiles = new List<string>();

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


            bool hasErrors = false;

            // ----------------------------------------------------------------
            // Process each fx files / tkfxo files
            // ----------------------------------------------------------------
            foreach (var fxFile in options.ModelFiles)
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
                Console.WriteLine("Compile Model from File [{0}]", filePath);
                var modelData = ModelCompiler.CompileFromFile(filePath);

                // TODO IMPLEMENT SAVING
            }

            if (hasErrors)
            {
                ErrorColor();
                Console.Error.WriteLine("Compilation has errors. Process aborted.");
                ResetColor();
                Environment.Exit(-1);
            }
        }
    }
}
