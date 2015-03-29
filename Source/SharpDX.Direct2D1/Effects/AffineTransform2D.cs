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
    /// Built in AffineTransform2D effect.
    /// </summary>
    public class AffineTransform2D : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AffineTransform2D"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public AffineTransform2D(DeviceContext context) : base(context, Effect.AffineTransform2D)
        {
        }

        /// <summary>
        /// The interpolation mode used to scale the image. There are 6 scale modes that range in quality and speed. 
        /// If you don't select a mode, the effect uses the interpolation mode of the device context. 
        /// See <see cref="InterpolationMode"/> for more info.
        /// </summary>
        public InterpolationMode InterpolationMode
        {
            get
            {
                return GetEnumValue<InterpolationMode>((int)AffineTransform2DProperties.InterpolationMode);
            }
            set
            {
                SetEnumValue((int)AffineTransform2DProperties.InterpolationMode, value);
            }
        }

        /// <summary>
        /// The mode used to calculate the border of the image, soft or hard. See <see cref="BorderMode"/> modes for more info.
        /// </summary>
        public BorderMode BorderMode
        {
            get
            {
                return GetEnumValue<BorderMode>((int)AffineTransform2DProperties.BorderMode);
            }
            set
            {
                SetEnumValue((int)AffineTransform2DProperties.BorderMode, value);
            }
        }

        /// <summary>
        /// The 3x2 matrix to transform the image using the Direct2D matrix transform. 
        /// </summary>
        public RawMatrix3x2 TransformMatrix
        {
            get
            {
                return GetMatrix3x2Value((int)AffineTransform2DProperties.TransformMatrix);
            }
            set
            {
                SetValue((int)AffineTransform2DProperties.TransformMatrix, value);
            }
        }

        /// <summary>
        /// In the high quality cubic interpolation mode, the sharpness level of the scaling filter as a float between 0 and 1. 
        /// The values are unitless. You can use sharpness to adjust the quality of an image when you scale the image.
        /// The sharpness factor affects the shape of the kernel. The higher the sharpness factor, the smaller the kernel.
        /// </summary>
        /// <remarks>
        /// This property affects only the high quality cubic interpolation mode.
        /// </remarks>
        public float Sharpness
        {
            get
            {
                return GetFloatValue((int)AffineTransform2DProperties.Sharpness);
            }
            set
            {
                SetValue((int)AffineTransform2DProperties.Sharpness, value);
            }
        }
    }
}