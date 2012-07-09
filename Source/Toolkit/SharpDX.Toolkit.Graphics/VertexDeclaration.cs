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
using System.Collections.ObjectModel;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>	
    /// A description of a vertex elements for particular slot for the input-assembler stage. 
    /// This structure is related to <see cref="Direct3D11.InputElement"/>.
    /// </summary>	
    /// <remarks>	
    /// Because <see cref="Direct3D11.InputElement"/> requires to have the same <see cref="SlotIndex"/>, <see cref="VertexClassification"/> and <see cref="InstanceDataStepRate"/>,
    /// this <see cref="VertexDeclaration"/> structure encapsulates a set of <see cref="VertexElement"/> for a particular slot, classification and instance data step rate.
    /// </remarks>	
    /// <seealso cref="VertexElement"/>
    /// <msdn-id>ff476180</msdn-id>	
    /// <unmanaged>D3D11_INPUT_ELEMENT_DESC</unmanaged>	
    /// <unmanaged-short>D3D11_INPUT_ELEMENT_DESC</unmanaged-short>	
    public struct VertexDeclaration : IEquatable<VertexDeclaration>
    {
        /// <summary>	
        /// An integer value that identifies the input-assembler (see input slot). Valid values are between 0 and 15, defined in D3D11.h.
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>unsigned int InputSlot</unmanaged>	
        /// <unmanaged-short>unsigned int InputSlot</unmanaged-short>	
        public readonly int SlotIndex;

        /// <summary>	
        /// Identifies the input data class for a single input slot (see <strong><see cref="SharpDX.Direct3D11.InputClassification"/></strong>).
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>D3D11_INPUT_CLASSIFICATION InputSlotClass</unmanaged>	
        /// <unmanaged-short>D3D11_INPUT_CLASSIFICATION InputSlotClass</unmanaged-short>	
        public readonly InputClassification VertexClassification;

        /// <summary>	
        /// The number of instances to draw using the same per-instance data before advancing in the buffer by one element. This value must be 0 for an  element that contains per-vertex data (the slot class is set to <see cref="SharpDX.Direct3D11.InputClassification.PerVertexData"/>).
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>unsigned int InstanceDataStepRate</unmanaged>	
        /// <unmanaged-short>unsigned int InstanceDataStepRate</unmanaged-short>	
        public readonly int InstanceDataStepRate;

        /// <summary>
        /// Vertex elements describing this declaration.
        /// </summary>
        public readonly ReadOnlyCollection<VertexElement> VertexElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDeclaration" /> struct.
        /// </summary>
        /// <param name="elements">The elements.</param>
        public VertexDeclaration(params VertexElement[] elements)
            : this(0, elements)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDeclaration" /> struct.
        /// </summary>
        /// <param name="slotIndex">Index of the slot.</param>
        /// <param name="elements">The elements.</param>
        /// <param name="vertexClassification">The vertex classification.</param>
        /// <param name="instanceDataStepRate">The instance data step rate.</param>
        public VertexDeclaration(int slotIndex, VertexElement[] elements, InputClassification vertexClassification = InputClassification.PerVertexData, int instanceDataStepRate = 0)
        {
            if (elements == null)
                throw new ArgumentNullException("elements", "VertexElements cannot be null");

            SlotIndex = slotIndex;
            VertexElements = new ReadOnlyCollection<VertexElement>(elements);
            VertexClassification = vertexClassification;
            InstanceDataStepRate = instanceDataStepRate;
        }

        public static implicit operator VertexDeclaration(VertexElement element)
        {
            return new VertexDeclaration(element);
        }

        public static implicit operator VertexDeclaration(VertexElement[] elements)
        {
            return new VertexDeclaration(elements);
        }

        public bool Equals(VertexDeclaration other)
        {
            // PreTest
            if (!(SlotIndex == other.SlotIndex && VertexClassification.Equals(other.VertexClassification) && InstanceDataStepRate == other.InstanceDataStepRate))
                return false;

            if (VertexElements.Count != other.VertexElements.Count)
                return false;

            for (int i = 0; i < VertexElements.Count; i++)
                if (VertexElements[i] != other.VertexElements[i])
                    return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexDeclaration && Equals((VertexDeclaration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = SlotIndex;
                hashCode = (hashCode * 397) ^ VertexClassification.GetHashCode();
                hashCode = (hashCode * 397) ^ InstanceDataStepRate;
                hashCode = (hashCode * 397) ^ VertexElements.Count;
                return hashCode;
            }
        }

        public static bool operator ==(VertexDeclaration left, VertexDeclaration right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexDeclaration left, VertexDeclaration right)
        {
            return !left.Equals(right);
        }
    }
}
