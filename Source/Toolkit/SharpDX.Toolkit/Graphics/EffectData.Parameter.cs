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
        /// An abstract parameter, which can be a <see cref="ResourceParameter"/> or a <see cref="ValueTypeParameter"/>.
        /// </summary>
        public abstract class Parameter : IDataSerializable, IEquatable<Parameter>
        {
            /// <summary>
            /// Name of this parameter.
            /// </summary>
            public string Name;

            /// <summary>
            /// The <see cref="EffectParameterClass"/> of this parameter.
            /// </summary>
            public EffectParameterClass Class;

            /// <summary>
            /// The <see cref="EffectParameterType"/> of this parameter.
            /// </summary>
            public EffectParameterType Type;

            /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
            public bool Equals(Parameter other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && Class == other.Class && Type == other.Type;
            }

            /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
            /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
            /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Parameter) obj);
            }

            /// <summary>Returns a hash code for this instance.</summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = Name.GetHashCode();
                    hashCode = (hashCode * 397) ^ Class.GetHashCode();
                    hashCode = (hashCode * 397) ^ Type.GetHashCode();
                    return hashCode;
                }
            }

            /// <summary>Implements the ==.</summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(Parameter left, Parameter right)
            {
                return Equals(left, right);
            }

            /// <summary>Implements the !=.</summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(Parameter left, Parameter right)
            {
                return !Equals(left, right);
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                InternalSerialize(serializer);
            }

            /// <summary>
            /// Serialize this instance but hides implementation from outside..
            /// </summary>
            /// <param name="serializer">The serializer.</param>
            internal virtual void InternalSerialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name);
                serializer.SerializeEnum(ref Class);
                serializer.SerializeEnum(ref Type);
            }

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return string.Format("Name: {0}, Class: {1}, Type: {2}", Name, Class, Type);
            }
        }
    }
}