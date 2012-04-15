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
    /// Structure using the same layout than <see cref="System.Drawing.Size"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawingSize : IEquatable<DrawingSize>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingSize"/> struct.
        /// </summary>
        /// <param name="width">The x.</param>
        /// <param name="height">The y.</param>
        public DrawingSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Width.
        /// </summary>
        public int Width;

        /// <summary>
        /// Height.
        /// </summary>
        public int Height;

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(DrawingSize other)
        {
            return other.Width == Width && other.Height == Height;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(DrawingSize)) return false;
            return Equals((DrawingSize)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
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
        public static bool operator ==(DrawingSize left, DrawingSize right)
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
        public static bool operator !=(DrawingSize left, DrawingSize right)
        {
            return !left.Equals(right);
        }

#if WinFormsInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.Size"/> to <see cref="SharpDX.DrawingSize"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DrawingSize(System.Drawing.Size input)
        {
            return new DrawingSize(input.Width, input.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.DrawingSize"/> to <see cref="System.Drawing.Size"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator System.Drawing.Size(DrawingSize input)
        {
            return new System.Drawing.Size(input.Width, input.Height);
        }
#endif
    }
}