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

using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectWrite
{
    public partial class GlyphRunAnalysis
    {
        /// <summary>
        /// Creates a glyph run analysis object, which encapsulates information used to render a glyph run.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="glyphRun">A structure that contains the properties of the glyph run (font face, advances, and so on).</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP (device independent pixel). For example, if rendering onto a 96 DPI bitmap then pixelsPerDip is 1. If rendering onto a 120 DPI bitmap then pixelsPerDip is 1.25.</param>
        /// <param name="renderingMode">A value that specifies the rendering mode, which must be one of the raster rendering modes (that is, not default and not outline).</param>
        /// <param name="measuringMode">Specifies the measuring mode to use with glyphs.</param>
        /// <param name="baselineOriginX">The horizontal position (X-coordinate) of the baseline origin, in DIPs.</param>
        /// <param name="baselineOriginY">Vertical position (Y-coordinate) of the baseline origin, in DIPs.</param>
        /// <remarks>
        /// The glyph run analysis object contains the results of analyzing the glyph run, including the positions of all the glyphs and references to all of the rasterized glyphs in the font cache.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFactory::CreateGlyphRunAnalysis([In] const DWRITE_GLYPH_RUN* glyphRun,[None] float pixelsPerDip,[In, Optional] const DWRITE_MATRIX* transform,[None] DWRITE_RENDERING_MODE renderingMode,[None] DWRITE_MEASURING_MODE measuringMode,[None] float baselineOriginX,[None] float baselineOriginY,[Out] IDWriteGlyphRunAnalysis** glyphRunAnalysis)</unmanaged>
        public GlyphRunAnalysis(Factory factory, GlyphRun glyphRun, float pixelsPerDip, RenderingMode renderingMode, MeasuringMode measuringMode, float baselineOriginX, float baselineOriginY)
            : this(factory, glyphRun, pixelsPerDip, null, renderingMode, measuringMode, baselineOriginX, baselineOriginY)
        {
        }

        /// <summary>
        /// Creates a glyph run analysis object, which encapsulates information used to render a glyph run.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="glyphRun">A structure that contains the properties of the glyph run (font face, advances, and so on).</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP (device independent pixel). For example, if rendering onto a 96 DPI bitmap then pixelsPerDip is 1. If rendering onto a 120 DPI bitmap then pixelsPerDip is 1.25.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified the emSize and pixelsPerDip.</param>
        /// <param name="renderingMode">A value that specifies the rendering mode, which must be one of the raster rendering modes (that is, not default and not outline).</param>
        /// <param name="measuringMode">Specifies the measuring mode to use with glyphs.</param>
        /// <param name="baselineOriginX">The horizontal position (X-coordinate) of the baseline origin, in DIPs.</param>
        /// <param name="baselineOriginY">Vertical position (Y-coordinate) of the baseline origin, in DIPs.</param>
        /// <remarks>
        /// The glyph run analysis object contains the results of analyzing the glyph run, including the positions of all the glyphs and references to all of the rasterized glyphs in the font cache.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFactory::CreateGlyphRunAnalysis([In] const DWRITE_GLYPH_RUN* glyphRun,[None] float pixelsPerDip,[In, Optional] const DWRITE_MATRIX* transform,[None] DWRITE_RENDERING_MODE renderingMode,[None] DWRITE_MEASURING_MODE measuringMode,[None] float baselineOriginX,[None] float baselineOriginY,[Out] IDWriteGlyphRunAnalysis** glyphRunAnalysis)</unmanaged>
        public GlyphRunAnalysis(Factory factory, GlyphRun glyphRun, float pixelsPerDip, RawMatrix3x2? transform, RenderingMode renderingMode, MeasuringMode measuringMode, float baselineOriginX, float baselineOriginY)
        {
            factory.CreateGlyphRunAnalysis(glyphRun, pixelsPerDip, transform, renderingMode, measuringMode, baselineOriginX, baselineOriginY, this);
        }
    }
}