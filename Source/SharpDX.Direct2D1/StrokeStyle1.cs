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

namespace SharpDX.Direct2D1
{
    public partial class StrokeStyle1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrokeStyle1"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="strokeStyleProperties">No documentation.</param>
        /// <unmanaged>HRESULT ID2D1Factory1::CreateStrokeStyle([In] const D2D1_STROKE_STYLE_PROPERTIES1* strokeStyleProperties,[In, Buffer, Optional] const float* dashes,[In] unsigned int dashesCount,[Out, Fast] ID2D1StrokeStyle1** strokeStyle)</unmanaged>
        public StrokeStyle1(Factory1 factory, SharpDX.Direct2D1.StrokeStyleProperties1 strokeStyleProperties)
            : base(IntPtr.Zero)
        {
            factory.CreateStrokeStyle(ref strokeStyleProperties, null, 0, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrokeStyle1"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="strokeStyleProperties">No documentation.</param>
        /// <param name="dashes">No documentation.</param>
        /// <unmanaged>HRESULT ID2D1Factory1::CreateStrokeStyle([In] const D2D1_STROKE_STYLE_PROPERTIES1* strokeStyleProperties,[In, Buffer, Optional] const float* dashes,[In] unsigned int dashesCount,[Out, Fast] ID2D1StrokeStyle1** strokeStyle)</unmanaged>
        ///   
        /// <unmanaged>HRESULT ID2D1Factory1::CreateStrokeStyle([In] const D2D1_STROKE_STYLE_PROPERTIES1* strokeStyleProperties,[In, Buffer, Optional] const float* dashes,[In] unsigned int dashesCount,[Out, Fast] ID2D1StrokeStyle1** strokeStyle)</unmanaged>
        /// <remarks>
        /// It is valid to specify a dash array only if <see cref="SharpDX.Direct2D1.DashStyle.Custom"/> is also specified.
        /// </remarks>
        public StrokeStyle1(Factory1 factory, SharpDX.Direct2D1.StrokeStyleProperties1 strokeStyleProperties, float[] dashes)
            : base(IntPtr.Zero)
        {
            factory.CreateStrokeStyle(ref strokeStyleProperties, dashes, dashes.Length, this);
        }
    }
}