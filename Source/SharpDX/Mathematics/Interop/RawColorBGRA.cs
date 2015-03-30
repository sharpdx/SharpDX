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

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharpDX.Mathematics.Interop
{
    /// <summary>
    /// Interop type for a ColorBGRA (BGRA, 4 bytes).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    [DebuggerDisplay("R:{R} G:{G} B:{B} A:{A}")]
    public struct RawColorBGRA
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawColorBGRA"/> struct.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="g">The g.</param>
        /// <param name="r">The r.</param>
        /// <param name="a">A.</param>
        public RawColorBGRA(byte b, byte g, byte r, byte a)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }

        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public byte B;

        /// <summary>
        /// The green component of the color.
        /// </summary>
        public byte G;

        /// <summary>
        /// The red component of the color.
        /// </summary>
        public byte R;

        /// <summary>
        /// The alpha component of the color.
        /// </summary>
        public byte A;
    }
}
