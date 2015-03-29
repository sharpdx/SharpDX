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

namespace SharpDX.Direct2D1
{
    public partial class GradientStopCollection1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GradientStopCollection1"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="straightAlphaGradientStops">An array of color values and offsets.</param>
        /// <param name="preInterpolationSpace">Specifies both the input color space and the space in which the color interpolation occurs.</param>
        /// <param name="postInterpolationSpace">The color space that colors will be converted to after interpolation occurs.</param>
        /// <param name="bufferPrecision">The precision of the texture used to hold interpolated values.</param>
        /// <param name="extendMode">Defines how colors outside of the range defined by the stop collection are determined.</param>
        /// <param name="colorInterpolationMode">The new gradient stop collection.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateGradientStopCollection([In, Buffer] const D2D1_GRADIENT_STOP* straightAlphaGradientStops,[In] unsigned int straightAlphaGradientStopsCount,[In] D2D1_COLOR_SPACE preInterpolationSpace,[In] D2D1_COLOR_SPACE postInterpolationSpace,[In] D2D1_BUFFER_PRECISION bufferPrecision,[In] D2D1_EXTEND_MODE extendMode,[In] D2D1_COLOR_INTERPOLATION_MODE colorInterpolationMode,[Out, Fast] ID2D1GradientStopCollection1** gradientStopCollection1)</unmanaged>
        /// <remarks>
        /// This method linearly interpolates between the color stops. An optional color space conversion is applied after interpolation. Whether and how this gamma conversion is applied is determined before and after interpolation. This method will fail if the device context does not support the requested buffer precision.Additional ReferencesD2D1_GRADIENT_STOP, D2D1_GAMMA_CONVERSION, <see cref="SharpDX.Direct2D1.BufferPrecision"/>, <see cref="SharpDX.Direct2D1.ExtendMode"/>, <see cref="SharpDX.Direct2D1.GradientStopCollection"/>RequirementsMinimum supported operating systemSame as Interface / Class Highest IRQL levelN/A (user mode) Callable from DlllMain()No Callable from services and session 0Yes Callable from UI threadYes?
        /// </remarks>
        public GradientStopCollection1(DeviceContext context, SharpDX.Direct2D1.GradientStop[] straightAlphaGradientStops, SharpDX.Direct2D1.ColorSpace preInterpolationSpace, SharpDX.Direct2D1.ColorSpace postInterpolationSpace, SharpDX.Direct2D1.BufferPrecision bufferPrecision, SharpDX.Direct2D1.ExtendMode extendMode, SharpDX.Direct2D1.ColorInterpolationMode colorInterpolationMode)
            : base(IntPtr.Zero)
        {
            context.CreateGradientStopCollection(straightAlphaGradientStops, straightAlphaGradientStops.Length, preInterpolationSpace, postInterpolationSpace, bufferPrecision, extendMode, colorInterpolationMode, this);

        }
    }
}