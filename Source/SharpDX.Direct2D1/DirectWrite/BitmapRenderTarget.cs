// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using SharpDX;

namespace SharpDX.DirectWrite
{
    public partial class BitmapRenderTarget
    {
        /// <summary>	
        /// Draws a run of glyphs to a bitmap target at the specified position.	
        /// </summary>	
        /// <remarks>	
        /// You can use the IDWriteBitmapRenderTarget::DrawGlyphRun to render to a bitmap from a custom text renderer that you implement.  The custom text renderer should call this method from within the <see cref="M:SharpDX.DirectWrite.TextRenderer.DrawGlyphRun(System.IntPtr,System.Single,System.Single,SharpDX.DirectWrite.MeasuringMode,SharpDX.DirectWrite.GlyphRun,SharpDX.DirectWrite.GlyphRunDescription,SharpDX.ComObject)" /> callback method as shown in the following code. 	
        /// <code> STDMETHODIMP GdiTextRenderer::DrawGlyphRun( __maybenull void* clientDrawingContext, FLOAT baselineOriginX, FLOAT baselineOriginY, DWRITE_MEASURING_MODE measuringMode, __in DWRITE_GLYPH_RUN const* glyphRun, __in DWRITE_GLYPH_RUN_DESCRIPTION const* glyphRunDescription, IUnknown* clientDrawingEffect )	
        /// { HRESULT hr = S_OK; // Pass on the drawing call to the render target to do the real work. RECT dirtyRect = {0}; hr = pRenderTarget_-&gt;DrawGlyphRun( baselineOriginX, baselineOriginY, measuringMode, glyphRun, pRenderingParams_, RGB(0,200,255), &amp;dirtyRect ); return hr;	
        /// } </code>	
        /// 
        /// The baselineOriginX, baslineOriginY, measuringMethod, and glyphRun parameters are provided (as arguments) when the callback method is invoked.  The renderingParams, textColor and blackBoxRect are not. Default rendering params can be retrieved by using the <see cref="M:SharpDX.DirectWrite.Factory.CreateMonitorRenderingParams(System.IntPtr,SharpDX.DirectWrite.RenderingParams@)" /> method.  	
        /// </remarks>	
        /// <param name="baselineOriginX">The horizontal position of the baseline origin, in DIPs, relative to the upper-left corner of the DIB. </param>
        /// <param name="baselineOriginY">The vertical position of the baseline origin, in DIPs, relative to the upper-left corner of the DIB. </param>
        /// <param name="measuringMode">The measuring method for glyphs in the run, used with the other properties to determine the rendering mode. </param>
        /// <param name="glyphRun">The structure containing the properties of the glyph run. </param>
        /// <param name="renderingParams">The object that controls rendering behavior. </param>
        /// <param name="textColor">The foreground color of the text. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteBitmapRenderTarget::DrawGlyphRun([None] float baselineOriginX,[None] float baselineOriginY,[None] DWRITE_MEASURING_MODE measuringMode,[In] const DWRITE_GLYPH_RUN* glyphRun,[None] IDWriteRenderingParams* renderingParams,[None] COLORREF textColor,[Out, Optional] RECT* blackBoxRect)</unmanaged>
        public  Result DrawGlyphRun(float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode, GlyphRun glyphRun, RenderingParams renderingParams, Color4 textColor)
        {
            Rectangle temp;
            return DrawGlyphRun(baselineOriginX, baselineOriginY, measuringMode, glyphRun, renderingParams, textColor, out temp);
        }
    }
}