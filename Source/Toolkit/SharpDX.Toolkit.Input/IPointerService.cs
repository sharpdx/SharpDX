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
    /// Provides access to platform-independent pointer events
    /// </summary>
    public interface IPointerService
    {
        /// <summary>
        /// Raised when <see cref="GameWindow.NativeWindow"/> object loses pointer capture
        /// </summary>
        event Action<PointerPoint> PointerCaptureLost;

        /// <summary>
        /// Raised when pointer enters <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        event Action<PointerPoint> PointerEntered;

        /// <summary>
        /// Raised when pointer exits <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        event Action<PointerPoint> PointerExited;

        /// <summary>
        /// Raised when pointer moves on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        event Action<PointerPoint> PointerMoved;

        /// <summary>
        /// Raised when pointer is pressed on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        event Action<PointerPoint> PointerPressed;

        /// <summary>
        /// Raised when pointer is released on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        event Action<PointerPoint> PointerReleased;

        /// <summary>
        /// Raised when pointer wheel is changed on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        event Action<PointerPoint> PointerWheelChanged;
    }
}