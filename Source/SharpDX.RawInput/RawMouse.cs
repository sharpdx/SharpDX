// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

using System.Runtime.InteropServices;

namespace SharpDX.RawInput
{
    /// <summary>
    /// Contains information about the state of the mouse.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawMouse
    {
        /// <summary>
        /// The mouse state.
        /// </summary>
        public short Flags;

        [StructLayout(LayoutKind.Explicit)]
        public struct RawMouseButtonsData
        {
            [FieldOffset(0)]
            public int Buttons;

            /// <summary>
            /// Flags for the event.
            /// </summary>
            [FieldOffset(0)]
            public short ButtonFlags;

            /// <summary>
            /// If the mouse wheel is moved, this will contain the delta amount.
            /// </summary>
            [FieldOffset(2)]
            public short ButtonData;
        }

        public RawMouseButtonsData ButtonsData;

        /// <summary>
        /// Raw button data.
        /// </summary>
        public int RawButtons;
        /// <summary>
        /// The motion in the X direction. This is signed relative motion or
        /// absolute motion, depending on the value of usFlags.
        /// </summary>
        public int LastX;
        /// <summary>
        /// The motion in the Y direction. This is signed relative motion or absolute motion,
        /// depending on the value of usFlags.
        /// </summary>
        public int LastY;
        /// <summary>
        /// The device-specific additional information for the event.
        /// </summary>
        public int ExtraInformation;
    }
}

