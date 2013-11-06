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

        /// <summary>The title.</summary>
        private string title;

        /// <summary>The game context.</summary>
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

        #endregion

        #region Public Methods and Operators

        /// <summary>Begins the screen device change.</summary>
        /// <param name="willBeFullScreen">if set to <see langword="true" /> [will be full screen].</param>
        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

        /// <summary>Ends the screen device change.</summary>
        public void EndScreenDeviceChange()
        {
            EndScreenDeviceChange(ClientBounds.Width, ClientBounds.Height);
        }

        /// <summary>Ends the screen device change.</summary>
        /// <param name="clientWidth">Width of the client.</param>
        /// <param name="clientHeight">Height of the client.</param>
        public abstract void EndScreenDeviceChange(int clientWidth, int clientHeight);

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the GameWindow with the specified window context.
        /// </summary>
        /// <param name="gameContext">The window context.</param>
        internal abstract bool CanHandle(GameContext gameContext);

        /// <summary>Initializes the specified game context.</summary>
        /// <param name="gameContext">The game context.</param>
        internal abstract void Initialize(GameContext gameContext);

        /// <summary>The exiting.</summary>
        internal bool Exiting;

        /// <summary>The initialize callback.</summary>
        internal Action InitCallback;

        /// <summary>The run callback.</summary>
        internal Action RunCallback;

        /// <summary>The exit callback.</summary>
        internal Action ExitCallback;

        /// <summary>Runs this instance.</summary>
        internal abstract void Run();

        /// <summary>Resizes the specified width.</summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        internal abstract void Resize(int width, int height);

        /// <summary>
        /// Allows derived classes to create a custom graphics presenter
        /// </summary>
        /// <param name="device">The graphics device to use for renderer creation</param>
        /// <param name="parameters">The desired presentation parameters to use for renderer creation</param>
        /// <returns>Default implementation returns null</returns>
        internal virtual GraphicsPresenter CreateGraphicsPresenter(GraphicsDevice device, PresentationParameters parameters) { return null; }

        /// <summary>Gets or sets the services.</summary>
        /// <value>The services.</value>
        internal IServiceRegistry Services { get; set; }

        /// <summary>Sets the supported orientations.</summary>
        /// <param name="orientations">The orientations.</param>
        protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

        /// <summary>Called when [activated].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnActivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        /// <summary>Called when [client size changed].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnClientSizeChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = ClientSizeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>Called when [deactivated].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnDeactivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null)
            {
                handler(source, e);
            }
        }

        /// <summary>Called when [orientation changed].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnOrientationChanged(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = OrientationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>Sets the title.</summary>
        /// <param name="title">The title.</param>
        protected abstract void SetTitle(string title);

        #endregion
    }
}