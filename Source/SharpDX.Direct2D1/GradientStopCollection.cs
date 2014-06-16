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
    public partial class GradientStopCollection
    {
        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.GradientStopCollection"/> from the specified gradient stops, a Gamma StandardRgb, and ExtendMode.Clamp.  	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="gradientStops">A pointer to an array of D2D1_GRADIENT_STOP structures.</param>
        /// <unmanaged>HRESULT CreateGradientStopCollection([In, Buffer] const D2D1_GRADIENT_STOP* gradientStops,[None] UINT gradientStopsCount,[None] D2D1_GAMMA colorInterpolationGamma,[None] D2D1_EXTEND_MODE extendMode,[Out] ID2D1GradientStopCollection** gradientStopCollection)</unmanaged>
        public GradientStopCollection(RenderTarget renderTarget, SharpDX.Direct2D1.GradientStop[] gradientStops) : this(renderTarget, gradientStops, Gamma.StandardRgb, Direct2D1.ExtendMode.Clamp)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.GradientStopCollection"/> from the specified gradient stops, color Gamma.StandardRgb, and extend mode.  	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="gradientStops">A pointer to an array of D2D1_GRADIENT_STOP structures.</param>
        /// <param name="extendMode">The behavior of the gradient outside the [0,1] normalized range.</param>
        /// <unmanaged>HRESULT CreateGradientStopCollection([In, Buffer] const D2D1_GRADIENT_STOP* gradientStops,[None] UINT gradientStopsCount,[None] D2D1_GAMMA colorInterpolationGamma,[None] D2D1_EXTEND_MODE extendMode,[Out] ID2D1GradientStopCollection** gradientStopCollection)</unmanaged>
        public GradientStopCollection(RenderTarget renderTarget, SharpDX.Direct2D1.GradientStop[] gradientStops, SharpDX.Direct2D1.ExtendMode extendMode)
            : this(renderTarget, gradientStops, Gamma.StandardRgb, extendMode)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.GradientStopCollection"/> from the specified gradient stops, color interpolation gamma, and ExtendMode.Clamp.  	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="gradientStops">A pointer to an array of D2D1_GRADIENT_STOP structures.</param>
        /// <param name="colorInterpolationGamma">The space in which color interpolation between the gradient stops is performed.</param>
        /// <unmanaged>HRESULT CreateGradientStopCollection([In, Buffer] const D2D1_GRADIENT_STOP* gradientStops,[None] UINT gradientStopsCount,[None] D2D1_GAMMA colorInterpolationGamma,[None] D2D1_EXTEND_MODE extendMode,[Out] ID2D1GradientStopCollection** gradientStopCollection)</unmanaged>
        public GradientStopCollection(RenderTarget renderTarget, SharpDX.Direct2D1.GradientStop[] gradientStops, SharpDX.Direct2D1.Gamma colorInterpolationGamma)
            : this(renderTarget, gradientStops, colorInterpolationGamma, Direct2D1.ExtendMode.Clamp)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.GradientStopCollection"/> from the specified gradient stops, color interpolation gamma, and extend mode.  	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="gradientStops">A pointer to an array of D2D1_GRADIENT_STOP structures.</param>
        /// <param name="colorInterpolationGamma">The space in which color interpolation between the gradient stops is performed.</param>
        /// <param name="extendMode">The behavior of the gradient outside the [0,1] normalized range.</param>
        /// <unmanaged>HRESULT CreateGradientStopCollection([In, Buffer] const D2D1_GRADIENT_STOP* gradientStops,[None] UINT gradientStopsCount,[None] D2D1_GAMMA colorInterpolationGamma,[None] D2D1_EXTEND_MODE extendMode,[Out] ID2D1GradientStopCollection** gradientStopCollection)</unmanaged>
        public GradientStopCollection(RenderTarget renderTarget, SharpDX.Direct2D1.GradientStop[] gradientStops, SharpDX.Direct2D1.Gamma colorInterpolationGamma, SharpDX.Direct2D1.ExtendMode extendMode) : base(IntPtr.Zero)
        {
            renderTarget.CreateGradientStopCollection(gradientStops, gradientStops.Length, colorInterpolationGamma, extendMode, this);
        }
    }
}
