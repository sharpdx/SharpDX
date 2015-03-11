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
    public partial class DrawingStateBlock
    {
        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.DrawingStateBlock"/> that can be used with the {{SaveDrawingState}} and {{RestoreDrawingState}} methods of a render target.	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        public DrawingStateBlock(Factory factory) : this(factory, null, null)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.DrawingStateBlock"/> that can be used with the {{SaveDrawingState}} and {{RestoreDrawingState}} methods of a render target.	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="drawingStateDescription">A structure that contains antialiasing, transform, and tags  information.</param>
        public DrawingStateBlock(Factory factory, SharpDX.Direct2D1.DrawingStateDescription drawingStateDescription) : this(factory, drawingStateDescription, null)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.DrawingStateBlock"/> that can be used with the {{SaveDrawingState}} and {{RestoreDrawingState}} methods of a render target.	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="textRenderingParams">Optional text parameters that indicate how text should be rendered.  </param>
        public DrawingStateBlock(Factory factory, SharpDX.DirectWrite.RenderingParams textRenderingParams)
            : this(factory, null, textRenderingParams)
        {
        }
 
        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.DrawingStateBlock"/> that can be used with the {{SaveDrawingState}} and {{RestoreDrawingState}} methods of a render target.	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="drawingStateDescription">A structure that contains antialiasing, transform, and tags  information.</param>
        /// <param name="textRenderingParams">Optional text parameters that indicate how text should be rendered.  </param>
        public DrawingStateBlock(Factory factory, SharpDX.Direct2D1.DrawingStateDescription? drawingStateDescription, SharpDX.DirectWrite.RenderingParams textRenderingParams)
            : base(IntPtr.Zero)
        {
            factory.CreateDrawingStateBlock(drawingStateDescription, textRenderingParams, this);
        }
    }
}
