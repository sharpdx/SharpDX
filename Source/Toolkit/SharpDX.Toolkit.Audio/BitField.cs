// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Helper class that provides methods to manipulate bit fields.
    /// </summary>
    internal static class BitField
    {
        /// <summary>
        /// Gets the bit values from the provided source at the 
        /// </summary>
        /// <param name="source">The source from where to read the bits.</param>
        /// <param name="length">The lenghts of the range to read.</param>
        /// <param name="offset">The offset of the bits to read.</param>
        /// <returns>The read bits.</returns>
        public static uint Get(uint source, int length, int offset)
        {
            var mask = uint.MaxValue >> (sizeof(uint) * 8 - length);

            return (source >> offset) & mask;
        }

        /// <summary>
        /// Fills the provided destination with the bits read from the specified source.
        /// </summary>
        /// <param name="source">The bits source.</param>
        /// <param name="length">The leght of the bit range.</param>
        /// <param name="offset">The offset of the bit range.</param>
        /// <param name="destination">The destination to write the bits.</param>
        public static void Set(uint source, int length, int offset, ref uint destination)
        {
            var mask = uint.MaxValue >> (sizeof(uint) * 8 - length);

            destination |= (source & mask) << offset;
        }
    }
}