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
using System.Windows.Forms;

using SharpDX.Windows;

namespace SharpDX.Toolkit
{
    internal class GameWindowForm : RenderForm
    {
        private bool allowUserResizing;
        private bool isFullScreenMaximized;
        private bool isMouseVisible;
        private bool isMouseCurrentlyHidden;

        public GameWindowForm() : this("SharpDX")
        {
        }

        public GameWindowForm(string text)
            : base(text)
        {
            // By default, non resizable
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MouseEnter += GameWindowForm_MouseEnter;
            MouseLeave += GameWindowForm_MouseLeave;
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

        internal bool IsMouseVisible
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

        internal bool AllowUserResizing
        {
            get
            {
                return allowUserResizing;
            }
            set
            {
                if (allowUserResizing != value)
                {
                    allowUserResizing = value;
                    MaximizeBox = allowUserResizing;
                    FormBorderStyle = allowUserResizing ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
                }
            }
        }
    }
}
#endif