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

namespace D2DCustomVertexShaderEffect
{
    public class EffectRenderer : Component
    {
        private DeviceManager _deviceManager;
        private SharpDX.WIC.FormatConverter _formatConverter;

        private Brush sceneColorBrush;
        private SharpDX.Direct2D1.Effect _waveEffect;
        private SharpDX.Direct2D1.Effects.BitmapSource bitmapSourceEffect;

        private Windows.UI.Xaml.UIElement _root;
        private Windows.UI.Xaml.DependencyObject _rootParent;
        private Stopwatch clock;
        private Size2 imageSize;
        private Size2 screenSize;
        private float angleX;
        private float angleY;

        private Windows.UI.Input.GestureRecognizer gestureRecognizer;

        public EffectRenderer(Windows.UI.Xaml.UIElement rootForPointerEvents, Windows.UI.Xaml.UIElement rootOfLayout)
        {
            _root = rootForPointerEvents;
            _rootParent = rootOfLayout;
            EnableClear = true;
            Show = true;

            clock = new Stopwatch();

            gestureRecognizer = new Windows.UI.Input.GestureRecognizer();
            gestureRecognizer.GestureSettings = Windows.UI.Input.GestureSettings.ManipulationTranslateX |
                Windows.UI.Input.GestureSettings.ManipulationTranslateY |
                Windows.UI.Input.GestureSettings.ManipulationTranslateInertia;
            gestureRecognizer.ManipulationUpdated += gestureRecognizer_ManipulationUpdated;

            _root.PointerPressed += _root_PointerPressed;
            _root.PointerReleased += _root_PointerReleased;
            //_root.PointerEntered += _root_PointerPressed;
            //_root.PointerExited += _root_PointerReleased;
            _root.PointerMoved += _root_PointerMoved;
            _root.PointerWheelChanged += _root_PointerWheelChanged;
            angleX = 0.16f;
            angleY = 1.2f;

            clock.Start();
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

            UpdateAngle(0.0f, 0.0f);
        }

        private void Update()
        {
            float delta = clock.ElapsedMilliseconds / 1000.0f;
            _waveEffect.SetValue((int)WaveProperties.WaveOffset, delta);
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

            if (_waveEffect == null) return;

            UpdateSize(target);

            var context2D = target.DeviceManager.ContextDirect2D;

            Update();

            context2D.BeginDraw();

            if (EnableClear)
                context2D.Clear(Color.CornflowerBlue);

            context2D.DrawImage(_waveEffect);
            
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
            _deviceManager.FactoryDirect2D.RegisterEffect<WaveEffect>();
            _waveEffect = new Effect<WaveEffect>(_deviceManager.ContextDirect2D);
            _waveEffect.SetInputEffect(0, bitmapSourceEffect);
        }

        private void UpdateEffectGraph()
        {
            CreateEffectGraph(_formatConverter);
        }

        void _root_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            try
            {
                var test = e.GetCurrentPoint(null);
                double posX = test.Position.X;
                double posY = test.Position.Y;
                gestureRecognizer.ProcessDownEvent(test);
            }
            catch (Exception ex)
            {
                // We got somtines an exception saying that the value is not in the expected range, WHHHYYY?
            }
        }

        void _root_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            gestureRecognizer.ProcessMoveEvents(e.GetIntermediatePoints(null));
        }

        void _root_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            gestureRecognizer.ProcessUpEvent(e.GetCurrentPoint(null));
        }

        void _root_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            gestureRecognizer.ProcessMouseWheelEvent(e.GetCurrentPoint(null), false, true);
        }

        void gestureRecognizer_ManipulationUpdated(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.ManipulationUpdatedEventArgs args)
        {
            UpdateAngle((float)args.Delta.Translation.X, (float)args.Delta.Translation.Y);
        }

        private void UpdateAngle(float deltaX, float deltaY)
        {
            angleX += (float)deltaX * .01f;
            angleY += (float)deltaY * .01f;

            _waveEffect.SetValue((int)WaveProperties.AngleX, angleX);
            _waveEffect.SetValue((int)WaveProperties.AngleY, angleY);
        }
    }
}
