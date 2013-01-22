// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Sampler state collection.
    /// </summary>
    public sealed class SamplerStateCollection : StateCollectionBase<SamplerState>
    {
        /// <summary>
        /// Default state is using linear filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState Default;

        /// <summary>
        /// Point filtering with texture coordinate wrapping.
        /// </summary>
        public readonly SamplerState PointWrap;

        /// <summary>
        /// Point filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState PointClamp;

        /// <summary>
        /// Point filtering with texture coordinate mirroring.
        /// </summary>
        public readonly SamplerState PointMirror;

        /// <summary>
        /// Linear filtering with texture coordinate wrapping.
        /// </summary>
        public readonly SamplerState LinearWrap;

        /// <summary>
        /// Linear filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState LinearClamp;

        /// <summary>
        /// Linear filtering with texture coordinate mirroring.
        /// </summary>
        public readonly SamplerState LinearMirror;

        /// <summary>
        /// Anisotropic filtering with texture coordinate wrapping.
        /// </summary>
        public readonly SamplerState AnisotropicWrap;

        /// <summary>
        /// Anisotropic filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState AnisotropicClamp;

        /// <summary>
        /// Anisotropic filtering with texture coordinate mirroring.
        /// </summary>
        public readonly SamplerState AnisotropicMirror;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerStateCollection" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal SamplerStateCollection(GraphicsDevice device) : base(device)
        {
            PointWrap = Add(SamplerState.New(device, "PointWrap", Filter.MinMagMipPoint, TextureAddressMode.Wrap));
            PointClamp = Add(SamplerState.New(device, "PointClamp", Filter.MinMagMipPoint, TextureAddressMode.Clamp));
            PointMirror = Add(SamplerState.New(device, "PointMirror", Filter.MinMagMipPoint, TextureAddressMode.Mirror));
            LinearWrap = Add(SamplerState.New(device, "LinearWrap", Filter.MinMagMipLinear, TextureAddressMode.Wrap));
            LinearClamp = Add(SamplerState.New(device, "LinearClamp", Filter.MinMagMipLinear, TextureAddressMode.Clamp));
            LinearMirror = Add(SamplerState.New(device, "LinearMirror", Filter.MinMagMipLinear, TextureAddressMode.Mirror));
            AnisotropicWrap = Add(SamplerState.New(device, "AnisotropicWrap", Filter.Anisotropic, TextureAddressMode.Wrap));
            AnisotropicClamp = Add(SamplerState.New(device, "AnisotropicClamp", Filter.Anisotropic, TextureAddressMode.Clamp));
            AnisotropicMirror = Add(SamplerState.New(device, "AnisotropicMirror", Filter.Anisotropic, TextureAddressMode.Mirror));
            Default = LinearClamp;
        }
    }
}