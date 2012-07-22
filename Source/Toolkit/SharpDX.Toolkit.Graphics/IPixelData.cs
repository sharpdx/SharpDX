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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Interface used to write to an arbitrary pixel data structure.
    /// </summary>
    public interface IPixelData
    {
        /// <summary>
        /// Gets the associated <see cref="PixelFormat"/>.
        /// </summary>
        PixelFormat Format { get; }

        /// <summary>
        /// Gets or sets the color on this pixel data as a HDR <see cref="Color4"/> (128 bit, 4 x floats) .
        /// </summary>
        Color4 Value { get; set; }

        /// <summary>
        /// Gets or sets the color on this pixel data as a LDR <see cref="Color"/> (32 bit, 1 x int) .
        /// </summary>
        Color Value32Bpp { get; set; }
    }
}