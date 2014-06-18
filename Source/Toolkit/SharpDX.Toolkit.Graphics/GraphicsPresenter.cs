// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a front end to <see cref="SwapChain" /> and <see cref="SwapChain1" />.
    /// </summary>
    /// <remarks>
    /// In order to create a new <see cref="GraphicsPresenter"/>, a <see cref="GraphicsDevice"/> should have been initialized first.
    /// </remarks>
    /// <msdn-id>bb174569</msdn-id>	
    /// <unmanaged>IDXGISwapChain</unmanaged>	
    /// <unmanaged-short>IDXGISwapChain</unmanaged-short>	
    public abstract class GraphicsPresenter : Component
    {
        private DepthStencilBuffer depthStencilBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="presentationParameters"> </param>
        protected GraphicsPresenter(GraphicsDevice device, PresentationParameters presentationParameters)
        {
            GraphicsDevice = device.MainDevice;
            Description = presentationParameters.Clone();

            DefaultViewport = new ViewportF(0, 0, Description.BackBufferWidth, Description.BackBufferHeight);

            // Creates a default DepthStencilBuffer.
            CreateDepthStencilBuffer();
        }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Gets the description of this presenter.
        /// </summary>
        public PresentationParameters Description { get; private set; }

        /// <summary>
        /// Default viewport that covers the whole presenter surface.
        /// </summary>
        public ViewportF DefaultViewport { get; protected set; }

        /// <summary>
        /// Gets the default back buffer for this presenter.
        /// </summary>
        public abstract RenderTarget2D BackBuffer { get; }

        /// <summary>
        /// Gets the default depth stencil buffer for this presenter.
        /// </summary>
        public DepthStencilBuffer DepthStencilBuffer
        {
            get
            {
                return depthStencilBuffer;
            }

            protected set
            {
                depthStencilBuffer = value;
            }
        }

        /// <summary>
        /// Gets the underlying native presenter (can be a <see cref="SharpDX.DXGI.SwapChain"/> or <see cref="SharpDX.DXGI.SwapChain1"/> or null, depending on the platform).
        /// </summary>
        /// <value>The native presenter.</value>
        public abstract object NativePresenter { get; }

        /// <summary>
        /// Gets or sets fullscreen mode for this presenter.
        /// </summary>
        /// <value><c>true</c> if this instance is full screen; otherwise, <c>false</c>.</value>
        /// <msdn-id>bb174579</msdn-id>
        ///   <unmanaged>HRESULT IDXGISwapChain::SetFullscreenState([In] BOOL Fullscreen,[In, Optional] IDXGIOutput* pTarget)</unmanaged>
        ///   <unmanaged-short>IDXGISwapChain::SetFullscreenState</unmanaged-short>
        /// <remarks>This method is only valid on Windows Desktop and has no effect on Windows Metro.</remarks>
        public abstract bool IsFullScreen { get; set; }

        /// <summary>
        /// Gets or sets the output index to use when switching to fullscreen mode.
        /// </summary>
        public int PrefferedFullScreenOutputIndex { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PresentInterval"/>. Default is to wait for one vertical blanking.
        /// </summary>
        /// <value>The present interval.</value>
        public PresentInterval PresentInterval
        {
            get { return Description.PresentationInterval; }
            set { Description.PresentationInterval = value; }
        }

        /// <summary>
        /// Presents the Backbuffer to the screen.
        /// </summary>
        /// <msdn-id>bb174576</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::Present([In] unsigned int SyncInterval,[In] DXGI_PRESENT_FLAGS Flags)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::Present</unmanaged-short>	
        public abstract void Present();

        /// <summary>
        /// Resizes the current presenter, by resizing the back buffer and the depth stencil buffer.
        /// </summary>
        /// <param name="width">New backbuffer width</param>
        /// <param name="height">New backbuffer height</param>
        /// <param name="format">Backbuffer display format.</param>
        /// <param name="refreshRate"></param>
        /// <returns><c>true</c> if the presenter was resized, <c>false</c> otherwise</returns>
        public virtual bool Resize(int width, int height, Format format, Rational? refreshRate = null)
        {
            if (Description.BackBufferWidth == width && Description.BackBufferHeight == height && Description.BackBufferFormat == format)
            {
                return false;
            }

            if (DepthStencilBuffer != null)
            {
                RemoveAndDispose(ref depthStencilBuffer);
            }

            Description.BackBufferWidth = width;
            Description.BackBufferHeight = height;
            Description.BackBufferFormat = format;
            if(refreshRate.HasValue)
            {
                Description.RefreshRate = refreshRate.Value;
            }

            DefaultViewport = new ViewportF(0, 0, Description.BackBufferWidth, Description.BackBufferHeight);

            CreateDepthStencilBuffer();
            return true;
        }

        /// <summary>
        /// Creates the depth stencil buffer.
        /// </summary>
        protected virtual void CreateDepthStencilBuffer()
        {
            // If no depth stencil buffer, just return
            if (Description.DepthStencilFormat == DepthFormat.None)
            {
                return;
            }

            // Creates the depth stencil buffer.
            DepthStencilBuffer =
                ToDispose(DepthStencilBuffer.New(GraphicsDevice,
                                                 Description.BackBufferWidth,
                                                 Description.BackBufferHeight,
                                                 Description.MultiSampleCount,
                                                 Description.DepthStencilFormat,
                                                 Description.DepthBufferShaderResource));
        }
    }
}