#if !W8CORE

namespace SharpDX.Toolkit.Input
{
    using System;

    internal sealed class WindowBinderDesktop : WindowBinder
    {
        public WindowBinderDesktop(object nativeWindow) : base(nativeWindow) { }

        protected override void BindWindow(object nativeWindow)
        {
            var w = (System.Windows.Forms.Control)nativeWindow;

            w.MouseDown += (_, e) => OnMouseDown(TranslateButton(e.Button));
            w.MouseUp += (_, e) => OnMouseUp(TranslateButton(e.Button));
            w.MouseWheel += (_, e) => OnMouseWheel(e.Delta);
        }

        protected override Point GetLocationInternal(object nativeWindow)
        {
            var w = (System.Windows.Forms.Control)nativeWindow;
            var p = w.PointToClient(System.Windows.Forms.Cursor.Position);

            return new Point(p.X, p.Y);
        }

        private static MouseButton TranslateButton(System.Windows.Forms.MouseButtons button)
        {
            switch (button)
            {
            case System.Windows.Forms.MouseButtons.Left:
                return MouseButton.Left;
            case System.Windows.Forms.MouseButtons.None:
                return MouseButton.None;
            case System.Windows.Forms.MouseButtons.Right:
                return MouseButton.Right;
            case System.Windows.Forms.MouseButtons.Middle:
                return MouseButton.Middle;
            case System.Windows.Forms.MouseButtons.XButton1:
                return MouseButton.XButton1;
            case System.Windows.Forms.MouseButtons.XButton2:
                return MouseButton.XButton2;
            default:
                throw new ArgumentOutOfRangeException("button");
            }
        }
    }
}

#endif