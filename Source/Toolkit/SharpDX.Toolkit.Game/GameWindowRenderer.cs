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

    using SharpDX.DXGI;
    using SharpDX.Toolkit.Graphics;

    /// <summary>A GameSystem that allows to draw to another window or control. Currently only valid on desktop with Windows.Forms.</summary>
    public class GameWindowRenderer : GameSystem
    {
        /// <summary>The begin draw ok.</summary>
        private bool beginDrawOk;

        /// <summary>The is back buffer automatic resize.</summary>
        private bool isBackBufferToResize;

        /// <summary>The preferred back buffer format.</summary>
        private PixelFormat preferredBackBufferFormat;

        /// <summary>The preferred back buffer height.</summary>
        private int preferredBackBufferHeight;

        /// <summary>The preferred back buffer width.</summary>
        private int preferredBackBufferWidth;

        /// <summary>The saved presenter.</summary>
        private GraphicsPresenter savedPresenter;

        /// <summary>The saved viewport.</summary>
        private ViewportF savedViewport;

        /// <summary>The window user resized.</summary>
        private bool windowUserResized;

        /// <summary>Initializes a new instance of the <see cref="GameWindowRenderer" /> class.</summary>
        /// <param name="registry">The registry.</param>
        /// <param name="gameContext">The window context.</param>
        public GameWindowRenderer(IServiceRegistry registry, GameContext gameContext = null)
            : base(registry)
        {
            this.GameContext = gameContext ?? new GameContext();
        }

        /// <summary>Initializes a new instance of the <see cref="GameWindowRenderer" /> class.</summary>
        /// <param name="game">The game.</param>
        /// <param name="gameContext">The window context.</param>
        public GameWindowRenderer(Game game, GameContext gameContext = null)
            : base(game)
        {
            this.GameContext = gameContext ?? new GameContext();
        }

        /// <summary>Gets the underlying native window.</summary>
        /// <value>The underlying native window.</value>
        public GameContext GameContext { get; private set; }

        /// <summary>Gets or sets the preferred back buffer format.</summary>
        /// <value>The preferred back buffer format.</value>
        public PixelFormat PreferredBackBufferFormat
        {
            get
            {
                return this.preferredBackBufferFormat;
            }

            set
            {
                if(this.preferredBackBufferFormat != value)
                {
                    this.preferredBackBufferFormat = value;
                    this.isBackBufferToResize = true;
                }
            }
        }

        /// <summary>Gets or sets the height of the preferred back buffer.</summary>
        /// <value>The height of the preferred back buffer.</value>
        public int PreferredBackBufferHeight
        {
            get
            {
                return this.preferredBackBufferHeight;
            }

            set
            {
                if(this.preferredBackBufferHeight != value)
                {
                    this.preferredBackBufferHeight = value;
                    this.isBackBufferToResize = true;
                }
            }
        }

        /// <summary>Gets or sets the width of the preferred back buffer.</summary>
        /// <value>The width of the preferred back buffer.</value>
        public int PreferredBackBufferWidth
        {
            get
            {
                return this.preferredBackBufferWidth;
            }

            set
            {
                if(this.preferredBackBufferWidth != value)
                {
                    this.preferredBackBufferWidth = value;
                    this.isBackBufferToResize = true;
                }
            }
        }

        /// <summary>Gets or sets the preferred depth stencil format.</summary>
        /// <value>The preferred depth stencil format.</value>
        public DepthFormat PreferredDepthStencilFormat { get; set; }

        /// <summary>Gets or sets the presenter.</summary>
        /// <value>The presenter.</value>
        public GraphicsPresenter Presenter { get; protected set; }

        /// <summary>Gets the window.</summary>
        /// <value>The window.</value>
        public GameWindow Window { get; private set; }

        /// <summary>Begins the draw.</summary>
        /// <returns><c>true</c> if begin draw is OK, <c>false</c> otherwise.</returns>
        public override bool BeginDraw()
        {
            if(this.GraphicsDevice != null && this.Window.Visible)
            {
                this.savedPresenter = this.GraphicsDevice.Presenter;
                this.savedViewport = this.GraphicsDevice.Viewport;

                this.CreateOrUpdatePresenter();

                if(this.isBackBufferToResize || this.windowUserResized)
                {
                    PixelFormat resizeFormat;
                    var size = this.GetRequestedSize(out resizeFormat);
                    this.Presenter.Resize(size.Width, size.Height, resizeFormat);

                    this.isBackBufferToResize = false;
                    this.windowUserResized = false;
                }

                this.GraphicsDevice.Presenter = this.Presenter;
                this.GraphicsDevice.SetViewport(this.Presenter.DefaultViewport);
                this.GraphicsDevice.SetRenderTargets(this.Presenter.DepthStencilBuffer, this.Presenter.BackBuffer);

                this.beginDrawOk = true;
                return true;
            }

            this.beginDrawOk = false;
            return false;
        }

        /// <summary>Ends the draw.</summary>
        public override void EndDraw()
        {
            if(this.beginDrawOk && this.GraphicsDevice != null)
            {
                try
                {
                    this.Presenter.Present();
                }
                catch(SharpDXException ex)
                {
                    // If this is not a DeviceRemoved or DeviceReset, than throw an exception
                    if(ex.ResultCode != ResultCode.DeviceRemoved && ex.ResultCode != ResultCode.DeviceReset) throw;
                }

                if(this.savedPresenter != null)
                {
                    this.GraphicsDevice.Presenter = this.savedPresenter;
                    this.GraphicsDevice.SetRenderTargets(this.savedPresenter.DepthStencilBuffer, this.savedPresenter.BackBuffer);
                    this.GraphicsDevice.SetViewport(this.savedViewport);
                }
            }
        }

        /// <summary>Initializes this instance.</summary>
        public override void Initialize()
        {
            var gamePlatform = (IGamePlatform)this.Services.GetService(typeof(IGamePlatform));
            this.GameContext.RequestedWidth = this.PreferredBackBufferWidth;
            this.GameContext.RequestedHeight = this.PreferredBackBufferHeight;
            this.Window = gamePlatform.CreateWindow(this.GameContext);
            this.Window.Visible = true;

            this.Window.ClientSizeChanged += this.WindowOnClientSizeChanged;

            base.Initialize();
        }

        /// <summary>Creates the original update presenter.</summary>
        protected virtual void CreateOrUpdatePresenter()
        {
            if(this.Presenter == null)
            {
                PixelFormat resizeFormat;
                var size = this.GetRequestedSize(out resizeFormat);
                var presentationParameters = new PresentationParameters(size.Width, size.Height, this.Window.NativeWindow, resizeFormat)
                                             {
                                                 DepthStencilFormat = this.PreferredDepthStencilFormat,
                                                 PresentationInterval = PresentInterval.Immediate
                                             };
                this.Presenter = new SwapChainGraphicsPresenter(this.GraphicsDevice, presentationParameters);
                this.isBackBufferToResize = false;
            }
        }

        /// <summary>Gets the size of the requested.</summary>
        /// <param name="format">The format.</param>
        /// <returns>Size2.</returns>
        private Size2 GetRequestedSize(out PixelFormat format)
        {
            var bounds = this.Window.ClientBounds;
            format = this.PreferredBackBufferFormat == PixelFormat.Unknown ? PixelFormat.R8G8B8A8.UNorm : this.PreferredBackBufferFormat;
            return new Size2(
                this.PreferredBackBufferWidth == 0 || this.windowUserResized ? bounds.Width : this.PreferredBackBufferWidth,
                this.PreferredBackBufferHeight == 0 || this.windowUserResized ? bounds.Height : this.PreferredBackBufferHeight);
        }

        /// <summary>Windows the configuration client size changed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void WindowOnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            this.windowUserResized = true;
        }
    }
}
