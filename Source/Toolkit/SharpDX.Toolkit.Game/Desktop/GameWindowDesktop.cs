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
#if !W8CORE
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using SharpDX.Toolkit.Graphics;
using SharpDX.Windows;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    internal class GameWindowDesktop : GameWindow
    {
        private bool isMouseVisible;
        private bool isMouseCurrentlyHidden;

        public Control Control;

        private RenderForm gameForm;
        private RenderLoop renderLoop;

        internal GameWindowDesktop()
        {
        }

        public override object NativeWindow
        {
            get
            {
                return Control;
            }
        }

        internal override bool IsBlockingRun { get { return false; } }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {

        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            if (gameForm != null)
            {
                gameForm.ClientSize = new Size(clientWidth, clientHeight);
            }
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        internal override bool CanHandle(GameContext gameContext)
        {
            return gameContext.ContextType == GameContextType.Desktop;
        }

        internal override void Initialize(GameContext gameContext)
        {
            this.GameContext = gameContext;

            Control = (Control)gameContext.Control;

            // Setup the initial size of the window
            var width = gameContext.RequestedWidth;
            if (width == 0)
            {
                width = Control is Form ? GraphicsDeviceManager.DefaultBackBufferWidth : Control.ClientSize.Width;
            }

            var height = gameContext.RequestedHeight;
            if (height == 0)
            {
                height = Control is Form ? GraphicsDeviceManager.DefaultBackBufferHeight : Control.ClientSize.Height;
            }

            Control.ClientSize = new System.Drawing.Size(width, height);

            Control.MouseEnter += GameWindowForm_MouseEnter;
            Control.MouseLeave += GameWindowForm_MouseLeave;

            gameForm = Control as RenderForm;
            if (gameForm != null)
            {
                gameForm.AppActivated += OnActivated;
                gameForm.AppDeactivated += OnDeactivated;
                gameForm.UserResized += OnClientSizeChanged;
            }
            else
            {
                Control.Resize += OnClientSizeChanged;
            }
        }

        internal override void Run()
        {
            Debug.Assert(InitCallback != null);
            Debug.Assert(RunCallback != null);

            InitCallback();

            try
            {
                // Use custom render loop
                Control.Show();
                using(renderLoop = new RenderLoop(Control))
                {
                    while(renderLoop.NextFrame())
                    {
                        if (Exiting)
                        {
                            if (Control != null)
                            {
                                Control.Dispose();
                                Control = null;
                            }
                            break;
                        }

                        RunCallback();
                    }
                }
            }
            finally
            {
                if (ExitCallback != null)
                {
                    ExitCallback();
                }
            }
        }

        private void GameWindowForm_MouseEnter(object sender, System.EventArgs e)
        {
            if (!isMouseVisible && !isMouseCurrentlyHidden)
            {
                Cursor.Hide();
                isMouseCurrentlyHidden = true;
            }
        }

        private void GameWindowForm_MouseLeave(object sender, System.EventArgs e)
        {
            if (isMouseCurrentlyHidden)
            {
                Cursor.Show();
                isMouseCurrentlyHidden = false;
            }
        }

        public override bool IsMouseVisible
        {
            get
            {
                return isMouseVisible;
            }
            set
            {
                if (isMouseVisible != value)
                {
                    isMouseVisible = value;
                    if (isMouseVisible)
                    {
                        if (isMouseCurrentlyHidden)
                        {
                            Cursor.Show();
                            isMouseCurrentlyHidden = false;
                        }
                    }
                    else if (!isMouseCurrentlyHidden)
                    {
                        Cursor.Hide();
                        isMouseCurrentlyHidden = true;
                    }
                }
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
                return Control.Visible;
            }
            set
            {
                Control.Visible = value;
            }
        }

        protected override void SetTitle(string title)
        {
            var form = Control as Form;
            if (form != null)
            {
                form.Text = title;
            }
        }

        internal override void Resize(int width, int height)
        {
            Control.ClientSize = new Size(width, height);
        }

        /// <inheritdoc />
        internal override void Switch(GameContext context)
        {
            // unbind event handlers from previous control
            Control.MouseEnter -= GameWindowForm_MouseEnter;
            Control.MouseLeave -= GameWindowForm_MouseLeave;

            gameForm = Control as RenderForm;
            if (gameForm != null)
            {
                gameForm.AppActivated -= OnActivated;
                gameForm.AppDeactivated -= OnDeactivated;
                gameForm.UserResized -= OnClientSizeChanged;
            }
            else
            {
                Control.Resize -= OnClientSizeChanged;
            }

            // setup and bind event handlers to new control
            Initialize(context);

            Control.Show(); // Make sure the control is visible
            renderLoop.Control = Control;
        }

        public override bool AllowUserResizing
        {
            get
            {
                return (Control is RenderForm && ((RenderForm)Control).AllowUserResizing);
            }
            set
            {
                var form = Control as RenderForm;
                if (form != null)
                {
                    form.AllowUserResizing = value;
                }
            }
        }

        public override Rectangle ClientBounds
        {
            get
            {
                return new Rectangle(0, 0, Control.ClientSize.Width, Control.ClientSize.Height);
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
                var form = Control as Form;
                if (form != null)
                {
                    return form.WindowState == FormWindowState.Minimized;
                }

                // Check for non-form control
                return false;
            }
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (Control != null)
                {
                    Control.Dispose();
                    Control = null;
                }

                gameForm = null;
            }

            base.Dispose(disposeManagedResources);
        }
    }
}
#endif