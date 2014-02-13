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

using System;
using System.Collections.Generic;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.DirectWrite.Factory;

namespace SharpDX.Toolkit.Graphics
{
    using System.Drawing;
    using System.Drawing.Imaging;
    
    // This code was originally taken from DirectXTk but rewritten with DirectWrite
    // for more accuracy in font rendering
    internal class TrueTypeImporter : IFontImporter
    {
        // Properties hold the imported font data.
        public IEnumerable<Glyph> Glyphs { get; private set; }

        public float LineSpacing { get; private set; }

        public void Import(FontDescription options)
        {
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
            LineSpacing = (float)(fontMetrics.LineGap + fontMetrics.Ascent + fontMetrics.Descent) / fontMetrics.DesignUnitsPerEm * fontSize;

            var baseLine = (float)(fontMetrics.LineGap + fontMetrics.Ascent) / fontMetrics.DesignUnitsPerEm * fontSize;

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

            factory.Dispose();
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

            var advanceWidth = (float)metric.AdvanceWidth / fontMetrics.DesignUnitsPerEm * fontSize;
            var advanceHeight = (float)metric.AdvanceHeight / fontMetrics.DesignUnitsPerEm * fontSize;

            var pixelWidth = (int)Math.Ceiling(width + 4);
            var pixelHeight = (int)Math.Ceiling(height + 4);

            var matrix = SharpDX.Matrix.Identity;
            matrix.M41 = -(float)Math.Floor(xOffset);
            matrix.M42 = -(float)Math.Floor(yOffset);

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

                    if (renderingMode == RenderingMode.Aliased)
                    {

                        var texture = new byte[bounds.Width * bounds.Height];
                        runAnalysis.CreateAlphaTexture(TextureType.Aliased1x1, bounds, texture, texture.Length);
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
                }
            }

            var glyph = new Glyph(character, bitmap)
                        {
                            XOffset = -matrix.M41,
                            XAdvance = (float)Math.Round(advanceWidth),
                            YOffset = -matrix.M42,
                        };
            return glyph;
        }

        // Converts a font size from points to pixels. Can't just let GDI+ do this for us,
        // because we want identical results on every machine regardless of system DPI settings.
        static float PointsToPixels(float points)
        {
            return points * 96 / 72;
        }
    }
}
