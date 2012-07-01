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

using System.Runtime.InteropServices;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A Texture 3D frontend to <see cref="SharpDX.Direct3D11.Texture3D"/>.
    /// </summary>
    public class Texture3D : Texture3DBase
    {
        internal Texture3D(Texture3DDescription description, params DataBox[] dataRectangles)
            : base(description, dataRectangles)
        {
        }

        internal Texture3D(GraphicsDevice device, Texture3DDescription description, params DataBox[] dataRectangles) : base(device, description, dataRectangles)
        {
        }

        internal Texture3D(Direct3D11.Texture3D texture) : base(texture)
        {
        }

        internal Texture3D(GraphicsDevice device, Direct3D11.Texture3D texture) : base(device, texture)
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
        public override Texture3DBase Clone()
        {
            return new Texture3D(GraphicsDevice, Description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Texture3DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(Texture3DDescription description)
        {
            return new Texture3D(description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Direct3D11.Texture3D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture3D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(Direct3D11.Texture3D texture)
        {
            return new Texture3D(texture);
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipCount">(optional) number of mips.</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(int width, int height, int depth, PixelFormat format, bool isUnorderedReadWrite = false, int mipCount = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return new Texture3D(NewDescription(width, height, depth, format, isUnorderedReadWrite, mipCount, usage));
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D" /> with a single mipmap texture data.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload to the texture</typeparam>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="textureData">The mip map textures. See remarks</param>
        /// <returns>A new instance of <see cref="Texture3D" /> class.</returns>
        /// <remarks>
        /// The first dimension of mipMapTextures describes the number of is an array ot Texture3D Array
        /// </remarks>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New<T>(int width, int height, int depth, PixelFormat format, T[] textureData, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            return New(width, height, depth, format, new[] {textureData}, isUnorderedReadWrite, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D" /> with texture data for each mipmaps.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload to the texture</typeparam>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="mipMapTextures">The mip map textures. See remarks</param>
        /// <returns>A new instance of <see cref="Texture3D" /> class.</returns>
        /// <remarks>
        /// The first dimension of mipMapTextures describes the number of is an array ot Texture3D Array
        /// </remarks>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New<T>(int width, int height, int depth, PixelFormat format, T[][] mipMapTextures, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            usage = isUnorderedReadWrite ? ResourceUsage.Default : usage;
            GCHandle[] handles = null;
            try
            {
                var dataRectangles = Pin(width, height, format, mipMapTextures, out handles);
                var texture = new Texture3D(NewDescription(width, height, depth, format, isUnorderedReadWrite, mipMapTextures.Length, usage), dataRectangles);
                return texture;
            }
            finally
            {
                UnPin(handles);
            }
        }
    }
}