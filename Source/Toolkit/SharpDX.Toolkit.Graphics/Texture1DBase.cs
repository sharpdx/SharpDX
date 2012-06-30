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
    /// Abstract class frontend to <see cref="SharpDX.Direct3D11.Texture1D"/>.
    /// </summary>
    public abstract class Texture1DBase : Texture
    {
        protected readonly new Direct3D11.Texture1D Resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture1DBase" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="dataRectangles">A variable-length parameters list containing data rectangles.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        protected internal Texture1DBase(Texture1DDescription description, params IntPtr[] dataRectangles)
            : this(GraphicsDevice.Current, description, dataRectangles)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture1DBase" /> class.
        /// </summary>
        /// <param name="device">The device local.</param>
        /// <param name="description">The description.</param>
        /// <param name="dataRectangles">A variable-length parameters list containing data rectangles.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        protected internal Texture1DBase(GraphicsDevice device, Texture1DDescription description, params IntPtr[] dataRectangles)
        {
            Description = description;
            Resource = new Direct3D11.Texture1D(device, description, dataRectangles);
            Initialize(device, Resource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture1DBase" /> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        protected internal Texture1DBase(Direct3D11.Texture1D texture)
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
        protected internal Texture1DBase(GraphicsDevice device, Direct3D11.Texture1D texture)
        {
            Description = texture.Description;
            Resource = texture;
            Initialize(device, Resource);
        }

        /// <summary>
        /// The description of this <see cref="Texture1DBase"/>.
        /// </summary>
        public readonly Texture1DDescription Description;

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public abstract Texture1DBase Clone();

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public T Clone<T>() where T : Texture1DBase
        {
            return (T)Clone();
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public Texture1D ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new Texture1D(this.GraphicsDevice, stagingDesc);            
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
                    var srvDescription = new ShaderResourceViewDescription() { Format = Description.Format };

                    // Initialize for texture arrays or texture cube
                    if (Description.ArraySize > 1)
                    {
                        // Else regular Texture1D
                        srvDescription.Dimension = ShaderResourceViewDimension.Texture1DArray;
                        srvDescription.Texture1DArray.ArraySize = arrayCount;
                        srvDescription.Texture1DArray.FirstArraySlice = arrayOrDepthSlice;
                        srvDescription.Texture1DArray.MipLevels = mipCount;
                        srvDescription.Texture1DArray.MostDetailedMip = mipIndex;
                    }
                    else
                    {
                        srvDescription.Dimension = ShaderResourceViewDimension.Texture1D;
                    }

                    srv = new ShaderResourceView(this.GraphicsDevice, this.Resource, srvDescription);
                    ShaderResourceViews[srvIndex] = ToDispose(srv);
                }
                return srv;
            }
        }

        public override UnorderedAccessView GetUnorderedAccessView(int arrayOrDepthSlice, int mipIndex)
        {
            if ((Description.BindFlags & BindFlags.UnorderedAccess) == 0)
                return null;

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(ViewSlice.Single, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var uavIndex = GetViewIndex(ViewSlice.Single, arrayOrDepthSlice, mipIndex);

            lock (UnorderedAccessViews)
            {
                var uav = UnorderedAccessViews[uavIndex];

                // Creates the unordered access view
                if (uav == null)
                {
                    var uavDescription = new UnorderedAccessViewDescription() {
                        Format = Description.Format,
                        Dimension = Description.ArraySize > 1 ? UnorderedAccessViewDimension.Texture1DArray : UnorderedAccessViewDimension.Texture1D
                    };

                    if (Description.ArraySize > 1)
                    {
                        uavDescription.Texture1DArray.ArraySize = arrayCount;
                        uavDescription.Texture1DArray.FirstArraySlice = arrayOrDepthSlice;
                        uavDescription.Texture1DArray.MipSlice = mipIndex;
                    }
                    else
                    {
                        uavDescription.Texture1D.MipSlice = mipIndex;
                    }

                    uav = new UnorderedAccessView(GraphicsDevice, Resource, uavDescription);
                    UnorderedAccessViews[uavIndex] = ToDispose(uav);
                }
                return uav;
            }
        }
        
        protected void GetViewSliceBounds(ViewSlice viewSlice, ref int arrayIndex, ref int mipIndex, out int arrayCount, out int mipCount)
        {
            switch (viewSlice)
            {
                case ViewSlice.Full:
                    arrayIndex = 0;
                    mipIndex = 0;
                    arrayCount = Description.ArraySize;
                    mipCount = Description.MipLevels;
                    break;
                case ViewSlice.Single:
                    arrayCount = 1;
                    mipCount = 1;
                    break;
                case ViewSlice.ArrayBand:
                    arrayCount = Description.ArraySize - arrayIndex;
                    mipCount = 1;
                    break;
                case ViewSlice.MipBand:
                    arrayCount = 1;
                    mipCount = Description.MipLevels - mipIndex;
                    break;
                default:
                    arrayCount = 0;
                    mipCount = 0;
                    break;
            }
        }

        protected int GetViewCount()
        {
            return GetViewIndex((ViewSlice)4, Description.ArraySize, Description.MipLevels);
        }

        protected int GetViewIndex(ViewSlice viewSlice, int arrayIndex, int mipIndex)
        {
            return (((int)viewSlice) * Description.ArraySize + arrayIndex) * Description.MipLevels + mipIndex;
        }
        
        protected static IntPtr[] Pin<T>(T[][] initialTextures, out GCHandle[] handles) where T : struct
        {
            var dataRectangles = new IntPtr[initialTextures.Length];
            handles = new GCHandle[initialTextures.Length];
            for (int i = 0; i < initialTextures.Length; i++)
            {
                var initialTexture = initialTextures[i];
                var handle = GCHandle.Alloc(initialTexture, GCHandleType.Pinned);
                handles[i] = handle;
                dataRectangles[i] = handle.AddrOfPinnedObject();
            }
            return dataRectangles;
        }

        protected static Texture1DDescription NewDescription(int width, PixelFormat format, bool isReadWrite, int mipCount, int arraySize, ResourceUsage usage)
        {
            var desc = new Texture1DDescription()
                           {
                               Width = width,
                               ArraySize = arraySize,
                               BindFlags = BindFlags.ShaderResource,
                               Format = format,
                               MipLevels = mipCount,
                               Usage = usage,
                               CpuAccessFlags = GetCputAccessFlagsFromUsage(usage)
                           };

            if (isReadWrite)
            {
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            return desc;
        }
    }
}