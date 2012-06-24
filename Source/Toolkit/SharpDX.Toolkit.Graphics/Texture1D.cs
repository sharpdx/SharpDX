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
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Texture1D"/>.
    /// </summary>
    /// <msdn-id>ff476633</msdn-id>	
    /// <unmanaged>ID3D11Texture1D</unmanaged>	
    /// <unmanaged-short>ID3D11Texture1D</unmanaged-short>	
    public class Texture1D : Texture<Direct3D11.Texture1D>
    {
        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        internal Texture1D(Texture1DDescription description)
            : this(GraphicsDevice.Current, description)
        {
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <param name="description">The description.</param>
        /// <param name="mipMapTextures">A variable-length parameters list containing mip map textures.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        internal Texture1D(GraphicsDevice device, Texture1DDescription description, params IntPtr[] mipMapTextures)
        {
            Description = description;
            Initialize(device, new Direct3D11.Texture1D(device, description, mipMapTextures));
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        internal Texture1D(Direct3D11.Texture1D texture)
            : this(GraphicsDevice.Current, texture)
        {
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="texture">The texture.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        internal Texture1D(GraphicsDevice device, Direct3D11.Texture1D texture)
        {
            Description = texture.Description;
            Initialize(device, texture);
        }

        /// <summary>The description.</summary>
        public Texture1DDescription Description { get; private set; }

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public Texture1D Clone()
        {
            return new Texture1D(GraphicsDevice, Description);
        }

        public Texture1D ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new Texture1D(this.GraphicsDevice, stagingDesc);
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
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <param name="width">Width in pixel of the texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="mipCount">(optional)number of mips.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(int width, PixelFormat format, int mipCount = 1)
        {
            return New(width, format, ResourceUsage.Default, false, mipCount);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(int width, PixelFormat format, bool isReadWrite, int mipCount = 1)
        {
            return New(width, format, ResourceUsage.Default, isReadWrite, mipCount);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New<T>(int width, PixelFormat format, params T[][] mipMapTextures)
        {
            return New(width, format, ResourceUsage.Immutable, false, mipMapTextures);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New<T>(int width, PixelFormat format, bool isReadWrite, params T[][] mipMapTextures)
        {
            return New(width, format, isReadWrite ? ResourceUsage.Default : ResourceUsage.Immutable, isReadWrite, mipMapTextures);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(int width, PixelFormat format, ResourceUsage usage, bool isReadWrite = false, int mipCount = 1)
        {
            return new Texture1D(NewDescription(width, format, isReadWrite, mipCount, usage));
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New<T>(int width, PixelFormat format, ResourceUsage usage, bool isReadWrite = false, params T[][] mipMapTextures)
        {
            GCHandle[] handles;
            var dataRectangles = Pin(mipMapTextures, out handles);
            var texture = new Texture1D(GraphicsDevice.Current, NewDescription(width, format, isReadWrite, mipMapTextures.Length, usage), dataRectangles);
            UnPin(handles);
            return texture;
        }

        protected static IntPtr[] Pin<T>(T[][] mipMapTextures, out GCHandle[] handles)
        {
            var pinnedPointers = new IntPtr[mipMapTextures.Length];
            handles = new GCHandle[mipMapTextures.Length];
            for (int i = 0; i < mipMapTextures.Length; i++)
            {
                var initialTexture = mipMapTextures[i];
                var handle = GCHandle.Alloc(initialTexture, GCHandleType.Pinned);
                handles[i] = handle;
                pinnedPointers[i] = handle.AddrOfPinnedObject();
            }
            return pinnedPointers;
        }

        protected static void UnPin(GCHandle[] handles)
        {
            for (int i = 0; i < handles.Length; i++)
                handles[i].Free();
        }

        protected static Texture1DDescription NewDescription(int width, PixelFormat format, bool isReadWrite = false, int mipCount = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            var desc = new Texture1DDescription() {
                Width = width,
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                Format = format,
                MipLevels = mipCount,
                Usage = usage,
                OptionFlags = ResourceOptionFlags.None,
                CpuAccessFlags = GetCputAccessFlagsFromUsage(usage)
            };

            if (isReadWrite)
            {
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            return desc;
        }

        public override ShaderResourceView GetShaderResourceView(ViewSlice viewSlice, int arrayIndex, int mipIndex)
        {
            throw new NotImplementedException();
        }

        public override RenderTargetView GetRenderTargetView(ViewSlice viewSlice, int arraySlice, int mipMapSlice)
        {
            throw new NotImplementedException();
        }

        public override UnorderedAccessView GetUnorderedAccessView(int arraySlice, int mipMapSlice)
        {
            throw new NotImplementedException();
        }
    }
}