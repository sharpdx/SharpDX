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

namespace SharpDX.Toolkit.Serialization
{
    /// <summary>
    /// Specify the size used for encoding length for array while using a <see cref="BinarySerializer"/>, just before an array is encoded.
    /// </summary>
    public enum ArrayLengthType
    {
        /// <summary>
        /// Use variable length 7Bit Encoding that will output from 1 byte to 5 byte depending on the range of length value.
        /// </summary>
        Dynamic = 0,

        /// <summary>
        /// Output a length as a byte. The length must not be greater than 255.
        /// </summary>
        Byte = 255,

        /// <summary>
        /// Output a length as an ushort. The length must not be greater than 65535.
        /// </summary>
        UShort = 65535,

        /// <summary>
        /// Output a length as an int. The length must not be greater than 2^31 - 1.
        /// </summary>
        Int = 0x7FFFFFF
    }
}