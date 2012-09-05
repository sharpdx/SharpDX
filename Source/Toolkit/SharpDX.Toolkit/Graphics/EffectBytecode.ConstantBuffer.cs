// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectBytecode 
    {
        /// <summary>
        /// Describes a constant buffer.
        /// </summary>
        public sealed class ConstantBuffer : IDataSerializable, IEquatable<ConstantBuffer>
        {
            /// <summary>
            /// Name of this constant buffer.
            /// </summary>
            public string Name;

            /// <summary>
            /// Slot index register to use for binding.
            /// </summary>
            public byte Slot;

            /// <summary>
            /// Size in bytes of this constant buffer.
            /// </summary>
            public int Size;

            /// <summary>
            /// List of parameters in this constant buffer.
            /// </summary>
            public List<ValueTypeParameter> Parameters;

            public bool Equals(ConstantBuffer other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && Slot == other.Slot && Size == other.Size && Utilities.Compare(Parameters, other.Parameters);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ConstantBuffer && Equals((ConstantBuffer) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = Name.GetHashCode();
                    hashCode = (hashCode * 397) ^ Slot.GetHashCode();
                    hashCode = (hashCode * 397) ^ Size;
                    hashCode = (hashCode * 397) ^ Parameters.Count;
                    return hashCode;
                }
            }

            public static bool operator ==(ConstantBuffer left, ConstantBuffer right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ConstantBuffer left, ConstantBuffer right)
            {
                return !Equals(left, right);
            }


            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name);
                serializer.Serialize(ref Slot);
                serializer.SerializePackedInt(ref Size);
                serializer.Serialize(ref Parameters);
            }
        }
    }
}