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
using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    class CompilerOptions
    {
        [ConsoleEx.Required]
        [ConsoleEx.Name("Effect *.fx Files")]
        public List<string> FxFiles = new List<string>();

        [ConsoleEx.Name("D", Description = "Define macro", Value = "<id>=<text>")]
        public readonly List<string> Defines = new List<string>();

        [ConsoleEx.Name("I", Description = "Additional include path\n", Value = "<include>")]
        public readonly List<string> IncludeDirs = new List<string>();

        [ConsoleEx.Name("Fo", Description = "Output object file. default is [output.tkfxo]\n", Value = "<file>")] 
        public string OutputFile = "output.tkfxo";

        [ConsoleEx.Name("Od", Description = "Output shader with debug information and no optimization")]
        public bool Debug;

        [ConsoleEx.Name("O", Description = "Optimization level 0..3.  1 is default", Value = "<0,1,2,3>")] 
        public int OptimizationLevel = 1;

        [ConsoleEx.Name("Zpr", Description = "Pack matrices in row-major order")]
        public bool PackRowMajor;

        [ConsoleEx.Name("Zpc", Description = "Pack matrices in column-major order")]
        public bool PackColumnMajor;
    }
}