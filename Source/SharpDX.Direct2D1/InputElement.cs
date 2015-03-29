// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using SharpDX.DXGI;

namespace SharpDX.Direct2D1
{
    public partial struct InputElement
    {
        /// <summary>
        ///   Returns a value that can be used for the offset parameter of an InputElement to indicate that the element
        ///   should be aligned directly after the previous element, including any packing if necessary.
        /// </summary>
        /// <returns>A value used to align input elements.</returns>
        /// <unmanaged>D2D1_APPEND_ALIGNED_ELEMENT</unmanaged>
        public static int AppendAligned
        {
            get { return -1; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct2D11.InputElement" /> struct.
        /// </summary>
        /// <param name = "name">The HLSL semantic associated with this element in a shader input-signature.</param>
        /// <param name = "index">The semantic index for the element. A semantic index modifies a semantic, with an integer index number. A semantic index is only needed in a case where there is more than one element with the same semantic. For example, a 4x4 matrix would have four components each with the semantic name matrix, however each of the four component would have different semantic indices (0, 1, 2, and 3).</param>
        /// <param name = "format">The data type of the element data.</param>
        /// <param name = "slot">An integer value that identifies the input-assembler. Valid values are between 0 and 15.</param>
        public InputElement(string name, int index, Format format, int slot)
        {
            this.SemanticName = name;
            this.SemanticIndex = index;
            this.Format = format;
            this.Slot = slot;
            this.AlignedByteOffset = AppendAligned;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct2D11.InputElement" /> struct.
        /// </summary>
        /// <param name = "name">The HLSL semantic associated with this element in a shader input-signature.</param>
        /// <param name = "index">The semantic index for the element. A semantic index modifies a semantic, with an integer index number. A semantic index is only needed in a case where there is more than one element with the same semantic. For example, a 4x4 matrix would have four components each with the semantic name matrix, however each of the four component would have different semantic indices (0, 1, 2, and 3).</param>
        /// <param name = "format">The data type of the element data.</param>
        /// <param name = "offset">Offset (in bytes) between each element. Use AppendAligned for convenience to define the current element directly after the previous one, including any packing if necessary.</param>
        /// <param name = "slot">An integer value that identifies the input-assembler. Valid values are between 0 and 15.</param>
        public InputElement(string name, int index, Format format, int offset, int slot)
        {
            this.SemanticName = name;
            this.SemanticIndex = index;
            this.Format = format;
            this.Slot = slot;
            this.AlignedByteOffset = offset;
        }

        /// <summary>
        /// Determines whether the specified <see cref="InputElement"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="InputElement"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="InputElement"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(InputElement other)
        {
            return Equals(other.SemanticName, SemanticName) && other.SemanticIndex == SemanticIndex && Equals(other.Format, Format) && other.Slot == Slot && other.AlignedByteOffset == AlignedByteOffset;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(InputElement)) return false;
            return Equals((InputElement)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = SemanticName.GetHashCode();
                result = (result * 397) ^ SemanticIndex.GetHashCode();
                result = (result * 397) ^ Format.GetHashCode();
                result = (result * 397) ^ Slot.GetHashCode();
                result = (result * 397) ^ AlignedByteOffset.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(InputElement left, InputElement right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(InputElement left, InputElement right)
        {
            return !left.Equals(right);
        }
    }
}