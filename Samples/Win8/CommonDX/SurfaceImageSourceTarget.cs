using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DXGI;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;

namespace CommonDX
{
    public class SurfaceImageSourceTarget : TargetBase
    {
        private Dictionary<IntPtr, SurfaceViewData> mapSurfaces = new Dictionary<IntPtr, SurfaceViewData>();

        private int pixelWidth;
        private int pixelHeight;
        private SurfaceImageSource surfaceImageSource;
        private ISurfaceImageSourceNative surfaceImageSourceNative;

        public SurfaceImageSourceTarget(int pixelWidth, int pixelHeight)
        {
            this.pixelWidth = pixelWidth;
            this.pixelHeight = pixelHeight;
            this.surfaceImageSource = new SurfaceImageSource(pixelWidth, pixelHeight);
            surfaceImageSourceNative = ComObject.As<SharpDX.DXGI.ISurfaceImageSourceNative>(surfaceImageSource);
        }

        public SurfaceImageSource ImageSource
        {
            get
            {
                return surfaceImageSource;
            }
        }

        public override void Initialize(DeviceManager deviceManager)
        {
            base.Initialize(deviceManager);
            surfaceImageSourceNative.Device = DeviceManager.DeviceDirect3D.QueryInterface<SharpDX.DXGI.Device>();
        }

        protected override Windows.Foundation.Rect CurrentControlBounds
        {
            get { 
                return new Windows.Foundation.Rect(0, 0, surfaceImageSource.PixelWidth, surfaceImageSource.PixelHeight); 
            }
        }

        public override void RenderAll()
        {
            SurfaceViewData viewData;

            DrawingPoint position;
            var regionToDraw = new SharpDX.Rectangle(0, 0, pixelWidth, pixelHeight);
            using (var surface = surfaceImageSourceNative.BeginDraw(regionToDraw, out position))
            {
                // Cache DXGI surface in order to avoid recreate all render target view, depth stencil...etc.
                // Is it the right way to do it?
                if (!mapSurfaces.TryGetValue(surface.NativePointer, out viewData))
                {
                    viewData = new SurfaceViewData();
                    mapSurfaces.Add(surface.NativePointer, viewData);

                    // Allocate a new renderTargetView if size is different
                    // Cache the rendertarget dimensions in our helper class for convenient use.
                    using (var backBuffer = surface.QueryInterface<SharpDX.Direct3D11.Texture2D>())
                    {
                        var desc = backBuffer.Description;
                        viewData.RenderTargetSize = new Size(desc.Width, desc.Height);
                        viewData.RenderTargetView = ToDispose(new SharpDX.Direct3D11.RenderTargetView(DeviceManager.DeviceDirect3D, backBuffer));
                    }

                    // Create a descriptor for the depth/stencil buffer.
                    // Allocate a 2-D surface as the depth/stencil buffer.
                    // Create a DepthStencil view on this surface to use on bind.
                    // TODO: Recreate a DepthStencilBuffer is inefficient. We should only have one depth buffer. Shared depth buffer?
                    using (var depthBuffer = new SharpDX.Direct3D11.Texture2D(DeviceManager.DeviceDirect3D, new SharpDX.Direct3D11.Texture2DDescription()
                    {
                        Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                        ArraySize = 1,
                        MipLevels = 1,
                        Width = (int)viewData.RenderTargetSize.Width,
                        Height = (int)viewData.RenderTargetSize.Height,
                        SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                        BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
                    }))
                        viewData.DepthStencilView = ToDispose(new SharpDX.Direct3D11.DepthStencilView(DeviceManager.DeviceDirect3D, depthBuffer, new SharpDX.Direct3D11.DepthStencilViewDescription() { Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D }));

                    // Create a viewport descriptor of the full window size.
                    viewData.Viewport = new SharpDX.Direct3D11.Viewport(position.X, position.Y, (float)viewData.RenderTargetSize.Width - position.X, (float)viewData.RenderTargetSize.Height - position.Y, 0.0f, 1.0f);
                }

                renderTargetView = viewData.RenderTargetView;
                depthStencilView = viewData.DepthStencilView;
                RenderTargetSize = viewData.RenderTargetSize;

                // Set the current viewport using the descriptor.
                DeviceManager.ContextDirect3D.Rasterizer.SetViewports(viewData.Viewport);

                base.RenderAll();
            }

            surfaceImageSourceNative.EndDraw();
        }


        class SurfaceViewData
        {
            public SharpDX.Direct3D11.RenderTargetView RenderTargetView;
            public SharpDX.Direct3D11.DepthStencilView DepthStencilView;
            public SharpDX.Direct3D11.Viewport Viewport;
            public Size RenderTargetSize;
        }
    }
}
