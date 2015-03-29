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
    /// Marks an area of an input image as opaque, so internal rendering optimizations to the graph are possible.
    /// </summary>
    /// <remarks>
    /// This effect doesn't modify the image itself to be opaque. It modifies data associated with the image so the renderer assumes the specified region is opaque.
    /// </remarks>
    public class OpacityMetadata : Effect
    {
        /// <summary>
        /// Creates a new instance of the <see cref="OpacityMetadata"/> class.
        /// </summary>
        /// <param name="deviceContext">The device context where this effect is attached to.</param>
        public OpacityMetadata(DeviceContext deviceContext)
            : base(deviceContext, Effect.OpacityMetadata)
        {
        }

        /// <summary>
        /// The portion of the source image that is opaque. The default is the entire input image.
        /// </summary>
        public RawVector4 OpaqueRectangle
        {
            get { return GetVector4Value((int)OpacityMetadataProperties.InputOpaqueRectangle); }
            set { SetValue((int)OpacityMetadataProperties.InputOpaqueRectangle, value); }
        }
    }
}