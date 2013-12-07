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
    /// The implementation of <see cref="GameWindow"/> for Desktop/WinForms platform.
    /// </summary>
    internal class GameWindowDesktop : GameWindow
    {
        private bool isMouseVisible;
        private bool isMouseCurrentlyHidden;

        private RenderForm gameForm;
        private RenderLoop renderLoop;

        /// <summary>
        /// The render control associated with the current <see cref="GameWindow"/>.
        /// </summary>
        public Control Control;

        /// <summary>
        /// Gets the native window object associated with the current instance of <see cref="GameWindowDesktop"/>.
        /// </summary>
        public override object NativeWindow { get { return Control; } }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override bool Visible
        {
            get { return Control.Visible; }
            set { Control.Visible = value; }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override Rectangle ClientBounds
        {
            get
            {
                return new Rectangle(0, 0, Control.ClientSize.Width, Control.ClientSize.Height);
            }
        }

        /// <inheritdoc />
        public override DisplayOrientation CurrentOrientation { get { return DisplayOrientation.Default; } }

        /// <inheritdoc />
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

        /// <summary>
        /// For Desktop/WinForms platform the <see cref="GameWindow.Run"/> call is blocking due to <see cref="RenderLoop"/> implementation.
        /// </summary>
        /// <returns>true</returns>
        internal override bool IsBlockingRun { get { return true; } }

        /// <inheritdoc />
        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        /// <inheritdoc />
        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            if (gameForm != null)
                gameForm.ClientSize = new Size(clientWidth, clientHeight);
        }

        /// <inheritdoc />
        internal override bool CanHandle(GameContext gameContext)
        {
            return gameContext.ContextType == GameContextType.Desktop;
        }

        /// <inheritdoc />
        internal override void Initialize(GameContext gameContext)
        {
            GameContext = gameContext;

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

            Control.ClientSize = new Size(width, height);

            Control.MouseEnter += HandleControlMouseEnter;
            Control.MouseLeave += HandleControlMouseLeave;

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

        /// <inheritdoc />
        internal override void Run()
        {
            Debug.Assert(InitCallback != null);
            Debug.Assert(RunCallback != null);

            InitCallback();

            try
            {
                // Use custom render loop
                Control.Show();
                using (renderLoop = new RenderLoop(Control) { UseCustomDoEvents =  GameContext.UseCustomDoEvents})
                {
                    while (renderLoop.NextFrame())
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

        /// <inheritdoc />
        internal override void Resize(int width, int height)
        {
            Control.ClientSize = new Size(width, height);
        }

        /// <inheritdoc />
        internal override void Switch(GameContext context)
        {
            // unbind event handlers from previous control
            Control.MouseEnter -= HandleControlMouseEnter;
            Control.MouseLeave -= HandleControlMouseLeave;

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

        /// <inheritdoc />
        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        /// <inheritdoc />
        protected override void SetTitle(string title)
        {
            var form = Control as Form;
            if (form != null)
            {
                form.Text = title;
            }
        }

        /// <inheritdoc />
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

        /// <summary>
        /// Handles the <see cref="Control.MouseEnter"/> event to set cursor visibility, depending on <see cref="GameWindow.IsMouseVisible"/> value.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleControlMouseEnter(object sender, System.EventArgs e)
        {
            if (!isMouseVisible && !isMouseCurrentlyHidden)
            {
                Cursor.Hide();
                isMouseCurrentlyHidden = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseEnter"/> event to restore cursor visibility, depending on <see cref="GameWindow.IsMouseVisible"/> value.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleControlMouseLeave(object sender, System.EventArgs e)
        {
            if (isMouseCurrentlyHidden)
            {
                Cursor.Show();
                isMouseCurrentlyHidden = false;
            }
        }
    }
}
#endif