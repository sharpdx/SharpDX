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
        /// <summary>
        /// Class Mesh
        /// </summary>
        public sealed class Mesh : IDataSerializable
        {
            /// <summary>
            /// Index of this mesh into the mesh collection.
            /// </summary>
            public int Index;

            /// <summary>
            /// Index of the parent bone for this mesh. The parent bone of a mesh contains a transformation matrix that describes how the mesh is located relative to any parent meshes in a model.
            /// </summary>
            public int ParentBoneIndex;

            /// <summary>
            /// Gets the name of this mesh.
            /// </summary>
            public string Name;

            /// <summary>
            /// Gets the <see cref="MeshPart"/> instances that make up this mesh. Each part of a mesh is composed of a set of primitives that share the same material. 
            /// </summary>
            public List<MeshPart> MeshParts;

            /// <summary>
            /// Gets attributes attached to this mesh.
            /// </summary>
            public List<AttributeData> Attributes;

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Index);
                serializer.Serialize(ref ParentBoneIndex);
                serializer.Serialize(ref Name, false, SerializeFlags.Nullable);
                serializer.Serialize(ref MeshParts);
                serializer.Serialize(ref Attributes);
            }
        }
    }
}