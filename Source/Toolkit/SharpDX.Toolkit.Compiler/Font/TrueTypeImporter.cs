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
using System.Globalization;
using System.Runtime.InteropServices;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using Factory = SharpDX.DirectWrite.Factory;

namespace SharpDX.Toolkit.Graphics
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    
    // Uses System.Drawing (aka GDI+) to rasterize TrueType fonts into a series of glyph bitmaps.
    internal class TrueTypeImporter : IFontImporter
    {
        // Properties hold the imported font data.
        public IEnumerable<Glyph> Glyphs { get; private set; }

        public float LineSpacing { get; private set; }


        // Size of the temp surface used for GDI+ rasterization.
        const int MaxGlyphSize = 1024;


        public void Import(FontDescription options)
        {
            //ImportBitmapFonts(options);
            //return;
            var factory = new DirectWrite.Factory();
            DirectWrite.Font font = null;

            using (var fontCollection = factory.GetSystemFontCollection(false))
            {
                int index;
                if(!fontCollection.FindFamilyName(options.FontName, out index))
                {
                    // Lets try to import System.Drawing for old system bitmap fonts (like MS Sans Serif)
                    throw new FontException(string.Format("Can't find font '{0}'.", options.FontName));
                }

                using(var fontFamily = fontCollection.GetFontFamily(index))
                {
                    var weight = FontWeight.Regular;
                    var style = DirectWrite.FontStyle.Normal;
                    switch(options.Style)
                    {
                        case FontStyle.Bold:
                            weight = FontWeight.Bold;
                            break;
                        case FontStyle.Italic:
                            weight = FontWeight.Regular;
                            style = DirectWrite.FontStyle.Italic;
                            break;
                        case FontStyle.Regular:
                            weight = FontWeight.Regular;
                            break;
                    }

                    font = fontFamily.GetFirstMatchingFont(weight, DirectWrite.FontStretch.Normal, style);
                }
            }

            var fontFace = new FontFace(font);
            var fontMetrics = fontFace.Metrics;

            // Create a bunch of GDI+ objects.
            var fontSize = PointsToPixels(options.Size);
            
            // Which characters do we want to include?
            var characters = CharacterRegion.Flatten(options.CharacterRegions);

            var glyphList = new List<Glyph>();

            // Store the font height.
            LineSpacing = (float)Math.Round((float)(fontMetrics.LineGap + fontMetrics.Ascent + fontMetrics.Descent) / fontMetrics.DesignUnitsPerEm * fontSize);

            var baseLine = (float)Math.Round((float)(fontMetrics.LineGap + fontMetrics.Ascent) / fontMetrics.DesignUnitsPerEm * fontSize);

            // If font size <= 13, use aliased fonts instead
            bool activateAntiAliasDetection = options.Size > 13;

            // Rasterize each character in turn.
            foreach (char character in characters)
            {
                var glyph = ImportGlyph(factory, fontFace, character, fontMetrics, fontSize, activateAntiAliasDetection);
                glyph.YOffset += baseLine;

                glyphList.Add(glyph);
            }

            Glyphs = glyphList;

        }

        private Glyph ImportGlyph(Factory factory, FontFace fontFace, char character, FontMetrics fontMetrics, float fontSize, bool activateAntiAliasDetection)
        {
            var indices = fontFace.GetGlyphIndices(new int[] {character});

            var metrics = fontFace.GetDesignGlyphMetrics(indices, false);
            var metric = metrics[0];

            var width = (float)(metric.AdvanceWidth - metric.LeftSideBearing - metric.RightSideBearing) / fontMetrics.DesignUnitsPerEm * fontSize;
            var height = (float)(metric.AdvanceHeight - metric.TopSideBearing - metric.BottomSideBearing) / fontMetrics.DesignUnitsPerEm * fontSize;

            var xOffset = (float)metric.LeftSideBearing / fontMetrics.DesignUnitsPerEm * fontSize;
            var yOffset = (float)(metric.TopSideBearing - metric.VerticalOriginY) / fontMetrics.DesignUnitsPerEm * fontSize;

            var advanceWidth = (float)(metric.AdvanceWidth) / fontMetrics.DesignUnitsPerEm * fontSize;
            var advanceHeight = (float)(metric.AdvanceHeight) / fontMetrics.DesignUnitsPerEm * fontSize;

            var pixelWidth = (int)Math.Ceiling(width + 2);
            var pixelHeight = (int)Math.Ceiling(height + 2);

            Bitmap bitmap;
            if(char.IsWhiteSpace(character))
            {
                bitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            }
            else
            {
                var glyphRun = new GlyphRun()
                               {
                                   FontFace = fontFace,
                                   Advances = new[] { (float)Math.Round(advanceWidth) },
                                   FontSize = fontSize,
                                   BidiLevel = 0,
                                   Indices = indices,
                                   IsSideways = false,
                                   Offsets = new[] {new GlyphOffset()}
                               };

                var matrix = SharpDX.Matrix.Identity;
                matrix.M41 = -(float)Math.Floor(xOffset - 1);
                matrix.M42 = -(float)Math.Floor(yOffset - 1);

                RenderingMode renderingMode;
                if (activateAntiAliasDetection)
                {
                    var rtParams = new RenderingParams(factory);
                    renderingMode = fontFace.GetRecommendedRenderingMode(fontSize, 1.0f, MeasuringMode.Natural, rtParams);
                    rtParams.Dispose();
                }
                else
                {
                    renderingMode = RenderingMode.Aliased;
                }

                using(var runAnalysis = new GlyphRunAnalysis(factory,
                    glyphRun,
                    1.0f,
                    matrix,
                    renderingMode,
                    MeasuringMode.Natural,
                    0.0f,
                    0.0f))
                {
                    var bounds = new SharpDX.Rectangle(0, 0, pixelWidth, pixelHeight);
                    bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);

                    if(renderingMode == RenderingMode.Aliased)
                    {
                        var texture = new byte[bounds.Width * bounds.Height];
                        runAnalysis.CreateAlphaTexture(TextureType.Aliased1x1, bounds, texture, texture.Length);
                        bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
                        for (int y = 0; y < bounds.Height; y++)
                        {
                            for (int x = 0; x < bounds.Width; x++)
                            {
                                int pixelX = y * bounds.Width + x;
                                var grey = texture[pixelX];
                                var color = Color.FromArgb(grey, grey, grey);

                                bitmap.SetPixel(x, y, color);
                            }
                        }
                    }
                    else
                    {
                        var texture = new byte[bounds.Width * bounds.Height * 3];
                        runAnalysis.CreateAlphaTexture(TextureType.Cleartype3x1, bounds, texture, texture.Length);
                        for (int y = 0; y < bounds.Height; y++)
                        {
                            for (int x = 0; x < bounds.Width; x++)
                            {
                                int pixelX = (y * bounds.Width + x) * 3;
                                var red = texture[pixelX];
                                var green = texture[pixelX + 1];
                                var blue = texture[pixelX + 2];
                                var color = Color.FromArgb(red, green, blue);

                                bitmap.SetPixel(x, y, color);
                            }
                        }
                    }

                    //var positionUnderline = (float)fontMetrics.UnderlinePosition / fontMetrics.DesignUnitsPerEm * fontSize;
                    //var positionUnderlineSize = (float)fontMetrics.UnderlineThickness / fontMetrics.DesignUnitsPerEm * fontSize;
                }
            }

            var glyph = new Glyph(character, bitmap)
                        {
                            XOffset = (float)Math.Floor(xOffset-1),
                            XAdvance = (float)Math.Round(advanceWidth),
                            YOffset = (float)Math.Floor(yOffset-1),
                        };
            return glyph;
        }

        public void ImportBitmapFonts(FontDescription options)
        {
            // Create a bunch of GDI+ objects.
            using (Font font = CreateFont(options))
            using (Brush brush = new SolidBrush(Color.White))
            using (StringFormat stringFormat = new StringFormat(StringFormatFlags.NoFontFallback))
            using (Bitmap bitmap = new Bitmap(MaxGlyphSize, MaxGlyphSize, PixelFormat.Format32bppArgb))
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
            {
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                // Which characters do we want to include?
                var characters = CharacterRegion.Flatten(options.CharacterRegions);

                var glyphList = new List<Glyph>();

                // Rasterize each character in turn.
                foreach (char character in characters)
                {
                    Glyph glyph = ImportGlyph(character, font, brush, stringFormat, bitmap, graphics);

                    glyphList.Add(glyph);
                }

                Glyphs = glyphList;

                // Store the font height.
                LineSpacing = font.GetHeight();
            }
        }


        // Attempts to instantiate the requested GDI+ font object.
        static Font CreateFont(FontDescription options)
        {
            Font font = new Font(options.FontName, PointsToPixels(options.Size), (System.Drawing.FontStyle)options.Style, GraphicsUnit.Pixel);

            try
            {
                // The font constructor automatically substitutes fonts if it can't find the one requested.
                // But we prefer the caller to know if anything is wrong with their data. A simple string compare
                // isn't sufficient because some fonts (eg. MS Mincho) change names depending on the locale.

                // Early out: in most cases the name will match the current or invariant culture.
                if (options.FontName.Equals(font.FontFamily.GetName(CultureInfo.CurrentCulture.LCID), StringComparison.OrdinalIgnoreCase) ||
                    options.FontName.Equals(font.FontFamily.GetName(CultureInfo.InvariantCulture.LCID), StringComparison.OrdinalIgnoreCase))
                {
                    return font;
                }

                // Check the font name in every culture.
                foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    if (options.FontName.Equals(font.FontFamily.GetName(culture.LCID), StringComparison.OrdinalIgnoreCase))
                    {
                        return font;
                    }
                }

                // A font substitution must have occurred.
                throw new FontException(string.Format("Can't find font '{0}'.", options.FontName));
            }
            catch
            {
                font.Dispose();
                throw;
            }
        }


        // Converts a font size from points to pixels. Can't just let GDI+ do this for us,
        // because we want identical results on every machine regardless of system DPI settings.
        static float PointsToPixels(float points)
        {
            return points * 96 / 72;
        }

        
        // Rasterizes a single character glyph.
        static Glyph ImportGlyph(char character, Font font, Brush brush, StringFormat stringFormat, Bitmap bitmap, System.Drawing.Graphics graphics)
        {
            string characterString = character.ToString();

            // Measure the size of this character.
            SizeF size = graphics.MeasureString(characterString, font, Point.Empty, stringFormat);

            int characterWidth = (int)Math.Ceiling(size.Width);
            int characterHeight = (int)Math.Ceiling(size.Height);

            // Pad to make sure we capture any overhangs (negative ABC spacing, etc.)
            int padWidth = characterWidth;
            int padHeight = characterHeight / 2;

            int bitmapWidth = characterWidth + padWidth * 2;
            int bitmapHeight = characterHeight + padHeight * 2;

            if (bitmapWidth > MaxGlyphSize || bitmapHeight > MaxGlyphSize)
                throw new FontException("Excessively large glyph won't fit in my lazily implemented fixed size temp surface.");

            // Render the character.
            graphics.Clear(Color.Black);
            graphics.DrawString(characterString, font, brush, padWidth, padHeight, stringFormat);
            graphics.Flush();

            // Clone the newly rendered image.
            Bitmap glyphBitmap = bitmap.Clone(new Rectangle(0, 0, bitmapWidth, bitmapHeight), PixelFormat.Format32bppArgb);

            //BitmapUtils.ConvertGreyToAlpha(glyphBitmap);

            // Query its ABC spacing.
            float? abc = GetCharacterWidth(character, font, graphics);

            // Construct the output Glyph object.
            return new Glyph(character, glyphBitmap)
            {
                XOffset = -padWidth,
                XAdvance = abc.HasValue ? padWidth - bitmapWidth + abc.Value : -padWidth,
                YOffset = -padHeight,
            };
        }


        // Queries APC spacing for the specified character.
        static float? GetCharacterWidth(char character, Font font, System.Drawing.Graphics graphics)
        {
            // Look up the native device context and font handles.
            IntPtr hdc = graphics.GetHdc();

            try
            {
                IntPtr hFont = font.ToHfont();

                try
                {
                    // Select our font into the DC.
                    IntPtr oldFont = NativeMethods.SelectObject(hdc, hFont);

                    try
                    {
                        // Query the character spacing.
                        var result = new NativeMethods.ABCFloat[1];

                        if (NativeMethods.GetCharABCWidthsFloatW(hdc, character, character, result))
                        {
                            return result[0].A + 
                                   result[0].B + 
                                   result[0].C;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    finally
                    {
                        NativeMethods.SelectObject(hdc, oldFont);
                    }
                }
                finally
                {
                    NativeMethods.DeleteObject(hFont);
                }
            }
            finally
            {
                graphics.ReleaseHdc(hdc);
            }
        }


        // Interop to the native GDI GetCharABCWidthsFloat method.
        static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll")]
            public static extern bool GetCharABCWidthsFloatW(IntPtr hdc, uint iFirstChar, uint iLastChar, [Out] ABCFloat[] lpABCF);


            [StructLayout(LayoutKind.Sequential)]
            public struct ABCFloat
            {
                public float A;
                public float B;
                public float C;
            }
        }
    }
}
