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
using SharpDX;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    public delegate void RequestAdditionalFrameHandler();
    public delegate void RecreateSynchronizedTextureHandler();

    /// <summary>
    /// This is a port of Direct3D C++ WP8 sample. This port is not clean and complete. 
    /// The preferred way to access Direct3D on WP8 is by using SharpDX.Toolkit.
    /// </summary>
    internal class SharpDXInterop : Windows.Phone.Input.Interop.IDrawingSurfaceManipulationHandler
    {
        public event RequestAdditionalFrameHandler RequestAdditionalFrame;
	    public event RecreateSynchronizedTextureHandler RecreateSynchronizedTexture;

        public Windows.Foundation.Size NativeResolution { get; set; }
        public Windows.Foundation.Size _renderResolution;
        public Windows.Foundation.Size WindowBounds { get; set; }

        public SharpDXInterop()
        {
            _timer = new BasicTimer();
        }

        public Windows.Foundation.Size RenderResolution {
            get { return _renderResolution; }
            set {
                if (value.Width != _renderResolution.Width ||
                    value.Height != _renderResolution.Height)
                {
                    _renderResolution = value;

                    if (_renderer!=null)
                    {
                        _renderer.UpdateForWindowSizeChange((float)_renderResolution.Width, (float)_renderResolution.Height);
                        if (RecreateSynchronizedTexture != null) RecreateSynchronizedTexture();
                    }
                }
            } 
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
