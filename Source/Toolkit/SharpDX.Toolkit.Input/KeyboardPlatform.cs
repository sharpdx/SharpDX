// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

using System;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Provides platform-specific bindings to keyboard input events
    /// </summary>
    internal abstract class KeyboardPlatform
    {
        /// <summary>
        /// Creates a new instance of <see cref="KeyboardPlatform"/> class.
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null</exception>
        protected KeyboardPlatform(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");
            BindWindow(nativeWindow);
        }

        /// <summary>
        /// Creates a platform-specific instance of <see cref="KeyboardPlatform"/> class.
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null</exception>
        /// <returns>The instance of <see cref="KeyboardPlatform"/></returns>
        internal static KeyboardPlatform Create(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");
#if !W8CORE
            return new KeyboardPlatformDesktop(nativeWindow); // Desktop platform
#elif WIN8METRO
            return new KeyboardPlatformWinRT(nativeWindow);
#else
            throw new NotSupportedException("This functionality is not supported on current platform."); // no other platforms are supported at this time
#endif
        }

        /// <summary>
        /// Raised when a key pressed
        /// </summary>
        internal event Action<Keys> KeyPressed;

        /// <summary>
        /// Raised when a key is released
        /// </summary>
        internal event Action<Keys> KeyReleased;

        /// <summary>
        /// Derived classes should implement platform-specific event bindings in this method
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        protected abstract void BindWindow(object nativeWindow);

        /// <summary>
        /// Raises the <see cref="KeyPressed"/> event.
        /// </summary>
        /// <param name="key">The key that was pressed</param>
        protected void RaiseKeyPressed(Keys key)
        {
            if (key == Keys.None) return;
            Raise(KeyPressed, key);
        }

        /// <summary>
        /// Raises the <see cref="KeyReleased"/> event.
        /// </summary>
        /// <param name="key">The key that was released</param>
        protected void RaiseKeyReleased(Keys key)
        {
            if (key == Keys.None) return;
            Raise(KeyReleased, key);
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