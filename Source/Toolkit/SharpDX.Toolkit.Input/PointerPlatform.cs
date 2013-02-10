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

using System;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Base class for platform-specific event bindings
    /// </summary>
    internal abstract class PointerPlatform : Component
    {
        protected readonly PointerManager manager;

        /// <summary>
        /// Initializes a new instance of <see cref="PointerPlatform"/> class
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        protected PointerPlatform(object nativeWindow, PointerManager manager)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");
            if (manager == null) throw new ArgumentNullException("manager");

            this.manager = manager;

            BindWindow(nativeWindow);
        }

        /// <summary>
        /// Creates a platform-specific instance of <see cref="PointerPlatform"/> class.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        /// <exception cref="NotSupportedException">Is thrown when this functionality is not supported on current platform</exception>
        /// <returns>The platform-specific instance.</returns>
        internal static PointerPlatform Create(object nativeWindow, PointerManager manager)
        {
#if !W8CORE
            return new PointerPlatformDesktop(nativeWindow, manager);
#elif WIN8METRO
            return new PointerPlatformWinRT(nativeWindow, manager);
#elif WP8
            return new PointerPlatformWP8(nativeWindow, manager);
#else
            throw new NotSupportedException("This functionality is not supported on current platform."); // no other platforms are supported at this time
#endif
        }

        /// <summary>
        /// Derived classes should perform the binding to platform-specific events on <paramref name="nativeWindow"/> and raise the corresponding events on <paramref name="manager"/>.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        protected abstract void BindWindow(object nativeWindow);
    }
}