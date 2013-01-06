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
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse) return;

            UpdateMouse(p);

            args.Handled = true;
        }

        private void HandlePointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            var p = args.CurrentPoint;
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse) return;

            UpdateMouse(p);

            args.Handled = true;
        }

        private void HandlePointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var p = args.CurrentPoint;
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse) return;

            UpdateMouse(p);

            args.Handled = true;
        }

        private void HandlePointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            var p = args.CurrentPoint;
            if (p.PointerDevice.PointerDeviceType != PointerDeviceType.Mouse) return;

            UpdateMouse(p);

            args.Handled = true;
        }

        private void UpdateMouse(PointerPoint p)
        {
            var dipFactor = DisplayProperties.LogicalDpi / 96.0f;
            pointerX = (int)(p.Position.X * dipFactor);
            pointerY = (int)(p.Position.Y * dipFactor);

            OnMouseWheel(p.Properties.MouseWheelDelta);
            RaiseButtonChange(p, MouseButton.Left, x => x.IsLeftButtonPressed);
            RaiseButtonChange(p, MouseButton.Middle, x => x.IsMiddleButtonPressed);
            RaiseButtonChange(p, MouseButton.Right, x => x.IsRightButtonPressed);
            RaiseButtonChange(p, MouseButton.XButton1, x => x.IsXButton1Pressed);
            RaiseButtonChange(p, MouseButton.XButton2, x => x.IsXButton2Pressed);
        }

        private void RaiseButtonChange(PointerPoint p, MouseButton button, System.Func<PointerPointProperties, bool> isDown)
        {
            if (isDown(p.Properties))
                OnMouseDown(button);
            else
                OnMouseUp(button);
        }
    }
}

#endif