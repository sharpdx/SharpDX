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
using System.Windows.Input;

#if !W8CORE && NET35Plus && !DIRECTX11_1

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Specific implementation of <see cref="PointerPlatform"/> for WPF desktop platform.
    /// </summary>
    internal sealed class PointerPlatformDesktopWpf : PointerPlatform
    {
        private SharpDXElement element;

        /// <summary>
        /// Initializes a new instance of <see cref="PointerPlatformDesktopWpf"/> class.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        public PointerPlatformDesktopWpf(object nativeWindow, PointerManager manager)
            : base(nativeWindow, manager)
        {
        }

        /// <summary>
        /// Binds to pointer events of specified <paramref name="nativeWindow"/> object and raises the corresponding events on <paramref name="manager"/>.
        /// </summary>
        /// <param name="nativeWindow">An instance of <see cref="SharpDXElement"/>.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null.</exception>
        protected override void BindWindow(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            element = (SharpDXElement)nativeWindow;

            element.MouseDown += HandleMouseDown;
            element.MouseUp += HandleMouseUp;
            element.MouseMove += HandleMouseMove;
            element.MouseEnter += HandleMouseEnter;
            element.MouseLeave += HandleMouseLeave;
            element.MouseWheel += HandleMouseWheel;

            element.StylusButtonDown += HandleStylusDown;
            element.StylusButtonUp += HandleStylusUp;
            element.StylusMove += HandleStylusMove;
            element.StylusEnter += HandleStylusEnter;
            element.StylusLeave += HandleStylusLeave;

            element.TouchDown += HandleTouchDown;
            element.TouchUp += HandleTouchUp;
            element.TouchMove += HandleTouchMove;
            element.TouchEnter += HandleTouchEnter;
            element.TouchLeave += HandleTouchLeave;
        }

        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = Mouse.GetPosition(element);

            var position = new Vector2((float)(p.X / element.ActualWidth), (float)(p.Y / element.ActualHeight));
            position.Saturate();

            var point = new PointerPoint
            {
                EventType = PointerEventType.Pressed,
                DeviceType = PointerDeviceType.Mouse,
                KeyModifiers = GetPressedKeyModifiers(),
                PointerId = 0,
                Position = position,
                Timestamp = (ulong)DateTime.Now.Ticks,
                ContactRect = new RectangleF(position.X, position.Y, 0f, 0f),
                IsBarrelButtonPresset = false,
                IsCanceled = false,
                IsEraser = false,
                IsHorizontalMouseWheel = false,
                IsInRange = false,
                IsInverted = false,
                IsLeftButtonPressed = e.LeftButton == MouseButtonState.Pressed,
                IsMiddleButtonPressed = e.MiddleButton == MouseButtonState.Pressed,
                IsPrimary = true,
                IsRightButtonPressed = e.RightButton == MouseButtonState.Pressed,
                IsXButton1Pressed = e.XButton1 == MouseButtonState.Pressed,
                IsXButton2Pressed = e.XButton2 == MouseButtonState.Pressed,
                MouseWheelDelta = 0,
                Orientation = 0f,
                TouchConfidence = false, // ?
                Twist = 0f,
                XTilt = 0f,
                YTilt = 0f,
                PointerUpdateKind = pointerUpdateKind
            };

            manager.AddPointerEvent(ref point);
        }

        private KeyModifiers GetPressedKeyModifiers()
        {
            var modifiers = KeyModifiers.None;

            var currentModifiers = Keyboard.Modifiers;
            if (currentModifiers.HasFlag(ModifierKeys.Alt)) modifiers |= KeyModifiers.Menu;
            if (currentModifiers.HasFlag(ModifierKeys.Control)) modifiers |= KeyModifiers.Control;
            if (currentModifiers.HasFlag(ModifierKeys.Shift)) modifiers |= KeyModifiers.Shift;
            if (currentModifiers.HasFlag(ModifierKeys.Windows)) modifiers |= KeyModifiers.Windows;

            return modifiers;
        }

        private void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleMouseEnter(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleMouseLeave(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleStylusDown(object sender, StylusButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleStylusUp(object sender, StylusButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleStylusMove(object sender, StylusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleStylusEnter(object sender, StylusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleStylusLeave(object sender, StylusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleTouchDown(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleTouchUp(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleTouchMove(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleTouchEnter(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleTouchLeave(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

#endif
