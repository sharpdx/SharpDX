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

#if DIRECTX11_1 && !WP8

namespace SharpDX.Toolkit
{
    using System;
    using Direct2D1;
    using Direct3D11;
    using Graphics;

    /// <summary>
    /// Base class for an Direct2D fullscreen surface with automatic redrawing functionality.
    /// </summary>
    public abstract class Direct2DSurface
    {
        private readonly DisposeCollector disposeCollector = new DisposeCollector();
        private readonly IServiceProvider serviceProvider;

        private IGraphicsDeviceService graphicsDeviceService;
        private IDirect2DService direct2DService;

        private RenderTarget2D renderTarget;
        private Bitmap1 bitmapTarget;
        private bool isVisible;

        /// <summary>
        /// Allows to set custom name for performance tracing
        /// </summary>
        protected string performanceMarkerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct2DSurface"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider from where to get <see cref="IGraphicsDeviceService"/> and <see cref="IDirect2DService"/>.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="serviceProvider"/> is null.</exception>
        protected Direct2DSurface(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            this.serviceProvider = serviceProvider;

            performanceMarkerName = "D2D: " + GetType().Name;

            IsDirty = true;
            IsVisible = true;
        }

        /// <summary>
        /// Indicates whether this surface needs to be redrawn.
        /// </summary>
        public bool IsDirty { get; private set; }

        /// <summary>
        /// Gets of sets the value indicating whether this surface is visible.
        /// </summary>
        /// <remarks>When this flag is changed from <c>false</c> to <c>true</c> - the surface is redrawn (<see cref="IsDirty"/> is set to true).</remarks>
        internal bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value && (!isVisible)) IsDirty = true;
                isVisible = value;
            }
        }

        /// <summary>
        /// The D3D shader resource view to be used for D3D drawing operations.
        /// </summary>
        public ShaderResourceView Surface { get { return renderTarget; } }

        /// <summary>
        /// The service provider to retrieve additional services in derived classes.
        /// </summary>
        protected IServiceProvider Services { get { return serviceProvider; } }

        /// <summary>
        /// The underlying Direct2D service to create additional resources.
        /// </summary>
        protected IDirect2DService Direct2DService { get { return direct2DService; } }

        /// <summary>
        /// Initializes this instance of <see cref="Direct2DSurface"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">Is thrown when <see cref="IGraphicsDeviceService"/> or <see cref="IDirect2DService"/> cannot be retrieved
        /// from the service registry provided in constructor.</exception>
        internal void InitializeInternal()
        {
            graphicsDeviceService = serviceProvider.GetService<IGraphicsDeviceService>();
            direct2DService = serviceProvider.GetService<IDirect2DService>();

            Initialize();
        }

        /// <summary>
        /// Loads the content and prepares the actual drawing surface.
        /// </summary>
        internal void LoadContentInternal()
        {
            renderTarget = ToDisposeContent(CreateRenderTarget());

            var bitmapProperties = new BitmapProperties1(new SharpDX.Direct2D1.PixelFormat(renderTarget.Format, AlphaMode.Premultiplied),
                                                         96f,
                                                         96f,
                                                         BitmapOptions.CannotDraw | BitmapOptions.Target);

            bitmapTarget = ToDisposeContent(new Bitmap1(direct2DService.DeviceContext, renderTarget, bitmapProperties));

            LoadContent();
        }

        /// <summary>
        /// Unloads the content and disposes all unmanaged resources.
        /// </summary>
        internal void UnloadContentInternal()
        {
            disposeCollector.DisposeAndClear();

            UnloadContent();
        }

        /// <summary>
        /// Performs the drawing of this surface.
        /// </summary>
        /// <remarks>
        /// This method call will replace the render target in the D2D device context.
        /// </remarks>
        internal void DrawInternal()
        {
            var context = direct2DService.DeviceContext;

            using (graphicsDeviceService.GraphicsDevice.Performance.CreateEvent(performanceMarkerName))
            {
                context.Target = bitmapTarget;
                context.Clear(new Color4(0f, 0f, 0f, 0f));

                Draw();

                context.Target = null;
            }

            IsDirty = false;
        }

        /// <summary>
        /// Updates the surface logic.
        /// </summary>
        /// <param name="time">The structure holding game time information.</param>
        internal void UpdateInternal(GameTime time)
        {
            Update(time);
        }

        /// <summary>
        /// Marks this surface as dirty which will cause it to be redrawn (<see cref="Draw"/>).
        /// </summary>
        internal void Redraw()
        {
            IsDirty = true;
        }

        /// <summary>
        /// Marks the provided object to be disposed once <see cref="UnloadContent"/> is called.
        /// </summary>
        /// <typeparam name="T">The type of the object, must implement <see cref="IDisposable"/>.</typeparam>
        /// <param name="item">The object to be marked.</param>
        /// <returns>The object provided in <paramref name="item"/>.</returns>
        protected T ToDisposeContent<T>(T item)
            where T : class, IDisposable
        {
            return disposeCollector.Collect(item);
        }

        /// <summary>
        /// Gets the size of this surface.
        /// </summary>
        /// <returns>The render target size of this surface.</returns>
        protected Size2 GetSize()
        {
            return new Size2(renderTarget.Width, renderTarget.Height);
        }

        /// <summary>
        /// Creates the render target.
        /// </summary>
        /// <returns>The created render target where this surface will be drawn.</returns>
        protected RenderTarget2D CreateRenderTarget()
        {
            var backbuffer = graphicsDeviceService.GraphicsDevice.BackBuffer;
            return RenderTarget2D.New(graphicsDeviceService.GraphicsDevice, backbuffer.Width, backbuffer.Height, backbuffer.Format);
        }

        /// <summary>
        /// Derived classes should implement this method to perform custom initialization.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Derived classes should implement this method to perform custom content loading.
        /// </summary>
        protected virtual void LoadContent() { }

        /// <summary>
        /// Derived classes should implement this method to perform custom content unloading.
        /// </summary>
        protected virtual void UnloadContent() { }

        /// <summary>
        /// Derived classes should implement this method to perform custom drawing operations.
        /// </summary>
        protected virtual void Draw() { }

        /// <summary>
        /// Derived classes should implement this method to perform custom logic updates.
        /// </summary>
        protected virtual void Update(GameTime time) { }
    }
}

#endif