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
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
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

            public bool Equals(ValueTypeParameter other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return base.Equals(other) && Offset == other.Offset && Count == other.Count && Size == other.Size && RowCount == other.RowCount && ColumnCount == other.ColumnCount && Utilities.Compare(DefaultValue, other.DefaultValue);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ValueTypeParameter && Equals((ValueTypeParameter) obj);
            }

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

            public static bool operator ==(ValueTypeParameter left, ValueTypeParameter right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ValueTypeParameter left, ValueTypeParameter right)
            {
                return !Equals(left, right);
            }

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