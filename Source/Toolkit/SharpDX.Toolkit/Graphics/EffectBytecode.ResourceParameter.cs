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
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectBytecode 
    {
        /// <summary>
        /// Describes a resource parameter.
        /// </summary>
        public sealed class ResourceParameter : Parameter, IEquatable<ResourceParameter>
        {
            /// <summary>
            /// The slot index register to bind to.
            /// </summary>
            public byte Slot;

            /// <summary>
            /// The number of slots to bind.
            /// </summary>
            public byte Count;

            public bool Equals(ResourceParameter other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return base.Equals(other) && Slot == other.Slot && Count == other.Count;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ResourceParameter && Equals((ResourceParameter) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = base.GetHashCode();
                    hashCode = (hashCode * 397) ^ Slot.GetHashCode();
                    hashCode = (hashCode * 397) ^ Count.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(ResourceParameter left, ResourceParameter right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ResourceParameter left, ResourceParameter right)
            {
                return !Equals(left, right);
            }

            /// <inheritdoc/>
            internal override void InternalSerialize(BinarySerializer serializer)
            {
                base.InternalSerialize(serializer);
                serializer.Serialize(ref Slot);
                serializer.Serialize(ref Count);
            }
        }
    }
}