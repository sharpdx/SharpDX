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
    /// This class is a frontend to <see cref="SwapChain" /> and <see cref="SwapChain1" />
    /// </summary>
    public class GraphicsPresenter : Component
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly SwapChain swapChain;
        private readonly SwapChainDescription swapChainDescription;

        /// <summary>
        /// Gets the description of this presenter.
        /// </summary>
        public readonly PresentationParameters Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="swapChain">The swap chain.</param>
        private GraphicsPresenter(GraphicsDevice device, SwapChain swapChain)
        {
            this.graphicsDevice = device;
            this.swapChain = ToDispose(swapChain);
            this.swapChainDescription = swapChain.Description;
            this.Description = swapChainDescription;

            // TODO handle multiple backbuffers
            BackBuffer = RenderTarget2D.New(swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));
        }

        /// <summary>
        /// Gets the default back buffer for this presenter.
        /// </summary>
        public readonly RenderTarget2D BackBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="width">The width in pixel of the output.</param>
        /// <param name="height">The height in pixel of the output</param>
        /// <param name="pixelFormat">The pixel format</param>
        /// <param name="windowHandle">A handle to the window (HWND/form.Handle for Desktop, CoreWindow IUnknown pointer for Metro)</param>
        /// <param name="usage">Usage of this presenter</param>
        /// <returns>A new instance of the <see cref="GraphicsPresenter" /> class.</returns>
        public static GraphicsPresenter New(int width, int height, PixelFormat pixelFormat, System.IntPtr windowHandle, SharpDX.DXGI.Usage usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput)
        {
#if WIN8METRO
            throw new NotImplementedException(); 
#else
            return NewForDesktop(width, height, pixelFormat, windowHandle, usage);
#endif
        }

#if !WIN8METRO
        private static GraphicsPresenter NewForDesktop(int width, int height, PixelFormat pixelFormat, System.IntPtr windowHandle, SharpDX.DXGI.Usage usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput)
        {
            // TODO make it configurable by parameters
            var refreshRate = new Rational(60, 1);

            var graphicsDevice = GraphicsDevice.CurrentSafe;
            var graphicsAdapter = graphicsDevice.Adapter;

            // Get display mode for the particular width, height, pixelformat
            var selectedModes = new List<DisplayMode>();
            for (int i = 0; i < graphicsAdapter.SupportedDisplayModes.Length; i++)
            {
                var displayMode = graphicsAdapter.SupportedDisplayModes[i];
                if (displayMode.Width == width && displayMode.Height == height && displayMode.Format == pixelFormat)
                    selectedModes.Add(displayMode);
            }

            // Calculate the closest / best refresh rate
            var refreshRateExpected = (float) refreshRate.Numerator / refreshRate.Denominator;
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

            return new GraphicsPresenter(GraphicsDevice.CurrentSafe, new SwapChain(GraphicsAdapter.Default, (Direct3D11.Device)GraphicsDevice.CurrentSafe, description));
        }
#endif

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