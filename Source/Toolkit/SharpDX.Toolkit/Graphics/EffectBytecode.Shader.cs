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

using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectBytecode
    {
        /// <summary>
        /// Describes a shader and associated bytecode.
        /// </summary>
        public sealed class Shader : IDataSerializable
        {
            /// <summary>
            /// Name of this shader, only valid for public shaders, else null.
            /// </summary>
            public string Name;

            /// <summary>
            /// Type of this shader.
            /// </summary>
            public EffectShaderType Type;

            /// <summary>
            /// Compiler flags used to compile this shader.
            /// </summary>
            public EffectCompilerFlags CompilerFlags;

            /// <summary>
            /// Level of this shader.
            /// </summary>
            public FeatureLevel Level;

            /// <summary>
            /// Bytecode of this shader.
            /// </summary>
            public byte[] Bytecode;

            /// <summary>
            /// Hashcode from the bytecode.
            /// </summary>
            /// <remarks>
            /// Shaders with same bytecode with have same hashcode.
            /// </remarks>
            public int Hashcode;

            /// <summary>
            /// Description of the input <see cref="Signature"/>.
            /// </summary>
            public Signature InputSignature;

            /// <summary>
            /// Description of the output <see cref="Signature"/>.
            /// </summary>
            public Signature OutputSignature;

            /// <summary>
            /// List of constant buffers used by this shader.
            /// </summary>
            public List<ConstantBuffer> ConstantBuffers;

            /// <summary>
            /// List of resource parameters used by this shader.
            /// </summary>
            public List<ResourceParameter> ResourceParameters;

            public override string ToString()
            {
                return string.Format("{0}Type: {1} {2}", Name == null ? string.Empty : string.Format("Name: {0},", Name), Type, Level);
            }

            /// <summary>
            /// Check if this instance is similar to another Shader.
            /// </summary>
            /// <param name="other">The other instance to check against.</param>
            /// <returns>True if this instance is similar, false otherwise.</returns>
            /// <remarks>
            /// Except the name, all fields are checked for deep equality.
            /// </remarks>
            public bool IsSimilar(Shader other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                if (!(Hashcode == other.Hashcode && Type.Equals(other.Type) && CompilerFlags.Equals(other.CompilerFlags) && Level.Equals(other.Level) && InputSignature.Equals(other.InputSignature) && OutputSignature.Equals(other.OutputSignature)))
                    return false;

                if (!Utilities.Compare(Bytecode, other.Bytecode))
                    return false;

                if (!Utilities.Compare(ConstantBuffers, other.ConstantBuffers))
                    return false;

                if (!Utilities.Compare(ResourceParameters, other.ResourceParameters))
                    return false;

                // Shaders are similar
                return true;
            }

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.AllowNull = true;
                serializer.Serialize(ref Name);
                serializer.AllowNull = false;

                serializer.SerializeEnum(ref Type);
                serializer.SerializeEnum(ref CompilerFlags);
                serializer.SerializeEnum(ref Level);
                serializer.Serialize(ref Bytecode);
                serializer.Serialize(ref Hashcode);
                serializer.Serialize(ref InputSignature);
                serializer.Serialize(ref OutputSignature);
                serializer.Serialize(ref ConstantBuffers);
                serializer.Serialize(ref ResourceParameters);
            }
        }
    }
}