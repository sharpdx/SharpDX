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
using System.IO;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.IO;
using DeviceChild = SharpDX.Direct3D11.DeviceChild;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base class for texture resources.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="N:SharpDX.Direct3D11"/> texture resource.</typeparam>
    public abstract class Texture : GraphicsResource
    {
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

        internal ShaderResourceView[] shaderResourceViews;
        internal RenderTargetView[] renderTargetViews;
        internal UnorderedAccessView[] unorderedAccessViews;
        private MipMapDescription[] mipmapDescriptions;

        protected Texture(TextureDescription description)
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

        protected override void Initialize(GraphicsDevice deviceArg, DeviceChild resource)
        {
            base.Initialize(deviceArg, resource);
            InitializeViews();
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
        /// <remarks>This method is used to allocated a texture data buffer to hold pixel datas: var textureData = new T[ texture.CalculatePixelCount&lt;T&gt;() ] ;.</remarks>
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
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public abstract Texture ToStaging();

        /// <summary>
        /// Gets a specific <see cref="ShaderResourceView" /> from this texture.
        /// </summary>
        /// <param name="viewType">Type of the view slice.</param>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipIndex">The mip map slice index.</param>
        /// <returns>An <see cref="ShaderResourceView" /></returns>
        internal abstract ShaderResourceView GetShaderResourceView(ViewType viewType, int arrayOrDepthSlice, int mipIndex);

        /// <summary>
        /// Gets a specific <see cref="RenderTargetView" /> from this texture.
        /// </summary>
        /// <param name="viewType">Type of the view slice.</param>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipMapSlice">The mip map slice index.</param>
        /// <returns>An <see cref="RenderTargetView" /></returns>
        internal abstract RenderTargetView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipMapSlice);

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
            return @from == null ? null : @from.shaderResourceViews != null ? @from.shaderResourceViews[0] : null;
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
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="isUnorderedReadWrite">True to load the texture with unordered access enabled. Default is false.</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <returns>A texture</returns>
        public static Texture Load(Stream stream, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable)
        {
            var image = Image.Load(stream);
            try
            {

                switch (image.Description.Dimension)
                {
                    case TextureDimension.Texture1D:
                        return Texture1D.New(image, isUnorderedReadWrite, usage);
                    case TextureDimension.Texture2D:
                        return Texture2D.New(image, isUnorderedReadWrite, usage);
                    case TextureDimension.Texture3D:
                        return Texture3D.New(image, isUnorderedReadWrite, usage);
                    case TextureDimension.TextureCube:
                        return TextureCube.New(image, isUnorderedReadWrite, usage);
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
        /// <param name="filePath">The file to load the texture from.</param>
        /// <param name="isUnorderedReadWrite">True to load the texture with unordered access enabled. Default is false.</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <returns>A texture</returns>
        public static Texture Load(string filePath, bool isUnorderedReadWrite = false, ResourceUsage usage = ResourceUsage.Immutable)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(stream, isUnorderedReadWrite, usage);
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
        /// Saves this texture to a stream with a specified format.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="stagingTexture">The staging texture used to temporary transfer the image from the GPU to CPU.</param>
        /// <param name="fileType">Type of the image file.</param>
        /// <exception cref="ArgumentException">If stagingTexture is not a staging texture.</exception>
        public void Save(Stream stream, Texture stagingTexture, ImageFileType fileType)
        {
            if (stagingTexture.Description.Usage != ResourceUsage.Staging)
                throw new ArgumentException("Invalid texture used as staging. Must have Usage = ResourceUsage.Staging", "stagingTexture");

            var image = Image.New(stagingTexture.Description);

            try
            {
                for (int arrayIndex = 0; arrayIndex < image.Description.ArraySize; arrayIndex++)
                {
                    for (int mipLevel = 0; mipLevel < image.Description.MipLevels; mipLevel++)
                    {
                        var pixelBuffer = image.PixelBuffer[arrayIndex, mipLevel];
                        GraphicsDevice.GetContent(this, stagingTexture, new DataPointer(pixelBuffer.DataPointer, pixelBuffer.BufferStride), arrayIndex, mipLevel);
                    }
                }

                image.Save(stream, fileType);
            } finally
            {
                image.Dispose();
            }
        }

        /// <summary>
        /// Saves this texture to a file with a specified format.
        /// </summary>
        /// <param name="filePath">The filepath to save the texture to.</param>
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
        /// <param name="filePath">The filepath to save the texture to.</param>
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

        protected static DataBox GetDataBox<T>(Format format, int width, int height, T[] textureData, IntPtr fixedPointer) where T : struct
        {
            // Check that the textureData size is correct
            if (textureData == null) throw new ArgumentNullException("textureData");
            int rowPitch;
            int slicePitch;
            int widthCount;
            int heightCount;
            Image.ComputePitch(format, width, height, out rowPitch, out slicePitch, out widthCount, out heightCount);
            if (Utilities.SizeOf(textureData) != slicePitch) throw new ArgumentException("Invalid size for TextureData");

            return new DataBox(fixedPointer, rowPitch, slicePitch);
        }

        internal static TextureDescription CreateTextureDescriptionFromImage(Image image, bool isUnorderedReadWrite, ResourceUsage usage)
        {
            var desc = (TextureDescription)image.Description;
            desc.BindFlags = BindFlags.ShaderResource;
            desc.Usage = usage;
            if (isUnorderedReadWrite)
            {
                desc.Usage = ResourceUsage.Default;
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            desc.CpuAccessFlags = GetCputAccessFlagsFromUsage(usage);
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

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                if (this.shaderResourceViews != null)
                {
                    for (int i = 0; i < this.shaderResourceViews.Length; i++)
                    {
                        var shaderResourceView = this.shaderResourceViews[i];
                        if (shaderResourceView != null)
                            shaderResourceView.DebugName = Name == null ? null : String.Format("{0} SRV[{1}]", i, Name);
                    }
                }

                if (this.renderTargetViews != null)
                {
                    for (int i = 0; i < this.renderTargetViews.Length; i++)
                    {
                        var renderTargetView = this.renderTargetViews[i];
                        if (renderTargetView != null)
                            renderTargetView.DebugName = Name == null ? null : String.Format("{0} RTV[{1}]", i, Name);
                    }
                }

                if (this.unorderedAccessViews != null)
                {
                    for (int i = 0; i < this.unorderedAccessViews.Length; i++)
                    {
                        var unorderedAccessView = this.unorderedAccessViews[i];
                        if (unorderedAccessView != null)
                            unorderedAccessView.DebugName = Name == null ? null : String.Format("{0} UAV[{1}]", i, Name);
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
    }
}