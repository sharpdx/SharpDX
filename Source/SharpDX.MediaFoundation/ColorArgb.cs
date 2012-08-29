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

#if DIRECTX11_1
namespace SharpDX.MediaFoundation
{
    public partial struct ColorBgra
    {
        /// <summary>
        /// Creates a new instance of <see cref="ColorBgra"/>.
        /// </summary>
        /// <param name="color"></param>
        public ColorBgra(Color4 color)
        {
            // Don't know why, but the colors are mapped differently????
            // Green => Red
            // Blue => Green
            // Red => Blue
            // Alpha => Alpha
            color.ToBgra(out Green, out Blue, out Red, out Alpha);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Color4"/> to <see cref="ColorBgra"/>.
        /// </summary>
        /// <param name="from">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ColorBgra(Color4 from)
        {
            return new ColorBgra(from);
        }
    }
}
#endif