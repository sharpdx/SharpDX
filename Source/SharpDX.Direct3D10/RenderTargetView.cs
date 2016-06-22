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

namespace SharpDX.Direct3D10
{
    public partial class RenderTargetView
    {
        /// <summary>
        ///   Creates a <see cref = "T:SharpDX.Direct3D10.RenderTargetView" /> for accessing resource data.
        /// </summary>
        /// <param name = "device">The device to use when creating this <see cref = "T:SharpDX.Direct3D10.RenderTargetView" />.</param>
        /// <param name = "resource">The resource that represents the render-target surface. This surface must have been created with the <see cref = "T:SharpDX.Direct3D10.BindFlags">RenderTarget</see> flag.</param>
        /// <unmanaged>ID3D10Device::CreateRenderTargetView</unmanaged>
        public RenderTargetView(Device device, Resource resource)
            : base(IntPtr.Zero)
        {
            device.CreateRenderTargetView(resource, null, this);
        }

        /// <summary>
        ///   Creates a <see cref = "T:SharpDX.Direct3D10.RenderTargetView" /> for accessing resource data.
        /// </summary>
        /// <param name = "device">The device to use when creating this <see cref = "T:SharpDX.Direct3D10.RenderTargetView" />.</param>
        /// <param name = "resource">The resource that represents the render-target surface. This surface must have been created with the <see cref = "T:SharpDX.Direct3D10.BindFlags">RenderTarget</see> flag.</param>
        /// <param name = "description">A structure describing the <see cref = "T:SharpDX.Direct3D10.RenderTargetView" /> to be created.</param>
        /// <unmanaged>ID3D10Device::CreateRenderTargetView</unmanaged>
        public RenderTargetView(Device device, Resource resource, RenderTargetViewDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateRenderTargetView(resource, description, this);
        }
    }
}