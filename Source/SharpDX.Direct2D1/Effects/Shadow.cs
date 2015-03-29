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
    /// Built in Shadow effect.
    /// </summary>
    public class Shadow : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Shadow"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Shadow(DeviceContext context) : base(context, Effect.Shadow)
        {
        }

        /// <summary>
        /// The amount of blur to be applied to the alpha channel of the image. You can compute the blur radius of the kernel by multiplying the standard deviation by 3. The units of both the standard deviation and blur radius are DIPs.
        /// This property is the same as the Gaussian Blur standard deviation property.
        /// </summary>
        public float BlurStandardDeviation
        {
            get
            {
                return GetFloatValue((int)ShadowProperties.BlurStandardDeviation);
            }
            set
            {
                SetValue((int)ShadowProperties.BlurStandardDeviation, value);
            }
        }
        
        /// <summary>
        /// The color of the drop shadow. This property is a <see cref="RawColor4"/> defined as: (R, G, B, A). 
        /// </summary>
        public RawColor4 Color
        {
            get
            {
                return GetColor4Value((int)ShadowProperties.Color);
            }
            set
            {
                SetValue((int)ShadowProperties.Color, value);
            }
        }

        /// <summary>
        /// The level of performance optimization.
        /// </summary>
        public ShadowOptimization Optimization
        {
            get
            {
                return GetEnumValue<ShadowOptimization>((int)ShadowProperties.Optimization);
            }
            set
            {
                SetEnumValue((int)ShadowProperties.Optimization, value);
            }
        }
    }
}