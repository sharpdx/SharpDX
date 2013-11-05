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

namespace SharpDX.Toolkit
{
    using System;

    using SharpDX.Toolkit.Content;
    using SharpDX.Toolkit.Graphics;

    /// <summary>Base class for a <see cref="GameSystem" /> component.</summary>
    /// <remarks>A <see cref="GameSystem" /> component can be used to</remarks>
    public class GameSystem : Component, IGameSystem, IUpdateable, IDrawable, IContentable
    {
        /// <summary>The content collector.</summary>
        private readonly DisposeCollector contentCollector;

        /// <summary>The draw order.</summary>
        private int drawOrder;

        /// <summary>The graphics device service.</summary>
        private IGraphicsDeviceService graphicsDeviceService;

        /// <summary>The is enabled.</summary>
        private bool isEnabled;

        /// <summary>The is visible.</summary>
        private bool isVisible;

        /// <summary>The update order.</summary>
        private int updateOrder;

        /// <summary>Initializes a new instance of the <see cref="GameSystem" /> class.</summary>
        /// <param name="registry">The registry.</param>
        public GameSystem(IServiceRegistry registry)
        {
            this.contentCollector = new DisposeCollector();
            this.Services = registry;
        }

        /// <summary>Initializes a new instance of the <see cref="GameSystem" /> class.</summary>
        /// <param name="game">The game.</param>
        public GameSystem(Game game)
            : this(game.Services)
        {
            this.Game = game;
        }

        /// <summary>Occurs when the <see cref="DrawOrder" /> property changes.</summary>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>Occurs when the <see cref="Enabled" /> property changes.</summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>Occurs when the <see cref="UpdateOrder" /> property changes.</summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>Occurs when the <see cref="Visible" /> property changes.</summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        ///     Gets the draw order relative to other objects. <see cref="IDrawable" /> objects with a lower value are drawn
        ///     first.
        /// </summary>
        /// <value>The draw order.</value>
        public int DrawOrder
        {
            get
            {
                return this.drawOrder;
            }
            set
            {
                if(this.drawOrder != value)
                {
                    this.drawOrder = value;
                    this.OnDrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>Gets a value indicating whether the game component's Update method should be called by <see cref="Update" />.</summary>
        /// <value><c>true</c> if update is enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                if(this.isEnabled != value)
                {
                    this.isEnabled = value;
                    this.OnEnabledChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>Gets the <see cref="Game" /> associated with this <see cref="GameSystem" />. This value can be null in a mock environment.</summary>
        /// <value>The game.</value>
        public Game Game { get; private set; }

        /// <summary>Gets the services registry.</summary>
        /// <value>The services registry.</value>
        public IServiceRegistry Services { get; private set; }

        /// <summary>Gets the update order relative to other game components. Lower values are updated first.</summary>
        /// <value>The update order.</value>
        public int UpdateOrder
        {
            get
            {
                return this.updateOrder;
            }
            set
            {
                if(this.updateOrder != value)
                {
                    this.updateOrder = value;
                    this.OnUpdateOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>Gets a value indicating whether the <see cref="Draw" /> method should be called by <see cref="Draw" />.</summary>
        /// <value><c>true</c> if this drawable component is visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if(this.isVisible != value)
                {
                    this.isVisible = value;
                    this.OnVisibleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>Gets the content manager.</summary>
        /// <value>The content.</value>
        protected IContentManager Content { get; private set; }

        /// <summary>Gets the graphics device.</summary>
        /// <value>The graphics device.</value>
        protected GraphicsDevice GraphicsDevice
        {
            get
            {
                return this.graphicsDeviceService != null ? this.graphicsDeviceService.GraphicsDevice : null;
            }
        }

        /// <summary>Starts the drawing of a frame. This method is followed by calls to Draw and EndDraw.</summary>
        /// <returns><c>true</c> if Draw should occur, <c>false</c> otherwise</returns>
        public virtual bool BeginDraw()
        {
            return true;
        }

        /// <summary>Draws this instance.</summary>
        /// <param name="gameTime">The current timing.</param>
        public virtual void Draw(GameTime gameTime) {}

        /// <summary>Ends the drawing of a frame. This method is preceded by calls to Draw and BeginDraw.</summary>
        public virtual void EndDraw() {}

        /// <summary>This method is called when the component is added to the game.</summary>
        /// <remarks>
        ///     This method can be used for tasks like querying for services the component needs and setting up non-graphics
        ///     resources.
        /// </remarks>
        public virtual void Initialize()
        {
            // Gets the Content Manager
            this.Content = (IContentManager)this.Services.GetService(typeof(IContentManager));

            // Gets the graphics device service
            this.graphicsDeviceService = (IGraphicsDeviceService)this.Services.GetService(typeof(IGraphicsDeviceService));
        }

        /// <summary>This method is called when this game component is updated.</summary>
        /// <param name="gameTime">The current timing.</param>
        public virtual void Update(GameTime gameTime) {}

        /// <summary>Loads the content.</summary>
        void IContentable.LoadContent()
        {
            this.LoadContent();
        }

        /// <summary>Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.</summary>
        void IContentable.UnloadContent()
        {
            this.contentCollector.DisposeAndClear();

            this.UnloadContent();
        }

        /// <summary>Loads the content.</summary>
        protected virtual void LoadContent() {}

        /// <summary>Called when [draw order changed].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnDrawOrderChanged(object source, EventArgs e)
        {
            var handler = this.DrawOrderChanged;
            if(handler != null) handler(source, e);
        }

        /// <summary>Called when [update order changed].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnUpdateOrderChanged(object source, EventArgs e)
        {
            var handler = this.UpdateOrderChanged;
            if(handler != null) handler(source, e);
        }

        /// <summary>Adds an object to be disposed automatically when <see cref="UnloadContent" /> is called. See remarks.</summary>
        /// <typeparam name="T">Type of the object to dispose</typeparam>
        /// <param name="disposable">The disposable object.</param>
        /// <returns>The disposable object.</returns>
        /// <remarks>Use this method for any content that is not loaded through the <see cref="ContentManager" />.</remarks>
        protected T ToDisposeContent<T>(T disposable) where T : IDisposable
        {
            return this.contentCollector.Collect(disposable);
        }

        /// <summary>Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.</summary>
        protected virtual void UnloadContent() {}

        /// <summary>Raises the <see cref="EnabledChanged" /> event.</summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnEnabledChanged(EventArgs e)
        {
            var handler = this.EnabledChanged;
            if(handler != null) handler(this, e);
        }

        /// <summary>Raises the <see cref="VisibleChanged" /> event.</summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnVisibleChanged(EventArgs e)
        {
            var handler = this.VisibleChanged;
            if(handler != null) handler(this, e);
        }
    }
}
