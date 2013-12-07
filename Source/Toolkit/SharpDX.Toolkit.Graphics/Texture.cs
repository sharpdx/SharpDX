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
using System.Collections.Generic;
using System.IO;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.IO;
using SharpDX.Toolkit.Content;
using DeviceChild = SharpDX.Direct3D11.DeviceChild;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base class for texture resources.
    /// </summary>
    [ContentReader(typeof(TextureContentReader))]
    public abstract class Texture : GraphicsResource, IComparable<Texture>
    {
        private long textureId;

        /// <summary>
        /// Common description for this texture.
        /// </summary>
        public readonly TextureDescription Description;

        /// <summary>
        /// Gets the selector for a <see cref="ShaderResourceView"/>
        /// </summary>
        public readonly ShaderResourceViewSelector ShaderResourceView;

        /// <summary>
        /// Gets the selector for a <see cref="RenderTargetView"/>
        /// </summary>
        public readonly RenderTargetViewSelector RenderTargetView;

        /// <summary>
        /// Gets the selector for a <see cref="UnorderedAccessView"/>
        /// </summary>
        public readonly UnorderedAccessViewSelector UnorderedAccessView;

        /// <summary>
        /// Gets a boolean indicating whether this <see cref="Texture"/> is a using a block compress format (BC1, BC2, BC3, BC4, BC5, BC6H, BC7).
        /// </summary>
        public readonly bool IsBlockCompressed;

        /// <summary>
        /// The width stride in bytes (number of bytes per row).
        /// </summary>
        internal readonly int RowStride;

        /// <summary>
        /// The depth stride in bytes (number of bytes per depth slice).
        /// </summary>
        internal readonly int DepthStride;

        internal TextureView defaultShaderResourceView;
        internal Dictionary<TextureViewKey, TextureView> shaderResourceViews;
        internal TextureView[] renderTargetViews;
        internal UnorderedAccessView[] unorderedAccessViews;
        private MipMapDescription[] mipmapDescriptions;

        protected Texture(GraphicsDevice device, TextureDescription description) : base(device.MainDevice)
        {
            Description = description;
            IsBlockCompressed = FormatHelper.IsCompressed(description.Format);
            RowStride = this.Description.Width * ((PixelFormat)this.Description.Format).SizeInBytes;
            DepthStride = RowStride * this.Description.Height;
            ShaderResourceView = new ShaderResourceViewSelector(this);
            RenderTargetView = new RenderTargetViewSelector(this);
            UnorderedAccessView = new UnorderedAccessViewSelector(this);
            mipmapDescriptions = Image.CalculateMipMapDescription(description);
        }

        /// <summary>	
        /// <dd> <p>Texture width (in texels). The  range is from 1 to <see cref="SharpDX.Direct3D11.Resource.MaximumTexture1DSize"/> (16384). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>	
        /// </summary>	
        /// <remarks>
        /// This field is valid for all textures: <see cref="Texture1D"/>, <see cref="Texture2D"/>, <see cref="Texture3D"/> and <see cref="TextureCube"/>.
        /// </remarks>
        public int Width
        {
            get { return Description.Width; }
        }

        /// <summary>	
        /// <dd> <p>Texture height (in texels). The  range is from 1 to <see cref="SharpDX.Direct3D11.Resource.MaximumTexture3DSize"/> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>	
        /// </summary>	
        /// <remarks>
        /// This field is only valid for <see cref="Texture2D"/>, <see cref="Texture3D"/> and <see cref="TextureCube"/>.
        /// </remarks>
        public int Height
        {
            get { return Description.Height; }
        }

        /// <summary>	
        /// <dd> <p>Texture depth (in texels). The  range is from 1 to <see cref="SharpDX.Direct3D11.Resource.MaximumTexture3DSize"/> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>	
        /// </summary>	
        /// <remarks>
        /// This field is only valid for <see cref="Texture3D"/>.
        /// </remarks>
        public int Depth
        {
            get { return Description.Depth; }
        }

        /// <summary>
        /// Gets the texture format.
        /// </summary>
        /// <value>The texture format.</value>
        public PixelFormat Format
        {
            get { return Description.Format; }
        }

        protected override void Initialize(DeviceChild resource)
        {
            // Be sure that we are storing only the main device (which contains the immediate context).
            base.Initialize(resource);
            InitializeViews();
            // Gets a Texture ID
            textureId = resource.NativePointer.ToInt64();
        }

        /// <summary>
        /// Initializes the views provided by this texture.
        /// </summary>
        protected abstract void InitializeViews();

        /// <summary>
        /// Gets the mipmap description of this instance for the specified mipmap level.
        /// </summary>
        /// <param name="mipmap">The mipmap.</param>
        /// <returns>A description of a particular mipmap for this texture.</returns>
        public MipMapDescription GetMipMapDescription(int mipmap)
        {
            return mipmapDescriptions[mipmap];
        }

        /// <summary>
        /// Generates the mip maps for this texture. See remarks.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Cannot generate mipmaps for this texture (Must be RenderTarget and ShaderResource and MipLevels > 1</exception>
        /// <remarks>This method is only working for texture that are RenderTarget and ShaderResource and with MipLevels &gt; 1</remarks>
        public void GenerateMipMaps()
        {
            GenerateMipMaps(GraphicsDevice);
        }

        /// <summary>
        /// Generates the mip maps for this texture. See remarks.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="System.NotSupportedException">Cannot generate mipmaps for this texture (Must be RenderTarget and ShaderResource and MipLevels > 1</exception>
        /// <remarks>This method is only working for texture that are RenderTarget and ShaderResource and with MipLevels &gt; 1</remarks>
        public void GenerateMipMaps(GraphicsDevice device)
        {
            // If the texture is a RenderTarget + ShaderResource + MipLevels > 1, then allow for GenerateMipMaps method
            if ((Description.BindFlags & BindFlags.RenderTarget) == 0 || (Description.BindFlags & BindFlags.ShaderResource) == 0 || Description.MipLevels == 1)
            {
                throw new NotSupportedException("Cannot generate mipmaps for this texture (Must be RenderTarget and ShaderResource and MipLevels > 1");
            }

            ((DeviceContext)device).GenerateMips(this);
        }

        /// <summary>
        /// Calculates the number of miplevels for a Texture 1D.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="mipLevels">A <see cref="MipMapCount"/>, set to true to calculates all mipmaps, to false to calculate only 1 miplevel, or > 1 to calculate a specific amount of levels.</param>
        /// <returns>The number of miplevels.</returns>
        public static int CalculateMipLevels(int width, MipMapCount mipLevels)
        {
            if (mipLevels > 1)
            {
                int maxMips = CountMips(width);
                if (mipLevels > maxMips)
                    throw new InvalidOperationException(String.Format("MipLevels must be <= {0}", maxMips));
            }
            else if (mipLevels == 0)
            {
                mipLevels = CountMips(width);
            }
            else
            {
                mipLevels = 1;
            }
            return mipLevels;
        }

        /// <summary>
        /// Calculates the number of miplevels for a Texture 2D.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="mipLevels">A <see cref="MipMapCount"/>, set to true to calculates all mipmaps, to false to calculate only 1 miplevel, or > 1 to calculate a specific amount of levels.</param>
        /// <returns>The number of miplevels.</returns>
        public static int CalculateMipLevels(int width, int height, MipMapCount mipLevels)
        {
            if (mipLevels > 1)
            {
                int maxMips = CountMips(width, height);
                if (mipLevels > maxMips)
                    throw new InvalidOperationException(String.Format("MipLevels must be <= {0}", maxMips));
            }
            else if (mipLevels == 0)
            {
                mipLevels = CountMips(width, height);
            }
            else
            {
                mipLevels = 1;
            }
            return mipLevels;
        }

        /// <summary>
        /// Calculates the number of miplevels for a Texture 2D.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="depth">The depth of the texture.</param>
        /// <param name="mipLevels">A <see cref="MipMapCount"/>, set to true to calculates all mipmaps, to false to calculate only 1 miplevel, or > 1 to calculate a specific amount of levels.</param>
        /// <returns>The number of miplevels.</returns>
        public static int CalculateMipLevels(int width, int height, int depth, MipMapCount mipLevels)
        {
            if (mipLevels > 1)
            {
                if (!IsPow2(width) || !IsPow2(height) || !IsPow2(depth))
                    throw new InvalidOperationException("Width/Height/Depth must be power of 2");

                int maxMips = CountMips(width, height, depth);
                if (mipLevels > maxMips)
                    throw new InvalidOperationException(String.Format("MipLevels must be <= {0}", maxMips));
            }
            else if (mipLevels == 0)
            {
                if (!IsPow2(width) || !IsPow2(height) || !IsPow2(depth))
                    throw new InvalidOperationException("Width/Height/Depth must be power of 2");

                mipLevels = CountMips(width, height, depth);
            }
            else
            {
                mipLevels = 1;
            }
            return mipLevels;
        }

        public static int CalculateMipSize(int width, int mipLevel)
        {
            mipLevel = Math.Min(mipLevel, CountMips(width));
            width = width >> mipLevel;
            return width > 0 ? width : 1;
        }

        /// <summary>
        /// Gets the absolute sub-resource index from the array and mip slice.
        /// </summary>
        /// <param name="arraySlice">The array slice index.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <returns>A value equals to arraySlice * Description.MipLevels + mipSlice.</returns>
        public int GetSubResourceIndex(int arraySlice, int mipSlice)
        {
            return arraySlice * Description.MipLevels + mipSlice;
        }

        /// <summary>
        /// Calculates the expected width of a texture using a specified type.
        /// </summary>
        /// <typeparam name="TData">The type of the T pixel data.</typeparam>
        /// <returns>The expected width</returns>
        /// <exception cref="System.ArgumentException">If the size is invalid</exception>
        public int CalculateWidth<TData>(int mipLevel = 0) where TData : struct
        {
            var widthOnMip = CalculateMipSize((int)Description.Width, mipLevel);
            var rowStride = widthOnMip * ((PixelFormat) Description.Format).SizeInBytes;

            var dataStrideInBytes = Utilities.SizeOf<TData>() * widthOnMip;
            var width = ((double)rowStride / dataStrideInBytes) * widthOnMip;
            if (Math.Abs(width - (int)width) > Double.Epsilon)
                throw new ArgumentException("sizeof(TData) / sizeof(Format) * Width is not an integer");

            return (int)width;
        }

        /// <summary>
        /// Calculates the number of pixel data this texture is requiring for a particular mip level.
        /// </summary>
        /// <typeparam name="TData">The type of the T pixel data.</typeparam>
        /// <param name="mipLevel">The mip level.</param>
        /// <returns>The number of pixel data.</returns>
        /// <remarks>This method is used to allocated a texture data buffer to hold pixel data: var textureData = new T[ texture.CalculatePixelCount&lt;T&gt;() ] ;.</remarks>
        public int CalculatePixelDataCount<TData>(int mipLevel = 0) where TData : struct
        {
            return CalculateWidth<TData>(mipLevel) * CalculateMipSize(Description.Height, mipLevel) * CalculateMipSize(Description.Depth, mipLevel);
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
        public abstract Texture Clone();

        /// <summary>
        /// Makes a copy of this texture with type casting.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public T Clone<T>() where T : Texture
        {
            return (T)this.Clone();
        }

        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="arrayOrDepthSlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <returns>The texture data.</returns>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// This method creates internally a staging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        public TData[] GetData<TData>(int arrayOrDepthSlice = 0, int mipSlice = 0) where TData : struct
        {
            var toData = new TData[this.CalculatePixelDataCount<TData>(mipSlice)];
            GetData(toData, arrayOrDepthSlice, mipSlice);
            return toData;
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture data.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// This method creates internally a staging resource if this texture is not already a staging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        public void GetData<TData>(TData[] toData, int arraySlice = 0, int mipSlice = 0) where TData : struct
        {
            // Get data from this resource
            if (Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(this, toData, arraySlice, mipSlice);
            }
            else
            {
                // Inefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = this.ToStaging())
                    GetData(throughStaging, toData, arraySlice, mipSlice);
            }
        }

        /// <summary>
        /// Creates a new texture with the specified generic texture description.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="description">The description.</param>
        /// <returns>A Texture instance, either a RenderTarget or DepthStencilBuffer or Texture, depending on Binding flags.</returns>
        public static Texture New(GraphicsDevice graphicsDevice, TextureDescription description)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }

            if ((description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                switch (description.Dimension)
                {
                    case TextureDimension.Texture1D:
                        return RenderTarget1D.New(graphicsDevice, description);
                    case TextureDimension.Texture2D:
                        return RenderTarget2D.New(graphicsDevice, description);
                    case TextureDimension.Texture3D:
                        return RenderTarget3D.New(graphicsDevice, description);
                    case TextureDimension.TextureCube:
                        return RenderTargetCube.New(graphicsDevice, description);
                }
            } 
            else if ((description.BindFlags & BindFlags.DepthStencil) != 0)
            {
                return DepthStencilBuffer.New(graphicsDevice, description);
            }
            else
            {
                switch (description.Dimension)
                {
                    case TextureDimension.Texture1D:
                        return Texture1D.New(graphicsDevice, description);
                    case TextureDimension.Texture2D:
                        return Texture2D.New(graphicsDevice, description);
                    case TextureDimension.Texture3D:
                        return Texture3D.New(graphicsDevice, description);
                    case TextureDimension.TextureCube:
                        return TextureCube.New(graphicsDevice, description);
                }
            }

            return null;
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="stagingTexture">The staging texture used to transfer the texture to.</param>
        /// <param name="toData">To data.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// </remarks>
        public unsafe void GetData<TData>(Texture stagingTexture, TData[] toData, int arraySlice = 0, int mipSlice = 0) where TData : struct
        {
            GetData(stagingTexture, new DataPointer((IntPtr)Interop.Fixed(toData), toData.Length * Utilities.SizeOf<TData>()), arraySlice, mipSlice);
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to a pointer on CPU memory using a specific staging resource.
        /// </summary>
        /// <param name="stagingTexture">The staging texture used to transfer the texture to.</param>
        /// <param name="toData">The pointer to data in CPU memory.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// </remarks>
        public unsafe void GetData(Texture stagingTexture, DataPointer toData, int arraySlice = 0, int mipSlice = 0)
        {
            var device = GraphicsDevice;
            var deviceContext = (Direct3D11.DeviceContext)device;

            // Get mipmap description for the specified mipSlice
            var mipmap = this.GetMipMapDescription(mipSlice);

            // Copy height, depth
            int height = mipmap.HeightPacked;
            int depth = mipmap.Depth;

            // Calculate depth stride based on mipmap level
            int rowStride = mipmap.RowStride;

            // Depth Stride
            int textureDepthStride = mipmap.DepthStride;

            // MipMap Stride
            int mipMapSize = mipmap.MipmapSize;

            // Check size validity of data to copy to
            if (toData.Size > mipMapSize)
                throw new ArgumentException(string.Format("Size of toData ({0} bytes) is not compatible expected size ({1} bytes) : Width * Height * Depth * sizeof(PixelFormat) size in bytes", toData.Size, mipMapSize));

            // Copy the actual content of the texture to the staging resource
            if (!ReferenceEquals(this, stagingTexture))
                device.Copy(this, stagingTexture);

            // Calculate the subResourceIndex for a Texture2D
            int subResourceIndex = this.GetSubResourceIndex(arraySlice, mipSlice);
            try
            {
                // Map the staging resource to a CPU accessible memory
                var box = deviceContext.MapSubresource(stagingTexture, subResourceIndex, MapMode.Read, MapFlags.None);

                // If depth == 1 (Texture1D, Texture2D or TextureCube), then depthStride is not used
                var boxDepthStride = this.Description.Depth == 1 ? box.SlicePitch : textureDepthStride;

                // The fast way: If same stride, we can directly copy the whole texture in one shot
                if (box.RowPitch == rowStride && boxDepthStride == textureDepthStride)
                {
                    Utilities.CopyMemory(toData.Pointer, box.DataPointer, mipMapSize);
                }
                else
                {
                    // Otherwise, the long way by copying each scanline
                    var sourcePerDepthPtr = (byte*)box.DataPointer;
                    var destPtr = (byte*)toData.Pointer;

                    // Iterate on all depths
                    for (int j = 0; j < depth; j++)
                    {
                        var sourcePtr = sourcePerDepthPtr;
                        // Iterate on each line
                        for (int i = 0; i < height; i++)
                        {
                            // Copy a single row
                            Utilities.CopyMemory(new IntPtr(destPtr), new IntPtr(sourcePtr), rowStride);
                            sourcePtr += box.RowPitch;
                            destPtr += rowStride;
                        }
                        sourcePerDepthPtr += box.SlicePitch;
                    }
                }
            }
            finally
            {
                // Make sure that we unmap the resource in case of an exception
                deviceContext.UnmapSubresource(stagingTexture, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <param name="region">Destination region</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// This method is only working on the main graphics device. Use method with explicit graphics device to set data on a deferred context.
        /// See also unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        public void SetData<TData>(TData[] fromData, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null) where TData : struct
        {
            SetData(GraphicsDevice, fromData, arraySlice, mipSlice, region);
        }

        /// <summary>
        /// Copies the content an data on CPU memory to this texture into GPU memory using the specified <see cref="GraphicsDevice"/> (The graphics device could be deferred).
        /// </summary>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <param name="region">Destination region</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// This method is only working on the main graphics device. Use method with explicit graphics device to set data on a deferred context.
        /// See also unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        public void SetData(DataPointer fromData, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null)
        {
            SetData(GraphicsDevice, fromData, arraySlice, mipSlice, region);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory using the specified <see cref="GraphicsDevice"/> (The graphics device could be deferred).
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <param name="region">Destination region</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void SetData<TData>(GraphicsDevice device, TData[] fromData, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null) where TData : struct
        {
            SetData(device, new DataPointer((IntPtr)Interop.Fixed(fromData), fromData.Length * Utilities.SizeOf<TData>()), arraySlice, mipSlice, region);
        }

        /// <summary>
        /// Copies the content an data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <param name="region">Destination region</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void SetData(GraphicsDevice device, DataPointer fromData, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null)
        {
            var deviceContext = (Direct3D11.DeviceContext)device;

            if (region.HasValue && this.Description.Usage != ResourceUsage.Default)
                throw new ArgumentException("Region is only supported for textures with ResourceUsage.Default");

            // Get mipmap description for the specified mipSlice
            var mipMapDesc = this.GetMipMapDescription(mipSlice);

            int width = mipMapDesc.Width;
            int height = mipMapDesc.Height;
            int depth = mipMapDesc.Depth;

            // If we are using a region, then check that parameters are fine
            if (region.HasValue)
            {
                int newWidth = region.Value.Right - region.Value.Left;
                int newHeight = region.Value.Bottom - region.Value.Top;
                int newDepth = region.Value.Back - region.Value.Front;
                if (newWidth > width)
                    throw new ArgumentException(string.Format("Region width [{0}] cannot be greater than mipmap width [{1}]", newWidth, width), "region");
                if (newHeight > height)
                    throw new ArgumentException(string.Format("Region height [{0}] cannot be greater than mipmap height [{1}]", newHeight, height), "region");
                if (newDepth > depth)
                    throw new ArgumentException(string.Format("Region depth [{0}] cannot be greater than mipmap depth [{1}]", newDepth, depth), "region");

                width = newWidth;
                height = newHeight;
                depth = newDepth;
            }

            // Size per pixel
            var sizePerElement = (int)FormatHelper.SizeOfInBytes(this.Description.Format);

            // Calculate depth stride based on mipmap level
            int rowStride;

            // Depth Stride
            int textureDepthStride;

            // Compute Actual pitch
            Image.ComputePitch(this.Description.Format, width, height, out rowStride, out textureDepthStride, out width, out height);

            // Size Of actual texture data
            int sizeOfTextureData = textureDepthStride * depth;

            // Check size validity of data to copy to
            if (fromData.Size != sizeOfTextureData)
                throw new ArgumentException(string.Format("Size of toData ({0} bytes) is not compatible expected size ({1} bytes) : Width * Height * Depth * sizeof(PixelFormat) size in bytes", fromData.Size, sizeOfTextureData));

            // Calculate the subResourceIndex for a Texture
            int subResourceIndex = this.GetSubResourceIndex(arraySlice, mipSlice);

            // If this texture is declared as default usage, we use UpdateSubresource that supports sub resource region.
            if (this.Description.Usage == ResourceUsage.Default)
            {
                // If using a specific region, we need to handle this case
                if (region.HasValue)
                {
                    var regionValue = region.Value;
                    var sourceDataPtr = fromData.Pointer;

                    // Workaround when using region with a deferred context and a device that does not support CommandList natively
                    // see http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx
                    if (device.needWorkAroundForUpdateSubResource)
                    {
                        if (this.IsBlockCompressed)
                        {
                            regionValue.Left /= 4;
                            regionValue.Right /= 4;
                            regionValue.Top /= 4;
                            regionValue.Bottom /= 4;
                        }
                        sourceDataPtr = new IntPtr((byte*)sourceDataPtr - (regionValue.Front * textureDepthStride) - (regionValue.Top * rowStride) - (regionValue.Left * sizePerElement));
                    }
                    deviceContext.UpdateSubresource(new DataBox(sourceDataPtr, rowStride, textureDepthStride), this, subResourceIndex, regionValue);
                }
                else
                {
                    deviceContext.UpdateSubresource(new DataBox(fromData.Pointer, rowStride, textureDepthStride), this, subResourceIndex);
                }
            }
            else
            {
                try
                {
                    var box = deviceContext.MapSubresource(this, subResourceIndex, this.Description.Usage == ResourceUsage.Dynamic ? MapMode.WriteDiscard : MapMode.Write, MapFlags.None);

                    // If depth == 1 (Texture1D, Texture2D or TextureCube), then depthStride is not used
                    var boxDepthStride = this.Description.Depth == 1 ? box.SlicePitch : textureDepthStride;

                    // The fast way: If same stride, we can directly copy the whole texture in one shot
                    if (box.RowPitch == rowStride && boxDepthStride == textureDepthStride)
                    {
                        Utilities.CopyMemory(box.DataPointer, fromData.Pointer, sizeOfTextureData);
                    }
                    else
                    {
                        // Otherwise, the long way by copying each scanline
                        var destPerDepthPtr = (byte*)box.DataPointer;
                        var sourcePtr = (byte*)fromData.Pointer;

                        // Iterate on all depths
                        for (int j = 0; j < depth; j++)
                        {
                            var destPtr = destPerDepthPtr;
                            // Iterate on each line
                            for (int i = 0; i < height; i++)
                            {
                                Utilities.CopyMemory((IntPtr)destPtr, (IntPtr)sourcePtr, rowStride);
                                destPtr += box.RowPitch;
                                sourcePtr += rowStride;
                            }
                            destPerDepthPtr += box.SlicePitch;
                        }

                    }
                }
                finally
                {
                    deviceContext.UnmapSubresource(this, subResourceIndex);
                }
            }
        }

        /// <summary>
        /// Copies the content of an image to this texture.
        /// </summary>
        /// <param name="image">The source image to copy from.</param>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public void SetData(Image image)
        {
            SetData(GraphicsDevice, image);
        }

        /// <summary>
        /// Copies the content of an image to this texture.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="image">The source image to copy from.</param>
        /// <exception cref="System.ArgumentException">Image is not same dimension and/or format than this texture</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public void SetData(GraphicsDevice graphicsDevice, Image image)
        {
            var textureDescription = image.Description;

            if (textureDescription.Width != Description.Width
                || textureDescription.Height != Description.Height
                || textureDescription.ArraySize != Description.ArraySize
                || textureDescription.Depth != Description.Depth
                || textureDescription.Dimension != Description.Dimension
                || textureDescription.Format != Description.Format
                || textureDescription.MipLevels != Description.MipLevels)
            {
                throw new ArgumentException("Image is not same dimension and/or format than this texture");
            }

            for (int arrayIndex = 0; arrayIndex < image.Description.ArraySize; arrayIndex++)
            {
                for (int mipLevel = 0; mipLevel < image.Description.MipLevels; mipLevel++)
                {
                    var pixelBuffer = image.PixelBuffer[arrayIndex, mipLevel];
                    SetData(graphicsDevice, new DataPointer(pixelBuffer.DataPointer, pixelBuffer.BufferStride), arrayIndex, mipLevel);
                }
            }
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public abstract Texture ToStaging();

        /// <summary>
        /// Gets a specific <see cref="ShaderResourceView" /> from this texture.
        /// </summary>
        /// <param name="viewFormat"></param>
        /// <param name="viewType">Type of the view slice.</param>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipIndex">The mip map slice index.</param>
        /// <returns>An <see cref="ShaderResourceView" /></returns>
        internal abstract TextureView GetShaderResourceView(Format viewFormat, ViewType viewType, int arrayOrDepthSlice, int mipIndex);

        /// <summary>
        /// Gets a specific <see cref="RenderTargetView" /> from this texture.
        /// </summary>
        /// <param name="viewType">Type of the view slice.</param>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipMapSlice">The mip map slice index.</param>
        /// <returns>An <see cref="RenderTargetView" /></returns>
        internal abstract TextureView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipMapSlice);

        /// <summary>
        /// Gets a specific <see cref="UnorderedAccessView"/> from this texture.
        /// </summary>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipMapSlice">The mip map slice index.</param>
        /// <returns>An <see cref="UnorderedAccessView"/></returns>
        internal abstract UnorderedAccessView GetUnorderedAccessView(int arrayOrDepthSlice, int mipMapSlice);

        /// <summary>
        /// ShaderResourceView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator ShaderResourceView(Texture from)
        {
            return @from == null ? null : from.defaultShaderResourceView;
        }

        /// <summary>
        /// UnorderedAccessView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator UnorderedAccessView(Texture from)
        {
            return @from == null ? null : @from.unorderedAccessViews != null ? @from.unorderedAccessViews[0] : null;
        }

        /// <summary>
        /// Loads a texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <returns>A texture</returns>
        public static Texture Load(GraphicsDevice device, Stream stream, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            var image = Image.Load(stream);
            if (image == null)
            {
                return null;
            }

            try
            {
                switch (image.Description.Dimension)
                {
                    case TextureDimension.Texture1D:
                        return Texture1D.New(device, image, flags, usage);
                    case TextureDimension.Texture2D:
                        return Texture2D.New(device, image, flags, usage);
                    case TextureDimension.Texture3D:
                        return Texture3D.New(device, image, flags, usage);
                    case TextureDimension.TextureCube:
                        return TextureCube.New(device, image, flags, usage);
                }
            } finally
            {
                image.Dispose();
            }

            throw new InvalidOperationException("Dimension not supported");
        }

        /// <summary>
        /// Loads a texture from a file.
        /// </summary>
        /// <param name="device">Specify the <see cref="GraphicsDevice"/> used to load and create a texture from a file.</param>
        /// <param name="filePath">The file to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <returns>A texture</returns>
        public static Texture Load(GraphicsDevice device, string filePath, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(device, stream, flags, usage);
        }

        /// <summary>
        /// Saves this texture to a stream with a specified format.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileType">Type of the image file.</param>
        public void Save(Stream stream, ImageFileType fileType)
        {
            using (var staging = ToStaging())
            {
                Save(stream, staging, fileType);
            }
        }

        /// <summary>
        /// Gets the GPU content of this texture as an <see cref="Image"/> on the CPU.
        /// </summary>
        public Image GetDataAsImage()
        {
            using (var stagingTexture = ToStaging())
                return GetDataAsImage(stagingTexture);
        }

        /// <summary>
        /// Gets the GPU content of this texture to an <see cref="Image"/> on the CPU.
        /// </summary>
        /// <param name="stagingTexture">The staging texture used to temporary transfer the image from the GPU to CPU.</param>
        /// <exception cref="ArgumentException">If stagingTexture is not a staging texture.</exception>
        public Image GetDataAsImage(Texture stagingTexture)
        {
            if (stagingTexture.Description.Usage != ResourceUsage.Staging)
                throw new ArgumentException("Invalid texture used as staging. Must have Usage = ResourceUsage.Staging", "stagingTexture");

            var image = Image.New(stagingTexture.Description);
            try {
                for (int arrayIndex = 0; arrayIndex < image.Description.ArraySize; arrayIndex++)
                {
                    for (int mipLevel = 0; mipLevel < image.Description.MipLevels; mipLevel++)
                    {
                        var pixelBuffer = image.PixelBuffer[arrayIndex, mipLevel];
                        GetData(stagingTexture, new DataPointer(pixelBuffer.DataPointer, pixelBuffer.BufferStride), arrayIndex, mipLevel);
                    }
                }

            } catch (Exception)
            {
                // If there was an exception, free the allocated image to avoid any memory leak.
                image.Dispose();
                throw;
            }
            return image;
        }

        /// <summary>
        /// Saves this texture to a stream with a specified format.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="stagingTexture">The staging texture used to temporary transfer the image from the GPU to CPU.</param>
        /// <param name="fileType">Type of the image file.</param>
        /// <exception cref="ArgumentException">If stagingTexture is not a staging texture.</exception>
        public void Save(Stream stream, Texture stagingTexture, ImageFileType fileType)
        {
            using (var image = GetDataAsImage(stagingTexture))
                image.Save(stream, fileType);
        }

        /// <summary>
        /// Saves this texture to a file with a specified format.
        /// </summary>
        /// <param name="filePath">The file path to save the texture to.</param>
        /// <param name="fileType">Type of the image file.</param>
        public void Save(string filePath, ImageFileType fileType)
        {
            using (var staging = ToStaging())
            {
                Save(filePath, staging, fileType);
            }
        }

        /// <summary>
        /// Saves this texture to a stream with a specified format.
        /// </summary>
        /// <param name="filePath">The file path to save the texture to.</param>
        /// <param name="stagingTexture">The staging texture used to temporary transfer the image from the GPU to CPU.</param>
        /// <param name="fileType">Type of the image file.</param>
        /// <exception cref="ArgumentException">If stagingTexture is not a staging texture.</exception>
        public void Save(string filePath, Texture stagingTexture, ImageFileType fileType)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write))
                Save(stream, stagingTexture, fileType);
        }

        /// <summary>
        /// Calculates the mip map count from a requested level.
        /// </summary>
        /// <param name="requestedLevel">The requested level.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <returns>The resulting mipmap count (clamp to [1, maxMipMapCount] for this texture)</returns>
        internal static int CalculateMipMapCount(MipMapCount requestedLevel, int width, int height = 0, int depth = 0)
        {
            int size = Math.Max(Math.Max(width, height), depth);
            int maxMipMap = 1 + (int)Math.Ceiling(Math.Log(size) / Math.Log(2.0));

            return requestedLevel  == 0 ? maxMipMap : Math.Min(requestedLevel, maxMipMap);
        }

        protected static DataBox GetDataBox<T>(Format format, int width, int height, int depth, T[] textureData, IntPtr fixedPointer) where T : struct
        {
            // Check that the textureData size is correct
            if (textureData == null) throw new ArgumentNullException("textureData");
            int rowPitch;
            int slicePitch;
            int widthCount;
            int heightCount;
            Image.ComputePitch(format, width, height, out rowPitch, out slicePitch, out widthCount, out heightCount);
            if (Utilities.SizeOf(textureData) != (slicePitch * depth)) throw new ArgumentException("Invalid size for TextureData");

            return new DataBox(fixedPointer, rowPitch, slicePitch);
        }

        internal static TextureDescription CreateTextureDescriptionFromImage(Image image, TextureFlags flags, ResourceUsage usage)
        {
            var desc = (TextureDescription)image.Description;
            desc.BindFlags = BindFlags.ShaderResource;
            desc.Usage = usage;
            if ((flags & TextureFlags.UnorderedAccess) != 0)
                desc.Usage = ResourceUsage.Default;

            desc.BindFlags = GetBindFlagsFromTextureFlags(flags);

            desc.CpuAccessFlags = GetCpuAccessFlagsFromUsage(usage);
            return desc;
        }

        internal void GetViewSliceBounds(ViewType viewType, ref int arrayOrDepthIndex, ref int mipIndex, out int arrayOrDepthCount, out int mipCount)
        {
            int arrayOrDepthSize = this.Description.Depth > 1 ? this.Description.Depth : this.Description.ArraySize;

            switch (viewType)
            {
                case ViewType.Full:
                    arrayOrDepthIndex = 0;
                    mipIndex = 0;
                    arrayOrDepthCount = arrayOrDepthSize;
                    mipCount = this.Description.MipLevels;
                    break;
                case ViewType.Single:
                    arrayOrDepthCount = 1;
                    mipCount = 1;
                    break;
                case ViewType.MipBand:
                    arrayOrDepthCount = arrayOrDepthSize - arrayOrDepthIndex;
                    mipCount = 1;
                    break;
                case ViewType.ArrayBand:
                    arrayOrDepthCount = 1;
                    mipCount = Description.MipLevels - mipIndex;
                    break;
                default:
                    arrayOrDepthCount = 0;
                    mipCount = 0;
                    break;
            }
        }

        internal int GetViewCount()
        {
            int arrayOrDepthSize = this.Description.Depth > 1 ? this.Description.Depth : this.Description.ArraySize;
            return GetViewIndex((ViewType)4, arrayOrDepthSize, this.Description.MipLevels);
        }

        internal int GetViewIndex(ViewType viewType, int arrayOrDepthIndex, int mipIndex)
        {
            int arrayOrDepthSize = this.Description.Depth > 1 ? this.Description.Depth : this.Description.ArraySize;
            return (((int)viewType) * arrayOrDepthSize + arrayOrDepthIndex) * this.Description.MipLevels + mipIndex;
        }

        /// <summary>
        /// Called when name changed for this component.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Name")
            {
                if (GraphicsDevice.IsDebugMode)
                {
                    if (this.shaderResourceViews != null)
                    {
                        int i = 0;
                        foreach(var shaderResourceViewItem in shaderResourceViews)
                        {
                            var shaderResourceView = shaderResourceViewItem.Value;
                            if (shaderResourceView != null) shaderResourceView.View.DebugName = Name == null ? null : String.Format("{0} SRV[{1}]", i, Name);
                            i++;
                        }
                    }

                    if (this.renderTargetViews != null)
                    {
                        for (int i = 0; i < this.renderTargetViews.Length; i++)
                        {
                            var renderTargetView = this.renderTargetViews[i];
                            if (renderTargetView != null) renderTargetView.View.DebugName = Name == null ? null : String.Format("{0} RTV[{1}]", i, Name);
                        }
                    }

                    if (this.unorderedAccessViews != null)
                    {
                        for (int i = 0; i < this.unorderedAccessViews.Length; i++)
                        {
                            var unorderedAccessView = this.unorderedAccessViews[i];
                            if (unorderedAccessView != null) unorderedAccessView.DebugName = Name == null ? null : String.Format("{0} UAV[{1}]", i, Name);
                        }
                    }
                }
            }
        }

        private static bool IsPow2( int x )
        {
            return ((x != 0) && (x & (x - 1)) == 0);
        }

        private static int CountMips(int width)
        {
            int mipLevels = 1;

            while (width > 1)
            {
                ++mipLevels;

                if (width > 1)
                    width >>= 1;
            }

            return mipLevels;
        }

        private static int CountMips(int width, int height)
        {
            int mipLevels = 1;

            while (height > 1 || width > 1)
            {
                ++mipLevels;

                if (height > 1)
                    height >>= 1;

                if (width > 1)
                    width >>= 1;
            }

            return mipLevels;
        }

        private static int CountMips(int width, int height, int depth)
        {
            int mipLevels = 1;

            while (height > 1 || width > 1 || depth > 1)
            {
                ++mipLevels;

                if (height > 1)
                    height >>= 1;

                if (width > 1)
                    width >>= 1;

                if (depth > 1)
                    depth >>= 1;
            }

            return mipLevels;
        }

        public int CompareTo(Texture obj)
        {
            return textureId.CompareTo(obj.textureId);
        }

        internal static BindFlags GetBindFlagsFromTextureFlags(TextureFlags flags)
        {
            var bindFlags = BindFlags.None;
            if ((flags & TextureFlags.ShaderResource) != 0)
                bindFlags |= BindFlags.ShaderResource;

            if ((flags & TextureFlags.UnorderedAccess) != 0)
                bindFlags |= BindFlags.UnorderedAccess;

            if ((flags & TextureFlags.RenderTarget) != 0)
                bindFlags |= BindFlags.RenderTarget;

            return bindFlags;
        }

        internal struct TextureViewKey : IEquatable<TextureViewKey>
        {
            public TextureViewKey(Format viewFormat, ViewType viewType, int arrayOrDepthSlice, int mipIndex)
            {
                ViewFormat = viewFormat;
                ViewType = viewType;
                ArrayOrDepthSlice = arrayOrDepthSlice;
                MipIndex = mipIndex;
            }

            public readonly DXGI.Format ViewFormat;

            public readonly ViewType ViewType;

            public readonly int ArrayOrDepthSlice;

            public readonly int MipIndex;

            public bool Equals(TextureViewKey other)
            {
                return ViewFormat == other.ViewFormat && ViewType == other.ViewType && ArrayOrDepthSlice == other.ArrayOrDepthSlice && MipIndex == other.MipIndex;
            }

            public override bool Equals(object obj)
            {
                if(ReferenceEquals(null, obj)) return false;
                return obj is TextureViewKey && Equals((TextureViewKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (int)ViewFormat;
                    hashCode = (hashCode * 397) ^ (int)ViewType;
                    hashCode = (hashCode * 397) ^ ArrayOrDepthSlice;
                    hashCode = (hashCode * 397) ^ MipIndex;
                    return hashCode;
                }
            }

            public static bool operator ==(TextureViewKey left, TextureViewKey right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(TextureViewKey left, TextureViewKey right)
            {
                return !left.Equals(right);
            }
        }
    }
}