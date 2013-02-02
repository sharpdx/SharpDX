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
#if WP8

using System;
using Windows.System;
using System.Windows.Controls;
using Windows.UI.Core;
using Windows.Phone.Input.Interop;

namespace SharpDX.Toolkit.Input
{
    internal sealed class PointerPlatformWP8 : PointerPlatform
    {
        private sealed class ManipulationHandler : IDrawingSurfaceManipulationHandler
        {
            private readonly PointerPlatformWP8 platform;

            public ManipulationHandler(PointerPlatformWP8 platform)
            {
                if (platform == null) throw new ArgumentNullException("platform");

                this.platform = platform;
            }

            public void SetManipulationHost(DrawingSurfaceManipulationHost manipulationHost)
            {
                manipulationHost.PointerMoved += (_, e) => platform.CreateAndAddPoint(PointerEventType.Moved, VirtualKeyModifiers.None, e.CurrentPoint);
                manipulationHost.PointerPressed += (_, e) => platform.CreateAndAddPoint(PointerEventType.Pressed, VirtualKeyModifiers.None, e.CurrentPoint);
                manipulationHost.PointerReleased += (_, e) => platform.CreateAndAddPoint(PointerEventType.Released, VirtualKeyModifiers.None, e.CurrentPoint);
            }
        }

        public PointerPlatformWP8(object nativeWindow, PointerManager manager)
            : base(nativeWindow, manager) { }

        protected override void BindWindow(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            var grid = nativeWindow as DrawingSurfaceBackgroundGrid;
            if (grid != null)
            {
                if (grid.Dispatcher.CheckAccess())
                    BindManipulationEvents(grid);
                else
                    grid.Dispatcher.BeginInvoke(() => BindManipulationEvents(grid));

                return;
            }

            throw new ArgumentException("Should be an instance of DrawingSurfaceBackgroundGrid", "nativeWindow");
        }

        private void BindManipulationEvents(DrawingSurfaceBackgroundGrid grid)
        {
            //grid.Loaded += (_, __) =>
                grid.SetBackgroundManipulationHandler(new ManipulationHandler(this));

            // TODO: review if we need to unbind the handlers
            grid.Unloaded += (_, __) => grid.SetBackgroundManipulationHandler(null);
            Disposing += (_, __) => grid.SetBackgroundManipulationHandler(null);
        }

        /// <summary>
        /// Creates a platform-independent instance of <see cref="PointerPoint"/> class from WP8-specific objects.
        /// </summary>
        /// <param name="modifiers">The pressed modifier keys.</param>
        /// <param name="point">The WP8-specific instance of pointer point.</param>
        /// <returns>An instance of <see cref="PointerPoint"/> class.</returns>
        private void CreateAndAddPoint(PointerEventType type, VirtualKeyModifiers modifiers, global::Windows.UI.Input.PointerPoint point)
        {
            if (point == null) throw new ArgumentNullException("point");

            var position = point.Position;
            var properties = point.Properties;
            var contactRect = properties.ContactRect;

            var result = new PointerPoint
            {
                EventType = type,
                DeviceType = PointerDeviceType.Touch,
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
                PointerUpdateKind = PointerUpdateKind.Other
            };

            manager.AddPointerEvent(ref result);
        }

        /// <summary>
        /// Maps from WP8-specific device type to platform-independent device type enum.
        /// </summary>
        /// <param name="pointerDeviceType">WP8-specific device type enumeration.</param>
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
        /// Maps from WP8-specific key modifiers enumeration to platform-independent flags.
        /// </summary>
        /// <param name="modifiers">WP8-specific key modifiers.</param>
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
        /// Maps from WP8-specific pointer update kind to platform-independent enum.
        /// </summary>
        /// <param name="pointerUpdateKind">WP8-specific pointer update kind enumeration.</param>
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