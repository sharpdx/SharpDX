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

namespace SharpDX.Direct3D11
{
    public partial class RasterizerState
    {
        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name = "device">The device with which to associate the state object.</param>
        /// <param name="description">A rasterizer state description</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public RasterizerState(Device device, RasterizerStateDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateRasterizerState(ref description, this);
        }
    }
}