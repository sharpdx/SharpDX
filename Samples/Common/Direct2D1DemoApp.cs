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
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;

namespace SharpDX.Samples
{
    /// <summary>
    /// Root class for Direct2D and DirectWrite Demo App.
    /// </summary>
    public class Direct2D1DemoApp : Direct3D10DemoApp
    {
        public Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public RenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set;}

        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            Factory2D = new SharpDX.Direct2D1.Factory();
            using (var surface = BackBuffer.QueryInterface<Surface>())
            {
                RenderTarget2D = new RenderTarget(Factory2D, surface,
                                                  new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            }
            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;

            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            SceneColorBrush = new SolidColorBrush(RenderTarget2D, Color.White);
        }

        protected override void BeginDraw()
        {
            base.BeginDraw();
            RenderTarget2D.BeginDraw();
        }

        protected override void EndDraw()
        {
            RenderTarget2D.EndDraw();
            base.EndDraw();
        }
    }
}
