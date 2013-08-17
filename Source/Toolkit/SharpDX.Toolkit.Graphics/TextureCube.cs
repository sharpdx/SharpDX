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
    /// A TextureCube front end to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    public class TextureCube : Texture2DBase
    {
        internal TextureCube(GraphicsDevice device, Texture2DDescription description2D, params DataBox[] dataBoxes) : base(device, description2D, dataBoxes)
        {
            Initialize(Resource);
        }

        internal TextureCube(GraphicsDevice device, Direct3D11.Texture2D texture) : base(device, texture)
        {
            Initialize(Resource);
        }

        internal override TextureView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipMapSlice)
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
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="TextureCube"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(GraphicsDevice device, Texture2DDescription description)
        {
            return new TextureCube(device, description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="TextureCube"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(GraphicsDevice device, Direct3D11.Texture2D texture)
        {
            return new TextureCube(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(GraphicsDevice device, int size, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, size, false, format, flags, usage);
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static TextureCube New(GraphicsDevice device, int size, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Default)
        {
            return new TextureCube(device, NewTextureCubeDescription(size, format, flags | TextureFlags.ShaderResource, mipCount, usage));
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube" /> from a initial data..
        /// </summary>
        /// <typeparam name="T">Type of a pixel data</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="textureData">an array of 6 textures. See remarks</param>
        /// <returns>A new instance of <see cref="TextureCube" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        /// <remarks>
        /// The first dimension of mipMapTextures describes the number of array (TextureCube Array), the second is the texture data for a particular cube face.
        /// </remarks>
        public unsafe static TextureCube New<T>(GraphicsDevice device, int size, PixelFormat format, T[][] textureData, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
        {
            if (textureData.Length != 6)
                throw new ArgumentException("Invalid texture data. First dimension must be equal to 6", "textureData");

            var dataBox1 = GetDataBox(format, size, size, 1, textureData[0], (IntPtr)Interop.Fixed(textureData[0]));
            var dataBox2 = GetDataBox(format, size, size, 1, textureData[0], (IntPtr)Interop.Fixed(textureData[1]));
            var dataBox3 = GetDataBox(format, size, size, 1, textureData[0], (IntPtr)Interop.Fixed(textureData[2]));
            var dataBox4 = GetDataBox(format, size, size, 1, textureData[0], (IntPtr)Interop.Fixed(textureData[3]));
            var dataBox5 = GetDataBox(format, size, size, 1, textureData[0], (IntPtr)Interop.Fixed(textureData[4]));
            var dataBox6 = GetDataBox(format, size, size, 1, textureData[0], (IntPtr)Interop.Fixed(textureData[5]));

            return new TextureCube(device, NewTextureCubeDescription(size, format, flags | TextureFlags.ShaderResource, 1, usage), dataBox1, dataBox2, dataBox3, dataBox4, dataBox5, dataBox6);
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube" /> from a initial data..
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="textureData">an array of 6 textures. See remarks</param>
        /// <returns>A new instance of <see cref="TextureCube" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        /// <remarks>
        /// The first dimension of mipMapTextures describes the number of array (TextureCube Array), the second is the texture data for a particular cube face.
        /// </remarks>
        public static TextureCube New(GraphicsDevice device, int size, PixelFormat format, DataBox[] textureData, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            if (textureData.Length != 6)
                throw new ArgumentException("Invalid texture data. First dimension must be equal to 6", "textureData");

            return new TextureCube(device, NewTextureCubeDescription(size, format, flags | TextureFlags.ShaderResource, 1, usage), textureData);
        }

        /// <summary>
        /// Creates a new <see cref="TextureCube" /> directly from an <see cref="Image"/>.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="image">An image in CPU memory.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="TextureCube" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static TextureCube New(GraphicsDevice device, Image image, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            if (image == null) throw new ArgumentNullException("image");
            if (image.Description.Dimension != TextureDimension.TextureCube)
                throw new ArgumentException("Invalid image. Must be Cube", "image");

            return new TextureCube(device, CreateTextureDescriptionFromImage(image, flags | TextureFlags.ShaderResource, usage), image.ToDataBox());
        }

        /// <summary>
        /// Loads a Cube texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type Cube</exception>
        /// <returns>A texture</returns>
        public static new TextureCube Load(GraphicsDevice device, Stream stream, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            var texture = Texture.Load(device, stream, flags | TextureFlags.ShaderResource, usage);
            if (!(texture is TextureCube))
                throw new ArgumentException(string.Format("Texture is not type of [TextureCube] but [{0}]", texture.GetType().Name));
            return (TextureCube)texture;
        }

        /// <summary>
        /// Loads a Cube texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="filePath">The file to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type Cube</exception>
        /// <returns>A texture</returns>
        public static new TextureCube Load(GraphicsDevice device, string filePath, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(device, stream, flags | TextureFlags.ShaderResource, usage);
        }

        protected static Texture2DDescription NewTextureCubeDescription(int size, PixelFormat format, TextureFlags flags, int mipCount, ResourceUsage usage)
        {
            var desc = NewDescription(size, size, format, flags, mipCount, 6, usage);
            desc.OptionFlags = ResourceOptionFlags.TextureCube;
            return desc;
        }
    }
}