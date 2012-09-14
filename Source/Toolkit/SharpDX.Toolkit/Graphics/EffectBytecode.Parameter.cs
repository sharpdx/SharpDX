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

            public bool Equals(Parameter other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && Class.Equals(other.Class) && Type.Equals(other.Type);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Parameter) obj);
            }

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

            public static bool operator ==(Parameter left, Parameter right)
            {
                return Equals(left, right);
            }

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

            public override string ToString()
            {
                return string.Format("Name: {0}, Class: {1}, Type: {2}", Name, Class, Type);
            }
        }
    }
}