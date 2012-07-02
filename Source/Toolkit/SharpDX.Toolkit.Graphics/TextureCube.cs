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
    /// A TextureCube frontend to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    public class TextureCube : Texture2DBase
    {
        internal TextureCube(Texture2DDescription description, params DataRectangle[] dataRectangles)
            : base(description, dataRectangles)
        {
        }

        internal TextureCube(GraphicsDevice device, Texture2DDescription description2D, params DataRectangle[] dataRectangles) : base(device, description2D, dataRectangles)
        {
        }

        internal TextureCube(Direct3D11.Texture2D texture) : base(texture)
        {
        }

        internal TextureCube(GraphicsDevice device, Direct3D11.Texture2D texture) : base(device, texture)
        {
        }

        public override RenderTargetView GetRenderTargetView(ViewSlice viewSlice, int arrayOrDepthSlice, int mipMapSlice)
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
            return new TextureCube(GraphicsDevice, this.Description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Texture2DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="TextureCube"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(Texture2DDescription description)
        {
            return new TextureCube(description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="TextureCube"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(Direct3D11.Texture2D texture)
        {
            return new TextureCube(texture);
        }


        /// <summary>
        /// Creates a new <see cref="TextureCube"/>.
        /// </summary>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipCount">(optional) number of mips.</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(int size, PixelFormat format, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(size, false, format, isUnorderedReadWrite, usage);
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube"/>.
        /// </summary>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(int size, MipMap mipCount, PixelFormat format, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Default)
        {
            return new TextureCube(NewTextureCubeDescription(size, format, isUnorderedReadWrite, mipCount, usage));
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube" /> with a single mipmap.
        /// </summary>
        /// <typeparam name="T">Type of a pixel data</typeparam>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="textureDataPerCubeFace">The textures for a single mipmap (6 cube face). See remarks</param>
        /// <returns>A new instance of <see cref="TextureCube" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        /// <remarks>
        /// The first dimension of mipMapTextures is the number of mipmaps, the second is the texture data for a particular mipmap.
        /// </remarks>
        public static TextureCube New<T>(int size, PixelFormat format, T[][] textureDataPerCubeFace, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            return New(size, format, new[] { textureDataPerCubeFace }, isUnorderedReadWrite, usage);
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube" /> array with a mipmaps for each array slice.
        /// </summary>
        /// <typeparam name="T">Type of a pixel data</typeparam>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipTextureDataPerCubeFace">The mip map textures with texture array. See remarks</param>
        /// <returns>A new instance of <see cref="TextureCube" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        /// <remarks>
        /// The first dimension of mipMapTextures describes the number of array (TextureCube Array), second dimension is the mipmap, the third is the texture data for a particular mipmap.
        /// </remarks>
        public static TextureCube New<T>(int size, PixelFormat format, T[][][] mipTextureDataPerCubeFace, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            if (mipTextureDataPerCubeFace.Length != 6)
                throw new ArgumentException("Invalid texture datas. First dimension must be equal to 6", "mipTextureDataPerCubeFace");

            usage = isUnorderedReadWrite ? ResourceUsage.Default : usage;
            GCHandle[] handles = null;
            try
            {
                var dataRectangles = Pin(size, format, mipTextureDataPerCubeFace, out handles);
                var texture = new TextureCube(NewTextureCubeDescription(size, format, isUnorderedReadWrite, mipTextureDataPerCubeFace[0].Length, usage), dataRectangles);
                return texture;
            }
            finally
            {
                UnPin(handles);
            }
        }

        protected static Texture2DDescription NewTextureCubeDescription(int size, PixelFormat format, bool isReadWrite, int mipCount, ResourceUsage usage)
        {
            var desc = Texture2DBase.NewDescription(size, size, format, isReadWrite, mipCount, 6, usage);
            desc.OptionFlags = ResourceOptionFlags.TextureCube;
            return desc;
        }
    }
}