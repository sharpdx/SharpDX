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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in DirectionalBlur effect.
    /// </summary>
    public class DirectionalBlur : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DirectionalBlur"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public DirectionalBlur(DeviceContext context) : base(context, Effect.DirectionalBlur)
        {
        }

        /// <summary>
        /// Gets or sets the amount of blur to be applied to the image. Default: 1.0f
        /// </summary>
        /// <remarks>
        /// You can compute the blur radius of the kernel by multiplying the standard deviation by 3. The units of both the standard deviation and blur radius are DIPs. A value of zero DIPs disables this effect entirely. 
        /// </remarks>
        public float StandardDeviation
        {
            get
            {
                return GetFloatValue((int)DirectionalBlurProperties.StandardDeviation);
            }
            set
            {
                SetValue((int)DirectionalBlurProperties.StandardDeviation, value);
            }
        }


        /// <summary>
        /// The angle of the blur relative to the x-axis, in the counterclockwise direction. The units are specified in degrees.
        /// The blur kernel is first generated using the same process as for the Gaussian Blur effect. The kernel values are then transformed according to the blur angle using this equation and then applied to the bitmap.
        /// offset2D – amount of transformation introduced in the blur kernel as a result of the blur angle.
        /// dist – distance from the center of the kernel to the current position in the kernel. offset2d = (dist * cos(⁡θ), dist * sin(⁡θ) ) 
        /// </summary>
        /// <remarks>
        /// You can compute the blur radius of the kernel by multiplying the standard deviation by 3. The units of both the standard deviation and blur radius are DIPs. A value of zero DIPs disables this effect entirely. 
        /// </remarks>
        public float Angle
        {
            get
            {
                return GetFloatValue((int)DirectionalBlurProperties.Angle);
            }
            set
            {
                SetValue((int)DirectionalBlurProperties.Angle, value);
            }
        }

        /// <summary>
        /// The optimization mode. See <see cref="DirectionalBlurOptimization"/> modes for more info.
        /// </summary>
        /// <remarks>
        /// Default value is <see cref="DirectionalBlurOptimization.Balanced"/>.
        /// </remarks>
        public DirectionalBlurOptimization Optimization
        {
            get
            {
                return GetEnumValue<DirectionalBlurOptimization>((int)DirectionalBlurProperties.Optimization);
            }
            set
            {
                SetEnumValue((int)DirectionalBlurProperties.Optimization, value);
            }
        }

        /// <summary>
        /// The mode used to calculate the border of the image, soft or hard. See <see cref="BorderMode"/> modes for more info.
        /// </summary>
        public BorderMode BorderMode
        {
            get
            {
                return GetEnumValue<BorderMode>((int)DirectionalBlurProperties.BorderMode);
            }
            set
            {
                SetEnumValue((int)DirectionalBlurProperties.BorderMode, value);
            }
        }
    }
}