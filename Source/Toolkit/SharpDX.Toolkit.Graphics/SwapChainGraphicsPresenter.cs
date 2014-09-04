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
#if WIN8METRO
using Windows.UI.Core;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;
#elif WP8

#else
using System.Windows.Forms;
#endif

using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Graphics presenter for SwapChain.
    /// </summary>
    public class SwapChainGraphicsPresenter : GraphicsPresenter
    {
        private RenderTarget2D backBuffer;

        private SwapChain swapChain;
#if DIRECTX11_2
        private SwapChain2 swapChain2;
#endif

        private int bufferCount;

        public SwapChainGraphicsPresenter(GraphicsDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
            PresentInterval = presentationParameters.PresentationInterval;

            // Initialize the swap chain
            swapChain = ToDispose(CreateSwapChain());

#if DIRECTX11_2
            swapChain2 = ToDispose(swapChain.QueryInterface<SwapChain2>());
#endif

            backBuffer = ToDispose(RenderTarget2D.New(device, swapChain.GetBackBuffer<Direct3D11.Texture2D>(0)));
        }

        public override RenderTarget2D BackBuffer
        {
            get
            {
                return backBuffer;
            }
        }

        public override object NativePresenter
        {
            get
            {
                return swapChain;
            }
        }

        public override bool IsFullScreen
        {
            get
            {
#if WIN8METRO
                return true;
#else
                return swapChain.IsFullScreen;
#endif
            }

            set
            {
#if !WIN8METRO
                if(swapChain == null)
                    return;
                
                var outputIndex = PrefferedFullScreenOutputIndex;

                // no outputs connected to the current graphics adapter
                var output = GraphicsDevice.Adapter == null || GraphicsDevice.Adapter.OutputsCount == 0 ? null : GraphicsDevice.Adapter.GetOutputAt(outputIndex);

                Output currentOutput = null;

                try
                {
                    Bool isCurrentlyFullscreen;
                    swapChain.GetFullscreenState(out isCurrentlyFullscreen, out currentOutput);

                    // check if the current fullscreen monitor is the same as new one
                    if (isCurrentlyFullscreen == value && output != null && currentOutput != null && currentOutput.NativePointer == ((Output)output).NativePointer)
                        return;
                }
                finally
                {
                    if (currentOutput != null)
                        currentOutput.Dispose();
                }

                bool switchToFullScreen = value;
                // If going to fullscreen mode: call 1) SwapChain.ResizeTarget 2) SwapChain.IsFullScreen
                var description = new ModeDescription(backBuffer.Width, backBuffer.Height, Description.RefreshRate, Description.BackBufferFormat);
                if(switchToFullScreen)
                {
                    Description.IsFullScreen = true;
                    // Destroy and recreate the full swapchain in case of fullscreen switch
                    // It seems to be more reliable then trying to change the current swap-chain.
                    RemoveAndDispose(ref backBuffer);
                    RemoveAndDispose(ref swapChain);

                    swapChain = CreateSwapChain();
                    backBuffer = ToDispose(RenderTarget2D.New(GraphicsDevice, swapChain.GetBackBuffer<Direct3D11.Texture2D>(0)));
                }
                else
                {
                    Description.IsFullScreen = false;
                    swapChain.IsFullScreen = false;

                    // call 1) SwapChain.IsFullScreen 2) SwapChain.Resize
                    Resize(backBuffer.Width, backBuffer.Height, backBuffer.Format);
                }

                // If going to window mode: 
                if (!switchToFullScreen)
                {
                    // call 1) SwapChain.IsFullScreen 2) SwapChain.Resize
                    description.RefreshRate = new Rational(0, 0);
                    swapChain.ResizeTarget(ref description);
                }
#endif
            }
        }

        public override void Present()
        {
            swapChain.Present((int)PresentInterval, PresentFlags.None);
        }

        /// <summary>
        /// Called when name changed for this component.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Name")
            {
                if (GraphicsDevice.IsDebugMode && swapChain != null)
                {
                    swapChain.DebugName = Name;
                }
            }
        }

        public override bool Resize(int width, int height, Format format, Rational? refreshRate = null)
        {
            if (!base.Resize(width, height, format, refreshRate)) return false;

            RemoveAndDispose(ref backBuffer);

#if DIRECTX11_2 && WIN8METRO
            var swapChainPanel = Description.DeviceWindowHandle as SwapChainPanel;
            if (swapChainPanel != null && swapChain2 != null)
            {

                swapChain2.MatrixTransform = Matrix3x2.Scaling(1f / swapChainPanel.CompositionScaleX, 1f / swapChainPanel.CompositionScaleY);
            }
#endif

            swapChain.ResizeBuffers(bufferCount, width, height, format, Description.Flags);

            // Recreate the back buffer
            backBuffer = ToDispose(RenderTarget2D.New(GraphicsDevice, swapChain.GetBackBuffer<Direct3D11.Texture2D>(0)));

            // Reinit the Viewport
            DefaultViewport = new ViewportF(0, 0, backBuffer.Width, backBuffer.Height);

            return true;
        }

        private SwapChain CreateSwapChain()
        {
            // Check for Window Handle parameter
            if (Description.DeviceWindowHandle == null)
            {
                throw new ArgumentException("DeviceWindowHandle cannot be null");
            }

#if WIN8METRO
            return CreateSwapChainForWinRT();
#else
            return CreateSwapChainForDesktop();
#endif
        }

#if WIN8METRO
        private SwapChain CreateSwapChainForWinRT()
        {
            var coreWindow = Description.DeviceWindowHandle as CoreWindow;
            var swapChainBackgroundPanel = Description.DeviceWindowHandle as SwapChainBackgroundPanel;

            bufferCount = 2;
            var description = new SwapChainDescription1
            {
                // Automatic sizing
                Width = Description.BackBufferWidth,
                Height = Description.BackBufferHeight,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm, // TODO: Check if we can use the Description.BackBufferFormat
                Stereo = false,
                SampleDescription = new SharpDX.DXGI.SampleDescription((int)Description.MultiSampleCount, 0),
                Usage = Description.RenderTargetUsage,
                // Use two buffers to enable flip effect.
                BufferCount = bufferCount,
                Scaling = SharpDX.DXGI.Scaling.Stretch,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
            };

            if (coreWindow != null)
            {
                // Creates a SwapChain from a CoreWindow pointer
                using (var comWindow = new ComObject(coreWindow))
                    return new SwapChain1((DXGI.Factory2)GraphicsAdapter.Factory, (Direct3D11.Device)GraphicsDevice, comWindow, ref description);
            }
            else if (swapChainBackgroundPanel != null)
            {
                var nativePanel = ComObject.As<ISwapChainBackgroundPanelNative>(swapChainBackgroundPanel);
                // Creates the swap chain for XAML composition
                var swapChain = new SwapChain1((DXGI.Factory2)GraphicsAdapter.Factory, (Direct3D11.Device)GraphicsDevice, ref description);

                // Associate the SwapChainBackgroundPanel with the swap chain
                nativePanel.SwapChain = swapChain;
                return swapChain;
            }
            else
            {
#if DIRECTX11_2
                using (var comObject = new ComObject(Description.DeviceWindowHandle))
                {
                    var swapChainPanel = comObject.QueryInterfaceOrNull<ISwapChainPanelNative>();
                    if (swapChainPanel != null)
                    {
                        var swapChain = new SwapChain1((DXGI.Factory2)GraphicsAdapter.Factory, (Direct3D11.Device)GraphicsDevice, ref description);
                        swapChainPanel.SwapChain = swapChain;
                        return swapChain;
                    }
                }
#endif
                throw new NotSupportedException();
            }
        }
#elif WP8
        private SwapChain CreateSwapChainForDesktop()
        {
            throw new NotImplementedException();
        }
#else
        private SwapChain CreateSwapChainForDesktop()
        {
            IntPtr? handle = null;
            var control = Description.DeviceWindowHandle as Control;
            if (control != null) handle = control.Handle;
            else if (Description.DeviceWindowHandle is IntPtr) handle = (IntPtr)Description.DeviceWindowHandle;

            if (!handle.HasValue)
            {
                throw new NotSupportedException(string.Format("DeviceWindowHandle of type [{0}] is not supported. Only System.Windows.Control or IntPtr are supported", Description.DeviceWindowHandle != null ? Description.DeviceWindowHandle.GetType().Name : "null"));
            }

            bufferCount = 1;
            var description = new SwapChainDescription
                {
                    ModeDescription = new ModeDescription(Description.BackBufferWidth, Description.BackBufferHeight, Description.RefreshRate, Description.BackBufferFormat),
                    BufferCount = bufferCount, // TODO: Do we really need this to be configurable by the user?
                    OutputHandle = handle.Value,
                    SampleDescription = new SampleDescription((int)Description.MultiSampleCount, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Description.RenderTargetUsage,
                    IsWindowed = true,
                    Flags = Description.Flags,
                };

            var newSwapChain = new SwapChain(GraphicsAdapter.Factory, (Direct3D11.Device)GraphicsDevice, description);
            if (Description.IsFullScreen)
            {
                // Before fullscreen switch
                newSwapChain.ResizeTarget(ref description.ModeDescription);

                // Switch to full screen
                newSwapChain.IsFullScreen = true;

                // This is really important to call ResizeBuffers AFTER switching to IsFullScreen 
                newSwapChain.ResizeBuffers(bufferCount, Description.BackBufferWidth, Description.BackBufferHeight, Description.BackBufferFormat, SwapChainFlags.AllowModeSwitch);
            }

            return newSwapChain;
        }
#endif
    }
}