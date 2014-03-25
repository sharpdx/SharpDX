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

using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    public abstract class GameWindow : Component
    {
        #region Fields

        private string title;

        internal GameContext GameContext;

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when this window is activated.
        /// </summary>
        public event EventHandler<EventArgs> Activated;

        /// <summary>
        /// Occurs, when device client size is changed.
        /// </summary>
        public event EventHandler<EventArgs> ClientSizeChanged;

        /// <summary>
        /// Occurs when this window is deactivated.
        /// </summary>
        public event EventHandler<EventArgs> Deactivated;

        /// <summary>
        /// Occurs, when device orientation is changed.
        /// </summary>
        public event EventHandler<EventArgs> OrientationChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets, user possibility to resize this window.
        /// </summary>
        public abstract bool AllowUserResizing { get; set; }

        /// <summary>
        /// Gets or sets the window location
        /// </summary>
        public abstract System.Drawing.Point Location { get; set; }

        /// <summary>
        /// Gets the client bounds.
        /// </summary>
        /// <value>The client bounds.</value>
        public abstract Rectangle ClientBounds { get; }

        /// <summary>
        /// Gets the current orientation.
        /// </summary>
        /// <value>The current orientation.</value>
        public abstract DisplayOrientation CurrentOrientation { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is minimized.
        /// </summary>
        /// <value><c>true</c> if this instance is minimized; otherwise, <c>false</c>.</value>
        public abstract bool IsMinimized { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse pointer is visible over this window.
        /// </summary>
        /// <value><c>true</c> if this instance is mouse visible; otherwise, <c>false</c>.</value>
        public abstract bool IsMouseVisible { get; set; }

        /// <summary>
        /// Gets the native window.
        /// </summary>
        /// <value>The native window.</value>
        public abstract object NativeWindow { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameWindow" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public abstract bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (title != value)
                {
                    title = value;
                    SetTitle(title);
                }
            }
        }

        internal abstract bool IsBlockingRun { get; }

        #endregion

        #region Public Methods and Operators

        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

        public void EndScreenDeviceChange()
        {
            EndScreenDeviceChange(ClientBounds.Width, ClientBounds.Height);
        }

        public abstract void EndScreenDeviceChange(int clientWidth, int clientHeight);

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the GameWindow with the specified window context.
        /// </summary>
        /// <param name="gameContext">The window context.</param>
        internal abstract bool CanHandle(GameContext gameContext);

        internal abstract void Initialize(GameContext gameContext);

        internal bool Exiting;

        internal Action InitCallback;

        internal Action RunCallback;

        internal Action ExitCallback;

        internal abstract void Run();

        internal abstract void Resize(int width, int height);

        /// <summary>
        /// Switches the rendering onto another game context.
        /// </summary>
        /// <param name="context">The new context to switch to.</param>
        internal abstract void Switch(GameContext context);

        /// <summary>
        /// Allows derived classes to create a custom graphics presenter
        /// </summary>
        /// <param name="device">The graphics device to use for renderer creation</param>
        /// <param name="parameters">The desired presentation parameters to use for renderer creation</param>
        /// <returns>Default implementation returns null</returns>
        internal virtual GraphicsPresenter CreateGraphicsPresenter(GraphicsDevice device, PresentationParameters parameters) { return null; }

        internal IServiceRegistry Services { get; set; }

        protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

        protected void OnActivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        protected void OnClientSizeChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = ClientSizeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnDeactivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        protected void OnOrientationChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = OrientationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected abstract void SetTitle(string title);

        #endregion
    }
}