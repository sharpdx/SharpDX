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
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
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

namespace D2DEffectsHelloWorld
{


    public sealed partial class MPSCBP : SwapChainBackgroundPanel
    {
        private DeviceManager deviceManager;

        private ImageBrush d2dBrush;
        private SwapChainBackgroundPanelTarget d2dTarget;


        private EffectRenderer effectRenderer;


        public MPSCBP()
        {
            this.InitializeComponent();


            effectRenderer = new EffectRenderer();
            var fpsRenderer = new FpsRenderer();

            d2dTarget = new SwapChainBackgroundPanelTarget(rootSCBP);
            d2dTarget.OnRender += effectRenderer.Render;
            d2dTarget.OnRender += fpsRenderer.Render;

            deviceManager = new DeviceManager();
            deviceManager.OnInitialize += d2dTarget.Initialize;
            deviceManager.OnInitialize += effectRenderer.Initialize;
            deviceManager.OnInitialize += fpsRenderer.Initialize;
           
            deviceManager.Initialize(DisplayProperties.LogicalDpi);

            // Setup rendering callback
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {

            d2dTarget.RenderAll();
            d2dTarget.Present();
        }

        private void BlurDeviationChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (effectRenderer != null) effectRenderer.UpdateBlurDeviation((float)e.NewValue);
        }

        private void ScaleChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (effectRenderer != null) effectRenderer.UpdateScaleDeviation((float)e.NewValue);
        }
    }
}
