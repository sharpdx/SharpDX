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
using System;
using System.Threading;
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

        private object wpfBorderControl;

        private ManualResetEvent renderingThreadForWpfCanRun;
        private ManualResetEvent renderingThreadForWpfHwndHostReady;
        private IntPtr windowHandle;


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

        private bool isFullScreenMaximized;
        private FormBorderStyle savedFormBorderStyle;
        private FormWindowState savedWindowState;
        private System.Drawing.Rectangle savedBounds;
        private System.Drawing.Size oldClientSize;
        private System.Drawing.Rectangle savedRestoreBounds;
        private bool oldVisible;
        private bool deviceChangeChangedVisible;
        private bool? deviceChangeWillBeFullScreen;

        /// <inheritdoc />
        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            if (gameForm != null)
                oldClientSize = gameForm.ClientSize;
            if (willBeFullScreen && !isFullScreenMaximized && gameForm != null)
            {
                savedFormBorderStyle = gameForm.FormBorderStyle;
                savedWindowState = gameForm.WindowState;
                savedBounds = gameForm.Bounds;
                if (gameForm.WindowState == FormWindowState.Maximized)
                    savedRestoreBounds = gameForm.RestoreBounds;
            }

            if (willBeFullScreen != isFullScreenMaximized)
            {
                deviceChangeChangedVisible = true;
                oldVisible = Visible;
                Visible = false;

                if (gameForm != null)
                    gameForm.SendToBack();
            }
            else
            {
                deviceChangeChangedVisible = false;
            }

            if (!willBeFullScreen && isFullScreenMaximized && gameForm != null)
            {
                gameForm.TopMost = false;
                gameForm.FormBorderStyle = savedFormBorderStyle;
            }

            deviceChangeWillBeFullScreen = willBeFullScreen;
        }

        /// <inheritdoc />
        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            if (!deviceChangeWillBeFullScreen.HasValue)
                return;

            if (deviceChangeWillBeFullScreen.Value)
            {
                if (!isFullScreenMaximized && gameForm != null)
                {
                    gameForm.TopMost = true;
                    gameForm.FormBorderStyle = FormBorderStyle.None;
                    gameForm.WindowState = FormWindowState.Normal;
                    gameForm.BringToFront();
                }
                isFullScreenMaximized = true;
            }
            else if (isFullScreenMaximized)
            {
                if (gameForm != null)
                    gameForm.BringToFront();
                isFullScreenMaximized = false;
            }

            if (deviceChangeChangedVisible)
                Visible = oldVisible;

            if (gameForm != null)
            {
                gameForm.ClientSize = new Size(clientWidth, clientHeight);
                gameForm.IsFullscreen = deviceChangeWillBeFullScreen.Value;
            }

            deviceChangeWillBeFullScreen = null;
        }

        /// <inheritdoc />
        internal override bool CanHandle(GameContext gameContext)
        {
            return gameContext.ContextType == GameContextType.Desktop || gameContext.ContextType == GameContextType.DesktopHwndWpf;
        }

        /// <inheritdoc />
        internal override void Initialize(GameContext gameContext)
        {
            GameContext = gameContext;

            if (gameContext.ContextType == GameContextType.Desktop)
            {
                Control = (Control)gameContext.Control;
                InitializeFromWinForm();
            }
            else if (gameContext.ContextType == GameContextType.DesktopHwndWpf)
            {
                InitializeFromWpfControl(gameContext.Control);
            }
        }

        private void InitializeFromWpfControl(object wpfControl)
        {
#if NET35Plus
            wpfBorderControl = (System.Windows.Controls.Border)wpfControl;
            renderingThreadForWpfCanRun = new ManualResetEvent(false);
            renderingThreadForWpfHwndHostReady = new ManualResetEvent(false);

            // Start a new rendering thread for the WinForm part
            var thread = new Thread(RunWpfRenderLoop) { IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
#endif
        }

        /// <inheritdoc />
        private void InitializeFromWinForm()
        {
            // Setup the initial size of the window
            var width = GameContext.RequestedWidth;
            if (width == 0)
            {
                width = Control is Form ? GraphicsDeviceManager.DefaultBackBufferWidth : Control.ClientSize.Width;
            }

            var height = GameContext.RequestedHeight;
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
#if NET35Plus
            if (wpfBorderControl != null)
            {
                StartWpfRenderLoop();
            }
            else
#endif
            {
                RunRenderLoop();
            }
        }

#if NET35Plus
        private void StartWpfRenderLoop()
        {
            // Wait for HwndHost ready
            renderingThreadForWpfHwndHostReady.WaitOne();

            // Create the toolkit HwndHost
            ((System.Windows.Controls.Border)wpfBorderControl).Child = new ToolkitHwndHost(windowHandle);

            // WPF rendering is done through a separate host
            renderingThreadForWpfCanRun.Set();
        }

        private void RunWpfRenderLoop()
        {
            // Allocation of the RenderForm should be done on the same thread
            Control = new RenderForm("SharpDX") { TopLevel = false, Visible = false };
            InitializeFromWinForm();

            windowHandle = Control.Handle;

            // Notifies that the control is ready
            renderingThreadForWpfHwndHostReady.Set();

            // Wait for actual run
            renderingThreadForWpfCanRun.WaitOne();

            RunRenderLoop();
        }
#endif

        private void RunRenderLoop()
        {
            Debug.Assert(InitCallback != null);
            Debug.Assert(RunCallback != null);

            InitCallback();

            try
            {
                // Use custom render loop
                // Show the control for WinForm, let HwndHost show it for WPF
                if (wpfBorderControl == null)
                    Control.Show();

                using (renderLoop = new RenderLoop(Control) { UseApplicationDoEvents = GameContext.UseApplicationDoEvents })
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