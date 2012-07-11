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
using System.Reflection;
using System.Text.RegularExpressions;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>	
    /// A description of a single element for the input-assembler stage. This structure is related to <see cref="Direct3D11.InputElement"/>.
    /// </summary>	
    /// <remarks>	
    /// Because <see cref="Direct3D11.InputElement"/> requires to have the same <see cref="VertexDeclaration.SlotIndex"/>, <see cref="VertexDeclaration.VertexClassification"/> and <see cref="VertexDeclaration.InstanceDataStepRate"/>,
    /// the <see cref="VertexDeclaration"/> structure encapsulates a set of <see cref="VertexElement"/> for a particular slot, classification and instance data step rate.
    /// Unlike the default <see cref="Direct3D11.InputElement"/>, this structure accepts a semantic name with a postfix number that will be automatically extracted to the semantic index.
    /// </remarks>	
    /// <seealso cref="VertexDeclaration"/>
    /// <msdn-id>ff476180</msdn-id>	
    /// <unmanaged>D3D11_INPUT_ELEMENT_DESC</unmanaged>	
    /// <unmanaged-short>D3D11_INPUT_ELEMENT_DESC</unmanaged-short>	
    public struct VertexElement : IEquatable<VertexElement>
    {
        /// <summary>
        ///   Returns a value that can be used for the offset parameter of an InputElement to indicate that the element
        ///   should be aligned directly after the previous element, including any packing if neccessary.
        /// </summary>
        /// <returns>A value used to align input elements.</returns>
        /// <unmanaged>D3D11_APPEND_ALIGNED_ELEMENT</unmanaged>
        public const int AppendAligned = -1;

        internal static readonly Regex MatchSemanticIndex = new Regex(@"(.*)(\d+)$");

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement" /> struct.
        /// </summary>
        /// <param name="semanticName">Name of the semantic.</param>
        /// <param name="format">The format.</param>
        /// <remarks>
        /// If the semantic name contains a postfix number, this number will be used as a semantic index.
        /// </remarks>
        public VertexElement(string semanticName, Format format) : this()
        {
            var match = MatchSemanticIndex.Match(semanticName);
            if (match.Success)
            {
                SemanticName = match.Groups[1].Value;
                SemanticIndex = int.Parse(match.Groups[2].Value);
            }
            else
            {
                SemanticName = semanticName;
            }
            Format = format;
            AlignedByteOffset = AppendAligned;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement" /> struct.
        /// </summary>
        /// <param name="semanticName">Name of the semantic.</param>
        /// <param name="semanticIndex">Index of the semantic.</param>
        /// <param name="format">The format.</param>
        /// <param name="alignedByteOffset">The aligned byte offset.</param>
        public VertexElement(string semanticName, int semanticIndex, Format format, int alignedByteOffset = AppendAligned) : this()
        {
            SemanticName = semanticName;
            SemanticIndex = semanticIndex;
            Format = format;
            AlignedByteOffset = alignedByteOffset;
        }

        /// <summary>	
        /// <dd> <p>The HLSL semantic associated with this element in a shader input-signature.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>const char* SemanticName</unmanaged>	
        /// <unmanaged-short>char SemanticName</unmanaged-short>	
        public readonly string SemanticName;

        /// <summary>	
        /// <dd> <p>The semantic index for the element. A semantic index modifies a semantic, with an integer index number. A semantic index is only needed in a  case where there is more than one element with the same semantic. For example, a 4x4 matrix would have four components each with the semantic  name </p>  <pre><code>matrix</code></pre>  <p>, however each of the four component would have different semantic indices (0, 1, 2, and 3).</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>unsigned int SemanticIndex</unmanaged>	
        /// <unmanaged-short>unsigned int SemanticIndex</unmanaged-short>	
        public readonly int SemanticIndex;

        /// <summary>	
        /// <dd> <p>The data type of the element data. See <strong><see cref="SharpDX.DXGI.Format"/></strong>.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>DXGI_FORMAT Format</unmanaged>	
        /// <unmanaged-short>DXGI_FORMAT Format</unmanaged-short>	
        public readonly SharpDX.DXGI.Format Format;

        /// <summary>	
        /// <dd> <p>Optional. Offset (in bytes) between each element. Use D3D11_APPEND_ALIGNED_ELEMENT for convenience to define the current element directly  after the previous one, including any packing if necessary.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>unsigned int AlignedByteOffset</unmanaged>	
        /// <unmanaged-short>unsigned int AlignedByteOffset</unmanaged-short>	
        public readonly int AlignedByteOffset;

        public bool Equals(VertexElement other)
        {
            return string.Compare(SemanticName, other.SemanticName, StringComparison.OrdinalIgnoreCase) == 0 && SemanticIndex == other.SemanticIndex && Format == other.Format && AlignedByteOffset == other.AlignedByteOffset;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexElement && Equals((VertexElement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (SemanticName != null ? SemanticName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ SemanticIndex;
                hashCode = (hashCode * 397) ^ Format.GetHashCode();
                hashCode = (hashCode * 397) ^ AlignedByteOffset;
                return hashCode;
            }
        }

        public static bool operator ==(VertexElement left, VertexElement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexElement left, VertexElement right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("{0}{1},{2},{3}", SemanticName, SemanticIndex == 0 ? string.Empty : string.Empty + SemanticIndex, Format, AlignedByteOffset);
        }

        public VertexElement[] FromType<T>() where T : struct
        {
            return FromType(typeof(T));
        }

        public VertexElement[] FromType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

#if WIN8METRO
            if (!type.GetTypeInfo().IsValueType)
                throw new ArgumentException("Type must be a value type");
#else
            if (!type.IsValueType)
                throw new ArgumentException("Type must be a value type");
#endif

            var vertexElements = new List<VertexElement>();
#if WIN8METRO
            foreach (var field in type.GetTypeInfo().DeclaredFields)
#else
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
#endif
            {
                var attributes = Utilities.GetCustomAttributes<VertexElementAttribute>(field);
                bool isVertexElementFound = false;
                foreach (var vertexElementAttribute in attributes)
                {
                    isVertexElementFound = true;
                    vertexElements.Add(new VertexElement(vertexElementAttribute.SemanticName, vertexElementAttribute.SemanticIndex, vertexElementAttribute.Format, vertexElementAttribute.AlignedByteOffset));
                    break;
                }

                if (!isVertexElementFound)
                    throw new ArgumentException(string.Format("Field {0} from type {1} doesn't have a [VertexElement] attribute", field.Name, type.Name), "type");
            }

            return vertexElements.ToArray();
        }
    }
}