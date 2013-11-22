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
using SharpDX.Windows;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// A <see cref="GameContext"/> to use for rendering to an existing WinForm <see cref="Control"/>.
    /// </summary>
    public partial class GameContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext" /> class with a default <see cref="RenderForm"/>.
        /// </summary>
        public GameContext()
            : this((Control)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public GameContext(Control control, int requestedWidth = 0, int requestedHeight = 0)
        {
            Control = control ?? new RenderForm("SharpDX Game") { AllowUserResizing = false };
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            ContextType = GameContextType.Desktop;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext" /> class.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public GameContext(IntPtr windowHandle, int requestedWidth = 0, int requestedHeight = 0)
        {
            Control = System.Windows.Forms.Control.FromHandle(windowHandle);
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            ContextType = GameContextType.Desktop;
        }

#if !W8CORE && NET35Plus
        protected GameContext(object control, int requestedWidth = 0, int requestedHeight = 0)
        {
            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            ContextType = GameContextType.DesktopWpf;
        }
#endif

        /// <summary>
        /// The control used as a GameWindow context (either an instance of <see cref="System.Windows.Forms.Control"/> or <see cref="System.Windows.Controls.Control"/>.
        /// </summary>
        public readonly object Control;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Control"/> to <see cref="GameContext"/>.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator GameContext(Control control)
        {
            return new GameContext(control);
        }
    }
}
#endif