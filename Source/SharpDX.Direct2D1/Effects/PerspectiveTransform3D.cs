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
    /// Rotates the image in 3 dimensions as if viewed from a distance.
    /// </summary>
    /// <remarks>
    /// The <see cref="PerspectiveTransform3D"/> is more convenient than the <see cref="Transform3D"/> effect, but only exposes a subset of the functionality. You can compute a full 3D transformation matrix and apply a more arbitrary transform matrix to an image using the <see cref="Transform3D"/> effect.
    /// </remarks>
    public class PerspectiveTransform3D : Effect
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PerspectiveTransform3D"/> class.
        /// </summary>
        /// <param name="deviceContext">The device context where this effect is attached to.</param>
        public PerspectiveTransform3D(DeviceContext deviceContext)
            : base(deviceContext, Effect.PerspectiveTransform3D)
        {
        }

        /// <summary>
        /// Image interpolation mode.
        /// </summary>
        public PerspectiveTransform3DInteroplationMode InterpolationMode
        {
            get { return GetEnumValue<PerspectiveTransform3DInteroplationMode>((int)PerspectiveTransform3DProperties.InterpolationMode); }
            set { SetEnumValue((int)PerspectiveTransform3DProperties.InterpolationMode, value); }
        }

        /// <summary>
        /// The border mode.
        /// </summary>
        public BorderMode BorderMode
        {
            get { return GetEnumValue<BorderMode>((int)PerspectiveTransform3DProperties.BorderMode); }
            set { SetEnumValue((int)PerspectiveTransform3DProperties.BorderMode, value); }
        }

        /// <summary>
        /// The perspective depth.
        /// </summary>
        public float Depth
        {
            get { return GetFloatValue((int)PerspectiveTransform3DProperties.Depth); }
            set { SetValue((int)PerspectiveTransform3DProperties.Depth, value); }
        }

        /// <summary>
        /// The perspective origin.
        /// </summary>
        public RawVector2 PerspectiveOrigin
        {
            get { return GetVector2Value((int)PerspectiveTransform3DProperties.PerspectiveOrigin); }
            set { SetValue((int)PerspectiveTransform3DProperties.PerspectiveOrigin, value); }
        }

        /// <summary>
        /// The transformation local offset.
        /// </summary>
        public RawVector3 LocalOffset
        {
            get { return GetVector3Value((int)PerspectiveTransform3DProperties.LocalOffset); }
            set { SetValue((int)PerspectiveTransform3DProperties.LocalOffset, value); }
        }

        /// <summary>
        /// The transformation global offset.
        /// </summary>
        public RawVector3 GlobalOffset
        {
            get { return GetVector3Value((int)PerspectiveTransform3DProperties.GlobalOffset); }
            set { SetValue((int)PerspectiveTransform3DProperties.GlobalOffset, value); }
        }

        /// <summary>
        /// The transformation rotation origin.
        /// </summary>
        public RawVector3 RotationOrigin
        {
            get { return GetVector3Value((int)PerspectiveTransform3DProperties.RotationOrigin); }
            set { SetValue((int)PerspectiveTransform3DProperties.RotationOrigin, value); }
        }

        /// <summary>
        /// The transformation rotation.
        /// </summary>
        public RawVector3 Rotation
        {
            get { return GetVector3Value((int)PerspectiveTransform3DProperties.Rotation); }
            set { SetValue((int)PerspectiveTransform3DProperties.Rotation, value); }
        }
    }
}