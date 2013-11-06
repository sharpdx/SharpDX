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
    /// A RenderTarget2D front end to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    /// <remarks>
    /// This class instantiates a <see cref="Texture2D"/> with the binding flags <see cref="BindFlags.RenderTarget"/>.
    /// This class is also castable to <see cref="Direct3D11.RenderTargetView"/>.
    /// </remarks>
    public class RenderTarget2D : Texture2DBase
    {
        private bool pureRenderTarget;
        private RenderTargetView customRenderTargetView;

        /// <summary>Initializes a new instance of the <see cref="Texture2DBase" /> class.</summary>
        /// <param name="device">The <see cref="GraphicsDevice" />.</param>
        /// <param name="description2D">The description.</param>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        internal RenderTarget2D(GraphicsDevice device, Texture2DDescription description2D)
            : base(device.MainDevice, description2D)
        {
            Initialize(Resource);
        }

        /// <summary>Initializes a new instance of the <see cref="RenderTarget2D"/> class.</summary>
        /// <param name="device">The device.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="renderTargetView">The render target view.</param>
        /// <param name="pureRenderTarget">if set to <see langword="true" /> [pure render target].</param>
        internal RenderTarget2D(GraphicsDevice device, Direct3D11.Texture2D texture, RenderTargetView renderTargetView = null, bool pureRenderTarget = false)
            : base(device.MainDevice, texture)
        {
            this.pureRenderTarget = pureRenderTarget;
            this.customRenderTargetView = renderTargetView;
            Initialize(Resource);
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator RenderTargetView(RenderTarget2D from)
        {
            return from == null ? null : from.renderTargetViews != null ? from.renderTargetViews[0] : null;
        }

        /// <summary>Initializes the views.</summary>
        protected override void InitializeViews()
        {
            if ((this.Description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                this.renderTargetViews = new TextureView[GetViewCount()];
            }

            if (pureRenderTarget)
            {
                renderTargetViews[0] = new TextureView(this, customRenderTargetView);
            }
            else
            {
                // Perform default initialization
                base.InitializeViews();

                if ((this.Description.BindFlags & BindFlags.RenderTarget) != 0)
                {
                    GetRenderTargetView(ViewType.Full, 0, 0);
                }
            }
        }

        /// <summary>Gets the render target view.</summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="arrayOrDepthSlice">The array original depth slice.</param>
        /// <param name="mipIndex">Index of the mip.</param>
        /// <returns>TextureView.</returns>
        /// <exception cref="System.NotSupportedException">ViewSlice.MipBand is not supported for render targets</exception>
        internal override TextureView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipIndex)
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
                        rtvDescription.Dimension = this.Description.SampleDescription.Count > 1 ? RenderTargetViewDimension.Texture2DMultisampledArray : RenderTargetViewDimension.Texture2DArray;
                        if (this.Description.SampleDescription.Count > 1)
                        {
                            rtvDescription.Texture2DMSArray.ArraySize = arrayCount;
                            rtvDescription.Texture2DMSArray.FirstArraySlice = arrayOrDepthSlice;
                        }
                        else
                        {
                            rtvDescription.Texture2DArray.ArraySize = arrayCount;
                            rtvDescription.Texture2DArray.FirstArraySlice = arrayOrDepthSlice;
                            rtvDescription.Texture2DArray.MipSlice = mipIndex;
                        }
                    }
                    else
                    {
                        rtvDescription.Dimension = this.Description.SampleDescription.Count > 1 ? RenderTargetViewDimension.Texture2DMultisampled : RenderTargetViewDimension.Texture2D;
                        if (this.Description.SampleDescription.Count <= 1)
                            rtvDescription.Texture2D.MipSlice = mipIndex;
                    }

                    rtv = new TextureView(this, new RenderTargetView(GraphicsDevice, Resource, rtvDescription));
                    this.renderTargetViews[rtvIndex] = ToDispose(rtv);
                }

                return rtv;
            }
        }

        /// <summary>Makes a copy of this texture.</summary>
        /// <returns>A copy of this texture.</returns>
        /// <remarks>This method doesn't copy the content of the texture.</remarks>
        public override Texture Clone()
        {
            return new RenderTarget2D(GraphicsDevice, this.Description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D"/> from a <see cref="Texture2DDescription"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTarget2D New(GraphicsDevice device, Texture2DDescription description)
        {
            return new RenderTarget2D(device, description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D"/> from a <see cref="Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTarget2D New(GraphicsDevice device, Direct3D11.Texture2D texture)
        {
            return new RenderTarget2D(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D"/> from a <see cref="RenderTargetView"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="renderTargetView">The native texture <see cref="RenderTargetView"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTarget2D New(GraphicsDevice device, RenderTargetView renderTargetView, bool pureRenderTarget = false)
        {
            using (var resource = renderTargetView.Resource)
            {
                return new RenderTarget2D(device, resource.QueryInterface<Direct3D11.Texture2D>(), renderTargetView, pureRenderTarget);
            }
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTarget2D New(GraphicsDevice device, int width, int height, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return New(device, width, height, false, format, flags, arraySize);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D" />.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTarget2D New(GraphicsDevice device, int width, int height, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return new RenderTarget2D(device, CreateDescription(device.MainDevice, width, height, format, flags, mipCount, arraySize, MSAALevel.None));
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D" /> using multisampling.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="multiSampleCount">The multisample count.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTarget2D New(GraphicsDevice device, int width, int height, MSAALevel multiSampleCount, PixelFormat format, int arraySize = 1)
        {
            if (multiSampleCount == MSAALevel.None)
            {
                throw new ArgumentException("Cannot declare a MSAA RenderTarget with MSAALevel.None. Use other non-MSAA constructors", "multiSampleCount");
            }

            return new RenderTarget2D(device, CreateDescription(device.MainDevice, width, height, format, TextureFlags.RenderTarget, 1, arraySize, multiSampleCount));
        }

        /// <summary>
        /// Creates a new texture description for a <see cref="RenderTarget2D" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2DDescription CreateDescription(GraphicsDevice device, int width, int height, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return CreateDescription(device, width, height, false, format, flags, arraySize);
        }

        /// <summary>
        /// Creates a new texture description <see cref="RenderTarget2D" />.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2DDescription CreateDescription(GraphicsDevice device, int width, int height, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return CreateDescription(device.MainDevice, width, height, format, flags, mipCount, arraySize, MSAALevel.None);
        }

        /// <summary>
        /// Creates a new texture description <see cref="RenderTarget2D" /> using multisampling.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="multiSampleCount">The multisample count.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2DDescription CreateDescription(GraphicsDevice device, int width, int height, MSAALevel multiSampleCount, PixelFormat format, int arraySize = 1)
        {
            if (multiSampleCount == MSAALevel.None)
            {
                throw new ArgumentException("Cannot declare a MSAA RenderTarget with MSAALevel.None. Use other non-MSAA constructors", "multiSampleCount");
            }

            return CreateDescription(device.MainDevice, width, height, format, TextureFlags.RenderTarget, 1, arraySize, multiSampleCount);
        }

        /// <summary>
        /// <see cref="SharpDX.Direct3D11.Texture2D"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator SharpDX.Direct3D11.Texture2D(RenderTarget2D from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.Resource;
        }

        /// <summary>Creates the description.</summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="textureFlags">The texture flags.</param>
        /// <param name="mipCount">The mip count.</param>
        /// <param name="arraySize">Size of the array.</param>
        /// <param name="multiSampleCount">The multi sample count.</param>
        /// <returns>Texture2DDescription.</returns>
        internal static Texture2DDescription CreateDescription(GraphicsDevice device, int width, int height, PixelFormat format, TextureFlags textureFlags, int mipCount, int arraySize, MSAALevel multiSampleCount)
        {
            // Make sure that the texture to create is a render target
            textureFlags |= TextureFlags.RenderTarget;
            var desc = Texture2DBase.NewDescription(width, height, format, textureFlags, mipCount, arraySize, ResourceUsage.Default);

            // Sets the MSAALevel
            int maximumMSAA = (int)device.Features[format].MSAALevelMax;
            desc.SampleDescription.Count = Math.Max(1, Math.Min((int)multiSampleCount, maximumMSAA));
            return desc;
        }
    }
}