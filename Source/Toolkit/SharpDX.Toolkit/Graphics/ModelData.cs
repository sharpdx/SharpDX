﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;
using System.IO;

using SharpDX.IO;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// The model data used to store 3D mesh model.
    /// </summary>
    public sealed partial class ModelData : CommonData, IDataSerializable
    {
        public const string MagicCode = "TKMD";

        public const int Version = 0x100;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelData" /> class.
        /// </summary>
        public ModelData()
        {
            Textures = new List<byte[]>();
            Materials = new List<Material>();
            Bones = new List<Bone>();
            // DISABLE_SKINNED_BONES
            //SkinnedBones = new List<Bone>();
            Meshes = new List<Mesh>();
            Attributes = new PropertyCollection();
        }

        /// <summary>
        /// Gets the maximum buffer size in bytes that will be needed when loading this model.
        /// </summary>
        public int MaximumBufferSizeInBytes;

        /// <summary>
        /// Embedded textures.
        /// </summary>
        public List<byte[]> Textures;

        /// <summary>
        /// Gets the material of this model.
        /// </summary>
        public List<Material> Materials;

        /// <summary>
        /// Gets the bones of this model.
        /// </summary>
        public List<Bone> Bones;

        // DISABLE_SKINNED_BONES
        ///// <summary>
        ///// Gets the bones used to perform skinning animation with this model.
        ///// </summary>
        //public List<Bone> SkinnedBones;

        /// <summary>
        /// Gets the mesh of this model.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// Gets the attributes attached to this instance.
        /// </summary>
        public PropertyCollection Attributes;

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
            serializer.ArrayLengthType = ArrayLengthType.Int;
            serializer.RegisterDynamicList<MaterialTexture>("MATL");
            return serializer;
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Starts the whole ModelData by the magiccode "TKMD"
            // If the serializer don't find the TKMD, It will throw an
            // exception that will be caught by Load method.

            // This code should not be modified without modifying the serialize code in Model.

            serializer.BeginChunk(MagicCode);

            // Writes the version
            if (serializer.Mode == SerializerMode.Read)
            {
                int version = serializer.Reader.ReadInt32();
                if (version != Version)
                {
                    throw new NotSupportedException(string.Format("ModelData version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, Version));
                }
            }
            else
            {
                serializer.Writer.Write(Version);
            }

            // Serialize the maximum buffer size used when loading this model.
            serializer.Serialize(ref MaximumBufferSizeInBytes);

            // Texture section
            serializer.BeginChunk("TEXS");
            if (serializer.Mode == SerializerMode.Read)
            {
                int textureCount = serializer.Reader.ReadInt32();
                Textures = new List<byte[]>(textureCount);
                for (int i = 0; i < textureCount; i++)
                {
                    byte[] textureData = null;
                    serializer.Serialize(ref textureData);
                    Textures.Add(textureData);
                }
            }
            else
            {
                serializer.Writer.Write(Textures.Count);
                for (int i = 0; i < Textures.Count; i++)
                {
                    byte[] textureData = Textures[i];
                    serializer.Serialize(ref textureData);
                }
            }
            serializer.EndChunk();

            // Material section
            serializer.BeginChunk("MATL");
            serializer.Serialize(ref Materials);
            serializer.EndChunk();

            // Bones section
            serializer.BeginChunk("BONE");
            serializer.Serialize(ref Bones);
            serializer.EndChunk();

            //// DISABLE_SKINNED_BONES
            //// Skinned Bones section
            //serializer.BeginChunk("SKIN");
            //serializer.Serialize(ref SkinnedBones);
            //serializer.EndChunk();

            // Mesh section
            serializer.BeginChunk("MESH");
            serializer.Serialize(ref Meshes);
            serializer.EndChunk();

            // Serialize attributes
            serializer.Serialize(ref Attributes);

            // Close TKMD section
            serializer.EndChunk();
        }
    }
}