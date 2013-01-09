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

namespace SharpDX.Toolkit.Input
{
    using System;

    /// <summary>
    /// Base class for platform-specific event bindings
    /// </summary>
    internal abstract class MousePlatform
    {
        private readonly object nativeWindow; // used to retrieve mouse location

        /// <summary>
        /// Initializes a new instance of <see cref="MousePlatform"/> class
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null</exception>
        protected MousePlatform(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            this.nativeWindow = nativeWindow;
            BindWindow(nativeWindow);
        }

        /// <summary>
        /// Creates a platform-specific instance of <see cref="MousePlatform"/> class.
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null</exception>
        /// <returns>The instance of <see cref="MousePlatform"/></returns>
        internal static MousePlatform Create(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");
#if !W8CORE
            return new MousePlatformDesktop(nativeWindow); // WinForms platform
#elif WIN8METRO
            return new MousePlatformWinRT(nativeWindow); // WinRT platform
#else
            throw new NotSupportedException("This functionality is not supported on current platform."); // no other platforms are supported at this time
#endif
        }

        /// <summary>
        /// Raised when a button is pressed
        /// </summary>
        internal event Action<MouseButton> MouseDown;

        /// <summary>
        /// Raised when a button is released
        /// </summary>
        internal event Action<MouseButton> MouseUp;

        /// <summary>
        /// Raised when mouse wheel delta is changed
        /// </summary>
        internal event Action<int> MouseWheelDelta;

        /// <summary>
        /// Returns the location of mouse cursor relative to program window
        /// </summary>
        /// <returns></returns>
        internal Point GetLocation()
        {
            return GetLocationInternal(nativeWindow);
        }

        /// <summary>
        /// Derived classes should implement platform-specific event bindings in this method
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        protected abstract void BindWindow(object nativeWindow);

        /// <summary>
        /// Derived classes should implement platform-specific code to retrieve the mouse cursor location
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        protected abstract Point GetLocationInternal(object nativeWindow);

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event
        /// </summary>
        /// <param name="button">Mouse button which has been pressed</param>
        protected void OnMouseDown(MouseButton button)
        {
            Raise(MouseDown, button);
        }

        /// <summary>
        /// Raises the <see cref="MouseUp"/> event
        /// </summary>
        /// <param name="button">Mouse button which has been released</param>
        protected void OnMouseUp(MouseButton button)
        {
            Raise(MouseUp, button);
        }

        /// <summary>
        /// Raises the <see cref="MouseWheelDelta"/> event
        /// </summary>
        /// <param name="wheelDelta">Current value of mouse wheel delta</param>
        protected void OnMouseWheel(int wheelDelta)
        {
            Raise(MouseWheelDelta, wheelDelta);
        }

        /// <summary>
        /// Generic helper method to call a single-parameter event handler
        /// </summary>
        /// <remarks>This ensures that during the call - the handler reference will not be lost (due to stack-copy of delegate reference)</remarks>
        /// <typeparam name="TArg">The type of event argument</typeparam>
        /// <param name="handler">The reference to event delegate</param>
        /// <param name="argument">The event argument</param>
        private static void Raise<TArg>(Action<TArg> handler, TArg argument)
        {
            if (handler != null)
                handler(argument);
        }
    }
}