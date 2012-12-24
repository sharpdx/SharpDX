// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
            this.registry = registry;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSystem" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public GameSystem(Game game) : this(game.Services)
        {
            this.game = game;
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

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public virtual bool BeginDraw()
        {
            return true;
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void EndDraw()
        {
        }

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

        public virtual void Initialize()
        {
            if (game == null)
            {
                game = (Game)registry.GetService(typeof(Game));
            }

            // Gets the Content Manager
            contentManager = (IContentManager)registry.GetService(typeof(IContentManager));

            // Gets the graphics device service
            graphicsDeviceService = (IGraphicsDeviceService)registry.GetService(typeof(IGraphicsDeviceService));

            ((IContentable)this).LoadContent();
        }

        #endregion

        #region IUpdateable Members

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public virtual void Update(GameTime gameTime)
        {
        }

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
            UnloadContent();
        }

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        #endregion
    }
}

