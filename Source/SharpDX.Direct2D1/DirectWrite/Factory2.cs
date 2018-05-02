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

namespace SharpDX.DirectWrite
{
    /// <summary>	
    /// <p>Creates a rendering parameters object with the specified properties.</p>	
    /// </summary>	
    /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='IDWriteFactory1']/*"/>	
    /// <msdn-id>Hh780402</msdn-id>	
    /// <unmanaged>IDWriteFactory2</unmanaged>	
    /// <unmanaged-short>IDWriteFactory2</unmanaged>	
    public partial class Factory2
    {

        /// <summary>
        /// <p>This method is called on a glyph run to translate it in to multiple color glyph runs.</p>
        /// </summary>
        /// <param name = "baselineOriginX"><dd>  <p>The horizontal baseline origin of the original glyph run.</p> </dd></param>
        /// <param name = "baselineOriginY"><dd>  <p>The vertical baseline origin of the original glyph run.</p> </dd></param>
        /// <param name = "glyphRun"><dd>  <p>Original glyph run containing monochrome glyph IDs.</p> </dd></param>
        /// <param name = "glyphRunDescription"><dd>  <p>Optional glyph run description.</p> </dd></param>
        /// <param name = "measuringMode"><dd>  <p>Measuring mode used to compute glyph positions if the run contains color glyphs.</p> </dd></param>
        /// <param name = "worldToDeviceTransform"><dd>  <p> World transform multiplied by any DPI scaling. This is needed to compute glyph positions if the run contains color glyphs and the  measuring mode is not <strong>DWRITE_MEASURING_MODE_NATURAL</strong>.  If this parameter is <strong><c>null</c></strong>, and identity transform is assumed. </p> </dd></param>
        /// <param name = "colorPaletteIndex"><dd>  <p> Zero-based index of the color palette to use. Valid indices are less than the number of palettes in the font, as  returned by <strong>IDWriteFontFace2::GetColorPaletteCount</strong>. </p> </dd></param>
        /// <param name = "colorLayers"><dd>  <p> If the original glyph run contains color glyphs, this parameter receives a reference to  an <strong><see cref = "SharpDX.DirectWrite.ColorGlyphRunEnumerator"/></strong> interface.  The client uses the returned interface to get information about glyph runs and associated colors to render instead of the original glyph run.  If the original glyph run does not contain color glyphs, this method returns <strong>DWRITE_E_NOCOLOR</strong> and the output reference is <strong><c>null</c></strong>. </p> </dd></param>
        /// <returns><p>If this method succeeds, it returns <strong><see cref = "SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref = "SharpDX.Result"/></strong> error code.</p></returns>
        /// <remarks>
        /// <p> If the code calls this method with a glyph run that contains no color information, the method returns <strong>DWRITE_E_NOCOLOR</strong> to  let the application know that it can just draw the original glyph run. If the glyph run contains color information, the function returns an object that can be enumerated through to expose runs and associated colors. The application then  calls <strong>DrawGlyphRun</strong> with each of the returned glyph runs and foreground colors. </p>
        /// </remarks>
        /// <msdn-id>dn280451</msdn-id>
        /// <unmanaged>HRESULT IDWriteFactory2::TranslateColorGlyphRun([In] float baselineOriginX,[In] float baselineOriginY,[In] const DWRITE_GLYPH_RUN* glyphRun,[In, Optional] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[In] DWRITE_MEASURING_MODE measuringMode,[In, Optional] const DWRITE_MATRIX* worldToDeviceTransform,[In] unsigned int colorPaletteIndex,[Out] IDWriteColorGlyphRunEnumerator** colorLayers)</unmanaged>
        /// <unmanaged-short>IDWriteFactory2::TranslateColorGlyphRun</unmanaged-short>
        public void TranslateColorGlyphRun(System.Single baselineOriginX, System.Single baselineOriginY, SharpDX.DirectWrite.GlyphRun glyphRun, SharpDX.DirectWrite.GlyphRunDescription glyphRunDescription, SharpDX.Direct2D1.MeasuringMode measuringMode, SharpDX.Mathematics.Interop.RawMatrix3x2? worldToDeviceTransform, System.Int32 colorPaletteIndex, out SharpDX.DirectWrite.ColorGlyphRunEnumerator colorLayers)
        {
            var result = TryTranslateColorGlyphRun(baselineOriginX, baselineOriginY, glyphRun, glyphRunDescription, measuringMode, worldToDeviceTransform, colorPaletteIndex, out colorLayers);
            result.CheckError();
        }


        /// <summary>
        /// <p>This method is called on a glyph run to translate it in to multiple color glyph runs.</p>
        /// </summary>
        /// <param name = "baselineOriginX"><dd>  <p>The horizontal baseline origin of the original glyph run.</p> </dd></param>
        /// <param name = "baselineOriginY"><dd>  <p>The vertical baseline origin of the original glyph run.</p> </dd></param>
        /// <param name = "glyphRun"><dd>  <p>Original glyph run containing monochrome glyph IDs.</p> </dd></param>
        /// <param name = "glyphRunDescription"><dd>  <p>Optional glyph run description.</p> </dd></param>
        /// <param name = "measuringMode"><dd>  <p>Measuring mode used to compute glyph positions if the run contains color glyphs.</p> </dd></param>
        /// <param name = "worldToDeviceTransform"><dd>  <p> World transform multiplied by any DPI scaling. This is needed to compute glyph positions if the run contains color glyphs and the  measuring mode is not <strong>DWRITE_MEASURING_MODE_NATURAL</strong>.  If this parameter is <strong><c>null</c></strong>, and identity transform is assumed. </p> </dd></param>
        /// <param name = "colorPaletteIndex"><dd>  <p> Zero-based index of the color palette to use. Valid indices are less than the number of palettes in the font, as  returned by <strong>IDWriteFontFace2::GetColorPaletteCount</strong>. </p> </dd></param>
        /// <returns><dd>  <p> If the original glyph run contains color glyphs, this parameter receives a reference to  an <strong><see cref = "SharpDX.DirectWrite.ColorGlyphRunEnumerator"/></strong> interface.  The client uses the returned interface to get information about glyph runs and associated colors to render instead of the original glyph run.  If the original glyph run does not contain color glyphs, this method returns <strong>DWRITE_E_NOCOLOR</strong> and the output reference is <strong><c>null</c></strong>. </p> </dd></returns>
        /// <remarks>
        /// <p> If the code calls this method with a glyph run that contains no color information, the method returns <strong>DWRITE_E_NOCOLOR</strong> to  let the application know that it can just draw the original glyph run. If the glyph run contains color information, the function returns an object that can be enumerated through to expose runs and associated colors. The application then  calls <strong>DrawGlyphRun</strong> with each of the returned glyph runs and foreground colors. </p>
        /// </remarks>
        /// <msdn-id>dn280451</msdn-id>
        /// <unmanaged>HRESULT IDWriteFactory2::TranslateColorGlyphRun([In] float baselineOriginX,[In] float baselineOriginY,[In] const DWRITE_GLYPH_RUN* glyphRun,[In, Optional] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[In] DWRITE_MEASURING_MODE measuringMode,[In, Optional] const DWRITE_MATRIX* worldToDeviceTransform,[In] unsigned int colorPaletteIndex,[Out] IDWriteColorGlyphRunEnumerator** colorLayers)</unmanaged>
        /// <unmanaged-short>IDWriteFactory2::TranslateColorGlyphRun</unmanaged-short>
        public SharpDX.DirectWrite.ColorGlyphRunEnumerator TranslateColorGlyphRun(System.Single baselineOriginX, System.Single baselineOriginY, SharpDX.DirectWrite.GlyphRun glyphRun, SharpDX.DirectWrite.GlyphRunDescription glyphRunDescription, SharpDX.Direct2D1.MeasuringMode measuringMode, SharpDX.Mathematics.Interop.RawMatrix3x2? worldToDeviceTransform, System.Int32 colorPaletteIndex)
        {
            SharpDX.DirectWrite.ColorGlyphRunEnumerator colorLayers;
            TranslateColorGlyphRun(baselineOriginX, baselineOriginY, glyphRun, glyphRunDescription, measuringMode, worldToDeviceTransform, colorPaletteIndex, out colorLayers);
            return colorLayers;
        }
    }
}