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
    public partial class ImageBrush
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBrush"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="image">The image.</param>
        /// <param name="imageBrushProperties">The image brush properties.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateImageBrush([In] ID2D1Image* image,[In] const D2D1_IMAGE_BRUSH_PROPERTIES* imageBrushProperties,[In, Optional] const D2D1_BRUSH_PROPERTIES* brushProperties,[Out, Fast] ID2D1ImageBrush** imageBrush)</unmanaged>	
        public ImageBrush(DeviceContext context, SharpDX.Direct2D1.Image image, SharpDX.Direct2D1.ImageBrushProperties imageBrushProperties)
            : base(IntPtr.Zero)
        {
            context.CreateImageBrush(image, ref imageBrushProperties, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBrush"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="image">The image.</param>
        /// <param name="imageBrushProperties">The image brush properties.</param>
        /// <param name="brushProperties">The brush properties.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateImageBrush([In] ID2D1Image* image,[In] const D2D1_IMAGE_BRUSH_PROPERTIES* imageBrushProperties,[In, Optional] const D2D1_BRUSH_PROPERTIES* brushProperties,[Out, Fast] ID2D1ImageBrush** imageBrush)</unmanaged>	
        public ImageBrush(DeviceContext context, SharpDX.Direct2D1.Image image, SharpDX.Direct2D1.ImageBrushProperties imageBrushProperties, SharpDX.Direct2D1.BrushProperties brushProperties)
            : base(IntPtr.Zero)
        {
            context.CreateImageBrush(image, ref imageBrushProperties, brushProperties, this);
        }


    }
}