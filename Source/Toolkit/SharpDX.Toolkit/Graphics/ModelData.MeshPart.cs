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

using System.Collections.Generic;
using System.Runtime.InteropServices;

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>The model data class.</summary>
    public sealed partial class ModelData
    {
        /// <summary>The mesh part class.</summary>
        public sealed class MeshPart : CommonData, IDataSerializable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MeshPart"/> class.
            /// </summary>
            public MeshPart()
            {
                Properties = new PropertyCollection();
            }

            /// <summary>
            /// The material index.
            /// </summary>
            public int MaterialIndex;

            /// <summary>
            /// The index buffer range. The slot in the buffer range is the position of the index buffer in <see cref="ModelData.IndexBuffer"/>s.
            /// </summary>
            public BufferRange IndexBufferRange;

            /// <summary>
            /// The vertex buffer range. The slot in the buffer range is the position of the vertex buffer in <see cref="ModelData.VertexBuffer"/>s.
            /// </summary>
            public BufferRange VertexBufferRange;

            /// <summary>
            /// Gets the offset matrix attached to each bone weights
            /// </summary>
            public Matrix[] BoneOffsetMatrices;

            /// <summary>
            /// The attributes attached to this mesh part.
            /// </summary>
            public PropertyCollection Properties;

            /// <summary>Reads or writes datas from/to the given binary serializer.</summary>
            /// <param name="serializer">The binary serializer.</param>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref MaterialIndex);
                serializer.Serialize(ref IndexBufferRange);
                serializer.Serialize(ref VertexBufferRange);
                serializer.Serialize(ref Properties);
            }
        }

        /// <summary>The buffer range struct.</summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct BufferRange : IDataSerializable
        {
            /// <summary>The slot.</summary>
            public int Slot;

            /// <summary>The start.</summary>
            public int Start;

            /// <summary>The count.</summary>
            public int Count;

            /// <summary>Reads or writes datas from/to the given binary serializer.</summary>
            /// <param name="serializer">The binary serializer.</param>
            public void Serialize(BinarySerializer serializer)
            {
                if (serializer.Mode == SerializerMode.Read)
                {
                    var reader = serializer.Reader;
                    Slot = reader.ReadInt32();
                    Start = reader.ReadInt32();
                    Count = reader.ReadInt32();
                }
                else
                {
                    var writer = serializer.Writer;
                    writer.Write(Slot);
                    writer.Write(Start);
                    writer.Write(Count);
                }
            }
        }
    }
}