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
    /// Built in Scale effect.
    /// </summary>
    public class Scale : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Scale"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Scale(DeviceContext context) : base(context, Effect.Scale)
        {
        }

        /// <summary>
        /// The scale amount in the X and Y direction as a ratio of the output size to the input size. This property a <see cref="Vector2"/> defined as: (X scale, Y scale). The scale amounts are FLOAT, unitless, and must be positive or 0.
        /// </summary>
        public RawVector2 ScaleAmount
        {
            get
            {
                return GetVector2Value((int)ScaleProperties.Scale);
            }
            set
            {
                SetValue((int)ScaleProperties.Scale, value);
            }
        }

        /// <summary>
        /// The image scaling center point. This property is a <see cref="Vector2"/> defined as: (point X, point Y). The units are in DIPs.
        /// Use the center point property to scale around a point other than the upper-left corner.
        /// </summary>
        public RawVector2 CenterPoint
        {
            get
            {
                return GetVector2Value((int)ScaleProperties.CenterPoint);
            }
            set
            {
                SetValue((int)ScaleProperties.CenterPoint, value);
            }
        }

        /// <summary>
        /// The mode used to calculate the border of the image, soft or hard. See Border modes for more info.
        /// </summary>
        public BorderMode BorderMode
        {
            get
            {
                return GetEnumValue<BorderMode>((int)ScaleProperties.BorderMode);
            }
            set
            {
                SetEnumValue((int)ScaleProperties.BorderMode, value);
            }
        }

        /// <summary>
        /// In the high quality cubic interpolation mode, the sharpness level of the scaling filter as a float between 0 and 1. The values are unitless. You can use sharpness to adjust the quality of an image when you scale the image down.
        /// The sharpness factor affects the shape of the kernel. The higher the sharpness factor, the smaller the kernel.
        /// </summary>
        /// <remarks>
        /// This property affects only the high quality cubic interpolation mode.
        /// </remarks>
        public float Sharpness
        {
            get
            {
                return GetFloatValue((int)ScaleProperties.Sharpness);
            }
            set
            {
                SetValue((int)ScaleProperties.Sharpness, value);
            }
        }

        /// <summary>
        /// The interpolation mode the effect uses to scale the image. 
        /// There are 6 scale modes that range in quality and speed. 
        /// If you don't select a mode, the effect uses the interpolation mode of the device context. See Interpolation modes for more info.
        /// </summary>
        public InterpolationMode InterpolationMode
        {
            get
            {
                return GetEnumValue<InterpolationMode>((int)ScaleProperties.InterpolationMode);
            }
            set
            {
                SetEnumValue((int)ScaleProperties.InterpolationMode, value);
            }
        }
    }
}