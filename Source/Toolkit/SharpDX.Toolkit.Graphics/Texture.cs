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
using SharpDX.Direct3D11;

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
        /// The width stride in bytes (number of bytes per row).
        /// </summary>
        internal readonly int RowStride;

        /// <summary>
        /// The depth stride in bytes (number of bytes per depth slice).
        /// </summary>
        internal readonly int DepthStride;

        protected ShaderResourceView[] ShaderResourceViews;
        protected RenderTargetView[] RenderTargetViews;
        protected UnorderedAccessView[] UnorderedAccessViews;

        protected Texture(TextureDescription description)
        {
            Description = description;
            RowStride = this.Description.Width * ((PixelFormat)this.Description.Format).SizeInBytes;
            DepthStride = RowStride * this.Description.Height;
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
        /// Calculates the size of the mip.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="mipLevel">The mip level.</param>
        /// <returns>Returns ceiling(size / (2 ^ mipLevel)) </returns>
        internal static int CalculateMipSize(int size, int mipLevel)
        {
            return (int)Math.Ceiling((double)size / (1 << mipLevel));            
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
            if (Math.Abs(width - (int)width) > double.Epsilon)
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
        public abstract ShaderResourceView GetShaderResourceView(ViewType viewType, int arrayOrDepthSlice, int mipIndex);

        /// <summary>
        /// Gets a specific <see cref="RenderTargetView" /> from this texture.
        /// </summary>
        /// <param name="viewType">Type of the view slice.</param>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipMapSlice">The mip map slice index.</param>
        /// <returns>An <see cref="RenderTargetView" /></returns>
        public abstract RenderTargetView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipMapSlice);

        /// <summary>
        /// Gets a specific <see cref="UnorderedAccessView"/> from this texture.
        /// </summary>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipMapSlice">The mip map slice index.</param>
        /// <returns>An <see cref="UnorderedAccessView"/></returns>
        public abstract UnorderedAccessView GetUnorderedAccessView(int arrayOrDepthSlice, int mipMapSlice);

        /// <summary>
        /// ShaderResourceView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator ShaderResourceView(Texture from)
        {
            return from == null ? null : from.ShaderResourceViews != null ? from.ShaderResourceViews[0] : null;
        }

        /// <summary>
        /// UnorderedAccessView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator UnorderedAccessView(Texture from)
        {
            return from == null ? null : from.UnorderedAccessViews != null ? from.UnorderedAccessViews[0] : null;
        }

        /// <summary>
        /// Calculates the mip map count from a requested level.
        /// </summary>
        /// <param name="requestedLevel">The requested level.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <returns>The resulting mipmap count (clamp to [1, maxMipMapCount] for this texture)</returns>
        internal static int CalculateMipMapCount(MipMap requestedLevel, int width, int height = 0, int depth = 0)
        {
            int size = Math.Max(Math.Max(width, height), depth);
            int maxMipMap = 1 + (int)Math.Ceiling(Math.Log(size) / Math.Log(2.0));

            return requestedLevel  == 0 ? maxMipMap : Math.Min(requestedLevel, maxMipMap);
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
                case ViewType.ArrayBand:
                    arrayOrDepthCount = arrayOrDepthSize - arrayOrDepthIndex;
                    mipCount = 1;
                    break;
                case ViewType.MipBand:
                    arrayOrDepthCount = 1;
                    mipCount = arrayOrDepthSize - mipIndex;
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
                if (ShaderResourceViews != null)
                {
                    for (int i = 0; i < ShaderResourceViews.Length; i++)
                    {
                        var shaderResourceView = ShaderResourceViews[i];
                        if (shaderResourceView != null)
                            shaderResourceView.DebugName = Name == null ? null : string.Format("{0} SRV[{1}]", i, Name);
                    }
                }

                if (RenderTargetViews != null)
                {
                    for (int i = 0; i < RenderTargetViews.Length; i++)
                    {
                        var renderTargetView = RenderTargetViews[i];
                        if (renderTargetView != null)
                            renderTargetView.DebugName = Name == null ? null : string.Format("{0} RTV[{1}]", i, Name);
                    }
                }

                if (UnorderedAccessViews != null)
                {
                    for (int i = 0; i < UnorderedAccessViews.Length; i++)
                    {
                        var unorderedAccessView = UnorderedAccessViews[i];
                        if (unorderedAccessView != null)
                            unorderedAccessView.DebugName = Name == null ? null : string.Format("{0} UAV[{1}]", i, Name);
                    }
                }
            }
        }
    }
}