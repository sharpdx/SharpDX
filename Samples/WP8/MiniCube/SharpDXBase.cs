using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    // Helper class that initializes SharpDX APIs for 3D rendering.
    internal abstract class SharpDXBase : Component
    {
        // Constructor.
        internal SharpDXBase()
        {
        }

        public void Update(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            if (device != _device)
            {
                _device = device;
                CreateDeviceResources();
            }

            _deviceContext = context;
            _renderTargetview = renderTargetView;

            CreateWindowSizeDependentResources();
        }

        public virtual void CreateDeviceResources()
        {
        }

        public virtual void CreateWindowSizeDependentResources()
        {
            var resource = _renderTargetview.Resource;
            using (var texture2D = new Texture2D(resource.NativePointer))
            {

                var currentWidth = (int) _renderTargetSize.Width;
                var currentHeight = (int) _renderTargetSize.Height;

                if (currentWidth != texture2D.Description.Width &&
                    currentHeight != texture2D.Description.Height)
                {
                    _renderTargetSize.Width = texture2D.Description.Width;
                    _renderTargetSize.Height = texture2D.Description.Height;

                    ComObject.Dispose(ref _depthStencilView);

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
             var viewport = new SharpDX.Direct3D11.Viewport(0, 0, (float)_renderTargetSize.Width, (float)_renderTargetSize.Height );

            _deviceContext.Rasterizer.SetViewports(viewport);
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

	        CreateWindowSizeDependentResources();
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
