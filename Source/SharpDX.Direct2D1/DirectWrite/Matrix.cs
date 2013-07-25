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

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// DirectWrite Matrix. Supports implicit cast to/from <see cref="SharpDX.Matrix3x2"/>.
    /// </summary>
    public partial struct Matrix
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="DirectWrite.Matrix"/> to <see cref="SharpDX.Matrix3x2"/>.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SharpDX.Matrix3x2(Matrix matrix)
        {
            return new Matrix3x2()
            {
                M11 = matrix.M11,
                M12 = matrix.M12,
                M21 = matrix.M21,
                M22 = matrix.M22,
                M31 = matrix.Dx,
                M32 = matrix.Dy
            };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Matrix3x2"/> to <see cref="DirectWrite.Matrix"/>.
        /// </summary>
        /// <param name="matrix3x2">The matrix3x2.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Matrix(SharpDX.Matrix3x2 matrix3x2)
        {
            return new Matrix()
            {
                M11 = matrix3x2.M11,
                M12 = matrix3x2.M12,
                M21 = matrix3x2.M21,
                M22 = matrix3x2.M22,
                Dx = matrix3x2.M31,
                Dy = matrix3x2.M32
            };
        }
    }
}
