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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    ///   The maximum number of bytes to which a pointer can point. Use for a count that must span the full range of a pointer.
    ///   Equivalent to Windows type SIZE_T.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PointerSize : IEquatable<PointerSize>
    {
        private IntPtr _size;

        /// <summary>
        /// An empty pointer size initialized to zero.
        /// </summary>
        public static readonly PointerSize Zero = new PointerSize(0);

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerSize"/> struct.
        /// </summary>
        /// <param name="size">The size.</param>
        public PointerSize(IntPtr size)
        {
            _size = size;
        }

        /// <summary>
        ///   Default constructor.
        /// </summary>
        /// <param name = "size">value to set</param>
        private unsafe PointerSize(void* size)
        {
            _size = new IntPtr(size);
        }

        /// <summary>
        ///   Default constructor.
        /// </summary>
        /// <param name = "size">value to set</param>
        public PointerSize(int size)
        {
            _size = new IntPtr(size);
        }

        /// <summary>
        ///   Default constructor.
        /// </summary>
        /// <param name = "size">value to set</param>
        public PointerSize(long size)
        {
            _size = new IntPtr(size);
        }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}", _size);
        }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <param name = "format">The format.</param>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "{0}", _size.ToString(format));
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _size.GetHashCode();
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "PointerSize" /> is equal to this instance.
        /// </summary>
        /// <param name = "other">The <see cref = "PointerSize" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "PointerSize" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(PointerSize other)
        {
            return _size.Equals(other._size);
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "value">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if(ReferenceEquals(null, value)) return false;
            return value is PointerSize && Equals((PointerSize)value);
        }

        /// <summary>
        ///   Adds two sizes.
        /// </summary>
        /// <param name = "left">The first size to add.</param>
        /// <param name = "right">The second size to add.</param>
        /// <returns>The sum of the two sizes.</returns>
        public static PointerSize operator +(PointerSize left, PointerSize right)
        {
            return new PointerSize(left._size.ToInt64() + right._size.ToInt64());
        }

        /// <summary>
        ///   Assert a size (return it unchanged).
        /// </summary>
        /// <param name = "value">The size to assert (unchanged).</param>
        /// <returns>The asserted (unchanged) size.</returns>
        public static PointerSize operator +(PointerSize value)
        {
            return value;
        }

        /// <summary>
        ///   Subtracts two sizes.
        /// </summary>
        /// <param name = "left">The first size to subtract.</param>
        /// <param name = "right">The second size to subtract.</param>
        /// <returns>The difference of the two sizes.</returns>
        public static PointerSize operator -(PointerSize left, PointerSize right)
        {
            return new PointerSize(left._size.ToInt64() - right._size.ToInt64());
        }

        /// <summary>
        ///   Reverses the direction of a given size.
        /// </summary>
        /// <param name = "value">The size to negate.</param>
        /// <returns>A size facing in the opposite direction.</returns>
        public static PointerSize operator -(PointerSize value)
        {
            return new PointerSize(-value._size.ToInt64());
        }

        /// <summary>
        ///   Scales a size by the given value.
        /// </summary>
        /// <param name = "value">The size to scale.</param>
        /// <param name = "scale">The amount by which to scale the size.</param>
        /// <returns>The scaled size.</returns>
        public static PointerSize operator *(int scale, PointerSize value)
        {
            return new PointerSize(scale*value._size.ToInt64());
        }

        /// <summary>
        ///   Scales a size by the given value.
        /// </summary>
        /// <param name = "value">The size to scale.</param>
        /// <param name = "scale">The amount by which to scale the size.</param>
        /// <returns>The scaled size.</returns>
        public static PointerSize operator *(PointerSize value, int scale)
        {
            return new PointerSize(scale*value._size.ToInt64());
        }

        /// <summary>
        ///   Scales a size by the given value.
        /// </summary>
        /// <param name = "value">The size to scale.</param>
        /// <param name = "scale">The amount by which to scale the size.</param>
        /// <returns>The scaled size.</returns>
        public static PointerSize operator /(PointerSize value, int scale)
        {
            return new PointerSize(value._size.ToInt64()/scale);
        }

        /// <summary>
        ///   Tests for equality between two objects.
        /// </summary>
        /// <param name = "left">The first value to compare.</param>
        /// <param name = "right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name = "left" /> has the same value as <paramref name = "right" />; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PointerSize left, PointerSize right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///   Tests for inequality between two objects.
        /// </summary>
        /// <param name = "left">The first value to compare.</param>
        /// <param name = "right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name = "left" /> has a different value than <paramref name = "right" />; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PointerSize left, PointerSize right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "PointerSize" /> to <see cref = "int" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(PointerSize value)
        {
            return value._size.ToInt32();
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "PointerSize" /> to <see cref = "long" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator long(PointerSize value)
        {
            return value._size.ToInt64();
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "PointerSize" /> to <see cref = "int" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PointerSize(int value)
        {
            return new PointerSize(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "PointerSize" /> to <see cref = "long" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PointerSize(long value)
        {
            return new PointerSize(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.IntPtr"/> to <see cref="PointerSize"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PointerSize(IntPtr value)
        {
            return new PointerSize(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "PointerSize" /> to <see cref = "IntPtr" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator IntPtr(PointerSize value)
        {
            return value._size;
        }

        /// <summary>
        ///   Performs an implicit conversion from void* to <see cref = "PointerSize" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static unsafe implicit operator PointerSize(void* value)
        {
            return new PointerSize(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "PointerSize" /> to void*.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static unsafe implicit operator void*(PointerSize value)
        {
            return (void*) value._size;
        }
    }
}