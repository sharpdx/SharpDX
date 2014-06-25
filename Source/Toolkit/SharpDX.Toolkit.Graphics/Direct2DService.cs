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

#if DIRECTX11_1 && !WP8

namespace SharpDX.Toolkit.Graphics
{
    using System;
    using Direct2D1;
    using Factory1 = DirectWrite.Factory1;

    /// <summary>
    /// Provides a service that offers Direct2D and DirectWrite contexts.
    /// </summary>
    public sealed class Direct2DService : Component, IDirect2DService
    {
        private readonly IGraphicsDeviceService graphicsDeviceService;
        private readonly DebugLevel debugLevel;
        private GraphicsDevice graphicsDeviceCache;
        private Device device;
        private DeviceContext deviceContext;
        private Factory1 directWriteFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="Direct2DService" />, subscribes to <see cref="GraphicsDevice" /> changes
        /// events via
        /// <see cref="IGraphicsDeviceService" />.
        /// </summary>
        /// <param name="graphicsDeviceService">The service responsible for <see cref="GraphicsDevice" /> management.</param>
        /// <param name="debugLevel">The debug level used when creating D2D device.</param>
        /// <exception cref="ArgumentNullException">Then either <paramref name="graphicsDeviceService" /> is null.</exception>
        public Direct2DService(IGraphicsDeviceService graphicsDeviceService, DebugLevel debugLevel = DebugLevel.Error)
        {
            if (graphicsDeviceService == null) throw new ArgumentNullException("graphicsDeviceService");
            this.graphicsDeviceService = graphicsDeviceService;
            this.debugLevel = debugLevel;

            graphicsDeviceService.DeviceCreated += GraphicsDeviceServiceOnDeviceCreated;
            graphicsDeviceService.DeviceDisposing += GraphicsDeviceServiceOnDeviceDisposing;
            graphicsDeviceService.DeviceChangeBegin += GraphicsDeviceServiceOnDeviceChangeBegin;
            graphicsDeviceService.DeviceChangeEnd += GraphicsDeviceServiceOnDeviceChangeEnd;
            graphicsDeviceService.DeviceLost += GraphicsDeviceServiceOnDeviceLost;
        }

        /// <summary>
        /// Gets a reference to the Direct2D device.
        /// </summary>
        public Device Device { get { return device; } }

        /// <summary>
        /// Gets a reference to the default <see cref="Direct2D1.DeviceContext" />.
        /// </summary>
        public DeviceContext DeviceContext { get { return deviceContext; } }

        /// <summary>
        /// Gets a reference to the default <see cref="SharpDX.DirectWrite.Factory1" />.
        /// </summary>
        public Factory1 DirectWriteFactory { get { return directWriteFactory; } }

        /// <summary>
        /// Diposes all resources associated with the current <see cref="Direct2DService" /> instance.
        /// </summary>
        /// <param name="disposeManagedResources">Indicates whether to dispose management resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            DisposeAll();
        }

        private void GraphicsDeviceServiceOnDeviceChangeBegin(object sender, EventArgs e)
        {
        }

        private void GraphicsDeviceServiceOnDeviceChangeEnd(object sender, EventArgs e)
        {
            CreateOrUpdateDirect2D();
        }

        private void GraphicsDeviceServiceOnDeviceLost(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the <see cref="IGraphicsDeviceService.DeviceCreated" /> event.
        /// Initializes the <see cref="Direct2DService.Device" /> and <see cref="DeviceContext" />.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void GraphicsDeviceServiceOnDeviceCreated(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the <see cref="IGraphicsDeviceService.DeviceDisposing" /> event.
        /// Disposes the <see cref="Direct2DService.Device" />, <see cref="DeviceContext" /> and its render target
        /// associated with the current <see cref="Direct2DService" /> instance.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void GraphicsDeviceServiceOnDeviceDisposing(object sender, EventArgs e)
        {
            DisposeAll();
        }

        private void CreateOrUpdateDirect2D()
        {
            // Dispose and recreate all devices only if the GraphicsDevice changed
            if (graphicsDeviceCache != graphicsDeviceService.GraphicsDevice)
            {
                graphicsDeviceCache = graphicsDeviceService.GraphicsDevice;

                DisposeAll();

                var d3dDevice = (Direct3D11.Device)graphicsDeviceCache;
                using (var dxgiDevice = d3dDevice.QueryInterface<DXGI.Device>())
                {
                    device = ToDispose(new Device(dxgiDevice, new CreationProperties { DebugLevel = debugLevel }));
                    deviceContext = ToDispose(new DeviceContext(this.device, DeviceContextOptions.None));
                }

                directWriteFactory = ToDispose(new Factory1());
            }
        }

        /// <summary>
        /// Disposes the <see cref="Direct2DService.Device" />, <see cref="DeviceContext" /> and its render target
        /// associated with the current <see cref="Direct2DService" /> instance.
        /// </summary>
        private void DisposeAll()
        {
            if(deviceContext != null)
                deviceContext.Target = null;

            RemoveAndDispose(ref directWriteFactory);
            RemoveAndDispose(ref deviceContext);
            RemoveAndDispose(ref device);
        }
    }
}

#endif