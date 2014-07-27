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

#if !W8CORE && NET35Plus
using SharpDX.Toolkit.Graphics;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SharpDX.Direct3D11;
using SharpDX.Mathematics;
using SharpDX.Mathematics.Interop;
using Device = SharpDX.Direct3D11.Device;
using System.Runtime.InteropServices;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An implementation of <see cref="GameWindow"/> for the Desktop/WPF platform.
    /// </summary>
    internal class GameWindowDesktopWpf : GameWindow
    {

        #region D3D9
        private sealed class D3D9 : IDisposable
        {
            private SharpDX.Direct3D9.Direct3DEx direct3d;
            private SharpDX.Direct3D9.DeviceEx device;

            public D3D9()
            {
                var presentparams = new SharpDX.Direct3D9.PresentParameters
                {
                    Windowed = true,
                    SwapEffect = SharpDX.Direct3D9.SwapEffect.Discard,
                    DeviceWindowHandle = GetDesktopWindow(),
                    PresentationInterval = SharpDX.Direct3D9.PresentInterval.Default
                };

                const SharpDX.Direct3D9.CreateFlags deviceFlags = SharpDX.Direct3D9.CreateFlags.HardwareVertexProcessing | SharpDX.Direct3D9.CreateFlags.Multithreaded | SharpDX.Direct3D9.CreateFlags.FpuPreserve;

                direct3d = new SharpDX.Direct3D9.Direct3DEx();
                device = new SharpDX.Direct3D9.DeviceEx(direct3d, 0, SharpDX.Direct3D9.DeviceType.Hardware, IntPtr.Zero, deviceFlags, presentparams);
            }

            ~D3D9()
            {
                Dispose();
            }

            public SharpDX.Direct3D9.DeviceEx Device { get { return device; } }

            public void Dispose()
            {
                Utilities.Dispose(ref direct3d);
                Utilities.Dispose(ref device);

                GC.SuppressFinalize(this);
            }

            [DllImport("user32.dll", SetLastError = false)]
            private static extern IntPtr GetDesktopWindow();
        }

        private sealed class RefCounter<T> where T : class, IDisposable, new()
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

        #endregion

        private SharpDXElement element;
        private D3D11Image image;
        private Window window;
        private DispatcherOperation previousRenderCall; // keep previous render call to avoid calling it again if prev one is not finished yet
        private readonly Action renderDelegate; //delegate cache to avoid garbage generation

        private bool isMouseVisible;
        private bool isMouseCurrentlyHidden;
        private bool isVisible;
        private Cursor oldCursor;
        private RenderTargetGraphicsPresenter presenter;
        private Query queryForCompletion; // used to syncronize draw calls with WPF presenter

        private TimeSpan lastRenderTime; // WPF may ask for rendering two times in a row, keep last render time to avoid this
        private DeviceContext deviceContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowDesktopWpf"/> class.
        /// </summary>
        public GameWindowDesktopWpf()
        {
            renderDelegate = RenderLoopCallback;
        }

        /// <inheritdoc />
        public override bool AllowUserResizing { get { return false; } set { /* ignore, WPF will resize everything itself */ } }

        /// <inheritdoc />
        public override Rectangle ClientBounds { get { return new Rectangle(0, 0, (int)element.ActualWidth, (int)element.ActualHeight); } }

        /// <inheritdoc />
        public override DisplayOrientation CurrentOrientation { get { return DisplayOrientation.Default; } }

        /// <inheritdoc />
        public override bool IsMinimized { get { return false; } }

        /// <inheritdoc />
        public override object NativeWindow { get { return element; } }

        /// <inheritdoc />
        public override bool IsMouseVisible
        {
            get { return isMouseVisible; }
            set
            {
                if (isMouseVisible == value) return;

                isMouseVisible = value;
                if (isMouseVisible)
                    TryShowMouse();
                else
                    TryHideMouse();
            }
        }

        /// <inheritdoc />
        public override bool Visible
        {
            get { return isVisible; }
            set
            {
                if (isVisible == value) return;
                isVisible = value;
                element.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }

        /// <summary>
        /// For Desktop/WPF platform the <see cref="GameWindow.Run"/> call is non-blocking because rendering is bound to <see cref="CompositionTarget.Rendering"/> event.
        /// </summary>
        /// <returns>false</returns>
        internal override bool IsBlockingRun { get { return false; } }

        /// <inheritdoc />
        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        /// <inheritdoc />
        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            if (element != null)
            {
                element.TrySetSize(clientWidth, clientHeight);
                SetElementBackbuffer();
            }
        }

        /// <inheritdoc />
        internal override bool CanHandle(GameContext gameContext)
        {
            if (gameContext == null) throw new ArgumentNullException("gameContext");

            return gameContext.ContextType == GameContextType.DesktopWpf;
        }

        /// <inheritdoc />
        internal override void Switch(GameContext context)
        {
            element.SetBackbufferImage(null);
            element.ResizeCompleted -= OnClientSizeChanged;
            element.MouseEnter -= OnMouseEnter;
            element.MouseLeave -= OnMouseLeave;
            element.Loaded -= HandleElementLoaded;
            element.Unloaded -= HandleElementUnloaded;
            element.Game = null;

            element = null;

            Initialize(context);
        }

        /// <inheritdoc />
        internal override GraphicsPresenter CreateGraphicsPresenter(GraphicsDevice device, PresentationParameters parameters)
        {
            var backbufferDesc = RenderTarget2D.CreateDescription(device,
                                                              parameters.BackBufferWidth,
                                                              parameters.BackBufferHeight,
                                                              Graphics.PixelFormat.B8G8R8A8.UNorm);

            // mandatory to share the surface with D3D9
            backbufferDesc.OptionFlags |= ResourceOptionFlags.Shared;

            RemoveAndDispose(ref presenter);
            RemoveAndDispose(ref queryForCompletion);

            if(image == null)
                d3d9.Value.AddReference();

            presenter = ToDispose(new RenderTargetGraphicsPresenter(device, backbufferDesc, parameters.DepthStencilFormat, false, true, parameters.DepthBufferShaderResource));
            // used to indicate if all drawing operations have completed
            queryForCompletion = ToDispose(new Query(presenter.GraphicsDevice, new QueryDescription { Type = QueryType.Event, Flags = QueryFlags.None }));

            // device context will be used to query drawing operations status
            deviceContext = ((Device)presenter.GraphicsDevice).ImmediateContext;

            SetElementBackbuffer();

            return presenter;
        }

        private void SetElementBackbuffer()
        {
            RemoveAndDispose(ref image);
            image = ToDispose(new D3D11Image(d3d9.Value.Instance.Device, presenter.BackBuffer));
            element.SetBackbufferImage(image);
        }

        /// <inheritdoc />
        internal override void Initialize(GameContext gameContext)
        {
            if (gameContext == null) throw new ArgumentNullException("gameContext");

            element = gameContext.Control as SharpDXElement;
            if (element == null) throw new ArgumentException("Only SharpDXElement is supported at this time", "gameContext");

            var width = gameContext.RequestedWidth;
            if (width <= 0)
                width = GraphicsDeviceManager.DefaultBackBufferWidth;

            var height = gameContext.RequestedHeight;
            if (height <= 0)
                height = GraphicsDeviceManager.DefaultBackBufferHeight;

            element.TrySetSize(width, height);

            element.ResizeCompleted += OnClientSizeChanged;
            element.MouseEnter += OnMouseEnter;
            element.MouseLeave += OnMouseLeave;
            element.Loaded += HandleElementLoaded;
            element.Unloaded += HandleElementUnloaded;

            
        }        

        /// <inheritdoc />
        internal override void Run()
        {
            Debug.Assert(InitCallback != null);
            Debug.Assert(RunCallback != null);

            InitCallback();

            CompositionTarget.Rendering += OnCompositionTargetRendering;
        }

        /// <inheritdoc />
        internal override void Resize(int width, int height)
        {
            element.TrySetSize(width, height);
        }

        /// <inheritdoc />
        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // orientations are not supported on Desktop platform
        }

        /// <inheritdoc />
        protected override void SetTitle(string title)
        {
            if (window != null)
            {
                window.Title = title;
            }
            // ignore. SharpDXElement doesn't have title
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposeManagedResources)
        {            
            base.Dispose(disposeManagedResources);
            d3d9.Value.RemoveReference();
        }

        /// <summary>
        /// Handles the <see cref="CompositionTarget.Rendering"/> event. Checks whether it is time for the next render call.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Should be an instance of <see cref="RenderingEventArgs"/> to determine last render time.</param>
        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            var args = (RenderingEventArgs)e;
            if (args.RenderingTime == lastRenderTime) return;
            lastRenderTime = args.RenderingTime;

            if (element.LowPriorityRendering)
            {
                // if we called render previously...
                if (previousRenderCall != null)
                {
                    var previousStatus = previousRenderCall.Status;

                    // ... and the operation didn't finish yet - then skip the current call
                    if (previousStatus == DispatcherOperationStatus.Pending
                        || previousStatus == DispatcherOperationStatus.Executing)
                    {
                        return;
                    }
                }

                previousRenderCall = element.Dispatcher.BeginInvoke(renderDelegate, DispatcherPriority.Input);
            }
            else
            {
                renderDelegate();
            }
        }

        /// <summary>
        /// The main render loop callback that is cached in a delegate and is invoked to process main game loop.
        /// </summary>
        private void RenderLoopCallback()
        {
            if (Exiting)
            {
                CompositionTarget.Rendering -= OnCompositionTargetRendering;
                if (element != null)
                {
                    element.Game = null;
                    element = null;
                }
               
                presenter.Dispose();

                return;
            }

            // run render loop once
            RunCallback();
            // mark completion of drawing operations
            deviceContext.End(queryForCompletion);

            // wait until drawing completes
            RawBool completed;
            while (!(deviceContext.GetData(queryForCompletion, out completed)
                   && completed)) Thread.Yield();

            // syncronize D3D surface with WPF
            image.InvalidateRendering();
        }

        /// <summary>
        /// Handles the <see cref="FrameworkElement.Loaded"/> event and raises the <see cref="GameWindow.Activated"/> event, in order to correctly enable game loop processing.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleElementLoaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(element);
            OnActivated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the <see cref="FrameworkElement.Unloaded"/> event and raises the <see cref="GameWindow.Deactivated"/> event, in order to correctly disable game loop processing.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleElementUnloaded(object sender, RoutedEventArgs e)
        {
            OnDeactivated(this, EventArgs.Empty);
            window = null;
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseEnter"/> event in order to adjust mouse cursor visibility based on <see cref="GameWindow.IsMouseVisible"/> value.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            TryHideMouse();
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseEnter"/> event in order to adjust mouse cursor visibility based on <see cref="GameWindow.IsMouseVisible"/> value.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            TryShowMouse();
        }

        /// <summary>
        /// Tries to hide mouse cursor if <see cref="GameWindow.IsMouseVisible"/> returns false.
        /// </summary>
        private void TryHideMouse()
        {
            if (isMouseVisible || isMouseCurrentlyHidden) return;

            oldCursor = element.Cursor;
            isMouseCurrentlyHidden = true;
        }

        /// <summary>
        /// Tries to show mouse cursor if it was previously hidden.
        /// </summary>
        private void TryShowMouse()
        {
            if (!isMouseVisible || !isMouseCurrentlyHidden) return;

            element.Cursor = oldCursor;
            isMouseCurrentlyHidden = false;
        }
    }
}
#endif