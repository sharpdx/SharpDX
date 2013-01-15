﻿// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
    /// Define a Rectangle. This structure is slightly different from System.Drawing.Rectangle as It is 
    /// internally storing Left,Top,Right,Bottom instead of Left,Top,Width,Height.
    /// Although automatic casting from a to System.Drawing.Rectangle is provided by this class.
    /// </summary>
#if !W8CORE
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle : IEquatable<Rectangle>, IDataSerializable
    {
        private int _left;
        private int _top;
        private int _right;
        private int _bottom;

        /// <summary>
        /// An empty rectangle.
        /// </summary>
        public static readonly Rectangle Empty;

        static Rectangle()
        {
            Empty = new Rectangle();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public Rectangle(int left, int top, int right, int bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        /// <summary>
        /// Checks, if specified point is inside <see cref="SharpDX.Rectangle"/>.
        /// </summary>
        /// <param name="x">X point coordinate.</param>
        /// <param name="y">Y point coordinate.</param>
        /// <returns><c>true</c> if point is inside <see cref="SharpDX.Rectangle"/>, otherwise <c>false</c>.</returns>
        public bool Contains(int x, int y)
        {
            if (x >= _left && x <= _right && y >= _top && y <= _bottom)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified point is inside <see cref="SharpDX.Rectangle"/>.
        /// </summary>
        /// <param name="x">X point coordinate.</param>
        /// <param name="y">Y point coordinate.</param>
        /// <returns><c>true</c> if point is inside <see cref="SharpDX.Rectangle"/>, otherwise <c>false</c>.</returns>
        public bool Contains(float x, float y)
        {
            if (x >= _left && x <= _right && y >= _top && y <= _bottom)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.Vector2"/> is inside <see cref="SharpDX.Rectangle"/>. 
        /// </summary> 
        /// <param name="vector2D">Coordinate <see cref="SharpDX.Vector2"/>.</param>
        /// <returns><c>true</c> if <see cref="SharpDX.Vector2"/> is inside <see cref="SharpDX.Rectangle"/>, otherwise <c>false</c>.</returns>
        public bool Contains(Vector2 vector2D)
        {
            if (vector2D.X >= _left && vector2D.X <= _right && vector2D.Y >= _top && vector2D.Y <= _bottom)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.DrawingPoint"/> is inside <see cref="SharpDX.Rectangle"/>. 
        /// </summary>
        /// <param name="point">Coordinate <see cref="SharpDX.DrawingPoint"/>.</param> 
        /// <returns><c>true</c> if <see cref="SharpDX.DrawingPoint"/> is inside <see cref="SharpDX.Rectangle"/>, otherwise <c>false</c>.</returns>
        public bool Contains(SharpDX.DrawingPoint point)
        {
            if (point.X >= _left && point.X <= _right && point.Y >= _top && point.Y <= _bottom)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.DrawingPointF"/> is inside <see cref="SharpDX.Rectangle"/>. 
        /// </summary>
        /// <param name="point">Coordinate <see cref="SharpDX.DrawingPointF"/>.</param> 
        /// <returns><c>true</c> if <see cref="SharpDX.DrawingPointF"/> is inside <see cref="SharpDX.Rectangle"/>, otherwise <c>false</c>.</returns>
        public bool Contains(SharpDX.DrawingPointF point)
        {
            if (point.X >= _left && point.X <= _right && point.Y >= _top && point.Y <= _bottom)
            {
                return true;
            }
            return false;
        } 

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>The left.</value>
        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>The top.</value>
        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>The right.</value>
        public int Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        public int Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        /// <summary>
        /// Gets the left position.
        /// </summary>
        /// <value>The left position.</value>
        public int X
        {
            get { return _left; }
        }

        /// <summary>
        /// Gets the top position.
        /// </summary>
        /// <value>The top position.</value>
        public int Y
        {
            get { return _top; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return Right - Left; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return Bottom - Top; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Rectangle)) return false;
            return Equals((Rectangle)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SharpDX.Rectangle"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SharpDX.Rectangle"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SharpDX.Rectangle"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Rectangle other)
        {
            return other._left == _left && other._top == _top && other._right == _right && other._bottom == _bottom;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = _left;
                result = (result * 397) ^ _top;
                result = (result * 397) ^ _right;
                result = (result * 397) ^ _bottom;
                return result;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.DrawingRectangle"/> to <see cref="SharpDX.Rectangle"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Rectangle(SharpDX.DrawingRectangle input)
        {
            return new Rectangle(input.X, input.Y, input.X + input.Width, input.Y + input.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Rectangle"/> to <see cref="SharpDX.DrawingRectangle"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SharpDX.DrawingRectangle(Rectangle input)
        {
            return new SharpDX.DrawingRectangle(input.Left, input.Top, input.Right - input.Left, input.Bottom - input.Top);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Write optimized version without using Serialize methods
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(_left);
                serializer.Writer.Write(_top);
                serializer.Writer.Write(_right);
                serializer.Writer.Write(_bottom);
            }
            else
            {
                _left = serializer.Reader.ReadInt32();
                _top = serializer.Reader.ReadInt32();
                _right = serializer.Reader.ReadInt32();
                _bottom = serializer.Reader.ReadInt32();
            }
        }
    }
}
