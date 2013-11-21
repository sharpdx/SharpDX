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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DXGI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CommonDX
{
    /// <summary>
    /// Target to render to a <see cref="SwapChainBackgroundPanel"/>.
    /// </summary>
    /// <remarks>
    /// This class should be use when efficient DirectX-XAML interop is required.
    /// </remarks>
    public class SwapChainBackgroundPanelTarget : SwapChainTargetBase
    {
        private SwapChainBackgroundPanel panel;
        private ISwapChainBackgroundPanelNative nativePanel;

        /// <summary>
        /// Initializes a new <see cref="SwapChainBackgroundPanelTarget"/> instance
        /// </summary>
        /// <param name="panel">The <see cref="SwapChainBackgroundPanel"/> to render to</param>
        public SwapChainBackgroundPanelTarget(SwapChainBackgroundPanel panel)
        {
            this.panel = panel;

            // Gets the native panel
            nativePanel = ComObject.As<ISwapChainBackgroundPanelNative>(panel);

            // Register event on Window Size Changed
            // So that resources dependent size can be resized
            Window.Current.CoreWindow.SizeChanged += CoreWindow_SizeChanged;
        }

        void CoreWindow_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            UpdateForSizeChange();
        }

        protected override Windows.Foundation.Rect CurrentControlBounds
        {
            get { return new Windows.Foundation.Rect(0, 0, panel.RenderSize.Width, panel.RenderSize.Height); }
        }

        protected override int Width
        {
            get
            {
                // Unlike CoreWindow, Width/Height of the SwapChain must be specified
                var currentWindow = Window.Current.CoreWindow;
                return (int)(currentWindow.Bounds.Width * DeviceManager.Dpi / 96.0);
            }
        }

        protected override int Height
        {
            get
            {
                // Unlike CoreWindow, Width/Height of the SwapChain must be specified
                var currentWindow = Window.Current.CoreWindow;
                return (int)(currentWindow.Bounds.Height * DeviceManager.Dpi / 96.0); // Returns 0 to fill the CoreWindow 
            }
        }

        protected override SwapChainDescription1 CreateSwapChainDescription()
        {
            // Get the default descirption.
            var desc = base.CreateSwapChainDescription();

            // Apart for the width and height, the other difference
            // in the SwapChainDescription is that Scaling must be 
            // set to Stretch for XAML Composition 
            desc.Scaling = Scaling.Stretch;
            return desc;
        }

        protected override SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            // Creates the swap chain for XAML composition
            var swapChain = new SwapChain1(factory, device, ref desc);

            // Associate the SwapChainBackgroundPanel with the swap chain
            nativePanel.SwapChain = swapChain;

            // Returns the new swap chain
            return swapChain;
        }
    }
}
