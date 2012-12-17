/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Assimp {
    /// <summary>
    /// Represents a three-dimensional vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vector3D : IEquatable<Vector3D> {
        /// <summary>
        /// X component.
        /// </summary>
        public float X;

        /// <summary>
        /// Y component.
        /// </summary>
        public float Y;

        /// <summary>
        /// Z component.
        /// </summary>
        public float Z;

        /// <summary>
        /// Gets or sets the component value at the specified zero-based index
        /// in the order of XYZ (index 0 access X, 1 access Y, etc). If
        /// the index is not in range, a value of zero is returned.
        /// </summary>
        /// <param name="index">Zero-based index.</param>
        /// <returns>The component value</returns>
        public float this[int index] {
            get {
                switch(index) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        return 0;
                }
            }
            set {
                switch(index) {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Constructs a new Vector3D.
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        /// <param name="z">Z component</param>
        public Vector3D(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Constructs a new Vector3D.
        /// </summary>
        /// <param name="value">Vector2D containing the X, Y values</param>
        /// <param name="z">Z component</param>
        public Vector3D(Vector2D value, float z) {
            X = value.X;
            Y = value.Y;
            Z = z;
        }

        /// <summary>
        /// Constructs a new Vector3D where each component is set
        /// to the same value.
        /// </summary>
        /// <param name="value">Value to set X, Y, and Z to</param>
        public Vector3D(float value) {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Sets the X, Y, and Z values.
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        /// <param name="z">Z component</param>
        public void Set(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>Vector's length</returns>
        public float Length() {
            return (float) Math.Sqrt(LengthSquared());
        }

        /// <summary>
        /// Calculates the length of the vector squared.
        /// </summary>
        /// <returns>Vector's length squared</returns>
        public float LengthSquared() {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        /// <summary>
        /// Normalizes the vector where all components add to one (Unit Vector), but preserves
        /// the direction that the vector represents.
        /// </summary>
        public void Normalize() {
            float invLength = 1.0f / (float) Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            X *= invLength;
            Y *= invLength;
            Z *= invLength;
        }

        /// <summary>
        /// Negates the vector.
        /// </summary>
        public void Negate() {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector3D Cross(Vector3D a, Vector3D b) {
            Vector3D v;
            v.X = (a.Y * b.Z) - (a.Z * b.Y);
            v.Y = (a.Z * b.X) - (a.X * b.Z);
            v.Z = (a.X * b.Y) - (a.Y * b.X);
            return v;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector</returns>
        public static float Dot(Vector3D a, Vector3D b) {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Added vector</returns>
        public static Vector3D operator+(Vector3D a, Vector3D b) {
            Vector3D v;
            v.X = a.X + b.X;
            v.Y = a.Y + b.Y;
            v.Z = a.Z + b.Z;
            return v;
        }

        /// <summary>
        /// Subtracts the second vector from the first vector.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector3D operator-(Vector3D a, Vector3D b) {
            Vector3D v;
            v.X = a.X - b.X;
            v.Y = a.Y - b.Y;
            v.Z = a.Z - b.Z;
            return v;
        }

        /// <summary>
        /// Multiplies two vectors together.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Multiplied vector</returns>
        public static Vector3D operator*(Vector3D a, Vector3D b) {
            Vector3D v;
            v.X = a.X * b.X;
            v.Y = a.Y * b.Y;
            v.Z = a.Z * b.Z;
            return v;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="value">Source vector</param>
        /// <param name="scale">Scalar value</param>
        /// <returns>Scaled vector</returns>
        public static Vector3D operator*(Vector3D value, float scale) {
            Vector3D v;
            v.X = value.X * scale;
            v.Y = value.Y * scale;
            v.Z = value.Z * scale;
            return v;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="scale">Scalar value</param>
        /// <param name="value">Source vector</param>
        /// <returns>Scaled vector</returns>
        public static Vector3D operator*(float scale, Vector3D value) {
            Vector3D v;
            v.X = value.X * scale;
            v.Y = value.Y * scale;
            v.Z = value.Z * scale;
            return v;
        }

        /// <summary>
        /// Transforms this vector by a 3x3 matrix. This "post-multiplies" the two.
        /// </summary>
        /// <param name="matrix">Source matrix</param>
        /// <param name="vector">Source vector</param>
        /// <returns>Transformed vector</returns>
        public static Vector3D operator*(Matrix3x3 matrix, Vector3D vector) {
            Vector3D v;
            v.X = (vector.X * matrix.A1) + (vector.Y * matrix.A2) + (vector.Z * matrix.A3);
            v.Y = (vector.X * matrix.B1) + (vector.Y * matrix.B2) + (vector.Z * matrix.B3);
            v.Z = (vector.X * matrix.C1) + (vector.Y * matrix.C2) + (vector.Z * matrix.C3);
            return v;
        }

        /// <summary>
        /// Transforms this vector by a 4x4 matrix. This "post-multiplies" the two.
        /// </summary>
        /// <param name="matrix">Source matrix</param>
        /// <param name="vector">Source vector</param>
        /// <returns>Transformed vector</returns>
        public static Vector3D operator*(Matrix4x4 matrix, Vector3D vector) {
            Vector3D v;
            v.X = (vector.X * matrix.A1) + (vector.Y * matrix.A2) + (vector.Z * matrix.A3) + matrix.A4;
            v.Y = (vector.X * matrix.B1) + (vector.Y * matrix.B2) + (vector.Z * matrix.B3) + matrix.B4;
            v.Z = (vector.X * matrix.C1) + (vector.Y * matrix.C2) + (vector.Z * matrix.C3) + matrix.C4;
            return v;
        }

        /// <summary>
        /// Divides the first vector by the second vector.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Divided vector</returns>
        public static Vector3D operator/(Vector3D a, Vector3D b) {
            Vector3D v;
            v.X = a.X / b.X;
            v.Y = a.Y / b.Y;
            v.Z = a.Z / b.Z;
            return v;
        }

        /// <summary>
        /// Divides the vector by a divisor value.
        /// </summary>
        /// <param name="value">Source vector</param>
        /// <param name="divisor">Divisor</param>
        /// <returns>Divided vector</returns>
        public static Vector3D operator/(Vector3D value, float divisor) {
            float invDivisor = 1.0f / divisor;
            Vector3D v;
            v.X = value.X * invDivisor;
            v.Y = value.Y * invDivisor;
            v.Z = value.Z * invDivisor;
            return v;
        }

        /// <summary>
        /// Negates the vector.
        /// </summary>
        /// <param name="value">Source vector</param>
        /// <returns>Negated vector</returns>
        public static Vector3D operator-(Vector3D value) {
            Vector3D v;
            v.X = -value.X;
            v.Y = -value.Y;
            v.Z = -value.Z;
            return v;
        }

        /// <summary>
        /// Tets equality between two vectors.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>True if the vectors are equal, false otherwise</returns>
        public static bool operator==(Vector3D a, Vector3D b) {
            return (a.X == b.X) && (a.Y == b.Y) && (a.Z == b.Z);
        }

        /// <summary>
        /// Tests inequality between two vectors.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>True if the vectors are not equal, false otherwise</returns>
        public static bool operator!=(Vector3D a, Vector3D b) {
            return (a.X != b.X) || (a.Y != b.Y) || (a.Z != b.Z);
        }

        /// <summary>
        /// Tests equality between this vector and another vector.
        /// </summary>
        /// <param name="other">Vector to test against</param>
        /// <returns>True if components are equal</returns>
        public bool Equals(Vector3D other) {
            return (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }

        /// <summary>
        /// Tests equality between this vector and another object.
        /// </summary>
        /// <param name="obj">Object to test against</param>
        /// <returns>True if the object is a vector and the components are equal</returns>
        public override bool Equals(object obj) {
            if(obj is Vector3D) {
                return Equals((Vector3D) obj);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return (X.GetHashCode() + Y.GetHashCode());
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            CultureInfo info = CultureInfo.CurrentCulture;
            return String.Format(info, "{{X:{0} Y:{1} Z:{2}}}",
                new Object[] { X.ToString(info), Y.ToString(info), Z.ToString(info) });
        }
    }
}
