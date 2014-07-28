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

using System.Collections.Generic;
using System.ComponentModel;
using SharpDX.Mathematics;
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public sealed partial class ModelData
    {
        /// <summary>
        /// Class Mesh
        /// </summary>
        public sealed class Mesh : IDataSerializable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mesh"/> class.
            /// </summary>
            public Mesh()
            {
                VertexBuffers = new List<VertexBuffer>();
                IndexBuffers = new List<IndexBuffer>();
                MeshParts = new List<MeshPart>();
                Properties = new PropertyCollection();
            }

            /// <summary>
            /// Gets the name of this mesh.
            /// </summary>
            public string Name;

            /// <summary>
            /// Index of the parent bone for this mesh. The parent bone of a mesh contains a transformation matrix that describes how the mesh is located relative to any parent meshes in a model.
            /// </summary>
            public int ParentBoneIndex;

            /// <summary>
            /// The bounding sphere for this mesh (in local object space).
            /// </summary>
            public BoundingSphere BoundingSphere;

            /// <summary>
            /// Gets the shared vertex buffers
            /// </summary>
            public List<VertexBuffer> VertexBuffers;

            /// <summary>
            /// Gets the shared index buffers
            /// </summary>
            public List<IndexBuffer> IndexBuffers;

            /// <summary>
            /// Gets the <see cref="MeshPart"/> instances that make up this mesh. Each part of a mesh is composed of a set of primitives that share the same material. 
            /// </summary>
            public List<MeshPart> MeshParts;

            /// <summary>
            /// Gets attributes attached to this mesh.
            /// </summary>
            public PropertyCollection Properties;

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name, false, SerializeFlags.Nullable);
                serializer.Serialize(ref ParentBoneIndex);
                serializer.Serialize(ref BoundingSphere);
                serializer.Serialize(ref VertexBuffers);
                serializer.Serialize(ref IndexBuffers);
                serializer.Serialize(ref MeshParts);
                serializer.Serialize(ref Properties);
            }
        }
    }
}