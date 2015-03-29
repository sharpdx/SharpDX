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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    public partial class DeviceContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext"/> class.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <unmanaged>HRESULT D2D1CreateDeviceContext([In] IDXGISurface* dxgiSurface,[In, Optional] const D2D1_CREATION_PROPERTIES* creationProperties,[Out] ID2D1DeviceContext** d2dDeviceContext)</unmanaged>	
        public DeviceContext(SharpDX.DXGI.Surface surface)
            : base(IntPtr.Zero)
        {
            D2D1.CreateDeviceContext(surface, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="creationProperties">The creation properties.</param>
        /// <unmanaged>HRESULT D2D1CreateDeviceContext([In] IDXGISurface* dxgiSurface,[In, Optional] const D2D1_CREATION_PROPERTIES* creationProperties,[Out] ID2D1DeviceContext** d2dDeviceContext)</unmanaged>	
        public DeviceContext(SharpDX.DXGI.Surface surface, CreationProperties creationProperties)
            : base(IntPtr.Zero)
        {
            D2D1.CreateDeviceContext(surface, creationProperties, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext"/> class using an existing <see cref="Device"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="options">The options to be applied to the created device context.</param>
        /// <remarks>
        /// The new device context will not have a  selected target bitmap. The caller must create and select a bitmap as the target surface of the context.
        /// </remarks>
        /// <unmanaged>HRESULT ID2D1Device::CreateDeviceContext([In] D2D1_DEVICE_CONTEXT_OPTIONS options,[Out] ID2D1DeviceContext** deviceContext)</unmanaged>
        public DeviceContext(Device device, DeviceContextOptions options)
            : base(IntPtr.Zero)
        {
            device.CreateDeviceContext(options, this);
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="effect">No documentation.</param>	
        /// <param name="targetOffset">No documentation.</param>	
        /// <param name="interpolationMode">No documentation.</param>	
        /// <param name="compositeMode">No documentation.</param>	
        /// <unmanaged>void ID2D1DeviceContext::DrawImage([In] ID2D1Image* image,[In, Optional] const D2D_POINT_2F* targetOffset,[In, Optional] const D2D_RECT_F* imageRectangle,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In] D2D1_COMPOSITE_MODE compositeMode)</unmanaged>	
        public void DrawImage(SharpDX.Direct2D1.Effect effect, RawVector2 targetOffset, SharpDX.Direct2D1.InterpolationMode interpolationMode = InterpolationMode.Linear, SharpDX.Direct2D1.CompositeMode compositeMode = CompositeMode.SourceOver)
        {
            using (var output = effect.Output)
                DrawImage(output, targetOffset, null, interpolationMode, compositeMode);
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name="effect">No documentation.</param>
        /// <param name="interpolationMode">No documentation.</param>
        /// <param name="compositeMode">No documentation.</param>
        /// <unmanaged>void ID2D1DeviceContext::DrawImage([In] ID2D1Image* image,[In, Optional] const D2D_POINT_2F* targetOffset,[In, Optional] const D2D_RECT_F* imageRectangle,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In] D2D1_COMPOSITE_MODE compositeMode)</unmanaged>
        public void DrawImage(SharpDX.Direct2D1.Effect effect, SharpDX.Direct2D1.InterpolationMode interpolationMode = InterpolationMode.Linear, SharpDX.Direct2D1.CompositeMode compositeMode = CompositeMode.SourceOver)
        {
            using (var output = effect.Output)
                DrawImage(output, null, null, interpolationMode, compositeMode);
        }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="image">No documentation.</param>	
        /// <param name="targetOffset">No documentation.</param>	
        /// <param name="interpolationMode">No documentation.</param>	
        /// <param name="compositeMode">No documentation.</param>	
        /// <unmanaged>void ID2D1DeviceContext::DrawImage([In] ID2D1Image* image,[In, Optional] const D2D_POINT_2F* targetOffset,[In, Optional] const D2D_RECT_F* imageRectangle,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In] D2D1_COMPOSITE_MODE compositeMode)</unmanaged>	
        public void DrawImage(SharpDX.Direct2D1.Image image, RawVector2 targetOffset, SharpDX.Direct2D1.InterpolationMode interpolationMode = InterpolationMode.Linear, SharpDX.Direct2D1.CompositeMode compositeMode = CompositeMode.SourceOver)
        {
            DrawImage(image, targetOffset, null, interpolationMode, compositeMode);
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name="image">No documentation.</param>
        /// <param name="interpolationMode">No documentation.</param>
        /// <param name="compositeMode">No documentation.</param>
        /// <unmanaged>void ID2D1DeviceContext::DrawImage([In] ID2D1Image* image,[In, Optional] const D2D_POINT_2F* targetOffset,[In, Optional] const D2D_RECT_F* imageRectangle,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In] D2D1_COMPOSITE_MODE compositeMode)</unmanaged>
        public void DrawImage(SharpDX.Direct2D1.Image image, SharpDX.Direct2D1.InterpolationMode interpolationMode = InterpolationMode.Linear, SharpDX.Direct2D1.CompositeMode compositeMode = CompositeMode.SourceOver)
        {
            DrawImage(image, null, null, interpolationMode, compositeMode);
        }

        /// <summary>
        /// Draws the bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolationMode">The interpolation mode.</param>
        /// <unmanaged>void ID2D1DeviceContext::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D_RECT_F* destinationRectangle,[In] float opacity,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D_RECT_F* sourceRectangle,[In, Optional] const D2D_MATRIX_4X4_F* perspectiveTransform)</unmanaged>
        public void DrawBitmap(SharpDX.Direct2D1.Bitmap bitmap, float opacity, SharpDX.Direct2D1.InterpolationMode interpolationMode)
        {
            DrawBitmap(bitmap, null, opacity, interpolationMode, null, null);
        }

        /// <summary>
        /// Draws the bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolationMode">The interpolation mode.</param>
        /// <param name="perspectiveTransformRef">The perspective transform ref.</param>
        /// <unmanaged>void ID2D1DeviceContext::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D_RECT_F* destinationRectangle,[In] float opacity,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D_RECT_F* sourceRectangle,[In, Optional] const D2D_MATRIX_4X4_F* perspectiveTransform)</unmanaged>
        public void DrawBitmap(SharpDX.Direct2D1.Bitmap bitmap, float opacity, SharpDX.Direct2D1.InterpolationMode interpolationMode, RawMatrix perspectiveTransformRef)
        {
            DrawBitmap(bitmap, null, opacity, interpolationMode, null, perspectiveTransformRef);
        }

        /// <summary>
        /// Draws the bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolationMode">The interpolation mode.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="perspectiveTransformRef">The perspective transform ref.</param>
        /// <unmanaged>void ID2D1DeviceContext::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D_RECT_F* destinationRectangle,[In] float opacity,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D_RECT_F* sourceRectangle,[In, Optional] const D2D_MATRIX_4X4_F* perspectiveTransform)</unmanaged>
        public void DrawBitmap(SharpDX.Direct2D1.Bitmap bitmap, float opacity, SharpDX.Direct2D1.InterpolationMode interpolationMode, RawRectangleF sourceRectangle, RawMatrix perspectiveTransformRef)
        {
            DrawBitmap(bitmap, null, opacity, interpolationMode, sourceRectangle, perspectiveTransformRef);
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="layerParameters">No documentation.</param>	
        /// <param name="layer">No documentation.</param>	
        /// <unmanaged>void ID2D1DeviceContext::PushLayer([In] const D2D1_LAYER_PARAMETERS1* layerParameters,[In, Optional] ID2D1Layer* layer)</unmanaged>	
        public void PushLayer(SharpDX.Direct2D1.LayerParameters1 layerParameters, SharpDX.Direct2D1.Layer layer)
        {
            PushLayer(ref layerParameters, layer);
        }

        /// <summary>
        /// Gets the effect invalid rectangles.
        /// </summary>
        /// <param name="effect">The effect.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID2D1DeviceContext::GetEffectInvalidRectangles([In] ID2D1Effect* effect,[Out, Buffer] D2D_RECT_F* rectangles,[In] unsigned int rectanglesCount)</unmanaged>
        public RawRectangleF[] GetEffectInvalidRectangles(SharpDX.Direct2D1.Effect effect)
        {
            var invalidRects = new RawRectangleF[GetEffectInvalidRectangleCount(effect)];
            if (invalidRects.Length == 0)
                return invalidRects;
            GetEffectInvalidRectangles(effect, invalidRects, invalidRects.Length);
            return invalidRects;
        }

        /// <summary>
        /// Gets the effect required input rectangles.
        /// </summary>
        /// <param name="renderEffect">The render effect.</param>
        /// <param name="inputDescriptions">The input descriptions.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID2D1DeviceContext::GetEffectRequiredInputRectangles([In] ID2D1Effect* renderEffect,[In, Optional] const D2D_RECT_F* renderImageRectangle,[In, Buffer] const D2D1_EFFECT_INPUT_DESCRIPTION* inputDescriptions,[Out, Buffer] D2D_RECT_F* requiredInputRects,[In] unsigned int inputCount)</unmanaged>
        public RawRectangleF[] GetEffectRequiredInputRectangles(SharpDX.Direct2D1.Effect renderEffect, SharpDX.Direct2D1.EffectInputDescription[] inputDescriptions)
        {
            var result = new RawRectangleF[inputDescriptions.Length];
            GetEffectRequiredInputRectangles(renderEffect, null, inputDescriptions, result, inputDescriptions.Length);
            return result;
        }

        /// <summary>
        /// Gets the effect required input rectangles.
        /// </summary>
        /// <param name="renderEffect">The render effect.</param>
        /// <param name="renderImageRectangle">The render image rectangle.</param>
        /// <param name="inputDescriptions">The input descriptions.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID2D1DeviceContext::GetEffectRequiredInputRectangles([In] ID2D1Effect* renderEffect,[In, Optional] const D2D_RECT_F* renderImageRectangle,[In, Buffer] const D2D1_EFFECT_INPUT_DESCRIPTION* inputDescriptions,[Out, Buffer] D2D_RECT_F* requiredInputRects,[In] unsigned int inputCount)</unmanaged>
        public RawRectangleF[] GetEffectRequiredInputRectangles(SharpDX.Direct2D1.Effect renderEffect, RawRectangleF renderImageRectangle, SharpDX.Direct2D1.EffectInputDescription[] inputDescriptions)
        {
            var result = new RawRectangleF[inputDescriptions.Length];
            GetEffectRequiredInputRectangles(renderEffect, renderImageRectangle, inputDescriptions, result, inputDescriptions.Length);
            return result;
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="opacityMask">No documentation.</param>	
        /// <param name="brush">No documentation.</param>	
        /// <unmanaged>void ID2D1DeviceContext::FillOpacityMask([In] ID2D1Bitmap* opacityMask,[In] ID2D1Brush* brush,[In, Optional] const D2D_RECT_F* destinationRectangle,[In, Optional] const D2D_RECT_F* sourceRectangle)</unmanaged>	
        public void FillOpacityMask(SharpDX.Direct2D1.Bitmap opacityMask, SharpDX.Direct2D1.Brush brush)
        {
            FillOpacityMask(opacityMask, brush, null, null);
        }
    }
}