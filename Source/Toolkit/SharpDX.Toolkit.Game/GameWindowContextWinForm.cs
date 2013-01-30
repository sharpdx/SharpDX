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
using System.Windows.Forms;

using SharpDX.Windows;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// A <see cref="GameWindowContext"/> to use for rendering to an existing WinForm <see cref="Control"/>.
    /// </summary>
    public class GameWindowContextWinForm : GameWindowContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowContextWinForm" /> class with a default <see cref="RenderForm"/>.
        /// </summary>
        public GameWindowContextWinForm() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowContextWinForm" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public GameWindowContextWinForm(Control control, int requestedWidth = 0, int requestedHeight = 0)
            : base(requestedWidth, requestedHeight)
        {
            Control = control ?? new GameForm("SharpDX.Toolkit.Game");
        }

        /// <summary>
        /// The control used as a GameWindow context.
        /// </summary>
        public readonly Control Control;
    }
}