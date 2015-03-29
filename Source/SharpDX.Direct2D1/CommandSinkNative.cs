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
    internal partial class CommandSinkNative
    {
        /// <unmanaged>HRESULT ID2D1CommandSink::BeginDraw()</unmanaged>	
        public void BeginDraw()
        {
            BeginDraw_();
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::EndDraw()</unmanaged>	
        public void EndDraw()
        {
            EndDraw_();
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::SetAntialiasMode([In] D2D1_ANTIALIAS_MODE antialiasMode)</unmanaged>	
        public AntialiasMode AntialiasMode { set { SetAntialiasMode_(value); } }

        /// <unmanaged>HRESULT ID2D1CommandSink::SetTags([In] unsigned longlong tag1,[In] unsigned longlong tag2)</unmanaged>	
        public void SetTags(long tag1, long tag2)
        {
            SetTags_(tag1, tag2);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::SetTextAntialiasMode([In] D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode)</unmanaged>	
        public TextAntialiasMode TextAntialiasMode { set { SetTextAntialiasMode_(value); } }

        /// <unmanaged>HRESULT ID2D1CommandSink::SetTextRenderingParams([In, Optional] IDWriteRenderingParams* textRenderingParams)</unmanaged>	
        public DirectWrite.RenderingParams TextRenderingParams { set { SetTextRenderingParams_(value); } }

        /// <unmanaged>HRESULT ID2D1CommandSink::SetTransform([In] const D2D_MATRIX_3X2_F* transform)</unmanaged>	
        public RawMatrix3x2 Transform { set { SetTransform_(ref value); } }

        /// <unmanaged>HRESULT ID2D1CommandSink::SetPrimitiveBlend([In] D2D1_PRIMITIVE_BLEND primitiveBlend)</unmanaged>	
        public PrimitiveBlend PrimitiveBlend { set { SetPrimitiveBlend_(value); } }

        /// <unmanaged>HRESULT ID2D1CommandSink::SetUnitMode([In] D2D1_UNIT_MODE unitMode)</unmanaged>	
        public UnitMode UnitMode { set { SetUnitMode_(value); } }

        /// <unmanaged>HRESULT ID2D1CommandSink::Clear([In, Optional] const D2D_COLOR_F* color)</unmanaged>	
        public void Clear(RawColor4? color = null)
        {
            Clear_(color);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::DrawGlyphRun([In] D2D_POINT_2F baselineOrigin,[In] const DWRITE_GLYPH_RUN* glyphRun,[In, Optional] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[In] ID2D1Brush* foregroundBrush,[In] DWRITE_MEASURING_MODE measuringMode)</unmanaged>	
        public void DrawGlyphRun(RawVector2 baselineOrigin, DirectWrite.GlyphRun glyphRun, DirectWrite.GlyphRunDescription glyphRunDescription, Brush foregroundBrush, MeasuringMode measuringMode)
        {
            DrawGlyphRun_(baselineOrigin, glyphRun, glyphRunDescription, foregroundBrush, measuringMode);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::DrawLine([In] D2D_POINT_2F point0,[In] D2D_POINT_2F point1,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
        public void DrawLine(RawVector2 point0, RawVector2 point1, Brush brush, float strokeWidth, StrokeStyle strokeStyle)
        {
            DrawLine_(point0, point1, brush, strokeWidth, strokeStyle);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::DrawGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
        public void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth, StrokeStyle strokeStyle)
        {
            DrawGeometry_(geometry, brush, strokeWidth, strokeStyle);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::DrawRectangle([In] const D2D_RECT_F* rect,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
        public void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth, StrokeStyle strokeStyle)
        {
            DrawRectangle_(rect, brush, strokeWidth, strokeStyle);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D_RECT_F* destinationRectangle,[In] float opacity,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D_RECT_F* sourceRectangle,[In, Optional] const D2D_MATRIX_4X4_F* perspectiveTransform)</unmanaged>	
        public void DrawBitmap(Bitmap bitmap, RawRectangleF? destinationRectangle, float opacity, InterpolationMode interpolationMode, RawRectangleF? sourceRectangle, RawMatrix? erspectiveTransformRef)
        {
            DrawBitmap_(bitmap, destinationRectangle, opacity, interpolationMode, sourceRectangle, erspectiveTransformRef);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::DrawImage([In] ID2D1Image* image,[In, Optional] const D2D_POINT_2F* targetOffset,[In, Optional] const D2D_RECT_F* imageRectangle,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In] D2D1_COMPOSITE_MODE compositeMode)</unmanaged>	
        public void DrawImage(Image image, RawVector2? targetOffset, RawRectangleF? imageRectangle, InterpolationMode interpolationMode, CompositeMode compositeMode)
        {
            DrawImage_(image, targetOffset, imageRectangle, interpolationMode, compositeMode);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::DrawGdiMetafile([In] ID2D1GdiMetafile* gdiMetafile,[In, Optional] const D2D_POINT_2F* targetOffset)</unmanaged>	
        public void DrawGdiMetafile(GdiMetafile gdiMetafile, RawVector2? targetOffset)
        {
            DrawGdiMetafile_(gdiMetafile, targetOffset);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::FillMesh([In] ID2D1Mesh* mesh,[In] ID2D1Brush* brush)</unmanaged>	
        public void FillMesh(Mesh mesh, Brush brush)
        {
            FillMesh_(mesh, brush);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::FillOpacityMask([In] ID2D1Bitmap* opacityMask,[In] ID2D1Brush* brush,[In, Optional] const D2D_RECT_F* destinationRectangle,[In, Optional] const D2D_RECT_F* sourceRectangle)</unmanaged>	
        public void FillOpacityMask(Bitmap opacityMask, Brush brush, RawRectangleF? destinationRectangle, RawRectangleF? sourceRectangle)
        {
            FillOpacityMask_(opacityMask, brush, destinationRectangle, sourceRectangle);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::FillGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[In, Optional] ID2D1Brush* opacityBrush)</unmanaged>	
        public void FillGeometry(Geometry geometry, Brush brush, Brush opacityBrush)
        {
            FillGeometry_(geometry, brush, opacityBrush);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::FillRectangle([In] const D2D_RECT_F* rect,[In] ID2D1Brush* brush)</unmanaged>	
        public void FillRectangle(RawRectangleF rect, Brush brush)
        {
            FillRectangle_(rect, brush);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::PushAxisAlignedClip([In] const D2D_RECT_F* clipRect,[In] D2D1_ANTIALIAS_MODE antialiasMode)</unmanaged>	
        public void PushAxisAlignedClip(RawRectangleF clipRect, AntialiasMode antialiasMode)
        {
            PushAxisAlignedClip_(clipRect, antialiasMode);
        }

        /// <unmanaged>HRESULT ID2D1CommandSink::PushLayer([In] const D2D1_LAYER_PARAMETERS1* layerParameters1,[In, Optional] ID2D1Layer* layer)</unmanaged>	
        public void PushLayer(ref LayerParameters1 layerParameters1, Layer layer)
        {
            PushLayer_(ref layerParameters1, layer);
        }

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::PopAxisAlignedClip()</unmanaged>	
        public void PopAxisAlignedClip()
        {
            PopAxisAlignedClip_();
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1CommandSink::PopLayer()</unmanaged>	
        public void PopLayer()
        {
            PopLayer_();
        }
    }
}
