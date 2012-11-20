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
        public Control Control;


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
            windowContext = windowContext ?? new GameWindowForm("SharpDX.Toolkit.Game");
            Control = windowContext as Control;
            if (Control == null)
            {
                throw new NotSupportedException("Unsupported window context. Unable to create game window. Only System.Windows.Control subclass are supported");
            }

            var renderForm = windowContext as GameWindowForm;
            if (renderForm != null)
            {
                renderForm.AppActivated += OnActivated;
                renderForm.AppDeactivated += OnDeactivated;
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