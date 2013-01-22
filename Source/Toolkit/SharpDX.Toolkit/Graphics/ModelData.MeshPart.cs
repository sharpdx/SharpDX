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

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public sealed partial class ModelData
    {
        public sealed class MeshPart : IDataSerializable
        {
            /// <summary>
            /// Gets the number of indices. Can be 0.
            /// </summary>
            public int IndexCount;

            /// <summary>
            /// The index buffer. This value can be null.
            /// </summary>
            public IndexBuffer IndexBuffer;

            /// <summary>
            /// The location in the index array at which to start reading vertices.
            /// </summary>
            public int StartIndex;

            /// <summary>
            /// The number of vertices used during a draw call.
            /// </summary>
            public int VertexCount;

            /// <summary>
            /// The layout of the vertex buffer.
            /// </summary>
            public VertexBuffer VertexBuffer;

            /// <summary>
            /// Gets the offset (in vertices) from the top of vertex buffer.
            /// </summary>
            public int VertexOffset;

            /// <summary>
            /// The attributes attached to this mesh part.
            /// </summary>
            public List<AttributeData> Attributes;

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref IndexCount);
                
                // IndexBuffer is stored as a unique object inside a whole model
                serializer.AllowIdentity = true;
                serializer.Serialize(ref IndexBuffer, SerializeFlags.Nullable);
                serializer.AllowIdentity = false;

                serializer.Serialize(ref StartIndex);

                serializer.Serialize(ref VertexCount);

                // VertexBuffer is stored as a unique object inside a whole model
                serializer.AllowIdentity = true;
                serializer.Serialize(ref VertexBuffer);
                serializer.AllowIdentity = false;
                
                serializer.Serialize(ref VertexOffset);
                
                serializer.Serialize(ref Attributes);
            }
        }
    }
}