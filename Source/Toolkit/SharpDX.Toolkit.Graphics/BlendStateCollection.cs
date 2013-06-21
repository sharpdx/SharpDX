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
    /// Blend state collection
    /// </summary>
    public sealed class BlendStateCollection : StateCollectionBase<BlendState>
    {
        /// <summary>
        /// A built-in state object with settings for additive blend, that is adding the destination data to the source data without using alpha.
        /// </summary>
        public readonly BlendState Additive;

        /// <summary>
        /// A built-in state object with settings for alpha blend, that is blending the source and destination data using alpha.
        /// </summary>
        public readonly BlendState AlphaBlend;

        /// <summary>
        /// A built-in state object with settings for blending with non-premultiplied alpha, that is blending source and destination data using alpha while assuming the color data contains no alpha information.
        /// </summary>
        public readonly BlendState NonPremultiplied;

        /// <summary>
        /// A built-in state object with settings for opaque blend, that is overwriting the source with the destination data.
        /// </summary>
        public readonly BlendState Opaque;

        /// <summary>
        /// A built-in default state object (no blending).
        /// </summary>
        public readonly BlendState Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendStateCollection" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal BlendStateCollection(GraphicsDevice device) : base(device)
        {
            Additive = Add(BlendState.New(device, "Additive", BlendOption.SourceAlpha, BlendOption.One));
            AlphaBlend = Add(BlendState.New(device, "AlphaBlend", BlendOption.One, BlendOption.InverseSourceAlpha));
            NonPremultiplied = Add(BlendState.New(device, "NonPremultiplied", BlendOption.SourceAlpha, BlendOption.InverseSourceAlpha));
            Opaque = Add(BlendState.New(device, "Opaque", BlendOption.One, BlendOption.Zero));
            Default = Add(BlendState.New(device, "Default", BlendStateDescription.Default()));
        }
    }
}