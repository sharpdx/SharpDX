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

using System;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>The effect data class.</summary>
    public partial class EffectData 
    {
        /// <summary>
        /// Describes a value type parameter used by a <see cref="ConstantBuffer"/>.
        /// </summary>
        public sealed class ValueTypeParameter : Parameter, IEquatable<ValueTypeParameter>
        {
            /// <summary>
            /// Offset in bytes into the <see cref="ConstantBuffer"/>.
            /// </summary>
            public int Offset;

            /// <summary>
            /// Number of elements.
            /// </summary>
            public int Count;

            /// <summary>
            /// Size in bytes in the <see cref="ConstantBuffer"/>.
            /// </summary>
            public int Size;

            /// <summary>
            /// Number of rows for this element.
            /// </summary>
            public byte RowCount;

            /// <summary>
            /// Number of columns for this element.
            /// </summary>
            public byte ColumnCount;

            /// <summary>
            /// The default value.
            /// </summary>
            public byte[] DefaultValue;

            /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
            public bool Equals(ValueTypeParameter other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return base.Equals(other) && Offset == other.Offset && Count == other.Count && Size == other.Size && RowCount == other.RowCount && ColumnCount == other.ColumnCount && Utilities.Compare(DefaultValue, other.DefaultValue);
            }

            /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
            /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
            /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ValueTypeParameter && Equals((ValueTypeParameter) obj);
            }

            /// <summary>Returns a hash code for this instance.</summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = base.GetHashCode();
                    hashCode = (hashCode * 397) ^ Offset;
                    hashCode = (hashCode * 397) ^ Count;
                    hashCode = (hashCode * 397) ^ Size;
                    hashCode = (hashCode * 397) ^ RowCount.GetHashCode();
                    hashCode = (hashCode * 397) ^ ColumnCount.GetHashCode();
                    hashCode = (hashCode * 397) ^ ((DefaultValue == null) ? 0 : DefaultValue.Length);
                    return hashCode;
                }
            }

            /// <summary>Implements the ==.</summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(ValueTypeParameter left, ValueTypeParameter right)
            {
                return Equals(left, right);
            }

            /// <summary>Implements the !=.</summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(ValueTypeParameter left, ValueTypeParameter right)
            {
                return !Equals(left, right);
            }

            /// <summary>Serialize this instance but hides implementation from outside..</summary>
            /// <param name="serializer">The serializer.</param>
            internal override void InternalSerialize(BinarySerializer serializer)
            {
                base.InternalSerialize(serializer);
                serializer.SerializePackedInt(ref Offset);
                serializer.SerializePackedInt(ref Count);
                serializer.SerializePackedInt(ref Size);
                serializer.Serialize(ref RowCount);
                serializer.Serialize(ref ColumnCount);
                serializer.Serialize(ref DefaultValue, SerializeFlags.Nullable);
            }
        }
    }
}