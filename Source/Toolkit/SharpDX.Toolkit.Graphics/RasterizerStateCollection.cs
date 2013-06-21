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
    /// Rasterizer state collection.
    /// </summary>
    public sealed class RasterizerStateCollection : StateCollectionBase<RasterizerState>
    {
        /// <summary>
        /// Built-in rasterizer state object with settings for wireframe rendering.
        /// </summary>
        public readonly RasterizerState WireFrame;

        /// <summary>
        /// Built-in rasterizer state object with settings for wireframe rendering.
        /// </summary>
        public readonly RasterizerState WireFrameCullNone;

        /// <summary>
        /// Built-in rasterizer state object with settings for culling primitives with clockwise winding order (front facing).
        /// </summary>
        public readonly RasterizerState CullFront;

        /// <summary>
        /// Built-in rasterizer state object with settings for culling primitives with counter-clockwise winding order (back facing).
        /// </summary>
        public readonly RasterizerState CullBack;

        /// <summary>
        /// Built-in rasterizer state object with settings for not culling any primitives.
        /// </summary>
        public readonly RasterizerState CullNone;

        /// <summary>
        /// Built-in default rasterizer state object is back facing (see <see cref="CullBack"/>).
        /// </summary>
        public readonly RasterizerState Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerStateCollection" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal RasterizerStateCollection(GraphicsDevice device) : base(device)
        {
            CullFront = Add(RasterizerState.New(device, "CullFront", CullMode.Front));
            CullBack =  Add(RasterizerState.New(device, "CullBack", CullMode.Back));
            CullNone =  Add(RasterizerState.New(device, "CullNone", CullMode.None));

            var wireFrameDesk = CullBack.Description;
            wireFrameDesk.FillMode = FillMode.Wireframe;
            WireFrame = Add(RasterizerState.New(device, "WireFrame", wireFrameDesk));

            wireFrameDesk.CullMode = CullMode.None;
            WireFrameCullNone = Add(RasterizerState.New(device, "WireFrameCullNone", wireFrameDesk));

            Default = CullBack;
        }
    }
}