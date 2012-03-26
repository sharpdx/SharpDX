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
using SharpDX.DXGI;
using MiniCube;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace MiniCubeBrushXaml
{
    /// <summary>
    /// CubeRenderer to a XAML panel.
    /// </summary>
    /// <remarks>
    /// This class only overrides the swap chain description and creation methods.
    /// </remarks>
    public class CubeRendererBrushXaml : CubeRenderer
    {
        private Dictionary<IntPtr, SurfaceViewData> mapSurfaces = new Dictionary<IntPtr, SurfaceViewData>();

        public SharpDX.DXGI.Surface BrushSurface { get; set; }

        public SharpDX.Direct3D11.Device Device { get { return d3dDevice; } }

        public Point Position { get; set; }

        public override void UpdateForWindowSizeChange()
        {
        }

        protected override void CreateWindowSizeDependentResources()
        {
        }

        public override void Render()
        {
            SurfaceViewData viewData;
            if (!mapSurfaces.TryGetValue(BrushSurface.NativePointer, out viewData))
            {
                viewData = new SurfaceViewData();
                mapSurfaces.Add(BrushSurface.NativePointer, viewData);

                // Allocate a new renderTargetView if size is different
                // Cache the rendertarget dimensions in our helper class for convenient use.
                using (var backBuffer = BrushSurface.QueryInterface<SharpDX.Direct3D11.Texture2D>())
                {
                    var desc = backBuffer.Description;
                    viewData.RenderTargetSize = new Size(desc.Width, desc.Height);
                    viewData.RenderTargetView = new SharpDX.Direct3D11.RenderTargetView(d3dDevice, backBuffer);
                }

                // Create a descriptor for the depth/stencil buffer.
                // Allocate a 2-D surface as the depth/stencil buffer.
                // Create a DepthStencil view on this surface to use on bind.
                using (var depthBuffer = new SharpDX.Direct3D11.Texture2D(d3dDevice, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = (int)viewData.RenderTargetSize.Width,
                    Height = (int)viewData.RenderTargetSize.Height,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
                }))
                    viewData.DepthStencilView = new SharpDX.Direct3D11.DepthStencilView(d3dDevice, depthBuffer, new SharpDX.Direct3D11.DepthStencilViewDescription() { Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D });

                // Create a viewport descriptor of the full window size.
                viewData.Viewport = new SharpDX.Direct3D11.Viewport(0, 0, (float)viewData.RenderTargetSize.Width, (float)viewData.RenderTargetSize.Height, 0.0f, 1.0f);
            }

            renderTargetView = viewData.RenderTargetView;
            depthStencilView = viewData.DepthStencilView;
            renderTargetSize = viewData.RenderTargetSize;

            // Set the current viewport using the descriptor.
            d3dContext.Rasterizer.SetViewports(viewData.Viewport);

            // Perform actual rendering of the scene
            base.Render();
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
