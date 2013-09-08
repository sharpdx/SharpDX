﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
    /// Define a RectangleF. This structure is slightly different from System.Drawing.RectangleF as it is
    /// internally storing Left,Top,Right,Bottom instead of Left,Top,Width,Height.
    /// </summary>
#if !W8CORE

    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct RectangleF : IEquatable<RectangleF>, IDataSerializable
    {
        private float _left;
        private float _top;
        private float _right;
        private float _bottom;

        /// <summary>
        /// An empty rectangle
        /// </summary>
        public static readonly RectangleF Empty;

        static RectangleF()
        {
            Empty = new RectangleF();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> struct.
        /// </summary>
        /// <param name="x">The left.</param>
        /// <param name="y">The top.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectangleF(float x, float y, float width, float height)
        {
            _left = x;
            _top = y;
            _right = x + width;
            _bottom = y + height;
        }

        /// <summary>
        /// Gets or sets the X position of the left edge.
        /// </summary>
        /// <value>The left.</value>
        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>The top.</value>
        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>The right.</value>
        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        /// <summary>
        /// Gets or sets the X position.
        /// </summary>
        /// <value>The X position.</value>
        public float X
        {
            get
            {
                return _left;
            }
            set
            {
                _right = value + Width;
                _left = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y position.
        /// </summary>
        /// <value>The Y position.</value>
        public float Y
        {
            get
            {
                return _top;
            }
            set
            {
                _bottom = value + Height;
                _top = value;
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get { return _right - _left; }
            set { _right = _left + value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public float Height
        {
            get { return _bottom - _top; }
            set { _bottom = _top + value; }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public Vector2 Location
        {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets the Point that specifies the center of the rectangle.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public Vector2 Center
        {
            get
            {
                return new Vector2(X + (Width / 2), Y + (Height / 2));
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the rectangle is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is empty]; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return (Width == 0.0f) && (Height == 0.0f) && (X == 0.0f) && (Y == 0.0f);
            }
        }

        /// <summary>
        /// Gets or sets the size of the rectangle.
        /// </summary>
        /// <value>The size of the rectangle.</value>
        public Size2F Size
        {
            get
            {
                return new Size2F(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets the position of the top-left corner of the rectangle.
        /// </summary>
        /// <value>The top-left corner of the rectangle.</value>
        public Vector2 TopLeft { get { return new Vector2(_left, _top); } }

        /// <summary>
        /// Gets the position of the top-right corner of the rectangle.
        /// </summary>
        /// <value>The top-right corner of the rectangle.</value>
        public Vector2 TopRight { get { return new Vector2(_right, _top); } }

        /// <summary>
        /// Gets the position of the bottom-left corner of the rectangle.
        /// </summary>
        /// <value>The bottom-left corner of the rectangle.</value>
        public Vector2 BottomLeft { get { return new Vector2(_left, _bottom); } }

        /// <summary>
        /// Gets the position of the bottom-right corner of the rectangle.
        /// </summary>
        /// <value>The bottom-right corner of the rectangle.</value>
        public Vector2 BottomRight { get { return new Vector2(_right, _bottom); } }

        /// <summary>Changes the position of the rectangle.</summary>
        /// <param name="amount">The values to adjust the position of the rectangle by.</param>
        public void Offset(Point amount)
        {
            Offset(amount.X, amount.Y);
        }

        /// <summary>Changes the position of the rectangle.</summary>
        /// <param name="amount">The values to adjust the position of the rectangle by.</param>
        public void Offset(Vector2 amount)
        {
            Offset(amount.X, amount.Y);
        }

        /// <summary>Changes the position of the rectangle.</summary>
        /// <param name="offsetX">Change in the x-position.</param>
        /// <param name="offsetY">Change in the y-position.</param>
        public void Offset(float offsetX, float offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        /// <summary>Pushes the edges of the rectangle out by the horizontal and vertical values specified.</summary>
        /// <param name="horizontalAmount">Value to push the sides out by.</param>
        /// <param name="verticalAmount">Value to push the top and bottom out by.</param>
        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }

        /// <summary>Determines whether this rectangle contains a specified Point.</summary>
        /// <param name="value">The Point to evaluate.</param>
        /// <param name="result">[OutAttribute] true if the specified Point is contained within this rectangle; false otherwise.</param>
        public void Contains(ref Vector2 value, out bool result)
        {
            result = (X <= value.X) && (value.X < Right) && (Y <= value.Y) && (value.Y < Bottom);
        }

        /// <summary>Determines whether this rectangle entirely contains a specified rectangle.</summary>
        /// <param name="value">The rectangle to evaluate.</param>
        public bool Contains(Rectangle value)
        {
            return (X <= value.X) && (value.Right <= Right) && (Y <= value.Y) && (value.Bottom <= Bottom);
        }

        /// <summary>Determines whether this rectangle entirely contains a specified rectangle.</summary>
        /// <param name="value">The rectangle to evaluate.</param>
        /// <param name="result">[OutAttribute] On exit, is true if this rectangle entirely contains the specified rectangle, or false if not.</param>
        public void Contains(ref RectangleF value, out bool result)
        {
            result = (X <= value.X) && (value.Right <= Right) && (Y <= value.Y) && (value.Bottom <= Bottom);
        }

        /// <summary>
        /// Checks, if specified point is inside <see cref="SharpDX.RectangleF"/>.
        /// </summary>
        /// <param name="x">X point coordinate.</param>
        /// <param name="y">Y point coordinate.</param>
        /// <returns><c>true</c> if point is inside <see cref="SharpDX.RectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(float x, float y)
        {
            return (x >= _left && x <= _right && y >= _top && y <= _bottom);
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.Vector2"/> is inside <see cref="SharpDX.RectangleF"/>.
        /// </summary>
        /// <param name="vector2D">Coordinate <see cref="SharpDX.Vector2"/>.</param>
        /// <returns><c>true</c> if <see cref="SharpDX.Vector2"/> is inside <see cref="SharpDX.RectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(Vector2 vector2D)
        {
            return Contains(vector2D.X, vector2D.Y);
        }

        /// <summary>
        /// Checks, if specified <see cref="SharpDX.Point"/> is inside <see cref="SharpDX.RectangleF"/>.
        /// </summary>
        /// <param name="point">Coordinate <see cref="SharpDX.Point"/>.</param>
        /// <returns><c>true</c> if <see cref="SharpDX.Point"/> is inside <see cref="SharpDX.RectangleF"/>, otherwise <c>false</c>.</returns>
        public bool Contains(Point point)
        {
            return Contains(point.X, point.Y);
        }

        /// <summary>Determines whether a specified rectangle intersects with this rectangle.</summary>
        /// <param name="value">The rectangle to evaluate.</param>
        public bool Intersects(RectangleF value)
        {
            bool result;
            Intersects(ref value, out result);
            return result;
        }

        /// <summary>
        /// Determines whether a specified rectangle intersects with this rectangle.
        /// </summary>
        /// <param name="value">The rectangle to evaluate</param>
        /// <param name="result">[OutAttribute] true if the specified rectangle intersects with this one; false otherwise.</param>
        public void Intersects(ref RectangleF value, out bool result)
        {
            result = (value.X < Right) && (X < value.Right) && (value.Y < Bottom) && (Y < value.Bottom);
        }

        /// <summary>
        /// Creates a rectangle defining the area where one rectangle overlaps with another rectangle.
        /// </summary>
        /// <param name="value1">The first Rectangle to compare.</param>
        /// <param name="value2">The second Rectangle to compare.</param>
        /// <returns>The intersection rectangle.</returns>
        public static RectangleF Intersect(RectangleF value1, RectangleF value2)
        {
            RectangleF result;
            Intersect(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>Creates a rectangle defining the area where one rectangle overlaps with another rectangle.</summary>
        /// <param name="value1">The first rectangle to compare.</param>
        /// <param name="value2">The second rectangle to compare.</param>
        /// <param name="result">[OutAttribute] The area where the two first parameters overlap.</param>
        public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            float newLeft = (value1.X > value2.X) ? value1.X : value2.X;
            float newTop = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            float newRight = (value1.Right < value2.Right) ? value1.Right : value2.Right;
            float newBottom = (value1.Bottom < value2.Bottom) ? value1.Bottom : value2.Bottom;
            if ((newRight > newLeft) && (newBottom > newTop))
            {
                result = new RectangleF(newLeft, newTop, newRight - newLeft, newBottom - newTop);
            }
            else
            {
                result = Empty;
            }
        }

        /// <summary>
        /// Creates a new rectangle that exactly contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first rectangle to contain.</param>
        /// <param name="value2">The second rectangle to contain.</param>
        /// <returns>The union rectangle.</returns>
        public static RectangleF Union(RectangleF value1, RectangleF value2)
        {
            RectangleF result;
            Union(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Creates a new rectangle that exactly contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first rectangle to contain.</param>
        /// <param name="value2">The second rectangle to contain.</param>
        /// <param name="result">[OutAttribute] The rectangle that must be the union of the first two rectangles.</param>
        public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            var left = Math.Min(value1.Left, value2.Left);
            var right = Math.Max(value1.Right, value2.Right);
            var top = Math.Min(value1.Top, value2.Top);
            var bottom = Math.Max(value1.Bottom, value2.Bottom);
            result = new RectangleF(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(RectangleF)) return false;
            return Equals((RectangleF)obj);
        }

        /// <inheritdoc/>
        public bool Equals(RectangleF other)
        {
            return (float)Math.Abs(other.Left - Left) < MathUtil.ZeroTolerance &&
                   (float)Math.Abs(other.Right - Right) < MathUtil.ZeroTolerance &&
                   (float)Math.Abs(other.Top - Top) < MathUtil.ZeroTolerance &&
                   (float)Math.Abs(other.Bottom - Bottom) < MathUtil.ZeroTolerance;
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
                int result = _left.GetHashCode();
                result = (result * 397) ^ _top.GetHashCode();
                result = (result * 397) ^ _right.GetHashCode();
                result = (result * 397) ^ _bottom.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RectangleF left, RectangleF right)
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
                _left = serializer.Reader.ReadSingle();
                _top = serializer.Reader.ReadSingle();
                _right = serializer.Reader.ReadSingle();
                _bottom = serializer.Reader.ReadSingle();
            }
        }
    }
}
