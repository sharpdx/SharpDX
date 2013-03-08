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

using SharpDX.IO;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    using System.Drawing;

    /// <summary>
    /// Main class used to compile a Font file XML file.
    /// </summary>
    public class FontCompiler
    {
        /// <summary>
        /// Compiles an XML font file description to a file. Optionally output dependency file.
        /// </summary>
        /// <param name="sourceXmlFile">The source XML file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="dependencyFile">The dependency file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static ContentCompilerResult CompileAndSave(string sourceXmlFile, string outputFile, string dependencyFile = null)
        {
            var logger = new Logger();
            var result = new ContentCompilerResult { Logger = logger };
            try
            {
                var fontDescription = FontDescription.Load(sourceXmlFile);

                var defaultOutputFile = Path.GetFileNameWithoutExtension(sourceXmlFile);

                // Compiles to SpriteData
                outputFile = outputFile ?? defaultOutputFile;

                result.IsContentGenerated = true;
                if (dependencyFile != null)
                {
                    if (!FileDependencyList.CheckForChanges(dependencyFile))
                    {
                        result.IsContentGenerated = false;
                    }
                }

                if (result.IsContentGenerated)
                {
                    // Make sure that directory name doesn't collide with filename
                    var directoryName = Path.GetDirectoryName(outputFile + ".tmp");
                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    using (var stream = new NativeFileStream(outputFile, NativeFileMode.Create, NativeFileAccess.Write))
                    {
                        Compile(fontDescription, stream);

                        if (dependencyFile != null)
                        {
                            var dependencies = new FileDependencyList();
                            dependencies.AddDefaultDependencies();
                            dependencies.AddDependencyPath(sourceXmlFile);
                            dependencies.Save(dependencyFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// Compiles the specified font description into a <see cref="SpriteFontData" /> object.
        /// </summary>
        /// <param name="fontDescription">The font description.</param>
        /// <param name="stream">The stream to output the compiled SpriteFontData.</param>
        /// <returns>A SpriteFontData object.</returns>
        public static void Compile(FontDescription fontDescription, Stream stream)
        {
            MakeSpriteFont(fontDescription, stream);
        }

        /// <summary>
        /// Compiles the specified font description into a <see cref="SpriteFontData"/> object.
        /// </summary>
        /// <param name="fontDescription">The font description.</param>
        /// <returns>A SpriteFontData object.</returns>
        public static SpriteFontData Compile(FontDescription fontDescription)
        {
            // We are using a MemoryStream, this is not efficient
            // but this was a quickiest way to use existing from MakeSpriteFont from DirectXTk
            var stream = new MemoryStream();
            MakeSpriteFont(fontDescription, stream);
            stream.Position = 0;
            return SpriteFontData.Load(stream);
        }

        static void MakeSpriteFont(FontDescription options, Stream stream)
        {
            float lineSpacing;

            Glyph[] glyphs = ImportFont(options, out lineSpacing);

            // Optimize.
            foreach (Glyph glyph in glyphs)
            {
                GlyphCropper.Crop(glyph);
            }

            Bitmap bitmap = GlyphPacker.ArrangeGlyphs(glyphs);

            // Adjust line and character spacing.
            lineSpacing += options.LineSpacing;

            foreach (Glyph glyph in glyphs)
            {
                glyph.XAdvance += options.Spacing;
            }

            // Automatically detect whether this is a monochromatic or color font?
            if (options.Format == FontTextureFormat.Auto)
            {
                bool isMono = BitmapUtils.IsRgbEntirely(Color.White, bitmap);

                options.Format = isMono ? FontTextureFormat.CompressedMono :
                                                 FontTextureFormat.Rgba32;
            }

            // Convert to premultiplied alpha format.
            if (!options.NoPremultiply)
            {
                BitmapUtils.PremultiplyAlpha(bitmap);
            }

            SpriteFontWriter.WriteSpriteFont(options, stream, glyphs, lineSpacing, bitmap);
        }

        static Glyph[] ImportFont(FontDescription options, out float lineSpacing)
        {
            // Which importer knows how to read this source font?
            IFontImporter importer;

            string fileExtension = Path.GetExtension(options.FontName).ToLowerInvariant();

            var BitmapFileExtensions = new List<string> { ".bmp", ".png", ".gif" };

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

            // Get all glyphs
            var glyphs = new List<Glyph>(importer.Glyphs);

            // Validate.
            if (glyphs.Count == 0)
            {
                throw new Exception("Font does not contain any glyphs.");
            }

            // Sort the glyphs
            glyphs.Sort((left, right) => left.Character.CompareTo(right.Character));


            // Check that the default character is part of the glyphs
            if (options.DefaultCharacter != 0)
            {
                bool defaultCharacterFound = false;
                foreach (var glyph in glyphs)
                {
                    if (glyph.Character == options.DefaultCharacter)
                    {
                        defaultCharacterFound = true;
                        break;
                    }
                }
                if (!defaultCharacterFound)
                {
                    throw new InvalidOperationException("The specified DefaultCharacter is not part of this font.");
                }
            }

            return glyphs.ToArray();
        }

    }
}