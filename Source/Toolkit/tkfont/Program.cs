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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using SharpDX;

namespace SharpDX.Toolkit.Graphics
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (!ConsoleProgram.ParseCommandLine(options, args, 32))
            {
                Environment.Exit(-1);
            }
            MakeSpriteFont(options);
        }

        static void MakeSpriteFont(CommandLineOptions options)
        {
            // Import.
            Console.WriteLine("Importing {0}", options.SourceFont);

            float lineSpacing;

            Glyph[] glyphs = ImportFont(options, out lineSpacing);

            // Optimize.
            Console.WriteLine("Cropping glyph borders");

            foreach (Glyph glyph in glyphs)
            {
                GlyphCropper.Crop(glyph);
            }

            Console.WriteLine("Packing glyphs into sprite sheet");

            Bitmap bitmap = GlyphPacker.ArrangeGlyphs(glyphs);

            // Adjust line and character spacing.
            lineSpacing += options.LineSpacing;

            foreach (Glyph glyph in glyphs)
            {
                glyph.XAdvance += options.CharacterSpacing;
            }

            // Automatically detect whether this is a monochromatic or color font?
            if (options.TextureFormat == TextureFormat.Auto)
            {
                bool isMono = BitmapUtils.IsRgbEntirely(Color.White, bitmap);

                options.TextureFormat = isMono ? TextureFormat.CompressedMono :
                                                 TextureFormat.Rgba32;
            }

            // Convert to premultiplied alpha format.
            if (!options.NoPremultiply)
            {
                Console.WriteLine("Premultiplying alpha");

                BitmapUtils.PremultiplyAlpha(bitmap);
            }

            // Save output files.
            if (!string.IsNullOrEmpty(options.DebugOutputSpriteSheet))
            {
                Console.WriteLine("Saving debug output spritesheet {0}", options.DebugOutputSpriteSheet);

                bitmap.Save(options.DebugOutputSpriteSheet);
            }

            Console.WriteLine("Writing {0} ({1} format)", options.OutputFile, options.TextureFormat);

            SpriteFontWriter.WriteSpriteFont(options, glyphs, lineSpacing, bitmap);
        }


        static Glyph[] ImportFont(CommandLineOptions options, out float lineSpacing)
        {
            // Which importer knows how to read this source font?
            IFontImporter importer;

            string fileExtension = Path.GetExtension(options.SourceFont).ToLowerInvariant();

            string[] BitmapFileExtensions = { ".bmp", ".png", ".gif" };

            if (BitmapFileExtensions.Contains(fileExtension))
            {
                importer = new BitmapImporter();
            }
            else
            {
                importer = new TrueTypeImporter();
            }

            // Import the source font data.
            importer.Import(options);

            lineSpacing = importer.LineSpacing;

            var glyphs = importer.Glyphs
                                 .OrderBy(glyph => glyph.Character)
                                 .ToArray();

            // Validate.
            if (glyphs.Length == 0)
            {
                throw new Exception("Font does not contain any glyphs.");
            }

            if ((options.DefaultCharacter != 0) && !glyphs.Any(glyph => glyph.Character == options.DefaultCharacter))
            {
                throw new Exception("The specified DefaultCharacter is not part of this font.");
            }

            return glyphs;
        }
    }
}
