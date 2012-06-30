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
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Abstract class frontend to <see cref="SharpDX.Direct3D11.Texture3D"/>.
    /// </summary>
    public abstract class Texture3DBase : Texture
    {
        protected readonly new Direct3D11.Texture3D Resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3DBase" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="dataRectangles">A variable-length parameters list containing data rectangles.</param>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        protected internal Texture3DBase(Texture3DDescription description, params DataBox[] dataRectangles)
            : this(GraphicsDevice.Current, description, dataRectangles)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3DBase" /> class.
        /// </summary>
        /// <param name="device">The device local.</param>
        /// <param name="description">The description.</param>
        /// <param name="dataRectangles">A variable-length parameters list containing data rectangles.</param>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        protected internal Texture3DBase(GraphicsDevice device, Texture3DDescription description, params DataBox[] dataRectangles)
        {
            Description = description;
            // Precalculates the stride
            RowStride = Description.Width * ((PixelFormat)Description.Format).SizeInBytes;
            DepthStride = RowStride * Description.Height;
            Resource = new Direct3D11.Texture3D(device, description, dataRectangles);
            Initialize(device, Resource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3DBase" /> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        protected internal Texture3DBase(Direct3D11.Texture3D texture)
            : this(GraphicsDevice.Current, texture)
        {
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="texture">The texture.</param>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        protected internal Texture3DBase(GraphicsDevice device, Direct3D11.Texture3D texture)
        {
            Description = texture.Description;
            RowStride = Description.Width * ((PixelFormat)Description.Format).SizeInBytes;
            DepthStride = RowStride * Description.Height;
            Resource = texture;
            Initialize(device, Resource);
        }

        /// <summary>
        /// The description of this <see cref="Texture3DBase"/>.
        /// </summary>
        public readonly Texture3DDescription Description;

        /// <summary>
        /// The stride - number of bytes per row - in bytes.
        /// </summary>
        public readonly int RowStride;

        /// <summary>
        /// The stride - number of bytes per row - in bytes.
        /// </summary>
        public readonly int DepthStride;

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public abstract Texture3DBase Clone();

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public T Clone<T>() where T : Texture3DBase
        {
            return (T)Clone();
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public Texture3D ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new Texture3D(this.GraphicsDevice, stagingDesc);            
        }

        public override ShaderResourceView GetShaderResourceView(ViewSlice viewSlice, int arrayOrDepthSlice, int mipIndex)
        {
            if ((Description.BindFlags & BindFlags.ShaderResource) == 0)
                return null;

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewSlice, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var srvIndex = GetViewIndex(viewSlice, arrayOrDepthSlice, mipIndex);

            lock (ShaderResourceViews)
            {
                var srv = ShaderResourceViews[srvIndex];

                // Creates the shader resource view
                if (srv == null)
                {
                    // Create the view
                    var srvDescription = new ShaderResourceViewDescription {
                        Format = Description.Format,
                        Dimension = ShaderResourceViewDimension.Texture3D,
                        Texture3D = {
                            MipLevels = mipCount,
                            MostDetailedMip = mipIndex
                        }
                    };

                    srv = new ShaderResourceView(this.GraphicsDevice, this.Resource, srvDescription);
                    ShaderResourceViews[srvIndex] = ToDispose(srv);
                }
                return srv;
            }
        }

        public override UnorderedAccessView GetUnorderedAccessView(int zSlice, int mipIndex)
        {
            if ((Description.BindFlags & BindFlags.UnorderedAccess) == 0)
                return null;

            int sliceCount;
            int mipCount;
            GetViewSliceBounds(ViewSlice.Single, ref zSlice, ref mipIndex, out sliceCount, out mipCount);

            var uavIndex = GetViewIndex(ViewSlice.Single, zSlice, mipIndex);

            lock (UnorderedAccessViews)
            {
                var uav = UnorderedAccessViews[uavIndex];

                // Creates the unordered access view
                if (uav == null)
                {
                    var uavDescription = new UnorderedAccessViewDescription() {
                        Format = Description.Format,
                        Dimension = UnorderedAccessViewDimension.Texture3D,
                        Texture3D = {
                            FirstWSlice = zSlice,
                            MipSlice = mipIndex,
                            WSize = sliceCount
                        }
                    };

                    uav = new UnorderedAccessView(GraphicsDevice, Resource, uavDescription);
                    UnorderedAccessViews[uavIndex] = ToDispose(uav);
                }
                return uav;
            }
        }

        protected override void InitializeViews()
        {
            base.InitializeViews();

            // Creates the shader resource view
            if ((Description.BindFlags & BindFlags.ShaderResource) != 0)
            {
                ShaderResourceViews = new ShaderResourceView[GetViewCount()];

                // Pre initialize by default the view on the first array/mipmap
                GetShaderResourceView(ViewSlice.Full, 0, 0);
            }

            // Creates the unordered access view
            if ((Description.BindFlags & BindFlags.UnorderedAccess) != 0)
            {
                // Initialize the unordered access views
                UnorderedAccessViews = new UnorderedAccessView[GetViewCount()];

                // Pre initialize by default the view on the first array/mipmap
                GetUnorderedAccessView(0, 0);
            }
        }
        
        protected void GetViewSliceBounds(ViewSlice viewSlice, ref int zSliceIndex, ref int mipIndex, out int zCount, out int mipCount)
        {
            switch (viewSlice)
            {
                case ViewSlice.Full:
                    zSliceIndex = 0;
                    mipIndex = 0;
                    zCount = Description.Depth;
                    mipCount = Description.MipLevels;
                    break;
                case ViewSlice.Single:
                    zCount = 1;
                    mipCount = 1;
                    break;
                case ViewSlice.ArrayBand:
                    zCount = Description.Depth - zSliceIndex;
                    mipCount = 1;
                    break;
                case ViewSlice.MipBand:
                    zCount = 1;
                    mipCount = Description.MipLevels - mipIndex;
                    break;
                default:
                    zCount = 0;
                    mipCount = 0;
                    break;
            }
        }

        protected int GetViewCount()
        {
            return GetViewIndex((ViewSlice)4, Description.Depth, Description.MipLevels);
        }

        protected int GetViewIndex(ViewSlice viewSlice, int zSliceIndex, int mipIndex)
        {
            return (((int)viewSlice) * Description.Depth + zSliceIndex) * Description.MipLevels + mipIndex;
        }

        protected static DataBox[] Pin<T>(int width, int height, PixelFormat format, T[][] initialTextures, out GCHandle[] handles) where T : struct
        {
            var dataRectangles = new DataBox[initialTextures.Length];
            handles = new GCHandle[initialTextures.Length];
            for (int i = 0; i < initialTextures.Length; i++)
            {
                var initialTexture = initialTextures[i];
                var handle = GCHandle.Alloc(initialTexture, GCHandleType.Pinned);
                handles[i] = handle;
                dataRectangles[i].DataPointer = handle.AddrOfPinnedObject();
                dataRectangles[i].RowPitch = width * format.SizeInBytes;
                dataRectangles[i].SlicePitch = width * height * format.SizeInBytes;
            }
            return dataRectangles;
        }

        protected static Texture3DDescription NewDescription(int width, int height, int depth, PixelFormat format, bool isReadWrite, int mipCount, ResourceUsage usage)
        {
            var desc = new Texture3DDescription()
                           {
                               Width = width,
                               Height = height,
                               Depth = depth,
                               BindFlags = BindFlags.ShaderResource,
                               Format = format,
                               MipLevels = mipCount,
                               Usage = usage,
                               CpuAccessFlags = GetCputAccessFlagsFromUsage(usage),
                               OptionFlags = ResourceOptionFlags.None
                           };

            if (isReadWrite)
            {
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            return desc;
        }
    }
}