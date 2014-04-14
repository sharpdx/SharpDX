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
        internal ButtonState leftButton;
        internal ButtonState middleButton;
        internal ButtonState rightButton;
        internal ButtonState xButton1;
        internal ButtonState xButton2;
        internal float x;
        internal float y;
        internal int wheelDelta;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseState"/> structure.
        /// </summary>
        /// <param name="leftButton">State of the left button</param>
        /// <param name="middleButton">State of the middle button</param>
        /// <param name="rightButton">State of the right button</param>
        /// <param name="xButton1">State of the X-Button 1</param>
        /// <param name="xButton2">State of the X-Button 2</param>
        /// <param name="x">X-position of the mouse cursor</param>
        /// <param name="y">Y-position of the mouse cursor</param>
        /// <param name="wheelDelta">Delta of mouse wheel relative to previous input event</param>
        public MouseState(ButtonState leftButton, ButtonState middleButton, ButtonState rightButton, ButtonState xButton1, ButtonState xButton2, float x, float y, int wheelDelta)
        {
            this.leftButton = leftButton;
            this.middleButton = middleButton;
            this.rightButton = rightButton;
            this.xButton1 = xButton1;
            this.xButton2 = xButton2;
            this.x = x;
            this.y = y;
            this.wheelDelta = wheelDelta;
        }

        /// <summary>
        /// State of the left button
        /// </summary>
        public ButtonState LeftButton { get { return leftButton; } }

        /// <summary>
        /// State of the middle button
        /// </summary>
        public ButtonState MiddleButton { get { return middleButton; } }

        /// <summary>
        /// State of the right button
        /// </summary>
        public ButtonState RightButton { get { return rightButton; } }

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

        public bool Equals(MouseState other)
        {
            return leftButton.Equals(other.leftButton) && middleButton.Equals(other.middleButton) && rightButton.Equals(other.rightButton) && xButton1.Equals(other.xButton1) && xButton2.Equals(other.xButton2) && x.Equals(other.x) && y.Equals(other.y) && wheelDelta == other.wheelDelta;
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            return obj is MouseState && Equals((MouseState)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = leftButton.GetHashCode();
                hashCode = (hashCode * 397) ^ middleButton.GetHashCode();
                hashCode = (hashCode * 397) ^ rightButton.GetHashCode();
                hashCode = (hashCode * 397) ^ xButton1.GetHashCode();
                hashCode = (hashCode * 397) ^ xButton2.GetHashCode();
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