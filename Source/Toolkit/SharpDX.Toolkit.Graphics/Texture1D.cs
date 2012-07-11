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
    /// <summary>
    /// A Texture 1D frontend to <see cref="SharpDX.Direct3D11.Texture1D"/>.
    /// </summary>
    public class Texture1D : Texture1DBase
    {
        internal Texture1D(Texture1DDescription description, params IntPtr[] dataRectangles)
            : base(description, dataRectangles)
        {
        }

        internal Texture1D(GraphicsDevice device, Texture1DDescription description1D, params IntPtr[] dataRectangles) : base(device, description1D, dataRectangles)
        {
        }

        internal Texture1D(Direct3D11.Texture1D texture) : base(texture)
        {
        }

        internal Texture1D(GraphicsDevice device, Direct3D11.Texture1D texture) : base(device, texture)
        {
        }

        internal override RenderTargetView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipMapSlice)
        {
            throw new System.NotSupportedException();
        }

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public override Texture Clone()
        {
            return new Texture1D(GraphicsDevice, this.Description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Texture1DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>		
        public static Texture1D New(Texture1DDescription description)
        {
            return new Texture1D(description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Direct3D11.Texture1D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture1D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(Direct3D11.Texture1D texture)
        {
            return new Texture1D(texture);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/> with a single mipmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(int width, PixelFormat format, bool isUnorderedReadWrite = false, int arraySize = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(width, false, format, isUnorderedReadWrite, arraySize, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(int width, MipMap mipCount, PixelFormat format, bool isUnorderedReadWrite = false, int arraySize = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return new Texture1D(NewDescription(width, format, isUnorderedReadWrite, mipCount, arraySize, usage));
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D" /> with a single level of mipmap.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="textureData">The texture data for a single mipmap and a single array slice. See remarks</param>
        /// <returns>A new instance of <see cref="Texture1D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_Texture1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>
        /// <remarks>
        /// Each value in textureData is a pixel in the destination texture.
        /// </remarks>
        public static Texture1D New<T>(int width, PixelFormat format, T[] textureData, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            return New(width, format, new[] { new[] { textureData } }, isUnorderedReadWrite, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D" /> with mipmaps.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipMapTextureArray">The mip map textures with arraySize = 1. See remarks</param>
        /// <returns>A new instance of <see cref="Texture1D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_Texture1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>
        /// <remarks>
        /// The first dimension of mipMapTextures is the number of mipmaps, the second is the texture data for a particular mipmap.
        /// </remarks>
        public static Texture1D New<T>(int width, PixelFormat format, T[][] mipMapTextureArray, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            return New(width, format, new[] { mipMapTextureArray }, isUnorderedReadWrite, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D" /> array with mipmaps for each array slice.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipMapTextureArray">The mip map textures with texture array. See remarks</param>
        /// <returns>A new instance of <see cref="Texture1D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_Texture1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>
        /// <remarks>
        /// The first dimension of mipMapTextures describes the number of array (Texture1D Array), second dimension is the mipmap, the third is the texture data for a particular mipmap.
        /// </remarks>
        public static Texture1D New<T>(int width, PixelFormat format, T[][][] mipMapTextureArray, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            usage = isUnorderedReadWrite ? ResourceUsage.Default : usage;
            GCHandle[] handles = null;
            try
            {
                var dataRectangles = Pin(mipMapTextureArray, out handles);
                var texture = new Texture1D(NewDescription(width, format, isUnorderedReadWrite, mipMapTextureArray[0].Length, mipMapTextureArray.Length, usage), dataRectangles);
                return texture;
            }
            finally
            {
                UnPin(handles);
            }
        }
    }
}