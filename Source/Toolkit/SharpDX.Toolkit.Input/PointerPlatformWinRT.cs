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

#if WIN8METRO

using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// The <see cref="PointerPlatform"/> implementation for WinRT platform.
    /// </summary>
    internal sealed class PointerPlatformWinRT : PointerPlatform
    {
        /// <summary>
        /// Initializes a new intance of the <see cref="PointerPlatformWinRT"/> class.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        internal PointerPlatformWinRT(object nativeWindow, PointerManager manager) : base(nativeWindow, manager) { }

        /// <summary>
        /// Binds to pointer events of specified <paramref name="nativeWindow"/> object and raises the corresponding events on <paramref name="manager"/>.
        /// </summary>
        /// <param name="nativeWindow">An instance of either <see cref="CoreWindow"/> or <see cref="UIElement"/> object.</param>
        /// <param name="manager">The related <see cref="PointerManager"/> instance.</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        protected override void BindWindow(object nativeWindow, PointerManager manager)
        {
            if(nativeWindow == null) throw new ArgumentNullException("nativeWindow");
            if(manager == null) throw new ArgumentNullException("manager");

            var window = nativeWindow as CoreWindow;
            if (window != null)
            {
                window.PointerCaptureLost += (_, e) => manager.RaiseCaptureLost(BuildPoint(e.KeyModifiers, e.CurrentPoint));
                window.PointerEntered += (_, e) => manager.RaiseEntered(BuildPoint(e.KeyModifiers, e.CurrentPoint));
                window.PointerExited += (_, e) => manager.RaiseExited(BuildPoint(e.KeyModifiers, e.CurrentPoint));
                window.PointerMoved += (_, e) => manager.RaiseMoved(BuildPoint(e.KeyModifiers, e.CurrentPoint));
                window.PointerPressed += (_, e) => manager.RaisePressed(BuildPoint(e.KeyModifiers, e.CurrentPoint));
                window.PointerReleased += (_, e) => manager.RaiseReleased(BuildPoint(e.KeyModifiers, e.CurrentPoint));
                window.PointerWheelChanged += (_, e) => manager.RaiseWheelChanged(BuildPoint(e.KeyModifiers, e.CurrentPoint));
                return;
            }

            var control = nativeWindow as UIElement;
            if (control != null)
            {
                control.PointerCaptureLost += (_, e) => manager.RaiseCaptureLost(BuildPoint(e.KeyModifiers, e.GetCurrentPoint(control)));
                control.PointerEntered += (_, e) => manager.RaiseEntered(BuildPoint(e.KeyModifiers, e.GetCurrentPoint(control)));
                control.PointerExited += (_, e) => manager.RaiseExited(BuildPoint(e.KeyModifiers, e.GetCurrentPoint(control)));
                control.PointerMoved += (_, e) => manager.RaiseMoved(BuildPoint(e.KeyModifiers, e.GetCurrentPoint(control)));
                control.PointerPressed += (_, e) => manager.RaisePressed(BuildPoint(e.KeyModifiers, e.GetCurrentPoint(control)));
                control.PointerReleased += (_, e) => manager.RaiseReleased(BuildPoint(e.KeyModifiers, e.GetCurrentPoint(control)));
                control.PointerWheelChanged += (_, e) => manager.RaiseWheelChanged(BuildPoint(e.KeyModifiers, e.GetCurrentPoint(control)));
                return;
            }

            throw new ArgumentException("Should be an instance of either CoreWindow or UIElement", "nativeWindow");
        }

        /// <summary>
        /// Creates a platform-independent instance of <see cref="PointerPoint"/> class from WinRT-specific objects.
        /// </summary>
        /// <param name="modifiers">The pressed modifier keys.</param>
        /// <param name="point">The WinRT-specific instance of pointer point.</param>
        /// <returns>An instance of <see cref="PointerPoint"/> class.</returns>
        private static PointerPoint BuildPoint(VirtualKeyModifiers modifiers, global::Windows.UI.Input.PointerPoint point)
        {
            if(point == null) throw new ArgumentNullException("point");

            var position = point.Position;
            var properties = point.Properties;
            var contactRect = properties.ContactRect;

            return new PointerPoint
                   {
                       DeviceType = GetDeviceType(point.PointerDevice.PointerDeviceType),
                       KeyModifiers = GetKeyModifiers(modifiers),
                       PointerId = point.PointerId,
                       Position = new DrawingPointF((float)position.X, (float)position.Y),
                       Timestamp = point.Timestamp,
                       ContactRect = new DrawingRectangleF((float)contactRect.X, (float)contactRect.Y, (float)contactRect.Width, (float)contactRect.Height),
                       IsBarrelButtonPresset = properties.IsBarrelButtonPressed,
                       IsCanceled = properties.IsCanceled,
                       IsEraser = properties.IsEraser,
                       IsHorizontalMouseWheel = properties.IsHorizontalMouseWheel,
                       IsInRange = properties.IsInRange,
                       IsInverted = properties.IsInverted,
                       IsLeftButtonPressed = properties.IsLeftButtonPressed,
                       IsMiddleButtonPressed = properties.IsMiddleButtonPressed,
                       IsPrimary = properties.IsPrimary,
                       IsRightButtonPressed = properties.IsRightButtonPressed,
                       IsXButton1Pressed = properties.IsXButton1Pressed,
                       IsXButton2Pressed = properties.IsXButton2Pressed,
                       MouseWheelDelta = properties.MouseWheelDelta,
                       Orientation = properties.Orientation,
                       TouchConfidence = properties.TouchConfidence,
                       Twist = properties.Twist,
                       XTilt = properties.XTilt,
                       YTilt = properties.YTilt,
                       PointerUpdateKind = GetPointerUpdateKind(properties.PointerUpdateKind)
                   };
        }

        /// <summary>
        /// Maps from WinRT-specific device type to platform-independent device type enum.
        /// </summary>
        /// <param name="pointerDeviceType">WinRT specific device type enumeration.</param>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="pointerDeviceType"/> is not recognized.</exception>
        /// <returns>Platform-independent device type enumeration</returns>
        private static PointerDeviceType GetDeviceType(global::Windows.Devices.Input.PointerDeviceType pointerDeviceType)
        {
            switch (pointerDeviceType)
            {
            case global::Windows.Devices.Input.PointerDeviceType.Touch:
                return PointerDeviceType.Touch;
            case global::Windows.Devices.Input.PointerDeviceType.Pen:
                return PointerDeviceType.Pen;
            case global::Windows.Devices.Input.PointerDeviceType.Mouse:
                return PointerDeviceType.Mouse;
            default:
                throw new ArgumentOutOfRangeException("pointerDeviceType");
            }
        }

        /// <summary>
        /// Maps from WinRT-specific key modifiers enumeration to platform-independent flags.
        /// </summary>
        /// <param name="modifiers">WinRT-specific key modifiers.</param>
        /// <returns>Platform-independent flags.</returns>
        private static KeyModifiers GetKeyModifiers(VirtualKeyModifiers modifiers)
        {
            var result = KeyModifiers.None;

            if (modifiers.HasFlag(VirtualKeyModifiers.Control)) result |= KeyModifiers.Control;
            if (modifiers.HasFlag(VirtualKeyModifiers.Menu)) result |= KeyModifiers.Menu;
            if (modifiers.HasFlag(VirtualKeyModifiers.Shift)) result |= KeyModifiers.Shift;
            if (modifiers.HasFlag(VirtualKeyModifiers.Windows)) result |= KeyModifiers.Windows;

            return result;
        }

        /// <summary>
        /// Maps from WinRT-specific pointer update kind to platform-independent enum.
        /// </summary>
        /// <param name="pointerUpdateKind">WinRT specific pointer update kind enumeration.</param>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="pointerUpdateKind"/> is not recognized.</exception>
        /// <returns>Platform-independent pointer update kind enumeration.</returns>
        private static PointerUpdateKind GetPointerUpdateKind(global::Windows.UI.Input.PointerUpdateKind pointerUpdateKind)
        {
            switch (pointerUpdateKind)
            {
            case global::Windows.UI.Input.PointerUpdateKind.Other:
                return PointerUpdateKind.Other;
            case global::Windows.UI.Input.PointerUpdateKind.LeftButtonPressed:
                return PointerUpdateKind.LeftButtonPressed;
            case global::Windows.UI.Input.PointerUpdateKind.LeftButtonReleased:
                return PointerUpdateKind.LeftButtonReleased;
            case global::Windows.UI.Input.PointerUpdateKind.RightButtonPressed:
                return PointerUpdateKind.RightButtonPressed;
            case global::Windows.UI.Input.PointerUpdateKind.RightButtonReleased:
                return PointerUpdateKind.RightButtonReleased;
            case global::Windows.UI.Input.PointerUpdateKind.MiddleButtonPressed:
                return PointerUpdateKind.MiddleButtonPressed;
            case global::Windows.UI.Input.PointerUpdateKind.MiddleButtonReleased:
                return PointerUpdateKind.MiddleButtonReleased;
            case global::Windows.UI.Input.PointerUpdateKind.XButton1Pressed:
                return PointerUpdateKind.XButton1Pressed;
            case global::Windows.UI.Input.PointerUpdateKind.XButton1Released:
                return PointerUpdateKind.XButton1Released;
            case global::Windows.UI.Input.PointerUpdateKind.XButton2Pressed:
                return PointerUpdateKind.XButton2Pressed;
            case global::Windows.UI.Input.PointerUpdateKind.XButton2Released:
                return PointerUpdateKind.XButton2Released;
            default:
                throw new ArgumentOutOfRangeException("pointerUpdateKind");
            }
        }
    }
}

#endif