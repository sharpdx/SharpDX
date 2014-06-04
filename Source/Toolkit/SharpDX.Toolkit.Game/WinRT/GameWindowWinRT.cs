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

using SharpDX.Mathematics;
#if WIN8METRO
using SharpDX.Toolkit.Graphics;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.Graphics.Display;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    internal class GameWindowWinRT : GameWindow, IFrameworkViewSource, IFrameworkView
    {
        #region Fields

        public CoreWindow CoreWindow;

        #endregion

        #region Public Properties

        public override bool AllowUserResizing
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public override Rectangle ClientBounds
        {
            get
            {
                return new Rectangle(0, 0, (int)(CoreWindow.Bounds.Width * DisplayProperties.LogicalDpi / 96.0), (int)(CoreWindow.Bounds.Height * DisplayProperties.LogicalDpi / 96.0));
            }
        }

        public override DisplayOrientation CurrentOrientation
        {
            get
            {
                return DisplayOrientation.Default;
            }
        }

        public override bool IsMinimized
        {
            get
            {
                return false;
            }
        }

        internal override bool IsBlockingRun { get { return false; } }

        public override bool IsMouseVisible { get; set; }

        public override object NativeWindow
        {
            get
            {
                return CoreWindow;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameWindow" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public override bool Visible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
        }

        #endregion

        #region Explicit Interface Methods

        void IFrameworkView.Initialize(CoreApplicationView applicationView)
        {
        }

        void IFrameworkView.Load(string entryPoint)
        {
        }

        void IFrameworkView.Run()
        {
            // Specify the cursor type as the standard arrow cursor.
            // CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);

            // Activate the application window, making it visible and enabling it to receive events.
            CoreWindow.Activate();

            bool windowIsClosed = false;

            CoreWindow.Closed += (sender, args) => windowIsClosed = true;

            // Enter the render loop.  Note that Metro style apps should never exit.
            while (!windowIsClosed)
            {
                // Process events incoming to the window.
                CoreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                RunCallback();
            }
        }

        void IFrameworkView.SetWindow(CoreWindow window)
        {
            CoreWindow = window;

            // Call the init callback once the window is activated
            InitCallback();
        }

        void IFrameworkView.Uninitialize()
        {
        }

        IFrameworkView IFrameworkViewSource.CreateView()
        {
            return this;
        }

        #endregion

        #region Methods

        internal override bool CanHandle(GameContext windowContext)
        {
            return windowContext.ContextType == GameContextType.WinRT;
        }

        internal override void Initialize(GameContext windowContext)
        {
        }

        internal override void Resize(int width, int height)
        {

        }

        internal override void Run()
        {
            CoreApplication.Run(this);
        }

        internal override void Switch(GameContext context)
        {
            // Nothing to switch here, GameContext is not used in this implementation.
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        protected override void SetTitle(string title)
        {
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (CoreWindow != null)
                {
                    CoreWindow.Close();
                    CoreWindow = null;
                }
            }

            base.Dispose(disposeManagedResources);
        }

        #endregion
    }
}

#endif