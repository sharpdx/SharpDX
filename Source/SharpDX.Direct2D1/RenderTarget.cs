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
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    public partial class RenderTarget
    {
        private float _strokeWidth = DefaultStrokeWidth;

        /// <summary>
        /// Default stroke width used for all methods that are not explicitly using it. Default is set to 1.0f.
        /// </summary>
        public const float DefaultStrokeWidth = 1.0f;

        /// <summary>
        /// Get or set the default stroke width used for all methods that are not explicitly using it. Default is set to 1.0f.
        /// </summary>
        public float StrokeWidth
        {
            get { return _strokeWidth; }
            set { _strokeWidth = value; }
        }

        /// <summary>	
        /// Creates a render target that draws to a DirectX Graphics Infrastructure (DXGI) surface. 	
        /// </summary>	
        /// <remarks>	
        /// To write to a Direct3D surface, you obtain an <see cref="SharpDX.DXGI.Surface"/> and pass it to the {{CreateDxgiSurfaceRenderTarget}} method to create a DXGI surface render target; you can then use the DXGI surface render target to draw 2-D content to the DXGI surface.  A DXGI surface render target is a type of <see cref="SharpDX.Direct2D1.RenderTarget"/>. Like other Direct2D render targets, you can use it to create resources and issue drawing commands. The DXGI surface render target and the DXGI surface must use the same DXGI format. If you specify the {{DXGI_FORMAT_UNKOWN}} format when you create the render target, it will automatically use the surface's format.The DXGI surface render target does not perform DXGI surface synchronization. To work with Direct2D, the Direct3D device that provides the <see cref="SharpDX.DXGI.Surface"/> must be created with the D3D10_CREATE_DEVICE_BGRA_SUPPORT flag.For more information about creating and using DXGI surface render targets, see the {{Direct2D and Direct3D Interoperability Overview}}.When you create a render target and hardware acceleration is available, you allocate resources on the computer's GPU. By creating a render target once and retaining it as long as possible, you gain performance benefits. Your application should create render targets once and hold onto them for the life of the application or until the render target's {{EndDraw}} method returns the {{D2DERR_RECREATE_TARGET}} error. When you receive this error, you need to recreate the render target (and any resources it created). 	
        /// </remarks>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="dxgiSurface">The DXGI surface to bind this render target to</param>
        /// <param name="properties">The rendering mode, pixel format, remoting options, DPI information, and the minimum DirectX support required for hardware rendering. For information about supported pixel formats, see  {{Supported Pixel  Formats and Alpha Modes}}.</param>
        public RenderTarget(Factory factory, Surface dxgiSurface,  RenderTargetProperties properties)
            : base(IntPtr.Zero)
        {
            factory.CreateDxgiSurfaceRenderTarget(dxgiSurface, ref properties, this);
        }


        /// <summary>	
        /// Draws the specified bitmap after scaling it to the size of the specified rectangle. 	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawBitmap}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="bitmap">The bitmap to render. </param>
        /// <param name="opacity">A value between 0.0f and 1.0f, inclusive, that specifies an opacity value to apply to the bitmap; this value is multiplied against the alpha values of the bitmap's contents.  The default value is 1.0f. </param>
        /// <param name="interpolationMode">The interpolation mode to use if the bitmap is scaled or rotated by the drawing operation. The default value is <see cref="F:SharpDX.Direct2D1.BitmapInterpolationMode.Linear" />.  </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_RECT_F* destinationRectangle,[None] float opacity,[None] D2D1_BITMAP_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D1_RECT_F* sourceRectangle)</unmanaged>
        public void DrawBitmap(Bitmap bitmap, float opacity, BitmapInterpolationMode interpolationMode)
        {
            DrawBitmap(bitmap, null, opacity, interpolationMode, null);
        }

        /// <summary>	
        /// Draws the specified bitmap after scaling it to the size of the specified rectangle. 	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawBitmap}}) failed, check the result returned by the <see cref="EndDraw()"/> or <see cref="Flush()"/> methods.  	
        /// </remarks>	
        /// <param name="bitmap">The bitmap to render. </param>
        /// <param name="destinationRectangle">The size and position, in device-independent pixels in the render target's coordinate space, of the area to which the bitmap is drawn; NULL to draw the selected portion of the bitmap at the origin of the render target.  If the rectangle is specified but not well-ordered, nothing is drawn, but the render target does not enter an error state. </param>
        /// <param name="opacity">A value between 0.0f and 1.0f, inclusive, that specifies an opacity value to apply to the bitmap; this value is multiplied against the alpha values of the bitmap's contents.  The default value is 1.0f. </param>
        /// <param name="interpolationMode">The interpolation mode to use if the bitmap is scaled or rotated by the drawing operation. The default value is <see cref="SharpDX.Direct2D1.BitmapInterpolationMode.Linear"/>.  </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_RECT_F* destinationRectangle,[None] float opacity,[None] D2D1_BITMAP_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D1_RECT_F* sourceRectangle)</unmanaged>
        public void DrawBitmap(SharpDX.Direct2D1.Bitmap bitmap, RawRectangleF destinationRectangle, float opacity, SharpDX.Direct2D1.BitmapInterpolationMode interpolationMode)
        {
            DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode, null);
        }

        /// <summary>	
        /// Draws the specified bitmap after scaling it to the size of the specified rectangle. 	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawBitmap}}) failed, check the result returned by the <see cref="EndDraw()" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="bitmap">The bitmap to render. </param>
        /// <param name="opacity">A value between 0.0f and 1.0f, inclusive, that specifies an opacity value to apply to the bitmap; this value is multiplied against the alpha values of the bitmap's contents.  The default value is 1.0f. </param>
        /// <param name="interpolationMode">The interpolation mode to use if the bitmap is scaled or rotated by the drawing operation. The default value is <see cref="F:SharpDX.Direct2D1.BitmapInterpolationMode.Linear" />.  </param>
        /// <param name="sourceRectangle">The size and position, in device-independent pixels in the bitmap's coordinate space, of the area within the bitmap to be drawn; NULL to draw the entire bitmap.  </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_RECT_F* destinationRectangle,[None] float opacity,[None] D2D1_BITMAP_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D1_RECT_F* sourceRectangle)</unmanaged>
        public void DrawBitmap(Bitmap bitmap, float opacity, BitmapInterpolationMode interpolationMode, RawRectangleF sourceRectangle)
        {
            DrawBitmap(bitmap, null, opacity, interpolationMode, sourceRectangle);
        }

        /// <summary>	
        /// Draws the outline of the specified ellipse using the specified stroke style. 	
        /// </summary>	
        /// <remarks>	
        /// The {{DrawEllipse}} method doesn't return an error code if it fails. To determine whether a drawing operation (such as DrawEllipse) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="ellipse">The position and radius of the ellipse to draw, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the ellipse's outline. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawEllipse([In] const D2D1_ELLIPSE* ellipse,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawEllipse(Ellipse ellipse, Brush brush)
        {
            DrawEllipse(ellipse, brush, StrokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of the specified ellipse using the specified stroke style. 	
        /// </summary>	
        /// <remarks>	
        /// The {{DrawEllipse}} method doesn't return an error code if it fails. To determine whether a drawing operation (such as DrawEllipse) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="ellipse">The position and radius of the ellipse to draw, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the ellipse's outline. </param>
        /// <param name="strokeWidth">The thickness of the ellipse's stroke. The stroke is centered on the ellipse's outline. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawEllipse([In] const D2D1_ELLIPSE* ellipse,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawEllipse(Ellipse ellipse, Brush brush, float strokeWidth)
        {
            DrawEllipse(ellipse, brush, strokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of the specified geometry. 	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as DrawGeometry) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="geometry">The geometry to draw. </param>
        /// <param name="brush">The brush used to paint the geometry's stroke. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawGeometry(Geometry geometry, Brush brush)
        {
            DrawGeometry(geometry, brush, StrokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of the specified geometry. 	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as DrawGeometry) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="geometry">The geometry to draw. </param>
        /// <param name="brush">The brush used to paint the geometry's stroke. </param>
        /// <param name="strokeWidth">The thickness of the geometry's stroke. The stroke is centered on the geometry's outline. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth)
        {
            DrawGeometry(geometry, brush, strokeWidth, null);
        }

        /// <summary>	
        /// Draws a line between the specified points. 	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as DrawLine) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="point0">The start point of the line, in device-independent pixels. </param>
        /// <param name="point1">The end point of the line, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the line's stroke. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawLine([None] D2D1_POINT_2F point0,[None] D2D1_POINT_2F point1,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush)
        {
            DrawLine(point0, point1, brush, StrokeWidth, null);
        }

        /// <summary>	
        /// Draws a line between the specified points. 	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as DrawLine) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="point0">The start point of the line, in device-independent pixels. </param>
        /// <param name="point1">The end point of the line, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the line's stroke. </param>
        /// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the stroke. If this parameter isn't specified, it defaults to 1.0f.  The stroke is centered on the line. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawLine([None] D2D1_POINT_2F point0,[None] D2D1_POINT_2F point1,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush, float strokeWidth)
        {
            DrawLine(point0, point1, brush, strokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of a rectangle that has the specified dimensions. 	
        /// </summary>	
        /// <remarks>	
        /// When this method fails, it does not return an error code. To determine whether a drawing method (such as {{DrawRectangle}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> method.  	
        /// </remarks>	
        /// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the rectangle's stroke. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawRectangle([In] const D2D1_RECT_F* rect,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawRectangle(RawRectangleF rect, Brush brush)
        {
            DrawRectangle(rect, brush, StrokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of a rectangle that has the specified dimensions and stroke style. 	
        /// </summary>	
        /// <remarks>	
        /// When this method fails, it does not return an error code. To determine whether a drawing method (such as {{DrawRectangle}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> method.  	
        /// </remarks>	
        /// <param name="rect">The dimensions of the rectangle to draw, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the rectangle's stroke. </param>
        /// <param name="strokeWidth">A value greater than or equal to 0.0f that specifies the width of the rectangle's stroke. The stroke is centered on the rectangle's outline. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawRectangle([In] const D2D1_RECT_F* rect,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth)
        {
            DrawRectangle(rect, brush, strokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of the specified rounded rectangle.	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawRoundedRectangle}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="roundedRect">The dimensions of the rounded rectangle to draw, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the rounded rectangle's outline.  </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawRoundedRectangle([In] const D2D1_ROUNDED_RECT* roundedRect,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush)
        {
            DrawRoundedRectangle(ref roundedRect, brush, StrokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of the specified rounded rectangle.	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawRoundedRectangle}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="roundedRect">The dimensions of the rounded rectangle to draw, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the rounded rectangle's outline.  </param>
        /// <param name="strokeWidth">The width of the rounded rectangle's stroke. The stroke is centered on the rounded rectangle's outline. The default value is 1.0f.  </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawRoundedRectangle([In] const D2D1_ROUNDED_RECT* roundedRect,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush, float strokeWidth)
        {
            DrawRoundedRectangle(ref roundedRect, brush, strokeWidth, null);
        }

        /// <summary>	
        /// Draws the outline of the specified rounded rectangle using the specified stroke style.	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawRoundedRectangle}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="roundedRect">The dimensions of the rounded rectangle to draw, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the rounded rectangle's outline.  </param>
        /// <param name="strokeWidth">The width of the rounded rectangle's stroke. The stroke is centered on the rounded rectangle's outline. The default value is 1.0f.  </param>
        /// <param name="strokeStyle">The style of the rounded rectangle's stroke, or NULL to paint a solid stroke. The default value is NULL. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawRoundedRectangle([In] const D2D1_ROUNDED_RECT* roundedRect,[In] ID2D1Brush* brush,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>
        public void DrawRoundedRectangle(RoundedRectangle roundedRect, Brush brush, float strokeWidth, StrokeStyle strokeStyle)
        {
            DrawRoundedRectangle(ref roundedRect, brush, strokeWidth, strokeStyle);
        }

        /// <summary>	
        /// Draws the specified text using the format information provided by an <see cref="T:SharpDX.DirectWrite.TextFormat" /> object. 	
        /// </summary>	
        /// <remarks>	
        /// To create an <see cref="T:SharpDX.DirectWrite.TextFormat" /> object, create an <see cref="T:SharpDX.DirectWrite.Factory" /> and call its {{CreateTextFormat}} method. This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawText}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="text">A reference to an array of Unicode characters to draw.  </param>
        /// <param name="textFormat">An object that describes formatting details of the text to draw, such as the font, the font size, and flow direction.   </param>
        /// <param name="layoutRect">The size and position of the area in which the text is drawn.  </param>
        /// <param name="defaultForegroundBrush">The brush used to paint the text. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawTextA([In, Buffer] const wchar_t* string,[None] int stringLength,[In] IDWriteTextFormat* textFormat,[In] const D2D1_RECT_F* layoutRect,[In] ID2D1Brush* defaultForegroundBrush,[None] D2D1_DRAW_TEXT_OPTIONS options,[None] DWRITE_MEASURING_MODE measuringMode)</unmanaged>
        public void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush)
        {
            DrawText(text, text.Length, textFormat, layoutRect, defaultForegroundBrush, DrawTextOptions.None, MeasuringMode.Natural);
        }

        /// <summary>	
        /// Draws the specified text using the format information provided by an <see cref="T:SharpDX.DirectWrite.TextFormat" /> object. 	
        /// </summary>	
        /// <remarks>	
        /// To create an <see cref="T:SharpDX.DirectWrite.TextFormat" /> object, create an <see cref="T:SharpDX.DirectWrite.Factory" /> and call its {{CreateTextFormat}} method. This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawText}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="text">A reference to an array of Unicode characters to draw.  </param>
        /// <param name="textFormat">An object that describes formatting details of the text to draw, such as the font, the font size, and flow direction.   </param>
        /// <param name="layoutRect">The size and position of the area in which the text is drawn.  </param>
        /// <param name="defaultForegroundBrush">The brush used to paint the text. </param>
        /// <param name="options">A value that indicates whether the text should be snapped to pixel boundaries and whether the text should be clipped to the layout rectangle. The default value is <see cref="F:SharpDX.Direct2D1.DrawTextOptions.None" />, which indicates that text should be snapped to pixel boundaries and it should not be clipped to the layout rectangle. </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawTextA([In, Buffer] const wchar_t* string,[None] int stringLength,[In] IDWriteTextFormat* textFormat,[In] const D2D1_RECT_F* layoutRect,[In] ID2D1Brush* defaultForegroundBrush,[None] D2D1_DRAW_TEXT_OPTIONS options,[None] DWRITE_MEASURING_MODE measuringMode)</unmanaged>
        public void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush, DrawTextOptions options)
        {
            DrawText(text, text.Length, textFormat, layoutRect, defaultForegroundBrush, options, MeasuringMode.Natural);
        }

        /// <summary>	
        /// Draws the specified text using the format information provided by an <see cref="T:SharpDX.DirectWrite.TextFormat" /> object. 	
        /// </summary>	
        /// <remarks>	
        /// To create an <see cref="T:SharpDX.DirectWrite.TextFormat" /> object, create an <see cref="T:SharpDX.DirectWrite.Factory" /> and call its {{CreateTextFormat}} method. This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{DrawText}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="text">A reference to an array of Unicode characters to draw.  </param>
        /// <param name="textFormat">An object that describes formatting details of the text to draw, such as the font, the font size, and flow direction.   </param>
        /// <param name="layoutRect">The size and position of the area in which the text is drawn.  </param>
        /// <param name="defaultForegroundBrush">The brush used to paint the text. </param>
        /// <param name="options">A value that indicates whether the text should be snapped to pixel boundaries and whether the text should be clipped to the layout rectangle. The default value is <see cref="F:SharpDX.Direct2D1.DrawTextOptions.None" />, which indicates that text should be snapped to pixel boundaries and it should not be clipped to the layout rectangle. </param>
        /// <param name="measuringMode">A value that indicates how glyph metrics are used to measure text when it is formatted.  The default value is DWRITE_MEASURING_MODE_NATURAL.  </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawTextA([In, Buffer] const wchar_t* string,[None] int stringLength,[In] IDWriteTextFormat* textFormat,[In] const D2D1_RECT_F* layoutRect,[In] ID2D1Brush* defaultForegroundBrush,[None] D2D1_DRAW_TEXT_OPTIONS options,[None] DWRITE_MEASURING_MODE measuringMode)</unmanaged>
        public void DrawText(string text, TextFormat textFormat, RawRectangleF layoutRect, Brush defaultForegroundBrush, DrawTextOptions options, MeasuringMode measuringMode)
        {
            DrawText(text, text.Length, textFormat, layoutRect, defaultForegroundBrush, options, measuringMode);
        }

        /// <summary>	
        /// Draws the formatted text described by the specified <see cref="T:SharpDX.DirectWrite.TextLayout" /> object.	
        /// </summary>	
        /// <remarks>	
        /// When drawing the same text repeatedly, using the DrawTextLayout method is more efficient than using the {{DrawText}} method because the text doesn't need to be formatted and the layout processed with each call. This method doesn't return an error code if it fails. To determine whether a drawing operation (such as DrawTextLayout) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="origin">The point, described in device-independent pixels, at which the upper-left corner of the text described by textLayout is drawn. </param>
        /// <param name="textLayout">The formatted text to draw. Any drawing effects that do not inherit from <see cref="T:SharpDX.Direct2D1.Resource" /> are ignored. If there are drawing effects that inherit from ID2D1Resource that are not brushes, this method fails and the render target is put in an error state.  </param>
        /// <param name="defaultForegroundBrush">The brush used to paint any text in textLayout that does not already have a brush associated with it as a drawing effect (specified by the <see cref="M:SharpDX.DirectWrite.TextLayout.SetDrawingEffect(SharpDX.ComObject,SharpDX.DirectWrite.TextRange)" /> method).  </param>
        /// <unmanaged>void ID2D1RenderTarget::DrawTextLayout([None] D2D1_POINT_2F origin,[In] IDWriteTextLayout* textLayout,[In] ID2D1Brush* defaultForegroundBrush,[None] D2D1_DRAW_TEXT_OPTIONS options)</unmanaged>
        public void DrawTextLayout(RawVector2 origin, TextLayout textLayout, Brush defaultForegroundBrush)
        {
            DrawTextLayout(origin, textLayout, defaultForegroundBrush, DrawTextOptions.None);
        }

        /// <summary>	
        /// <p>Ends drawing operations  on the render target and indicates the current error state and associated tags. </p>	
        /// </summary>	
        /// <param name="tag1"><dd>  <p>When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</p> </dd></param>	
        /// <param name="tag2"><dd>  <p>When this method returns, contains the tag for drawing operations that caused errors or 0 if there were no errors. This parameter is passed uninitialized.</p> </dd></param>	
        /// <returns><p>If the method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code and sets <em>tag1</em> and <em>tag2</em> to the tags that were active when the error occurred. </p></returns>	
        /// <remarks>	
        /// <p>Drawing operations can only be issued between a <strong>BeginDraw</strong> and <strong>EndDraw</strong> call.</p><p> <strong>BeginDraw</strong> and <strong>EndDraw</strong> are use to indicate that a render target is in use by the Direct2D system. Different implementations of <strong><see cref="SharpDX.Direct2D1.RenderTarget"/></strong> might behave differently when <strong>BeginDraw</strong> is called. An <strong><see cref="SharpDX.Direct2D1.BitmapRenderTarget"/></strong> may be locked between <strong>BeginDraw</strong>/<strong>EndDraw</strong> calls, a DXGI surface render target might be acquired on <strong>BeginDraw</strong> and released on <strong>EndDraw</strong>, while an <strong><see cref="SharpDX.Direct2D1.WindowRenderTarget"/></strong> may begin batching at <strong>BeginDraw</strong> and may present on <strong>EndDraw</strong>, for example.</p><p> The <strong>BeginDraw</strong> method must be called before rendering operations can be called, though state-setting and state-retrieval operations can be performed even outside of <strong>BeginDraw</strong>/<strong>EndDraw</strong>. </p><p>After <strong>BeginDraw</strong> is called, a render target will normally build up a batch of rendering commands, but defer processing of these commands until either an internal buffer is full, the <strong>Flush</strong> method is called, or until <strong>EndDraw</strong> is called. The <strong>EndDraw</strong> method causes any batched drawing operations to complete, and then returns an <strong><see cref="SharpDX.Result"/></strong> indicating the success of the operations and, optionally, the tag state of the render target at the time the error occurred. The <strong>EndDraw</strong> method always succeeds: it should not be called twice even if a previous <strong>EndDraw</strong> resulted in a failing <strong><see cref="SharpDX.Result"/></strong>. </p><p>If <strong>EndDraw</strong> is called without a matched call to <strong>BeginDraw</strong>, it returns an error indicating that <strong>BeginDraw</strong> must be called before <strong>EndDraw</strong>. Calling <strong>BeginDraw</strong> twice on a render target puts the target into an error state where nothing further is drawn, and returns an appropriate <strong><see cref="SharpDX.Result"/></strong> and error information when <strong>EndDraw</strong> is called.	
        /// </p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1RenderTarget::EndDraw']/*"/>	
        /// <msdn-id>dd371924</msdn-id>	
        /// <unmanaged>HRESULT ID2D1RenderTarget::EndDraw([Out, Optional] unsigned longlong* tag1,[Out, Optional] unsigned longlong* tag2)</unmanaged>	
        /// <unmanaged-short>ID2D1RenderTarget::EndDraw</unmanaged-short>	
        public void EndDraw(out long tag1, out long tag2)
        {
            SharpDX.Result result = TryEndDraw(out tag1, out tag2);
            result.CheckError();
        }

        /// <summary>	
        /// Ends drawing operations  on the render target and indicates the current error state and associated tags. 	
        /// </summary>	
        /// <remarks>	
        /// Drawing operations can only be issued between a {{BeginDraw}} and EndDraw call.BeginDraw and EndDraw are use to indicate that a render target is in use by the Direct2D system. Different implementations of <see cref="SharpDX.Direct2D1.RenderTarget"/> might behave differently when {{BeginDraw}} is called. An <see cref="SharpDX.Direct2D1.BitmapRenderTarget"/> may be locked between BeginDraw/EndDraw calls, a DXGI surface render target might be acquired on BeginDraw and released on EndDraw, while an <see cref="WindowRenderTarget"/> may begin batching at BeginDraw and may present on EndDraw, for example. The BeginDraw method must be called before rendering operations can be called, though state-setting and state-retrieval operations can be performed even outside of {{BeginDraw}}/EndDraw. After {{BeginDraw}} is called, a render target will normally build up a batch of rendering commands, but defer processing of these commands until either an internal buffer is full, the {{Flush}} method is called, or until EndDraw is called. The EndDraw method causes any batched drawing operations to complete, and then returns an HRESULT indicating the success of the operations and, optionally, the tag state of the render target at the time the error occurred. The EndDraw method always succeeds: it should not be called twice even if a previous EndDraw resulted in a failing HRESULT. If EndDraw is called without a matched call to {{BeginDraw}}, it returns an error indicating that BeginDraw must be called before EndDraw. Calling BeginDraw twice on a render target puts the target into an error state where nothing further is drawn, and returns an appropriate HRESULT and error information when EndDraw is called.	
        /// </remarks>	
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code and sets tag1 and tag2 to the tags that were active when the error occurred. </returns>
        public void EndDraw()
        {
            long tag1;
            long tag2;
            EndDraw(out tag1, out tag2);
        }

        /// <summary>	
        /// Paints the interior of the specified geometry. 	
        /// </summary>	
        /// <remarks>	
        /// If the opacityBrush parameter is not NULL, the alpha value of each pixel of the mapped opacityBrush is used to determine the resulting opacity of each corresponding pixel of the geometry. Only the alpha value of each color in the brush is used for this processing; all other color information is ignored.  The alpha value specified by the brush is multiplied by the alpha value of the geometry after the geometry has been painted by brush.  	
        /// When this method fails, it does not return an error code. To determine whether a drawing operation (such as FillGeometry) failed, check the result returned by the <see cref="EndDraw()"/> or <see cref="Flush()"/> method. 	
        /// </remarks>	
        /// <param name="geometry">The geometry to paint.</param>
        /// <param name="brush">The brush used to paint the geometry's interior.</param>
        /// <unmanaged>void FillGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[In, Optional] ID2D1Brush* opacityBrush)</unmanaged>
        public void FillGeometry(SharpDX.Direct2D1.Geometry geometry, SharpDX.Direct2D1.Brush brush)
        {
            FillGeometry(geometry, brush, null);
        }

        /// <summary>	
        /// Applies the opacity mask described by the specified bitmap to a brush and uses that brush to paint a region of the render target.    	
        /// </summary>	
        /// <remarks>	
        /// For this method to work properly, the render target must be using the <see cref="F:SharpDX.Direct2D1.AntialiasMode.Aliased" /> antialiasing mode. You can set the antialiasing mode by calling the <see cref="M:SharpDX.Direct2D1.RenderTarget.SetAntialiasMode(SharpDX.Direct2D1.AntialiasMode)" /> method. This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{FillOpacityMask}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="opacityMask">The opacity mask to apply to the brush. The alpha value of each pixel in the  region specified by sourceRectangle is multiplied with the alpha value of the brush after the brush has been mapped to the area defined by destinationRectangle. </param>
        /// <param name="brush">The brush used to paint the region of the render target specified by destinationRectangle. </param>
        /// <param name="content">The type of content the opacity mask contains. The value is used to determine the color space in which the opacity mask is blended. </param>
        /// <unmanaged>void ID2D1RenderTarget::FillOpacityMask([In] ID2D1Bitmap* opacityMask,[In] ID2D1Brush* brush,[None] D2D1_OPACITY_MASK_CONTENT content,[In, Optional] const D2D1_RECT_F* destinationRectangle,[In, Optional] const D2D1_RECT_F* sourceRectangle)</unmanaged>
        public void FillOpacityMask(Bitmap opacityMask, Brush brush, OpacityMaskContent content)
        {
            FillOpacityMask(opacityMask, brush, content, null, null);
        }

        /// <summary>	
        /// Paints the interior of the specified rounded rectangle.	
        /// </summary>	
        /// <remarks>	
        /// This method doesn't return an error code if it fails. To determine whether a drawing operation (such as {{FillRoundedRectangle}}) failed, check the result returned by the <see cref="M:SharpDX.Direct2D1.RenderTarget.EndDraw(System.Int64@,System.Int64@)" /> or <see cref="M:SharpDX.Direct2D1.RenderTarget.Flush(System.Int64@,System.Int64@)" /> methods.  	
        /// </remarks>	
        /// <param name="roundedRect">The dimensions of the rounded rectangle to paint, in device-independent pixels. </param>
        /// <param name="brush">The brush used to paint the interior of the rounded rectangle. </param>
        /// <unmanaged>void ID2D1RenderTarget::FillRoundedRectangle([In] const D2D1_ROUNDED_RECT* roundedRect,[In] ID2D1Brush* brush)</unmanaged>
        public void FillRoundedRectangle(RoundedRectangle roundedRect, Brush brush)
        {
            FillRoundedRectangle(ref roundedRect, brush);
        }

        /// <summary>	
        /// Executes all pending drawing commands. 	
        /// </summary>	
        /// <remarks>	
        /// This command does not flush the device that is associated with the render target.   Calling this method resets the error state of the render target. 	
        /// </remarks>	
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code and sets tag1 and tag2 to the tags that were active when the error occurred. If no error occurred, this method sets the error tag state to be (0,0). </returns>
        /// <unmanaged>HRESULT ID2D1RenderTarget::Flush([Out, Optional] D2D1_TAG* tag1,[Out, Optional] D2D1_TAG* tag2)</unmanaged>
        public void Flush()
        {
            long tag1;
            long tag2;
            Flush(out tag1, out tag2);
        }

        /// <summary>	
        /// Get or sets the dots per inch (DPI) of the render target. 	
        /// </summary>	
        /// <remarks>	
        /// This method specifies the mapping from pixel space to device-independent space  for the render target.  If both dpiX and dpiY are 0, the factory-read system DPI is chosen. If one parameter is zero and the other unspecified, the DPI is not changed. For <see cref="WindowRenderTarget"/>, the DPI defaults to the most recently factory-read system DPI. The default value for all other render targets is 96 DPI.   	
        /// </remarks>	
        /// <unmanaged>void ID2D1RenderTarget::SetDpi([None] float dpiX,[None] float dpiY)</unmanaged>
        public Size2F DotsPerInch
        {
            get
            {
                float y;
                float x;
                GetDpi(out x, out y);
                return new Size2F(x, y);
            }
            set
            {
                SetDpi(value.Width, value.Height);
            }
        }
    }
}
