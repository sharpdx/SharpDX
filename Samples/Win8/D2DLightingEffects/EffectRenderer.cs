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

namespace D2DLightingEffects
{
    public class EffectRenderer : Component
    {
        private DeviceManager _deviceManager;
        private SharpDX.WIC.FormatConverter _formatConverter;

        private Brush sceneColorBrush;
        private SharpDX.Direct2D1.Effect _effectGraph;


        private SharpDX.Direct2D1.Effects.PointSpecular _pointSpecularEffect;
        private SharpDX.Direct2D1.Effects.SpotSpecular _spotSpecularEffect;
        private SharpDX.Direct2D1.Effects.DistantSpecular _distantSpecularEffect;
        private SharpDX.Direct2D1.Effects.PointDiffuse _pointDiffuseEffect;
        private SharpDX.Direct2D1.Effects.SpotDiffuse _spotDiffuseEffect;
        private SharpDX.Direct2D1.Effects.DistantDiffuse _distantDiffuseEffect;

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
        public float BlurDeviation { get; set; }
        public float _lightPositionZ { get; set; }



        public enum LightingEffect
        {
            PointSpecular,
            SpotSpecular,
            DistantSpecular,
            PointDiffuse,
            SpotDiffuse,
            DistantDiffuse
        };

        public enum LightingProperty
        {
            LightPositionZ,
            SpecularExponent,
            SpecularConstant,
            DiffuseConstant,
            Focus,
            LimitingConeAngle,
            Elevation,
            Azimuth,
            SurfaceScale
        };

        public virtual void Initialize(DeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, Color.White);
            
            //GET IMAGE DATA
            _formatConverter = DecodeImage();

            //DEFAULTS MAY NEED THIS DATA
            var size = deviceManager.ContextDirect2D.Size;
            int pixelWidth = (int)(size.Width * Windows.Graphics.Display.DisplayProperties.LogicalDpi / 96.0);
            int pixelHeight = (int)(size.Height * Windows.Graphics.Display.DisplayProperties.LogicalDpi / 96.0);


            //CREATE EFFECT-GRAPH USING IMAGE DATA
            Scale = new Vector2(0.8f, 0.8f);
            BlurDeviation = 0f;
            _lightPositionZ = 100f;
            PointsAt = new Vector3(pixelWidth / 2, pixelHeight / 2, 0.0f);


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
                                                                                        @"Assets\heightmap.png",
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



        private SharpDX.Direct2D1.Effect CreateEffectGraph(SharpDX.WIC.FormatConverter formatConverter, Vector2 scale, float blurDeviation)
        {
            // Setup local variables
            var d2dDevice = _deviceManager.DeviceDirect2D;
            var d2dContext = _deviceManager.ContextDirect2D;

            // Effect 1 : BitmapSource - take decoded image data and get a BitmapSource from it
            SharpDX.Direct2D1.Effects.BitmapSource bitmapSourceEffect = new SharpDX.Direct2D1.Effects.BitmapSource(d2dContext);
            bitmapSourceEffect.ScaleSource = scale;
            bitmapSourceEffect.WicBitmapSource = formatConverter;
            //bitmapSourceEffect.Cached = true; // Because the image will not be changing, we should cache the effect for performance reasons.

            // Effect 2 : PointSpecular
            _pointSpecularEffect = new SharpDX.Direct2D1.Effects.PointSpecular(d2dContext);
            _pointSpecularEffect.SetInput(0, bitmapSourceEffect.Output, true);

            // Effect 3 : SpotSpecular
            _spotSpecularEffect = new SharpDX.Direct2D1.Effects.SpotSpecular(d2dContext);
            _spotSpecularEffect.SetInput(0, bitmapSourceEffect.Output, true);

            // Effect 4 : DistantSpecular
            _distantSpecularEffect = new SharpDX.Direct2D1.Effects.DistantSpecular(d2dContext);
            _distantSpecularEffect.SetInput(0, bitmapSourceEffect.Output, true);

            // Effect 5 : PointDiffuse
            _pointDiffuseEffect = new SharpDX.Direct2D1.Effects.PointDiffuse(d2dContext);
            _pointDiffuseEffect.SetInput(0, bitmapSourceEffect.Output, true);

            // Effect 6 : SpotDiffuse
            _spotDiffuseEffect = new SharpDX.Direct2D1.Effects.SpotDiffuse(d2dContext);
            _spotDiffuseEffect.SetInput(0, bitmapSourceEffect.Output, true);

            // Effect 7 : DistantDiffuse
            _distantDiffuseEffect = new SharpDX.Direct2D1.Effects.DistantDiffuse(d2dContext);
            _distantDiffuseEffect.SetInput(0, bitmapSourceEffect.Output, true);


            return _pointSpecularEffect;

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

        private void UpdatePointer(float x, float y)
        {

            PointsAt = new Vector3(x, y, _lightPositionZ);
            _pointSpecularEffect.LightPosition = PointsAt;
            _spotSpecularEffect.LightPosition = PointsAt;
            _pointDiffuseEffect.LightPosition = PointsAt;
            _spotDiffuseEffect.LightPosition = PointsAt; 
            
        }


        void root_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var newPosition = e.GetCurrentPoint(null);

            
            var gtRoot = ((Windows.UI.Xaml.UIElement)_rootParent).TransformToVisual(_root);
            var rootPosition = gtRoot.TransformPoint(new Windows.Foundation.Point(newPosition.Position.X, newPosition.Position.Y));


            UpdatePointer((float)rootPosition.X, (float)rootPosition.Y);

        }



        public void SetLightingEffect(int lightingEffect)
        {

            switch (lightingEffect)
            {
                case (int)LightingEffect.PointSpecular:
                    _effectGraph = _pointSpecularEffect;
                    break;
                case (int)LightingEffect.SpotSpecular:
                    _effectGraph = _spotSpecularEffect;
                    break;
                case (int)LightingEffect.DistantSpecular:
                    _effectGraph = _distantSpecularEffect;
                    break;
                case (int)LightingEffect.PointDiffuse:
                    _effectGraph = _pointDiffuseEffect;
                    break;
                case (int)LightingEffect.SpotDiffuse:
                    _effectGraph = _spotDiffuseEffect;
                    break;
                case (int)LightingEffect.DistantDiffuse:
                    _effectGraph = _distantDiffuseEffect;
                    break;
                default:
            
                    break;
            }

        }



        public void SetLightingProperty(LightingProperty lightingProperty, float value)
        {


            // Not all effects have all properties. For example, the Specular effects do not have a DiffuseConstant property.
            switch (lightingProperty)
            {
                case LightingProperty.LightPositionZ:
                    _lightPositionZ = value;
                    break;
                case LightingProperty.SpecularConstant:
                    _pointSpecularEffect.SpecularConstant = value;
                    _spotSpecularEffect.SpecularConstant = value;
                    _distantSpecularEffect.SpecularConstant = value;
                    break;
                case LightingProperty.SpecularExponent:
                    _pointSpecularEffect.SpecularExponent = value;
                    _spotSpecularEffect.SpecularExponent = value;
                    _distantSpecularEffect.SpecularExponent = value;
                    break;
                case LightingProperty.DiffuseConstant:
                    _pointDiffuseEffect.DiffuseConstant = value;
                    _spotDiffuseEffect.DiffuseConstant = value;
                    _distantDiffuseEffect.DiffuseConstant = value;
                    break;
                case LightingProperty.Focus:
                    _spotSpecularEffect.Focus = value;
                    _spotDiffuseEffect.Focus = value;
                    break;
                case LightingProperty.LimitingConeAngle:
                    _spotSpecularEffect.LimitingConeAngle = value;
                    _spotDiffuseEffect.LimitingConeAngle = value;
                    break;
                case LightingProperty.Azimuth:
                    _distantSpecularEffect.Azimuth = value;
                    _distantDiffuseEffect.Azimuth = value;
                    break;
                case LightingProperty.Elevation:
                    _distantSpecularEffect.Elevation = value;
                    _distantDiffuseEffect.Elevation = value;
                    break;
                case LightingProperty.SurfaceScale:
                    _pointSpecularEffect.SurfaceScale = value;
                    _spotSpecularEffect.SurfaceScale = value;
                    _distantSpecularEffect.SurfaceScale = value;
                    _pointDiffuseEffect.SurfaceScale = value;
                    _spotDiffuseEffect.SurfaceScale = value;
                    _distantDiffuseEffect.SurfaceScale = value;
                    break;
                default:
            
                    break;
            }

        }



    }
}
