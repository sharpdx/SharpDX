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

using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Graphics presenter for SwapChain.
    /// </summary>
    public class RenderTargetGraphicsPresenter : GraphicsPresenter
    {
        private RenderTarget2D backBuffer;

        public RenderTargetGraphicsPresenter(GraphicsDevice device, RenderTarget2D renderTarget, DepthFormat depthFormat = DepthFormat.None)
            : base(device, CreatePresentationParameters(renderTarget, depthFormat))
        {
            PresentInterval = Description.PresentationInterval;

            // Initialize the swap chain
            backBuffer = renderTarget;
        }

        private static PresentationParameters CreatePresentationParameters(RenderTarget2D renderTarget2D, DepthFormat depthFormat)
        {
            return new PresentationParameters()
                {
                    BackBufferWidth = renderTarget2D.Width,
                    BackBufferHeight = renderTarget2D.Height,
                    BackBufferFormat = renderTarget2D.Description.Format,
                    DepthStencilFormat = depthFormat,
                    DeviceWindowHandle = renderTarget2D,
                    Flags = SwapChainFlags.None,
                    IsFullScreen = true,
                    MultiSampleCount = MSAALevel.None,
                    PresentationInterval = PresentInterval.One,
                    RefreshRate = new Rational(60, 1),
                    RenderTargetUsage = Usage.RenderTargetOutput
                };
        }

        public override RenderTarget2D BackBuffer
        {
            get
            {
                return backBuffer;
            }
        }

        public void SetBackBuffer(RenderTarget2D backBuffer)
        {
            this.backBuffer = backBuffer;
        }

        public override object NativePresenter
        {
            get
            {
                return backBuffer;
            }
        }

        public override bool IsFullScreen
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        public override void Present()
        {
        }
    }
}