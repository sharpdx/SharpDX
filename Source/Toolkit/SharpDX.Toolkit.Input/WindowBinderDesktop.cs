#if !W8CORE

namespace SharpDX.Toolkit.Input
{
    using System;
    using System.Windows.Forms;

    internal sealed class WindowBinderDesktop : WindowBinder
    {
        public WindowBinderDesktop(object nativeWindow) : base(nativeWindow) { }

        protected override void BindWindow(object nativeWindow)
        {
            var w = (Control)nativeWindow;

            w.MouseDown += HandleMouseDown;
            w.MouseUp += HandleMouseUp;
            w.MouseMove += HandleMouseMove;
            w.MouseWheel += HandleMouseWheel;
        }

        protected override Point GetLocationInternal(object nativeWindow)
        {
            var w = (Control)nativeWindow;
            var p = w.PointToClient(Cursor.Position);

            return new Point(p.X, p.Y);
        }

        private void HandleMouseDown(object _, MouseEventArgs e)
        {
            OnMouseDown(TranslateButton(e.Button));
            OnMouseWheel(e.Delta);
        }

        private void HandleMouseUp(object _, MouseEventArgs e)
        {
            OnMouseUp(TranslateButton(e.Button));
            OnMouseWheel(e.Delta);
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e.Delta);
        }

        private void HandleMouseWheel(object _, MouseEventArgs e)
        {
            OnMouseWheel(e.Delta);
        }

        private static MouseButton TranslateButton(MouseButtons button)
        {
            switch (button)
            {
            case MouseButtons.Left:
                return MouseButton.Left;
            case MouseButtons.None:
                return MouseButton.None;
            case MouseButtons.Right:
                return MouseButton.Right;
            case MouseButtons.Middle:
                return MouseButton.Middle;
            case MouseButtons.XButton1:
                return MouseButton.XButton1;
            case MouseButtons.XButton2:
                return MouseButton.XButton2;
            default:
                throw new ArgumentOutOfRangeException("button");
            }
        }
    }
}

#endif