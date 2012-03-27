// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

namespace CommonDX
{
    public abstract class TargetBase : Component
    {
        // DirectXBase
        public DeviceManager DeviceManager { get; private set; }

        // Rendertargets for Direct3D 
        protected SharpDX.Direct3D11.RenderTargetView renderTargetView;
        public SharpDX.Direct3D11.RenderTargetView RenderTargetView { get { return renderTargetView; } }

        protected SharpDX.Direct3D11.DepthStencilView depthStencilView;
        public SharpDX.Direct3D11.DepthStencilView DepthStencilView { get { return depthStencilView; } }

        // RenderTarget for Direct2D
        protected SharpDX.Direct2D1.Bitmap1 bitmapTarget;
        public SharpDX.Direct2D1.Bitmap1 BitmapTarget2D { get { return bitmapTarget; } }

        public Windows.Foundation.Size RenderTargetSize { get; protected set; }

        public Windows.Foundation.Rect ControlBounds { get; protected set; }

        public event Action<TargetBase> OnSizeChanged;

        public event Action<TargetBase> OnRender;

        public TargetBase()
        {
        }

        public virtual void Initialize(DeviceManager deviceManager) {
            this.DeviceManager = deviceManager;

            // If the DPI is changed, we need to perform a OnSizeChanged event
            deviceManager.OnDpiChanged -= devices_OnDpiChanged;
            deviceManager.OnDpiChanged += devices_OnDpiChanged;
        }

        protected abstract Windows.Foundation.Rect CurrentControlBounds
        {
            get;
        }

        public virtual void UpdateForSizeChange()
        {
            var newBounds = CurrentControlBounds;

            if (newBounds.Width != ControlBounds.Width ||
                newBounds.Height != ControlBounds.Height)
            {
                // Store the window bounds so the next time we get a SizeChanged event we can
                // avoid rebuilding everything if the size is identical.
                ControlBounds = newBounds;

                if (OnSizeChanged != null)
                    OnSizeChanged(this);
            }
        }

        public virtual void RenderAll()
        {
            if (OnRender != null)
                OnRender(this);
        }

        private void devices_OnDpiChanged(DeviceManager obj)
        {
            if (OnSizeChanged != null)
                OnSizeChanged(this);
        }
    }
}
