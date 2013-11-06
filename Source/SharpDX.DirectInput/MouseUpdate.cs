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

using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    /// <summary>The mouse update struct.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseUpdate : IStateUpdate
    {
        /// <summary>Gets or sets the raw offset.</summary>
        /// <value>The raw offset.</value>
        public int RawOffset { get; set; }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public int Value { get; set; }

        /// <summary>Gets or sets the timestamp.</summary>
        /// <value>The timestamp.</value>
        public int Timestamp { get; set; }

        /// <summary>Gets or sets the sequence.</summary>
        /// <value>The sequence.</value>
        public int Sequence { get; set; }

        /// <summary>Gets the offset.</summary>
        /// <value>The offset.</value>
        public MouseOffset Offset { get { return (MouseOffset)RawOffset; } }

        /// <summary>Gets a value indicating whether this instance is button.</summary>
        /// <value><see langword="true" /> if this instance is button; otherwise, <see langword="false" />.</value>
        public bool IsButton { get { return Offset >= MouseOffset.Buttons0 && Offset <= MouseOffset.Buttons7;  } }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            object value;
            if (Offset >= MouseOffset.Buttons0) 
                value = (Value & 0x80) != 0;
            else
                value = Value;
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Offset: {0}, Value: {1} Timestamp: {2} Sequence: {3}", Offset, value, Timestamp, Sequence);
        }
    }
}