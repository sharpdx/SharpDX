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
using System.Runtime.InteropServices;

namespace SharpDX.Mathematics.Interop
{
    /// <summary>
    /// A boolean value stored on 4 bytes (instead of 1 in .NET).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct RawBool : IEquatable<RawBool>
    {
        private readonly int boolValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RawBool" /> class.
        /// </summary>
        /// <param name="boolValue">if set to <c>true</c> [bool value].</param>
        public RawBool(bool boolValue)
        {
            this.boolValue = boolValue ? 1 : 0;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if <paramref name="other" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public bool Equals(RawBool other)
        {
            return this.boolValue == other.boolValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is RawBool && Equals((RawBool)obj);
        }

        public override int GetHashCode()
        {
            return this.boolValue;
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RawBool left, RawBool right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RawBool left, RawBool right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="RawBool"/> to <see cref="bool"/>.
        /// </summary>
        /// <param name="booleanValue">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator bool(RawBool booleanValue)
        {
            return booleanValue.boolValue != 0;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="bool"/> to <see cref="RawBool"/>.
        /// </summary>
        /// <param name="boolValue">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RawBool(bool boolValue)
        {
            return new RawBool(boolValue);
        }

        public override string ToString()
        {
            return string.Format("{0}", boolValue != 0);
        }
    }
}