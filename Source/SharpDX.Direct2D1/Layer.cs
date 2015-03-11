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
using System;

namespace SharpDX.Direct2D1
{
    public partial class Layer
    {

        /// <summary>	
        /// Creates a layer resource that can be used with this render target and its compatible render targets. The new layer has the specified initial size. The layer resource is allocated to the minimum size when {{PushLayer}} is called.
        /// </summary>	
        /// <remarks>	
        /// Regardless of whether a size is initially specified, the layer automatically resizes as needed.	
        /// </remarks>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <unmanaged>HRESULT CreateLayer([In, Optional] const D2D1_SIZE_F* size,[Out] ID2D1Layer** layer)</unmanaged>
        public Layer(RenderTarget renderTarget)
            : this(renderTarget, null)
        {
        }

        /// <summary>	
        /// Creates a layer resource that can be used with this render target and its compatible render targets. The new layer has the specified initial size.  	
        /// </summary>	
        /// <remarks>	
        /// Regardless of whether a size is initially specified, the layer automatically resizes as needed.	
        /// </remarks>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">If (0, 0) is specified, no backing store is created behind the layer resource. The layer resource is allocated to the minimum size when {{PushLayer}} is called.</param>
        /// <unmanaged>HRESULT CreateLayer([In, Optional] const D2D1_SIZE_F* size,[Out] ID2D1Layer** layer)</unmanaged>
        public Layer(RenderTarget renderTarget, Size2F? size) : base(IntPtr.Zero)
        {
            renderTarget.CreateLayer(size, this);
        }
    }
}
