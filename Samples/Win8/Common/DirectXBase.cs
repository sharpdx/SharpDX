// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D;
using Windows.UI.Core;

namespace SharpDX.Win8
{
    /// <summary>
    /// This class is a straight conversion of the C++ DirectXBase class found 
    /// from Windows 8 Metro-Style App Samples.
    /// </summary>
    /// <remarks>
    /// The difference between the original samples is that the swap chain
    /// description can be overrided in a separate method, as well as the
    /// creation of the swap chain.
    /// </remarks>
    public abstract class DirectXBase : Component
    {
        protected CoreWindow window;

        // Declare Direct2D Objects
        protected SharpDX.Direct2D1.Factory1           d2dFactory;
        protected SharpDX.Direct2D1.Device             d2dDevice;
        protected SharpDX.Direct2D1.DeviceContext      d2dContext;
        protected SharpDX.Direct2D1.Bitmap1            d2dTargetBitmap;

        // Declare DirectWrite & Windows Imaging Component Objects
        protected SharpDX.DirectWrite.Factory         dwriteFactory;
        protected SharpDX.WIC.ImagingFactory2          wicFactory;

        // Direct3D Objects
        protected SharpDX.Direct3D11.Device1           d3dDevice;
        protected SharpDX.Direct3D11.DeviceContext1    d3dContext;
        protected SharpDX.DXGI.SwapChain1              swapChain;
        protected SharpDX.Direct3D11.RenderTargetView  renderTargetView;
        protected SharpDX.Direct3D11.DepthStencilView  depthStencilView;

        protected FeatureLevel featureLevel;
        protected Windows.Foundation.Size renderTargetSize;
        protected Windows.Foundation.Rect windowBounds;
        protected float dpi;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="window">Window to receive the rendering</param>
        /// <param name="dpi">dpi used for the rendering</param>
        public virtual void Initialize(CoreWindow window, float dpi) {
            this.window = window;

            CreateDeviceIndependentResources();
            CreateDeviceResources();
            Dpi = dpi;
        }

        /// <summary>
        /// Creates device independent resources.
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        protected virtual void CreateDeviceIndependentResources() {
#if DEBUG
            var debugLevel = SharpDX.Direct2D1.DebugLevel.Information;
#else
            var debugLevel = SharpDX.Direct2D1.DebugLevel.None;
#endif
            d2dFactory = ToDispose(new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded, debugLevel));
            dwriteFactory = ToDispose(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared));
            wicFactory = ToDispose(new SharpDX.WIC.ImagingFactory2());
        }

        /// <summary>
        /// Creates device resources. 
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        protected virtual void CreateDeviceResources()
        {
            // Enable compatibility with Direct2D
            // Retrieve the Direct3D 11.1 device amd device context
            using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport))
                d3dDevice = defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>();
            featureLevel = d3dDevice.FeatureLevel;

            // Get Direct3D 11.1 context
            d3dContext = ToDispose(d3dDevice.ImmediateContext.QueryInterface<SharpDX.Direct3D11.DeviceContext1>());

            // Create Direct2D device
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
                d2dDevice = ToDispose(new SharpDX.Direct2D1.Device(dxgiDevice));

            // Create Direct2D context
            d2dContext = ToDispose(new SharpDX.Direct2D1.DeviceContext(d2dDevice, SharpDX.Direct2D1.DeviceContextOptions.None));

            // Release swap chain
            ComObject.Dispose(ref swapChain);
        }

        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        public virtual float Dpi
        {
            get
            {
                return dpi;
            }
            set
            {
                if (dpi != value)
                {
                    dpi = value;
                    d2dContext.DotsPerInch = new SharpDX.DrawingSizeF(dpi, dpi);
                    UpdateForWindowSizeChange();
                }
            }
        }

        /// <summary>
        /// This method must be called when a resize occured and window
        /// size dependent resources must be re-created.
        /// </summary>
        public virtual void UpdateForWindowSizeChange()
        {
            if (window.Bounds.Width != windowBounds.Width ||
                window.Bounds.Height != windowBounds.Height)
            {
                d2dContext.Target = null;
                ComObject.Dispose(ref d2dTargetBitmap);
                ComObject.Dispose(ref renderTargetView);
                ComObject.Dispose(ref depthStencilView);
                CreateWindowSizeDependentResources();
            }
        }

        /// <summary>
        /// Creates windows size dependent resources.
        /// </summary>
        /// <remarks>
        /// This method is called by <see cref="UpdateForWindowSizeChange"/>.
        /// </remarks>
        protected virtual void CreateWindowSizeDependentResources()
        {
            // Store the window bounds so the next time we get a SizeChanged event we can
            // avoid rebuilding everything if the size is identical.
            windowBounds = window.Bounds;

            // If the swap chain already exists, resize it.
            if (swapChain != null)
            {
                swapChain.ResizeBuffers(2, 0, 0, SharpDX.DXGI.Format.B8G8R8A8_UNorm, 0);
            }
            // Otherwise, create a new one.
            else
            {
                // SwapChain description
                var desc = CreateSwapChainDescription();

                // Once the desired swap chain description is configured, it must be created on the same adapter as our D3D Device

                // First, retrieve the underlying DXGI Device from the D3D Device.
                // Creates the swap chain 
                using (var dxgiDevice2 = d3dDevice.QueryInterface<SharpDX.DXGI.Device2>())
                using (var dxgiAdapter = dxgiDevice2.Adapter)
                using (var dxgiFactory2 = dxgiAdapter.GetParent<SharpDX.DXGI.Factory2>())
                {
                    swapChain = CreateSwapChain(dxgiFactory2, d3dDevice, desc);

                    // Ensure that DXGI does not queue more than one frame at a time. This both reduces 
                    // latency and ensures that the application will only render after each VSync, minimizing 
                    // power consumption.
                    dxgiDevice2.MaximumFrameLatency = 1;
                }
            }

            // Obtain the backbuffer for this window which will be the final 3D rendertarget.
            using (var backBuffer = SharpDX.Direct3D11.Texture2D.FromSwapChain<SharpDX.Direct3D11.Texture2D>(swapChain, 0))
            {
                // Create a view interface on the rendertarget to use on bind.
                renderTargetView = new SharpDX.Direct3D11.RenderTargetView(d3dDevice, backBuffer);

                // Cache the rendertarget dimensions in our helper class for convenient use.
                var backBufferDesc = backBuffer.Description;
                renderTargetSize.Width = backBufferDesc.Width;
                renderTargetSize.Height = backBufferDesc.Height;
            }

            // Create a descriptor for the depth/stencil buffer.
            // Allocate a 2-D surface as the depth/stencil buffer.
            // Create a DepthStencil view on this surface to use on bind.
            using (var depthBuffer = new SharpDX.Direct3D11.Texture2D(d3dDevice, new SharpDX.Direct3D11.Texture2DDescription()
            {
                Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = (int)renderTargetSize.Width,
                Height = (int)renderTargetSize.Height,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
            }))
                depthStencilView = new SharpDX.Direct3D11.DepthStencilView(d3dDevice, depthBuffer, new SharpDX.Direct3D11.DepthStencilViewDescription() { Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D });

            // Create a viewport descriptor of the full window size.
            var viewport = new SharpDX.Direct3D11.Viewport(0, 0, (float)renderTargetSize.Width, (float)renderTargetSize.Height, 0.0f, 1.0f);

            // Set the current viewport using the descriptor.
            d3dContext.Rasterizer.SetViewports(viewport);

            // Now we set up the Direct2D render target bitmap linked to the swapchain. 
            // Whenever we render to this bitmap, it will be directly rendered to the 
            // swapchain associated with the window.
            var bitmapProperties = new SharpDX.Direct2D1.BitmapProperties1(
                new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                dpi,
                dpi,
                SharpDX.Direct2D1.BitmapOptions.Target | SharpDX.Direct2D1.BitmapOptions.CannotDraw);

            // Direct2D needs the dxgi version of the backbuffer surface pointer.
            // Get a D2D surface from the DXGI back buffer to use as the D2D render target.
            using (var dxgiBAckBuffer = swapChain.GetBackBuffer<SharpDX.DXGI.Surface>(0))
                d2dTargetBitmap = new SharpDX.Direct2D1.Bitmap1(d2dContext, dxgiBAckBuffer, bitmapProperties);

            // So now we can set the Direct2D render target.
            d2dContext.Target = d2dTargetBitmap;

            // Set D2D text anti-alias mode to Grayscale to ensure proper rendering of text on intermediate surfaces.
            d2dContext.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
        }

        /// <summary>
        /// Creates the swap chain description.
        /// </summary>
        /// <returns>A swap chain description</returns>
        /// <remarks>
        /// This method can be overloaded in order to modify default parameters.
        /// </remarks>
        protected virtual SharpDX.DXGI.SwapChainDescription1 CreateSwapChainDescription()
        {
            // SwapChain description
            var desc = new SharpDX.DXGI.SwapChainDescription1()
            {
                // Automatic sizing
                Width = 0,
                Height = 0,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = false,
                // Use two buffers to enable flip effect.
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                BufferCount = 2,
                Scaling = SharpDX.DXGI.Scaling.None,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
            };
            return desc;
        }

        /// <summary>
        /// Creates the swap chain.
        /// </summary>
        /// <param name="factory">The DXGI factory</param>
        /// <param name="device">The D3D11 device</param>
        /// <param name="desc">The swap chain description</param>
        /// <returns>An instance of swap chain</returns>
        protected virtual SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            using (var comWindow = new ComObject(window))
                return factory.CreateSwapChainForCoreWindow(device, comWindow, ref desc, null);
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        /// <remarks>
        /// This method must be called on each frame.
        /// </remarks>
        public abstract void Render();

        /// <summary>
        /// Present the results to the swap chain.
        /// </summary>
        public virtual void Present()
        {
            // The application may optionally specify "dirty" or "scroll" rects to improve efficiency
            // in certain scenarios.  In this sample, however, we do not utilize those features.
            var parameters = new SharpDX.DXGI.PresentParameters();

            try
            {
                // The first argument instructs DXGI to block until VSync, putting the application
                // to sleep until the next VSync. This ensures we don't waste any cycles rendering
                // frames that will never be displayed to the screen.
                swapChain.Present(1, SharpDX.DXGI.PresentFlags.None, parameters);
            }
            catch (SharpDX.SharpDXException ex)
            {
                // If the device was removed either by a disconnect or a driver upgrade, we 
                // must completely reinitialize the renderer.
                if (ex.ResultCode == SharpDX.DXGI.DXGIError.DeviceRemoved
                    || ex.ResultCode == SharpDX.DXGI.DXGIError.DeviceReset)
                    Initialize(window, dpi);
                else
                    throw;
            }
        }
    }
}
