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
using Windows.UI.Core;

namespace CommonDX
{
    using SharpDX.DXGI;

    /// <summary>
    /// Target to render to a <see cref="CoreWindow"/>
    /// </summary>
    public class CoreWindowTarget : SwapChainTargetBase
    {
        protected CoreWindow window;

        /// <summary>
        /// Initialzies a new <see cref="CoreWindowTarget"/> instance.
        /// </summary>
        /// <param name="window"></param>
        public CoreWindowTarget(CoreWindow window)
        {
            this.window = window;

            // Register event on Window Size Changed
            // So that resources dependent size can be resized
            window.SizeChanged += window_SizeChanged;
        }

        protected override Windows.Foundation.Rect CurrentControlBounds
        {
            get { return window.Bounds; }
        }

        protected override int Width
        {
            get
            {
                return 0; // Returns 0 to fill the CoreWindow 
            }
        }

        protected override int Height
        {
            get
            {
                return 0; // Returns 0 to fill the CoreWindow 
            }
        }

        protected override SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            // Creates a SwapChain from a CoreWindow pointer
            using (var comWindow = new ComObject(window))
                return new SwapChain1(factory, device, comWindow, ref desc);
        }

        private void window_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            UpdateForSizeChange();
        }
    }
}
