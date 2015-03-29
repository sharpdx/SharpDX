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
    /// Built in Turbulence effect.
    /// </summary>
    public class Turbulence : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Turbulence"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Turbulence(DeviceContext context) : base(context, Effect.Turbulence)
        {
        }
       
        /// <summary>
        /// The coordinates where the turbulence output is generated.
        /// The algorithm used to generate the Perlin noise is position dependent, so a different offset results in a different output. This property is not bounded and the units are specified in DIPs
        /// </summary>
        /// <remarks>
        /// The offset does not have the same effect as a translation because the noise function output is infinite and the function will wrap around the tile.
        /// </remarks>
        public RawVector2 Offset
        {
            get
            {
                return GetVector2Value((int)TurbulenceProperties.Offset);
            }
            set
            {
                SetValue((int)TurbulenceProperties.Offset, value);
            }
        }

        /// <summary>
        /// The base frequencies in the X and Y direction.. This property is a float and must be greater than 0. The units are specified in 1/DIPs.
        /// A value of 1 (1/DIPs) for the base frequency results in the Perlin noise completing an entire cycle between two pixels. The ease interpolation for these pixels results in completely random pixels, since there is no correlation between the pixels.
        /// A value of 0.1(1/DIPs) for the base frequency, the Perlin noise function repeats every 10 DIPs. This results in correlation between pixels and the typical turbulence effect is visible
        /// </summary>
        public RawVector2 BaseFrequency
        {
            get
            {
                return GetVector2Value((int)TurbulenceProperties.BaseFrequency);
            }
            set
            {
                SetValue((int)TurbulenceProperties.BaseFrequency, value);
            }
        }

        /// <summary>
        /// The number of octaves for the noise function. This property is an int and must be greater than 0.
        /// </summary>
        public int OctaveCount
        {
            get
            {
                return unchecked((int)GetUIntValue((int)TurbulenceProperties.NumOctaves));
            }
            set
            {
                SetValue((int)TurbulenceProperties.NumOctaves, unchecked((uint)value));
            }
        }

        /// <summary>
        /// The seed for the pseudo random generator. This property is unbounded.
        /// </summary>
        public int Seed
        {
            get
            {
                return unchecked((int)GetUIntValue((int)TurbulenceProperties.Seed));
            }
            set
            {
                SetValue((int)TurbulenceProperties.Seed, unchecked((uint)value));
            }
        }
        
        /// <summary>
        /// The turbulence noise mode. This property can be either fractal sum or turbulence. Indicates whether to generate a bitmap based on Fractal Noise or the Turbulence function. See Noise modes for more info.
        /// </summary>
        public TurbulenceNoise Noise
        {
            get
            {
                return GetEnumValue<TurbulenceNoise>((int)TurbulenceProperties.Noise);
            }
            set
            {
                SetEnumValue((int)TurbulenceProperties.Noise, value);
            }
        }

        /// <summary>
        /// Turns stitching on or off. The base frequency is adjusted so that output bitmap can be stitched. This is useful if you want to tile multiple copies of the turbulence effect output.
        /// true: The output bitmap can be tiled (using the tile effect) without the appearance of seams. The base frequency is adjusted so that output bitmap can be stitched. 
        /// false: The base frequency is not adjusted, so seams may appear between tiles if the bitmap is tiled.
        /// </summary>
        public bool Stitchable
        {
            get
            {
                return GetBoolValue((int)TurbulenceProperties.Stitchable);
            }
            set
            {
                SetValue((int)TurbulenceProperties.Stitchable, value);
            }
        }
    }
}