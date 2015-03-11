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

namespace SharpDX.Mathematics.Interop
{
    /// <summary>
    /// Interop type for a float5x4 (20 floats).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RawMatrix5x4
    {
        /// <summary>
        /// Value at row 1 column 1.
        /// </summary>
        public float M11;

        /// <summary>
        /// Value at row 1 column 2.
        /// </summary>
        public float M12;

        /// <summary>
        /// Value at row 1 column 3.
        /// </summary>
        public float M13;

        /// <summary>
        /// Value at row 1 column 4.
        /// </summary>
        public float M14;

        /// <summary>
        /// Value at row 2 column 1.
        /// </summary>
        public float M21;

        /// <summary>
        /// Value at row 2 column 2.
        /// </summary>
        public float M22;

        /// <summary>
        /// Value at row 2 column 3.
        /// </summary>
        public float M23;

        /// <summary>
        /// Value at row 2 column 4.
        /// </summary>
        public float M24;

        /// <summary>
        /// Value at row 3 column 1.
        /// </summary>
        public float M31;

        /// <summary>
        /// Value at row 3 column 2.
        /// </summary>
        public float M32;

        /// <summary>
        /// Value at row 3 column 3.
        /// </summary>
        public float M33;

        /// <summary>
        /// Value at row 3 column 4.
        /// </summary>
        public float M34;

        /// <summary>
        /// Value at row 4 column 1.
        /// </summary>
        public float M41;

        /// <summary>
        /// Value at row 4 column 2.
        /// </summary>
        public float M42;

        /// <summary>
        /// Value at row 4 column 3.
        /// </summary>
        public float M43;

        /// <summary>
        /// Value at row 4 column 4.
        /// </summary>
        public float M44;

        /// <summary>
        /// Value at row 5 column 1.
        /// </summary>
        public float M51;

        /// <summary>
        /// Value at row 5 column 2.
        /// </summary>
        public float M52;

        /// <summary>
        /// Value at row 5 column 3.
        /// </summary>
        public float M53;

        /// <summary>
        /// Value at row 5 column 4.
        /// </summary>
        public float M54;
    }
}