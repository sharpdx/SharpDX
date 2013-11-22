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
#if !W8CORE && NET35Plus
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using SharpDX.Direct3D9;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An framework element that supports rendering D3D11 scene.
    /// </summary>
    public sealed class SharpDXElement : FrameworkElement, IDisposable
    {
        private readonly D3DImage image;
        private readonly Direct3DEx direct3D;
        private readonly DeviceEx device9;
        private readonly DispatcherTimer resizeDelayTimer;
        private Texture texture;

        private bool isDisposed;

        // used to avoid infinite loop when both ReceiveResizeFromGameProperty and SendResizeToGameProperty are set to true
        private bool isResizeCompletedBeingRaised;

        /// <summary>
        /// Indicates whether the size of this <see cref="SharpDXElement"/> should be affected by the Game settings. Default is false.
        /// </summary>
        public static readonly DependencyProperty ReceiveResizeFromGameProperty = DependencyProperty
            .Register("ReceiveResizeFromGame", typeof(bool), typeof(SharpDXElement), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Indicates whether the <see cref="SizeChanged"/> event will cause size changes in the bound <see cref="Game"/> class. Default is false.
        /// </summary>
        public static readonly DependencyProperty SendResizeToGameProperty = DependencyProperty
            .Register("SendResizeToGame", typeof(bool), typeof(SharpDXElement), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Indicates the time delay before the resize event will be propagated to the bound Game class which will cause its backbuffer resize.
        /// Default is 1 second.
        /// </summary>
        public static readonly DependencyProperty SendResizeDelayProperty = DependencyProperty
            .Register("SendResizeDelay", typeof(TimeSpan), typeof(SharpDXElement), new FrameworkPropertyMetadata(TimeSpan.FromSeconds(1), HandleResizeDelayChanged));

        /// <summary>
        /// Indicates whether the rendering should be done in the <see cref="System.Windows.Threading.DispatcherPriority.Input"/> priority.
        /// This may cause loss of FPS, but will not interfere with the input processing.
        /// Default is false.
        /// </summary>
        public static readonly DependencyProperty LowPriorityRenderingProperty = DependencyProperty
            .Register("LowPriorityRendering", typeof(bool), typeof(SharpDXElement), new PropertyMetadata(default(bool)));

        private static void HandleResizeDelayChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as SharpDXElement;
            if (element == null) return;

            if (e.NewValue != DependencyProperty.UnsetValue)
                element.resizeDelayTimer.Interval = (TimeSpan)e.NewValue;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SharpDXElement"/> class.
        /// Initializes the D3D9 runtime.
        /// </summary>
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

            resizeDelayTimer = new DispatcherTimer(DispatcherPriority.Normal);
            resizeDelayTimer.Interval = SendResizeDelay;
            resizeDelayTimer.Tick += HandleResizeDelayTimerTick;

            SizeChanged += HandleSizeChanged;
            Unloaded += HandleUnloaded;
        }

        /// <summary>
        /// Disposes all unmanaged resources associated with this instance
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;

            DisposeD3D9Backbuffer();

            device9.Dispose();
            direct3D.Dispose();

            isDisposed = true;
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="ReceiveResizeFromGameProperty"/> dependency property.
        /// </summary>
        public bool ReceiveResizeFromGame
        {
            get { return (bool)GetValue(ReceiveResizeFromGameProperty); }
            set { SetValue(ReceiveResizeFromGameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="SendResizeToGameProperty"/> dependency property.
        /// </summary>
        public bool SendResizeToGame
        {
            get { return (bool)GetValue(SendResizeToGameProperty); }
            set { SetValue(SendResizeToGameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the the value of the <see cref="SendResizeDelay"/> dependency property.
        /// </summary>
        public TimeSpan SendResizeDelay
        {
            get { return (TimeSpan)GetValue(SendResizeDelayProperty); }
            set { SetValue(SendResizeDelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="LowPriorityRenderingProperty"/> dependency property.
        /// </summary>
        public bool LowPriorityRendering
        {
            get { return (bool)GetValue(LowPriorityRenderingProperty); }
            set { SetValue(LowPriorityRenderingProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed or not.
        /// </summary>
        public bool IsDisposed { get { return isDisposed; } }

        /// <summary>
        /// Converts an <see cref="SharpDXElement"/> to <see cref="GameContext"/>.
        /// Operator is placed here to avoid WPF references when only WinForms is used.
        /// </summary>
        /// <param name="element">The <see cref="SharpDXElement"/> representing the game context.</param>
        /// <returns>An <see cref="GameContextWpf"/> instance derived from <see cref="GameContext"/>.</returns>
        public static implicit operator GameContext(SharpDXElement element)
        {
            return new GameContextWpf(element);
        }

        internal event EventHandler ResizeCompleted;

        /// <summary>
        /// Associates an D3D11 render target with the current instance.
        /// </summary>
        /// <param name="renderTarget">An valid D3D11 render target. It must be created with the "Shared" flag.</param>
        internal void SetBackbuffer(Direct3D11.Texture2D renderTarget)
        {
            DisposedGuard();

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

        /// <summary>
        /// Marks the surface of element as invalid and requests its presentation on screen.
        /// </summary>
        internal void InvalidateRendering()
        {
            DisposedGuard();

            image.Lock();
            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }

        /// <summary>
        /// Tries to set the width and height of the current instance.
        /// </summary>
        /// <param name="width">The width in dips.</param>
        /// <param name="height">The height in dips.</param>
        internal void TrySetSize(int width, int height)
        {
            if (!ReceiveResizeFromGame || isResizeCompletedBeingRaised) return;

            DisposedGuard();

            Width = width;
            Height = height;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (image != null && image.IsFrontBufferAvailable)
                drawingContext.DrawImage(image, new Rect(new System.Windows.Point(), RenderSize));
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the current instance is already disposed.
        /// </summary>
        private void DisposedGuard()
        {
            if (isDisposed)
                throw new ObjectDisposedException("SharpDXElement", "The element is disposed - either explicitly or via Unloaded event, it cannot be reused.");
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void HandleResizeDelayTimerTick(object sender, EventArgs e)
        {
            resizeDelayTimer.Stop();

            if (SendResizeToGame)
                RaiseResizeCompleted(ResizeCompleted);
        }

        private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeDelayTimer.Stop();
            resizeDelayTimer.Start();
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

        private void RaiseResizeCompleted(EventHandler handler)
        {
            if (handler != null)
            {
                try
                {
                    isResizeCompletedBeingRaised = true;
                    handler(this, EventArgs.Empty);
                }
                finally
                {
                    isResizeCompletedBeingRaised = false;
                }
            }
        }

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();
    }
}
#endif