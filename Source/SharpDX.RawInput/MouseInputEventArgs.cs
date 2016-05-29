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

using System;

namespace SharpDX.RawInput
{
    /// <summary>
    /// RawInput Mouse event.
    /// </summary>
    public class MouseInputEventArgs : RawInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInputEventArgs"/> class.
        /// </summary>
        public MouseInputEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInputEventArgs"/> class.
        /// </summary>
        /// <param name="rawInput">The raw input.</param>
        /// <param name="hwnd">The handle of the window that received the RawInput mesage.</param>
        internal MouseInputEventArgs(ref RawInput rawInput, IntPtr hwnd)
            : base(ref rawInput, hwnd)
        {
            Mode = (MouseMode) rawInput.Data.Mouse.Flags;
            ButtonFlags = (MouseButtonFlags)rawInput.Data.Mouse.ButtonsData.ButtonFlags;
            WheelDelta = rawInput.Data.Mouse.ButtonsData.ButtonData;
            Buttons = rawInput.Data.Mouse.RawButtons;
            X = rawInput.Data.Mouse.LastX;
            Y = rawInput.Data.Mouse.LastY;
            ExtraInformation = rawInput.Data.Mouse.ExtraInformation;
        }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public MouseMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the button flags.
        /// </summary>
        /// <value>
        /// The button flags.
        /// </value>
        public MouseButtonFlags ButtonFlags { get; set; }

        /// <summary>
        /// Gets or sets the extra information.
        /// </summary>
        /// <value>
        /// The extra information.
        /// </value>
        public int ExtraInformation { get; set; }

        /// <summary>
        /// Gets or sets the raw buttons.
        /// </summary>
        /// <value>
        /// The raw buttons.
        /// </value>
        public int Buttons { get; set; }

        /// <summary>
        /// Gets or sets the wheel delta.
        /// </summary>
        /// <value>
        /// The wheel delta.
        /// </value>
        public int WheelDelta { get; set; }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public int Y { get; set; }
    }
}