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

using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    using Direct3D11;

    /// <summary>
    /// Graphics presenter for SwapChain.
    /// </summary>
    public class RenderTargetGraphicsPresenter : GraphicsPresenter
    {
        private Texture2DDescription renderTargetDescription;
        private readonly bool allowFormatChange;
        private readonly bool allowRecreateBackBuffer;
        private RenderTarget2D backBuffer;

        /// <summary>Initializes a new instance of the <see cref="RenderTargetGraphicsPresenter"/> class.</summary>
        /// <param name="device">The device.</param>
        /// <param name="renderTargetDescription">The render target description.</param>
        /// <param name="depthFormat">The depth format.</param>
        /// <param name="allowFormatChange">if set to <see langword="true" /> [allow format change].</param>
        /// <param name="disposeRenderTarget">if set to <see langword="true" /> [dispose render target].</param>
        public RenderTargetGraphicsPresenter(GraphicsDevice device, Texture2DDescription renderTargetDescription, DepthFormat depthFormat = DepthFormat.None, bool allowFormatChange = true, bool disposeRenderTarget = false)
            : base(device, CreatePresentationParameters(renderTargetDescription, depthFormat))
        {
            PresentInterval = Description.PresentationInterval;

            this.renderTargetDescription = renderTargetDescription;
            this.allowFormatChange = allowFormatChange;

            allowRecreateBackBuffer = true;

            backBuffer = RenderTarget2D.New(device, renderTargetDescription);

            if (disposeRenderTarget)
                ToDispose(backBuffer);
        }

        /// <summary>Initializes a new instance of the <see cref="RenderTargetGraphicsPresenter"/> class.</summary>
        /// <param name="device">The device.</param>
        /// <param name="backBuffer">The back buffer.</param>
        /// <param name="depthFormat">The depth format.</param>
        /// <param name="disposeRenderTarget">if set to <see langword="true" /> [dispose render target].</param>
        public RenderTargetGraphicsPresenter(GraphicsDevice device, RenderTarget2D backBuffer, DepthFormat depthFormat = DepthFormat.None, bool disposeRenderTarget = false)
            : base(device, CreatePresentationParameters(backBuffer, depthFormat))
        {
            PresentInterval = Description.PresentationInterval;

            this.backBuffer = backBuffer;

            if (disposeRenderTarget)
                ToDispose(this.backBuffer);
        }

        private static PresentationParameters CreatePresentationParameters(Texture2DDescription renderTargetDescription, DepthFormat depthFormat)
        {
            return new PresentationParameters()
                {
                    BackBufferWidth = renderTargetDescription.Width,
                    BackBufferHeight = renderTargetDescription.Height,
                    BackBufferFormat = renderTargetDescription.Format,
                    DepthStencilFormat = depthFormat,
                    DeviceWindowHandle = renderTargetDescription,
                    Flags = SwapChainFlags.None,
                    IsFullScreen = true,
                    MultiSampleCount = MSAALevel.None,
                    PresentationInterval = PresentInterval.One,
                    RefreshRate = new Rational(60, 1),
                    RenderTargetUsage = Usage.RenderTargetOutput
                };
        }

        private static PresentationParameters CreatePresentationParameters(RenderTarget2D renderTarget2D, DepthFormat depthFormat)
        {
            return new PresentationParameters()
            {
                BackBufferWidth = renderTarget2D.Width,
                BackBufferHeight = renderTarget2D.Height,
                BackBufferFormat = renderTarget2D.Description.Format,
                DepthStencilFormat = depthFormat,
                DeviceWindowHandle = renderTarget2D,
                Flags = SwapChainFlags.None,
                IsFullScreen = true,
                MultiSampleCount = MSAALevel.None,
                PresentationInterval = PresentInterval.One,
                RefreshRate = new Rational(60, 1),
                RenderTargetUsage = Usage.RenderTargetOutput
            };
        }

        /// <summary>Gets the default back buffer for this presenter.</summary>
        /// <value>The back buffer.</value>
        public override RenderTarget2D BackBuffer
        {
            get
            {
                return backBuffer;
            }
        }

        /// <summary>Gets the underlying native presenter (can be a <see cref="SharpDX.DXGI.SwapChain" /> or <see cref="SharpDX.DXGI.SwapChain1" /> or null, depending on the platform).</summary>
        /// <value>The native presenter.</value>
        public override object NativePresenter
        {
            get
            {
                return backBuffer;
            }
        }

        /// <summary>Gets or sets fullscreen mode for this presenter.</summary>
        /// <value><c>true</c> if this instance is full screen; otherwise, <c>false</c>.</value>
        /// <msdn-id>bb174579</msdn-id>
        ///   <unmanaged>HRESULT IDXGISwapChain::SetFullscreenState([In] BOOL Fullscreen,[In, Optional] IDXGIOutput* pTarget)</unmanaged>
        ///   <unmanaged-short>IDXGISwapChain::SetFullscreenState</unmanaged-short>
        /// <remarks>This method is only valid on Windows Desktop and has no effect on Windows Metro.</remarks>
        public override bool IsFullScreen
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        /// <summary>Sets the back buffer.</summary>
        /// <param name="backBuffer">The back buffer.</param>
        public void SetBackBuffer(RenderTarget2D backBuffer)
        {
            this.backBuffer = backBuffer;
        }

        /// <summary>Presents the Backbuffer to the screen.</summary>
        /// <msdn-id>bb174576</msdn-id>
        ///   <unmanaged>HRESULT IDXGISwapChain::Present([In] unsigned int SyncInterval,[In] DXGI_PRESENT_FLAGS Flags)</unmanaged>
        ///   <unmanaged-short>IDXGISwapChain::Present</unmanaged-short>
        public override void Present()
        {
#if !WP8
            GraphicsDevice.Flush();
#endif
        }

        /// <summary>Resizes the current presenter, by resizing the back buffer and the depth stencil buffer.</summary>
        /// <param name="width">New backbuffer width</param>
        /// <param name="height">New backbuffer height</param>
        /// <param name="format">Backbuffer display format.</param>
        /// <returns><c>true</c> if the presenter was resized, <c>false</c> otherwise</returns>
        public override bool Resize(int width, int height, Format format)
        {
            if (!base.Resize(width, height, format)) return false;

            // backbuffer was set externally, do not touch it
            if (!allowRecreateBackBuffer) return false;
            
            renderTargetDescription.Width = width;
            renderTargetDescription.Height = height;

            if (allowFormatChange)
                renderTargetDescription.Format = format;

            if (backBuffer != null)
            {
                RemoveAndDispose(ref backBuffer);
                backBuffer = RenderTarget2D.New(GraphicsDevice, renderTargetDescription);
            }
            return true;
        }
    }
}