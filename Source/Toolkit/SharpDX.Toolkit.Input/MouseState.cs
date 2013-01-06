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

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// The <see cref="MouseState"/> structure represents a snapshot of mouse state.
    /// </summary>
    /// <remarks>Is inmutable.</remarks>
    public struct MouseState
    {
        private readonly KeyState left;
        private readonly KeyState middle;
        private readonly KeyState right;
        private readonly KeyState xButton1;
        private readonly KeyState xButton2;
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
        public MouseState(KeyState left, KeyState middle, KeyState right, KeyState xButton1, KeyState xButton2, int x, int y, int wheelDelta)
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
        public KeyState Left { get { return left; } }

        /// <summary>
        /// State of the middle button
        /// </summary>
        public KeyState Middle { get { return middle; } }

        /// <summary>
        /// State of the right button
        /// </summary>
        public KeyState Right { get { return right; } }

        /// <summary>
        /// State of the X-Button 1
        /// </summary>
        public KeyState XButton1 { get { return xButton1; } }

        /// <summary>
        /// State of the X-Button 2
        /// </summary>
        public KeyState XButton2 { get { return xButton2; } }

        /// <summary>
        /// X-position of the mouse cursor
        /// </summary>
        public int X { get { return x; } }

        /// <summary>
        /// Y-position of the mouse cursor
        /// </summary>
        public int Y { get { return y; } }

        /// <summary>
        /// Delta of mouse wheel relative to previous input event
        /// </summary>
        public int WheelDelta { get { return wheelDelta; } }
    }
}