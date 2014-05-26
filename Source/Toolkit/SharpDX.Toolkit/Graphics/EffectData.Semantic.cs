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
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public sealed partial class EffectData
    {
        /// <summary>	
        /// <p>Describes a semantic signature.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>A shader can take n inputs and can produce m outputs. The order of the input (or output) parameters, their associated types, and any attached semantics make up the shader signature. Each shader has an input and an output signature.</p><p>When compiling a shader or an effect, some API calls validate shader signatures  That is, they compare the output signature of one shader (like a vertex shader) with the input signature of another shader (like a pixel shader). This ensures that a shader outputs data that is compatible with a downstream shader that is consuming that data. Compatible means that a shader signature is a exact-match subset of the preceding shader stage. Exact match means parameter types and semantics must exactly match. Subset means that a parameter that is not required by a downstream stage, does not need to include that parameter in its shader signature.</p><p>Get a shader-signature from a shader or an effect by calling APIs such as <strong><see cref="SharpDX.D3DCompiler.ShaderReflection.GetInputParameterDescription"/></strong>. </p>	
        /// </remarks>	
        /// <msdn-id>ff476215</msdn-id>	
        /// <unmanaged>D3D11_SIGNATURE_PARAMETER_DESC</unmanaged>	
        /// <unmanaged-short>D3D11_SIGNATURE_PARAMETER_DESC</unmanaged-short>	
        public sealed class Semantic : IDataSerializable, IEquatable<Semantic>
        {
            internal string name;
            internal byte index;
            internal byte register;
            internal byte systemValueType;
            internal byte componentType;
            internal byte usageMask;
            internal byte readWriteMask;
            internal byte stream;

            /// <summary>
            /// Semantic
            /// </summary>
            public Semantic()
            {
            }

            /// <summary>
            /// Initializes a new instance of <see cref="Semantic"/> class.
            /// </summary>
            /// <param name="name">Name of the semantic.</param>
            /// <param name="index">Index of the semantic.</param>
            /// <param name="register">Register.</param>
            /// <param name="systemValueType">A predefined string that determines the functionality of certain pipeline stages.</param>
            /// <param name="componentType">The per-component-data type that is stored in a register.</param>
            /// <param name="usageMask">Mask which indicates which components of a register are used.</param>
            /// <param name="readWriteMask">Mask which indicates whether a given component is never written (if the signature is an output signature) or always read (if the signature is an input signature).</param>
            /// <param name="stream">Indicates which stream the geometry shader is using for the signature parameter.</param>
            public Semantic(string name, byte index, byte register, byte systemValueType, byte componentType, byte usageMask, byte readWriteMask, byte stream)
            {
                this.name = name;
                this.index = index;
                this.register = register;
                this.systemValueType = systemValueType;
                this.componentType = componentType;
                this.usageMask = usageMask;
                this.readWriteMask = readWriteMask;
                this.stream = stream;
            }

            /// <summary>	
            /// <dd> <p>A per-parameter string that identifies how the data will be used. </p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>const char* SemanticName</unmanaged>	
            /// <unmanaged-short>char SemanticName</unmanaged-short>	
            public string Name
            {
                get { return name; }
            }

            /// <summary>	
            /// <dd> <p>Semantic index that modifies the semantic. Used to differentiate different parameters that use the same semantic.</p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>unsigned int SemanticIndex</unmanaged>	
            /// <unmanaged-short>unsigned int SemanticIndex</unmanaged-short>	
            public byte Index
            {
                get { return index; }
            }

            /// <summary>	
            /// <dd> <p>The register that will contain this variable's data.</p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>unsigned int Register</unmanaged>	
            /// <unmanaged-short>unsigned int Register</unmanaged-short>	
            public byte Register
            {
                get { return register; }
            }

            /// <summary>	
            /// <dd> <p>A predefined string that determines the functionality of certain pipeline stages. See <strong>D3D10_NAME</strong>.</p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>D3D_NAME SystemValueType</unmanaged>	
            /// <unmanaged-short>D3D_NAME SystemValueType</unmanaged-short>	
            public byte SystemValueType
            {
                get { return systemValueType; }
            }

            /// <summary>	
            /// <dd> <p>The per-component-data type that is stored in a register.  See <strong>D3D10_REGISTER_COMPONENT_TYPE</strong>. Each register can store up to four-components of data.</p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>D3D_REGISTER_COMPONENT_TYPE ComponentType</unmanaged>	
            /// <unmanaged-short>D3D_REGISTER_COMPONENT_TYPE ComponentType</unmanaged-short>	
            public byte ComponentType
            {
                get { return componentType; }
            }

            /// <summary>	
            /// <dd> <p>Mask which indicates which components of a register are used.</p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_FLAG Mask</unmanaged>	
            /// <unmanaged-short>D3D11_REGISTER_COMPONENT_MASK_FLAG Mask</unmanaged-short>	
            public byte UsageMask
            {
                get { return usageMask; }
            }

            /// <summary>	
            /// <dd> <p>Mask which indicates whether a given component is never written (if the signature is an output signature) or always read (if the signature is an input signature). </p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_FLAG ReadWriteMask</unmanaged>	
            /// <unmanaged-short>D3D11_REGISTER_COMPONENT_MASK_FLAG ReadWriteMask</unmanaged-short>	
            public byte ReadWriteMask
            {
                get { return readWriteMask; }
            }

            /// <summary>	
            /// <dd> <p>Indicates which stream the geometry shader is using for the signature parameter.</p> </dd>	
            /// </summary>	
            /// <msdn-id>ff476215</msdn-id>	
            /// <unmanaged>unsigned int Stream</unmanaged>	
            /// <unmanaged-short>unsigned int Stream</unmanaged-short>	
            public byte Stream
            {
                get { return stream; }
            }

            public bool Equals(Semantic other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && Index == other.Index && Register == other.Register && SystemValueType == other.SystemValueType && ComponentType == other.ComponentType && UsageMask == other.UsageMask && ReadWriteMask == other.ReadWriteMask && Stream == other.Stream;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Semantic) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = Name.GetHashCode();
                    hashCode = (hashCode * 397) ^ Index.GetHashCode();
                    hashCode = (hashCode * 397) ^ Register.GetHashCode();
                    hashCode = (hashCode * 397) ^ SystemValueType.GetHashCode();
                    hashCode = (hashCode * 397) ^ ComponentType.GetHashCode();
                    hashCode = (hashCode * 397) ^ UsageMask.GetHashCode();
                    hashCode = (hashCode * 397) ^ ReadWriteMask.GetHashCode();
                    hashCode = (hashCode * 397) ^ Stream.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(Semantic left, Semantic right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Semantic left, Semantic right)
            {
                return !Equals(left, right);
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref name);
                serializer.Serialize(ref index);
                serializer.Serialize(ref register);
                serializer.Serialize(ref systemValueType);
                serializer.Serialize(ref componentType);
                serializer.Serialize(ref usageMask);
                serializer.Serialize(ref readWriteMask);
                serializer.Serialize(ref stream);
            }
        }
    }
}