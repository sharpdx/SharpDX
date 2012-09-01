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
    /// Structure using the same layout than <see cref="System.Drawing.RectangleF"/>
    /// </summary>
#if !WIN8METRO
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawingRectangleF : IEquatable<DrawingRectangleF>, IDataSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingRectangleF"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public DrawingRectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Left coordinate.
        /// </summary>
        public float X;

        /// <summary>
        /// Top coordinate.
        /// </summary>
        public float Y;

        /// <summary>
        /// Width of this rectangle.
        /// </summary>
        public float Width;

        /// <summary>
        /// Height of this rectangle.
        /// </summary>
        public float Height;

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

#if WinFormsInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.RectangleF"/> to <see cref="SharpDX.DrawingRectangleF"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DrawingRectangleF(System.Drawing.RectangleF input)
        {
            return new DrawingRectangleF(input.X, input.Y, input.Width, input.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.DrawingRectangleF"/> to <see cref="System.Drawing.RectangleF"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator System.Drawing.RectangleF(DrawingRectangleF input)
        {
            return new System.Drawing.RectangleF(input.X, input.Y, input.Width, input.Height);
        }
#endif

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
                X = serializer.Reader.ReadSingle();
                Y = serializer.Reader.ReadSingle();
                Width = serializer.Reader.ReadSingle();
                Height = serializer.Reader.ReadSingle();
            }
        }
    }
}