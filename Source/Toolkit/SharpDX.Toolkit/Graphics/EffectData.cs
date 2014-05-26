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
using System.Collections.Generic;
using System.IO;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Diagnostics;
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Container for shader bytecodes and effect metadata.
    /// </summary>
    /// <remarks>
    /// This class is responsible to store shader bytecodes, effect, techniques, passes...etc.
    /// It is serializable using <see cref="Load(Stream)"/> and <see cref="Save(Stream)"/> method.
    /// </remarks>
    [ContentReader(typeof(EffectDataContentReader))]
    public sealed partial class EffectData : IDataSerializable
    {
        public const string MagicCode = "TKFX";

        public const int Version = 0x101;

        public EffectData()
        {
        }

        /// <summary>
        /// List of compiled shaders.
        /// </summary>
        public List<Shader> Shaders;

        public Effect Description;

        /// <summary>
        /// Saves this <see cref="EffectData"/> instance to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            var serializer = GetSerializer(stream, SerializerMode.Write);
            serializer.Save(this);
        }

        /// <summary>
        /// Saves this <see cref="EffectData"/> instance to the specified file.
        /// </summary>
        /// <param name="fileName">The output filename.</param>
        public void Save(string  fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write))
                Save(stream);
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An <see cref="EffectData"/>. Null if the stream is not a serialized <see cref="EffectData"/>.</returns>
        /// <remarks>
        /// </remarks>
        public static EffectData Load(Stream stream)
        {
            var serializer = GetSerializer(stream, SerializerMode.Read);
            try
            {
                return serializer.Load<EffectData>();
            } catch (InvalidChunkException chunkException)
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
        /// Loads an <see cref="EffectData"/> from the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static EffectData Load(byte[] buffer)
        {
            return Load(new MemoryStream(buffer));
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified file.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static EffectData Load(string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(stream);
        }

        private static BinarySerializer GetSerializer(Stream stream, SerializerMode mode)
        {
            var serializer = new BinarySerializer(stream, mode, Text.Encoding.ASCII);
            return serializer;
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Starts the whole EffectData by the magiccode "TKFX"
            // If the serializer don't find the TKFX, It will throw an
            // exception that will be caught by Load method.
            serializer.BeginChunk(MagicCode);

            // Writes the version
            if (serializer.Mode == SerializerMode.Read)
            {
                int version = serializer.Reader.ReadInt32();
                if (version != Version)
                {
                    throw new NotSupportedException(string.Format("EffectData version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, Version));
                }
            }
            else
            {
                serializer.Writer.Write(Version);
            }

            // Shaders section
            serializer.BeginChunk("SHDR");
            serializer.Serialize(ref Shaders);
            serializer.EndChunk();

            // Effects section
            serializer.BeginChunk("EFFX");
            serializer.Serialize(ref Description);
            serializer.EndChunk();

            // Close TKFX section
            serializer.EndChunk();
        }
    }
}