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
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>	
    /// A description of a vertex elements for particular slot for the input-assembler stage. 
    /// This structure is related to <see cref="Direct3D11.InputElement"/>.
    /// </summary>	
    /// <remarks>	
    /// Because <see cref="Direct3D11.InputElement"/> requires to have the same <see cref="SlotIndex"/>, <see cref="InstanceCount"/>,
    /// this <see cref="VertexBufferLayout"/> structure encapsulates a set of <see cref="VertexElement"/> for a particular slot, instance count.
    /// </remarks>	
    /// <seealso cref="VertexElement"/>
    /// <msdn-id>ff476180</msdn-id>	
    /// <unmanaged>D3D11_INPUT_ELEMENT_DESC</unmanaged>	
    /// <unmanaged-short>D3D11_INPUT_ELEMENT_DESC</unmanaged-short>	
    public class VertexBufferLayout : IEquatable<VertexBufferLayout>
    {
        /// <summary>
        /// Vertex buffer slot index.
        /// </summary>
        public readonly int SlotIndex;

        /// <summary>	
        /// The number of instances to draw using the same per-instance data before advancing in the buffer by one element. This value must be 0 for an  element that contains per-vertex data (the slot class is set to <see cref="SharpDX.Direct3D11.InputClassification.PerVertexData"/>).
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>unsigned int InstanceDataStepRate</unmanaged>	
        /// <unmanaged-short>unsigned int InstanceDataStepRate</unmanaged-short>	
        public readonly int InstanceCount;

        /// <summary>
        /// Vertex elements describing this declaration.
        /// </summary>
        public readonly ReadOnlyArray<VertexElement> VertexElements;

        /// <summary>
        /// Precalculate hashcode for faster comparison.
        /// </summary>
        private int hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> struct.
        /// </summary>
        /// <param name="slot">The slot to bind this vertex buffer to. </param>
        /// <param name="elements">The elements.</param>
        /// <param name="instanceCount">The instance data step rate.</param>
        private VertexBufferLayout(int slot, VertexElement[] elements, int instanceCount)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");

            if (elements.Length == 0)
                throw new ArgumentException("Vertex elements cannot have zero elements.", "elements");

            // Make a copy of the elements.
            var copyElements = new VertexElement[elements.Length];
            Array.Copy(elements, copyElements, elements.Length);

            SlotIndex = slot;
            VertexElements = new ReadOnlyArray<VertexElement>(CalculateStaticOffsets(copyElements));
            InstanceCount = instanceCount;

            ComputeHashcode();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> struct.
        /// </summary>
        /// <param name="slot">The slot to bind this vertex buffer to.</param>
        /// <param name="structType">Type of a structure that is using <see cref="VertexElementAttribute" />.</param>
        /// <param name="instanceCount">Specify the instancing count. Set to 0 for no instancing.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New(int slot, Type structType, int instanceCount = 0)
        {
            return New(slot, VertexElement.FromType(structType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> struct.
        /// </summary>
        /// <typeparam name="T">Type of a structure that is using <see cref="VertexElementAttribute" />.</typeparam>
        /// <param name="slot">The slot to bind this vertex buffer to.</param>
        /// <param name="instanceCount">Specify the instancing count. Set to 0 for no instancing.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New<T>(int slot, int instanceCount = 0) where T : struct
        {
            return New(slot, typeof (T), instanceCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> struct.
        /// </summary>
        /// <param name="slot">The slot to bind this vertex buffer to.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New(int slot, params VertexElement[] elements)
        {
            return New(slot, elements, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferLayout" /> struct with instantiated data.
        /// </summary>
        /// <param name="slot">The slot to bind this vertex buffer to.</param>
        /// <param name="elements">The elements.</param>
        /// <param name="instanceCount">Specify the instancing count. Set to 0 for no instancing.</param>
        /// <returns>A new instance of <see cref="VertexBufferLayout"/>.</returns>
        public static VertexBufferLayout New(int slot, VertexElement[] elements, int instanceCount)
        {
            return new VertexBufferLayout(slot, elements, instanceCount);
        }

        private static VertexElement[] CalculateStaticOffsets(VertexElement[] vertexElements)
        {
            int offset = 0;
            for(int i = 0; i < vertexElements.Length; i++)
            {
                // If offset is not specified, use the current offset
                if (vertexElements[i].AlignedByteOffset == -1)
                    vertexElements[i].alignedByteOffset = offset;
                else
                    offset = vertexElements[i].AlignedByteOffset;

                // Move to the next field.
                offset += FormatHelper.SizeOfInBits(vertexElements[i].Format);
            }
            return vertexElements;
        }

        public bool Equals(VertexBufferLayout other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return hashCode == other.hashCode && VertexElements.Equals(other.VertexElements) && InstanceCount == other.InstanceCount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VertexBufferLayout) obj);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        private void ComputeHashcode()
        {
            // precalculate the hashcode for this instance
            hashCode = InstanceCount;
            hashCode = (hashCode * 397) ^ VertexElements.GetHashCode();
        }

        public static bool operator ==(VertexBufferLayout left, VertexBufferLayout right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VertexBufferLayout left, VertexBufferLayout right)
        {
            return !Equals(left, right);
        }
    }
}
