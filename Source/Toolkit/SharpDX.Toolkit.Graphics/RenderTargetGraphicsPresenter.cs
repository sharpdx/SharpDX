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
    using Direct3D11;

    /// <summary>
    /// Graphics presenter for SwapChain.
    /// </summary>
    public class RenderTargetGraphicsPresenter : GraphicsPresenter
    {
        private Texture2DDescription renderTargetDescription;
        private readonly bool allowFormatChange;
        private RenderTarget2D backBuffer;

        public RenderTargetGraphicsPresenter(GraphicsDevice device, Texture2DDescription renderTargetDescription, DepthFormat depthFormat = DepthFormat.None, bool allowFormatChange = true, bool disposeRenderTarget = false)
            : base(device, CreatePresentationParameters(renderTargetDescription, depthFormat))
        {
            PresentInterval = Description.PresentationInterval;

            // Initialize the swap chain
            this.renderTargetDescription = renderTargetDescription;
            this.allowFormatChange = allowFormatChange;

            backBuffer = RenderTarget2D.New(device, renderTargetDescription);

            if (disposeRenderTarget)
                ToDispose(backBuffer);
        }

        private static PresentationParameters CreatePresentationParameters(Texture2DDescription renderTargetDescription, DepthFormat depthFormat)
        {
            return new PresentationParameters()
                {
                    BackBufferWidth = renderTargetDescription.Width,
                    BackBufferHeight = renderTargetDescription.Height,
                    BackBufferFormat = renderTargetDescription.Format,
                    DepthStencilFormat = depthFormat,
                    DeviceWindowHandle = renderTargetDescription,
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
            GraphicsDevice.Flush();
        }

        public override void Resize(int width, int height, Format format)
        {
            base.Resize(width, height, format);

            renderTargetDescription.Width = width;
            renderTargetDescription.Height = height;

            if (allowFormatChange)
                renderTargetDescription.Format = format;

            if (backBuffer != null)
            {
                RemoveAndDispose(ref backBuffer);
                backBuffer = RenderTarget2D.New(GraphicsDevice, renderTargetDescription);
            }
        }
    }
}