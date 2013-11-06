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
using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    /// <summary>The keyboard update struct.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardUpdate : IStateUpdate
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

        /// <summary>Gets the key.</summary>
        /// <value>The key.</value>
        public Key Key { get { return ConvertRawKey(this.RawOffset); } }

        /// <summary>Gets a value indicating whether this instance is pressed.</summary>
        /// <value><see langword="true" /> if this instance is pressed; otherwise, <see langword="false" />.</value>
        public bool IsPressed { get { return (this.Value & 0x80) != 0; } }

        /// <summary>Gets a value indicating whether this instance is released.</summary>
        /// <value><see langword="true" /> if this instance is released; otherwise, <see langword="false" />.</value>
        public bool IsReleased { get { return !IsPressed; } }

        /// <summary>Converts the raw key.</summary>
        /// <param name="rawKey">The raw key.</param>
        /// <returns>Key.</returns>
        private static Key ConvertRawKey(int rawKey)
        {
            if (Enum.IsDefined(typeof(Key), rawKey))
                return (Key)rawKey;
            return Key.Unknown;
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("Key: {0}, IsPressed: {1} Timestamp: {2} Sequence: {3}", Key, IsPressed, this.Timestamp, this.Sequence);
        }
    }
}