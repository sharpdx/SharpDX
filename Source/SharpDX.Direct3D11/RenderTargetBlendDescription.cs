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
namespace SharpDX.Direct3D11
{
    public partial struct RenderTargetBlendDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBlendDescription" /> struct.
        /// </summary>
        /// <param name="isBlendEnabled">The is blend enabled.</param>
        /// <param name="sourceBlend">The source blend.</param>
        /// <param name="destinationBlend">The destination blend.</param>
        /// <param name="blendOperation">The blend operation.</param>
        /// <param name="sourceAlphaBlend">The source alpha blend.</param>
        /// <param name="destinationAlphaBlend">The destination alpha blend.</param>
        /// <param name="alphaBlendOperation">The alpha blend operation.</param>
        /// <param name="renderTargetWriteMask">The render target write mask.</param>
        public RenderTargetBlendDescription(bool isBlendEnabled, BlendOption sourceBlend, BlendOption destinationBlend, BlendOperation blendOperation, BlendOption sourceAlphaBlend, BlendOption destinationAlphaBlend, BlendOperation alphaBlendOperation, ColorWriteMaskFlags renderTargetWriteMask)
        {
            IsBlendEnabled = isBlendEnabled;
            SourceBlend = sourceBlend;
            DestinationBlend = destinationBlend;
            BlendOperation = blendOperation;
            SourceAlphaBlend = sourceAlphaBlend;
            DestinationAlphaBlend = destinationAlphaBlend;
            AlphaBlendOperation = alphaBlendOperation;
            RenderTargetWriteMask = renderTargetWriteMask;
        }

        public override string ToString()
        {
            return string.Format("IsBlendEnabled: {0}, SourceBlend: {1}, DestinationBlend: {2}, BlendOperation: {3}, SourceAlphaBlend: {4}, DestinationAlphaBlend: {5}, AlphaBlendOperation: {6}, RenderTargetWriteMask: {7}", IsBlendEnabled, SourceBlend, DestinationBlend, BlendOperation, SourceAlphaBlend, DestinationAlphaBlend, AlphaBlendOperation, RenderTargetWriteMask);
        }
    }
}