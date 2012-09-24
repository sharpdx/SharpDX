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
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>	
    /// An attribute to use on a field in a structure, to describe a single vertex element for the input-assembler stage.
    /// </summary>	
    /// <seealso cref="VertexInputLayout"/>
    /// <seealso cref="VertexBufferLayout"/>
    /// <seealso cref="VertexElement"/>
    /// <msdn-id>ff476180</msdn-id>	
    /// <unmanaged>D3D11_INPUT_ELEMENT_DESC</unmanaged>	
    /// <unmanaged-short>D3D11_INPUT_ELEMENT_DESC</unmanaged-short>	
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexElementAttribute : Attribute
    {
        readonly string semanticName;
        readonly int semanticIndex;
        readonly Format format;
        readonly int alignedByteOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement" /> struct.
        /// </summary>
        /// <param name="semanticName">Name of the semantic.</param>
        /// <remarks>
        /// If the semantic name contains a postfix number, this number will be used as a semantic index. 
        /// The <see cref="SharpDX.DXGI.Format"/> will be mapped from the field type.
        /// </remarks>
        public VertexElementAttribute(string semanticName) : this(semanticName, Format.Unknown)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement" /> struct.
        /// </summary>
        /// <param name="semanticName">Name of the semantic.</param>
        /// <param name="format">The format.</param>
        /// <remarks>
        /// If the semantic name contains a postfix number, this number will be used as a semantic index.
        /// </remarks>
        public VertexElementAttribute(string semanticName, Format format) 
        {
            var match = VertexElement.MatchSemanticIndex.Match(semanticName);
            if (match.Success)
            {
                this.semanticName = match.Groups[1].Value;
                this.semanticIndex = int.Parse(match.Groups[2].Value);
            }
            else
            {
                this.semanticName = semanticName;
            }
            this.format = format;
            alignedByteOffset = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement" /> struct.
        /// </summary>
        /// <param name="semanticName">Name of the semantic.</param>
        /// <param name="semanticIndex">Index of the semantic.</param>
        /// <param name="format">The format.</param>
        /// <param name="alignedByteOffset">The aligned byte offset.</param>
        public VertexElementAttribute(string semanticName, int semanticIndex, Format format, int alignedByteOffset = VertexElement.AppendAligned)
        {
            this.semanticName = semanticName;
            this.semanticIndex = semanticIndex;
            this.format = format;
            this.alignedByteOffset = alignedByteOffset;
        }

        /// <summary>	
        /// <dd> <p>The HLSL semantic associated with this element in a shader input-signature.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>const char* SemanticName</unmanaged>	
        /// <unmanaged-short>char SemanticName</unmanaged-short>	
        public string SemanticName
        {
            get { return semanticName; }
        }

        /// <summary>	
        /// <dd> <p>The semantic index for the element. A semantic index modifies a semantic, with an integer index number. A semantic index is only needed in a  case where there is more than one element with the same semantic. For example, a 4x4 matrix would have four components each with the semantic  name </p>  <pre><code>matrix</code></pre>  <p>, however each of the four component would have different semantic indices (0, 1, 2, and 3).</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>unsigned int SemanticIndex</unmanaged>	
        /// <unmanaged-short>unsigned int SemanticIndex</unmanaged-short>	
        public int SemanticIndex
        {
            get { return semanticIndex; }
        }

        /// <summary>	
        /// <dd> <p>The data type of the element data. See <strong><see cref="SharpDX.DXGI.Format"/></strong>.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>DXGI_FORMAT Format</unmanaged>	
        /// <unmanaged-short>DXGI_FORMAT Format</unmanaged-short>	
        public SharpDX.DXGI.Format Format
        {
            get { return format; }
        }

        /// <summary>	
        /// <dd> <p>Optional. Offset (in bytes) between each element. Use D3D11_APPEND_ALIGNED_ELEMENT for convenience to define the current element directly  after the previous one, including any packing if necessary.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff476180</msdn-id>	
        /// <unmanaged>unsigned int AlignedByteOffset</unmanaged>	
        /// <unmanaged-short>unsigned int AlignedByteOffset</unmanaged-short>	
        public int AlignedByteOffset
        {
            get { return alignedByteOffset; }
        }
    }
}