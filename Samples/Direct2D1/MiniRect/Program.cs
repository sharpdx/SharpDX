// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using SharpDX.Windows;

using Device1 = SharpDX.Direct3D10.Device1;
using DriverType = SharpDX.Direct3D10.DriverType;
using Factory = SharpDX.DXGI.Factory;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;

namespace MiniRect
{
    /// <summary>
    /// SharpDX port of SlimDX MiniTri Direct3D 10 Sample
    /// </summary>
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var form = new RenderForm("SharpDX - MiniTri Direct2D - Direct3D 10 Sample");

            // SwapChain description
            var desc = new SwapChainDescription()
                           {
                               BufferCount = 1,
                               ModeDescription = 
                                   new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                                       new Rational(60, 1), Format.R8G8B8A8_UNorm),
                               IsWindowed = true,
                               OutputHandle = form.Handle,
                               SampleDescription = new SampleDescription(1, 0),
                               SwapEffect = SwapEffect.Discard,
                               Usage = Usage.RenderTargetOutput
                           };

            // Create Device and SwapChain
            Device1 device;
            SwapChain swapChain;
            Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug|DeviceCreationFlags.BgraSupport, desc, FeatureLevel.Level_10_0, out device, out swapChain);

            var d2dFactory = new SharpDX.Direct2D1.Factory();

            int width = form.ClientSize.Width;
            int height = form.ClientSize.Height;

            var rectangleGeometry = new RoundedRectangleGeometry(d2dFactory, new RoundedRect() { RadiusX = 32, RadiusY = 32, Rect = new RectangleF(128, 128, width - 128, height-128) });

            // Ignore all windows events
            Factory factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            var renderView = new RenderTargetView(device, backBuffer);

            Surface surface = backBuffer.QueryInterface<Surface>();


            var d2dRenderTarget = new RenderTarget(d2dFactory, surface,
                                                            new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            var solidColorBrush = new SolidColorBrush(d2dRenderTarget, new Color4(1, 1, 1, 1));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Main loop
            RenderLoop.Run(form, () =>
                                      {
                                          d2dRenderTarget.BeginDraw();
                                          d2dRenderTarget.Clear(new Color4(1.0f, 0.0f, 0.0f, 0.0f));
                                          solidColorBrush.Color = new Color4((float) Math.Abs(Math.Cos(stopwatch.ElapsedMilliseconds*.001)), 1, 1, 1);
                                          d2dRenderTarget.FillGeometry(rectangleGeometry, solidColorBrush, null);
                                          d2dRenderTarget.EndDraw();

                                          swapChain.Present(0, PresentFlags.None);
                                      });

            // Release all resources
            renderView.Release();
            backBuffer.Release();
            device.ClearState();
            device.Flush();
            device.Release();
            device.Release();
            swapChain.Release();
            factory.Release();
        }
    }
}