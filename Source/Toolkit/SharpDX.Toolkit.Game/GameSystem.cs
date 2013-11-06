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

using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// Base class for a <see cref="GameSystem"/> component.
    /// </summary>
    /// <remarks>
    /// A <see cref="GameSystem"/> component can be used to 
    /// </remarks>
    public class GameSystem : Component, IGameSystem, IUpdateable, IDrawable, IContentable
    {
        private readonly DisposeCollector contentCollector = new DisposeCollector();
        private readonly IServiceRegistry registry;
        private int drawOrder;
        private bool enabled;
        private Game game;
        private int updateOrder;
        private bool visible;
        private IContentManager contentManager;
        private IGraphicsDeviceService graphicsDeviceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSystem" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public GameSystem(IServiceRegistry registry)
        {
            if(registry == null) throw new ArgumentNullException("registry");
            this.registry = registry;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSystem" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public GameSystem(Game game)
        {
            if(game == null) throw new ArgumentNullException("game");
            this.game = game;
            this.registry = game.Services;
        }

        /// <summary>
        /// Gets the <see cref="Game"/> associated with this <see cref="GameSystem"/>. This value can be null in a mock environment.
        /// </summary>
        /// <value>The game.</value>
        public Game Game
        {
            get { return game; }
        }

        /// <summary>
        /// Gets the services registry.
        /// </summary>
        /// <value>The services registry.</value>
        public IServiceRegistry Services
        {
            get
            {
                return registry;
            }
        }

        /// <summary>
        /// Gets the content manager.
        /// </summary>
        /// <value>The content.</value>
        protected IContentManager Content
        {
            get
            {
                return contentManager;
            }
        }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        protected GraphicsDevice GraphicsDevice
        {
            get
            {
                return graphicsDeviceService != null ? graphicsDeviceService.GraphicsDevice : null;
            }
        }

        #region IDrawable Members

        /// <summary>Occurs when the <see cref="DrawOrder" /> property changes.</summary>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>Occurs when the <see cref="Visible" /> property changes.</summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>Starts the drawing of a frame. This method is followed by calls to Draw and EndDraw.</summary>
        /// <returns><c>true</c> if Draw should occur, <c>false</c> otherwise</returns>
        public virtual bool BeginDraw()
        {
            return true;
        }

        /// <summary>Draws this instance.</summary>
        /// <param name="gameTime">The current timing.</param>
        public virtual void Draw(GameTime gameTime)
        {
        }

        /// <summary>Ends the drawing of a frame. This method is preceded by calls to Draw and BeginDraw.</summary>
        public virtual void EndDraw()
        {
        }

        /// <summary>Gets a value indicating whether the <see cref="Draw" /> method should be called by <see cref="Game.Draw" />.</summary>
        /// <value><c>true</c> if this drawable component is visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    OnVisibleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>Gets the draw order relative to other objects. <see cref="IDrawable" /> objects with a lower value are drawn first.</summary>
        /// <value>The draw order.</value>
        public int DrawOrder
        {
            get { return drawOrder; }
            set
            {
                if (drawOrder != value)
                {
                    drawOrder = value;
                    OnDrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region IGameSystem Members

        /// <summary>This method is called when the component is added to the game.</summary>
        /// <remarks>This method can be used for tasks like querying for services the component needs and setting up non-graphics resources.</remarks>
        public virtual void Initialize()
        {
            // Gets the Content Manager
            contentManager = (IContentManager)registry.GetService(typeof(IContentManager));

            // Gets the graphics device service
            graphicsDeviceService = (IGraphicsDeviceService)registry.GetService(typeof(IGraphicsDeviceService));
        }

        #endregion

        #region IUpdateable Members

        /// <summary>Occurs when the <see cref="Enabled" /> property changes.</summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>Occurs when the <see cref="UpdateOrder" /> property changes.</summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>This method is called when this game component is updated.</summary>
        /// <param name="gameTime">The current timing.</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>Gets a value indicating whether the game component's Update method should be called by <see cref="Game.Update" />.</summary>
        /// <value><c>true</c> if update is enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnEnabledChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>Gets the update order relative to other game components. Lower values are updated first.</summary>
        /// <value>The update order.</value>
        public int UpdateOrder
        {
            get { return updateOrder; }
            set
            {
                if (updateOrder != value)
                {
                    updateOrder = value;
                    OnUpdateOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        /// <summary>Called when [draw order changed].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnDrawOrderChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = DrawOrderChanged;
            if (handler != null) handler(source, e);
        }

        private void OnVisibleChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = VisibleChanged;
            if (handler != null) handler(this, e);
        }

        private void OnEnabledChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = EnabledChanged;
            if (handler != null) handler(this, e);
        }

        /// <summary>Called when [update order changed].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnUpdateOrderChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = UpdateOrderChanged;
            if (handler != null) handler(source, e);
        }

        #region Implementation of IContentable

        void IContentable.LoadContent()
        {
            LoadContent();
        }

        void IContentable.UnloadContent()
        {
            contentCollector.DisposeAndClear();

            UnloadContent();
        }

        /// <summary>Called when graphics resources (content) need to be loaded. Override this method to load any game-specific graphics resources.</summary>
        protected virtual void LoadContent()
        {
        }

        /// <summary>Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.</summary>
        protected virtual void UnloadContent()
        {
        }

        #endregion

        /// <summary>
        /// Adds an object to be disposed automatically when <see cref="UnloadContent"/> is called. See remarks.
        /// </summary>
        /// <typeparam name="T">Type of the object to dispose</typeparam>
        /// <param name="disposable">The disposable object.</param>
        /// <returns>The disposable object.</returns>
        /// <remarks>
        /// Use this method for any content that is not loaded through the <see cref="ContentManager"/>.
        /// </remarks>
        protected T ToDisposeContent<T>(T disposable) where T : IDisposable
        {
            return contentCollector.Collect(disposable);
        }
    }
}

