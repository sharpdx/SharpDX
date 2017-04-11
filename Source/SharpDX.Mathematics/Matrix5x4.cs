// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// Represents a 4x4 mathematical Matrix5x4.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix5x4 : IEquatable<Matrix5x4>, IFormattable
    {
        /// <summary>
        /// The size of the <see cref="Matrix5x4"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Utilities.SizeOf<Matrix5x4>();

        /// <summary>
        /// A <see cref="Matrix5x4"/> with all of its components set to zero.
        /// </summary>
        public static readonly Matrix5x4 Zero = new Matrix5x4();

        /// <summary>
        /// The identity <see cref="Matrix5x4"/>.
        /// </summary>
        public static readonly Matrix5x4 Identity = new Matrix5x4() { M11 = 1.0f, M22 = 1.0f, M33 = 1.0f, M44 = 1.0f, M54 = 0.0f };

        /// <summary>
        /// Value at row 1 column 1 of the Matrix5x4.
        /// </summary>
        public float M11;

        /// <summary>
        /// Value at row 1 column 2 of the Matrix5x4.
        /// </summary>
        public float M12;

        /// <summary>
        /// Value at row 1 column 3 of the Matrix5x4.
        /// </summary>
        public float M13;

        /// <summary>
        /// Value at row 1 column 4 of the Matrix5x4.
        /// </summary>
        public float M14;

        /// <summary>
        /// Value at row 2 column 1 of the Matrix5x4.
        /// </summary>
        public float M21;

        /// <summary>
        /// Value at row 2 column 2 of the Matrix5x4.
        /// </summary>
        public float M22;

        /// <summary>
        /// Value at row 2 column 3 of the Matrix5x4.
        /// </summary>
        public float M23;

        /// <summary>
        /// Value at row 2 column 4 of the Matrix5x4.
        /// </summary>
        public float M24;

        /// <summary>
        /// Value at row 3 column 1 of the Matrix5x4.
        /// </summary>
        public float M31;

        /// <summary>
        /// Value at row 3 column 2 of the Matrix5x4.
        /// </summary>
        public float M32;

        /// <summary>
        /// Value at row 3 column 3 of the Matrix5x4.
        /// </summary>
        public float M33;

        /// <summary>
        /// Value at row 3 column 4 of the Matrix5x4.
        /// </summary>
        public float M34;

        /// <summary>
        /// Value at row 4 column 1 of the Matrix5x4.
        /// </summary>
        public float M41;

        /// <summary>
        /// Value at row 4 column 2 of the Matrix5x4.
        /// </summary>
        public float M42;

        /// <summary>
        /// Value at row 4 column 3 of the Matrix5x4.
        /// </summary>
        public float M43;

        /// <summary>
        /// Value at row 4 column 4 of the Matrix5x4.
        /// </summary>
        public float M44;

        /// <summary>
        /// Value at row 5 column 1 of the Matrix5x4.
        /// </summary>
        public float M51;

        /// <summary>
        /// Value at row 5 column 2 of the Matrix5x4.
        /// </summary>
        public float M52;

        /// <summary>
        /// Value at row 5 column 3 of the Matrix5x4.
        /// </summary>
        public float M53;

        /// <summary>
        /// Value at row 5 column 4 of the Matrix5x4.
        /// </summary>
        public float M54;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix5x4"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Matrix5x4(float value)
        {
            M11 = M12 = M13 = M14 =
            M21 = M22 = M23 = M24 =
            M31 = M32 = M33 = M34 =
            M41 = M42 = M43 = M44 = 
            M51 = M52 = M53 = M54 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix5x4"/> struct.
        /// </summary>
        /// <param name="M11">The value to assign at row 1 column 1 of the Matrix5x4.</param>
        /// <param name="M12">The value to assign at row 1 column 2 of the Matrix5x4.</param>
        /// <param name="M13">The value to assign at row 1 column 3 of the Matrix5x4.</param>
        /// <param name="M14">The value to assign at row 1 column 4 of the Matrix5x4.</param>
        /// <param name="M21">The value to assign at row 2 column 1 of the Matrix5x4.</param>
        /// <param name="M22">The value to assign at row 2 column 2 of the Matrix5x4.</param>
        /// <param name="M23">The value to assign at row 2 column 3 of the Matrix5x4.</param>
        /// <param name="M24">The value to assign at row 2 column 4 of the Matrix5x4.</param>
        /// <param name="M31">The value to assign at row 3 column 1 of the Matrix5x4.</param>
        /// <param name="M32">The value to assign at row 3 column 2 of the Matrix5x4.</param>
        /// <param name="M33">The value to assign at row 3 column 3 of the Matrix5x4.</param>
        /// <param name="M34">The value to assign at row 3 column 4 of the Matrix5x4.</param>
        /// <param name="M41">The value to assign at row 4 column 1 of the Matrix5x4.</param>
        /// <param name="M42">The value to assign at row 4 column 2 of the Matrix5x4.</param>
        /// <param name="M43">The value to assign at row 4 column 3 of the Matrix5x4.</param>
        /// <param name="M44">The value to assign at row 4 column 4 of the Matrix5x4.</param>
        /// <param name="M51">The value to assign at row 5 column 1 of the Matrix5x4.</param>
        /// <param name="M52">The value to assign at row 5 column 2 of the Matrix5x4.</param>
        /// <param name="M53">The value to assign at row 5 column 3 of the Matrix5x4.</param>
        /// <param name="M54">The value to assign at row 5 column 4 of the Matrix5x4.</param>
        public Matrix5x4(float M11, float M12, float M13, float M14,
            float M21, float M22, float M23, float M24,
            float M31, float M32, float M33, float M34,
            float M41, float M42, float M43, float M44,
            float M51, float M52, float M53, float M54)
        {
            this.M11 = M11; this.M12 = M12; this.M13 = M13; this.M14 = M14;
            this.M21 = M21; this.M22 = M22; this.M23 = M23; this.M24 = M24;
            this.M31 = M31; this.M32 = M32; this.M33 = M33; this.M34 = M34;
            this.M41 = M41; this.M42 = M42; this.M43 = M43; this.M44 = M44;
            this.M51 = M51; this.M52 = M52; this.M53 = M53; this.M54 = M54;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix5x4"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the components of the Matrix5x4. This must be an array with sixteen elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than sixteen elements.</exception>
        public Matrix5x4(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 20)
                throw new ArgumentOutOfRangeException("values", "There must be 20 input values for Matrix5x4.");

            M11 = values[0];
            M12 = values[1];
            M13 = values[2];
            M14 = values[3];

            M21 = values[4];
            M22 = values[5];
            M23 = values[6];
            M24 = values[7];

            M31 = values[8];
            M32 = values[9];
            M33 = values[10];
            M34 = values[11];

            M41 = values[12];
            M42 = values[13];
            M43 = values[14];
            M44 = values[15];

            M51 = values[16];
            M52 = values[17];
            M53 = values[18];
            M54 = values[19];
        }

        /// <summary>
        /// Gets or sets the first row in the Matrix5x4; that is M11, M12, M13, and M14.
        /// </summary>
        public Vector4 Row1
        {
            get { return new Vector4(M11, M12, M13, M14); }
            set { M11 = value.X; M12 = value.Y; M13 = value.Z; M14 = value.W; }
        }

        /// <summary>
        /// Gets or sets the second row in the Matrix5x4; that is M21, M22, M23, and M24.
        /// </summary>
        public Vector4 Row2
        {
            get { return new Vector4(M21, M22, M23, M24); }
            set { M21 = value.X; M22 = value.Y; M23 = value.Z; M24 = value.W; }
        }

        /// <summary>
        /// Gets or sets the third row in the Matrix5x4; that is M31, M32, M33, and M34.
        /// </summary>
        public Vector4 Row3
        {
            get { return new Vector4(M31, M32, M33, M34); }
            set { M31 = value.X; M32 = value.Y; M33 = value.Z; M34 = value.W; }
        }

        /// <summary>
        /// Gets or sets the fourth row in the Matrix5x4; that is M41, M42, M43, and M44.
        /// </summary>
        public Vector4 Row4
        {
            get { return new Vector4(M41, M42, M43, M44); }
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; M44 = value.W; }
        }

        /// <summary>
        /// Gets or sets the fifth row in the Matrix5x4; that is M51, M52, M53, and M54.
        /// </summary>
        public Vector4 Row5
        {
            get { return new Vector4(M51, M52, M53, M54); }
            set { M51 = value.X; M52 = value.Y; M53 = value.Z; M54 = value.W; }
        }

        /// <summary>
        /// Gets or sets the translation of the Matrix5x4; that is M41, M42, and M43.
        /// </summary>
        public Vector4 TranslationVector
        {
            get { return new Vector4(M51, M52, M53, M54); }
            set { M51 = value.X; M52 = value.Y; M53 = value.Z; M54 = value.W; }
        }

        /// <summary>
        /// Gets or sets the scale of the Matrix5x4; that is M11, M22, and M33.
        /// </summary>
        public Vector4 ScaleVector
        {
            get { return new Vector4(M11, M22, M33, M44); }
            set { M11 = value.X; M22 = value.Y; M33 = value.Z; M44 = value.W; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an identity Matrix5x4.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an identity Matrix5x4; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity
        {
            get { return this.Equals(Identity); }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the Matrix5x4 component, depending on the index.</value>
        /// <param name="index">The zero-based index of the component to access.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 15].</exception>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return M11;
                    case 1: return M12;
                    case 2: return M13;
                    case 3: return M14;
                    case 4: return M21;
                    case 5: return M22;
                    case 6: return M23;
                    case 7: return M24;
                    case 8: return M31;
                    case 9: return M32;
                    case 10: return M33;
                    case 11: return M34;
                    case 12: return M41;
                    case 13: return M42;
                    case 14: return M43;
                    case 15: return M44;
                    case 16: return M51;
                    case 17: return M52;
                    case 18: return M53;
                    case 19: return M54;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Matrix5x4 run from 0 to 19, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M13 = value; break;
                    case 3: M14 = value; break;
                    case 4: M21 = value; break;
                    case 5: M22 = value; break;
                    case 6: M23 = value; break;
                    case 7: M24 = value; break;
                    case 8: M31 = value; break;
                    case 9: M32 = value; break;
                    case 10: M33 = value; break;
                    case 11: M34 = value; break;
                    case 12: M41 = value; break;
                    case 13: M42 = value; break;
                    case 14: M43 = value; break;
                    case 15: M44 = value; break;
                    case 16: M51 = value; break;
                    case 17: M52 = value; break;
                    case 18: M53 = value; break;
                    case 19: M54 = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Matrix5x4 run from 0 to 19, inclusive.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the Matrix5x4 component, depending on the index.</value>
        /// <param name="row">The row of the Matrix5x4 to access.</param>
        /// <param name="column">The column of the Matrix5x4 to access.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="row"/> or <paramref name="column"/>is out of the range [0, 3].</exception>
        public float this[int row, int column]
        {
            get
            {
                if (row < 0 || row > 4)
                    throw new ArgumentOutOfRangeException("row", "Rows for matrices run from 0 to 4, inclusive.");
                if (column < 0 || column > 3)
                    throw new ArgumentOutOfRangeException("column", "Columns for matrices run from 0 to 3, inclusive.");

                return this[(row * 4) + column];
            }

            set
            {
                if (row < 0 || row > 4)
                    throw new ArgumentOutOfRangeException("row", "Rows for matrices run from 0 to 4, inclusive.");
                if (column < 0 || column > 3)
                    throw new ArgumentOutOfRangeException("column", "Columns for matrices run from 0 to 3, inclusive.");

                this[(row * 4) + column] = value;
            }
        }

        /// <summary>
        /// Determines the sum of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix5x4 to add.</param>
        /// <param name="right">The second Matrix5x4 to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two matrices.</param>
        public static void Add(ref Matrix5x4 left, ref Matrix5x4 right, out Matrix5x4 result)
        {
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M14 = left.M14 + right.M14;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M24 = left.M24 + right.M24;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
            result.M34 = left.M34 + right.M34;
            result.M41 = left.M41 + right.M41;
            result.M42 = left.M42 + right.M42;
            result.M43 = left.M43 + right.M43;
            result.M44 = left.M44 + right.M44;
            result.M51 = left.M51 + right.M51;
            result.M52 = left.M52 + right.M52;
            result.M53 = left.M53 + right.M53;
            result.M54 = left.M54 + right.M54;
        }

        /// <summary>
        /// Determines the sum of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix5x4 to add.</param>
        /// <param name="right">The second Matrix5x4 to add.</param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix5x4 Add(Matrix5x4 left, Matrix5x4 right)
        {
            Matrix5x4 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Determines the difference between two matrices.
        /// </summary>
        /// <param name="left">The first Matrix5x4 to subtract.</param>
        /// <param name="right">The second Matrix5x4 to subtract.</param>
        /// <param name="result">When the method completes, contains the difference between the two matrices.</param>
        public static void Subtract(ref Matrix5x4 left, ref Matrix5x4 right, out Matrix5x4 result)
        {
            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M13 = left.M13 - right.M13;
            result.M14 = left.M14 - right.M14;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M23 = left.M23 - right.M23;
            result.M24 = left.M24 - right.M24;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
            result.M33 = left.M33 - right.M33;
            result.M34 = left.M34 - right.M34;
            result.M41 = left.M41 - right.M41;
            result.M42 = left.M42 - right.M42;
            result.M43 = left.M43 - right.M43;
            result.M44 = left.M44 - right.M44;
            result.M51 = left.M51 - right.M51;
            result.M52 = left.M52 - right.M52;
            result.M53 = left.M53 - right.M53;
            result.M54 = left.M54 - right.M54;
        }

        /// <summary>
        /// Determines the difference between two matrices.
        /// </summary>
        /// <param name="left">The first Matrix5x4 to subtract.</param>
        /// <param name="right">The second Matrix5x4 to subtract.</param>
        /// <returns>The difference between the two matrices.</returns>
        public static Matrix5x4 Subtract(Matrix5x4 left, Matrix5x4 right)
        {
            Matrix5x4 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix5x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix5x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled Matrix5x4.</param>
        public static void Multiply(ref Matrix5x4 left, float right, out Matrix5x4 result)
        {
            result.M11 = left.M11 * right;
            result.M12 = left.M12 * right;
            result.M13 = left.M13 * right;
            result.M14 = left.M14 * right;
            result.M21 = left.M21 * right;
            result.M22 = left.M22 * right;
            result.M23 = left.M23 * right;
            result.M24 = left.M24 * right;
            result.M31 = left.M31 * right;
            result.M32 = left.M32 * right;
            result.M33 = left.M33 * right;
            result.M34 = left.M34 * right;
            result.M41 = left.M41 * right;
            result.M42 = left.M42 * right;
            result.M43 = left.M43 * right;
            result.M44 = left.M44 * right;
            result.M51 = left.M51 * right;
            result.M52 = left.M52 * right;
            result.M53 = left.M53 * right;
            result.M54 = left.M54 * right;
        }

        /// <summary>
        /// Scales a Matrix5x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix5x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled Matrix5x4.</param>
        public static void Divide(ref Matrix5x4 left, float right, out Matrix5x4 result)
        {
            float inv = 1.0f / right;

            result.M11 = left.M11 * inv;
            result.M12 = left.M12 * inv;
            result.M13 = left.M13 * inv;
            result.M14 = left.M14 * inv;
            result.M21 = left.M21 * inv;
            result.M22 = left.M22 * inv;
            result.M23 = left.M23 * inv;
            result.M24 = left.M24 * inv;
            result.M31 = left.M31 * inv;
            result.M32 = left.M32 * inv;
            result.M33 = left.M33 * inv;
            result.M34 = left.M34 * inv;
            result.M41 = left.M41 * inv;
            result.M42 = left.M42 * inv;
            result.M43 = left.M43 * inv;
            result.M44 = left.M44 * inv;
            result.M51 = left.M51 * inv;
            result.M52 = left.M52 * inv;
            result.M53 = left.M53 * inv;
            result.M54 = left.M54 * inv;
        }

        /// <summary>
        /// Negates a Matrix5x4.
        /// </summary>
        /// <param name="value">The Matrix5x4 to be negated.</param>
        /// <param name="result">When the method completes, contains the negated Matrix5x4.</param>
        public static void Negate(ref Matrix5x4 value, out Matrix5x4 result)
        {
            result.M11 = -value.M11;
            result.M12 = -value.M12;
            result.M13 = -value.M13;
            result.M14 = -value.M14;
            result.M21 = -value.M21;
            result.M22 = -value.M22;
            result.M23 = -value.M23;
            result.M24 = -value.M24;
            result.M31 = -value.M31;
            result.M32 = -value.M32;
            result.M33 = -value.M33;
            result.M34 = -value.M34;
            result.M41 = -value.M41;
            result.M42 = -value.M42;
            result.M43 = -value.M43;
            result.M44 = -value.M44;
            result.M51 = -value.M51;
            result.M52 = -value.M52;
            result.M53 = -value.M53;
            result.M54 = -value.M54;
        }

        /// <summary>
        /// Negates a Matrix5x4.
        /// </summary>
        /// <param name="value">The Matrix5x4 to be negated.</param>
        /// <returns>The negated Matrix5x4.</returns>
        public static Matrix5x4 Negate(Matrix5x4 value)
        {
            Matrix5x4 result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two matrices.
        /// </summary>
        /// <param name="start">Start Matrix5x4.</param>
        /// <param name="end">End Matrix5x4.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two matrices.</param>
        /// <remarks>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static void Lerp(ref Matrix5x4 start, ref Matrix5x4 end, float amount, out Matrix5x4 result)
        {
            result.M11 = MathUtil.Lerp(start.M11, end.M11, amount);
            result.M12 = MathUtil.Lerp(start.M12, end.M12, amount);
            result.M13 = MathUtil.Lerp(start.M13, end.M13, amount);
            result.M14 = MathUtil.Lerp(start.M14, end.M14, amount);
            result.M21 = MathUtil.Lerp(start.M21, end.M21, amount);
            result.M22 = MathUtil.Lerp(start.M22, end.M22, amount);
            result.M23 = MathUtil.Lerp(start.M23, end.M23, amount);
            result.M24 = MathUtil.Lerp(start.M24, end.M24, amount);
            result.M31 = MathUtil.Lerp(start.M31, end.M31, amount);
            result.M32 = MathUtil.Lerp(start.M32, end.M32, amount);
            result.M33 = MathUtil.Lerp(start.M33, end.M33, amount);
            result.M34 = MathUtil.Lerp(start.M34, end.M34, amount);
            result.M41 = MathUtil.Lerp(start.M41, end.M41, amount);
            result.M42 = MathUtil.Lerp(start.M42, end.M42, amount);
            result.M43 = MathUtil.Lerp(start.M43, end.M43, amount);
            result.M44 = MathUtil.Lerp(start.M44, end.M44, amount);
            result.M51 = MathUtil.Lerp(start.M51, end.M51, amount);
            result.M52 = MathUtil.Lerp(start.M52, end.M52, amount);
            result.M53 = MathUtil.Lerp(start.M53, end.M53, amount);
            result.M54 = MathUtil.Lerp(start.M54, end.M54, amount);
        }

        /// <summary>
        /// Performs a linear interpolation between two matrices.
        /// </summary>
        /// <param name="start">Start Matrix5x4.</param>
        /// <param name="end">End Matrix5x4.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two matrices.</returns>
        /// <remarks>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static Matrix5x4 Lerp(Matrix5x4 start, Matrix5x4 end, float amount)
        {
            Matrix5x4 result;
            Lerp(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs a cubic interpolation between two matrices.
        /// </summary>
        /// <param name="start">Start Matrix5x4.</param>
        /// <param name="end">End Matrix5x4.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the cubic interpolation of the two matrices.</param>
        public static void SmoothStep(ref Matrix5x4 start, ref Matrix5x4 end, float amount, out Matrix5x4 result)
        {
            amount = MathUtil.SmoothStep(amount);
            Lerp(ref start, ref end, amount, out result);
        }

        /// <summary>
        /// Performs a cubic interpolation between two matrices.
        /// </summary>
        /// <param name="start">Start Matrix5x4.</param>
        /// <param name="end">End Matrix5x4.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The cubic interpolation of the two matrices.</returns>
        public static Matrix5x4 SmoothStep(Matrix5x4 start, Matrix5x4 end, float amount)
        {
            Matrix5x4 result;
            SmoothStep(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Creates a Matrix5x4 that scales along the x-axis, y-axis, y-axis and w-axis
        /// </summary>
        /// <param name="scale">Scaling factor for all three axes.</param>
        /// <param name="result">When the method completes, contains the created scaling Matrix5x4.</param>
        public static void Scaling(ref Vector4 scale, out Matrix5x4 result)
        {
            Scaling(scale.X, scale.Y, scale.Z, scale.W, out result);
        }

        /// <summary>
        /// Creates a Matrix5x4 that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="scale">Scaling factor for all three axes.</param>
        /// <returns>The created scaling Matrix5x4.</returns>
        public static Matrix5x4 Scaling(Vector4 scale)
        {
            Matrix5x4 result;
            Scaling(ref scale, out result);
            return result;
        }

        /// <summary>
        /// Creates a Matrix5x4 that scales along the x-axis, y-axis, z-axis and w-axis.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <param name="z">Scaling factor that is applied along the z-axis.</param>
        /// <param name="w">Scaling factor that is applied along the w-axis.</param>
        /// <param name="result">When the method completes, contains the created scaling Matrix5x4.</param>
        public static void Scaling(float x, float y, float z, float w, out Matrix5x4 result)
        {
            result = Matrix5x4.Identity;
            result.M11 = x;
            result.M22 = y;
            result.M33 = z;
            result.M44 = w;
        }

        /// <summary>
        /// Creates a Matrix5x4 that scales along the x-axis, y-axis, z-axis and w-axis.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <param name="z">Scaling factor that is applied along the z-axis.</param>
        /// <param name="w">Scaling factor that is applied along the w-axis.</param>
        /// <returns>The created scaling Matrix5x4.</returns>
        public static Matrix5x4 Scaling(float x, float y, float z, float w)
        {
            Matrix5x4 result;
            Scaling(x, y, z, w, out result);
            return result;
        }

        /// <summary>
        /// Creates a Matrix5x4 that uniformly scales along all three axis.
        /// </summary>
        /// <param name="scale">The uniform scale that is applied along all axis.</param>
        /// <param name="result">When the method completes, contains the created scaling Matrix5x4.</param>
        public static void Scaling(float scale, out Matrix5x4 result)
        {
            result = Matrix5x4.Identity;
            result.M11 = result.M22 = result.M33 = result.M44 = scale;
        }

        /// <summary>
        /// Creates a Matrix5x4 that uniformly scales along all three axis.
        /// </summary>
        /// <param name="scale">The uniform scale that is applied along all axis.</param>
        /// <returns>The created scaling Matrix5x4.</returns>
        public static Matrix5x4 Scaling(float scale)
        {
            Matrix5x4 result;
            Scaling(scale, out result);
            return result;
        }

        /// <summary>
        /// Creates a translation Matrix5x4 using the specified offsets.
        /// </summary>
        /// <param name="value">The offset for all three coordinate planes.</param>
        /// <param name="result">When the method completes, contains the created translation Matrix5x4.</param>
        public static void Translation(ref Vector4 value, out Matrix5x4 result)
        {
            Translation(value.X, value.Y, value.Z, value.W, out result);
        }

        /// <summary>
        /// Creates a translation Matrix5x4 using the specified offsets.
        /// </summary>
        /// <param name="value">The offset for all three coordinate planes.</param>
        /// <returns>The created translation Matrix5x4.</returns>
        public static Matrix5x4 Translation(Vector4 value)
        {
            Matrix5x4 result;
            Translation(ref value, out result);
            return result;
        }

        /// <summary>
        /// Creates a translation Matrix5x4 using the specified offsets.
        /// </summary>
        /// <param name="x">X-coordinate offset.</param>
        /// <param name="y">Y-coordinate offset.</param>
        /// <param name="z">Z-coordinate offset.</param>
        /// <param name="w">W-coordinate offset.</param>
        /// <param name="result">When the method completes, contains the created translation Matrix5x4.</param>
        public static void Translation(float x, float y, float z, float w,  out Matrix5x4 result)
        {
            result = Matrix5x4.Identity;
            result.M51 = x;
            result.M52 = y;
            result.M53 = z;
            result.M54 = w;
        }

        /// <summary>
        /// Creates a translation Matrix5x4 using the specified offsets.
        /// </summary>
        /// <param name="x">X-coordinate offset.</param>
        /// <param name="y">Y-coordinate offset.</param>
        /// <param name="z">Z-coordinate offset.</param>
        /// <param name="w">W-coordinate offset.</param>
        /// <returns>The created translation Matrix5x4.</returns>
        public static Matrix5x4 Translation(float x, float y, float z, float w)
        {
            Matrix5x4 result;
            Translation(x, y, z, w, out result);
            return result;
        }

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="left">The first Matrix5x4 to add.</param>
        /// <param name="right">The second Matrix5x4 to add.</param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix5x4 operator +(Matrix5x4 left, Matrix5x4 right)
        {
            Matrix5x4 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Assert a Matrix5x4 (return it unchanged).
        /// </summary>
        /// <param name="value">The Matrix5x4 to assert (unchanged).</param>
        /// <returns>The asserted (unchanged) Matrix5x4.</returns>
        public static Matrix5x4 operator +(Matrix5x4 value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="left">The first Matrix5x4 to subtract.</param>
        /// <param name="right">The second Matrix5x4 to subtract.</param>
        /// <returns>The difference between the two matrices.</returns>
        public static Matrix5x4 operator -(Matrix5x4 left, Matrix5x4 right)
        {
            Matrix5x4 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Negates a Matrix5x4.
        /// </summary>
        /// <param name="value">The Matrix5x4 to negate.</param>
        /// <returns>The negated Matrix5x4.</returns>
        public static Matrix5x4 operator -(Matrix5x4 value)
        {
            Matrix5x4 result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix5x4 by a given value.
        /// </summary>
        /// <param name="right">The Matrix5x4 to scale.</param>
        /// <param name="left">The amount by which to scale.</param>
        /// <returns>The scaled Matrix5x4.</returns>
        public static Matrix5x4 operator *(float left, Matrix5x4 right)
        {
            Matrix5x4 result;
            Multiply(ref right, left, out result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix5x4 by a given value.
        /// </summary>
        /// <param name="left">The Matrix5x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled Matrix5x4.</returns>
        public static Matrix5x4 operator *(Matrix5x4 left, float right)
        {
            Matrix5x4 result;
            Multiply(ref left, right, out result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix5x4 by a given value.
        /// </summary>
        /// <param name="left">The Matrix5x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled Matrix5x4.</returns>
        public static Matrix5x4 operator /(Matrix5x4 left, float right)
        {
            Matrix5x4 result;
            Divide(ref left, right, out result);
            return result;
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        [MethodImpl((MethodImplOptions)0x100)] // MethodImplOptions.AggressiveInlining
        public static bool operator ==(Matrix5x4 left, Matrix5x4 right)
        {
            return left.Equals(ref right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        [MethodImpl((MethodImplOptions)0x100)] // MethodImplOptions.AggressiveInlining
        public static bool operator !=(Matrix5x4 left, Matrix5x4 right)
        {
            return !left.Equals(ref right);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M3:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}] [M51:{16} M52:{17} M53:{18} M54:{19}]",
                M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44, M51, M52, M53, M54);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(format, CultureInfo.CurrentCulture, "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M3:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}] [M51:{16} M52:{17} M53:{18} M54:{19}]",
                M11.ToString(format, CultureInfo.CurrentCulture), M12.ToString(format, CultureInfo.CurrentCulture), M13.ToString(format, CultureInfo.CurrentCulture), M14.ToString(format, CultureInfo.CurrentCulture),
                M21.ToString(format, CultureInfo.CurrentCulture), M22.ToString(format, CultureInfo.CurrentCulture), M23.ToString(format, CultureInfo.CurrentCulture), M24.ToString(format, CultureInfo.CurrentCulture),
                M31.ToString(format, CultureInfo.CurrentCulture), M32.ToString(format, CultureInfo.CurrentCulture), M33.ToString(format, CultureInfo.CurrentCulture), M34.ToString(format, CultureInfo.CurrentCulture),
                M41.ToString(format, CultureInfo.CurrentCulture), M42.ToString(format, CultureInfo.CurrentCulture), M43.ToString(format, CultureInfo.CurrentCulture), M44.ToString(format, CultureInfo.CurrentCulture),
                M51.ToString(format, CultureInfo.CurrentCulture), M52.ToString(format, CultureInfo.CurrentCulture), M53.ToString(format, CultureInfo.CurrentCulture), M54.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M3:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}] [M51:{16} M52:{17} M53:{18} M54:{19}]",
                M11.ToString(formatProvider), M12.ToString(formatProvider), M13.ToString(formatProvider), M14.ToString(formatProvider),
                M21.ToString(formatProvider), M22.ToString(formatProvider), M23.ToString(formatProvider), M24.ToString(formatProvider),
                M31.ToString(formatProvider), M32.ToString(formatProvider), M33.ToString(formatProvider), M34.ToString(formatProvider),
                M41.ToString(formatProvider), M42.ToString(formatProvider), M43.ToString(formatProvider), M44.ToString(formatProvider),
                M51.ToString(formatProvider), M52.ToString(formatProvider), M53.ToString(formatProvider), M54.ToString(formatProvider));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                return ToString(formatProvider);

            return string.Format(format, formatProvider, "[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M3:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}] [M51:{16} M52:{17} M53:{18} M54:{19}]",
                M11.ToString(format, formatProvider), M12.ToString(format, formatProvider), M13.ToString(format, formatProvider), M14.ToString(format, formatProvider),
                M21.ToString(format, formatProvider), M22.ToString(format, formatProvider), M23.ToString(format, formatProvider), M24.ToString(format, formatProvider),
                M31.ToString(format, formatProvider), M32.ToString(format, formatProvider), M33.ToString(format, formatProvider), M34.ToString(format, formatProvider),
                M41.ToString(format, formatProvider), M42.ToString(format, formatProvider), M43.ToString(format, formatProvider), M44.ToString(format, formatProvider),
                M51.ToString(format, formatProvider), M52.ToString(format, formatProvider), M53.ToString(format, formatProvider), M54.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = M11.GetHashCode();
                hashCode = (hashCode * 397) ^ M12.GetHashCode();
                hashCode = (hashCode * 397) ^ M13.GetHashCode();
                hashCode = (hashCode * 397) ^ M14.GetHashCode();
                hashCode = (hashCode * 397) ^ M21.GetHashCode();
                hashCode = (hashCode * 397) ^ M22.GetHashCode();
                hashCode = (hashCode * 397) ^ M23.GetHashCode();
                hashCode = (hashCode * 397) ^ M24.GetHashCode();
                hashCode = (hashCode * 397) ^ M31.GetHashCode();
                hashCode = (hashCode * 397) ^ M32.GetHashCode();
                hashCode = (hashCode * 397) ^ M33.GetHashCode();
                hashCode = (hashCode * 397) ^ M34.GetHashCode();
                hashCode = (hashCode * 397) ^ M41.GetHashCode();
                hashCode = (hashCode * 397) ^ M42.GetHashCode();
                hashCode = (hashCode * 397) ^ M43.GetHashCode();
                hashCode = (hashCode * 397) ^ M44.GetHashCode();
                hashCode = (hashCode * 397) ^ M51.GetHashCode();
                hashCode = (hashCode * 397) ^ M52.GetHashCode();
                hashCode = (hashCode * 397) ^ M53.GetHashCode();
                hashCode = (hashCode * 397) ^ M54.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="Matrix5x4"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Matrix5x4"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Matrix5x4"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref Matrix5x4 other)
        {
            return (MathUtil.NearEqual(other.M11, M11) &&
                MathUtil.NearEqual(other.M12, M12) &&
                MathUtil.NearEqual(other.M13, M13) &&
                MathUtil.NearEqual(other.M14, M14) &&
                MathUtil.NearEqual(other.M21, M21) &&
                MathUtil.NearEqual(other.M22, M22) &&
                MathUtil.NearEqual(other.M23, M23) &&
                MathUtil.NearEqual(other.M24, M24) &&
                MathUtil.NearEqual(other.M31, M31) &&
                MathUtil.NearEqual(other.M32, M32) &&
                MathUtil.NearEqual(other.M33, M33) &&
                MathUtil.NearEqual(other.M34, M34) &&
                MathUtil.NearEqual(other.M41, M41) &&
                MathUtil.NearEqual(other.M42, M42) &&
                MathUtil.NearEqual(other.M43, M43) &&
                MathUtil.NearEqual(other.M44, M44) &&
                MathUtil.NearEqual(other.M51, M51) &&
                MathUtil.NearEqual(other.M52, M52) &&
                MathUtil.NearEqual(other.M53, M53) &&
                MathUtil.NearEqual(other.M54, M54));
        }
        /// <summary>
        /// Determines whether the specified <see cref="Matrix5x4"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Matrix5x4"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Matrix5x4"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl((MethodImplOptions)0x100)] // MethodImplOptions.AggressiveInlining
        public bool Equals(Matrix5x4 other)
        {
            return Equals(ref other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (!(value is Matrix5x4))
                return false;

            var strongValue = (Matrix5x4)value;
            return Equals(ref strongValue);
        }
    }
}
