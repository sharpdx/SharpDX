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
// THE SOFTWARE.using System;

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using SharpDX.Serialization;

namespace SharpDX
{
    /// <summary>
    /// Direct2D Matrix3x2 3x2. Use <see cref="SharpDX.Matrix3x2"/> and implicit cast to <see cref="Matrix3x2"/>.
    /// </summary>
#if !W8CORE
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix3x2 : IDataSerializable
    {
        /// <summary>
        /// Gets the identity matrix.
        /// </summary>
        /// <value>The identity matrix.</value>
        public readonly static Matrix3x2 Identity = new Matrix3x2(1, 0, 0, 1, 0, 0);

        /// <summary>
        /// Element (1,1)
        /// </summary>
        public float M11;

        /// <summary>
        /// Element (1,2)
        /// </summary>
        public float M12;

        /// <summary>
        /// Element (2,1)
        /// </summary>
        public float M21;

        /// <summary>
        /// Element (2,2)
        /// </summary>
        public float M22;

        /// <summary>
        /// Element (3,1)
        /// </summary>
        public float M31;

        /// <summary>
        /// Element (3,2)
        /// </summary>
        public float M32;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Matrix3x2"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Matrix3x2(float value)
        {
            M11 = M12 = 
            M21 = M22 = 
            M31 = M32 = value; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Matrix3x2"/> struct.
        /// </summary>
        /// <param name="M11">The value to assign at row 1 column 1 of the matrix.</param>
        /// <param name="M12">The value to assign at row 1 column 2 of the matrix.</param>
        /// <param name="M21">The value to assign at row 2 column 1 of the matrix.</param>
        /// <param name="M22">The value to assign at row 2 column 2 of the matrix.</param>
        /// <param name="M31">The value to assign at row 3 column 1 of the matrix.</param>
        /// <param name="M32">The value to assign at row 3 column 2 of the matrix.</param>
        public Matrix3x2(float M11, float M12, float M21, float M22, float M31, float M32)
        {
            this.M11 = M11; this.M12 = M12;
            this.M21 = M21; this.M22 = M22;
            this.M31 = M31; this.M32 = M32;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Matrix3x2"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the components of the matrix. This must be an array with sixteen elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than sixteen elements.</exception>
        public Matrix3x2(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 6)
                throw new ArgumentOutOfRangeException("values", "There must be only input values for Matrix3x2.");

            M11 = values[0];
            M12 = values[1];

            M21 = values[2];
            M22 = values[3];

            M31 = values[4];
            M32 = values[5];
        }

        /// <summary>
        /// Gets or sets the first row in the matrix; that is M11, M12.
        /// </summary>
        public Vector2 Row1
        {
            get { return new Vector2(M11, M12); }
            set { M11 = value.X; M12 = value.Y;  }
        }

        /// <summary>
        /// Gets or sets the second row in the matrix; that is M21, M22.
        /// </summary>
        public Vector2 Row2
        {
            get { return new Vector2(M21, M22); }
            set { M21 = value.X; M22 = value.Y; }
        }

        /// <summary>
        /// Gets or sets the third row in the matrix; that is M31, M32.
        /// </summary>
        public Vector2 Row3
        {
            get { return new Vector2(M31, M32); }
            set { M31 = value.X; M32 = value.Y; }
        }

        /// <summary>
        /// Gets or sets the first column in the matrix; that is M11, M21, M31.
        /// </summary>
        public Vector3 Column1
        {
            get { return new Vector3(M11, M21, M31); }
            set { M11 = value.X; M21 = value.Y; M31 = value.Z; }
        }

        /// <summary>
        /// Gets or sets the second column in the matrix; that is M12, M22, M32, and M42.
        /// </summary>
        public Vector3 Column2
        {
            get { return new Vector3(M12, M22, M32); }
            set { M12 = value.X; M22 = value.Y; M32 = value.Z;}
        }

        /// <summary>
        /// Gets or sets the translation of the matrix; that is M41, M42.
        /// </summary>
        public Vector2 TranslationVector
        {
            get { return new Vector2(M31, M32); }
            set { M31 = value.X; M32 = value.Y;  }
        }

        /// <summary>
        /// Gets or sets the scale of the matrix; that is M11, M22.
        /// </summary>
        public Vector2 ScaleVector
        {
            get { return new Vector2(M11, M22); }
            set { M11 = value.X; M22 = value.Y; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an identity matrix.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an identity matrix; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity
        {
            get { return this.Equals(Identity); }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the matrix component, depending on the index.</value>
        /// <param name="index">The zero-based index of the component to access.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 5].</exception>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return M11;
                    case 1:  return M12;
                    case 2:  return M21;
                    case 3:  return M22;
                    case 4:  return M31;
                    case 5:  return M32;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Matrix3x2 run from 0 to 5, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M21 = value; break;
                    case 3: M22 = value; break;
                    case 4: M31 = value; break;
                    case 5: M32 = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Matrix3x2 run from 0 to 5, inclusive.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the matrix component, depending on the index.</value>
        /// <param name="row">The row of the matrix to access.</param>
        /// <param name="column">The column of the matrix to access.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="row"/> or <paramref name="column"/>is out of the range [0, 3].</exception>
        public float this[int row, int column]
        {
            get
            {
                if (row < 0 || row > 2)
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 2, inclusive.");
                if (column < 0 || column > 1)
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 1, inclusive.");

                return this[(row * 2) + column];
            }

            set
            {
                if (row < 0 || row > 2)
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 2, inclusive.");
                if (column < 0 || column > 1)
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 1, inclusive.");

                this[(row * 2) + column] = value;
            }
        }

        /// <summary>
        /// Creates an array containing the elements of the matrix.
        /// </summary>
        /// <returns>A sixteen-element array containing the components of the matrix.</returns>
        public float[] ToArray()
        {
            return new[] { M11, M12, M21, M22, M31, M32 };
        }

        /// <summary>
        /// Determines the sum of two matrices.
        /// </summary>
        /// <param name="left">The first matrix to add.</param>
        /// <param name="right">The second matrix to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two matrices.</param>
        public static void Add(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
        {
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
        }

        /// <summary>
        /// Determines the sum of two matrices.
        /// </summary>
        /// <param name="left">The first matrix to add.</param>
        /// <param name="right">The second matrix to add.</param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix3x2 Add(Matrix3x2 left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Determines the difference between two matrices.
        /// </summary>
        /// <param name="left">The first matrix to subtract.</param>
        /// <param name="right">The second matrix to subtract.</param>
        /// <param name="result">When the method completes, contains the difference between the two matrices.</param>
        public static void Subtract(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
        {
            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
        }

        /// <summary>
        /// Determines the difference between two matrices.
        /// </summary>
        /// <param name="left">The first matrix to subtract.</param>
        /// <param name="right">The second matrix to subtract.</param>
        /// <returns>The difference between the two matrices.</returns>
        public static Matrix3x2 Subtract(Matrix3x2 left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a matrix by the given value.
        /// </summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled matrix.</param>
        public static void Multiply(ref Matrix3x2 left, float right, out Matrix3x2 result)
        {
            result.M11 = left.M11 * right;
            result.M12 = left.M12 * right;
            result.M21 = left.M21 * right;
            result.M22 = left.M22 * right;
            result.M31 = left.M31 * right;
            result.M32 = left.M32 * right;
        }

        /// <summary>
        /// Scales a matrix by the given value.
        /// </summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled matrix.</returns>
        public static Matrix3x2 Multiply(Matrix3x2 left, float right)
        {
            Matrix3x2 result;
            Multiply(ref left, right, out result);
            return result;
        }

        /// <summary>
        /// Determines the product of two matrices.
        /// </summary>
        /// <param name="left">The first matrix to multiply.</param>
        /// <param name="right">The second matrix to multiply.</param>
        /// <param name="result">The product of the two matrices.</param>
        public static void Multiply(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
        {
            result = new Matrix3x2();
            result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21);
            result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22);
            result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21);
            result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22);
            result.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + right.M31;
            result.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + right.M32;
        }

        /// <summary>
        /// Determines the product of two matrices.
        /// </summary>
        /// <param name="left">The first matrix to multiply.</param>
        /// <param name="right">The second matrix to multiply.</param>
        /// <returns>The product of the two matrices.</returns>
        public static Matrix3x2 Multiply(Matrix3x2 left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a matrix by the given value.
        /// </summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled matrix.</param>
        public static void Divide(ref Matrix3x2 left, float right, out Matrix3x2 result)
        {
            float inv = 1.0f / right;

            result.M11 = left.M11 * inv;
            result.M12 = left.M12 * inv;
            result.M21 = left.M21 * inv;
            result.M22 = left.M22 * inv;
            result.M31 = left.M31 * inv;
            result.M32 = left.M32 * inv;
        }

        /// <summary>
        /// Determines the quotient of two matrices.
        /// </summary>
        /// <param name="left">The first matrix to divide.</param>
        /// <param name="right">The second matrix to divide.</param>
        /// <param name="result">When the method completes, contains the quotient of the two matrices.</param>
        public static void Divide(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
        {
            result.M11 = left.M11 / right.M11;
            result.M12 = left.M12 / right.M12;
            result.M21 = left.M21 / right.M21;
            result.M22 = left.M22 / right.M22;
            result.M31 = left.M31 / right.M31;
            result.M32 = left.M32 / right.M32;
        }

        /// <summary>
        /// Negates a matrix.
        /// </summary>
        /// <param name="value">The matrix to be negated.</param>
        /// <param name="result">When the method completes, contains the negated matrix.</param>
        public static void Negate(ref Matrix3x2 value, out Matrix3x2 result)
        {
            result.M11 = -value.M11;
            result.M12 = -value.M12;
            result.M21 = -value.M21;
            result.M22 = -value.M22;
            result.M31 = -value.M31;
            result.M32 = -value.M32;
        }

        /// <summary>
        /// Negates a matrix.
        /// </summary>
        /// <param name="value">The matrix to be negated.</param>
        /// <returns>The negated matrix.</returns>
        public static Matrix3x2 Negate(Matrix3x2 value)
        {
            Matrix3x2 result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Creates a matrix that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="scale">Scaling factor for all three axes.</param>
        /// <param name="result">When the method completes, contains the created scaling matrix.</param>
        public static void Scaling(ref Vector2 scale, out Matrix3x2 result)
        {
            Scaling(scale.X, scale.Y, out result);
        }

        /// <summary>
        /// Creates a matrix that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="scale">Scaling factor for all three axes.</param>
        /// <returns>The created scaling matrix.</returns>
        public static Matrix3x2 Scaling(Vector2 scale)
        {
            Matrix3x2 result;
            Scaling(ref scale, out result);
            return result;
        }

        /// <summary>
        /// Creates a matrix that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <param name="result">When the method completes, contains the created scaling matrix.</param>
        public static void Scaling(float x, float y, out Matrix3x2 result)
        {
            result = Matrix3x2.Identity;
            result.M11 = x;
            result.M22 = y;
        }

        /// <summary>
        /// Creates a matrix that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <returns>The created scaling matrix.</returns>
        public static Matrix3x2 Scaling(float x, float y)
        {
            Matrix3x2 result;
            Scaling(x, y, out result);
            return result;
        }

        /// <summary>
        /// Creates a matrix that uniformly scales along all three axis.
        /// </summary>
        /// <param name="scale">The uniform scale that is applied along all axis.</param>
        /// <param name="result">When the method completes, contains the created scaling matrix.</param>
        public static void Scaling(float scale, out Matrix3x2 result)
        {
            result = Matrix3x2.Identity;
            result.M11 = result.M22 = scale;
        }

        /// <summary>
        /// Creates a matrix that uniformly scales along all three axis.
        /// </summary>
        /// <param name="scale">The uniform scale that is applied along all axis.</param>
        /// <returns>The created scaling matrix.</returns>
        public static Matrix3x2 Scaling(float scale)
        {
            Matrix3x2 result;
            Scaling(scale, out result);
            return result;
        }

        /// <summary>
        /// Creates a matrix that is scaling from a specified center.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <param name="center">The center of the scaling.</param>
        /// <returns>The created scaling matrix.</returns>
        public static Matrix3x2 Scaling( float x, float y, Vector2 center )
        {
            Matrix3x2 result;

            result.M11 = x;     result.M12 = 0.0f;
            result.M21 = 0.0f;  result.M22 = y;

            result.M31 = center.X - (x * center.X);
            result.M32 = center.Y - (y * center.Y);

            return result;
        }

        /// <summary>
        /// Creates a matrix that is scaling from a specified center.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <param name="center">The center of the scaling.</param>
        /// <param name="result">The created scaling matrix.</param>
        public static void Scaling( float x, float y, ref Vector2 center, out Matrix3x2 result)
        {
            Matrix3x2 localResult;

            localResult.M11 = x;     localResult.M12 = 0.0f;
            localResult.M21 = 0.0f;  localResult.M22 = y;

            localResult.M31 = center.X - (x * center.X);
            localResult.M32 = center.Y - (y * center.Y);

            result = localResult;
        }

        /// <summary>
        /// Calculates the determinant of this matrix.
        /// </summary>
        /// <returns>Result of the determinant.</returns>
        public float Determinant()
        {
                return (M11 * M22) - (M12 * M21);
        }

        /// <summary>
        /// Creates a matrix that rotates.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis.</param>
        /// <param name="result">When the method completes, contains the created rotation matrix.</param>
        public static void Rotation(float angle, out Matrix3x2 result)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            result = Matrix3x2.Identity;
            result.M11 = cos;
            result.M12 = sin;
            result.M21 = -sin;
            result.M22 = cos;
        }

        /// <summary>
        /// Creates a matrix that rotates.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis.</param>
        /// <returns>The created rotation matrix.</returns>
        public static Matrix3x2 Rotation(float angle)
        {
            Matrix3x2 result;
            Rotation(angle, out result);
            return result;
        }

        /// <summary>
        /// Creates a translation matrix using the specified offsets.
        /// </summary>
        /// <param name="value">The offset for all three coordinate planes.</param>
        /// <param name="result">When the method completes, contains the created translation matrix.</param>
        public static void Translation(ref Vector2 value, out Matrix3x2 result)
        {
            Translation(value.X, value.Y, out result);
        }

        /// <summary>
        /// Creates a translation matrix using the specified offsets.
        /// </summary>
        /// <param name="value">The offset for all three coordinate planes.</param>
        /// <returns>The created translation matrix.</returns>
        public static Matrix3x2 Translation(Vector2 value)
        {
            Matrix3x2 result;
            Translation(ref value, out result);
            return result;
        }

        /// <summary>
        /// Creates a translation matrix using the specified offsets.
        /// </summary>
        /// <param name="x">X-coordinate offset.</param>
        /// <param name="y">Y-coordinate offset.</param>
        /// <param name="z">Z-coordinate offset.</param>
        /// <param name="result">When the method completes, contains the created translation matrix.</param>
        public static void Translation(float x, float y, out Matrix3x2 result)
        {
            result = Matrix3x2.Identity;
            result.M31 = x;
            result.M32 = y;
        }

        /// <summary>
        /// Creates a translation matrix using the specified offsets.
        /// </summary>
        /// <param name="x">X-coordinate offset.</param>
        /// <param name="y">Y-coordinate offset.</param>
        /// <param name="z">Z-coordinate offset.</param>
        /// <returns>The created translation matrix.</returns>
        public static Matrix3x2 Translation(float x, float y)
        {
            Matrix3x2 result;
            Translation(x, y, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by this matrix.
        /// </summary>
        /// <param name="matrix">The matrix to use as a transformation matrix.</param>
        /// <param name="point">The original vector to apply the transformation.</param>
        /// <returns>The result of the transformation for the input vector.</returns>
        public static Vector2 TransformPoint(Matrix3x2 matrix, Vector2 point)
        {
            Vector2 result;
            result.X = (point.X * matrix.M11) + (point.Y * matrix.M21) + matrix.M31;
            result.Y = (point.X * matrix.M12) + (point.Y * matrix.M22) + matrix.M32;
            return result;
        }

        /// <summary>
        /// Transforms a vector by this matrix.
        /// </summary>
        /// <param name="matrix">The matrix to use as a transformation matrix.</param>
        /// <param name="point">The original vector to apply the transformation.</param>
        /// <param name="result">The result of the transformation for the input vector.</param>
        /// <returns></returns>
        public static void TransformPoint(ref Matrix3x2 matrix, ref Vector2 point, out Vector2 result)
        {
            Vector2 localResult;
            localResult.X = (point.X * matrix.M11) + (point.Y * matrix.M21) + matrix.M31;
            localResult.Y = (point.X * matrix.M12) + (point.Y * matrix.M22) + matrix.M32;
            result = localResult;
        }

        /// <summary>
        /// Calculates the inverse of this matrix instance.
        /// </summary>
        public void Invert()
        {
            Invert(ref this, out this);
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="value">The matrix whose inverse is to be calculated.</param>
        /// <returns>the inverse of the specified matrix.</returns>
        public static Matrix3x2 Invert(Matrix3x2 value)
        {
            Matrix3x2 result;
            Invert(ref value, out result);
            return result;
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="value">The matrix whose inverse is to be calculated.</param>
        /// <param name="result">When the method completes, contains the inverse of the specified matrix.</param>
        public static void Invert(ref Matrix3x2 value, out Matrix3x2 result)
        {
            float determinant = value.Determinant();

            if (MathUtil.WithinEpsilon(determinant, 0.0f))
            {
                result = Identity;
                return;
            }

            float invdet = 1.0f / determinant;
            float _offsetX = value.M31;
            float _offsetY = value.M32;

            result = new Matrix3x2(
                value.M22 * invdet,
                -value.M12 * invdet,
                -value.M21 * invdet,
                value.M11 * invdet,
                (value.M21 * _offsetY - _offsetX * value.M22) * invdet,
                (_offsetX * value.M12 - value.M11 * _offsetY) * invdet);
        }

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="left">The first matrix to add.</param>
        /// <param name="right">The second matrix to add.</param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix3x2 operator +(Matrix3x2 left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Assert a matrix (return it unchanged).
        /// </summary>
        /// <param name="value">The matrix to assert (unchanged).</param>
        /// <returns>The asserted (unchanged) matrix.</returns>
        public static Matrix3x2 operator +(Matrix3x2 value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="left">The first matrix to subtract.</param>
        /// <param name="right">The second matrix to subtract.</param>
        /// <returns>The difference between the two matrices.</returns>
        public static Matrix3x2 operator -(Matrix3x2 left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Negates a matrix.
        /// </summary>
        /// <param name="value">The matrix to negate.</param>
        /// <returns>The negated matrix.</returns>
        public static Matrix3x2 operator -(Matrix3x2 value)
        {
            Matrix3x2 result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Scales a matrix by a given value.
        /// </summary>
        /// <param name="right">The matrix to scale.</param>
        /// <param name="left">The amount by which to scale.</param>
        /// <returns>The scaled matrix.</returns>
        public static Matrix3x2 operator *(float left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Multiply(ref right, left, out result);
            return result;
        }

        /// <summary>
        /// Scales a matrix by a given value.
        /// </summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled matrix.</returns>
        public static Matrix3x2 operator *(Matrix3x2 left, float right)
        {
            Matrix3x2 result;
            Multiply(ref left, right, out result);
            return result;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="left">The first matrix to multiply.</param>
        /// <param name="right">The second matrix to multiply.</param>
        /// <returns>The product of the two matrices.</returns>
        public static Matrix3x2 operator *(Matrix3x2 left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a matrix by a given value.
        /// </summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled matrix.</returns>
        public static Matrix3x2 operator /(Matrix3x2 left, float right)
        {
            Matrix3x2 result;
            Divide(ref left, right, out result);
            return result;
        }

        /// <summary>
        /// Divides two matrices.
        /// </summary>
        /// <param name="left">The first matrix to divide.</param>
        /// <param name="right">The second matrix to divide.</param>
        /// <returns>The quotient of the two matrices.</returns>
        public static Matrix3x2 operator /(Matrix3x2 left, Matrix3x2 right)
        {
            Matrix3x2 result;
            Divide(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix3x2 left, Matrix3x2 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix3x2 left, Matrix3x2 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
                M11, M12, M21, M22, M31, M32);
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

            return string.Format(format, CultureInfo.CurrentCulture, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
                M11.ToString(format, CultureInfo.CurrentCulture), M12.ToString(format, CultureInfo.CurrentCulture),
                M21.ToString(format, CultureInfo.CurrentCulture), M22.ToString(format, CultureInfo.CurrentCulture),
                M31.ToString(format, CultureInfo.CurrentCulture), M32.ToString(format, CultureInfo.CurrentCulture));
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
            return string.Format(formatProvider, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
                M11.ToString(formatProvider), M12.ToString(formatProvider),
                M21.ToString(formatProvider), M22.ToString(formatProvider),
                M31.ToString(formatProvider), M32.ToString(formatProvider));
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

            return string.Format(format, formatProvider, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
                M11.ToString(format, formatProvider), M12.ToString(format, formatProvider),
                M21.ToString(format, formatProvider), M22.ToString(format, formatProvider),
                M31.ToString(format, formatProvider), M32.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return M11.GetHashCode() + M12.GetHashCode() + 
               M21.GetHashCode() + M22.GetHashCode() +
               M31.GetHashCode() + M32.GetHashCode();
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Write optimized version without using Serialize methods
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(M11);
                serializer.Writer.Write(M12);
                serializer.Writer.Write(M21);
                serializer.Writer.Write(M22);
                serializer.Writer.Write(M31);
                serializer.Writer.Write(M32);
            }
            else
            {
                M11 = serializer.Reader.ReadSingle();
                M12 = serializer.Reader.ReadSingle();
                M21 = serializer.Reader.ReadSingle();
                M22 = serializer.Reader.ReadSingle();
                M31 = serializer.Reader.ReadSingle();
                M32 = serializer.Reader.ReadSingle();
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SharpDX.Matrix3x2"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SharpDX.Matrix3x2"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SharpDX.Matrix3x2"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Matrix3x2 other)
        {
            return (Math.Abs(other.M11 - M11) < MathUtil.ZeroTolerance &&
                Math.Abs(other.M12 - M12) < MathUtil.ZeroTolerance &&

                Math.Abs(other.M21 - M21) < MathUtil.ZeroTolerance &&
                Math.Abs(other.M22 - M22) < MathUtil.ZeroTolerance &&

                Math.Abs(other.M31 - M31) < MathUtil.ZeroTolerance &&
                Math.Abs(other.M32 - M32) < MathUtil.ZeroTolerance);
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
            if (value == null)
                return false;

            if (!ReferenceEquals(value.GetType(), typeof(Matrix3x2)))
                return false;

            return Equals((Matrix3x2)value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Matrix3x2"/> to <see cref="SharpDX.Matrix3x2"/>.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Matrix3x2(Matrix matrix)
        {
            return new Matrix3x2
                                   {
                                       M11 = matrix.M11,
                                       M12 = matrix.M12,
                                       M21 = matrix.M21,
                                       M22 = matrix.M22,
                                       M31 = matrix.M41,
                                       M32 = matrix.M42
                                   };
        }
    }

}
