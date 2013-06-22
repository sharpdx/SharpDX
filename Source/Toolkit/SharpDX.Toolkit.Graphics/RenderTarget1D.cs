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
using System.Runtime.InteropServices;

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A RenderTarget1D front end to <see cref="SharpDX.Direct3D11.Texture1D"/>.
    /// </summary>
    /// <remarks>
    /// This class instantiates a <see cref="Texture1D"/> with the binding flags <see cref="BindFlags.RenderTarget"/>.
    /// This class is also castable to <see cref="RenderTargetView"/>.
    /// </remarks>
    public class RenderTarget1D : Texture1DBase
    {
        internal RenderTarget1D(GraphicsDevice device, Texture1DDescription description1D)
            : base(device, description1D)
        {
        }

        internal RenderTarget1D(GraphicsDevice device, Direct3D11.Texture1D texture)
            : base(device, texture)
        {
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator RenderTargetView(RenderTarget1D from)
        {
            return from == null ? null : from.renderTargetViews != null ? from.renderTargetViews[0] : null;
        }

        protected override void InitializeViews()
        {
            // Perform default initialization
            base.InitializeViews();

            if ((this.Description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                this.renderTargetViews = new RenderTargetView[GetViewCount()];
                GetRenderTargetView(ViewType.Full, 0, 0);
            }
        }

        internal override RenderTargetView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipIndex)
        {
            if ((this.Description.BindFlags & BindFlags.RenderTarget) == 0)
                return null;

            if (viewType == ViewType.MipBand)
                throw new NotSupportedException("ViewSlice.MipBand is not supported for render targets");

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewType, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var rtvIndex = GetViewIndex(viewType, arrayOrDepthSlice, mipIndex);

            lock (this.renderTargetViews)
            {
                var rtv = this.renderTargetViews[rtvIndex];

                // Creates the shader resource view
                if (rtv == null)
                {
                    // Create the render target view
                    var rtvDescription = new RenderTargetViewDescription() { Format = this.Description.Format };

                    if (this.Description.ArraySize > 1)
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
                    this.renderTargetViews[rtvIndex] = ToDispose(rtv);
                }

                // Associate this instance
                rtv.Tag = this;

                return rtv;
            }
        }

        public override Texture Clone()
        {
            return new RenderTarget1D(GraphicsDevice, this.Description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D"/> from a <see cref="Texture1DDescription"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New(GraphicsDevice device, Texture1DDescription description)
        {
            return new RenderTarget1D(device, description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D"/> from a <see cref="Direct3D11.Texture1D"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture1D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New(GraphicsDevice device, Direct3D11.Texture1D texture)
        {
            return new RenderTarget1D(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 1D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget1D" /> class.</returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New(GraphicsDevice device, int width, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return New(device, width, false, format, flags | TextureFlags.RenderTarget, arraySize);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget1D" />.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 1D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget1D" /> class.</returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static RenderTarget1D New(GraphicsDevice device, int width, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return new RenderTarget1D(device, NewRenderTargetDescription(width, format, flags | TextureFlags.RenderTarget, mipCount, arraySize));
        }

        protected static Texture1DDescription NewRenderTargetDescription(int width, PixelFormat format, TextureFlags textureFlags,int mipCount, int arraySize)
        {
            var desc = Texture1DBase.NewDescription(width, format, textureFlags, mipCount, arraySize, ResourceUsage.Default);
            return desc;
        }
    }
}