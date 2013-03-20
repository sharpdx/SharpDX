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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    /// <summary>
    /// This is a port of Direct3D C++ WP8 sample. This port is not clean and complete. 
    /// The preferred way to access Direct3D on WP8 is by using SharpDX.Toolkit.
    /// </summary>
    internal class SharpDXContentProvider : DrawingSurfaceBackgroundContentProviderNativeBase
    {
        public SharpDXContentProvider(SharpDXInterop controller)
        {
            _controller = controller;
        }

        public override void Connect(DrawingSurfaceRuntimeHost host, Device device)
        {
            _host = host;
            _controller.Connect();
        }

        public override void Disconnect()
        {
            _controller.Disconnect();
            _host = null;
            _synchronizedTexture = null;
        }

        public override void Draw(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            _controller.Render(device, context, renderTargetView);
            _host.RequestAdditionalFrame();
        }

        public override void PrepareResources(DateTime presentTargetTime, ref SharpDX.Size2F desiredRenderTargetSize)
        {
        }

	    private readonly SharpDXInterop _controller;
	    DrawingSurfaceRuntimeHost _host;
	    DrawingSurfaceSynchronizedTexture _synchronizedTexture;
    }
}
