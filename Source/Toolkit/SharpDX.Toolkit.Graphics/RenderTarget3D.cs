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
    public class RenderTarget3D : Texture3DBase
    {
        internal RenderTarget3D(Texture3DDescription description, params DataBox[] dataRectangles) : base(description, dataRectangles)
        {
        }

        internal RenderTarget3D(GraphicsDevice device, Texture3DDescription description, params DataBox[] dataRectangles)
            : base(device, description, dataRectangles)
        {
        }

        internal RenderTarget3D(Direct3D11.Texture3D texture)
            : base(texture)
        {
        }

        internal RenderTarget3D(GraphicsDevice device, Direct3D11.Texture3D texture)
            : base(device, texture)
        {
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator RenderTargetView(RenderTarget3D from)
        {
            return from.RenderTargetViews != null ? from.RenderTargetViews[0] : null;
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
                    var rtvDescription = new RenderTargetViewDescription()
                                             {
                                                 Format = Description.Format,
                                                 Dimension = RenderTargetViewDimension.Texture3D,
                                                 Texture3D =
                                                 {
                                                     DepthSliceCount = arrayCount,
                                                     FirstDepthSlice = arrayOrDepthSlice,
                                                     MipSlice = mipIndex,
                                                 }
                                             };

                    rtv = new RenderTargetView(GraphicsDevice, Resource, rtvDescription);
                    RenderTargetViews[rtvIndex] = ToDispose(rtv);
                }
                return rtv;
            }
        }

        public override Texture3DBase Clone()
        {
            return new RenderTarget3D(GraphicsDevice, Description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D"/> from a <see cref="Texture3DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget3D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static RenderTarget3D New(Texture3DDescription description)
        {
            return new RenderTarget3D(description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D"/> from a <see cref="Direct3D11.Texture3D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture3D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget3D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static RenderTarget3D New(Direct3D11.Texture3D texture)
        {
            return new RenderTarget3D(texture);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipCount">(optional) number of mips.</param>
        /// <param name="arraySize">Size of the texture 3D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget3D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>
        public static RenderTarget3D New(int width, int height, int depth,  PixelFormat format, bool isUnorderedReadWrite = false, int mipCount = 1, int arraySize = 1)
        {
            return new RenderTarget3D(NewDescription(width, height, depth, format, isUnorderedReadWrite, mipCount, ResourceUsage.Default));
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D" /> with a single texture data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="mipMapTextureArray">The mip map textures with arraySize = 1. See remarks</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="RenderTarget3D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>
        /// <remarks>The first dimension of mipMapTextures is the number of mipmaps, the second is the texture data for a particular mipmap.</remarks>
        public static RenderTarget3D New<T>(int width, int height, int depth, PixelFormat format, T[] mipMapTextureArray, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            return New(width, height, depth, format, new[] { mipMapTextureArray }, isUnorderedReadWrite, usage);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D" /> with mipmaps.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="mipMapTextureArray">The mip map textures with texture array. See remarks</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="RenderTarget3D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>
        /// <remarks>The first dimension of mipMapTextures describes the number of array (RenderTarget3D Array), second dimension is the mipmap, the third is the texture data for a particular mipmap.</remarks>
        public static RenderTarget3D New<T>(int width, int height, int depth, PixelFormat format, T[][] mipMapTextureArray, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            usage = isUnorderedReadWrite ? ResourceUsage.Default : usage;
            GCHandle[] handles = null;
            try
            {
                var dataRectangles = Pin(width, height, format, mipMapTextureArray, out handles);
                var texture = new RenderTarget3D(NewDescription(width, height, depth, format, isUnorderedReadWrite, mipMapTextureArray.Length, usage), dataRectangles);
                return texture;
            }
            finally
            {
                UnPin(handles);
            }
        }

        protected new static Texture3DDescription NewDescription(int width, int height, int depth, PixelFormat format, bool isReadWrite, int mipCount, ResourceUsage usage)
        {
            var desc = Texture3DBase.NewDescription(width, height, depth, format, isReadWrite, mipCount, ResourceUsage.Default);
            desc.BindFlags |= BindFlags.RenderTarget;
            return desc;
        }
    }
}