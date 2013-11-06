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
    /// <summary>The joystick update struct.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct JoystickUpdate : IStateUpdate
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
        public JoystickOffset Offset { get { return (JoystickOffset)RawOffset; } }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Offset: {0}, Value: {1} Timestamp: {2} Sequence: {3}", Offset, Value, Timestamp, Sequence);
        }
    }
}