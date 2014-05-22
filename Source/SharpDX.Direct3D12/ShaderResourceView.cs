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

using System;

namespace SharpDX.Direct3D12
{
    public partial class ShaderResourceView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResourceView"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="desc">The desc.</param>
        /// <unmanaged>HRESULT ID3D12Device::CreateShaderResourceView([In] ID3D12Resource* pResource,[In, Optional] const D3D12_SHADER_RESOURCE_VIEW_DESC* pDesc,[Out, Fast] ID3D12ShaderResourceView** ppView)</unmanaged>
        ///   <unmanaged-short>ID3D12Device::CreateShaderResourceView</unmanaged-short>
        public ShaderResourceView(Device device, SharpDX.Direct3D12.Resource resource, 
            SharpDX.Direct3D12.ShaderResourceViewDescription? desc = null
            ) : base(IntPtr.Zero)
        {
            if(device == null) throw new ArgumentNullException("device");
            device.CreateShaderResourceView(resource, desc, this);
        }
    }
}