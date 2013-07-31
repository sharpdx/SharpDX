﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Default abstract implementation of TextRenderer. Need to implement a least a DrawXXX method to use it.
    /// </summary>
    public abstract class TextRendererBase : CallbackBase, TextRenderer
    {
        /// <summary>
        /// Determines whether pixel snapping is disabled. The recommended default is FALSE,
        /// unless doing animation that requires subpixel vertical placement.
        /// </summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <returns>Receives TRUE if pixel snapping is disabled or FALSE if it not. </returns>
        /// <unmanaged>HRESULT IsPixelSnappingDisabled([None] void* clientDrawingContext,[Out] BOOL* isDisabled)</unmanaged>
        public virtual bool IsPixelSnappingDisabled(object clientDrawingContext)
        {
            return false;
        }

        /// <summary>	
        ///  Gets a transform that maps abstract coordinates to DIPs. 	
        /// </summary>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>a structure which has transform information for  pixel snapping.</returns>
        /// <unmanaged>HRESULT GetCurrentTransform([None] void* clientDrawingContext,[Out] DWRITE_MATRIX* transform)</unmanaged>
        public virtual Matrix3x2 GetCurrentTransform(object clientDrawingContext)
        {
            return new Matrix3x2 { M11 = 1, M22 = 1 };
        }

        /// <summary>	
        ///  Gets the number of physical pixels per DIP. 	
        /// </summary>	
        /// <remarks>	
        ///  Because a DIP (device-independent pixel) is 1/96 inch,  the pixelsPerDip value is the number of logical pixels per inch divided by 96.	
        /// </remarks>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>the number of physical pixels per DIP</returns>
        /// <unmanaged>HRESULT GetPixelsPerDip([None] void* clientDrawingContext,[Out] FLOAT* pixelsPerDip)</unmanaged>
        public virtual float GetPixelsPerDip(object clientDrawingContext)
        {
            return 1.0f;
        }

        /// <summary>	
        ///  IDWriteTextLayout::Draw calls this function to instruct the client to render a run of glyphs. 	
        /// </summary>	
        /// <remarks>	
        /// The <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/> function calls this callback function with all the information about glyphs to render. The application implements this callback by mostly delegating the call to the underlying platform's graphics API such as {{Direct2D}} to draw glyphs on the drawing context. An application that uses GDI can implement this callback in terms of the <see cref="BitmapRenderTarget.DrawGlyphRun(float,float,MeasuringMode,SharpDX.DirectWrite.GlyphRun,SharpDX.DirectWrite.RenderingParams,SharpDX.Color4)"/> method.	
        /// </remarks>	
        /// <param name="clientDrawingContext">The application-defined drawing context passed to  <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <param name="baselineOriginX">The pixel location (X-coordinate) at the baseline origin of the glyph run.</param>
        /// <param name="baselineOriginY">The pixel location (Y-coordinate) at the baseline origin of the glyph run.</param>
        /// <param name="measuringMode"> The measuring method for glyphs in the run, used with the other properties to determine the rendering mode.</param>
        /// <param name="glyphRun">Pointer to the glyph run instance to render. </param>
        /// <param name="glyphRunDescription">A pointer to the optional glyph run description instance which contains properties of the characters  associated with this run.</param>
        /// <param name="clientDrawingEffect">Application-defined drawing effects for the glyphs to render. Usually this argument represents effects such as the foreground brush filling the interior of text.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT DrawGlyphRun([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[None] DWRITE_MEASURING_MODE measuringMode,[In] const DWRITE_GLYPH_RUN* glyphRun,[In] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[None] IUnknown* clientDrawingEffect)</unmanaged>
        public virtual Result DrawGlyphRun(object clientDrawingContext, float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode, GlyphRun glyphRun, GlyphRunDescription glyphRunDescription, ComObject clientDrawingEffect)
        {
            return Result.NotImplemented;
        }

        /// <summary>	
        ///  IDWriteTextLayout::Draw calls this function to instruct the client to draw an underline. 	
        /// </summary>	
        /// <remarks>	
        ///  A single underline can be broken into multiple calls, depending on how the formatting changes attributes. If font sizes/styles change within an underline, the thickness and offset will be averaged weighted according to characters. To get an appropriate starting pixel position, add underline::offset to the baseline. Otherwise there will be no spacing between the text. The x coordinate will always be passed as the left side, regardless of text directionality. This simplifies drawing and reduces the problem of round-off that could potentially cause gaps or a double stamped alpha blend. To avoid alpha overlap, round the end points to the nearest device pixel. 	
        /// </remarks>	
        /// <param name="clientDrawingContext">The application-defined drawing context passed to  IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">The pixel location (X-coordinate) at the baseline origin of the run where underline applies.</param>
        /// <param name="baselineOriginY">The pixel location (Y-coordinate) at the baseline origin of the run where underline applies.</param>
        /// <param name="underline">Pointer to  a structure containing underline logical information.</param>
        /// <param name="clientDrawingEffect"> Application-defined effect to apply to the underline. Usually this argument represents effects such as the foreground brush filling the interior of a line.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT DrawUnderline([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[In] const DWRITE_UNDERLINE* underline,[None] IUnknown* clientDrawingEffect)</unmanaged>
        public virtual Result DrawUnderline(object clientDrawingContext, float baselineOriginX, float baselineOriginY, ref Underline underline, ComObject clientDrawingEffect)
        {
            return Result.NotImplemented;
        }

        /// <summary>	
        ///  IDWriteTextLayout::Draw calls this function to instruct the client to draw a strikethrough. 	
        /// </summary>	
        /// <remarks>	
        ///  A single strikethrough can be broken into multiple calls, depending on how the formatting changes attributes. Strikethrough is not averaged across font sizes/styles changes. To get an appropriate starting pixel position, add strikethrough::offset to the baseline. Like underlines, the x coordinate will always be passed as the left side, regardless of text directionality. 	
        /// </remarks>	
        /// <param name="clientDrawingContext">The application-defined drawing context passed to  IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">The pixel location (X-coordinate) at the baseline origin of the run where strikethrough applies.</param>
        /// <param name="baselineOriginY">The pixel location (Y-coordinate) at the baseline origin of the run where strikethrough applies.</param>
        /// <param name="strikethrough">Pointer to  a structure containing strikethrough logical information.</param>
        /// <param name="clientDrawingEffect">Application-defined effect to apply to the strikethrough.  Usually this argument represents effects such as the foreground brush filling the interior of a line.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT DrawStrikethrough([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[In] const DWRITE_STRIKETHROUGH* strikethrough,[None] IUnknown* clientDrawingEffect)</unmanaged>
        public virtual Result DrawStrikethrough(object clientDrawingContext, float baselineOriginX, float baselineOriginY, ref Strikethrough strikethrough, ComObject clientDrawingEffect)
        {
            return Result.NotImplemented;
        }

        /// <summary>	
        ///  IDWriteTextLayout::Draw calls this application callback when it needs to draw an inline object. 	
        /// </summary>	
        /// <param name="clientDrawingContext">The application-defined drawing context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="originX">X-coordinate at the top-left corner of the inline object.</param>
        /// <param name="originY">Y-coordinate at the top-left corner of the inline object.</param>
        /// <param name="inlineObject">The application-defined inline object set using IDWriteTextFormat::SetInlineObject.</param>
        /// <param name="isSideways">A Boolean flag that indicates whether the object's baseline runs alongside the baseline axis of the line.</param>
        /// <param name="isRightToLeft">A Boolean flag that indicates whether the object is in a right-to-left context, hinting that the drawing may want to mirror the normal image.</param>
        /// <param name="clientDrawingEffect">Application-defined drawing effects for the glyphs to render. Usually this argument represents effects such as the foreground brush filling the interior of a line.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT DrawInlineObject([None] void* clientDrawingContext,[None] FLOAT originX,[None] FLOAT originY,[None] IDWriteInlineObject* inlineObject,[None] BOOL isSideways,[None] BOOL isRightToLeft,[None] IUnknown* clientDrawingEffect)</unmanaged>
        public virtual Result DrawInlineObject(object clientDrawingContext, float originX, float originY, InlineObject inlineObject, bool isSideways, bool isRightToLeft, ComObject clientDrawingEffect)
        {
            return Result.NotImplemented;
        }
   }
}