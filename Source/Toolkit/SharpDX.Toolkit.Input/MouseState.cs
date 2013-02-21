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

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// The <see cref="MouseState"/> structure represents a snapshot of mouse state.
    /// </summary>
    /// <remarks>Is inmutable.</remarks>
    public struct MouseState
    {
        private readonly ButtonState left;
        private readonly ButtonState middle;
        private readonly ButtonState right;
        private readonly ButtonState xButton1;
        private readonly ButtonState xButton2;
        private readonly int x;
        private readonly int y;
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
        public MouseState(ButtonState left, ButtonState middle, ButtonState right, ButtonState xButton1, ButtonState xButton2, int x, int y, int wheelDelta)
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
        /// X-position of the mouse cursor
        /// </summary>
        public int X { get { return x; } }

        /// <summary>
        /// Y-position of the mouse cursor
        /// </summary>
        public int Y { get { return y; } }

        /// <summary>
        /// Gets the cumulative mouse scroll wheel value since the game was started.
        /// </summary>
        public int WheelDelta { get { return wheelDelta; } }
    }
}