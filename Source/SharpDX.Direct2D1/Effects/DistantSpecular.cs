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
    /// Built in DistantSpecular effect.
    /// </summary>
    public class DistantSpecular : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DistantSpecular"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public DistantSpecular(DeviceContext context) : base(context, Effect.DistantSpecular)
        {
        }

        /// <summary>
        /// The direction angle of the light source in the XY plane relative to the X-axis in the counter clock wise direction. The units are in degrees and must be between 0 and 360 degrees.
        /// </summary>
        public float Azimuth
        {
            get
            {
                return GetFloatValue((int)DistantSpecularProperties.Azimuth);
            }
            set
            {
                SetValue((int)DistantSpecularProperties.Azimuth, value);
            }
        }

        /// <summary>
        /// The direction angle of the light source in the YZ plane relative to the Y-axis in the counter clock wise direction. The units are in degrees and must be between 0 and 360 degrees.
        /// </summary>
        public float Elevation
        {
            get
            {
                return GetFloatValue((int)DistantSpecularProperties.Elevation);
            }
            set
            {
                SetValue((int)DistantSpecularProperties.Elevation, value);
            }
        }

        /// <summary>
        /// The exponent for the specular term in the Phong lighting equation. A larger value corresponds to a more reflective surface. The value is unitless and must be between 1.0 and 128. 
        /// </summary>
        public float SpecularExponent
        {
            get
            {
                return GetFloatValue((int)DistantSpecularProperties.SpecularExponent);
            }
            set
            {
                SetValue((int)DistantSpecularProperties.SpecularExponent, value);
            }
        }

        /// <summary>
        /// The ratio of specular reflection to the incoming light. The value is unitless and must be between 0 and 10,000.
        /// </summary>
        public float SpecularConstant
        {
            get
            {
                return GetFloatValue((int)DistantSpecularProperties.SpecularConstant);
            }
            set
            {
                SetValue((int)DistantSpecularProperties.SpecularConstant, value);
            }
        }

        /// <summary>
        /// The scale factor in the Z direction. The value is unitless and must be between 0 and 10,000.
        /// </summary>
        public float SurfaceScale
        {
            get
            {
                return GetFloatValue((int)DistantSpecularProperties.SurfaceScale);
            }
            set
            {
                SetValue((int)DistantSpecularProperties.SurfaceScale, value);
            }
        }

        /// <summary>
        /// The color of the incoming light. This property is exposed as a <see cref="Vector3"/> – (R, G, B) and used to compute LR, LG, LB. 
        /// </summary>
        public RawVector3 Color
        {
            get
            {
                return GetVector3Value((int)DistantSpecularProperties.Color);
            }
            set
            {
                SetValue((int)DistantSpecularProperties.Color, value);
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
                return GetVector2Value((int)DistantSpecularProperties.KernelUnitLength);
            }
            set
            {
                SetValue((int)DistantSpecularProperties.KernelUnitLength, value);
            }
        }

        /// <summary>
        /// The interpolation mode the effect uses to scale the image to the corresponding kernel unit length. There are six scale modes that range in quality and speed. If you don't select a mode, the effect uses the interpolation mode of the device context. See Scale modes for more info.
        /// </summary>
        public DistantSpecularScaleMode ScaleMode
        {
            get
            {
                return GetEnumValue<DistantSpecularScaleMode>((int)DistantSpecularProperties.ScaleMode);
            }
            set
            {
                SetEnumValue((int)DistantSpecularProperties.ScaleMode, value);
            }
        }
    }
}