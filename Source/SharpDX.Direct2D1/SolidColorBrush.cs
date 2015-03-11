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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    public partial class SolidColorBrush
    {
        /// <summary>	
        /// Creates a new <see cref="SharpDX.Direct2D1.SolidColorBrush"/> that has the specified color and opacity. 	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="color">The red, green, blue, and alpha values of the brush's color.</param>
        public SolidColorBrush(RenderTarget renderTarget, RawColor4 color) : this(renderTarget, color, null)
        {
        }

        /// <summary>	
        /// Creates a new <see cref="SharpDX.Direct2D1.SolidColorBrush"/> that has the specified color and opacity. 	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="color">The red, green, blue, and alpha values of the brush's color.</param>
        /// <param name="brushProperties">The base opacity of the brush.</param>
        public SolidColorBrush(RenderTarget renderTarget, RawColor4 color, SharpDX.Direct2D1.BrushProperties? brushProperties)
            : base(IntPtr.Zero)
        {
            renderTarget.CreateSolidColorBrush(color, brushProperties, this);
        }
    }
}
