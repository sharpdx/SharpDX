// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Runtime.InteropServices;

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    public class RenderTarget1D : Texture1DBase
    {
        internal RenderTarget1D(Texture1DDescription description, params IntPtr[] dataRectangles) : base(description, dataRectangles)
        {
        }

        internal RenderTarget1D(GraphicsDevice device, Texture1DDescription description, params IntPtr[] dataRectangles)
            : base(device, description, dataRectangles)
        {
        }

        internal RenderTarget1D(Direct3D11.Texture1D texture)
            : base(texture)
        {
        }

        internal RenderTarget1D(GraphicsDevice device, Direct3D11.Texture1D texture)
            : base(device, texture)
        {
        }

        protected override void InitializeViews()
        {
            // Perform default initialization
            base.InitializeViews();

            if ((Description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                RenderTargetViews = new RenderTargetView[GetViewCount()];
                GetRenderTargetView(ViewSlice.Full, 0, 0);
            }
        }

        public override RenderTargetView GetRenderTargetView(ViewSlice viewSlice, int arrayOrDepthSlice, int mipIndex)
        {
            if ((Description.BindFlags & BindFlags.RenderTarget) == 0)
                return null;

            if (viewSlice == ViewSlice.MipBand)
                throw new NotSupportedException("ViewSlice.MipBand is not supported for render targets");

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewSlice, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var rtvIndex = GetViewIndex(viewSlice, arrayOrDepthSlice, mipIndex);

            lock (RenderTargetViews)
            {
                var rtv = RenderTargetViews[rtvIndex];

                // Creates the shader resource view
                if (rtv == null)
                {
                    // Create the render target view
                    var rtvDescription = new RenderTargetViewDescription() { Format = Description.Format };

                    if (Description.ArraySize > 1)
                    {
                        rtvDescription.Dimension = RenderTargetViewDimension.Texture1DArray;
                        rtvDescription.Texture1DArray.ArraySize = arrayCount;
                        rtvDescription.Texture1DArray.FirstArraySlice = arrayOrDepthSlice;
                        rtvDescription.Texture1DArray.MipSlice = mipIndex;
                    }
                    else
                    {
                        rtvDescription.Dimension = RenderTargetViewDimension.Texture1D;
                        rtvDescription.Texture1D.MipSlice = mipIndex;
                    }

                    rtv = new RenderTargetView(GraphicsDevice, Resource, rtvDescription);
                    RenderTargetViews[rtvIndex] = ToDispose(rtv);
                }
                return rtv;
            }
        }

        public override Texture1DBase Clone()
        {
            return new RenderTarget1D(GraphicsDevice, Description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D"/> from a <see cref="Texture1DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New(Texture1DDescription description)
        {
            return new RenderTarget1D(description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D"/> from a <see cref="Direct3D11.Texture1D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture1D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New(Direct3D11.Texture1D texture)
        {
            return new RenderTarget1D(texture);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="mipCount">(optional) number of mips.</param>
        /// <param name="arraySize">Size of the texture 1D array, default to 1.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <returns>A new instance of <see cref="RenderTarget1D" /> class.</returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New(int width, PixelFormat format, bool isUnorderedReadWrite = false, int mipCount = 1, int arraySize = 1)
        {
            return new RenderTarget1D(NewDescription(width, format, isUnorderedReadWrite, mipCount, 1));
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D" />.
        /// </summary>
        /// <typeparam name="T">Type of the data contained in the mip map textures.</typeparam>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipMapTextures">The mip map textures.</param>
        /// <returns>A new instance of <see cref="RenderTarget1D" /> class.</returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New<T>(int width, PixelFormat format, T[][] mipMapTextures, bool isUnorderedReadWrite = false) where T : struct
        {
            GCHandle[] handles = null;
            try
            {
                var dataRectangles = Pin(mipMapTextures, out handles);
                var texture = new RenderTarget1D(NewDescription(width, format, isUnorderedReadWrite, mipMapTextures.Length, 1), dataRectangles);
                return texture;
            }
            finally
            {
                UnPin(handles);
            }
        }

        protected static Texture1DDescription NewDescription(int width, PixelFormat format, bool isReadWrite, int mipCount, int arraySize)
        {
            var desc = Texture1DBase.NewDescription(width, format, isReadWrite, mipCount, arraySize, ResourceUsage.Default);
            desc.BindFlags |= BindFlags.RenderTarget;
            return desc;
        }
    }
}