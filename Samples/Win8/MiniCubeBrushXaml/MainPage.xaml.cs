// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using CommonDX;
using MiniCube;
using MiniShape;
using SharpDX;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Input;
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
        private ImageBrush d3dBrush;
        private ImageBrush d2dBrush;
        private DeviceManager deviceManager;
        private SurfaceImageSourceTarget d3dTarget;
        private SurfaceImageSourceTarget d2dTarget;
        private CubeRenderer cubeRenderer;
        private ShapeRenderer shapeRenderer;
        private DragHandler d3dDragHandler;
        private DragHandler d2dDragHandler;

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
            d3dDragHandler = new DragHandler(d3dCanvas) { CursorOver = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeAll, 1) };
            d2dDragHandler = new DragHandler(d2dCanvas) { CursorOver = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeAll, 1) };

            d3dBrush = new ImageBrush();
            d3dRectangle.Fill = d3dBrush;

            d2dBrush = new ImageBrush();
            d2dRectangle.Fill = d2dBrush;

            // Safely dispose any previous instance
            // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
            deviceManager = new DeviceManager();

            // New CubeRenderer
            cubeRenderer = new CubeRenderer();
            shapeRenderer = new ShapeRenderer();

            int pixelWidth = (int)(d3dRectangle.Width * DisplayProperties.LogicalDpi / 96.0);
            int pixelHeight = (int)(d3dRectangle.Height * DisplayProperties.LogicalDpi / 96.0);

            // Use CoreWindowTarget as the rendering target (Initialize SwapChain, RenderTargetView, DepthStencilView, BitmapTarget)
            d3dTarget = new SurfaceImageSourceTarget(pixelWidth, pixelHeight);
            d3dBrush.ImageSource = d3dTarget.ImageSource;

            d2dTarget = new SurfaceImageSourceTarget(pixelWidth, pixelHeight);
            d2dBrush.ImageSource = d2dTarget.ImageSource;

            // Add Initializer to device manager
            deviceManager.OnInitialize += d3dTarget.Initialize;
            deviceManager.OnInitialize += d2dTarget.Initialize;
            deviceManager.OnInitialize += cubeRenderer.Initialize;
            deviceManager.OnInitialize += shapeRenderer.Initialize;

            // Render the cube within the CoreWindow
            d3dTarget.OnRender += cubeRenderer.Render;
            d2dTarget.OnRender += shapeRenderer.Render;

            // Initialize the device manager and all registered deviceManager.OnInitialize 
            deviceManager.Initialize(DisplayProperties.LogicalDpi);

            CoreWindow.GetForCurrentThread().VisibilityChanged += (_, args) =>
                                                                  {
                                                                      if (args.Visible)
                                                                          BindRenderingEvents();
                                                                      else
                                                                          UnbindRenderingEvents();
                                                                  };
        }

        private void BindRenderingEvents()
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            DisplayProperties.LogicalDpiChanged += DisplayProperties_LogicalDpiChanged;
        }

        private void UnbindRenderingEvents()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            DisplayProperties.LogicalDpiChanged -= DisplayProperties_LogicalDpiChanged;
        }

        void DisplayProperties_LogicalDpiChanged(object sender)
        {
            deviceManager.Dpi = DisplayProperties.LogicalDpi;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            d3dTarget.RenderAll();
            d2dTarget.RenderAll();
        }
    }
}
