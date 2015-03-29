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

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in ArithmeticComposite effect.
    /// </summary>
    public class ArithmeticComposite : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ArithmeticComposite"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public ArithmeticComposite(DeviceContext context) : base(context, Effect.ArithmeticComposite)
        {
        }

        /// <summary>
        /// The coefficients for the equation used to composite the two input images. The coefficients are unitless and unbounded.
        /// </summary>
        public RawVector4 Coefficients
        {
            get
            {
                return GetVector4Value((int)ArithmeticCompositeProperties.Coefficients);
            }
            set
            {
                SetValue((int)ArithmeticCompositeProperties.Coefficients, value);
            }
        }

        /// <summary>
        /// Whether the effect clamps color values to between 0 and 1 before the effect passes the values to the next effect in the graph. The effect clamps the values before it premultiplies the alpha.
        /// if you set this to TRUE the effect will clamp the values. If you set this to FALSE, the effect will not clamp the color values, but other effects and the output surface may clamp the values if they are not of high enough precision.
        /// </summary>
        public bool ClampOutput
        {
            get
            {
                return GetBoolValue((int)ArithmeticCompositeProperties.ClampOutput);
            }
            set
            {
                SetValue((int)ArithmeticCompositeProperties.ClampOutput, value);
            }
        }
    }
}