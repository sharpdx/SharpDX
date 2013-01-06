#if WIN8METRO

using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Input;

namespace SharpDX.Toolkit.Input
{
    internal class WindowBinderWinRT : WindowBinder
    {
        private int pointerX;
        private int pointerY;

        internal WindowBinderWinRT(object nativeWindow) : base(nativeWindow) { }

        protected override void BindWindow(object nativeWindow)
        {
            var w = (CoreWindow)nativeWindow;

            w.PointerPressed += HandlePointerPressed;
            w.PointerReleased += HandlePointerReleased;
            w.PointerWheelChanged += HandlePointerWheelChanged;
            w.PointerMoved += HandlePointerMoved;
        }

        protected override Point GetLocationInternal(object nativeWindow)
        {
            return new Point(pointerX, pointerY);
        }

        private void HandlePointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            var p = args.CurrentPoint;
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse
                || p.Properties.IsHorizontalMouseWheel) return;

            OnMouseWheel(p.Properties.MouseWheelDelta);

            args.Handled = true;
        }

        private void HandlePointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            var p = args.CurrentPoint;
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse) return;

            OnMouseDown(GetMouseButton(p));

            args.Handled = true;
        }

        private void HandlePointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var p = args.CurrentPoint;
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse) return;

            OnMouseUp(GetMouseButton(p));

            args.Handled = true;
        }

        private void HandlePointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            var p = args.CurrentPoint;
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse) return;

            var dipFactor = DisplayProperties.LogicalDpi / 96.0f;
            pointerX = (int)(p.Position.X * dipFactor);
            pointerY = (int)(p.Position.Y * dipFactor);
        }

        private static MouseButton GetMouseButton(PointerPoint p)
        {
            var properties = p.Properties;

            MouseButton button;
            if (properties.IsLeftButtonPressed)
                button = MouseButton.Left;
            else if (properties.IsRightButtonPressed)
                button = MouseButton.Right;
            else if (properties.IsMiddleButtonPressed)
                button = MouseButton.Middle;
            else if (properties.IsXButton1Pressed)
                button = MouseButton.XButton1;
            else if (properties.IsXButton2Pressed)
                button = MouseButton.XButton2;
            else
                throw new System.ArgumentOutOfRangeException("p", "Cannot determine which mouse button was pressed");
            return button;
        }
    }
}

#endif