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
using SharpDX.Serialization;

namespace SharpDX
{
    /// <summary>
    /// Structure using the same layout than <see cref="System.Drawing.RectangleF"/>
    /// </summary>
#if !W8CORE
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawingRectangleF : IEquatable<DrawingRectangleF>, IDataSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingRectangleF"/> struct.
        /// </summary>
        /// <param name="position">The x-y position of this rectangle.</param>
        /// <param name="size">The x-y size of this rectangle.</param>
        public DrawingRectangleF(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingRectangleF"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public DrawingRectangleF(float x, float y, float width, float height)
        {
            Position = new Vector2(x,y);
            Size = new Vector2(width, height);
        }
        
          /// <summary>
        /// Checks, if specified point is inside <see cref="SharpDX.DrawingRectangleF"/>.
        /// </summary>
        /// <param name="x">X point coordinate.</param>
        /// <param name="y">Y point coordinate.</param>
        /// <returns><c>true</c> if point is inside <see cref="SharpDX.DrawingRectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(int x, int y)
        {
            if (x >= X && x <= X+Width && y >= Y && y <= Y+Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified point is inside <see cref="SharpDX.DrawingRectangleF"/>.
        /// </summary>
        /// <param name="x">X point coordinate.</param>
        /// <param name="y">Y point coordinate.</param>
        /// <returns><c>true</c> if point is inside <see cref="SharpDX.DrawingRectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(float x, float y)
        {
            if (x >= X && x <= X+Width && y >= Y && y <= Y+Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.Vector2"/> is inside <see cref="SharpDX.DrawingRectangleF"/>. 
        /// </summary> 
        /// <param name="vector2D">Coordinate <see cref="SharpDX.Vector2"/>.</param>
        /// <returns><c>true</c> if <see cref="SharpDX.Vector2"/> is inside <see cref="SharpDX.DrawingRectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(Vector2 vector2D)
        {
            if (vector2D.X >= X && vector2D.X <= X+Width && vector2D.Y >= Y && vector2D.Y <= Y+Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.DrawingPoint"/> is inside <see cref="SharpDX.DrawingRectangleF"/>. 
        /// </summary>
        /// <param name="point">Coordinate <see cref="SharpDX.DrawingPoint"/>.</param> 
        /// <returns><c>true</c> if <see cref="SharpDX.DrawingPoint"/> is inside <see cref="SharpDX.DrawingRectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(SharpDX.DrawingPoint point)
        {
            if (point.X >= X && point.X <= X+Width && point.Y >= Y && point.Y <= Y+Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.DrawingPointF"/> is inside <see cref="SharpDX.DrawingRectangleF"/>. 
        /// </summary>
        /// <param name="point">Coordinate <see cref="SharpDX.DrawingPointF"/>.</param> 
        /// <returns><c>true</c> if <see cref="SharpDX.DrawingPointF"/> is inside <see cref="SharpDX.DrawingRectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(SharpDX.DrawingPointF point)
        {
            if (point.X >= X && point.X <= X+Width && point.Y >= Y && point.Y <= Y+Height)
            {
                return true;
            }
            return false;
        }  

        /// <summary>
        /// The Position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The Size.
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// Left coordinate.
        /// </summary>
        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }

        /// <summary>
        /// Top coordinate.
        /// </summary>
        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        /// <summary>
        /// Width of this rectangle.
        /// </summary>
        public float Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }

        /// <summary>
        /// Height of this rectangle.
        /// </summary>
        public float Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(DrawingRectangleF other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Width.Equals(Width) && other.Height.Equals(Height);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(DrawingRectangleF)) return false;
            return Equals((DrawingRectangleF)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Width.GetHashCode();
                result = (result * 397) ^ Height.GetHashCode();
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
        public static bool operator ==(DrawingRectangleF left, DrawingRectangleF right)
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
        public static bool operator !=(DrawingRectangleF left, DrawingRectangleF right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("(X: {0} Y: {1} W: {2} H: {3})", X, Y, Width, Height);
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Write optimized version without using Serialize methods
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(Position.X);
                serializer.Writer.Write(Position.Y);
                serializer.Writer.Write(Size.X);
                serializer.Writer.Write(Size.Y);
            }
            else
            {
                X = serializer.Reader.ReadSingle();
                Y = serializer.Reader.ReadSingle();
                Width = serializer.Reader.ReadSingle();
                Height = serializer.Reader.ReadSingle();
            }
        }
    }
}
