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

using SharpDX.Direct3D11;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectData 
    {
        /// <summary>A link to a compiled shader.</summary>
        public sealed class ShaderLink : IDataSerializable, IEquatable<ShaderLink>
        {
            /// <summary>The null shader.</summary>
            public static readonly ShaderLink NullShader = new ShaderLink();

            private int index;
            private string importName;

            /// <summary>
            /// The stream output rasterized stream (-1 if no rasterized stream).
            /// </summary>
            public int StreamOutputRasterizedStream;

            /// <summary>
            /// The stream output elements only valid for a geometry shader, can be null.
            /// </summary>
            public StreamOutputElement[] StreamOutputElements;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShaderLink" /> class.
            /// </summary>
            public ShaderLink()
            {
                index = -1;
                StreamOutputRasterizedStream = -1;
            }

            /// <summary>
            /// Gets a value indicating whether this is an import.
            /// </summary>
            /// <value><c>true</c> if this is an import; otherwise, <c>false</c>.</value>
            /// <remarks>
            /// When this is an import, the <see cref="Index"/> is not valid. Only <see cref="ImportName"/> is valid.
            /// </remarks>
            public bool IsImport
            {
                get { return importName != null; }
            }

            /// <summary>
            /// Gets or sets the index in the shader pool.
            /// </summary>
            /// <value>The index.</value>
            /// <remarks>
            /// This index is a direct reference to the shader in <see cref="EffectData.Shaders"/>.
            /// </remarks>
            public int Index
            {
                get { return index; }
                set { index = value; }
            }

            /// <summary>
            /// Gets or sets the name of the shader import. Can be null.
            /// </summary>
            /// <value>The name of the import.</value>
            /// <remarks>
            /// This property is not null when there is no shader compiled and this is an import.
            /// </remarks>
            public string ImportName
            {
                get { return importName; }
                set { importName = value; }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is a null shader.
            /// </summary>
            /// <value><c>true</c> if this instance is null shader; otherwise, <c>false</c>.</value>
            public bool IsNullShader
            {
                get { return index < 0; }
            }

            /// <summary>Clones this instance.</summary>
            /// <returns>ShaderLink.</returns>
            public ShaderLink Clone()
            {
                return (ShaderLink)MemberwiseClone();
            }

            /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
            public bool Equals(ShaderLink other)
            {
                if (ReferenceEquals(null, other))
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return this.index == other.index && string.Equals(this.importName, other.importName);
            }

            /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
            /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
            /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                return obj is ShaderLink && Equals((ShaderLink)obj);
            }

            /// <summary>Returns a hash code for this instance.</summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (this.index * 397) ^ (this.importName != null ? this.importName.GetHashCode() : 0);
                }
            }

            /// <summary>Implements the ==.</summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(ShaderLink left, ShaderLink right)
            {
                return Equals(left, right);
            }

            /// <summary>Implements the !=.</summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(ShaderLink left, ShaderLink right)
            {
                return !Equals(left, right);
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.SerializePackedInt(ref index);

                // Enable null reference just for the import name
                serializer.Serialize(ref importName, SerializeFlags.Nullable);

                // Serialize GS rasterized stream if any.
                serializer.Serialize(ref StreamOutputRasterizedStream);

                // Enable null for StreamOutputElements
                serializer.Serialize(ref StreamOutputElements, SerializeFlags.Nullable);
            }
        }
    }
}