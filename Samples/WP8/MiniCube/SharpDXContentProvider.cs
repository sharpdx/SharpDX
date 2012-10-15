using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
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

        public override void PrepareResources(DateTime presentTargetTime, SharpDX.DrawingSizeF desiredRenderTargetSize)
        {
        }

	    private readonly SharpDXInterop _controller;
	    DrawingSurfaceRuntimeHost _host;
	    DrawingSurfaceSynchronizedTexture _synchronizedTexture;
    }
}
