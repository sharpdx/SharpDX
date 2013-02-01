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
    /// These flags represent the corresponding modifier keys that were pressed at some specific event.
    /// </summary>
    [Flags]
    public enum KeyModifiers
    {
        /// <summary>
        /// No modifier key are pressed.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The CTRL modifier key.
        /// </summary>
        Control = 0x01,

        /// <summary>
        /// The SHIFT modifier key.
        /// </summary>
        Shift = 0x02,

        /// <summary>
        /// The ALT modifier key.
        /// </summary>
        Menu = 0x04,

        /// <summary>
        /// The WIN modifier key.
        /// </summary>
        Windows = 0x08
    }
}