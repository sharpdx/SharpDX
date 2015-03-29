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

namespace SharpDX.Direct2D1
{
    [ShadowAttribute(typeof(SourceTransformShadow))]
    public partial interface SourceTransform
    {
        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="renderInfo"><para>The interface supplied to the transform to allow specifying the precision-based transform pass.</para></param>	
        /// <returns>If the method succeeds, it returns <see cref="SharpDX.Result.Ok"/>. If it fails, it returns an <see cref="SharpDX.Result"/> error code.</returns>	
        /// <remarks>	
        /// Provides a render information interface to the source transform to allow it to specify state to the rendering system. This part of the render information interface is shared with the GPU transform.	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1SourceTransform::SetRenderInfo']/*"/>	
        /// <unmanaged>HRESULT ID2D1SourceTransform::SetRenderInfo([In] ID2D1RenderInfo* renderInfo)</unmanaged>	
        void SetRenderInformation(SharpDX.Direct2D1.RenderInformation renderInfo);

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="target">No documentation.</param>	
        /// <param name="drawRect">No documentation.</param>	
        /// <param name="targetOrigin">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1SourceTransform::Draw']/*"/>	
        /// <unmanaged>HRESULT ID2D1SourceTransform::Draw([In] ID2D1Bitmap1* target,[In] const RECT* drawRect,[In] D2D_POINT_2U targetOrigin)</unmanaged>	
        void Draw(SharpDX.Direct2D1.Bitmap1 target, RawRectangle drawRect, RawPoint targetOrigin);
    }
}