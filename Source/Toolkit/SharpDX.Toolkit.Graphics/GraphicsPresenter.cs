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
using System.Collections.Generic;
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
    public class GraphicsPresenter : Component
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly SwapChain swapChain;

        /// <summary>
        /// Gets the description of this presenter.
        /// </summary>
        public readonly PresentationParameters Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="swapChain">The swap chain.</param>
        /// <param name="presentationParameters"> </param>
        private GraphicsPresenter(GraphicsDevice device, SwapChain swapChain, PresentationParameters presentationParameters)
        {
            device = device.MainDevice;
            this.graphicsDevice = device;
            this.swapChain = ToDispose(swapChain);
            this.Description = presentationParameters.Clone();
            this.PresentInterval = PresentInterval.Default;

            // TODO handle multiple backbuffers
            BackBuffer = RenderTarget2D.New(device, swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));

            ApplySwapChainDescription();
        }

        private void ApplySwapChainDescription()
        {
            var swapChainDesc = swapChain.Description;
            Description.BackBufferWidth = swapChainDesc.ModeDescription.Width;
            Description.BackBufferHeight = swapChainDesc.ModeDescription.Height;
            Description.BackBufferFormat = swapChainDesc.ModeDescription.Format;
            Description.RefreshRate = swapChainDesc.ModeDescription.RefreshRate;

            if (Description.IsFullScreen)
                IsFullScreen = Description.IsFullScreen;
        }

        /// <summary>
        /// Gets the default back buffer for this presenter.
        /// </summary>
        public readonly RenderTarget2D BackBuffer;

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
            return NewForDesktop(device, presentationParameters);
#endif
        }

#if !WIN8METRO
        private static GraphicsPresenter NewForDesktop(GraphicsDevice graphicsDevice, PresentationParameters presentationParameters)
        {
            int width = presentationParameters.BackBufferWidth;
            int height = presentationParameters.BackBufferHeight;
            PixelFormat pixelFormat = presentationParameters.BackBufferFormat;
            Rational refreshRate = presentationParameters.RefreshRate;
            SharpDX.DXGI.Usage usage = presentationParameters.RenderTargetUsage;
            System.IntPtr windowHandle;

            // Check for Window Handle parameter
            if (presentationParameters.DeviceWindowHandle == null)
                throw new ArgumentException("DeviceWindowHandle cannot be null");

            var deviceWindowHandle = presentationParameters.DeviceWindowHandle;

            if (deviceWindowHandle is GraphicsWindow)
            {
                deviceWindowHandle = ((GraphicsWindow) deviceWindowHandle).BackControl;
            }

            if (deviceWindowHandle is IntPtr)
            {
                windowHandle = (IntPtr)deviceWindowHandle;
            }
            else if (deviceWindowHandle is System.Windows.Forms.Control)
            {
                var control = (System.Windows.Forms.Control)deviceWindowHandle;
                control.ClientSize = new System.Drawing.Size(width, height);
                windowHandle = control.Handle;
            }
            else
            {
                throw new NotSupportedException(string.Format("DeviceWindowHandle of type [{0}] is not supported. Only Window Handle IntPtr or WinForm Control", deviceWindowHandle.GetType().Name));
            }

            // By default, use 60Hz for displaying in full screen
            if (refreshRate == Rational.Empty)
                refreshRate = new Rational(60, 1);

            var graphicsAdapter = graphicsDevice.Adapter ?? GraphicsAdapter.Default;

            // Get display mode for the particular width, height, pixelformat
            var selectedModes = new List<DisplayMode>();
            for (int i = 0; i < graphicsAdapter.SupportedDisplayModes.Length; i++)
            {
                var displayMode = graphicsAdapter.SupportedDisplayModes[i];
                if (displayMode.Width == width && displayMode.Height == height && displayMode.Format == pixelFormat)
                    selectedModes.Add(displayMode);
            }

            if (selectedModes.Count == 0 && presentationParameters.IsFullScreen)
                throw new NotSupportedException("Resolution is not supported by the graphics adapter.");

            // Calculate the closest / best refresh rate
            var refreshRateExpected = (float)refreshRate.Numerator / refreshRate.Denominator;
            var bestRefreshRateDiff = float.MaxValue;
            foreach (var selectedMode in selectedModes)
            {
                float refreshRateDiff = Math.Abs((float) selectedMode.RefreshRate.Numerator / selectedMode.RefreshRate.Denominator - refreshRateExpected);
                if (refreshRateDiff < bestRefreshRateDiff)
                {
                    refreshRate = selectedMode.RefreshRate;
                    bestRefreshRateDiff = refreshRateDiff;
                }
            }

            var description = new SwapChainDescription()
            {
                ModeDescription = new ModeDescription(width, height, refreshRate, pixelFormat),
                BufferCount = 1,
                OutputHandle = windowHandle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = usage,
                IsWindowed = true,
                Flags = SwapChainFlags.None,
            };

            var graphicsPresenter = new GraphicsPresenter(graphicsDevice, new SwapChain(GraphicsAdapter.Factory, (Direct3D11.Device)graphicsDevice, description), presentationParameters);
            return graphicsPresenter;
        }
#endif

        /// <summary>
        /// Gets or sets fullscreen mode for this presenter.
        /// </summary>
        /// <remarks>
        /// This method is only valid on Windows Desktop and has no effect on Windows Metro.
        /// </remarks>
        /// <msdn-id>bb174579</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::SetFullscreenState([In] BOOL Fullscreen,[In, Optional] IDXGIOutput* pTarget)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::SetFullscreenState</unmanaged-short>	
        public bool IsFullScreen
        {
            get
            {
#if WIN8METRO
                return true;
#else
                return swapChain.IsFullScreen;
#endif
            }

            set {
#if WIN8METRO
#else
                swapChain.IsFullScreen = true;
#endif
            }
        }

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
        public void Present()
        {
            swapChain.Present((int)PresentInterval, PresentFlags.None);
        }

        /// <summary>
        /// Called when name changed for this component.
        /// </summary>
        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (graphicsDevice.IsDebugMode)
                this.swapChain.DebugName = Name;
        }

        public static implicit operator SwapChain(GraphicsPresenter from)
        {
            return from == null ? null : from.swapChain;
        }
    }
}