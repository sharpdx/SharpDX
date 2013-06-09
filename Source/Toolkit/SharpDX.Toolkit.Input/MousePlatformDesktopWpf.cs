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

#if !W8CORE && NET35Plus && !DIRECTX11_1

using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Represents a specific <see cref="MousePlatform"/> implementation for WPF platform (desktop)
    /// </summary>
    internal sealed class MousePlatformDesktopWpf : MousePlatform
    {
        private SharpDXElement element;

        /// <summary>
        /// Initializes a new instance of <see cref="MousePlatformDesktopWpf" /> class.
        /// </summary>
        /// <param name="nativeWindow">A reference to <see cref="SharpDXElement" /> class.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null.</exception>
        public MousePlatformDesktopWpf(object nativeWindow) : base(nativeWindow) { }

        /// <inheritdoc />
        internal override void SetLocation(Vector2 point)
        {
            var controlCoords = new System.Windows.Point(element.ActualWidth * point.X,
                                                         element.ActualHeight * point.Y);

            var screenCoords = element.PointToScreen(controlCoords);

            SetCursorPos((int)screenCoords.X, (int)screenCoords.Y);
        }

        /// <summary>
        /// Binds to specific events of the provided CoreWindow
        /// </summary>
        /// <param name="nativeWindow">A reference to <see cref="SharpDXElement"/> class.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null.</exception>
        /// <exception cref="InvalidCastException">Is thrown when <paramref name="nativeWindow"/> is not an instance of the <see cref="SharpDXElement"/> class.</exception>
        protected override void BindWindow(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            element = (SharpDXElement)nativeWindow;

            element.MouseDown += HandleMouseDown;
            element.MouseUp += HandleMouseUp;
            element.MouseWheel += HandleMouseWheel;
        }

        /// <summary>
        /// Handles the <see cref="System.Windows.UIElement.MouseDown"/> event to update the mouse state.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Event arguments from where is determined the changed button.</param>
        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseDown(TranslateButton(e.ChangedButton));
        }

        /// <summary>
        /// Handles the <see cref="System.Windows.UIElement.MouseUp"/> event to update the mouse state.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Event arguments from where is determined the changed button.</param>
        private void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseUp(TranslateButton(e.ChangedButton));
        }

        /// <summary>
        /// Handles the <see cref="System.Windows.UIElement.MouseWheel"/> event to update the mouse state.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Event arguments from where is determined the wheel delta.</param>
        private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(e.Delta);
        }

        /// <inheritdoc />
        protected override Vector2 GetLocationInternal()
        {
            var p = Mouse.GetPosition(element);

            var position = new Vector2((float)(p.X / element.ActualWidth), (float)(p.Y / element.ActualHeight));
            position.Saturate();
            return position;
        }

        /// <summary>
        /// Translates the <see cref="System.Windows.Input.MouseButton"/> enum (WinForms specific) to platform-independent <see cref="MouseButton"/>.
        /// </summary>
        /// <param name="button">WinForms-specific <see cref="System.Windows.Input.MouseButton"/> value.</param>
        /// <returns>Platform-independent <see cref="MouseButton"/> value.</returns>
        private static MouseButton TranslateButton(System.Windows.Input.MouseButton button)
        {
            switch (button)
            {
                case System.Windows.Input.MouseButton.Left:
                    return MouseButton.Left;
                case System.Windows.Input.MouseButton.Middle:
                    return MouseButton.Middle;
                case System.Windows.Input.MouseButton.Right:
                    return MouseButton.Right;
                case System.Windows.Input.MouseButton.XButton1:
                    return MouseButton.XButton1;
                case System.Windows.Input.MouseButton.XButton2:
                    return MouseButton.XButton2;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms648394%28v=vs.85%29.aspx
        /// </summary>
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);
    }
}

#endif