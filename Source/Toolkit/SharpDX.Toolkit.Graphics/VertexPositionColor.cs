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
using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes a custom vertex format structure that contains position and color information. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionColor : IEquatable<VertexPositionColor>
    {
        /// <summary>
        /// Initializes a new <see cref="VertexPositionColor"/> instance.
        /// </summary>
        /// <param name="position">The position of this vertex.</param>
        /// <param name="color">The color of this vertex.</param>
        public VertexPositionColor(Vector3 position, Color color) : this()
        {
            Position = position;
            Color = color;
        }

        /// <summary>
        /// XYZ position.
        /// </summary>
        [VertexElement("SV_Position")]
        public Vector3 Position;

        /// <summary>
        /// The vertex color.
        /// </summary>
        [VertexElement("COLOR")]
        public Color Color;
        
        /// <summary>
        /// Defines structure byte size.
        /// </summary>
        public static readonly int Size = 16;

        public bool Equals(VertexPositionColor other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionColor && Equals((VertexPositionColor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position.GetHashCode() * 397) ^ Color.GetHashCode();
            }
        }

        public static bool operator ==(VertexPositionColor left, VertexPositionColor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionColor left, VertexPositionColor right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("Position: {0}, Color: {1}", Position, Color);
        }
    }
}
