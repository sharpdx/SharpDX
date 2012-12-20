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
using System.Diagnostics;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    /// <summary>
    /// Helper class that initializes SharpDX APIs for 3D rendering.
    /// This is a port of Direct3D C++ WP8 sample. This port is not clean and complete. 
    /// The preferred way to access Direct3D on WP8 is by using SharpDX.Toolkit.
    /// </summary>
    internal abstract class SharpDXBase : Component
    {
        // Constructor.
        internal SharpDXBase()
        {
        }

        public void Initialize()
        {
	        CreateDeviceResources();
        }

        public virtual void Update(float timeTotal, float timeDelta)
        {

        }


        public virtual void CreateDeviceResources()
        {
            // This flag adds support for surfaces with a different color channel ordering
	        // than the API default. It is required for compatibility with Direct2D.
	        DeviceCreationFlags creationFlags = DeviceCreationFlags.BgraSupport  ; //| DeviceCreationFlags.Debug;
            
	        // This array defines the set of DirectX hardware feature levels this app will support.
	        // Note the ordering should be preserved.
	        // Don't forget to declare your application's minimum required feature level in its
	        // description.  All applications are assumed to support 9.1 unless otherwise stated.
            
	        SharpDX.Direct3D.FeatureLevel[] featureLevels = 
	        {
                SharpDX.Direct3D.FeatureLevel.Level_11_1,
		        SharpDX.Direct3D.FeatureLevel.Level_11_0,
		        SharpDX.Direct3D.FeatureLevel.Level_10_1,
		        SharpDX.Direct3D.FeatureLevel.Level_10_0,
		        SharpDX.Direct3D.FeatureLevel.Level_9_3
	        };

	        // Create the Direct3D 11 API device object and a corresponding context.
            _device = new Device( DriverType.Hardware, creationFlags, featureLevels);
            _deviceContext = new DeviceContext(_device);

            _featureLevel = _device.FeatureLevel;

        }

        public virtual void CreateWindowSizeDependentResources()
        {
            Texture2DDescription renderTargetDesc = new Texture2DDescription()
            {
                Width = (int) _renderTargetSize.Width,
                Height = (int) _renderTargetSize.Height,
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags =  CpuAccessFlags.None,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.SharedKeyedmutex | ResourceOptionFlags.SharedNthandle,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };
            


            
            // Allocate a 2-D surface as the render target buffer.
            _renderTarget = new Texture2D(_device, renderTargetDesc);
            
            _renderTargetview = new RenderTargetView(_device, _renderTarget);

            Texture2DDescription depthStencilDesc = new Texture2DDescription()
            {
                Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                Width = (int)_renderTargetSize.Width,
                Height = (int)_renderTargetSize.Height,
                ArraySize = 1,
                MipLevels = 1,
                BindFlags = BindFlags.DepthStencil,
                Usage = ResourceUsage.Default,
                CpuAccessFlags = CpuAccessFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0), 
                OptionFlags = ResourceOptionFlags.None

            };


            Texture2D depthStencil = new Texture2D(_device, depthStencilDesc);
            //DepthStencilViewDescription depthStencilViewDesc = new DepthStencilViewDescription()
            //{
            //     Dimension = DepthStencilViewDimension.Texture2D,
            //};
            
             ComObject.Dispose(ref _depthStencilView);
            _depthStencilView = new DepthStencilView(_device, depthStencil); //, depthStencilViewDesc);
            

            _windowBounds.Width = _renderTargetSize.Width;
            _windowBounds.Height = _renderTargetSize.Height;

            // Create a viewport descriptor of the full window size.
             var viewport = new SharpDX.Direct3D11.Viewport(0, 0, (float)_renderTargetSize.Width, (float)_renderTargetSize.Height );

            _deviceContext.Rasterizer.SetViewports(viewport);
        }

        public virtual void UpdateForWindowSizeChange(float width, float height)
        {
	        _windowBounds.Width = width;
            _windowBounds.Height = height;


        }

        public virtual void UpdateForRenderResolutionChange(float width, float height)
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
