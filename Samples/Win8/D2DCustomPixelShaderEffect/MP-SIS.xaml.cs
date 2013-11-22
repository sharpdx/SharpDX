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
using SharpDX;
using SharpDX.IO;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace D2DCustomPixelShaderEffect
{
    using Windows.UI.Core;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MPSIS : Page
    {
        private DeviceManager deviceManager;

        private ImageBrush d2dBrush;
        private SurfaceImageSourceTarget d2dTarget;
        private DragHandler d2dDragHandler;

        private EffectRenderer effectRenderer;

        public MPSIS()
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
            d2dDragHandler = new DragHandler(d2dCanvas) { CursorOver = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeAll, 1) };

            d2dBrush = new ImageBrush();
            d2dRectangle.Fill = d2dBrush;

            effectRenderer = new EffectRenderer(d2dRectangle, root);
            
            deviceManager = new DeviceManager();
            
            int pixelWidth = (int)(d2dRectangle.Width * DisplayProperties.LogicalDpi / 96.0);
            int pixelHeight = (int)(d2dRectangle.Height * DisplayProperties.LogicalDpi / 96.0);

            d2dTarget = new SurfaceImageSourceTarget(pixelWidth, pixelHeight);
            d2dBrush.ImageSource = d2dTarget.ImageSource;
            d2dTarget.OnRender += effectRenderer.Render;
            
            deviceManager.OnInitialize += d2dTarget.Initialize;
            deviceManager.OnInitialize += effectRenderer.Initialize;
            deviceManager.Initialize(DisplayProperties.LogicalDpi);

            var window = CoreWindow.GetForCurrentThread();
            if (window.Visible)
                BindRenderingEvents();

            window.VisibilityChanged += (_, args) =>
                                        {
                                            if(args.Visible)
                                                BindRenderingEvents();
                                            else
                                                UnbindRenderingEvents();
                                        };
        }

        private void BindRenderingEvents()
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void UnbindRenderingEvents()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            d2dTarget.RenderAll();
        }
    }
}
