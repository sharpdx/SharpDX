// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
    internal sealed partial class PointerPlatformDesktop : PointerPlatform
    {
        private Control control;

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
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null.</exception>
        protected override void BindWindow(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            control = nativeWindow as Control;
            if (control == null && nativeWindow is IntPtr)
            {
                control = Control.FromHandle((IntPtr)nativeWindow);
            }

            if (control == null)
                throw new InvalidOperationException(string.Format("Unsupported native window: {0}", nativeWindow));

            control.MouseLeave += (o, e) => CreateAndAddPoint(PointerEventType.Exited, PointerUpdateKind.Other, 0);
            control.MouseEnter += (o, e) => CreateAndAddPoint(PointerEventType.Entered, PointerUpdateKind.Other, 0);
            control.MouseMove += (o, e) => CreateAndAddPoint(PointerEventType.Moved, PointerUpdateKind.Other, e.Delta);
            control.MouseWheel += (o, e) => CreateAndAddPoint(PointerEventType.WheelChanged, PointerUpdateKind.Other, e.Delta);

            // these events have more complex handling, so they are moved to separate methods
            control.MouseDown += HandleMouseDown;
            control.MouseUp += HandleMouseUp;

            // try register touch events
            TryRegisterTouch();
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Is used to retrieve information about changed button (<see cref="MouseEventArgs.Button"/>) and wheel delta (<see cref="MouseEventArgs.Delta"/>).</param>
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            pressedButtonsCount++;

            var eventType = pressedButtonsCount > 1 ? PointerEventType.Moved : PointerEventType.Pressed;
            CreateAndAddPoint(eventType, TranslateMouseButtonDown(e.Button), e.Delta);
        }

        /// <summary>
        /// Handles the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Is used to retrieve information about changed button (<see cref="MouseEventArgs.Button"/>) and wheel delta (<see cref="MouseEventArgs.Delta"/>).</param>
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            pressedButtonsCount--;

            var eventType = pressedButtonsCount > 0 ? PointerEventType.Moved : PointerEventType.Released;
            CreateAndAddPoint(eventType, TranslateMouseButtonUp(e.Button), e.Delta);
        }

        /// <summary>
        /// Creates a <see cref="PointerPoint"/> instance from current mouse state.
        /// </summary>
        /// <param name="eventType">The type of pointer event.</param>
        /// <param name="pointerUpdateKind">The kind of pointer event.</param>
        /// <param name="wheelDelta">The current mouse wheel delta.</param>
        private void CreateAndAddPoint(PointerEventType eventType, PointerUpdateKind pointerUpdateKind, int wheelDelta)
        {
            var p = control.PointToClient(Control.MousePosition);

            var mouseButtons = Control.MouseButtons;

            var clientSize = control.ClientSize;
            var position = new Vector2((float)p.X / clientSize.Width, (float)p.Y / clientSize.Height);
            position.Saturate();

            var point = new PointerPoint
                   {
                       EventType = eventType,
                       DeviceType = PointerDeviceType.Mouse,
                       KeyModifiers = GetCurrentKeyModifiers(),
                       PointerId = 0,
                       Position = position,
                       Timestamp = (ulong)DateTime.Now.Ticks,
                       ContactRect = new RectangleF(position.X, position.Y, 0f, 0f),
                       IsBarrelButtonPressed = false,
                       IsCanceled = false,
                       IsEraser = false,
                       IsHorizontalMouseWheel = false,
                       IsInRange = false,
                       IsInverted = false,
                       IsLeftButtonPressed = (mouseButtons & MouseButtons.Left) != 0,
                       IsMiddleButtonPressed = (mouseButtons & MouseButtons.Middle) != 0,
                       IsPrimary = true,
                       IsRightButtonPressed = (mouseButtons & MouseButtons.Right) != 0,
                       IsXButton1Pressed = (mouseButtons & MouseButtons.XButton1) != 0,
                       IsXButton2Pressed = (mouseButtons & MouseButtons.XButton2) != 0,
                       MouseWheelDelta = wheelDelta,
                       Orientation = 0f,
                       TouchConfidence = false, // ?
                       Twist = 0f,
                       XTilt = 0f,
                       YTilt = 0f,
                       PointerUpdateKind = pointerUpdateKind
                   };

            manager.AddPointerEvent(ref point);
        }

        /// <summary>
        /// Gets the currently pressed key modifiers
        /// </summary>
        /// <returns>Currently pressed key modifiers</returns>
        private KeyModifiers GetCurrentKeyModifiers()
        {
            var modifierKeysDesktop = Control.ModifierKeys;
            var modifierKeys = KeyModifiers.None;

            if ((modifierKeysDesktop & FKeys.Shift) != 0) modifierKeys |= KeyModifiers.Shift;
            if ((modifierKeysDesktop & FKeys.Alt) != 0) modifierKeys |= KeyModifiers.Menu;
            if ((modifierKeysDesktop & FKeys.Control) != 0) modifierKeys |= KeyModifiers.Control;

            return modifierKeys;
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