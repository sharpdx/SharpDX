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
using System;
using SharpDX.DXGI;

namespace SharpDX.Direct2D1
{
    public partial class StrokeStyle
    {
        /// <summary>
        /// Creates an <see cref="SharpDX.Direct2D1.StrokeStyle"/> that describes start cap, dash pattern, and other features of a stroke.	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="properties">a definition for this render target</param>
        public StrokeStyle(Factory factory, StrokeStyleProperties properties) : base(IntPtr.Zero)
        {
            factory.CreateStrokeStyle(ref properties, null, 0, this);
        }

        /// <summary>
        /// Creates an <see cref="SharpDX.Direct2D1.StrokeStyle"/> that describes start cap, dash pattern, and other features of a stroke.	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="properties">A structure that describes the stroke's line cap, dash offset, and other details of a stroke.</param>
        /// <param name="dashes">An array whose elements are set to the length of each dash and space in the dash pattern. The first element sets the length of a dash, the second element sets the length of a space, the third element sets the length of a dash, and so on. The length of each dash and space in the dash pattern is the product of the element value in the array and the stroke width. </param>
        public StrokeStyle(Factory factory, StrokeStyleProperties properties, float[] dashes)
            : base(IntPtr.Zero)
        {
            factory.CreateStrokeStyle(ref properties, dashes, dashes.Length, this);
        }
    }
}
