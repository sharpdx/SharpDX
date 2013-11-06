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
using System.Runtime.InteropServices;

using SharpDX.Direct3D;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Values that indicate how the pipeline interprets vertex data that is bound to the input-assembler stage. These primitive topology values determine how the vertex data is rendered on screen.
    /// PrimitiveType is equivalent to <see cref="SharpDX.Direct3D.PrimitiveTopology"/>.
    /// </summary>
    /// <remarks>
    /// This structure is implicitly castable to and from <see cref="SharpDX.Direct3D.PrimitiveTopology"/>, you can use it in place where <see cref="SharpDX.Direct3D.PrimitiveTopology"/> is required
    /// and vice-versa.
    /// </remarks>
    /// <msdn-id>ff728726</msdn-id>	
    /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY</unmanaged>	
    /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY</unmanaged-short>	
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct PrimitiveType : IEquatable<PrimitiveType>
    {
        /// <summary>
        /// Gets the value as a <see cref="SharpDX.Direct3D.PrimitiveTopology"/> enum.
        /// </summary>
        private readonly PrimitiveTopology Value;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="type"></param>
        private PrimitiveType(PrimitiveTopology type)
        {
            this.Value = type;
        }

        /// <summary>	
        /// The IA stage has not been initialized with a primitive topology. The IA stage will not function properly unless a primitive topology is defined.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_UNDEFINED</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_UNDEFINED</unmanaged-short>	
        public static readonly PrimitiveType Undefined = new PrimitiveType(PrimitiveTopology.Undefined);
        
        /// <summary>	
        /// Interpret the vertex data as a list of points.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_POINTLIST</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_POINTLIST</unmanaged-short>	
        public static readonly PrimitiveType PointList = new PrimitiveType(PrimitiveTopology.PointList);
        
        /// <summary>	
        /// Interpret the vertex data as a list of lines.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_LINELIST</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_LINELIST</unmanaged-short>	
        public static readonly PrimitiveType LineList = new PrimitiveType(PrimitiveTopology.LineList);

        /// <summary>	
        /// Interpret the vertex data as a line strip.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_LINESTRIP</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_LINESTRIP</unmanaged-short>	
        public static readonly PrimitiveType LineStrip = new PrimitiveType(PrimitiveTopology.LineStrip);	
        
        /// <summary>	
        /// Interpret the vertex data as a list of triangles.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST</unmanaged-short>	
        public static readonly PrimitiveType TriangleList = new PrimitiveType(PrimitiveTopology.TriangleList);		
        
        /// <summary>	
        /// Interpret the vertex data as a triangle strip.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP</unmanaged-short>	
        public static readonly PrimitiveType TriangleStrip = new PrimitiveType(PrimitiveTopology.TriangleStrip);	
        
        /// <summary>	
        /// Interpret the vertex data as a list of lines with adjacency data.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_LINELIST_ADJ</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_LINELIST_ADJ</unmanaged-short>	
        public static readonly PrimitiveType LineListWithAdjacency = new PrimitiveType(PrimitiveTopology.LineListWithAdjacency);	
        
        /// <summary>	
        /// Interpret the vertex data as a line strip with adjacency data.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_LINESTRIP_ADJ</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_LINESTRIP_ADJ</unmanaged-short>	
        public static readonly PrimitiveType LineStripWithAdjacency = new PrimitiveType(PrimitiveTopology.LineStripWithAdjacency);		
        
        /// <summary>	
        /// Interpret the vertex data as a list of triangles with adjacency data.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST_ADJ</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST_ADJ</unmanaged-short>	
        public static readonly PrimitiveType TriangleListWithAdjacency = new PrimitiveType(PrimitiveTopology.TriangleListWithAdjacency);		
        
        /// <summary>	
        /// Interpret the vertex data as a triangle strip with adjacency data.
        /// </summary>	
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP_ADJ</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP_ADJ</unmanaged-short>	
        public static readonly PrimitiveType TriangleStripWithAdjacency = new PrimitiveType(PrimitiveTopology.TriangleStripWithAdjacency);

        /// <summary>	
        /// Interpret the vertex data as a patch list.
        /// </summary>	
        /// <param name="controlPoints">Number of control points. Value must be in the range 1 to 32.</param>
        /// <unmanaged>D3D_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP_ADJ</unmanaged>	
        /// <unmanaged-short>D3D_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP_ADJ</unmanaged-short>	
        public static PrimitiveType PatchList(int controlPoints)
        {
            if (controlPoints < 1 || controlPoints > 32)
                throw new ArgumentException("Value must be in between 1 and 32", "controlPoints");
            return new PrimitiveType((PrimitiveTopology) ((int) PrimitiveTopology.PatchListWith1ControlPoints - 1 + controlPoints));
        }

        /// <summary>Performs an implicit conversion from <see cref="PrimitiveType"/> to <see cref="PrimitiveTopology"/>.</summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PrimitiveTopology(PrimitiveType from)
        {
            return from.Value;
        }

        /// <summary>Performs an implicit conversion from <see cref="PrimitiveTopology"/> to <see cref="PrimitiveType"/>.</summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PrimitiveType(PrimitiveTopology from)
        {
            return new PrimitiveType(from);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(PrimitiveType other)
        {
            return Value.Equals(other.Value);
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PrimitiveType && Equals((PrimitiveType) obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>Implements the ==.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PrimitiveType left, PrimitiveType right)
        {
            return left.Equals(right);
        }

        /// <summary>Implements the !=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PrimitiveType left, PrimitiveType right)
        {
            return !left.Equals(right);
        }
    }
}