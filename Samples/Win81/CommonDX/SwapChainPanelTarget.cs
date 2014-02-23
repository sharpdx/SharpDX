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
using SharpDX;
using SharpDX.DXGI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CommonDX
{
    /// <summary>
    /// Target to render to a <see cref="SwapChainPanel"/>.
    /// </summary>
    /// <remarks>
    /// This class should be use when efficient DirectX-XAML interop is required.
    /// </remarks>
    public class SwapChainPanelTarget : SwapChainTargetBase
    {
        private SwapChainPanel panel;
        private ISwapChainPanelNative nativePanel;
        private float lastCompositionScaleX = 0;
        private float lastCompositionScaleY = 0;

        private SwapChain2 swapChain2;

        /// <summary>
        /// Initializes a new <see cref="SwapChainPanelTarget"/> instance
        /// </summary>
        /// <param name="panel">The <see cref="SwapChainPanel"/> to render to</param>
        public SwapChainPanelTarget(SwapChainPanel panel)
        {
            this.panel = panel;

            // Gets the native panel
            nativePanel = ComObject.As<ISwapChainPanelNative>(panel);
            panel.CompositionScaleChanged += panel_CompositionScaleChanged;
            panel.SizeChanged += panel_SizeChanged;
        }

        private void panel_CompositionScaleChanged(SwapChainPanel sender, object args)
        {
            if (panel.CompositionScaleX != lastCompositionScaleX && panel.CompositionScaleY != lastCompositionScaleY)
            {
                CreateSizeDependentResources(this);
            }
        }

        void panel_SizeChanged(object sender, SizeChangedEventArgs e)
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
                var width = Double.IsNaN(panel.Width) ? 1 : panel.Width;
                return (int)(width * panel.CompositionScaleX + 0.5f);
            }
        }

        protected override int Height
        {
            get
            {
                var height = Double.IsNaN(panel.Height) ? 1 : panel.Height;
                return (int)(height * panel.CompositionScaleY + 0.5f);
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

        protected override void CreateSizeDependentResources(TargetBase renderBase)
        {
            base.CreateSizeDependentResources(renderBase);

            if (swapChain2 != null)
            {
                swapChain2.MatrixTransform = Matrix3x2.Scaling(1f / panel.CompositionScaleX, 1f / panel.CompositionScaleY);
                lastCompositionScaleX = panel.CompositionScaleX;
                lastCompositionScaleY = panel.CompositionScaleY;
            }
        }

        protected override SharpDX.DXGI.SwapChain2 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            // Creates the swap chain for XAML composition
            var swapChain1 = new SwapChain1(factory, device, ref desc);
            var swapChain2 = new SwapChain2(swapChain1.NativePointer);

            // Associate the SwapChainPanel with the swap chain
            nativePanel.SwapChain = swapChain2;

            // Returns the new swap chain
            return swapChain2;
        }
    }
}
