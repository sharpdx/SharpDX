namespace SharpDX.Toolkit.Input
{
    using System;

    internal abstract class WindowBinder
    {
        private readonly object nativeWindow;

        internal WindowBinder(object nativeWindow)
        {
            this.nativeWindow = nativeWindow;
            BindWindow(nativeWindow);
        }

        internal static WindowBinder Create(object nativeWindow)
        {
#if !W8CORE
            return new WindowBinderDesktop(nativeWindow);
#elif WIN8METRO
#else
            throw new NotSupportedException();
#endif
        }

        internal event Action<MouseButton> MouseDown;
        internal event Action<MouseButton> MouseUp;
        internal event Action<int> MouseWheelDelta;

        internal Point GetLocation()
        {
            return GetLocationInternal(nativeWindow);
        }

        protected abstract void BindWindow(object nativeWindow);

        protected abstract Point GetLocationInternal(object nativeWindow);

        protected void OnMouseDown(MouseButton button)
        {
            RaiseEvent(MouseDown, button);
        }

        protected void OnMouseUp(MouseButton button)
        {
            RaiseEvent(MouseUp, button);
        }

        protected void OnMouseWheel(int wheelDelta)
        {
            RaiseEvent(MouseWheelDelta, wheelDelta);
        }

        private void RaiseEvent<TArg>(Action<TArg> handler, TArg argument)
        {
            if (handler != null)
                handler(argument);
        }
    }
}