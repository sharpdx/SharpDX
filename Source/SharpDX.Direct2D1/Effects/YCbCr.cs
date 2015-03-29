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


namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// The built-in YCbCr effect.
    /// </summary>
    public class YCbCr : Effect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YCbCr"/> effect.
        /// </summary>
        /// <param name="context">The device context where this effect instance is attached to.</param>
        public YCbCr(DeviceContext context)
            : base(context, Effect.YCbCr)
        {
        }

        /// <summary>
        /// Gets or sets the chroma subsampling of the input chroma image.
        /// </summary>
        public YcbcrChromaSubSampling ChromaSubSampling
        {
            get { return GetEnumValue<YcbcrChromaSubSampling>((int)YCbCrProperties.ChromaSubSampling); }
            set { SetEnumValue((int)YCbCrProperties.ChromaSubSampling, value); }
        }

        /// <summary>
        /// Gets or sets the axis-aligned affine transform of the image. Axis aligned transforms include Scale, Flips, and 90 degree rotations.
        /// </summary>
        public SharpDX.Mathematics.Interop.RawMatrix3x2 Transform
        {
            get { return GetMatrix3x2Value((int)YCbCrProperties.TransformMatrix); }
            set { SetValue((int)YCbCrProperties.TransformMatrix, value); }
        }

        /// <summary>
        /// Gets or sets the interpolation mode.
        /// </summary>
        public YcbcrInterpolationMode InterpolationMode
        {
            get { return GetEnumValue<YcbcrInterpolationMode>((int)YCbCrProperties.InterpolationMode); }
            set { SetEnumValue((int)YCbCrProperties.InterpolationMode, value); }
        }
    }
}
