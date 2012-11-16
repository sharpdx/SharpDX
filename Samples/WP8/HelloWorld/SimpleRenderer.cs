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
using System.Diagnostics;

using SharpDX;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    /// <summary>
    /// This class implements the minimal interface to draw a Direct3D content into a XAML DrawingSurface.
    /// </summary>
    public class SimpleRenderer : DrawingSurfaceBackgroundContentProviderNativeBase
    {
        private DrawingSurfaceRuntimeHost host;

        public override void Connect(DrawingSurfaceRuntimeHost host, Device device)
        {
            this.host = host;
        }

        public override void Disconnect()
        {
        }

        public override void PrepareResources(DateTime presentTargetTime, ref DrawingSizeF desiredRenderTargetSize)
        {
        }

        public override void Draw(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            // We just clear the render target view
            context.ClearRenderTargetView(renderTargetView, Color.CornflowerBlue);

            // Ask the DrawingSurface to call us back
            host.RequestAdditionalFrame();
        }
    }
}