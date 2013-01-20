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
using FKeys = System.Windows.Forms.Keys;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Specific implementation of <see cref="PointerPlatform"/> for Desktop
    /// </summary>
    internal sealed class PointerPlatformDesktop : PointerPlatform
    {
        private Control control;
        private PointerManager manager;

        // used to store the count of currently pressed mouse buttons, as subsequent buttons should raise 'Moved' events.
        private int pressedButtonsCount;

        /// <summary>
        /// Initializes a new instance of <see cref="PointerPlatformDesktop"/> class.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        public PointerPlatformDesktop(object nativeWindow, PointerManager manager)
            : base(nativeWindow, manager) { }

        /// <summary>
        /// Binds to pointer events of specified <paramref name="nativeWindow"/> object and raises the corresponding events on <paramref name="manager"/>.
        /// </summary>
        /// <param name="nativeWindow">An instance of <see cref="Control"/>.</param>
        /// <param name="manager">The related <see cref="PointerManager"/> instance.</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        protected override void BindWindow(object nativeWindow, PointerManager manager)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");
            if (manager == null) throw new ArgumentNullException("manager");

            control = (Control)nativeWindow;
            this.manager = manager;

            control.MouseLeave += HandleMouseLeave;
            control.MouseEnter += HandleMouseEnter;
            control.MouseMove += HandleMouseMove;
            control.MouseDown += HandleMouseDown;
            control.MouseUp += HandleMouseUp;
            control.MouseWheel += HandleMouseWheel;
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleMouseLeave(object sender, EventArgs e)
        {
            manager.RaiseExited(CreatePoint(PointerUpdateKind.Other, 0));
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseEnter"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleMouseEnter(object sender, EventArgs e)
        {
            manager.RaiseEntered(CreatePoint(PointerUpdateKind.Other, 0));
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseMove"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Is used to retrieve information about mouse wheel delta (<see cref="MouseEventArgs.Delta"/>).</param>
        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            manager.RaiseMoved(CreatePoint(PointerUpdateKind.Other, e.Delta));
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Is used to retrieve information about changed button (<see cref="MouseEventArgs.Button"/>) and wheel delta (<see cref="MouseEventArgs.Delta"/>).</param>
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            pressedButtonsCount++;

            var point = CreatePoint(TranslateMouseButtonDown(e.Button), e.Delta);

            if (pressedButtonsCount > 1)
                manager.RaiseMoved(point);
            else
                manager.RaisePressed(point);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Is used to retrieve information about changed button (<see cref="MouseEventArgs.Button"/>) and wheel delta (<see cref="MouseEventArgs.Delta"/>).</param>
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            pressedButtonsCount--;

            var point = CreatePoint(TranslateMouseButtonUp(e.Button), e.Delta);

            if (pressedButtonsCount > 0)
                manager.RaiseMoved(point);
            else
                manager.RaiseReleased(point);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseWheel"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void HandleMouseWheel(object sender, MouseEventArgs e)
        {
            manager.RaiseWheelChanged(CreatePoint(PointerUpdateKind.Other, e.Delta));
        }

        /// <summary>
        /// Creates a <see cref="PointerPoint"/> instance from current mouse state.
        /// </summary>
        /// <param name="pointerUpdateKind">The kind of pointer event.</param>
        /// <param name="wheelDelta">The current mouse wheel delta.</param>
        /// <returns>An instance of <see cref="PointerPoint"/> that represents the current state of mouse device.</returns>
        private PointerPoint CreatePoint(PointerUpdateKind pointerUpdateKind, int wheelDelta)
        {
            var modifierKeysDesktop = Control.ModifierKeys;

            var modifierKeys = KeyModifiers.None;
            if (modifierKeysDesktop.HasFlag(FKeys.Shift)) modifierKeys |= KeyModifiers.Shift;
            if (modifierKeysDesktop.HasFlag(FKeys.Alt)) modifierKeys |= KeyModifiers.Menu;
            if (modifierKeysDesktop.HasFlag(FKeys.Control)) modifierKeys |= KeyModifiers.Control;

            var position = control.PointToClient(Control.MousePosition);

            var mouseButtons = Control.MouseButtons;

            var props = new PointerPointProperties
                        {
                            ContactRect = new DrawingRectangleF(position.X, position.Y, 0f, 0f),
                            IsBarrelButtonPresset = false,
                            IsCanceled = false,
                            IsEraser = false,
                            IsHorizontalMouseWheel = false,
                            IsInRange = false,
                            IsInverted = false,
                            IsLeftButtonPressed = mouseButtons.HasFlag(MouseButtons.Left),
                            IsMiddleButtonPressed = mouseButtons.HasFlag(MouseButtons.Middle),
                            IsPrimary = true,
                            IsRightButtonPressed = mouseButtons.HasFlag(MouseButtons.Right),
                            IsXButton1Pressed = mouseButtons.HasFlag(MouseButtons.XButton1),
                            IsXButton2Pressed = mouseButtons.HasFlag(MouseButtons.XButton2),
                            MouseWheelDelta = wheelDelta,
                            Orientation = 0f,
                            TouchConfidence = false, // ?
                            Twist = 0f,
                            XTilt = 0f,
                            YTilt = 0f,
                            PointerUpdateKind = pointerUpdateKind
                        };

            return new PointerPoint
                   {
                       DeviceType = PointerDeviceType.Mouse,
                       KeyModifiers = modifierKeys,
                       PointerId = 0,
                       Position = new DrawingPointF(position.X, position.Y),
                       Timestamp = (ulong)DateTime.Now.Ticks,
                       Properties = props
                   };
        }

        /// <summary>
        /// Translates the <see cref="MouseButtons"/> to corresponding <see cref="PointerUpdateKind"/> according to <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="button">The pressed mouse button.</param>
        /// <returns>The corresponding pointer update kind.</returns>
        private static PointerUpdateKind TranslateMouseButtonDown(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return PointerUpdateKind.LeftButtonPressed;
                case MouseButtons.None:
                    return PointerUpdateKind.Other;
                case MouseButtons.Right:
                    return PointerUpdateKind.RightButtonPressed;
                case MouseButtons.Middle:
                    return PointerUpdateKind.MiddleButtonPressed;
                case MouseButtons.XButton1:
                    return PointerUpdateKind.XButton1Pressed;
                case MouseButtons.XButton2:
                    return PointerUpdateKind.XButton2Pressed;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }

        /// <summary>
        /// Translates the <see cref="MouseButtons"/> to corresponding <see cref="PointerUpdateKind"/> according to <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="button">The released mouse button.</param>
        /// <returns>The corresponding pointer update kind.</returns>
        private static PointerUpdateKind TranslateMouseButtonUp(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return PointerUpdateKind.LeftButtonReleased;
                case MouseButtons.None:
                    return PointerUpdateKind.Other;
                case MouseButtons.Right:
                    return PointerUpdateKind.RightButtonReleased;
                case MouseButtons.Middle:
                    return PointerUpdateKind.MiddleButtonReleased;
                case MouseButtons.XButton1:
                    return PointerUpdateKind.XButton1Released;
                case MouseButtons.XButton2:
                    return PointerUpdateKind.XButton2Released;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }
    }
}

#endif