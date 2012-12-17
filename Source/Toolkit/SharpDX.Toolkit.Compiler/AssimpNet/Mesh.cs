/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// A mesh represents geometry with a single material.
    /// </summary>
    internal sealed class Mesh {
        private String _name;
        private PrimitiveType _primitiveType;
        private int _materialIndex;
        private int _vertexCount;
        private Vector3D[] _vertices;
        private Vector3D[] _normals;
        private Vector3D[] _tangents;
        private Vector3D[] _bitangents;
        private Face[] _faces;
        private List<Color4D[]> _colors;
        private List<Vector3D[]> _texCoords;
        private List<uint> _texComponentNumber;
        private Bone[] _bones;
        private MeshAnimationAttachment[] _meshAttachments;

        /// <summary>
        /// Gets the mesh name. This tends to be used
        /// when formats name nodes and meshes independently,
        /// vertex animations refer to meshes by their names,
        /// or importers split meshes up, each mesh will reference
        /// the same (dummy) name.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the primitive type. This may contain more than one
        /// type unless if <see cref="PostProcessSteps.SortByPrimitiveType"/>
        /// option is not set.
        /// </summary>
        public PrimitiveType PrimitiveType {
            get {
                return _primitiveType;
            }
        }

        /// <summary>
        /// Gets the index of the material associated with this mesh.
        /// </summary>
        public int MaterialIndex {
            get {
                return _materialIndex;
            }
        }

        /// <summary>
        /// Gets the number of vertices in this mesh. This is also
        /// the size for all per-vertex data arays.
        /// </summary>
        public int VertexCount {
            get {
                return _vertexCount;
            }
        }

        /// <summary>
        /// Gets if the mesh has a vertex array. This should always return
        /// true provided no special scene flags are set.
        /// </summary>
        public bool HasVertices {
            get {
                return _vertices != null;
            }
        }

        /// <summary>
        /// Gets the vertex position array.
        /// </summary>
        public Vector3D[] Vertices {
            get {
                return _vertices;
            }
        }

        /// <summary>
        /// Gets if the mesh as normals.
        /// </summary>
        public bool HasNormals {
            get {
                return _normals != null;
            }
        }

        /// <summary>
        /// Gets the vertex normal array.
        /// </summary>
        public Vector3D[] Normals {
            get {
                return _normals;
            }
        }

        /// <summary>
        /// Gets if the mesh has tangents and bitangents. It is not
        /// possible for one to be without the other.
        /// </summary>
        public bool HasTangentBasis {
            get {
                return VertexCount > 0 && _tangents != null &&_bitangents != null;
            }
        }

        /// <summary>
        /// Gets the vertex tangent array.
        /// </summary>
        public Vector3D[] Tangents {
            get {
                return _tangents;
            }
        }

        /// <summary>
        /// Gets the vertex bitangent array.
        /// </summary>
        public Vector3D[] BiTangents {
            get {
                return _bitangents;
            }
        }

        /// <summary>
        /// Gets the number of faces contained in the mesh.
        /// </summary>
        public int FaceCount {
            get {
                return (_faces == null) ? 0 : _faces.Length;
            }
        }

        /// <summary>
        /// Gets if the mesh contains faces. If no special
        /// scene flags are set, this should always return true.
        /// </summary>
        public bool HasFaces {
            get {
                return _faces != null;
            }
        }

        /// <summary>
        /// Gets the mesh's faces. Each face will contain indices
        /// to the vertices.
        /// </summary>
        public Face[] Faces {
            get {
                return _faces;
            }
        }

        /// <summary>
        /// Gets the number of valid vertex color channels contained in the
        /// mesh. This can be a value between zero and the maximum vertex color count.
        /// </summary>
        public int VertexColorChannelCount {
            get {
                return (_colors == null) ? 0 : _colors.Count;
            }
        }

        /// <summary>
        /// Gets the number of valid texture coordinate channels contained
        /// in the mesh. This can be a value between zero and the maximum texture coordinate count.
        /// </summary>
        public int TextureCoordsChannelCount {
            get {
                return (_texCoords == null) ? 0 : _texCoords.Count;
            }
        }

        /// <summary>
        /// Gets the number of bones that influence this mesh.
        /// </summary>
        public int BoneCount {
            get {
                return (_bones == null) ? 0 : _bones.Length;
            }
        }

        /// <summary>
        /// Gets if this mesh has bones.
        /// </summary>
        public bool HasBones {
            get {
                return _bones != null;
            }
        }

        /// <summary>
        /// Gets the bones that influence this mesh.
        /// </summary>
        public Bone[] Bones {
            get {
                return _bones;
            }
        }

        /// <summary>
        /// Constructs a new Mesh.
        /// </summary>
        /// <param name="mesh">Unmanaged AiMesh struct.</param>
        internal Mesh(AiMesh mesh) {
            _name = mesh.Name.GetString();
            _primitiveType = mesh.PrimitiveTypes;
            _vertexCount = (int) mesh.NumVertices;
            _materialIndex = (int) mesh.MaterialIndex;

            //Load per-vertex arrays
            if(mesh.NumVertices > 0) {
                if(mesh.Vertices != IntPtr.Zero) {
                    _vertices = MemoryHelper.MarshalArray<Vector3D>(mesh.Vertices, _vertexCount);
                }
                if(mesh.Normals != IntPtr.Zero) {
                    _normals = MemoryHelper.MarshalArray<Vector3D>(mesh.Normals, _vertexCount);
                }
                if(mesh.Tangents != IntPtr.Zero) {
                    _tangents = MemoryHelper.MarshalArray<Vector3D>(mesh.Tangents, _vertexCount);
                }
                if(mesh.BiTangents != IntPtr.Zero) {
                    _bitangents = MemoryHelper.MarshalArray<Vector3D>(mesh.BiTangents, _vertexCount);
                }
            }

            //Load faces
            if(mesh.NumFaces > 0 && mesh.Faces != IntPtr.Zero) {
                AiFace[] faces = MemoryHelper.MarshalArray<AiFace>(mesh.Faces, (int) mesh.NumFaces);
                _faces = new Face[faces.Length];
                for(int i = 0; i < _faces.Length; i++) {
                    _faces[i] = new Face(faces[i]);
                }
            }

            //Load UVW components - this should match the texture coordinate channels
            uint[] components = mesh.NumUVComponents;
            if(components != null) {
                _texComponentNumber = new List<uint>();
                foreach(uint num in components) {
                    if(num > 0) {
                        _texComponentNumber.Add(num);
                    }
                }
            }

            //Load texture coordinate channels
            IntPtr[] texCoords = mesh.TextureCoords;
            if(texCoords != null) {
                _texCoords = new List<Vector3D[]>();
                foreach(IntPtr texPtr in texCoords) {
                    if(texPtr != IntPtr.Zero) {
                        _texCoords.Add(MemoryHelper.MarshalArray<Vector3D>(texPtr, _vertexCount));
                    }
                }
            }

            //Load vertex color channels
            IntPtr[] vertexColors = mesh.Colors;
            if(vertexColors != null) {
                _colors = new List<Color4D[]>();
                foreach(IntPtr colorPtr in vertexColors) {
                    if(colorPtr != IntPtr.Zero) {
                        _colors.Add(MemoryHelper.MarshalArray<Color4D>(colorPtr, _vertexCount));
                    }
                }
            }

            //Load bones
            if(mesh.NumBones > 0 && mesh.Bones != IntPtr.Zero) {
                AiBone[] bones = MemoryHelper.MarshalArray<AiBone>(mesh.Bones, (int) mesh.NumBones, true);
                _bones = new Bone[bones.Length];
                for(int i = 0; i < _bones.Length; i++) {
                    _bones[i] = new Bone(bones[i]);
                }
            }

            //Load anim meshes (attachment meshes)
            if(mesh.NumAnimMeshes > 0 && mesh.AnimMeshes != IntPtr.Zero) {
                AiAnimMesh[] animMeshes = MemoryHelper.MarshalArray<AiAnimMesh>(mesh.AnimMeshes, (int) mesh.NumAnimMeshes, true);
                _meshAttachments = new MeshAnimationAttachment[animMeshes.Length];
                for(int i = 0; i < _meshAttachments.Length; i++) {
                    _meshAttachments[i] = new MeshAnimationAttachment(animMeshes[i]);
                }
            }
        }

        /// <summary>
        /// Checks if the mesh has vertex colors for the specified channel. If
        /// this returns true, you can be confident that the channel contains
        /// the same number of vertex colors as there are vertices in this mesh.
        /// </summary>
        /// <param name="channelIndex">Channel index</param>
        /// <returns>True if vertex colors are present in the channel.</returns>
        public bool HasVertexColors(int channelIndex) {
            if(_colors != null) {
                if(channelIndex >= _colors.Count || channelIndex < 0) {
                    return false;
                }
                return VertexCount > 0 && _colors[channelIndex] != null;
            }
            return false;
        }

        /// <summary>
        /// Checks if the mesh has texture coordinates for the specified channel.
        /// If this returns true, you can be confident that the channel contains the same
        /// number of texture coordinates as there are vertices in this mesh.
        /// </summary>
        /// <param name="channelIndex">Channel index</param>
        /// <returns>True if texture coordinates are present in the channel.</returns>
        public bool HasTextureCoords(int channelIndex) {
            if(_texCoords != null) {
                if(channelIndex >= _texCoords.Count || channelIndex < 0) {
                    return false;
                }
                return VertexCount > 0 && _texCoords[channelIndex] != null;
            }
            return false;
        }

        /// <summary>
        /// Gets the array of vertex colors from the specified vertex color channel.
        /// </summary>
        /// <param name="channelIndex">Channel index</param>
        /// <returns>The vertex color array, or null if it does not exist.</returns>
        public Color4D[] GetVertexColors(int channelIndex) {
            if(HasVertexColors(channelIndex)) {
                return _colors[channelIndex];
            }
            return null;
        }

        /// <summary>
        /// Gets the array of texture coordinates from the specified texture coordinate
        /// channel.
        /// </summary>
        /// <param name="channelIndex">Channel index</param>
        /// <returns>The texture coordinate array, or null if it does not exist.</returns>
        public Vector3D[] GetTextureCoords(int channelIndex) {
            if(HasTextureCoords(channelIndex)) {
                return _texCoords[channelIndex];
            }
            return null;
        }

        /// <summary>
        /// Gets the number of UV(W) components for the texture coordinate channel, this
        /// usually either 2 (UV) or 3 (UVW). No components mean the texture coordinate channel
        /// does not exist. The channel index matches the texture coordinate channel index.
        /// </summary>
        /// <param name="channelIndex">Channel index</param>
        /// <returns>The number of UV(W) components the texture coordinate channel contains</returns>
        public int GetUVComponentCount(int channelIndex) {
            if(HasTextureCoords(channelIndex)) {
                if(_texComponentNumber != null) {
                    return (int)_texComponentNumber[channelIndex];
                }
            }
            return 0;
        }

        /// <summary>
        /// Convienence method for accumulating all face indices into a single
        /// index array.
        /// </summary>
        /// <returns>uint index array</returns>
        public uint[] GetIndices() {
            if(HasFaces) {
                List<uint> indices = new List<uint>();
                foreach(Face face in _faces) {
                    if(face.IndexCount > 0 && face.Indices != null) {
                        indices.AddRange(face.Indices);
                    }
                }
                return indices.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Convienence method for accumulating all face indices into a single
        /// index array.
        /// </summary>
        /// <returns>int index array</returns>
        public int[] GetIntIndices() {
            //We could use a dirty hack here to do a conversion...but may as well be
            //safe just in case
            if(HasFaces) {
                List<int> indices = new List<int>();
                foreach(Face face in _faces) {
                    if(face.IndexCount > 0 && face.Indices != null) {
                        foreach(uint index in face.Indices) {
                            indices.Add((int) index);
                        }
                    }
                }
                return indices.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Convienence method for accumulating all face indices into a single
        /// index array.
        /// </summary>
        /// <returns>short index array</returns>
        public short[] GetShortIndices() {
            //We could use a dirty hack here to do a conversion...but may as well be
            //safe just in case
            if(HasFaces) {
                List<short> indices = new List<short>();
                foreach(Face face in _faces) {
                    if(face.IndexCount > 0 && face.Indices != null) {
                        foreach(uint index in face.Indices) {
                            indices.Add((short) index);
                        }
                    }
                }
                return indices.ToArray();
            }
            return null;
        }
    }
}
