using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    internal class SharpDXInterop : Windows.Phone.Input.Interop.IDrawingSurfaceManipulationHandler
    {
        public SharpDXInterop()
        {
            _timer = new BasicTimer();
        }

        internal object CreateContentProvider()
        {
	        var provider =  new SharpDXContentProvider(this);
            return provider;
        }

        // IDrawingSurfaceManipulationHandler
        public void SetManipulationHost(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost manipulationHost)
        {
            manipulationHost.PointerPressed += OnPointerPressed;

            manipulationHost.PointerMoved += OnPointerMoved;

            manipulationHost.PointerReleased += OnPointerReleased;
        }

        // Event Handlers
        protected void OnPointerPressed(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost sender, Windows.UI.Core.PointerEventArgs args)
        {
            // Insert your code here.
        }

        protected void OnPointerMoved(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost sender, Windows.UI.Core.PointerEventArgs args)
        {
            // Insert your code here.
        }

        protected void OnPointerReleased(Windows.Phone.Input.Interop.DrawingSurfaceManipulationHost sender, Windows.UI.Core.PointerEventArgs args)
        {
            // Insert your code here.
        }

        internal void Connect()
        {
	        _renderer = new CubeRenderer();

	        // Restart timer after renderer has finished initializing.
	        _timer.Reset();
        }

        internal void Disconnect()
        {
            _renderer = null;
        }

        internal void UpdateForWindowSizeChange(float width, float height)
        {
            _renderer.UpdateForWindowSizeChange(width, height);
        }

        internal void Render(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            _timer.Update();
            _renderer.Update(device, context, renderTargetView);
            _renderer.Render();
        }

        internal Texture2D GetTexture()
        {
            return _renderer.GetTexture();
        }

        private CubeRenderer _renderer;
        private readonly BasicTimer _timer;

    }
}
