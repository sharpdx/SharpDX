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
using System.Runtime.InteropServices;
using SharpDX.Serialization;

namespace SharpDX
{
    /// <summary>
    /// Structure using the same layout than <see cref="System.Drawing.Rectangle"/> 
    /// </summary>
#if !W8CORE
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawingRectangle : IEquatable<DrawingRectangle>, IDataSerializable
    {
        /// <summary>
        /// An empty drawing rectangle.
        /// </summary>
        public readonly static DrawingRectangle Empty = new DrawingRectangle();

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingRectangle"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public DrawingRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Left coordinate.
        /// </summary>
        public int X;

        /// <summary>
        /// Top coordinate.
        /// </summary>
        public int Y;

        /// <summary>
        /// Width of this rectangle.
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of this rectangle.
        /// </summary>
        public int Height;

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(DrawingRectangle other)
        {
            return other.X == X && other.Y == Y && other.Width == Width && other.Height == Height;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(DrawingRectangle)) return false;
            return Equals((DrawingRectangle)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = X;
                result = (result * 397) ^ Y;
                result = (result * 397) ^ Width;
                result = (result * 397) ^ Height;
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
        public static bool operator ==(DrawingRectangle left, DrawingRectangle right)
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
        public static bool operator !=(DrawingRectangle left, DrawingRectangle right)
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
                serializer.Writer.Write(X);
                serializer.Writer.Write(Y);
                serializer.Writer.Write(Width);
                serializer.Writer.Write(Height);
            }
            else
            {
                X = serializer.Reader.ReadInt32();
                Y = serializer.Reader.ReadInt32();
                Width = serializer.Reader.ReadInt32();
                Height = serializer.Reader.ReadInt32();
            }
        }
    }
}