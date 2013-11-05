﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using SharpDX.Serialization;

namespace SharpDX
{
    /// <summary>A boolean value stored on 4 bytes (instead of 1 in .NET).</summary>
#if !W8CORE
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    [DynamicSerializer("TKB1")]
    public struct Bool : IEquatable<Bool>, IDataSerializable
    {
        private int boolValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bool" /> class.
        /// </summary>
        /// <param name="boolValue">if set to <c>true</c> [bool value].</param>
        public Bool(bool boolValue)
        {
            this.boolValue = boolValue ? 1 : 0;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if <paramref name="other" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public bool Equals(Bool other)
        {
            return this.boolValue == other.boolValue;
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Bool && Equals((Bool)obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
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
        public static bool operator ==(Bool left, Bool right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Bool left, Bool right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.Bool"/> to <see cref="bool"/>.
        /// </summary>
        /// <param name="booleanValue">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator bool(Bool booleanValue)
        {
            return booleanValue.boolValue != 0;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="bool"/> to <see cref="SharpDX.Bool"/>.
        /// </summary>
        /// <param name="boolValue">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Bool(bool boolValue)
        {
            return new Bool(boolValue);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}", boolValue != 0);
        }

        /// <summary>Reads or writes datas from/to the given binary serializer.</summary>
        /// <param name="serializer">The binary serializer.</param>
        /// <inheritdoc />
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Write optimized version without using Serialize methods
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(boolValue);
            }
            else
            {
                boolValue = serializer.Reader.ReadInt32();
            }
        }

    }
}