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
    /// Built in Transform3D effect.
    /// </summary>
    public class Transform3D : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Transform3D"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Transform3D(DeviceContext context)
            : base(context, Effect.Transform3D)
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
                return GetEnumValue<InterpolationMode>((int)Transform3DProperties.InterpolationMode);
            }
            set
            {
                SetEnumValue((int)Transform3DProperties.InterpolationMode, value);
            }
        }

        /// <summary>
        /// The mode used to calculate the border of the image, soft or hard. See <see cref="BorderMode"/> modes for more info.
        /// </summary>
        public BorderMode BorderMode
        {
            get
            {
                return GetEnumValue<BorderMode>((int)Transform3DProperties.BorderMode);
            }
            set
            {
                SetEnumValue((int)Transform3DProperties.BorderMode, value);
            }
        }

        /// <summary>
        /// A 4x4 transform matrix applied to the projection plane.
        /// </summary>
        public RawMatrix TransformMatrix
        {
            get
            {
                return GetMatrixValue((int)Transform3DProperties.TransformMatrix);
            }
            set
            {
                SetValue((int)Transform3DProperties.TransformMatrix, value);
            }
        }
    }
}