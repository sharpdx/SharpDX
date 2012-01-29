// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// Define a RectangleF. This structure is slightly different from System.Drawing.RectangleF as It is 
    /// internally storing Left,Top,Right,Bottom instead of Left,Top,Width,Height.
    /// Although automatic casting from a to System.Drawing.Rectangle is provided by this class.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RectangleF
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
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public RectangleF(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        /// <summary>
        /// Gets or sets the left.
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
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get { return Right - Left; }
            set { Right = Left + value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public float Height
        {
            get { return Bottom - Top; }
            set { Top = Bottom + value; }
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
            if (!(obj is RectangleF))
            {
                return false;
            }
            RectangleF rectangle = (RectangleF)obj;
            return ((((rectangle.Left == this.Left) && (rectangle.Right == this.Right)) && (rectangle.Bottom == this.Bottom)) && (rectangle.Top == this.Top));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (((((int)this.Left) ^ (((int)this.Top) << 13) | (((int)this.Top) >> 0x13))) ^ ((((int)this.Bottom) << 0x1a) | (((int)this.Bottom) >> 6))) ^ ((((int)this.Right) << 7) | (((int)this.Right) >> 0x19));
        }
#if WinFormsInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.RectangleF"/> to <see cref="SharpDX.RectangleF"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RectangleF(System.Drawing.RectangleF input)
        {
            return new RectangleF(input.Left, input.Top, input.Right, input.Bottom);
        }

        /// <summary>
        /// Rectangles the F.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static implicit operator System.Drawing.RectangleF(RectangleF input)
        {
            return new System.Drawing.RectangleF(input.Left, input.Top, input.Right - input.Left, input.Bottom - input.Top);
        }
#endif
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return ((((left.Left == right.Left) && (left.Right == right.Right)) && (left.Top == right.Top)) && (left.Bottom == right.Bottom));
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
    }
}
