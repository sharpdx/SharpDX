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
using System.Windows.Forms;

namespace SharpDX.RawInput
{
    /// <summary>
    /// RawInput Keyboard event.
    /// </summary>
    public class KeyboardInputEventArgs : RawInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInputEventArgs"/> class.
        /// </summary>
        public KeyboardInputEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInputEventArgs"/> class.
        /// </summary>
        /// <param name="rawInput">The raw input.</param>
        /// <param name="hwnd">The handle of the window that received the RawInput mesage.</param>
        internal KeyboardInputEventArgs(ref RawInput rawInput, IntPtr hwnd)
            : base(ref rawInput, hwnd)
        {
            Key = (Keys) rawInput.Data.Keyboard.VKey;
            MakeCode = rawInput.Data.Keyboard.MakeCode;
            ScanCodeFlags = rawInput.Data.Keyboard.Flags;
            State = rawInput.Data.Keyboard.Message;
            ExtraInformation = rawInput.Data.Keyboard.ExtraInformation;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Keys Key { get; set; }

        /// <summary>
        /// Gets or sets the make code.
        /// </summary>
        /// <value>
        /// The make code.
        /// </value>
        public int MakeCode { get; set; }

        /// <summary>
        /// Gets or sets the scan code flags.
        /// </summary>
        /// <value>
        /// The scan code flags.
        /// </value>
        public ScanCodeFlags ScanCodeFlags { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public KeyState State { get; set; }

        /// <summary>
        /// Gets or sets the extra information.
        /// </summary>
        /// <value>
        /// The extra information.
        /// </value>
        public int ExtraInformation { get; set; }
    }
}