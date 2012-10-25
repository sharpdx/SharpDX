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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a frontend to <see cref="SwapChain" /> and <see cref="SwapChain1" />.
    /// </summary>
    /// <remarks>
    /// In order to create a new <see cref="GraphicsPresenter"/>, a <see cref="GraphicsDevice"/> should have been initialized first.
    /// </remarks>
    /// <msdn-id>bb174569</msdn-id>	
    /// <unmanaged>IDXGISwapChain</unmanaged>	
    /// <unmanaged-short>IDXGISwapChain</unmanaged-short>	
    public abstract class GraphicsPresenter : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="presentationParameters"> </param>
        protected GraphicsPresenter(GraphicsDevice device, PresentationParameters presentationParameters)
        {
            GraphicsDevice = device.MainDevice;
            Description = presentationParameters.Clone();
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
        /// Gets the default back buffer for this presenter.
        /// </summary>
        public abstract RenderTarget2D BackBuffer { get; }

        /// <summary>
        /// Gets the underlying native presenter (can be a <see cref="SharpDX.DXGI.SwapChain"/> or <see cref="SharpDX.DXGI.SwapChain1"/> or null, depending on the platform).
        /// </summary>
        /// <value>The native presenter.</value>
        public abstract object NativePresenter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="presentationParameters">The presentation parameters </param>
        /// <returns>A new instance of the <see cref="GraphicsPresenter" /> class.</returns>
        public static GraphicsPresenter New(GraphicsDevice device, PresentationParameters presentationParameters)
        {
#if WIN8METRO
            throw new NotImplementedException(); 
#else
            return new GraphicsPresenterForSwapChain(device, presentationParameters);
#endif
        }

        /// <summary>
        /// Gets or sets fullscreen mode for this presenter.
        /// </summary>
        /// <remarks>
        /// This method is only valid on Windows Desktop and has no effect on Windows Metro.
        /// </remarks>
        /// <msdn-id>bb174579</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::SetFullscreenState([In] BOOL Fullscreen,[In, Optional] IDXGIOutput* pTarget)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::SetFullscreenState</unmanaged-short>	
        public abstract bool IsFullScreen { get; set; }

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
    }
}