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

using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A DepthStencilBuffer frontend to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    /// <remarks>
    /// This class instantiates a <see cref="Texture2D"/> with the binding flags <see cref="BindFlags.DepthStencil"/>.
    /// </remarks>
    public class DepthStencilBuffer : Texture2DBase
    {
        internal readonly DXGI.Format DefaultViewFormat;
        private DepthStencilView depthStencilView;

        /// <summary>
        /// Gets the <see cref="Graphics.DepthFormat"/> of this depth stencil buffer.
        /// </summary>
        public readonly DepthFormat DepthFormat;

        /// <summary>
        /// Gets a boolean value indicating if this buffer is supporting stencil.
        /// </summary>
        public readonly bool HasStencil;

        /// <summary>
        /// Gets a boolean value indicating if this buffer is supporting read-only view.
        /// </summary>
        public readonly bool HasReadOnlyView;

        /// <summary>
        /// Gets a a read-only <see cref="DepthStencilView"/>.
        /// </summary>
        /// <remarks>
        /// This value can be null if not supported by hardware (minimum features level is 11.0)
        /// </remarks>
        public readonly DepthStencilView ReadOnlyView;

        internal DepthStencilBuffer(Texture2DDescription description, DepthFormat format) 
            : this(GraphicsDevice.Current, description, format)

        {
        }

        internal DepthStencilBuffer(GraphicsDevice device, Texture2DDescription description2D, DepthFormat depthFormat)
            : base(device, description2D)
        {
            DepthFormat = depthFormat;
            DefaultViewFormat = ComputeViewFormat(DepthFormat, out HasStencil);
            HasReadOnlyView = InitializeViewsDelayed(out ReadOnlyView);
        }

        internal DepthStencilBuffer(Direct3D11.Texture2D texture, DepthFormat format)
            : this(GraphicsDevice.Current, texture, format)
        {
        }

        internal DepthStencilBuffer(GraphicsDevice device, Direct3D11.Texture2D texture, DepthFormat depthFormat)
            : base(device, texture)
        {
            DepthFormat = depthFormat;
            DefaultViewFormat = ComputeViewFormat(DepthFormat, out HasStencil);
            HasReadOnlyView = InitializeViewsDelayed(out ReadOnlyView);
        }

        protected override void InitializeViews()
        {
            // Override this, because we need the DepthFormat setup in order to initialize this class
            // This is caused by a bad design of the constructors/initialize sequence. 
            // TODO: Fix this problem
        }

        protected bool InitializeViewsDelayed(out DepthStencilView readOnlyView)
        {
            bool hasReadOnlyView = false;
            readOnlyView = null;

            // Perform default initialization
            base.InitializeViews();         
   
            if ((Description.BindFlags & BindFlags.DepthStencil) == 0)
                return hasReadOnlyView;

            // Create a Depth stencil view on this texture2D
            var depthStencilViewDescription = new SharpDX.Direct3D11.DepthStencilViewDescription
                                                  {
                                                      Format = (Format)DepthFormat,
                                                      Flags = SharpDX.Direct3D11.DepthStencilViewFlags.None,
                                                      Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D,
                                                      Texture2D = { MipSlice = 0 }
                                                  };
            if (Description.SampleDescription.Count > 1)
                depthStencilViewDescription.Dimension = DepthStencilViewDimension.Texture2DMultisampled;

            // Create the Depth Stencil View
            depthStencilView = new SharpDX.Direct3D11.DepthStencilView(GraphicsDevice, Resource, depthStencilViewDescription);

            // ReadOnly for feature level Direct3D11
            if (GraphicsDevice.Features.Level >= FeatureLevel.Level_11_0)
            {
                // Create a Depth stencil view on this texture2D
                depthStencilViewDescription.Flags = DepthStencilViewFlags.ReadOnlyDepth;
                if (HasStencil)
                    depthStencilViewDescription.Flags |= DepthStencilViewFlags.ReadOnlyStencil;

                readOnlyView = new SharpDX.Direct3D11.DepthStencilView(GraphicsDevice, Resource, depthStencilViewDescription);
                hasReadOnlyView = true;
            }

            return hasReadOnlyView;
        }

        protected override Format GetDefaultViewFormat()
        {
            return DefaultViewFormat;
        }

        /// <summary>
        /// DepthStencilView casting operator.
        /// </summary>
        /// <param name="buffer">Source for the.</param>
        public static implicit operator DepthStencilView(DepthStencilBuffer buffer)
        {
            return buffer == null ? null : buffer.depthStencilView;
        }

        public override RenderTargetView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipIndex)
        {
            throw new NotSupportedException("DepthStencilBuffer doesn't support rendert target view");
        }

        public override Texture Clone()
        {
            return new DepthStencilBuffer(GraphicsDevice, this.Description, DepthFormat);
        }

        /// <summary>
        /// Creates a new <see cref="DepthStencilBuffer"/> from a <see cref="Texture2DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="DepthStencilBuffer"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static DepthStencilBuffer New(Texture2DDescription description)
        {
            return new DepthStencilBuffer(description, ComputeViewFormat(description.Format));
        }

        /// <summary>
        /// Creates a new <see cref="DepthStencilBuffer"/> from a <see cref="Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="DepthStencilBuffer"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static DepthStencilBuffer New(Direct3D11.Texture2D texture)
        {
            return new DepthStencilBuffer(texture, ComputeViewFormat(texture.Description.Format));
        }

        /// <summary>
        /// Creates a new <see cref="DepthStencilBuffer" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="DepthStencilBuffer" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static DepthStencilBuffer New(int width, int height, DepthFormat format, int arraySize = 1)
        {
            return new DepthStencilBuffer(NewDepthStencilBufferDescription(width, height, format, MSAALevel.None, arraySize), format);
        }

        /// <summary>
        /// Creates a new <see cref="DepthStencilBuffer" /> using multisampling.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="multiSampleCount">The multisample count.</param>
        /// <returns>A new instance of <see cref="DepthStencilBuffer" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static DepthStencilBuffer New(int width, int height, MSAALevel multiSampleCount, DepthFormat format, int arraySize = 1)
        {
            return new DepthStencilBuffer(NewDepthStencilBufferDescription(width, height, format, multiSampleCount, arraySize), format);
        }

        protected static Texture2DDescription NewDepthStencilBufferDescription(int width, int height, DepthFormat format, MSAALevel multiSampleCount, int arraySize)
        {
            var desc = Texture2DBase.NewDescription(width, height, DXGI.Format.Unknown, false, 1, arraySize, ResourceUsage.Default);
            desc.BindFlags |= BindFlags.DepthStencil;
            // Sets the MSAALevel
            int maximumMSAA = (int)GraphicsDevice.Current.Features[(DXGI.Format)format].MSAALevelMax;
            desc.SampleDescription.Count = Math.Max(1, Math.Min((int)multiSampleCount, maximumMSAA));

            var typelessDepthFormat = SharpDX.DXGI.Format.Unknown;

            // Determine TypeLess Format and ShaderResourceView Format
            switch (format)
            {
                case DepthFormat.Depth16:
                    typelessDepthFormat = SharpDX.DXGI.Format.R16_Typeless;
                    break;
                case DepthFormat.Depth32:
                    typelessDepthFormat = SharpDX.DXGI.Format.R32_Typeless;
                    break;
                case DepthFormat.Depth24Stencil8:
                    typelessDepthFormat = SharpDX.DXGI.Format.R24G8_Typeless;
                    break;
                case DepthFormat.Depth32Stencil8X24:
                    typelessDepthFormat = SharpDX.DXGI.Format.R32G8X24_Typeless;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unsupported DepthFormat [{0}] for depth buffer", format));
            }

            desc.Format = typelessDepthFormat;

            return desc;
        }

        private static DXGI.Format ComputeViewFormat(DepthFormat format, out bool hasStencil)
        {
            DXGI.Format viewFormat;
            hasStencil = false;

            // Determine TypeLess Format and ShaderResourceView Format
            switch (format)
            {
                case DepthFormat.Depth16:
                    viewFormat = SharpDX.DXGI.Format.R16_Float;
                    break;
                case DepthFormat.Depth32:
                    viewFormat = SharpDX.DXGI.Format.R32_Float;
                    break;
                case DepthFormat.Depth24Stencil8:
                    viewFormat = SharpDX.DXGI.Format.R24_UNorm_X8_Typeless;
                    hasStencil = true;
                    break;
                case DepthFormat.Depth32Stencil8X24:
                    viewFormat = SharpDX.DXGI.Format.R32_Float_X8X24_Typeless;
                    hasStencil = true;
                    break;
                default:
                    viewFormat = DXGI.Format.Unknown;
                    break;
            }

            return viewFormat;
        }


        private static DepthFormat ComputeViewFormat(DXGI.Format format)
        {
            switch (format)
            {
                case SharpDX.DXGI.Format.D16_UNorm:
                case DXGI.Format.R16_Float:
                    return DepthFormat.Depth16;

                case SharpDX.DXGI.Format.D32_Float:
                case DXGI.Format.R32_Float:
                    return DepthFormat.Depth32;

                case SharpDX.DXGI.Format.D24_UNorm_S8_UInt:
                case SharpDX.DXGI.Format.R24_UNorm_X8_Typeless:
                    return DepthFormat.Depth24Stencil8;

                case SharpDX.DXGI.Format.D32_Float_S8X24_UInt:
                case SharpDX.DXGI.Format.R32_Float_X8X24_Typeless:
                    return DepthFormat.Depth32Stencil8X24;
            }
            throw new InvalidOperationException(string.Format("Unsupported DXGI.FORMAT [{0}] for depth buffer", format));
        }
    }
}