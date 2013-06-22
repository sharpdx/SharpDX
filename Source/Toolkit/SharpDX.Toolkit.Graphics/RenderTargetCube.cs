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
using SharpDX.DXGI;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A RenderTargetCube front end to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    /// <remarks>
    /// This class instantiates a <see cref="Texture2D"/> with the binding flags <see cref="BindFlags.RenderTarget"/>.
    /// This class is also castable to <see cref="RenderTargetView"/>.
    /// </remarks>
    public class RenderTargetCube : Texture2DBase
    {
        internal RenderTargetCube(GraphicsDevice device, Texture2DDescription description2D, params DataBox[] dataBoxes)
            : base(device, description2D, dataBoxes)
        {
            Initialize(Resource);
        }

        internal RenderTargetCube(GraphicsDevice device, Direct3D11.Texture2D texture)
            : base(device, texture)
        {
            Initialize(Resource);
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator RenderTargetView(RenderTargetCube from)
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
                    var rtvDescription = new RenderTargetViewDescription {
                        Format = this.Description.Format,
                        Dimension = RenderTargetViewDimension.Texture2DArray,
                        Texture2DArray = {
                            ArraySize = arrayCount,
                            FirstArraySlice = arrayOrDepthSlice,
                            MipSlice = mipIndex
                        }
                    };

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
            return new RenderTargetCube(GraphicsDevice, this.Description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTargetCube"/> from a <see cref="Texture2DDescription"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTargetCube"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTargetCube New(GraphicsDevice device, Texture2DDescription description)
        {
            return new RenderTargetCube(device, description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTargetCube"/> from a <see cref="Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTargetCube"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTargetCube New(GraphicsDevice device, Direct3D11.Texture2D texture)
        {
            return new RenderTargetCube(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTargetCube" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>A new instance of <see cref="RenderTargetCube" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTargetCube New(GraphicsDevice device, int size, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource)
        {
            return New(device, size, false, format, flags | TextureFlags.RenderTarget);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTargetCube" />.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>A new instance of <see cref="RenderTargetCube" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTargetCube New(GraphicsDevice device, int size, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource)
        {
            return new RenderTargetCube(device, NewRenderTargetDescription(size, format, flags | TextureFlags.RenderTarget, mipCount));
        }

        protected static Texture2DDescription NewRenderTargetDescription(int size, PixelFormat format, TextureFlags flags, int mipCount)
        {
            var desc = Texture2DBase.NewDescription(size, size, format, flags, mipCount, 6, ResourceUsage.Default);
            desc.OptionFlags = ResourceOptionFlags.TextureCube;
            return desc;
        }
    }
}