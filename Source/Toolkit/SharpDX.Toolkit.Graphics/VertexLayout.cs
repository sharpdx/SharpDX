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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// VertexLayout is equivalent to <see cref="SharpDX.Direct3D11.InputLayout"/>.
    /// </summary>
    /// <msdn-id>ff476588</msdn-id>	
    /// <unmanaged>ID3D11InputLayout</unmanaged>	
    /// <unmanaged-short>ID3D11InputLayout</unmanaged-short>	
    public class VertexLayout : GraphicsResource
    {
        /// <summary>
        /// Gets the bytecode for this declaration.
        /// </summary>
        public readonly byte[] ShaderBytecode;

        /// <summary>
        /// Gets the description of vertex declaration.
        /// </summary>
        public readonly VertexDeclaration[] VertexDeclarations;

        private static readonly Dictionary<VertexDeclarationSet, VertexLayout> VertexLayoutCache = new Dictionary<VertexDeclarationSet, VertexLayout>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexLayout" /> class.
        /// </summary>
        /// <param name="descriptions">The description.</param>
        private VertexLayout(VertexDeclarationSet descriptions)
            : this(GraphicsDevice.CurrentSafe, descriptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexLayout" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="descriptions">The description.</param>
        private VertexLayout(GraphicsDevice deviceLocal, VertexDeclarationSet descriptions)
        {
            ShaderBytecode = descriptions.ShaderBytecode;
            VertexDeclarations = descriptions.VertexDeclarations;
            Initialize(deviceLocal, new InputLayout(deviceLocal, ShaderBytecode, ToInputElements(descriptions.VertexDeclarations)));
        }

        /// <summary>	
        /// <p>Create a sampler-state object that encapsulates sampling information for a texture.</p>	
        /// </summary>
        /// <param name="vertexShaderBytecode">The  </param>
        /// <param name="description">A sampler state description</param>	
        /// <returns>A new <see cref="VertexLayout"/> instance</returns>	
        /// <remarks>	
        /// <p>4096 unique sampler state objects can be created on a device at a time.</p><p>If an application attempts to create a sampler-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique sampler state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476518</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateVertexLayout([In] const D3D11_SAMPLER_DESC* pSamplerDesc,[Out, Fast] ID3D11VertexLayout** ppVertexLayout)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateVertexLayout</unmanaged-short>	
        public static VertexLayout New(byte[] vertexShaderBytecode, params VertexDeclaration[] description)
        {
            if (description == null)
                throw new ArgumentNullException("description");

            var internalDesc = new VertexDeclarationSet(vertexShaderBytecode, description);

            VertexLayout vertexLayout;
            lock (VertexLayoutCache)
            {
                if (!VertexLayoutCache.TryGetValue(internalDesc, out vertexLayout))
                {
                    vertexLayout = new VertexLayout(internalDesc);
                    VertexLayoutCache[internalDesc] = vertexLayout;
                }
            }
            return vertexLayout;
        }
        
        protected override DeviceChild CreateResource()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator Direct3D11.InputLayout(VertexLayout from)
        {
            return (Direct3D11.InputLayout)(from == null ? null : from.GetOrCreateResource());
        }

        public static InputElement[] ToInputElements(params VertexDeclaration[] descriptions)
        {
            if (descriptions == null)
                throw new ArgumentNullException("description");

            var inputElements = new List<InputElement>();
            foreach (var vertexDesc in descriptions)
            {
                foreach (var vertexElement in vertexDesc.VertexElements)
                {
                    inputElements.Add(new InputElement()
                                          {
                                              SemanticName = vertexElement.SemanticName,
                                              SemanticIndex = vertexElement.SemanticIndex,
                                              Format = vertexElement.Format,
                                              AlignedByteOffset = vertexElement.AlignedByteOffset,
                                              Slot = vertexDesc.SlotIndex,
                                              Classification = vertexDesc.VertexClassification,
                                              InstanceDataStepRate = vertexDesc.InstanceDataStepRate,
                                          }
                        );
                }
            }
            return inputElements.ToArray();
        }

        class VertexDeclarationSet : IEquatable<VertexDeclarationSet>
        {
            public VertexDeclarationSet(byte[] shaderBytecode, VertexDeclaration[] descriptions)
            {
                ShaderBytecode = shaderBytecode;
                VertexDeclarations = descriptions;
            }

            public readonly byte[] ShaderBytecode;

            public readonly VertexDeclaration[] VertexDeclarations;

            public bool Equals(VertexDeclarationSet other)
            {
                if (ShaderBytecode.Length != other.ShaderBytecode.Length)
                    return false;

                if (VertexDeclarations.Length != other.VertexDeclarations.Length)
                    return false;

                for (int i = 0; i < ShaderBytecode.Length; i++)
                    if (ShaderBytecode[i] != other.ShaderBytecode[i])
                        return false;

                for (int i = 0; i < VertexDeclarations.Length; i++)
                    if (VertexDeclarations[i] != other.VertexDeclarations[i])
                        return false;

                return true;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is VertexDeclarationSet && Equals((VertexDeclarationSet) obj);
            }

            public override int GetHashCode()
            {
                int hashCode = VertexDeclarations.Length.GetHashCode();
                foreach(var vertexDeclaration in VertexDeclarations)
                    hashCode = (hashCode * 397) ^ vertexDeclaration.GetHashCode();
                return hashCode;
            }

            public static bool operator ==(VertexDeclarationSet left, VertexDeclarationSet right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(VertexDeclarationSet left, VertexDeclarationSet right)
            {
                return !left.Equals(right);
            }
        }
    }
}