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

using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    public abstract class GameWindow : Component
    {
        private string title;

        #region Public Events

        public event EventHandler<EventArgs> ClientSizeChanged;

        public event EventHandler<EventArgs> OrientationChanged;

        internal event EventHandler<EventArgs> Activated;

        internal event EventHandler<EventArgs> Deactivated;

        #endregion

        #region Public Properties

        public abstract bool AllowUserResizing { get; set; }

        /// <summary>
        /// Gets the client bounds.
        /// </summary>
        /// <value>The client bounds.</value>
        public abstract DrawingRectangle ClientBounds { get; }

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
        /// Gets the native window.
        /// </summary>
        /// <value>The native window.</value>
        public abstract object NativeWindow { get; }

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

        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

        public void EndScreenDeviceChange()
        {
            EndScreenDeviceChange(ClientBounds.Width, ClientBounds.Height);
        }

        public abstract void EndScreenDeviceChange(int clientWidth, int clientHeight);

        #endregion

        protected void OnClientSizeChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = ClientSizeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnOrientationChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = OrientationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public abstract bool IsFullScreenMandatory { get; }

        protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

        internal abstract void Initialize(object windowContext);

        protected abstract void SetTitle(string title);

        protected void OnActivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null)
            {
                handler(source, e);
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
    }
}