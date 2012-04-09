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

namespace CustomPixelShaderEffect
{
    public class EffectRenderer : Component
    {
        private DeviceManager _deviceManager;
        private SharpDX.WIC.FormatConverter _formatConverter;

        private Brush sceneColorBrush;
        private SharpDX.Direct2D1.CustomEffect _effectGraph;


        private RippleEffect _rippleEffect;
        
        private Windows.UI.Xaml.UIElement _root;
        private Windows.UI.Xaml.DependencyObject _rootParent;

        public EffectRenderer(Windows.UI.Xaml.UIElement rootForPointerEvents, Windows.UI.Xaml.UIElement rootOfLayout)
        {
            _root = rootForPointerEvents;
            _rootParent = rootOfLayout;
            EnableClear = false;
            Show = true;

            _root.PointerMoved += root_PointerMoved;
        }



        public bool EnableClear { get; set; }

        public bool Show { get; set; }
        public Vector2 Scale { get; set; }
        public Vector3 PointsAt { get; set; }





        public virtual void Initialize(DeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, Colors.White);
            
            //GET IMAGE DATA
            _formatConverter = DecodeImage();

            //DEFAULTS MAY NEED THIS DATA
            var size = deviceManager.ContextDirect2D.Size;
            int pixelWidth = (int)(size.Width * Windows.Graphics.Display.DisplayProperties.LogicalDpi / 96.0);
            int pixelHeight = (int)(size.Height * Windows.Graphics.Display.DisplayProperties.LogicalDpi / 96.0);


            //CREATE EFFECT-GRAPH USING IMAGE DATA
            Scale = new Vector2(0.8f, 0.8f);

            PointsAt = new Vector3(pixelWidth / 2, pixelHeight / 2, 0.0f);


            UpdateEffectGraph();

        }


        public virtual void Render(TargetBase target)
        {
            return;
            if (!Show)
                return;

            if (_effectGraph == null) return;

            var context2D = target.DeviceManager.ContextDirect2D;

            context2D.BeginDraw();

            if (EnableClear)
                context2D.Clear(Colors.Black);

            context2D.Clear(Colors.Transparent);

            //context2D.DrawImage(_effectGraph.Output, InterpolationMode.Linear, CompositeMode.DestinationAtop);


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
            //formatConverter.Initialize( bitmapSource, SharpDX.WIC.PixelFormat.Format32bppBGRA);
            formatConverter.Initialize(
                bitmapSource,
                SharpDX.WIC.PixelFormat.Format32bppBGRA, 
                SharpDX.WIC.BitmapDitherType.None, 
                null, 
                0.0f, 
                SharpDX.WIC.BitmapPaletteType.Custom
                ); 
            
            return formatConverter;
        }



        private SharpDX.Direct2D1.CustomEffect CreateEffectGraph(SharpDX.WIC.FormatConverter formatConverter, Vector2 scale)
        {
            // Setup local variables
            var d2dDevice = _deviceManager.DeviceDirect2D;
            var d2dContext = _deviceManager.ContextDirect2D;

            // Effect 1 : BitmapSource - take decoded image data and get a BitmapSource from it
            SharpDX.Direct2D1.Effects.BitmapSourceEffect bitmapSourceEffect = new SharpDX.Direct2D1.Effects.BitmapSourceEffect(d2dContext);
            bitmapSourceEffect.ScaleSource = scale;
            bitmapSourceEffect.WicBitmapSource = formatConverter;
            //bitmapSourceEffect.Cached = true; // Because the image will not be changing, we should cache the effect for performance reasons.

            _deviceManager.FactoryDirect2D.RegisterEffect<RippleEffect>();
            

            // Effect 2 : PointSpecular
            _rippleEffect = new RippleEffect();

            
            return _rippleEffect;

        }



        private void UpdateEffectGraph()
        {
            
            _effectGraph = CreateEffectGraph(
                                _formatConverter,
                                Scale
                              );
        }

        private void UpdatePointer(float x, float y)
        {

            PointsAt = new Vector3(x, y, 0);
            //_rippleEffect.LightPosition = PointsAt;
            
        }


        void root_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerEventArgs e)
        {
            var newPosition = e.GetCurrentPoint(null);

            
            var gtRoot = ((Windows.UI.Xaml.UIElement)_rootParent).TransformToVisual(_root);
            var rootPosition = gtRoot.TransformPoint(new Windows.Foundation.Point(newPosition.Position.X, newPosition.Position.Y));


            UpdatePointer((float)rootPosition.X, (float)rootPosition.Y);

        }









    }
}
