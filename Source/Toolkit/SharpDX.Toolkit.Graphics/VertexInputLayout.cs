// Copyright (c) 2010 SharpDX - Alexandre Mutel
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
    /// Defines the layout of all vertex buffers that will be bound to the input-assembler stage. 
    /// This structure is related to <see cref="Direct3D11.InputElement"/>.
    /// </summary>	
    /// <remarks>	
    /// <p>Because <see cref="Direct3D11.InputElement"/> requires to have the same <see cref="InputElement.Slot"/>, <see cref="InputElement.InstanceDataStepRate"/>,
    /// this <see cref="VertexInputLayout"/> structure encapsulates a set of <see cref="VertexBufferLayout"/>.</p>
    /// <p>
    /// This class is caching <see cref="VertexInputLayout"/> to improve performance.
    /// The same description set of <see cref="VertexBufferLayout"/> will return the same <see cref="VertexInputLayout"/> instance.
    /// </p>
    /// </remarks>	
    /// <seealso cref="VertexElement"/>
    /// <msdn-id>ff476180</msdn-id>	
    /// <unmanaged>D3D11_INPUT_ELEMENT_DESC</unmanaged>	
    /// <unmanaged-short>D3D11_INPUT_ELEMENT_DESC</unmanaged-short>	
    public sealed class VertexInputLayout : IEquatable<VertexInputLayout>
    {
        private static readonly Dictionary<ReadOnlyArray<VertexBufferLayout>, VertexInputLayout> VertexBufferBindingCache = new Dictionary<ReadOnlyArray<VertexBufferLayout>, VertexInputLayout>();

        internal readonly InputElement[] InputElements;

        /// <summary>
        /// Gets a unique identifier of this VertexInputLayout configuration.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Gets the buffer layout.
        /// </summary>
        public readonly ReadOnlyArray<VertexBufferLayout> BufferLayouts;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputLayout" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="bufferLayouts">The vertex buffer layouts.</param>
        private VertexInputLayout(int id, ReadOnlyArray<VertexBufferLayout> bufferLayouts)
        {
            this.Id = id;
            this.BufferLayouts = bufferLayouts;
            InputElements = ComputeInputElements();
        }

        public bool Equals(VertexInputLayout other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is VertexInputLayout && Equals((VertexInputLayout) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        private InputElement[] ComputeInputElements()
        {
            var inputElements = new List<InputElement>();
            foreach (var vertexDesc in BufferLayouts)
            {
                foreach (var vertexElement in vertexDesc.VertexElements)
                {
                    // TODO: Perform consistency check on the input elements

                    inputElements.Add(new InputElement()
                    {
                        SemanticName = vertexElement.SemanticName,
                        SemanticIndex = vertexElement.SemanticIndex,
                        Format = vertexElement.Format,
                        AlignedByteOffset = vertexElement.AlignedByteOffset,
                        Slot = vertexDesc.SlotIndex,
                        Classification = vertexDesc.InstanceCount > 0 ? InputClassification.PerInstanceData : InputClassification.PerVertexData,
                        InstanceDataStepRate = vertexDesc.InstanceCount,
                    }
                        );
                }
            }
            return inputElements.ToArray();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(VertexInputLayout left, VertexInputLayout right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(VertexInputLayout left, VertexInputLayout right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputLayout" /> with a single slot from a structure that is using <see cref="VertexElementAttribute"/>.
        /// </summary>
        /// <param name="slot">The slot index in the input-assembler stage.</param>
        /// <param name="structType">Type of a structure that is using <see cref="VertexElementAttribute" />.</param>
        /// <returns>A new instance of <see cref="VertexInputLayout"/>.</returns>
        public static VertexInputLayout New(int slot, Type structType)
        {
            return New(VertexBufferLayout.New(slot, structType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputLayout" /> with a single slot from a structure that is using <see cref="VertexElementAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of a structure that is using <see cref="VertexElementAttribute" />.</typeparam>
        /// <param name="slot">The slot index in the input-assembler stage.</param>
        /// <returns>A new instance of <see cref="VertexInputLayout"/>.</returns>
        public static VertexInputLayout New<T>(int slot) where T : struct
        {
            return New(VertexBufferLayout.New(slot, typeof(T)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputLayout" /> with a single slot from a structure that is using <see cref="VertexElementAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of a structure that is using <see cref="VertexElementAttribute" />.</typeparam>
        /// <param name="slot">The slot index in the input-assembler stage.</param>
        /// <returns>A new instance of <see cref="VertexInputLayout"/>.</returns>
        public static VertexInputLayout FromBuffer<T>(int slot, Buffer<T> buffer) where T : struct
        {
            return New(VertexBufferLayout.New(slot, typeof(T)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputLayout" /> with a single slot.
        /// </summary>
        /// <param name="slot">The slot index in the input-assembler stage.</param>
        /// <param name="vertexElements">Description of vertex elements.</param>
        /// <returns>A new instance of <see cref="VertexInputLayout"/>.</returns>
        public static VertexInputLayout New(int slot, params VertexElement[] vertexElements)
        {
            return New(VertexBufferLayout.New(slot, vertexElements));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexInputLayout" />.
        /// </summary>
        /// <param name="layouts">A set of description of input layout for each slots in input-assembler stage.</param>
        /// <returns>A new instance of <see cref="VertexInputLayout"/>.</returns>
        public static VertexInputLayout New(params VertexBufferLayout[] layouts)
        {
            // Make sure that the original buffer won't be modified, so we make a copy.
            var layoutCopy = new VertexBufferLayout[layouts.Length];
            Array.Copy(layouts, layoutCopy, layouts.Length);

            // Create a readonly array.
            var vertexBufferLayouts = new ReadOnlyArray<VertexBufferLayout>(layoutCopy);

            // Register a unique VertexInputLayout.
            lock (VertexBufferBindingCache)
            {
                VertexInputLayout vertexInputLayout;
                if (!VertexBufferBindingCache.TryGetValue(vertexBufferLayouts, out vertexInputLayout))
                {
                    vertexInputLayout = new VertexInputLayout(VertexBufferBindingCache.Count, vertexBufferLayouts);
                    VertexBufferBindingCache.Add(vertexBufferLayouts, vertexInputLayout);
                }
                return vertexInputLayout;
            }
        }
    }
}