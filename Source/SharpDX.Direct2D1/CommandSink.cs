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

using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    public partial interface CommandSink
    {
        /// <summary>	
        /// Begins a draw sequence.
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::BeginDraw()</unmanaged>	
        void BeginDraw();

        /// <summary>	
        /// Ends a draw sequence.
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::EndDraw()</unmanaged>	
        void EndDraw();

        /// <summary>	
        /// Sets the antialias mode.
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::SetAntialiasMode([In] D2D1_ANTIALIAS_MODE antialiasMode)</unmanaged>	
        SharpDX.Direct2D1.AntialiasMode AntialiasMode { set; }

        /// <summary>	
        /// Sets tags.
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::SetTags([In] unsigned longlong tag1,[In] unsigned longlong tag2)</unmanaged>	
        void SetTags(long tag1, long tag2);

        /// <summary>	
        /// Sets the text antialias mode.
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::SetTextAntialiasMode([In] D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode)</unmanaged>	
        SharpDX.Direct2D1.TextAntialiasMode TextAntialiasMode { set; }

        /// <summary>	
        /// Sets the parameters for text rendering.
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::SetTextRenderingParams([In, Optional] IDWriteRenderingParams* textRenderingParams)</unmanaged>	
        SharpDX.DirectWrite.RenderingParams TextRenderingParams { set; }

        /// <summary>	
        /// Sets the matrix transform.
        /// </summary>	
        /// <remarks>	
        /// The transform will be applied to the corresponding device context.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::SetTransform([In] const D2D_MATRIX_3X2_F* transform)</unmanaged>	
        RawMatrix3x2 Transform { set; }

        /// <summary>	
        /// Sets the blending for primitives.
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::SetPrimitiveBlend([In] D2D1_PRIMITIVE_BLEND primitiveBlend)</unmanaged>	
        SharpDX.Direct2D1.PrimitiveBlend PrimitiveBlend { set; }

        /// <summary>	
        /// Sets the unit mode
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::SetUnitMode([In] D2D1_UNIT_MODE unitMode)</unmanaged>	
        SharpDX.Direct2D1.UnitMode UnitMode { set; }

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="color"><para>The color to which the command sink should be cleared.</para></param>	
        /// <remarks>	
        /// The clear color is restricted by the currently selected clip and layer bounds.If no color is specified, the color should be interpreted by context. Examples include but are not limited to:Transparent black for a premultiplied bitmap target. Opaque black for an ignore bitmap target. Containing no content (or white) for a printer page.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::Clear([In, Optional] const D2D_COLOR_F* color)</unmanaged>	
        void Clear(RawColor4? color = null);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="baselineOrigin"><para>The sequence of  glyphs to be sent.</para></param>	
        /// <param name="glyphRun"><para>Additional non-rendering information about the glyphs.</para></param>	
        /// <param name="glyphRunDescription"><para>The brush used to fill the glyphs.</para></param>	
        /// <param name="foregroundBrush"><para>The measuring mode to apply to the glyphs.</para></param>	
        /// <param name="measuringMode">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1CommandSink::DrawGlyphRun([In] D2D_POINT_2F baselineOrigin,[In] const DWRITE_GLYPH_RUN* glyphRun,[In, Optional] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[In] ID2D1Brush* foregroundBrush,[In] DWRITE_MEASURING_MODE measuringMode)</unmanaged>	
        void DrawGlyphRun(RawVector2 baselineOrigin, SharpDX.DirectWrite.GlyphRun glyphRun, SharpDX.DirectWrite.GlyphRunDescription glyphRunDescription, SharpDX.Direct2D1.Brush foregroundBrush, SharpDX.Direct2D1.MeasuringMode measuringMode);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="point0"><para>The start point of the line.</para></param>	
        /// <param name="point1"><para>The end point of the line.</para></param>	
        /// <param name="brush"><para>The brush used to fill the line.</para></param>	
        /// <param name="strokeWidth"><para>The width of the stroke to fill the line.</para></param>	
        /// <param name="strokeStyle"><para>The style of the stroke. If not specified, the stroke is solid.</para></param>	
        /// <remarks>	
        /// Additional References	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::DrawLine([In] D2D_POINT_2F point0,[In] D2D_POINT_2F point1,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
        void DrawLine(RawVector2 point0, RawVector2 point1, SharpDX.Direct2D1.Brush brush, float strokeWidth, SharpDX.Direct2D1.StrokeStyle strokeStyle);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="geometry"><para>The geometry to be stroked.</para></param>	
        /// <param name="brush"><para>The brush that will be used to fill the stroked geometry.</para></param>	
        /// <param name="strokeWidth"><para>The width of the stroke.</para></param>	
        /// <param name="strokeStyle"><para>The style of the stroke.</para></param>	
        /// <remarks>	
        /// You must convert ellipses and rounded rectangles to the corresponding ellipse and rounded rectangle geometries before calling into the DrawGeometry method.Additional ReferencesID2D1CommandList::Stream, <see cref="SharpDX.Direct2D1.RenderTarget.DrawGeometry"/>RequirementsMinimum supported operating systemSame as Interface / Class Highest IRQL levelN/A (user mode) Callable from DlllMain()No Callable from services and session 0Yes Callable from UI threadYes?	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::DrawGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
        void DrawGeometry(SharpDX.Direct2D1.Geometry geometry, SharpDX.Direct2D1.Brush brush, float strokeWidth, SharpDX.Direct2D1.StrokeStyle strokeStyle);

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="rect">No documentation.</param>	
        /// <param name="brush">No documentation.</param>	
        /// <param name="strokeWidth">No documentation.</param>	
        /// <param name="strokeStyle">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1CommandSink::DrawRectangle([In] const D2D_RECT_F* rect,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
        void DrawRectangle(RawRectangleF rect, SharpDX.Direct2D1.Brush brush, float strokeWidth, SharpDX.Direct2D1.StrokeStyle strokeStyle);

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="bitmap">No documentation.</param>	
        /// <param name="destinationRectangle">No documentation.</param>	
        /// <param name="opacity">No documentation.</param>	
        /// <param name="interpolationMode">No documentation.</param>	
        /// <param name="sourceRectangle">No documentation.</param>	
        /// <param name="erspectiveTransformRef">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1CommandSink::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D_RECT_F* destinationRectangle,[In] float opacity,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D_RECT_F* sourceRectangle,[In, Optional] const D2D_MATRIX_4X4_F* perspectiveTransform)</unmanaged>	
        void DrawBitmap(SharpDX.Direct2D1.Bitmap bitmap, RawRectangleF? destinationRectangle, float opacity, SharpDX.Direct2D1.InterpolationMode interpolationMode, RawRectangleF? sourceRectangle, RawMatrix? erspectiveTransformRef);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="image"><para>The image to be drawn to the command sink.</para></param>	
        /// <param name="targetOffset"><para>This defines the offset in the destination space that the image will be rendered to. The entire logical extent of the image will be rendered to the corresponding destination. If not specified, the destination origin will be (0, 0). The top-left corner of the image will be mapped to the target offset. This will not necessarily be the origin.</para></param>	
        /// <param name="imageRectangle"><para>The corresponding rectangle in the image space will be mapped to the provided origins when processing the image.</para></param>	
        /// <param name="interpolationMode"><para>The interpolation mode that will be used to scale the image if necessary.</para></param>	
        /// <param name="compositeMode"><para>If specified, the composite mode that will be applied to the limits of the currently selected clip.</para></param>	
        /// <remarks>	
        /// Because the image can itself be a command list or contain an effect graph that in turn contains a command list, this method can result in recursive processing.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::DrawImage([In] ID2D1Image* image,[In, Optional] const D2D_POINT_2F* targetOffset,[In, Optional] const D2D_RECT_F* imageRectangle,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In] D2D1_COMPOSITE_MODE compositeMode)</unmanaged>	
        void DrawImage(SharpDX.Direct2D1.Image image, RawVector2? targetOffset, RawRectangleF? imageRectangle, SharpDX.Direct2D1.InterpolationMode interpolationMode, SharpDX.Direct2D1.CompositeMode compositeMode);

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="gdiMetafile">No documentation.</param>	
        /// <param name="targetOffset">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1CommandSink::DrawGdiMetafile([In] ID2D1GdiMetafile* gdiMetafile,[In, Optional] const D2D_POINT_2F* targetOffset)</unmanaged>	
        void DrawGdiMetafile(SharpDX.Direct2D1.GdiMetafile gdiMetafile, RawVector2? targetOffset);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="mesh"><para>The mesh object to be filled.</para></param>	
        /// <param name="brush"><para>The brush with which to fill the mesh.</para></param>	
        /// <unmanaged>HRESULT ID2D1CommandSink::FillMesh([In] ID2D1Mesh* mesh,[In] ID2D1Brush* brush)</unmanaged>	
        void FillMesh(SharpDX.Direct2D1.Mesh mesh, SharpDX.Direct2D1.Brush brush);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="opacityMask"><para>The bitmap whose alpha channel will be sampled to define the opacity mask.</para></param>	
        /// <param name="brush"><para>The brush with which to fill the mask.</para></param>	
        /// <param name="destinationRectangle"><para>The type of content that the mask represents.</para></param>	
        /// <param name="sourceRectangle"><para>The destination rectangle in which to fill the mask. If not specified, this is the origin.</para></param>	
        /// <remarks>	
        /// The opacity mask bitmap must be considered to be clamped on each axis.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::FillOpacityMask([In] ID2D1Bitmap* opacityMask,[In] ID2D1Brush* brush,[In, Optional] const D2D_RECT_F* destinationRectangle,[In, Optional] const D2D_RECT_F* sourceRectangle)</unmanaged>	
        void FillOpacityMask(SharpDX.Direct2D1.Bitmap opacityMask, SharpDX.Direct2D1.Brush brush, RawRectangleF? destinationRectangle, RawRectangleF? sourceRectangle);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="geometry"><para>The geometry that should be filled.</para></param>	
        /// <param name="brush"><para>The primary brush used to fill the geometry.</para></param>	
        /// <param name="opacityBrush"><para>A brush whose alpha channel is used to modify the opacity of the primary fill brush.  </para></param>	
        /// <remarks>	
        /// If the opacity brush is specified, the primary brush will be a bitmap brush fixed on both the x-axis and the y-axis.Ellipses and rounded rectangles are converted to the corresponding geometry before being passed to FillGeometry.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::FillGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[In, Optional] ID2D1Brush* opacityBrush)</unmanaged>	
        void FillGeometry(SharpDX.Direct2D1.Geometry geometry, SharpDX.Direct2D1.Brush brush, SharpDX.Direct2D1.Brush opacityBrush);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="rect"><para>The rectangle to fill.</para></param>	
        /// <param name="brush"><para>The brush with which to fill the rectangle.</para></param>	
        /// <unmanaged>HRESULT ID2D1CommandSink::FillRectangle([In] const D2D_RECT_F* rect,[In] ID2D1Brush* brush)</unmanaged>	
        void FillRectangle(RawRectangleF rect, SharpDX.Direct2D1.Brush brush);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="clipRect"><para>The rectangle that defines the clip.</para></param>	
        /// <param name="antialiasMode"><para>Whether the given clip should be antialiased.</para></param>	
        /// <remarks>	
        /// If the current world transform is not preserving the axis, clipRectangle is transformed and the bounds of the transformed rectangle are used instead.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1CommandSink::PushAxisAlignedClip([In] const D2D_RECT_F* clipRect,[In] D2D1_ANTIALIAS_MODE antialiasMode)</unmanaged>	
        void PushAxisAlignedClip(RawRectangleF clipRect, SharpDX.Direct2D1.AntialiasMode antialiasMode);

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="layerParameters1">No documentation.</param>	
        /// <param name="layer">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1CommandSink::PushLayer([In] const D2D1_LAYER_PARAMETERS1* layerParameters1,[In, Optional] ID2D1Layer* layer)</unmanaged>	
        void PushLayer(ref SharpDX.Direct2D1.LayerParameters1 layerParameters1, SharpDX.Direct2D1.Layer layer);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::PopAxisAlignedClip()</unmanaged>	
        void PopAxisAlignedClip();

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::PopLayer()</unmanaged>	
        void PopLayer();
    }
}