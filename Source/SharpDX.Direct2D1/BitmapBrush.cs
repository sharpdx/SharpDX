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
    public partial class BitmapBrush
    {
        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.BitmapBrush"/> from the specified bitmap.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="bitmap">The bitmap contents of the new brush.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmapBrush([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_BITMAP_BRUSH_PROPERTIES* bitmapBrushProperties,[In, Optional] const D2D1_BRUSH_PROPERTIES* brushProperties,[Out, Fast] ID2D1BitmapBrush** bitmapBrush)</unmanaged>	
        public BitmapBrush(RenderTarget renderTarget, SharpDX.Direct2D1.Bitmap bitmap)
            : this(renderTarget, bitmap, null, null)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.BitmapBrush"/> from the specified bitmap.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="bitmap">The bitmap contents of the new brush.</param>
        /// <param name="bitmapBrushProperties">The extend modes and interpolation mode of the new brush, or NULL. If this parameter is NULL, the brush defaults to the <see cref="SharpDX.Direct2D1.ExtendMode.Clamp"/> horizontal and vertical extend modes and the <see cref="SharpDX.Direct2D1.BitmapInterpolationMode.Linear"/> interpolation mode. </param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmapBrush([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_BITMAP_BRUSH_PROPERTIES* bitmapBrushProperties,[In, Optional] const D2D1_BRUSH_PROPERTIES* brushProperties,[Out, Fast] ID2D1BitmapBrush** bitmapBrush)</unmanaged>	
        public BitmapBrush(RenderTarget renderTarget, SharpDX.Direct2D1.Bitmap bitmap, SharpDX.Direct2D1.BitmapBrushProperties bitmapBrushProperties)
            : this(renderTarget, bitmap, bitmapBrushProperties, null)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.BitmapBrush"/> from the specified bitmap.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="bitmap">The bitmap contents of the new brush.</param>
        /// <param name="brushProperties">The opacity and transform of the new brush, or NULL. If this parameter is NULL, the brush defaults to an opacity of 1.0f and its transform is the identity matrix.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmapBrush([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_BITMAP_BRUSH_PROPERTIES* bitmapBrushProperties,[In, Optional] const D2D1_BRUSH_PROPERTIES* brushProperties,[Out, Fast] ID2D1BitmapBrush** bitmapBrush)</unmanaged>	
        public BitmapBrush(RenderTarget renderTarget, SharpDX.Direct2D1.Bitmap bitmap, SharpDX.Direct2D1.BrushProperties brushProperties)
            : this(renderTarget, bitmap, null, brushProperties)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.BitmapBrush"/> from the specified bitmap.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="bitmap">The bitmap contents of the new brush.</param>
        /// <param name="bitmapBrushProperties">The extend modes and interpolation mode of the new brush, or NULL. If this parameter is NULL, the brush defaults to the <see cref="SharpDX.Direct2D1.ExtendMode.Clamp"/> horizontal and vertical extend modes and the <see cref="SharpDX.Direct2D1.BitmapInterpolationMode.Linear"/> interpolation mode. </param>
        /// <param name="brushProperties">The opacity and transform of the new brush, or NULL. If this parameter is NULL, the brush defaults to an opacity of 1.0f and its transform is the identity matrix.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmapBrush([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_BITMAP_BRUSH_PROPERTIES* bitmapBrushProperties,[In, Optional] const D2D1_BRUSH_PROPERTIES* brushProperties,[Out, Fast] ID2D1BitmapBrush** bitmapBrush)</unmanaged>	
        public BitmapBrush(RenderTarget renderTarget, SharpDX.Direct2D1.Bitmap bitmap, SharpDX.Direct2D1.BitmapBrushProperties? bitmapBrushProperties, SharpDX.Direct2D1.BrushProperties? brushProperties)
            : base(IntPtr.Zero)
        {
            renderTarget.CreateBitmapBrush(bitmap, bitmapBrushProperties, brushProperties, this);
        }

    }
}
