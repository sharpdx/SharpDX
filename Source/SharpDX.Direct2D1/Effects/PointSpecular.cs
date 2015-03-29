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
    /// Built in PointSpecular effect.
    /// </summary>
    public class PointSpecular : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PointSpecular"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public PointSpecular(DeviceContext context) : base(context, Effect.PointSpecular)
        {
        }

        /// <summary>
        /// The light position of the point light source. The property is a D2D1_VECTOR_3F defined as (x, y, z). The units are in device-independent pixels (DIPs) and the values are unitless and unbounded.
        /// </summary>
        public RawVector3 LightPosition
        {
            get
            {
                return GetVector3Value((int)PointSpecularProperties.LightPosition);
            }
            set
            {
                SetValue((int)PointSpecularProperties.LightPosition, value);
            }
        }

        /// <summary>
        /// The exponent for the specular term in the Phong lighting equation. A larger value corresponds to a more reflective surface. The value is unitless and must be between 1.0 and 128. 
        /// </summary>
        public float SpecularExponent
        {
            get
            {
                return GetFloatValue((int)PointSpecularProperties.SpecularExponent);
            }
            set
            {
                SetValue((int)PointSpecularProperties.SpecularExponent, value);
            }
        }

        /// <summary>
        /// The ratio of specular reflection to the incoming light. The value is unitless and must be between 0 and 10,000.
        /// </summary>
        public float SpecularConstant
        {
            get
            {
                return GetFloatValue((int)PointSpecularProperties.SpecularConstant);
            }
            set
            {
                SetValue((int)PointSpecularProperties.SpecularConstant, value);
            }
        }

        /// <summary>
        /// The scale factor in the Z direction. The value is unitless and must be between 0 and 10,000.
        /// </summary>
        public float SurfaceScale
        {
            get
            {
                return GetFloatValue((int)PointSpecularProperties.SurfaceScale);
            }
            set
            {
                SetValue((int)PointSpecularProperties.SurfaceScale, value);
            }
        }

        /// <summary>
        /// The color of the incoming light. This property is exposed as a <see cref="Vector3"/> – (R, G, B) and used to compute LR, LG, LB. 
        /// </summary>
        public RawVector3 Color
        {
            get
            {
                return GetVector3Value((int)PointSpecularProperties.Color);
            }
            set
            {
                SetValue((int)PointSpecularProperties.Color, value);
            }
        }

        /// <summary>
        /// The size of an element in the Sobel kernel used to generate the surface normal in the X and Y direction. 
        /// This property maps to the dx and dy values in the Sobel gradient. 
        /// This property is a <see cref="Vector2"/> (Kernel Unit Length X, Kernel Unit Length Y) and is defined in (device-independent pixels (DIPs)/Kernel Unit). 
        /// The effect uses bilinear interpolation to scale the bitmap to match size of kernel elements.
        /// </summary>
        public RawVector2 KernelUnitLength
        {
            get
            {
                return GetVector2Value((int)PointSpecularProperties.KernelUnitLength);
            }
            set
            {
                SetValue((int)PointSpecularProperties.KernelUnitLength, value);
            }
        }

        /// <summary>
        /// The interpolation mode the effect uses to scale the image to the corresponding kernel unit length. There are six scale modes that range in quality and speed. If you don't select a mode, the effect uses the interpolation mode of the device context. See Scale modes for more info.
        /// </summary>
        public PointSpecularScaleMode ScaleMode
        {
            get
            {
                return GetEnumValue<PointSpecularScaleMode>((int)PointSpecularProperties.ScaleMode);
            }
            set
            {
                SetEnumValue((int)PointSpecularProperties.ScaleMode, value);
            }
        }
    }
}