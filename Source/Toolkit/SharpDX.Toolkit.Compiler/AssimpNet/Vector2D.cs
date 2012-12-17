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
    /// Represents a two-dimensional vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vector2D : IEquatable<Vector2D> {
        /// <summary>
        /// X component.
        /// </summary>
        public float X;

        /// <summary>
        /// Y component
        /// </summary>
        public float Y;

        /// <summary>
        /// Gets or sets the component value at the specified zero-based index
        /// in the order of XY (index 0 access X, 1 access Y. If
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
                }
            }
        }

        /// <summary>
        /// Constructs a new Vector2D.
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public Vector2D(float x, float y) {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Constructs a new Vector2D with both components
        /// set the same value.
        /// </summary>
        /// <param name="value">Value to set both X and Y to</param>
        public Vector2D(float value) {
            X = value;
            Y = value;
        }

        /// <summary>
        /// Sets the X and Y values.
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public void Set(float x, float y) {
            X = x;
            Y = y;
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
            return (X * X) + (Y * Y);
        }

        /// <summary>
        /// Normalizes the vector where all components add to one (Unit Vector), but preserves
        /// the direction that the vector represents.
        /// </summary>
        public void Normalize() {
            float invLength = 1.0f / (float) System.Math.Sqrt((X * X) + (Y * Y));
            X *= invLength;
            Y *= invLength;
        }

        /// <summary>
        /// Negates the vector.
        /// </summary>
        public void Negate() {
            X = -X;
            Y = -Y;
        }

        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Added vector</returns>
        public static Vector2D operator+(Vector2D a, Vector2D b) {
            Vector2D v;
            v.X = a.X + b.X;
            v.Y = a.Y + b.Y;
            return v;
        }

        /// <summary>
        /// Subtracts the second vector from the first vector.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector2D operator-(Vector2D a, Vector2D b) {
            Vector2D v;
            v.X = a.X - b.X;
            v.Y = a.Y - b.Y;
            return v;
        }

        /// <summary>
        /// Multiplies two vectors together.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Multiplied vector</returns>
        public static Vector2D operator*(Vector2D a, Vector2D b) {
            Vector2D v;
            v.X = a.X * b.X;
            v.Y = a.Y * b.Y;
            return v;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="value">Source vector</param>
        /// <param name="scale">Scalar value</param>
        /// <returns>Scaled vector</returns>
        public static Vector2D operator*(Vector2D value, float scale) {
            Vector2D v;
            v.X = value.X * scale;
            v.Y = value.Y * scale;
            return v;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="scale">Scalar value</param>
        /// <param name="value">Source vector</param>
        /// <returns>Scaled vector</returns>
        public static Vector2D operator*(float scale, Vector2D value) {
            Vector2D v;
            v.X = value.X * scale;
            v.Y = value.Y * scale;
            return v;
        }

        /// <summary>
        /// Divides the first vector by the second vector.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Divided vector</returns>
        public static Vector2D operator/(Vector2D a, Vector2D b) {
            Vector2D v;
            v.X = a.X / b.X;
            v.Y = a.Y / b.Y;
            return v;
        }

        /// <summary>
        /// Divides the vector by a divisor value.
        /// </summary>
        /// <param name="value">Source vector</param>
        /// <param name="divisor">Divisor</param>
        /// <returns>Divided vector</returns>
        public static Vector2D operator/(Vector2D value, float divisor) {
            float invDivisor = 1.0f / divisor;
            Vector2D v;
            v.X = value.X * invDivisor;
            v.Y = value.Y * invDivisor;
            return v;
        }

        /// <summary>
        /// Negates the vector.
        /// </summary>
        /// <param name="value">Source vector</param>
        /// <returns>Negated vector</returns>
        public static Vector2D operator-(Vector2D value) {
            Vector2D v;
            v.X = -value.X;
            v.Y = -value.Y;
            return v;
        }

        /// <summary>
        /// Tets equality between two vectors.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>True if the vectors are equal, false otherwise</returns>
        public static bool operator==(Vector2D a, Vector2D b) {
            return (a.X == b.X) && (a.Y == b.Y);
        }

        /// <summary>
        /// Tests inequality between two vectors.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>True if the vectors are not equal, false otherwise</returns>
        public static bool operator!=(Vector2D a, Vector2D b) {
            return (a.X != b.X) || (a.Y != b.Y);
        }

        /// <summary>
        /// Tests equality between this vector and another vector.
        /// </summary>
        /// <param name="other">Vector to test against</param>
        /// <returns>True if components are equal</returns>
        public bool Equals(Vector2D other) {
            return (X == other.X) && (Y == other.Y);
        }

        /// <summary>
        /// Tests equality between this vector and another object.
        /// </summary>
        /// <param name="obj">Object to test against</param>
        /// <returns>True if the object is a vector and the components are equal</returns>
        public override bool Equals(object obj) {
            if(obj is Vector2D) {
                return Equals((Vector2D) obj);
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
            return String.Format(info, "{{X:{0} Y:{1}}}",
                new Object[] { X.ToString(info), Y.ToString(info) });
        }
    }
}
