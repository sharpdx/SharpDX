// Copyright (c) 2010-2011 SharpDX - Jose Fajardo
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
using System.Threading.Tasks;
using CommonDX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Matrix = SharpDX.Matrix;

namespace D2DEffectsHelloWorld
{
    public class EffectRenderer : Component
    {
        
        Brush sceneColorBrush;
        SharpDX.Direct2D1.Effect _effectGraph;
        
        public EffectRenderer()
        {
            EnableClear = false;
            Show = true;
        }

        public bool EnableClear { get; set; }

        public bool Show { get; set; }
        public Vector2 Scale { get; set; }
        public float BlurDeviation { get; set; }

        private DeviceManager _deviceManager;
        private SharpDX.WIC.FormatConverter _formatConverter;


        public virtual void Initialize(DeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, Color.White);
            
            //GET IMAGE DATA
            _formatConverter = DecodeImage();

            //CREATE EFFECT-GRAPH USING IMAGE DATA
            Scale = new Vector2(0.7f, 0.7f);
            BlurDeviation = 0f;
            UpdateEffectGraph();



            
        }


        public virtual void Render(TargetBase target)
        {
            if (!Show)
                return;

            if (_effectGraph == null) return;

            var context2D = target.DeviceManager.ContextDirect2D;

            context2D.BeginDraw();

            if (EnableClear)
                context2D.Clear(Color.Black);

            context2D.Clear(Color.Transparent);

            context2D.DrawImage(_effectGraph.Output, InterpolationMode.Linear, CompositeMode.DestinationAtop);


            context2D.EndDraw();
        }



        private SharpDX.WIC.FormatConverter DecodeImage()
        {
            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

            SharpDX.WIC.BitmapDecoder bitmapDecoder = new SharpDX.WIC.BitmapDecoder(
                                                                                        _deviceManager.WICFactory,
                                                                                        @"Assets\SydneyOperaHouse002.png",
                                                                                        SharpDX.IO.NativeFileAccess.Read,
                                                                                        SharpDX.WIC.DecodeOptions.CacheOnDemand
                                                                                    );

            SharpDX.WIC.BitmapFrameDecode bitmapFrameDecode = bitmapDecoder.GetFrame(0);

            SharpDX.WIC.BitmapSource bitmapSource = new SharpDX.WIC.BitmapSource(bitmapFrameDecode.NativePointer);

            SharpDX.WIC.FormatConverter formatConverter = new SharpDX.WIC.FormatConverter(_deviceManager.WICFactory);
            formatConverter.Initialize(bitmapSource, SharpDX.WIC.PixelFormat.Format32bppBGRA);

            return formatConverter;
        }


        private SharpDX.Direct2D1.Effect CreateEffectGraph(SharpDX.WIC.FormatConverter formatConverter, Vector2 scale, float blurDeviation)
        {
            // Setup local variables
            var d2dDevice = _deviceManager.DeviceDirect2D;
            var d2dContext = _deviceManager.ContextDirect2D;

            // Effect 1 : BitmapSource - take decoded image data and get a BitmapSource from it
            SharpDX.Direct2D1.Effects.BitmapSource bitmapSourceEffect = new SharpDX.Direct2D1.Effects.BitmapSource(d2dContext);
            bitmapSourceEffect.ScaleSource = scale;
            bitmapSourceEffect.WicBitmapSource = formatConverter;

            // Effect 2 : GaussianBlur - give the bitmapsource a gaussian blurred effect
            SharpDX.Direct2D1.Effects.GaussianBlur gaussianBlurEffect = new SharpDX.Direct2D1.Effects.GaussianBlur(d2dContext);
            gaussianBlurEffect.SetInput(0, bitmapSourceEffect.Output, true);
            gaussianBlurEffect.StandardDeviation = blurDeviation;

            return gaussianBlurEffect;

        }


        public void UpdateBlurDeviation(float deviation)
        {
            BlurDeviation = deviation;

            UpdateEffectGraph();

        }


        public void UpdateScaleDeviation(float factor)
        {
            Scale = new Vector2(factor,factor);
            
            UpdateEffectGraph();

        }

        private void UpdateEffectGraph()
        {
            
            _effectGraph = CreateEffectGraph(
                                _formatConverter,
                                Scale,
                                BlurDeviation
                              );
        }
    }
}
