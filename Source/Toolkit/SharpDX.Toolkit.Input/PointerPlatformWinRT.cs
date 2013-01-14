namespace SharpDX.Toolkit.Input
{
    using System;
    using global::Windows.System;
    using global::Windows.UI.Core;
    using global::Windows.UI.Xaml;

    internal sealed class PointerPlatformWinRT : PointerPlatform
    {
        public PointerPlatformWinRT(object nativeWindow, PointerManager manager) : base(nativeWindow, manager) { }

        protected override void BindWindow(object nativeWindow, PointerManager manager)
        {
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

        private static PointerPoint BuildPoint(VirtualKeyModifiers modifiers, global::Windows.UI.Input.PointerPoint point)
        {
            var position = point.Position;

            return new PointerPoint
                   {
                       DeviceType = GetDeviceType(point.PointerDevice.PointerDeviceType),
                       KeyModifiers = GetKeyModifiers(modifiers),
                       PointerId = point.PointerId,
                       Position = new DrawingPointF((float)position.X, (float)position.Y),
                       Timestamp = point.Timestamp,
                       Properties = GetProperties(point.Properties)
                   };
        }

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

        private static KeyModifiers GetKeyModifiers(VirtualKeyModifiers modifiers)
        {
            var result = KeyModifiers.None;

            if (modifiers.HasFlag(VirtualKeyModifiers.Control)) result |= KeyModifiers.Control;
            if (modifiers.HasFlag(VirtualKeyModifiers.Menu)) result |= KeyModifiers.Menu;
            if (modifiers.HasFlag(VirtualKeyModifiers.Shift)) result |= KeyModifiers.Shift;
            if (modifiers.HasFlag(VirtualKeyModifiers.Windows)) result |= KeyModifiers.Windows;

            return result;
        }

        private static PointerPointProperties GetProperties(global::Windows.UI.Input.PointerPointProperties properties)
        {
            var contactRect = properties.ContactRect;

            return new PointerPointProperties
                   {
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