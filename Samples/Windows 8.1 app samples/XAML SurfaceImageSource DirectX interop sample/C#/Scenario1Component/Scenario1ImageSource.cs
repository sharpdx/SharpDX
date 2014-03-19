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
// -----------------------------------------------------------------------------
// This is a port of the sample "XAML SurfaceImageSource DirectX interop sample
// from the Windows 8.1 SDK
// -----------------------------------------------------------------------------

//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Windows.UI.Xaml;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;

namespace Scenario1Component
{
    public sealed class Scenario1ImageSource : Windows.UI.Xaml.Media.Imaging.SurfaceImageSource
    {
        private Device d3dDevice;
        private SharpDX.Direct2D1.Device d2dDevice;
        private SharpDX.Direct2D1.DeviceContext d2dContext;
        private readonly int width;
        private readonly int height;

        public Scenario1ImageSource(int pixelWidth, int pixelHeight, bool isOpaque)
            : base(pixelWidth, pixelHeight, isOpaque)
        {
            width = pixelWidth;
            height = pixelHeight;

            CreateDeviceResources();

            Application.Current.Suspending += OnSuspending;
        }

        // Initialize hardware-dependent resources.
        private void CreateDeviceResources()
        {
            // Unlike the original C++ sample, we don't have smart pointers so we need to
            // dispose Direct3D objects explicitly
            Utilities.Dispose(ref d3dDevice);
            Utilities.Dispose(ref d2dDevice);
            Utilities.Dispose(ref d2dContext);

            // This flag adds support for surfaces with a different color channel ordering
            // than the API default. It is required for compatibility with Direct2D.
            var creationFlags = DeviceCreationFlags.BgraSupport;

#if DEBUG
            // If the project is in a debug build, enable debugging via SDK Layers.
            creationFlags |= DeviceCreationFlags.Debug;
#endif

            // This array defines the set of DirectX hardware feature levels this app will support.
            // Note the ordering should be preserved.
            // Don't forget to declare your application's minimum required feature level in its
            // description.  All applications are assumed to support 9.1 unless otherwise stated.
            FeatureLevel[] featureLevels =
            {
                FeatureLevel.Level_11_1,
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3,
                FeatureLevel.Level_9_2,
                FeatureLevel.Level_9_1,
            };

            // Create the Direct3D 11 API device object.
            d3dDevice = new Device(DriverType.Hardware, creationFlags, featureLevels);

            // Get the Direct3D 11.1 API device.
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
            {
                // Create the Direct2D device object and a corresponding context.
                d2dDevice = new SharpDX.Direct2D1.Device(dxgiDevice);

                d2dContext = new SharpDX.Direct2D1.DeviceContext(d2dDevice, DeviceContextOptions.None);

                // Query for ISurfaceImageSourceNative interface.
                using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
                    sisNative.Device = dxgiDevice;
            }
        }

        public void BeginDraw()
        {
            BeginDraw(new Windows.Foundation.Rect(0, 0, width, height));
        }
        
        public void BeginDraw(Windows.Foundation.Rect updateRect)
        {
            // Express target area as a native RECT type.
            var updateRectNative = new Rectangle
            {
                Left = (int) updateRect.Left,
                Top = (int) updateRect.Top,
                Right = (int) updateRect.Right,
                Bottom = (int) updateRect.Bottom
            };

            // Query for ISurfaceImageSourceNative interface.
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
            {
                // Begin drawing - returns a target surface and an offset to use as the top left origin when drawing.
                try
                {
                    Point offset;
                    using (var surface = sisNative.BeginDraw(updateRectNative, out offset))
                    {

                        // Create render target.
                        using (var bitmap = new Bitmap1(d2dContext, surface))
                        {
                            // Set context's render target.
                            d2dContext.Target = bitmap;
                        }

                        // Begin drawing using D2D context.
                        d2dContext.BeginDraw();

                        // Apply a clip and transform to constrain updates to the target update area.
                        // This is required to ensure coordinates within the target surface remain
                        // consistent by taking into account the offset returned by BeginDraw, and
                        // can also improve performance by optimizing the area that is drawn by D2D.
                        // Apps should always account for the offset output parameter returned by 
                        // BeginDraw, since it may not match the passed updateRect input parameter's location.
                        d2dContext.PushAxisAlignedClip(
                            new RectangleF(
                                (offset.X),
                                (offset.Y),
                                (offset.X + (float) updateRect.Width),
                                (offset.Y + (float) updateRect.Height)
                                ),
                            AntialiasMode.Aliased
                            );

                        d2dContext.Transform = Matrix3x2.Translation(offset.X, offset.Y);
                    }
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceRemoved ||
                        ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceReset)
                    {
                        // If the device has been removed or reset, attempt to recreate it and continue drawing.
                        CreateDeviceResources();
                        BeginDraw(updateRect);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void EndDraw()
        {
            // Remove the transform and clip applied in BeginDraw since
            // the target area can change on every update.
            d2dContext.Transform = Matrix3x2.Identity;
            d2dContext.PopAxisAlignedClip();

            // Remove the render target and end drawing.
            d2dContext.EndDraw();

            d2dContext.Target = null;

            // Query for ISurfaceImageSourceNative interface.
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
                sisNative.EndDraw();
        }

        public void Clear(Windows.UI.Color color)
        {
            d2dContext.Clear(ConvertToColorF(color));
        }

        public void FillSolidRect(Windows.UI.Color color, Windows.Foundation.Rect rect)
        {
            // Create a solid color D2D brush.
            using (var brush = new SolidColorBrush(d2dContext, ConvertToColorF(color)))
            {
                // Draw a filled rectangle.
                d2dContext.FillRectangle(ConvertToRectF(rect), brush);
            }
        }

        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // Hints to the driver that the app is entering an idle state and that its memory can be used temporarily for other apps.
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device3>())
                dxgiDevice.Trim();
        }

        private static Color ConvertToColorF(Windows.UI.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        private static RectangleF ConvertToRectF(Windows.Foundation.Rect rect)
        {
            return new RectangleF((float) rect.X, (float) rect.Y, (float) rect.Width, (float) rect.Height);
        }
    }
}
