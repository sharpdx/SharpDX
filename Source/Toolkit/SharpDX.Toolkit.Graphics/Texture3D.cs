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
using System.IO;
using SharpDX.Direct3D11;
using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A Texture 3D front end to <see cref="SharpDX.Direct3D11.Texture3D"/>.
    /// </summary>
    public class Texture3D : Texture3DBase
    {
        internal Texture3D(GraphicsDevice device, Texture3DDescription description3D, params DataBox[] dataBox)
            : base(device, description3D, dataBox)
        {
        }

        internal Texture3D(GraphicsDevice device, Direct3D11.Texture3D texture) : base(device, texture)
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
            return new Texture3D(GraphicsDevice, this.Description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Texture3DDescription"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(GraphicsDevice device, Texture3DDescription description)
        {
            return new Texture3D(device, description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Direct3D11.Texture3D"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture3D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(GraphicsDevice device, Direct3D11.Texture3D texture)
        {
            return new Texture3D(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D"/> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(GraphicsDevice device, int width, int height, int depth, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, width, height, depth, false, format, flags, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(GraphicsDevice device, int width, int height, int depth, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Default)
        {
            return new Texture3D(device, NewDescription(width, height, depth, format, flags | TextureFlags.ShaderResource, mipCount, usage));
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D" /> with texture data for the firs map.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload to the texture</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="textureData">The texture data, width * height * depth data </param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>A new instance of <see cref="Texture3D" /> class.</returns>
        /// <remarks>
        /// The first dimension of mipMapTextures describes the number of is an array ot Texture3D Array
        /// </remarks>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public unsafe static Texture3D New<T>(GraphicsDevice device, int width, int height, int depth, PixelFormat format, T[] textureData, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            return New(device, width, height, depth, 1, format, new[] { GetDataBox(format, width, height, depth, textureData, (IntPtr)Interop.Fixed(textureData)) }, flags, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="textureData">DataBox used to fill texture data.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>
        /// A new instance of <see cref="Texture3D"/> class.
        /// </returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(GraphicsDevice device, int width, int height, int depth, MipMapCount mipCount, PixelFormat format, DataBox[] textureData, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Default)
        {
            // TODO Add check for number of texture data according to width/height/depth/mipCount.
            return new Texture3D(device, NewDescription(width, height, depth, format, flags | TextureFlags.ShaderResource, mipCount, usage), textureData);
        }

        /// <summary>
        /// Creates a new <see cref="Texture3D" /> directly from an <see cref="Image"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="image">An image in CPU memory.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="Texture3D" /> class.</returns>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static Texture3D New(GraphicsDevice device, Image image, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            if (image == null) throw new ArgumentNullException("image");
            if (image.Description.Dimension != TextureDimension.Texture3D)
                throw new ArgumentException("Invalid image. Must be 3D", "image");

            return new Texture3D(device, CreateTextureDescriptionFromImage(image, flags | TextureFlags.ShaderResource, usage), image.ToDataBox());
        }

        /// <summary>
        /// Loads a 3D texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type 3D</exception>
        /// <returns>A texture</returns>
        public static new Texture3D Load(GraphicsDevice device, Stream stream, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            var texture = Texture.Load(device, stream, flags | TextureFlags.ShaderResource, usage);
            if (!(texture is Texture3D))
                throw new ArgumentException(string.Format("Texture is not type of [Texture3D] but [{0}]", texture.GetType().Name));
            return (Texture3D)texture;
        }

        /// <summary>
        /// Loads a 3D texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="filePath">The file to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type 3D</exception>
        /// <returns>A texture</returns>
        public static new Texture3D Load(GraphicsDevice device, string filePath, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(device, stream, flags, usage);
        }
    }
}