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
using System.Diagnostics;

namespace SharpDX.Direct3D11
{
    public partial class Texture1D
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public Texture1D(Device device, Texture1DDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateTexture1D(ref description, null, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public Texture1D(Device device, Texture1DDescription description, params DataStream[] data)
            : base(IntPtr.Zero)
        {
            DataBox[] subResourceDatas = null;
            if (data != null && data.Length > 0)
            {
                subResourceDatas = new DataBox[data.Length];
                for (int i = 0; i < subResourceDatas.Length; i++)
                    subResourceDatas[i].DataPointer = data[i].DataPointer;

            }
            device.CreateTexture1D(ref description, subResourceDatas, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public Texture1D(Device device, Texture1DDescription description, params IntPtr[] data)
            : base(IntPtr.Zero)
        {
            DataBox[] subResourceDatas = null;
            if (data != null && data.Length > 0)
            {
                subResourceDatas = new DataBox[data.Length];
                for (int i = 0; i < subResourceDatas.Length; i++)
                    subResourceDatas[i].DataPointer = data[i];

            }
            device.CreateTexture1D(ref description, subResourceDatas, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public Texture1D(Device device, Texture1DDescription description, DataBox[] data)
            : base(IntPtr.Zero)
        {
            device.CreateTexture1D(ref description, data, this);
        }

        /// <inheritdoc/>
        public override int CalculateSubResourceIndex(int mipSlice, int arraySlice, out int mipSize)
        {
            var desc = Description;
            mipSize = CalculateMipSize(mipSlice, desc.Width);
            return CalculateSubResourceIndex(mipSlice, arraySlice, desc.MipLevels);
        }
    }
}