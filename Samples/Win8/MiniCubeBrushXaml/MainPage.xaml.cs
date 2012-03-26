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
using System.IO;
using System.Linq;
using SharpDX;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MiniCubeBrushXaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CubeRendererBrushXaml cubeRenderer;
        private ImageBrush d3dBrush;
        private SurfaceImageSource d3dImageSource;
        private SharpDX.DXGI.ISurfaceImageSourceNative d3dImageSourceNative;

        /// <summary>
        /// Initialize a new instance of <see cref="MainPage"/>
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            cubeRenderer = new CubeRendererBrushXaml();

            d3dBrush = new ImageBrush();
            d3dRectangle.Fill = d3dBrush;

            // Initialize our DirectXBase without a window
            cubeRenderer.Initialize(null, DisplayProperties.LogicalDpi);

            // Setup rendering callback
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            // Callback on DpiChanged
            DisplayProperties.LogicalDpiChanged += DisplayProperties_LogicalDpiChanged;
        }

        void DisplayProperties_LogicalDpiChanged(object sender)
        {
            cubeRenderer.Dpi = DisplayProperties.LogicalDpi;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            int pixelWidth = (int)(d3dRectangle.Width * DisplayProperties.LogicalDpi / 96.0);
            int pixelHeight = (int)(d3dRectangle.Height * DisplayProperties.LogicalDpi / 96.0);
            if (d3dImageSource == null)
            {
                d3dImageSource = new SurfaceImageSource(pixelWidth, pixelHeight );
                d3dImageSourceNative = ComObject.As<SharpDX.DXGI.ISurfaceImageSourceNative>(d3dImageSource);
                d3dImageSourceNative.Device = cubeRenderer.Device.QueryInterface<SharpDX.DXGI.Device>();
                d3dBrush.ImageSource = d3dImageSource;
            }
           
            DrawingPoint point;
            var regionToDraw = new Rectangle(0, 0, pixelWidth, pixelHeight);
            using (var surface = d3dImageSourceNative.BeginDraw(regionToDraw, out point))
            {
                cubeRenderer.BrushSurface = surface;
                cubeRenderer.Render();
            }
            d3dImageSourceNative.EndDraw();
        }
    }
}
