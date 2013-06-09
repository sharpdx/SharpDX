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
#if !W8CORE && NET35Plus && !DIRECTX11_1
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using SharpDX.Direct3D9;

namespace SharpDX.Toolkit
{
    public sealed class SharpDXElement : FrameworkElement, IDisposable
    {
        private readonly D3DImage image;
        private readonly Direct3DEx direct3D;
        private readonly DeviceEx device9;
        private Texture texture;

        private bool isDisposed;

        public SharpDXElement()
        {
            image = new D3DImage();

            var presentparams = new PresentParameters
                {
                    Windowed = true,
                    SwapEffect = SwapEffect.Discard,
                    DeviceWindowHandle = GetDesktopWindow(),
                    PresentationInterval = PresentInterval.Default
                };

            direct3D = new Direct3DEx();

            device9 = new DeviceEx(direct3D,
                                   0,
                                   DeviceType.Hardware,
                                   IntPtr.Zero,
                                   CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve,
                                   presentparams);

            Unloaded += HandleUnloaded;
        }

        public void Dispose()
        {
            if (isDisposed) return;

            DisposeD3D9Backbuffer();

            device9.Dispose();
            direct3D.Dispose();

            isDisposed = true;
        }

        internal void SetBackbuffer(Direct3D11.Texture2D renderTarget)
        {
            DisposeD3D9Backbuffer();

            using (var resource = renderTarget.QueryInterface<DXGI.Resource>())
            {
                var handle = resource.SharedHandle;
                texture = new Texture(device9,
                                      renderTarget.Description.Width,
                                      renderTarget.Description.Height,
                                      1,
                                      Usage.RenderTarget,
                                      Format.A8R8G8B8,
                                      Pool.Default,
                                      ref handle);
            }

            using (var surface = texture.GetSurfaceLevel(0))
                TrySetBackbufferPointer(surface.NativePointer);
        }

        internal void InvalidateRendering()
        {
            image.Lock();
            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //if (image != null && image.IsFrontBufferAvailable)
                drawingContext.DrawImage(image, new Rect(new System.Windows.Point(), RenderSize));
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void TrySetBackbufferPointer(IntPtr ptr)
        {
            // TODO: use TryLock and check multithreading scenarios
            image.Lock();
            try
            {
                image.SetBackBuffer(D3DResourceType.IDirect3DSurface9, ptr);
            }
            finally
            {
                image.Unlock();
            }
        }

        private void DisposeD3D9Backbuffer()
        {
            if (texture != null)
            {
                TrySetBackbufferPointer(IntPtr.Zero);

                texture.Dispose();
                texture = null;
            }
        }

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();
    }
}
#endif