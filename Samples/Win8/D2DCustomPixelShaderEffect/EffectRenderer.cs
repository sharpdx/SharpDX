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

namespace D2DCustomPixelShaderEffect
{
    public class EffectRenderer : Component
    {
        private DeviceManager _deviceManager;
        private SharpDX.WIC.FormatConverter _formatConverter;

        private Brush sceneColorBrush;
        private SharpDX.Direct2D1.Effect _rippleEffect;
        private SharpDX.Direct2D1.Effects.BitmapSource bitmapSourceEffect;

        private Windows.UI.Xaml.UIElement _root;
        private Windows.UI.Xaml.DependencyObject _rootParent;
        private Stopwatch clock;
        private Size2 imageSize;
        private Size2 screenSize;

        public EffectRenderer(Windows.UI.Xaml.UIElement rootForPointerEvents, Windows.UI.Xaml.UIElement rootOfLayout)
        {
            _root = rootForPointerEvents;
            _rootParent = rootOfLayout;
            EnableClear = false;
            Show = true;

            clock = new Stopwatch();

            _root.PointerMoved += _root_PointerMoved;
            _root.PointerPressed += _root_PointerPressed;
            _root.PointerReleased += _root_PointerReleased;
        }

        public bool EnableClear { get; set; }

        public bool Show { get; set; }
        public Vector3 PointsAt { get; set; }

        public virtual void Initialize(DeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, Color.White);
            
            //GET IMAGE DATA
            _formatConverter = DecodeImage();

            //CREATE EFFECT-GRAPH USING IMAGE DATA
            UpdateEffectGraph();
        }

        private void Update()
        {
            float delta = clock.ElapsedMilliseconds / 1000.0f;

            if (!clock.IsRunning || delta > 4)
            {
                delta = 4;
                clock.Stop();
            }

            _rippleEffect.SetValue((int)RippleProperties.Frequency, 140.0f - delta * 30.0f);

            _rippleEffect.SetValue((int)RippleProperties.Phase, -delta * 20.0f);

            _rippleEffect.SetValue((int)RippleProperties.Amplitude, 60.0f - delta * 15.0f);

            _rippleEffect.SetValue((int)RippleProperties.Spread, 0.01f + delta / 10.0f);
        }

        private void UpdateSize(TargetBase target)
        {
            var localSize = new Size2((int)target.RenderTargetSize.Width, (int)target.RenderTargetSize.Height);
            if (localSize != screenSize)
            {
                screenSize = localSize;
                bitmapSourceEffect.ScaleSource = new Vector2((float)screenSize.Width / imageSize.Width, (float)screenSize.Height / imageSize.Height);
            }
        }

        public virtual void Render(TargetBase target)
        {
            if (!Show)
                return;

            if (_rippleEffect == null) return;

            UpdateSize(target);

            var context2D = target.DeviceManager.ContextDirect2D;

            Update();

            context2D.BeginDraw();

            if (EnableClear)
                context2D.Clear(Color.CornflowerBlue);

            context2D.DrawImage(_rippleEffect);
            
            context2D.EndDraw();
        }

        private SharpDX.WIC.FormatConverter DecodeImage()
        {
            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

            SharpDX.WIC.BitmapDecoder bitmapDecoder = new SharpDX.WIC.BitmapDecoder(
                                                                                        _deviceManager.WICFactory,
                                                                                        @"Assets\Nepal.jpg",
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

            imageSize = formatConverter.Size;
            
            return formatConverter;
        }

        private void CreateEffectGraph(SharpDX.WIC.FormatConverter formatConverter)
        {
            // Setup local variables
            var d2dDevice = _deviceManager.DeviceDirect2D;
            var d2dContext = _deviceManager.ContextDirect2D;

            // Effect 1 : BitmapSource - take decoded image data and get a BitmapSource from it
            bitmapSourceEffect = new SharpDX.Direct2D1.Effects.BitmapSource(d2dContext);
            bitmapSourceEffect.WicBitmapSource = formatConverter;
            bitmapSourceEffect.Cached = true; // Because the image will not be changing, we should cache the effect for performance reasons.
            
            // Effect 2 : PointSpecular
            _deviceManager.FactoryDirect2D.RegisterEffect<RippleEffect>();
            _rippleEffect = new Effect<RippleEffect>(_deviceManager.ContextDirect2D);
            _rippleEffect.SetInputEffect(0, bitmapSourceEffect);
        }

        private void UpdateEffectGraph()
        {
            
            CreateEffectGraph(_formatConverter);
        }

        private void UpdatePointer(float x, float y)
        {

            PointsAt = new Vector3(x, y, 0);
            _rippleEffect.SetValue((int)RippleProperties.Center, new Vector2(x, y));
        }

        private bool pointerPressed = false;

        void _root_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            pointerPressed = true;
            SetRipplePosition(e);
        }

        void _root_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (pointerPressed)
                SetRipplePosition(e);
        }

        void _root_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            pointerPressed = false;
        }

        private void SetRipplePosition(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var newPosition = e.GetCurrentPoint(null);

            var gtRoot = ((Windows.UI.Xaml.UIElement)_rootParent).TransformToVisual(_root);
            var rootPosition = gtRoot.TransformPoint(new Windows.Foundation.Point(newPosition.Position.X, newPosition.Position.Y));


            UpdatePointer((float)rootPosition.X, (float)rootPosition.Y);

            clock.Restart();

       }
    }
}
