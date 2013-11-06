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

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// The <see cref="MouseState"/> structure represents a snapshot of mouse state.
    /// </summary>
    /// <remarks>Is immutable.</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseState : IEquatable<MouseState>
    {
        private readonly ButtonState left;
        private readonly ButtonState middle;
        private readonly ButtonState right;
        private readonly ButtonState xButton1;
        private readonly ButtonState xButton2;
        private readonly float x;
        private readonly float y;
        private readonly int wheelDelta;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseState"/> structure.
        /// </summary>
        /// <param name="left">State of the left button</param>
        /// <param name="middle">State of the middle button</param>
        /// <param name="right">State of the right button</param>
        /// <param name="xButton1">State of the X-Button 1</param>
        /// <param name="xButton2">State of the X-Button 2</param>
        /// <param name="x">X-position of the mouse cursor</param>
        /// <param name="y">Y-position of the mouse cursor</param>
        /// <param name="wheelDelta">Delta of mouse wheel relative to previous input event</param>
        public MouseState(ButtonState left, ButtonState middle, ButtonState right, ButtonState xButton1, ButtonState xButton2, float x, float y, int wheelDelta)
        {
            this.left = left;
            this.middle = middle;
            this.right = right;
            this.xButton1 = xButton1;
            this.xButton2 = xButton2;
            this.x = x;
            this.y = y;
            this.wheelDelta = wheelDelta;
        }

        /// <summary>
        /// State of the left button
        /// </summary>
        public ButtonState Left { get { return left; } }

        /// <summary>
        /// State of the middle button
        /// </summary>
        public ButtonState Middle { get { return middle; } }

        /// <summary>
        /// State of the right button
        /// </summary>
        public ButtonState Right { get { return right; } }

        /// <summary>
        /// State of the X-Button 1
        /// </summary>
        public ButtonState XButton1 { get { return xButton1; } }

        /// <summary>
        /// State of the X-Button 2
        /// </summary>
        public ButtonState XButton2 { get { return xButton2; } }

        /// <summary>
        /// X-position of the mouse cursor in the range [0,1]
        /// </summary>
        public float X { get { return x; } }

        /// <summary>
        /// Y-position of the mouse cursor in the range [0,1]
        /// </summary>
        public float Y { get { return y; } }

        /// <summary>
        /// Gets the cumulative mouse scroll wheel value since the game was started.
        /// </summary>
        public int WheelDelta { get { return wheelDelta; } }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(MouseState other)
        {
            return left == other.left && middle == other.middle && right == other.right && xButton1 == other.xButton1 && xButton2 == other.xButton2 && MathUtil.NearEqual(x, other.x) && MathUtil.NearEqual(y, other.y) && wheelDelta == other.wheelDelta;
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is MouseState && Equals((MouseState)obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)left;
                hashCode = (hashCode * 397) ^ (int)middle;
                hashCode = (hashCode * 397) ^ (int)right;
                hashCode = (hashCode * 397) ^ (int)xButton1;
                hashCode = (hashCode * 397) ^ (int)xButton2;
                hashCode = (hashCode * 397) ^ x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ wheelDelta;
                return hashCode;
            }
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MouseState left, MouseState right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MouseState left, MouseState right)
        {
            return !left.Equals(right);
        }
    }
}