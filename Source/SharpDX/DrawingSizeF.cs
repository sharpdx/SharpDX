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
    /// Structure using the same layout than <see cref="System.Drawing.SizeF"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawingSizeF : IEquatable<DrawingSizeF>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingSizeF"/> struct.
        /// </summary>
        /// <param name="width">The x.</param>
        /// <param name="height">The y.</param>
        public DrawingSizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Width.
        /// </summary>
        public float Width;

        /// <summary>
        /// Height.
        /// </summary>
        public float Height;

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(DrawingSizeF other)
        {
            return other.Width == Width && other.Height == Height;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(DrawingSizeF)) return false;
            return Equals((DrawingSizeF)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
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
        public static bool operator ==(DrawingSizeF left, DrawingSizeF right)
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
        public static bool operator !=(DrawingSizeF left, DrawingSizeF right)
        {
            return !left.Equals(right);
        }

#if WinFormsInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.SizeF"/> to <see cref="SharpDX.DrawingSizeF"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DrawingSizeF(System.Drawing.SizeF input)
        {
            return new DrawingSizeF(input.Width, input.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.DrawingSizeF"/> to <see cref="System.Drawing.SizeF"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator System.Drawing.SizeF(DrawingSizeF input)
        {
            return new System.Drawing.SizeF(input.Width, input.Height);
        }
#endif

        public override string ToString()
        {
            return string.Format("({0},{1})", Width, Height);
        }
    }
}