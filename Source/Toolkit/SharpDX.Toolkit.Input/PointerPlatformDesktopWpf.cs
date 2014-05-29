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

using SharpDX.Mathematics;
#if !W8CORE && NET35Plus && !DIRECTX11_1
using System;
using System.Windows.Input;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Specific implementation of <see cref="PointerPlatform"/> for WPF desktop platform.
    /// </summary>
    internal sealed class PointerPlatformDesktopWpf : PointerPlatform
    {
        private SharpDXElement element;
        private int mousePressedButtonsCount;
        private int penPressedButtonsCount;
        private int touchPressedButtonsCount;

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
        /// Binds to pointer events of specified <paramref name="nativeWindow"/> object and raises the corresponding events on <see cref="PointerManager"/>.
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
            mousePressedButtonsCount++;
            // if there is already a pressed button - we should dispatch a 'Moved' event instead of 'Pressed'
            var eventType = mousePressedButtonsCount > 1 ? PointerEventType.Moved : PointerEventType.Pressed;
            AddMousePointerEvent(eventType, GetPressedUpdateKind(e.ChangedButton), 0, e);
        }

        private void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressedButtonsCount--;
            // if there remains a pressed button - we should dispatch a 'Moved' event instead of 'Released'
            var eventType = mousePressedButtonsCount > 0 ? PointerEventType.Moved : PointerEventType.Released;
            AddMousePointerEvent(eventType, GetReleasedUpdateKind(e.ChangedButton), 0, e);
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            AddMousePointerEvent(PointerEventType.Moved, PointerUpdateKind.Other, 0, e);
        }

        private void HandleMouseEnter(object sender, MouseEventArgs e)
        {
            AddMousePointerEvent(PointerEventType.Entered, PointerUpdateKind.Other, 0, e);
        }

        private void HandleMouseLeave(object sender, MouseEventArgs e)
        {
            AddMousePointerEvent(PointerEventType.Exited, PointerUpdateKind.Other, 0, e);
        }

        private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            AddMousePointerEvent(PointerEventType.WheelChanged, PointerUpdateKind.Other, e.Delta, e);
        }

        private void AddMousePointerEvent(PointerEventType eventType, PointerUpdateKind updateKind, int wheelDelta, MouseEventArgs e)
        {
            var p = Mouse.GetPosition(element);

            var point = new PointerPoint();

            FillPointInformation(ref point,
                                 eventType,
                                 PointerDeviceType.Mouse,
                                 updateKind,
                                 p);

            FillPressedButtons(e, ref point);

            point.MouseWheelDelta = wheelDelta;

            manager.AddPointerEvent(ref point);
        }

        private void HandleStylusDown(object sender, StylusButtonEventArgs e)
        {
            penPressedButtonsCount++;
            var eventType = penPressedButtonsCount > 1 ? PointerEventType.Moved : PointerEventType.Pressed;

            var updateKind = e.Inverted ? PointerUpdateKind.RightButtonPressed : PointerUpdateKind.LeftButtonPressed;

            AddPenPointerEvent(eventType, updateKind, true, e);
        }

        private void HandleStylusUp(object sender, StylusButtonEventArgs e)
        {
            penPressedButtonsCount--;
            var eventType = penPressedButtonsCount > 0 ? PointerEventType.Moved : PointerEventType.Released;

            var updateKind = e.Inverted ? PointerUpdateKind.RightButtonReleased : PointerUpdateKind.LeftButtonReleased;

            AddPenPointerEvent(eventType, updateKind, false, e);
        }

        private void HandleStylusMove(object sender, StylusEventArgs e)
        {
            AddPenPointerEvent(PointerEventType.Moved, PointerUpdateKind.Other, false, e);
        }

        private void HandleStylusEnter(object sender, StylusEventArgs e)
        {
            AddPenPointerEvent(PointerEventType.Entered, PointerUpdateKind.Other, false, e);
        }

        private void HandleStylusLeave(object sender, StylusEventArgs e)
        {
            AddPenPointerEvent(PointerEventType.Exited, PointerUpdateKind.Other, false, e);
        }

        private void AddPenPointerEvent(PointerEventType eventType, PointerUpdateKind updateKind, bool isPress, StylusEventArgs e)
        {
            var p = e.GetPosition(element);

            var point = new PointerPoint();

            FillPointInformation(ref point,
                                 eventType,
                                 PointerDeviceType.Pen,
                                 updateKind,
                                 p);

            // if this was a press - try to determine which button was pressed
            if (isPress)
            {
                if (e.Inverted)
                    point.IsRightButtonPressed = true;
                else
                    point.IsLeftButtonPressed = true;
            }

            manager.AddPointerEvent(ref point);
        }

        private void HandleTouchDown(object sender, TouchEventArgs e)
        {
            touchPressedButtonsCount++;
            var eventType = touchPressedButtonsCount > 1 ? PointerEventType.Moved : PointerEventType.Pressed;
            AddTouchEvent(eventType, e);
        }

        private void HandleTouchUp(object sender, TouchEventArgs e)
        {
            touchPressedButtonsCount--;
            var eventType = touchPressedButtonsCount > 0 ? PointerEventType.Moved : PointerEventType.Released;
            AddTouchEvent(eventType, e);
        }

        private void HandleTouchMove(object sender, TouchEventArgs e)
        {
            AddTouchEvent(PointerEventType.Moved, e);
        }

        private void HandleTouchEnter(object sender, TouchEventArgs e)
        {
            AddTouchEvent(PointerEventType.Entered, e);
        }

        private void HandleTouchLeave(object sender, TouchEventArgs e)
        {
            AddTouchEvent(PointerEventType.Exited, e);
        }

        private void AddTouchEvent(PointerEventType eventType, TouchEventArgs e)
        {
            var p = e.GetTouchPoint(element);

            var point = new PointerPoint();

            FillPointInformation(ref point,
                                 eventType,
                                 PointerDeviceType.Touch,
                                 GetTouchUpdateKind(p.Action),
                                 p.Position);

            manager.AddPointerEvent(ref point);
        }

        private static PointerUpdateKind GetTouchUpdateKind(TouchAction touchAction)
        {
            switch (touchAction)
            {
                case TouchAction.Down:
                    return PointerUpdateKind.LeftButtonPressed;
                case TouchAction.Move:
                    return PointerUpdateKind.Other;
                case TouchAction.Up:
                    return PointerUpdateKind.LeftButtonReleased;
                default:
                    throw new ArgumentOutOfRangeException("touchAction");
            }
        }

        private void FillPointInformation(ref PointerPoint point,
                                          PointerEventType eventType,
                                          PointerDeviceType deviceType,
                                          PointerUpdateKind updateKind,
                                          System.Windows.Point positionPoint)
        {
            var position = new Vector2((float)(positionPoint.X / element.ActualWidth), (float)(positionPoint.Y / element.ActualHeight));
            position.Saturate();

            point.EventType = eventType;
            point.DeviceType = deviceType;
            point.KeyModifiers = GetPressedKeyModifiers();
            point.PointerId = 0;
            point.Position = position;
            point.Timestamp = (ulong)DateTime.Now.Ticks;
            point.ContactRect = new RectangleF(position.X, position.Y, 0f, 0f);
            point.IsBarrelButtonPressed = false;
            point.IsCanceled = false;
            point.IsEraser = false;
            point.IsHorizontalMouseWheel = false;
            point.IsInRange = false;
            point.IsInverted = false;
            point.IsPrimary = true;
            point.MouseWheelDelta = 0;
            point.Orientation = 0f;
            point.TouchConfidence = false;
            point.Twist = 0f;
            point.XTilt = 0f;
            point.YTilt = 0f;
            point.PointerUpdateKind = updateKind;
        }

        private static void FillPressedButtons(MouseEventArgs e, ref PointerPoint point)
        {
            point.IsLeftButtonPressed = e.LeftButton == MouseButtonState.Pressed;
            point.IsMiddleButtonPressed = e.MiddleButton == MouseButtonState.Pressed;
            point.IsRightButtonPressed = e.RightButton == MouseButtonState.Pressed;
            point.IsXButton1Pressed = e.XButton1 == MouseButtonState.Pressed;
            point.IsXButton2Pressed = e.XButton2 == MouseButtonState.Pressed;
        }

        private static PointerUpdateKind GetPressedUpdateKind(System.Windows.Input.MouseButton changedButton)
        {
            switch (changedButton)
            {
                case System.Windows.Input.MouseButton.Left:
                    return PointerUpdateKind.LeftButtonPressed;
                case System.Windows.Input.MouseButton.Middle:
                    return PointerUpdateKind.MiddleButtonPressed;
                case System.Windows.Input.MouseButton.Right:
                    return PointerUpdateKind.RightButtonPressed;
                case System.Windows.Input.MouseButton.XButton1:
                    return PointerUpdateKind.XButton1Pressed;
                case System.Windows.Input.MouseButton.XButton2:
                    return PointerUpdateKind.XButton2Pressed;
                default:
                    throw new ArgumentOutOfRangeException("changedButton");
            }
        }

        private static PointerUpdateKind GetReleasedUpdateKind(System.Windows.Input.MouseButton changedButton)
        {
            switch (changedButton)
            {
                case System.Windows.Input.MouseButton.Left:
                    return PointerUpdateKind.LeftButtonReleased;
                case System.Windows.Input.MouseButton.Middle:
                    return PointerUpdateKind.MiddleButtonReleased;
                case System.Windows.Input.MouseButton.Right:
                    return PointerUpdateKind.RightButtonReleased;
                case System.Windows.Input.MouseButton.XButton1:
                    return PointerUpdateKind.XButton1Released;
                case System.Windows.Input.MouseButton.XButton2:
                    return PointerUpdateKind.XButton2Released;
                default:
                    throw new ArgumentOutOfRangeException("changedButton");
            }
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
    }
}

#endif
