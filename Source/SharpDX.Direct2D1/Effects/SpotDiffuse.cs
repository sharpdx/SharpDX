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
    /// Builtin SpotDiffuse effect.
    /// </summary>
    public class SpotDiffuse : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SpotDiffuse"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public SpotDiffuse(DeviceContext context) : base(context, Effect.SpotDiffuse)
        {
        }

        /// <summary>
        /// The light position of the point light source. The property is a <see cref="Vector3"/> defined as (x, y, z). The units are in device-independent pixels (DIPs) and the values are unitless and unbounded.
        /// </summary>
        public RawVector3 LightPosition
        {
            get
            {
                return GetVector3Value((int)SpotDiffuseProperties.LightPosition);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.LightPosition, value);
            }
        }

        /// <summary>
        /// Where the spot light is focused. The property is exposed as a <see cref="Vector3"/>  with – (x, y, z). The units are in DIPs and the values are unbounded.
        /// </summary>
        public RawVector3 PointsAt
        {
            get
            {
                return GetVector3Value((int)SpotDiffuseProperties.PointsAt);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.PointsAt, value);
            }
        }

        /// <summary>
        /// The focus of the spot light. This property is unitless and is defined between 0 and 200.
        /// </summary>
        public float Focus
        {
            get
            {
                return GetFloatValue((int)SpotDiffuseProperties.Focus);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.Focus, value);
            }
        }

        /// <summary>
        /// The cone angle that restricts the region where the light is projected. No light is projected outside the cone. The limiting cone angle is the angle between the spot light axis (the axis between the LightPosition and PointsAt properties) and the spot light cone. This property is defined in degrees and must be between 0 to 90 degrees.
        /// </summary>
        public float LimitingConeAngle
        {
            get
            {
                return GetFloatValue((int)SpotDiffuseProperties.LimitingConeAngle);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.LimitingConeAngle, value);
            }
        }

        /// <summary>
        /// The ratio of diffuse reflection to amount of incoming light. This property must be between 0 and 10,000 and is unitless. 
        /// </summary>
        public float DiffuseConstant
        {
            get
            {
                return GetFloatValue((int)SpotDiffuseProperties.DiffuseConstant);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.DiffuseConstant, value);
            }
        }

        /// <summary>
        /// The scale factor in the Z direction. The value is unitless and must be between 0 and 10,000.
        /// </summary>
        public float SurfaceScale
        {
            get
            {
                return GetFloatValue((int)SpotDiffuseProperties.SurfaceScale);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.SurfaceScale, value);
            }
        }

        /// <summary>
        /// The color of the incoming light. This property is exposed as a <see cref="Vector3"/> – (R, G, B) and used to compute LR, LG, LB. 
        /// </summary>
        public RawVector3 Color
        {
            get
            {
                return GetVector3Value((int)SpotDiffuseProperties.Color);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.Color, value);
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
                return GetVector2Value((int)SpotDiffuseProperties.KernelUnitLength);
            }
            set
            {
                SetValue((int)SpotDiffuseProperties.KernelUnitLength, value);
            }
        }

        /// <summary>
        /// The interpolation mode the effect uses to scale the image to the corresponding kernel unit length. 
        /// There are six scale modes that range in quality and speed. 
        /// If you don't select a mode, the effect uses the interpolation mode of the device context. 
        /// See Scale modes for more info.
        /// </summary>
        public SpotDiffuseScaleMode ScaleMode
        {
            get
            {
                return GetEnumValue<SpotDiffuseScaleMode>((int)SpotDiffuseProperties.ScaleMode);
            }
            set
            {
                SetEnumValue((int)SpotDiffuseProperties.ScaleMode, value);
            }
        }
    }
}