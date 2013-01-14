namespace SharpDX.Toolkit.Input
{
    using System;

    internal abstract class PointerPlatform
    {
        protected PointerPlatform(object nativeWindow, PointerManager manager)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");
            if (manager == null) throw new ArgumentNullException("manager");

            BindWindow(nativeWindow, manager);
        }

        internal static PointerPlatform Create(object nativeWindow, PointerManager manager)
        {
#if !W8CORE
#elif WIN8METRO
            return new PointerPlatformWinRT(nativeWindow, manager);
#else
            throw new NotSupportedException("This functionality is not supported on current platform."); // no other platforms are supported at this time
#endif
        }

        protected abstract void BindWindow(object nativeWindow, PointerManager manager);
    }
}