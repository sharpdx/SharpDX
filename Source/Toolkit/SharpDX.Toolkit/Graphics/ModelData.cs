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
using System.IO;

using SharpDX.IO;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// The model data used to store 3D mesh model.
    /// </summary>
    public sealed partial class ModelData : IDataSerializable
    {
        private const string MagicCode = "TKMD";

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelData" /> class.
        /// </summary>
        public ModelData()
        {
            Bones = new List<Node>();
            SkinnedBones = new List<Node>();
            Meshes = new List<Mesh>();
            Materials = new List<Material>();
            Attributes = new List<AttributeData>();
        }

        /// <summary>
        /// Gets the bones of this model.
        /// </summary>
        public List<Node> Bones;

        /// <summary>
        /// Gets the bones used to perform skinning animation with this model.
        /// </summary>
        public List<Node> SkinnedBones;

        /// <summary>
        /// Gets the mesh of this model.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// Gets the material of this model.
        /// </summary>
        public List<Material> Materials;

        /// <summary>
        /// Gets the attributes attached to this instance.
        /// </summary>
        public List<AttributeData> Attributes;

        /// <summary>
        /// Loads a <see cref="ModelData"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A <see cref="ModelData"/>. Null if the stream is not a serialized <see cref="ModelData"/>.</returns>
        /// <remarks>
        /// </remarks>
        public static ModelData Load(Stream stream)
        {
            var serializer = GetSerializer(stream, SerializerMode.Read);
            try
            {
                return serializer.Load<ModelData>();
            }
            catch (InvalidChunkException chunkException)
            {
                // If we have an exception of the magiccode, just return null
                if (chunkException.ExpectedChunkId == MagicCode)
                {
                    return null;
                }
                throw;
            }
        }

        /// <summary>
        /// Loads a <see cref="ModelData"/> from the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>A <see cref="ModelData"/> </returns>
        public static ModelData Load(byte[] buffer)
        {
            return Load(new MemoryStream(buffer));
        }

        /// <summary>
        /// Loads an <see cref="ModelData"/> from the specified file.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>A <see cref="ModelData"/> </returns>
        public static ModelData Load(string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(stream);
        }

        /// <summary>
        /// Saves this <see cref="ModelData"/> instance to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            var serializer = GetSerializer(stream, SerializerMode.Write);
            serializer.Save(this);
        }

        /// <summary>
        /// Saves this <see cref="ModelData"/> instance to the specified file.
        /// </summary>
        /// <param name="fileName">The output filename.</param>
        public void Save(string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write))
                Save(stream);
        }

        private static BinarySerializer GetSerializer(Stream stream, SerializerMode mode)
        {
            var serializer = new BinarySerializer(stream, mode, Text.Encoding.ASCII);
            serializer.RegisterDynamic<Color4>();
            serializer.RegisterDynamic<Color3>();
            serializer.RegisterDynamic<Color>();
            serializer.RegisterDynamic<Vector4>();
            serializer.RegisterDynamic<Vector3>();
            serializer.RegisterDynamic<Vector2>();
            return serializer;
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Starts the whole ModelData by the magiccode "TKMD"
            // If the serializer don't find the TKMD, It will throw an
            // exception that will be catched by Load method.
            serializer.BeginChunk(MagicCode);

            // Bones section
            serializer.BeginChunk("BONE");
            serializer.Serialize(ref Bones);
            serializer.EndChunk();

            // Skinned Bones section
            serializer.BeginChunk("SKIN");
            serializer.Serialize(ref SkinnedBones);
            serializer.EndChunk();

            // Mesh section
            serializer.BeginChunk("MESH");
            serializer.Serialize(ref Meshes);
            serializer.EndChunk();

            // Material section
            serializer.BeginChunk("MATE");
            serializer.Serialize(ref Materials);
            serializer.EndChunk();

            // Serialize attributes
            serializer.Serialize(ref Attributes);

            // Close TKFX section
            serializer.EndChunk();
        }
    }
}