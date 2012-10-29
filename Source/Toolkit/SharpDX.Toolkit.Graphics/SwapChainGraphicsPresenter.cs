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
        private readonly RenderTarget2D backBuffer;

        private readonly SwapChain swapChain;

        public SwapChainGraphicsPresenter(GraphicsDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
            PresentInterval = presentationParameters.PresentationInterval;

            // Initialize the swap chain
            swapChain = CreateSwapChain();

            backBuffer = RenderTarget2D.New(device, swapChain.GetBackBuffer<Direct3D11.Texture2D>(0));
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
#if WIN8METRO
                if (!value)
                {
                    throw new ArgumentException("Cannot switch to non-full screen in Windows RT");
                }
#else
                swapChain.IsFullScreen = true;
#endif
            }
        }

        public override void Present()
        {
            swapChain.Present((int)PresentInterval, PresentFlags.None);
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode && swapChain != null)
            {
                swapChain.DebugName = Name;
            }
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

            var description = new SwapChainDescription1
            {
                // Automatic sizing
                Width = Description.BackBufferWidth,
                Height = Description.BackBufferHeight,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SharpDX.DXGI.SampleDescription((int)Description.MultiSampleCount, 0),
                Usage = Description.RenderTargetUsage,
                // Use two buffers to enable flip effect.
                BufferCount = 2,
                Scaling = SharpDX.DXGI.Scaling.Stretch,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
            }; 
            
            if (coreWindow != null)
            {
                // Creates a SwapChain from a CoreWindow pointer
                using (var comWindow = new ComObject(coreWindow)) return ((DXGI.Factory2)GraphicsAdapter.Factory).CreateSwapChainForCoreWindow((Direct3D11.Device)GraphicsDevice, comWindow, ref description, null);
            }
            else if (swapChainBackgroundPanel != null)
            {
                var nativePanel = ComObject.As<ISwapChainBackgroundPanelNative>(swapChainBackgroundPanel);
                // Creates the swap chain for XAML composition
                var swapChain = ((DXGI.Factory2)GraphicsAdapter.Factory).CreateSwapChainForComposition((Direct3D11.Device)GraphicsDevice, ref description, null);

                // Associate the SwapChainBackgroundPanel with the swap chain
                nativePanel.SwapChain = swapChain;
                return swapChain;
            }
            else
            {
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
            var control = Description.DeviceWindowHandle as Control;
            if (control == null)
            {
                throw new NotSupportedException(string.Format("Form of type [{0}] is not supported. Only System.Windows.Control are supported", Description.DeviceWindowHandle != null ? Description.DeviceWindowHandle.GetType().Name : "null"));
            }

            var description = new SwapChainDescription
                {
                    ModeDescription = new ModeDescription(Description.BackBufferWidth, Description.BackBufferHeight, Description.RefreshRate, Description.BackBufferFormat), 
                    BufferCount = 1, 
                    OutputHandle = control.Handle, 
                    SampleDescription = new SampleDescription((int)Description.MultiSampleCount, 0), 
                    SwapEffect = SwapEffect.Discard, 
                    Usage = Description.RenderTargetUsage, 
                    IsWindowed = !Description.IsFullScreen, 
                    Flags = SwapChainFlags.None, 
                };

            return new SwapChain(GraphicsAdapter.Factory, (Direct3D11.Device)GraphicsDevice, description);
        }
#endif
    }
}