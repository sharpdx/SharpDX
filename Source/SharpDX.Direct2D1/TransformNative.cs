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
#if DIRECTX11_1
using System;
namespace SharpDX.Direct2D1
{
    public partial class TransformNative
    {
        /// <inheritdoc/>
        public void MapOutputRectangleToInputRectangles(SharpDX.Rectangle outputRect, SharpDX.Rectangle[] inputRects)
        {
            MapOutputRectToInputRects_(outputRect, inputRects, inputRects.Length);
        }

        /// <inheritdoc/>
        public SharpDX.Rectangle MapInputRectanglesToOutputRectangle(SharpDX.Rectangle[] inputRects, SharpDX.Rectangle[] inputOpaqueSubRects, out SharpDX.Rectangle outputOpaqueSubRect)
        {
            return MapInputRectsToOutputRect_(inputRects, inputOpaqueSubRects, inputRects.Length, out outputOpaqueSubRect);
        }

        /// <inheritdoc/>
        public Rectangle MapInvalidRect(int inputIndex, Rectangle invalidInputRect)
        {
            return MapInvalidRect_(inputIndex, invalidInputRect);
        }
    }
}
#endif