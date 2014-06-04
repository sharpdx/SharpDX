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
using SharpDX.Serialization;
using System;
using System.Globalization;

namespace SharpDX
{
    /// <summary>
    /// Represents a transformation composed of a scaling, rotation and translation operation.
    /// </summary>
    public struct CompositeTransform : IEquatable<CompositeTransform>, IFormattable, IDataSerializable
    {
        /// <summary>
        /// The scaling component of the transformation.
        /// </summary>
        public Vector3 Scale;

        /// <summary>
        /// The rotation component of the transformation.
        /// </summary>
        public Quaternion Rotation;

        /// <summary>
        /// The translation component of the transformation.
        /// </summary>
        public Vector3 Translation;

        /// <summary>
        /// The identety <see cref="SharpDX.CompositeTransform"/>.
        /// </summary>
        public static readonly CompositeTransform Identity = new CompositeTransform(Vector3.One, Quaternion.Identity, Vector3.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.CompositeTransform"/> struct.
        /// </summary>
        /// <param name="scale">The scaling component of the transformation.</param>
        /// <param name="rotation">The rotation component of the transformation.</param>
        /// <param name="translation">The translation component of the transformation.</param>
        public CompositeTransform(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            Scale = scale;
            Rotation = rotation;
            Translation = translation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.CompositeTransform"/> struct.
        /// </summary>
        /// <param name="transform">The transformation matrix.</param>
        /// <remarks>
        /// This constructor is designed to decompose a SRT transformation matrix only.
        /// </remarks>
        public CompositeTransform(Matrix transform)
        {
            transform.Decompose(out Scale, out Rotation, out Translation);
        }

        /// <summary>
        /// Interpolates between two transformation, using spherical linear interpolation for rotations and linear interpolation for scaling and translation.
        /// </summary>
        /// <param name="start">Start transformation.</param>
        /// <param name="end">End transformation.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the interpolation of the two transformations.</param>
        public static void Slerp(ref CompositeTransform start, ref CompositeTransform end, float amount, out CompositeTransform result)
        {
            Vector3.Lerp(ref start.Scale, ref end.Scale, amount, out result.Scale);
            Quaternion.Slerp(ref start.Rotation, ref end.Rotation, amount, out result.Rotation);
            Vector3.Lerp(ref start.Translation, ref end.Translation, amount, out result.Translation);
        }

        /// <summary>
        /// Interpolates between two transformation, using spherical linear interpolation for rotations and linear interpolation for scaling and translation.
        /// </summary>
        /// <param name="start">Start transformation.</param>
        /// <param name="end">End transformation.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The interpolation of the two transformations.</returns>
        public static CompositeTransform Slerp(CompositeTransform start, CompositeTransform end, float amount)
        {
            CompositeTransform result;
            Slerp(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.CompositeTransform"/> to <see cref="SlimDX.Matrix"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Matrix(CompositeTransform value)
        {
            return Matrix.Scaling(value.Scale) * Matrix.RotationQuaternion(value.Rotation) * Matrix.Translation(value.Translation);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(CompositeTransform left, CompositeTransform right)
        {
            return left.Equals(ref right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(CompositeTransform left, CompositeTransform right)
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
            return string.Format(CultureInfo.CurrentCulture, "Scale:{0} Rotation:{1} Translation:{2}", Scale.ToString(), Rotation.ToString(), Translation.ToString());
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

            return string.Format(CultureInfo.CurrentCulture, "Scale:{0} Rotation:{1} Translation:{2}", Scale.ToString(format, CultureInfo.CurrentCulture),
                Rotation.ToString(format, CultureInfo.CurrentCulture), Translation.ToString(format, CultureInfo.CurrentCulture));
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
            return string.Format(formatProvider, "Scale:{0} Rotation:{1} Translation:{2}", Scale.ToString(), Rotation.ToString(), Translation.ToString());
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

            return string.Format(formatProvider, "Scale:{0} Rotation:{1} Translation:{2}", Scale.ToString(format, formatProvider),
                Rotation.ToString(format, formatProvider), Translation.ToString(format, formatProvider));
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
                var hashCode = Scale.GetHashCode();
                hashCode = (hashCode * 397) ^ Rotation.GetHashCode();
                hashCode = (hashCode * 397) ^ Translation.GetHashCode();
                return hashCode;
            }
        }

        /// <inheritdoc/>
        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref Scale);
            serializer.Serialize(ref Rotation);
            serializer.Serialize(ref Translation);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SharpDX.CompositeTransform"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SharpDX.CompositeTransform"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SharpDX.CompositeTransform"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref CompositeTransform other)
        {
            return Scale.Equals(ref other.Scale) && Rotation.Equals(ref other.Rotation) && Translation.Equals(ref other.Translation);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SharpDX.CompositeTransform"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SharpDX.CompositeTransform"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SharpDX.CompositeTransform"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(CompositeTransform other)
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
            if (!(value is CompositeTransform))
                return false;

            var strongValue = (CompositeTransform)value;
            return Equals(ref strongValue);
        }
    }
}
