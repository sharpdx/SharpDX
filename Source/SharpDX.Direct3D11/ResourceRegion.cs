// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
namespace SharpDX.Direct3D11
{
    public partial struct ResourceRegion
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ResourceRegion"/> struct.
        /// </summary>
        /// <param name="left">Left coordinates (inclusive)</param>
        /// <param name="top">Top coordinates (inclusive)</param>
        /// <param name="front">Front coordinates (inclusive)</param>
        /// <param name="right">Right coordinates (exclusive)</param>
        /// <param name="bottom">Bottom coordinates (exclusive)</param>
        /// <param name="back">Back coordinates (exclusive)</param>
        /// <remarks>
        /// <ul>
        /// <li>For a Width of 1 pixels, (right - left) = 1. If left = 0, right = Width. </li>
        /// <li>For a Height of 1 pixels, (bottom - top) = 1. If top = 0, bottom = Height.</li>
        /// <li>For a Depth of 1 pixels, (back - front) = 1. If front = 0, back = Depth. </li>
        /// </ul>
        /// </remarks>
        public ResourceRegion(int left, int top, int front, int right, int bottom, int back)
        {
            Left = left;
            Top = top;
            Front = front;
            Right = right;
            Bottom = bottom;
            Back = back;
        }
    }
}