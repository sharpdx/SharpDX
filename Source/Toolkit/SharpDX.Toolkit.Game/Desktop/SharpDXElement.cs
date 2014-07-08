// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

using System.Threading;

#if !W8CORE && NET35Plus

namespace SharpDX.Toolkit
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Threading;
    using Direct3D9;

    /// <summary>
    /// An framework element that supports rendering D3D11 scene.
    /// </summary>
    public sealed class SharpDXElement : FrameworkElement
    {
        internal sealed class D3D9 : IDisposable
        {
            private Direct3DEx direct3d;
            private DeviceEx device;

            public D3D9()
            {
                var presentparams = new PresentParameters
                                    {
                                        Windowed = true,
                                        SwapEffect = SwapEffect.Discard,
                                        DeviceWindowHandle = GetDesktopWindow(),
                                        PresentationInterval = PresentInterval.Default
                                    };

                const CreateFlags deviceFlags = CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve;

                direct3d = new Direct3DEx();
                device = new DeviceEx(direct3d, 0, DeviceType.Hardware, IntPtr.Zero, deviceFlags, presentparams);
            }

            ~D3D9()
            {
                Dispose();
            }

            public DeviceEx Device { get { return device; } }

            public void Dispose()
            {
                Utilities.Dispose(ref direct3d);
                Utilities.Dispose(ref device);

                GC.SuppressFinalize(this);
            }

            [DllImport("user32.dll", SetLastError = false)]
            private static extern IntPtr GetDesktopWindow();
        }

        internal sealed class RefCounter<T> where T : class, IDisposable, new()
        {
            private int instancesCount;
            private T instance;

            public T Instance { get { return instance; } }

            public void AddReference()
            {
                instancesCount++;
                if (instancesCount == 1)
                {
                    System.Diagnostics.Debug.Assert(instance == null);
                    instance = new T();
                }
            }

            public void RemoveReference()
            {
                instancesCount--;

                System.Diagnostics.Debug.WriteLine("Instances: {0}", instancesCount);
                if (instancesCount == 0)
                {
                    System.Diagnostics.Debug.Assert(instance != null);
                    instance.Dispose();
                    instance = null;
                }
            }
        }

        private static readonly ThreadLocal<RefCounter<D3D9>> d3d9 = new ThreadLocal<RefCounter<D3D9>>(() => new RefCounter<D3D9>());

        private readonly D3DImage image;
        private readonly DispatcherTimer resizeDelayTimer;
        private Texture texture;
        private IntPtr textureSurfaceHandle;
        private bool isLoaded;

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

        /// <summary>
        /// Binds a Game object to use this element as rendering surface.
        /// </summary>
        /// <remarks>
        /// Internally uses <see cref="Game.Run(GameContext)"/> <see cref="Game.Switch(GameContext)"/> depending whether <see cref="Game.IsRunning"/>.
        /// This property is intended to be used only in WPF MVVM scenarios, do not use it if you use the above methods directly.
        /// </remarks>
        public static readonly DependencyProperty GameProperty = DependencyProperty
            .Register("Game", typeof(Game), typeof(SharpDXElement), new FrameworkPropertyMetadata(default(Game), HandleGameChanged));

        private static void HandleResizeDelayChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as SharpDXElement;
            if (element == null) return;

            if (e.NewValue != DependencyProperty.UnsetValue)
                element.resizeDelayTimer.Interval = (TimeSpan)e.NewValue;
        }

        private static void HandleGameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as SharpDXElement;
            if (element == null) return;

            var game = e.NewValue as Game;
            if (game != null)
            {
                if (game.IsRunning)
                    game.Switch(element);
                else
                    game.Run(element);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="SharpDXElement"/> class.
        /// Initializes the D3D9 runtime.
        /// </summary>
        public SharpDXElement()
        {
            image = new D3DImage();
            image.IsFrontBufferAvailableChanged += HandleIsFrontBufferAvailableChanged;

            resizeDelayTimer = new DispatcherTimer(DispatcherPriority.Normal);
            resizeDelayTimer.Tick += HandleResizeDelayTimerTick;
            resizeDelayTimer.Interval = SendResizeDelay;

            Focusable = true;

            SizeChanged += HandleSizeChanged;
            Loaded += HandleLoaded;
            Unloaded += HandleUnloaded;

            Dispatcher.ShutdownStarted += HandleShutdownStarted;
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
        /// Gets or sets the associated game instance. See <see cref="GameProperty"/> for details.
        /// </summary>
        public Game Game
        {
            get { return (Game)GetValue(GameProperty); }
            set { SetValue(GameProperty, value); }
        }

        /// <summary>
        /// Converts an <see cref="SharpDXElement"/> to <see cref="GameContext"/>.
        /// Operator is placed here to avoid WPF references when only WinForms is used.
        /// </summary>
        /// <param name="element">The <see cref="SharpDXElement"/> representing the game context.</param>
        /// <returns>An instance of <see cref="GameContext"/>.</returns>
        public static implicit operator GameContext(SharpDXElement element)
        {
            return new GameContext(element);
        }

        internal event EventHandler ResizeCompleted;

        /// <summary>
        /// Associates an D3D11 render target with the current instance.
        /// </summary>
        /// <param name="renderTarget">An valid D3D11 render target. It must be created with the "Shared" flag.</param>
        internal void SetBackbuffer(Direct3D11.Texture2D renderTarget)
        {
            EnsureD3D9IsReady();

            DisposeD3D9Backbuffer();

            using (var resource = renderTarget.QueryInterface<DXGI.Resource>())
            {
                var handle = resource.SharedHandle;
                texture = new Texture(d3d9.Value.Instance.Device,
                                      renderTarget.Description.Width,
                                      renderTarget.Description.Height,
                                      1,
                                      Usage.RenderTarget,
                                      Format.A8R8G8B8,
                                      Pool.Default,
                                      ref handle);
            }

            using (var surface = texture.GetSurfaceLevel(0))
            {
                textureSurfaceHandle = surface.NativePointer;
                TrySetBackbufferPointer(textureSurfaceHandle);
            }
        }

        /// <summary>
        /// Marks the surface of element as invalid and requests its presentation on screen.
        /// </summary>
        internal void InvalidateRendering()
        {
            if (!isLoaded || texture == null) return;

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
            if (!ReceiveResizeFromGame || isResizeCompletedBeingRaised || !isLoaded) return;

            Width = width;
            Height = height;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (image != null && image.IsFrontBufferAvailable)
                drawingContext.DrawImage(image, new Rect(new Point(), RenderSize));
        }

        private void HandleIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (image.IsFrontBufferAvailable)
                TrySetBackbufferPointer(textureSurfaceHandle);
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            EnsureD3D9IsReady();
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Unload();
        }

        private void HandleShutdownStarted(object sender, EventArgs eventArgs)
        {
            Unload();
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

        private void EnsureD3D9IsReady()
        {
            if (isLoaded) return;
            d3d9.Value.AddReference();

            isLoaded = true;
        }

        private void Unload()
        {
            if (!isLoaded) return;

            isLoaded = false;
            textureSurfaceHandle = IntPtr.Zero;
            DisposeD3D9Backbuffer();
            d3d9.Value.RemoveReference();
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
    }
}
#endif