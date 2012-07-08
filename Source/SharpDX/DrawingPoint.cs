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

namespace SharpDX
{
    /// <summary>
    /// Structure using the same layout than <see cref="System.Drawing.Point"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawingPoint : IEquatable<DrawingPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingPoint"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public DrawingPoint(int x, int y)
        {
            X = x;
            Y = y;
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
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(DrawingPoint other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(DrawingPoint)) return false;
            return Equals((DrawingPoint)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
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
        public static bool operator ==(DrawingPoint left, DrawingPoint right)
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
        public static bool operator !=(DrawingPoint left, DrawingPoint right)
        {
            return !left.Equals(right);
        }

#if WinFormsInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.Point"/> to <see cref="SharpDX.DrawingPoint"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DrawingPoint(System.Drawing.Point input)
        {
            return new DrawingPoint(input.X, input.Y);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.DrawingPoint"/> to <see cref="System.Drawing.Point"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator System.Drawing.Point(DrawingPoint input)
        {
            return new System.Drawing.Point(input.X, input.Y);
        }
#endif

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}