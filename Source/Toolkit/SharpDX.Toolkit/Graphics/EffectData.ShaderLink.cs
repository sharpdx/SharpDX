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
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectData 
    {
        /// <summary>
        /// A link to a compiled shader.
        /// </summary>
        public sealed class ShaderLink : IDataSerializable
        {
            private int index;
            private string importName;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShaderLink" /> class.
            /// </summary>
            public ShaderLink()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ShaderLink" /> class.
            /// </summary>
            /// <param name="index">The index in the shader pool.</param>
            public ShaderLink(int index)
            {
                this.index = index;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ShaderLink" /> class.
            /// </summary>
            /// <param name="importName">Name of the import function.</param>
            public ShaderLink(string importName)
            {
                this.importName = importName;
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

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.SerializePackedInt(ref index);

                // Enable null reference just for the import name
                serializer.AllowNull = true;
                serializer.Serialize(ref importName);
                serializer.AllowNull = false;
            }
        }
    }
}