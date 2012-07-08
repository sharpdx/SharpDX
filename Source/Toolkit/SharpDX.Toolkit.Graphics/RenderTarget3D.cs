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
    /// A RenderTarget3D frontend to <see cref="SharpDX.Direct3D11.Texture3D"/>.
    /// </summary>
    /// <remarks>
    /// This class instantiates a <see cref="Texture3D"/> with the binding flags <see cref="BindFlags.RenderTarget"/>.
    /// This class is also castable to <see cref="RenderTargetView"/>.
    /// </remarks>
    public class RenderTarget3D : Texture3DBase
    {
        internal RenderTarget3D(Texture3DDescription description) : base(description)
        {
        }

        internal RenderTarget3D(GraphicsDevice device, Texture3DDescription description3D)
            : base(device, description3D)
        {
        }

        internal RenderTarget3D(Direct3D11.Texture3D texture)
            : base(texture)
        {
        }

        internal RenderTarget3D(GraphicsDevice device, Direct3D11.Texture3D texture)
            : base(device, texture)
        {
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator RenderTargetView(RenderTarget3D from)
        {
            return from == null ? null : from.RenderTargetViews != null ? from.RenderTargetViews[0] : null;
        }

        protected override void InitializeViews()
        {
            // Perform default initialization
            base.InitializeViews();

            if ((this.Description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                RenderTargetViews = new RenderTargetView[GetViewCount()];
                GetRenderTargetView(ViewType.Full, 0, 0);
            }
        }

        public override RenderTargetView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipIndex)
        {
            if ((this.Description.BindFlags & BindFlags.RenderTarget) == 0)
                return null;

            if (viewType == ViewType.MipBand)
                throw new NotSupportedException("ViewSlice.MipBand is not supported for render targets");

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewType, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var rtvIndex = GetViewIndex(viewType, arrayOrDepthSlice, mipIndex);

            lock (RenderTargetViews)
            {
                var rtv = RenderTargetViews[rtvIndex];

                // Creates the shader resource view
                if (rtv == null)
                {
                    // Create the render target view
                    var rtvDescription = new RenderTargetViewDescription()
                                             {
                                                 Format = this.Description.Format,
                                                 Dimension = RenderTargetViewDimension.Texture3D,
                                                 Texture3D =
                                                 {
                                                     DepthSliceCount = arrayCount,
                                                     FirstDepthSlice = arrayOrDepthSlice,
                                                     MipSlice = mipIndex,
                                                 }
                                             };

                    rtv = new RenderTargetView(GraphicsDevice, Resource, rtvDescription);
                    RenderTargetViews[rtvIndex] = ToDispose(rtv);
                }
                return rtv;
            }
        }

        public override Texture Clone()
        {
            return new RenderTarget3D(GraphicsDevice, this.Description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D"/> from a <see cref="Texture3DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget3D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static RenderTarget3D New(Texture3DDescription description)
        {
            return new RenderTarget3D(description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D"/> from a <see cref="Direct3D11.Texture3D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture3D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget3D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>	
        public static RenderTarget3D New(Direct3D11.Texture3D texture)
        {
            return new RenderTarget3D(texture);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D" /> with a single mipmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 3D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget3D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>
        public static RenderTarget3D New(int width, int height, int depth,  PixelFormat format, bool isUnorderedReadWrite = false, int arraySize = 1)
        {
            return New(width, height, depth, false, format, isUnorderedReadWrite, arraySize);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget3D" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="isUnorderedReadWrite">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 3D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget3D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture3D([In] const D3D11_TEXTURE3D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture3D** ppTexture3D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture3D</unmanaged-short>
        public static RenderTarget3D New(int width, int height, int depth, MipMap mipCount, PixelFormat format, bool isUnorderedReadWrite = false, int arraySize = 1)
        {
            return new RenderTarget3D(NewRenderTargetDescription(width, height, depth, format, isUnorderedReadWrite, mipCount));
        }

        protected static Texture3DDescription NewRenderTargetDescription(int width, int height, int depth, PixelFormat format, bool isReadWrite, int mipCount)
        {
            var desc = Texture3DBase.NewDescription(width, height, depth, format, isReadWrite, mipCount, ResourceUsage.Default);
            desc.BindFlags |= BindFlags.RenderTarget;
            return desc;
        }
    }
}