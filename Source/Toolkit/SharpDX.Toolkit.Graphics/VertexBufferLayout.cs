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
    /// this <see cref="VertexBufferLayout"/> structure encapsulates a set of <see cref="VertexBufferSlot"/>.</p>
    /// <p>
    /// This class is caching <see cref="VertexBufferLayout"/> to improve performance.
    /// The same description set of <see cref="VertexBufferSlot"/> will return the same <see cref="VertexBufferLayout"/> instance.
    /// </p>
    /// </remarks>	
    /// <seealso cref="VertexElement"/>
    /// <msdn-id>ff476180</msdn-id>	
    /// <unmanaged>D3D11_INPUT_ELEMENT_DESC</unmanaged>	
    /// <unmanaged-short>D3D11_INPUT_ELEMENT_DESC</unmanaged-short>	
    public sealed class VertexBufferLayout : IEquatable<VertexBufferLayout>
    {
        private static readonly Dictionary<ReadOnlyArray<VertexBufferSlot>, VertexBufferLayout> VertexBufferBindingCache = new Dictionary<ReadOnlyArray<VertexBufferSlot>, VertexBufferLayout>();

        /// <summary>
        /// Gets a unique identifier of this VertexBufferLayout configuration.
        /// </summary>
        public readonly int Id;

        public readonly ReadOnlyArray<VertexBufferSlot> VertexBufferLayouts;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="vertexBufferLayouts">The vertex buffer layouts.</param>
        private VertexBufferLayout(int id, ReadOnlyArray<VertexBufferSlot> vertexBufferLayouts)
        {
            this.Id = id;
            VertexBufferLayouts = vertexBufferLayouts;
        }

        public bool Equals(VertexBufferLayout other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is VertexBufferLayout && Equals((VertexBufferLayout) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(VertexBufferLayout left, VertexBufferLayout right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(VertexBufferLayout left, VertexBufferLayout right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> with a single slot from a structure that is using <see cref="VertexElementAttribute"/>.
        /// </summary>
        /// <param name="slot">The slot index in the input-assembler stage.</param>
        /// <param name="structType">Type of a structure that is using <see cref="VertexElementAttribute" />.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New(int slot, Type structType)
        {
            return New(VertexBufferSlot.New(slot, structType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> with a single slot from a structure that is using <see cref="VertexElementAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of a structure that is using <see cref="VertexElementAttribute" />.</typeparam>
        /// <param name="slot">The slot index in the input-assembler stage.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New<T>(int slot) where T : struct
        {
            return New(VertexBufferSlot.New(slot, typeof(T)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> with a single slot.
        /// </summary>
        /// <param name="slot">The slot index in the input-assembler stage.</param>
        /// <param name="vertexElements">Description of vertex elements.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New(int slot, params VertexElement[] vertexElements)
        {
            return New(VertexBufferSlot.New(slot, vertexElements));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" />.
        /// </summary>
        /// <param name="slots">A set of description of input layout for each slots in input-assembler stage.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New(params VertexBufferSlot[] slots)
        {
            // Make sure that the original buffer won't be modified, so we make a copy.
            var layoutCopy = new VertexBufferSlot[slots.Length];
            Array.Copy(slots, layoutCopy, slots.Length);

            // Create a readonly array.
            var vertexBufferLayouts = new ReadOnlyArray<VertexBufferSlot>(layoutCopy);

            // Register a unique VertexBufferLayout.
            lock (VertexBufferBindingCache)
            {
                VertexBufferLayout vertexBufferLayout;
                if (!VertexBufferBindingCache.TryGetValue(vertexBufferLayouts, out vertexBufferLayout))
                {
                    vertexBufferLayout = new VertexBufferLayout(VertexBufferBindingCache.Count, vertexBufferLayouts);
                    VertexBufferBindingCache.Add(vertexBufferLayouts, vertexBufferLayout);
                }
                return vertexBufferLayout;
            }
        }
    }
}