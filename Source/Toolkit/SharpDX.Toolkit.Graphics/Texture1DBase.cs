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

using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Abstract class front end to <see cref="SharpDX.Direct3D11.Texture1D"/>.
    /// </summary>
    public abstract class Texture1DBase : Texture
    {
        protected readonly new Direct3D11.Texture1D Resource;
        private DXGI.Surface dxgiSurface;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture1DBase" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description1D">The description.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        protected internal Texture1DBase(GraphicsDevice device, Texture1DDescription description1D)
            : base(device, description1D)
        {
            Resource = new Direct3D11.Texture1D(device, description1D);
            Initialize(Resource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture1DBase" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description1D">The description.</param>
        /// <param name="dataBox">A variable-length parameters list containing data rectangles.</param>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        protected internal Texture1DBase(GraphicsDevice device, Texture1DDescription description1D, DataBox[] dataBox)
            : base(device, description1D)
        {
            Resource = new Direct3D11.Texture1D(device, description1D, dataBox);
            Initialize(Resource);
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
            : base(device, texture.Description)
        {
            Resource = texture;
            Initialize(Resource);
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public override Texture ToStaging()
        {
            return new Texture1D(this.GraphicsDevice, this.Description.ToStagingDescription());
        }

        internal override ShaderResourceView GetShaderResourceView(ViewType viewType, int arrayOrDepthSlice, int mipIndex)
        {
            if ((this.Description.BindFlags & BindFlags.ShaderResource) == 0)
                return null;

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewType, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var srvIndex = GetViewIndex(viewType, arrayOrDepthSlice, mipIndex);

            lock (this.shaderResourceViews)
            {
                var srv = this.shaderResourceViews[srvIndex];

                // Creates the shader resource view
                if (srv == null)
                {
                    // Create the view
                    var srvDescription = new ShaderResourceViewDescription() { Format = this.Description.Format };

                    // Initialize for texture arrays or texture cube
                    if (this.Description.ArraySize > 1)
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
                        srvDescription.Texture1D.MipLevels = mipCount;
                        srvDescription.Texture1D.MostDetailedMip = mipIndex;
                    }

                    srv = new ShaderResourceView(this.GraphicsDevice, this.Resource, srvDescription);
                    this.shaderResourceViews[srvIndex] = ToDispose(srv);
                }

                // Associate this instance
                srv.Tag = this;

                return srv;
            }
        }

        internal override UnorderedAccessView GetUnorderedAccessView(int arrayOrDepthSlice, int mipIndex)
        {
            if ((this.Description.BindFlags & BindFlags.UnorderedAccess) == 0)
                return null;

            int arrayCount = 1;
            // Use Full although we are binding to a single array/mimap slice, just to get the correct index
            var uavIndex = GetViewIndex(ViewType.Full, arrayOrDepthSlice, mipIndex);

            lock (this.unorderedAccessViews)
            {
                var uav = this.unorderedAccessViews[uavIndex];

                // Creates the unordered access view
                if (uav == null)
                {
                    var uavDescription = new UnorderedAccessViewDescription() {
                        Format = this.Description.Format,
                        Dimension = this.Description.ArraySize > 1 ? UnorderedAccessViewDimension.Texture1DArray : UnorderedAccessViewDimension.Texture1D
                    };

                    if (this.Description.ArraySize > 1)
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
                    this.unorderedAccessViews[uavIndex] = ToDispose(uav);
                }

                // Associate this instance
                uav.Tag = this;

                return uav;
            }
        }

        /// <summary>
        /// <see cref="SharpDX.DXGI.Surface"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator SharpDX.DXGI.Surface(Texture1DBase from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.dxgiSurface ?? (from.dxgiSurface = from.ToDispose(from.Resource.QueryInterface<DXGI.Surface>()));
        }

        protected override void InitializeViews()
        {
            // Creates the shader resource view
            if ((this.Description.BindFlags & BindFlags.ShaderResource) != 0)
            {
                this.shaderResourceViews = new ShaderResourceView[GetViewCount()];

                // Pre initialize by default the view on the first array/mipmap
                GetShaderResourceView(ViewType.Full, 0, 0);
            }

            // Creates the unordered access view
            if ((this.Description.BindFlags & BindFlags.UnorderedAccess) != 0)
            {
                // Initialize the unordered access views
                this.unorderedAccessViews = new UnorderedAccessView[GetViewCount()];

                // Pre initialize by default the view on the first array/mipmap
                GetUnorderedAccessView(0, 0);
            }
        }

        protected static Texture1DDescription NewDescription(int width, PixelFormat format, TextureFlags textureFlags, int mipCount, int arraySize, ResourceUsage usage)
        {
            if ((textureFlags & TextureFlags.UnorderedAccess) != 0)
                usage = ResourceUsage.Default;

            var desc = new Texture1DDescription()
                           {
                               Width = width,
                               ArraySize = arraySize,
                               BindFlags = GetBindFlagsFromTextureFlags(textureFlags),
                               Format = format,
                               MipLevels = CalculateMipMapCount(mipCount, width),
                               Usage = usage,
                               CpuAccessFlags = GetCputAccessFlagsFromUsage(usage),
                               OptionFlags = ResourceOptionFlags.None
                           };

            // If the texture is a RenderTarget + ShaderResource + MipLevels > 1, then allow for GenerateMipMaps method
            if ((desc.BindFlags & BindFlags.RenderTarget) != 0 && (desc.BindFlags & BindFlags.ShaderResource) != 0 && desc.MipLevels > 1)
            {
                desc.OptionFlags |= ResourceOptionFlags.GenerateMipMaps;
            }

            return desc;
        }
    }
}