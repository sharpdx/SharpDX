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
using System.IO;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.WIC;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.WIC.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace RenderToWicApp
{
    /// <summary>
    /// SharpDX Direct2D Software rendering output to WIC Bitmap.
    /// </summary>
    internal static class Program
    {
        private static void Main()
        {
            var wicFactory = new ImagingFactory();
            var d2dFactory = new SharpDX.Direct2D1.Factory();

            string filename = "output.jpg";
            const int width = 512;
            const int height = 512;

            var rectangleGeometry = new RoundedRectangleGeometry(d2dFactory, new RoundedRectangle() { RadiusX = 32, RadiusY = 32, Rect = new RectangleF(128, 128, width - 128 * 2, height-128 * 2) });

            var wicBitmap = new Bitmap(wicFactory, width, height, SharpDX.WIC.PixelFormat.Format32bppBGR, BitmapCreateCacheOption.CacheOnLoad);

            var renderTargetProperties = new RenderTargetProperties(RenderTargetType.Default, new PixelFormat(Format.Unknown, AlphaMode.Unknown), 0, 0, RenderTargetUsage.None, FeatureLevel.Level_DEFAULT);

            var d2dRenderTarget = new WicRenderTarget(d2dFactory, wicBitmap, renderTargetProperties);

            var solidColorBrush = new SolidColorBrush(d2dRenderTarget, Color.White);

            d2dRenderTarget.BeginDraw();
            d2dRenderTarget.Clear(Color.Black);
            d2dRenderTarget.FillGeometry(rectangleGeometry, solidColorBrush, null);
            d2dRenderTarget.EndDraw();

            if (File.Exists(filename))
                File.Delete(filename);

            var stream = new WICStream(wicFactory, filename, NativeFileAccess.Write);
            // Initialize a Jpeg encoder with this stream
            var encoder = new JpegBitmapEncoder(wicFactory);
            encoder.Initialize(stream);

            // Create a Frame encoder
            var bitmapFrameEncode = new BitmapFrameEncode(encoder);
            bitmapFrameEncode.Initialize();
            bitmapFrameEncode.SetSize(width, height);
            var pixelFormatGuid = SharpDX.WIC.PixelFormat.FormatDontCare;
            bitmapFrameEncode.SetPixelFormat(ref pixelFormatGuid);
            bitmapFrameEncode.WriteSource(wicBitmap);

            bitmapFrameEncode.Commit();
            encoder.Commit();

            bitmapFrameEncode.Dispose();
            encoder.Dispose();
            stream.Dispose();

            System.Diagnostics.Process.Start(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory,filename)));
        }
    }
}