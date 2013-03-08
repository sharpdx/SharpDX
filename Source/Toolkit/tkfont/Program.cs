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
// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//--------------------------------------------------------------------
using System;
using System.IO;
using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
    internal class Program : ConsoleProgram
    {
        [Option("XML Font File Description", Required = true)]
        public string XmlFontFile = null;

        [Option("O", Description = "Output File, default is <XML Font File> without extension", Value = "<filename>")]
        public string OutputFile = null;

        [Option("Ti", Description = "Compile the file only if the source file is newer.\n")]
        public bool CompileOnlyIfNewer;

        [Option("To", Description = "Output directory for the dependency file (default '.' in the same directory than file to compile\n")]
        public string OutputDependencyDirectory = ".";

        [Option("S", Description = "Output Bitmap filename for debug in DDS format. Default is null.", Value = "<filename>")]
        public string DebugOutputSpriteSheet = null;

        public static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public void Run(string[] args)
        {
            // Print the exe header
            PrintHeader();

            // Parse the command line
            if (!ParseCommandLine(args))
            {
                Environment.Exit(-1);
            }

            // Loads the description
            var filePath = Path.Combine(Environment.CurrentDirectory, XmlFontFile);

            var fontDescription = FontDescription.Load(XmlFontFile);

            var defaultOutputFile = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));

            // Compiles to SpriteData
            OutputFile = OutputFile ?? defaultOutputFile;

            string dependencyFile = null;
            if (CompileOnlyIfNewer)
            {
                dependencyFile = Path.Combine(OutputDependencyDirectory, FileDependencyList.GetDependencyFileNameFromSourcePath(Path.GetFileName(filePath)));
            }

            bool forceCompilation = (DebugOutputSpriteSheet != null && !File.Exists(DebugOutputSpriteSheet));

            var result = FontCompiler.CompileAndSave(filePath, OutputFile, dependencyFile);
            if (result.IsContentGenerated || forceCompilation)
            {
                Console.WriteLine("Writing [{0}] ({1} format)", OutputFile, fontDescription.Format);

                // Save output files.
                if (!string.IsNullOrEmpty(DebugOutputSpriteSheet))
                {
                    Console.WriteLine("Saving debug output spritesheet {0}", DebugOutputSpriteSheet);

                    var spriteFontData = SpriteFontData.Load(OutputFile);

                    if (spriteFontData.Bitmaps.Length > 0 && spriteFontData.Bitmaps[0].Data is SpriteFontData.BitmapData)
                    {
                        var bitmapData = (SpriteFontData.BitmapData)spriteFontData.Bitmaps[0].Data;
                        using (var image = Image.New2D(bitmapData.Width, bitmapData.Height, 1, bitmapData.PixelFormat))
                        {
                            Utilities.Write(image.DataPointer, bitmapData.Data, 0, bitmapData.Data.Length);
                            image.Save(DebugOutputSpriteSheet, ImageFileType.Dds);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No need to write [{0}]. File is up-to-date from XML description", OutputFile);
            }
        }
    }
}
