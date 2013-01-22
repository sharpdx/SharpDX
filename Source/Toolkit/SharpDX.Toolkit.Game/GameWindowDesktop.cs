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
        private bool isInitialized;
        private bool isMouseVisible;
        private bool isMouseCurrentlyHidden;

        public Control Control;

        private GameWindowForm gameWindowForm;


        internal GameWindowDesktop()
        {
        }

        public bool IsForm
        {
            get
            {
                return Control is Form;
            }
        }

        public override object NativeWindow
        {
            get
            {
                return Control;
            }
        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            
        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            
        }

        public override bool IsFullScreenMandatory
        {
            get
            {
                return false;
            }
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        internal override void Initialize(object windowContext)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException("GameWindow is already initialized");
            }

            windowContext = windowContext ?? new GameWindowForm("SharpDX.Toolkit.Game");
            Control = windowContext as Control;
            if (Control == null)
            {
                throw new NotSupportedException("Unsupported window context. Unable to create game window. Only System.Windows.Control subclass are supported");
            }

            Control.MouseEnter += GameWindowForm_MouseEnter;
            Control.MouseLeave += GameWindowForm_MouseLeave;

            gameWindowForm = windowContext as GameWindowForm;
            if (gameWindowForm != null)
            {
                gameWindowForm.AppActivated += OnActivated;
                gameWindowForm.AppDeactivated += OnDeactivated;
                gameWindowForm.UserResized += OnClientSizeChanged;
            }
            else
            {
                Control.Resize += OnClientSizeChanged;
            }

            isInitialized = true;
        }

        void GameWindowForm_MouseEnter(object sender, System.EventArgs e)
        {
            if (!isMouseVisible && !isMouseCurrentlyHidden)
            {
                Cursor.Hide();
                isMouseCurrentlyHidden = true;
            }
        }

        void GameWindowForm_MouseLeave(object sender, System.EventArgs e)
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

        public override bool AllowUserResizing
        {
            get
            {
                return (Control is GameWindowForm && ((GameWindowForm)Control).AllowUserResizing);
            }
            set
            {
                if (Control is GameWindowForm)
                {
                    ((GameWindowForm)Control).AllowUserResizing = value;
                }
            }
        }

        public override DrawingRectangle ClientBounds
        {
            get
            {
                return new DrawingRectangle(0, 0, Control.ClientSize.Width, Control.ClientSize.Height);
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
                var form  = Control as Form;
                if (form != null)
                {
                    return form.WindowState == FormWindowState.Minimized;
                }

                // Check for non-form control
                return false;
            }
        }
    }
}
#endif