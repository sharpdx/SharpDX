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

using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.Serialization;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Container for shader bytecodes and effect metadata.
    /// </summary>
    /// <remarks>
    /// This class is responsible to store shader bytecodes, effects, techniques, passes...etc.
    /// It is working like an archive and is able to store multiple effect in a single object.
    /// It is serializable using <see cref="Load(Stream)"/> and <see cref="Save(Stream)"/> method.
    /// </remarks>
    public sealed partial class EffectData : IDataSerializable
    {
        private const string MagicCode = "TKFX";

        public EffectData()
        {
            Shaders = new List<Shader>();
            Effects = new List<Effect>();
        }

        /// <summary>
        /// Gets the Source HashCode, a hashcode generated from input source file names (including all includes) and modified time for each file.
        /// </summary>
        public int HashCode;

        /// <summary>
        /// List of compiled shaders.
        /// </summary>
        public List<Shader> Shaders;

        /// <summary>
        /// List of effects.
        /// </summary>
        public List<Effect> Effects;

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


        public int FindShader(Shader shader)
        {
            for (int i = 0; i < Shaders.Count; i++)
            {
                if (Shaders[i].IsSimilar(shader))
                    return i;
            }
            return -1;
        }

        public int FindShaderByName(string name)
        {
            for (int i = 0; i < Shaders.Count; i++)
            {
                if (Shaders[i].Name == name)
                    return i;
            }
            return -1;

        }

        /// <summary>
        /// Merges an existing <see cref="EffectData"/> into this instance.
        /// </summary>
        /// <param name="source">The EffectData to merge.</param>
        /// <exception cref="InvalidOperationException">If the merge failed.</exception>
        /// <remarks>
        /// This method is useful to build an archive of several effects.
        /// </remarks>
        public void MergeFrom(EffectData source)
        {
            var logger = new Logger();
            if (!MergeFrom(source, logger))
                throw new InvalidOperationException(Utilities.Join("\r\n",logger.Messages));
        }

        /// <summary>
        /// Merges an existing <see cref="EffectData"/> into this instance.
        /// </summary>
        /// <param name="source">The EffectData to merge.</param>
        /// <param name="logger">Logger used to report merging errors.</param>
        /// <remarks>
        /// This method is useful to build an archive of several effects.
        /// </remarks>
        public bool MergeFrom(EffectData source, Logger logger)
        {
            bool isMergeOk = true;

            // Clear the HashCode as it doesn't mean anything
            HashCode = 0;

            foreach (var effect in source.Effects)
            {
                bool skipEffect = false;

                // Add effect that is not already in the archive with this same name.
                foreach (var effect2 in Effects)
                {
                    if (effect2.Name == effect.Name)
                    {
                        skipEffect = true;
                        break;
                    }
                }

                if (skipEffect)
                    continue;
                
                Effects.Add(effect);

                foreach (var technique in effect.Techniques)
                {
                    foreach (var pass in technique.Passes)
                    {
                        foreach (var shaderLink in pass.Pipeline)
                        {
                            if (shaderLink == null)
                                continue;

                            if (shaderLink.IsImport)
                            {
                                // If this is an import, we try first to resolve it directly
                                // Else we keep the name as-is
                                var index = FindShaderByName(shaderLink.ImportName);
                                if (index >= 0)
                                {
                                    shaderLink.ImportName = null;
                                    shaderLink.Index = index;
                                }
                            }
                            else if (!shaderLink.IsNullShader)
                            {
                                var shader = source.Shaders[shaderLink.Index];
                                var index = FindShader(shader);
                                if (index >= 0)
                                {
                                    var previousShader = Shaders[index];

                                    if (shader.Name != null)
                                    {
                                        // if shader from this instance is local and shader from source is global => transform current shader to global
                                        if (previousShader.Name == null)
                                        {
                                            previousShader.Name = shader.Name;
                                        }
                                        else if (shader.Name != previousShader.Name)
                                        {
                                            // If shader from this instance is global and shader from source is global => check names. If exported names are different, this is an error
                                            logger.Error("Cannot merge shader [{0}] into this instance, as there is already a global shader with a different name [{1}]", shader.Name, previousShader.Name);
                                            isMergeOk = false;
                                        }
                                    }

                                    shaderLink.Index = index;
                                }
                                else
                                {
                                    shaderLink.Index = Shaders.Count;
                                    Shaders.Add(shader);
                                }
                            }
                        }
                    }
                }
            }

            return isMergeOk;
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
        /// Get the HashCode from the binary effect, without loading the full effect in memory.
        /// </summary>
        /// <param name="stream">The effect binary stream</param>
        /// <returns>The hashcode stored in the binary effect.</returns>
        public static int GetHashCode(Stream stream)
        {
            int hash = 0;
            var dataReader = new BinaryReader(stream);
            var savePosition = stream.Position;
            if ((stream.Length - stream.Position) > 12)
            {
                var magicCode = (FourCC)dataReader.ReadInt32();
                if (magicCode == MagicCode)
                {
                    dataReader.ReadInt32();
                    hash = dataReader.ReadInt32();
                }
            }
            stream.Position = savePosition;
            return hash;
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
            serializer.RegisterDynamic<Vector4>();
            serializer.RegisterDynamic<Vector3>();
            serializer.RegisterDynamic<Vector2>();
            return serializer;
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Starts the whole EffectData by the magiccode "TKFX"
            // If the serializer don't find the TKFX, It will throw an
            // exception that will be catched by Load method.
            serializer.BeginChunk(MagicCode);

            // Serialize the HashCode
            serializer.Serialize(ref HashCode);

            // Shaders section
            serializer.BeginChunk("SHDR");
            serializer.Serialize(ref Shaders);
            serializer.EndChunk();

            // Effects section
            serializer.BeginChunk("EFFX");
            serializer.Serialize(ref Effects);
            serializer.EndChunk();

            // Close TKFX section
            serializer.EndChunk();
        }
    }
}