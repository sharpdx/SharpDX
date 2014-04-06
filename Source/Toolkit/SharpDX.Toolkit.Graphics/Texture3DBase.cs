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

using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Abstract class front end to <see cref="SharpDX.Direct3D11.Texture3D"/>.
    /// </summary>
    public abstract class Texture3DBase : Texture
    {
        protected readonly new Direct3D11.Texture3D Resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3DBase" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description3D">The description.</param>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        protected internal Texture3DBase(GraphicsDevice device, Texture3DDescription description3D)
            : base(device, description3D)
        {
            Resource = new Direct3D11.Texture3D(device, description3D);
            Initialize(Resource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3DBase" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description3D">The description.</param>
        /// <param name="dataRectangles">A variable-length parameters list containing data rectangles.</param>
        /// <msdn-id>ff476522</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        protected internal Texture3DBase(GraphicsDevice device, Texture3DDescription description3D, DataBox[] dataRectangles)
            : base(device, description3D)
        {
            Resource = new Direct3D11.Texture3D(device, description3D, dataRectangles);
            Initialize(Resource);
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
            return new Texture3D(this.GraphicsDevice, this.Description.ToStagingDescription());
        }

        internal override TextureView GetShaderResourceView(Format viewFormat, ViewType viewType, int arrayOrDepthSlice, int mipIndex)
        {
            if ((this.Description.BindFlags & BindFlags.ShaderResource) == 0)
                return null;

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewType, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var textureViewKey = new TextureViewKey(viewFormat, viewType, arrayOrDepthSlice, mipIndex);

            lock (this.shaderResourceViews)
            {
                TextureView srv;
                // Creates the shader resource view
                if (!shaderResourceViews.TryGetValue(textureViewKey, out srv))
                {
                    // Create the view
                    var srvDescription = new ShaderResourceViewDescription {
                        Format = viewFormat,
                        Dimension = ShaderResourceViewDimension.Texture3D,
                        Texture3D = {
                            MipLevels = mipCount,
                            MostDetailedMip = mipIndex
                        }
                    };

                    srv = new TextureView(this, new ShaderResourceView(this.GraphicsDevice, this.Resource, srvDescription));
                    this.shaderResourceViews.Add(textureViewKey, ToDispose(srv));
                }

                return srv;
            }
        }

        internal override UnorderedAccessView GetUnorderedAccessView(int zSlice, int mipIndex)
        {
            if ((this.Description.BindFlags & BindFlags.UnorderedAccess) == 0)
                return null;

            int sliceCount = 1;

            // Use Full although we are binding to a single array/mimap slice, just to get the correct index
            var uavIndex = GetViewIndex(ViewType.Full, zSlice, mipIndex);

            lock (this.unorderedAccessViews)
            {
                var uav = this.unorderedAccessViews[uavIndex];

                // Creates the unordered access view
                if (uav == null)
                {
                    var uavDescription = new UnorderedAccessViewDescription() {
                        Format = this.Description.Format,
                        Dimension = UnorderedAccessViewDimension.Texture3D,
                        Texture3D = {
                            FirstWSlice = zSlice,
                            MipSlice = mipIndex,
                            WSize = sliceCount
                        }
                    };

                    uav = new UnorderedAccessView(GraphicsDevice, Resource, uavDescription) { Tag = this };
                    this.unorderedAccessViews[uavIndex] = ToDispose(uav);
                }

                return uav;
            }
        }

        protected override void InitializeViews()
        {
            // Creates the shader resource view
            if ((this.Description.BindFlags & BindFlags.ShaderResource) != 0)
            {
                shaderResourceViews = new Dictionary<TextureViewKey, TextureView>();

                // Pre initialize by default the view on the first array/mipmap
                var viewFormat = Format;
                if (!FormatHelper.IsTypeless(viewFormat))
                {
                    // Only valid for non-typeless viewformat
                    defaultShaderResourceView = GetShaderResourceView(viewFormat, ViewType.Full, 0, 0);
                }
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

        protected static Texture3DDescription NewDescription(int width, int height, int depth, PixelFormat format, TextureFlags textureFlags, int mipCount, ResourceUsage usage)
        {
            if ((textureFlags & TextureFlags.UnorderedAccess) != 0)
                usage = ResourceUsage.Default;
            
            var desc = new Texture3DDescription()
                           {
                               Width = width,
                               Height = height,
                               Depth = depth,
                               BindFlags = GetBindFlagsFromTextureFlags(textureFlags),
                               Format = format,
                               MipLevels = CalculateMipMapCount(mipCount, width, height, depth),
                               Usage = usage,
                               CpuAccessFlags = GetCpuAccessFlagsFromUsage(usage),
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