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
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectData
    {
        public struct ShaderMacro : IEquatable<Direct3D.ShaderMacro>, IDataSerializable
        {
            /// <summary>
            /// The name of the macro.
            /// </summary>
            public string Name;

            /// <summary>
            /// The value of the macro.
            /// </summary>
            public string Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShaderMacro" /> struct.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public ShaderMacro(string name, object value)
            {
                Name = name;
                Value = value == null ? null : value.ToString();
            }

            public bool Equals(Direct3D.ShaderMacro other)
            {
                return string.Equals(this.Name, other.Name) && string.Equals(this.Value, other.Definition);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is Direct3D.ShaderMacro && Equals((Direct3D.ShaderMacro)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^ (this.Value != null ? this.Value.GetHashCode() : 0);
                }
            }

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name);
                serializer.Serialize(ref Value, SerializeFlags.Nullable);
            }

            public static bool operator ==(ShaderMacro left, ShaderMacro right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(ShaderMacro left, ShaderMacro right)
            {
                return !left.Equals(right);
            }
        }
    }
}