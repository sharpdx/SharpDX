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
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectWrite
{
    public partial class BitmapRenderTarget
    {
        /// <summary>	
        /// <p> Draws a run of glyphs to a bitmap target at the specified position.</p>	
        /// </summary>	
        /// <param name="baselineOriginX"><dd>  <p> The horizontal position of the baseline origin, in DIPs, relative to the upper-left corner of the DIB.</p> </dd></param>	
        /// <param name="baselineOriginY"><dd>  <p> The vertical position of the baseline origin, in DIPs, relative to the upper-left corner of the DIB.</p> </dd></param>	
        /// <param name="measuringMode"><dd>  <p> The measuring method for glyphs in the run, used with the other properties to determine the rendering mode.</p> </dd></param>	
        /// <param name="glyphRun"><dd>  <p> The structure containing the properties of the glyph run.</p> </dd></param>	
        /// <param name="renderingParams"><dd>  <p> The object that controls rendering behavior.</p> </dd></param>	
        /// <param name="textColor"><dd>  <p> The foreground color of the text.</p> </dd></param>	
        /// <param name="blackBoxRect"><dd>  <p> The optional rectangle that receives the bounding box (in pixels not DIPs) of all the pixels affected by  drawing the glyph run. The black box rectangle may extend beyond the dimensions of the bitmap.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>You can use the <strong><see cref="SharpDX.DirectWrite.BitmapRenderTarget.DrawGlyphRun"/></strong> to render to a bitmap from a custom text renderer that you implement.  The custom text renderer should call this method from within the <strong><see cref="SharpDX.DirectWrite.TextRenderer.DrawGlyphRun"/></strong> callback method as shown in the following code.</p><pre>STDMETHODIMP GdiTextRenderer::DrawGlyphRun( __maybenull void* clientDrawingContext, FLOAT baselineOriginX, FLOAT baselineOriginY, <see cref="SharpDX.Direct2D1.MeasuringMode"/> measuringMode, __in <see cref="SharpDX.DirectWrite.GlyphRun"/> const* glyphRun, __in <see cref="SharpDX.DirectWrite.GlyphRunDescription"/> const* glyphRunDescription, <see cref="SharpDX.ComObject"/>* clientDrawingEffect )	
        /// { <see cref="SharpDX.Result"/> hr = <see cref="SharpDX.Result.Ok"/>; // Pass on the drawing call to the render target to do the real work. <see cref="SharpDX.Mathematics.Interop.RawRectangle"/> dirtyRect = {0}; hr = pRenderTarget_-&gt;DrawGlyphRun( baselineOriginX, baselineOriginY, measuringMode, glyphRun, pRenderingParams_, RGB(0,200,255), &amp;dirtyRect ); return hr;	
        /// }	
        /// </pre><p>The <em>baselineOriginX</em>, <em>baslineOriginY</em>, <em>measuringMethod</em>, and <em>glyphRun</em> parameters are provided (as arguments) when the callback method is invoked.  The <em>renderingParams</em>, <em>textColor</em> and <em>blackBoxRect</em> are not.</p><p>Default rendering params can be retrieved by using the <strong><see cref="SharpDX.DirectWrite.Factory.CreateMonitorRenderingParams"/></strong> method.</p><p></p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDWriteBitmapRenderTarget::DrawGlyphRun']/*"/>	
        /// <msdn-id>dd368167</msdn-id>	
        /// <unmanaged>HRESULT IDWriteBitmapRenderTarget::DrawGlyphRun([In] float baselineOriginX,[In] float baselineOriginY,[In] DWRITE_MEASURING_MODE measuringMode,[In] const DWRITE_GLYPH_RUN* glyphRun,[In] IDWriteRenderingParams* renderingParams,[In] int textColor,[Out, Optional] RECT* blackBoxRect)</unmanaged>	
        /// <unmanaged-short>IDWriteBitmapRenderTarget::DrawGlyphRun</unmanaged-short>	
        public void DrawGlyphRun(float baselineOriginX,
            float baselineOriginY,
            SharpDX.Direct2D1.MeasuringMode measuringMode,
            SharpDX.DirectWrite.GlyphRun glyphRun,
            SharpDX.DirectWrite.RenderingParams renderingParams,
            RawColorBGRA textColor,
            out SharpDX.Mathematics.Interop.RawRectangle blackBoxRect)
        {
            int colorRgb = (textColor.R ) | (textColor.G << 8) | (textColor.B << 16);
            DrawGlyphRun(baselineOriginX, baselineOriginY, measuringMode, glyphRun, renderingParams, colorRgb, out blackBoxRect);
        }

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
        public void DrawGlyphRun(float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode, GlyphRun glyphRun, RenderingParams renderingParams, RawColorBGRA textColor)
        {
            RawRectangle temp;
            DrawGlyphRun(baselineOriginX, baselineOriginY, measuringMode, glyphRun, renderingParams, textColor, out temp);
        }
    }
}