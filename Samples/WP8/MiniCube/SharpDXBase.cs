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
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    /// <summary>
    /// Helper class that initializes SharpDX APIs for 3D rendering.
    /// This is a port of Direct3D C++ WP8 sample. This port is not clean and complete. 
    /// DO NOT USE IT AS A STARTING POINT FOR DEVELOPING A PRODUCTION QUALITY APPLICATION
    /// </summary>
    internal abstract class SharpDXBase : Component
    {
        // Constructor.
        internal SharpDXBase()
        {
        }

        public void Update(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            bool isNewDevice = false;
            if (device != _device)
            {
                _device = device;
                CreateDeviceResources();
                isNewDevice = true;
            }
            _deviceContext.ClearState();

            _deviceContext = context;
            _renderTargetview = renderTargetView;

            CreateWindowSizeDependentResources(isNewDevice);
        }

        public virtual void CreateDeviceResources()
        {
        }

        public virtual void CreateWindowSizeDependentResources(bool isNewDevice)
        {
            var resource = _renderTargetview.Resource;
            using (var texture2D = new Texture2D(resource.NativePointer))
            {

                var currentWidth = (int) _renderTargetSize.Width;
                var currentHeight = (int) _renderTargetSize.Height;

                if ((currentWidth != texture2D.Description.Width &&
                    currentHeight != texture2D.Description.Height) || isNewDevice)
                {
                    _renderTargetSize.Width = texture2D.Description.Width;
                    _renderTargetSize.Height = texture2D.Description.Height;

                    Utilities.Dispose(ref _depthStencilView);

                    using (var depthTexture = new Texture2D(
                        _device,
                        new Texture2DDescription()
                            {
                                Width = (int) _renderTargetSize.Width,
                                Height = (int) _renderTargetSize.Height,
                                ArraySize = 1,
                                BindFlags = BindFlags.DepthStencil,
                                CpuAccessFlags = CpuAccessFlags.None,
                                Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                                MipLevels = 1,
                                OptionFlags = ResourceOptionFlags.None,
                                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                                Usage = ResourceUsage.Default
                            }))
                        _depthStencilView = new DepthStencilView(_device, depthTexture);
                }
            }

            _windowBounds.Width = _renderTargetSize.Width;
            _windowBounds.Height = _renderTargetSize.Height;

            // Create a viewport descriptor of the full window size.
             var viewport = new SharpDX.ViewportF(0, 0, (float)_renderTargetSize.Width, (float)_renderTargetSize.Height );

            _deviceContext.Rasterizer.SetViewport(viewport);
        }

        public virtual void UpdateForWindowSizeChange(float width, float height)
        {
	        _renderTargetSize.Width = width;
	        _renderTargetSize.Height = height;

	        RenderTargetView[] nullViews = {null};
	        //_deviceContext.SetRenderTargets(ARRAYSIZE(nullViews), nullViews, null);
	        _renderTarget = null;
	        _renderTargetview = null;
	        _depthStencilView = null;
	        _deviceContext.Flush();

	        CreateWindowSizeDependentResources(false);
        }

        public abstract void Render();

        internal virtual Texture2D GetTexture()
        {
            return _renderTarget;
        }

        // Direct3D Objects.
        protected Device _device;
        protected DeviceContext _deviceContext;
        protected Texture2D _renderTarget;
        protected RenderTargetView _renderTargetview;
        protected DepthStencilView _depthStencilView;

        // Cached renderer properties.
        protected FeatureLevel _featureLevel;
        protected Windows.Foundation.Size _renderTargetSize;
        protected Windows.Foundation.Rect _windowBounds;
    }
}
