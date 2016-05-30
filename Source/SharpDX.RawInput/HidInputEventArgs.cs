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
    /// Describes the format of the raw input from a Human Interface Device (HID). 
    /// </summary>
    public class HidInputEventArgs : RawInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HidInputEventArgs"/> class.
        /// </summary>
        public HidInputEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HidInputEventArgs"/> class.
        /// </summary>
        /// <param name="rawInput">The raw input.</param>
        /// <param name="hwnd">The handle of the window that received the RawInput mesage.</param>
        internal HidInputEventArgs(ref RawInput rawInput, IntPtr hwnd) : base(ref rawInput, hwnd)
        {
            Count = rawInput.Data.Hid.Count;
            DataSize = rawInput.Data.Hid.SizeHid;
            RawData = new byte[Count * DataSize];
            unsafe
            {
                if (RawData.Length > 0) fixed (void* __to = RawData) fixed (void* __from = &rawInput.Data.Hid.RawData) SharpDX.Utilities.CopyMemory((IntPtr)__to, (IntPtr)__from, RawData.Length *sizeof(byte));
            }
        }

        /// <summary>
        /// Gets or sets the number of Hid structure in the <see cref="RawData"/>.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the size of the Hid structure in the <see cref="RawData"/>.
        /// </summary>
        /// <value>
        /// The size of the data.
        /// </value>
        public int DataSize { get; set; }

        /// <summary>
        /// Gets or sets the raw data.
        /// </summary>
        /// <value>
        /// The raw data.
        /// </value>
        public byte[] RawData { get; set; }
    }
}