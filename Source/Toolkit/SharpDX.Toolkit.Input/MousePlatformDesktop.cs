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

namespace SharpDX.Toolkit.Input
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Represents a specific <see cref="MousePlatform"/> implementation for WinForms platform (desktop)
    /// </summary>
    internal sealed class MousePlatformDesktop : MousePlatform
    {
        /// <summary>
        /// Initializes a new instance of <see cref="WindowBinderDesktop"/> class.
        /// </summary>
        /// <param name="nativeWindow">A reference to <see cref="Control"/> class.</param>
        public MousePlatformDesktop(object nativeWindow) : base(nativeWindow) { }

        /// <summary>
        /// Binds to specific events of the provided CoreWindow
        /// </summary>
        /// <param name="nativeWindow">A reference to <see cref="Control"/> class.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null.</exception>
        /// <exception cref="InvalidCastException">Is thrown when <paramref name="nativeWindow"/> is not an instance of the <see cref="Control"/> class.</exception>
        protected override void BindWindow(object nativeWindow)
        {
            if(nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            var w = (Control)nativeWindow;

            w.MouseDown += HandleMouseDown;
            w.MouseUp += HandleMouseUp;
            w.MouseMove += HandleMouseMove;
            w.MouseWheel += HandleMouseWheel;
        }

        /// <summary>
        /// Returns the mouse cursor position from cached values
        /// </summary>
        /// <param name="nativeWindow">A reference to <see cref="Control"/> class.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null.</exception>
        /// <exception cref="InvalidCastException">Is thrown when <paramref name="nativeWindow"/> is not an instance of the <see cref="Control"/> class.</exception>
        protected override Point GetLocationInternal(object nativeWindow)
        {
            if(nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            var w = (Control)nativeWindow;
            var p = w.PointToClient(Cursor.Position);

            return new Point(p.X, p.Y);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseDown"/> event
        /// </summary>
        /// <param name="sender">Event sender. Ignored.</param>
        /// <param name="e">Event arguments</param>
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(TranslateButton(e.Button));
            OnMouseWheel(e.Delta);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseUp"/> event
        /// </summary>
        /// <param name="sender">Event sender. Ignored.</param>
        /// <param name="e">Event arguments</param>
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(TranslateButton(e.Button));
            OnMouseWheel(e.Delta);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseMove"/> event
        /// </summary>
        /// <param name="sender">Event sender. Ignored.</param>
        /// <param name="e">Event arguments</param>
        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e.Delta);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseWheel"/> event
        /// </summary>
        /// <param name="sender">Event sender. Ignored.</param>
        /// <param name="e">Event arguments</param>
        private void HandleMouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e.Delta);
        }

        /// <summary>
        /// Translates the <see cref="MouseButtons"/> enum (WinForms specific) to platform-independent <see cref="MouseButton"/>.
        /// </summary>
        /// <param name="button">WinForms-specific <see cref="MouseButtons"/> value</param>
        /// <returns>Platform-independent <see cref="MouseButton"/> value</returns>
        private static MouseButton TranslateButton(MouseButtons button)
        {
            switch (button)
            {
            case MouseButtons.Left:
                return MouseButton.Left;
            case MouseButtons.None:
                return MouseButton.None;
            case MouseButtons.Right:
                return MouseButton.Right;
            case MouseButtons.Middle:
                return MouseButton.Middle;
            case MouseButtons.XButton1:
                return MouseButton.XButton1;
            case MouseButtons.XButton2:
                return MouseButton.XButton2;
            default:
                throw new ArgumentOutOfRangeException("button");
            }
        }
    }
}

#endif