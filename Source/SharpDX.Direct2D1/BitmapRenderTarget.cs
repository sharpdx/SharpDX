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
    public partial class BitmapRenderTarget
    {
        /// <summary>	
        ///  Creates a bitmap render target for use during intermediate offscreen drawing that is compatible with the current render target with same size, pixel size and pixel format.
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="options">A value that specifies whether the new render target must be compatible with GDI.</param>
        /// <unmanaged>HRESULT CreateCompatibleRenderTarget([In, Optional] const D2D1_SIZE_F* desiredSize,[In, Optional] const D2D1_SIZE_U* desiredPixelSize,[In, Optional] const D2D1_PIXEL_FORMAT* desiredFormat,[None] D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS options,[Out] ID2D1BitmapRenderTarget** bitmapRenderTarget)</unmanaged>
        public BitmapRenderTarget(RenderTarget renderTarget, SharpDX.Direct2D1.CompatibleRenderTargetOptions options)
            : this(renderTarget, options, null, null, null)
        {
        }

        /// <summary>	
        ///  Creates a bitmap render target for use during intermediate offscreen drawing that is compatible with the current render target with same pixel size and pixel format.
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="options">A value that specifies whether the new render target must be compatible with GDI.</param>
        /// <param name="desiredSize">The desired size of the new render target in device-independent pixels if it should be different from the original render target. For more information, see the Remarks section.</param>
        /// <unmanaged>HRESULT CreateCompatibleRenderTarget([In, Optional] const D2D1_SIZE_F* desiredSize,[In, Optional] const D2D1_SIZE_U* desiredPixelSize,[In, Optional] const D2D1_PIXEL_FORMAT* desiredFormat,[None] D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS options,[Out] ID2D1BitmapRenderTarget** bitmapRenderTarget)</unmanaged>
        public BitmapRenderTarget(RenderTarget renderTarget, SharpDX.Direct2D1.CompatibleRenderTargetOptions options, Size2F desiredSize)
            : this(renderTarget, options, desiredSize, null, null)
        {
        }

        /// <summary>	
        ///  Creates a bitmap render target for use during intermediate offscreen drawing that is compatible with the current render target with same size and pixel size.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="desiredFormat">The desired pixel format and alpha mode of the new render target. If the pixel format is set to DXGI_FORMAT_UNKNOWN, the new render target uses the same pixel format as the original render target. If the alpha mode is <see cref="SharpDX.Direct2D1.AlphaMode.Unknown"/>, the alpha mode of the new render target defaults to D2D1_ALPHA_MODE_PREMULTIPLIED. For information about supported pixel formats, see  {{Supported Pixel  Formats and Alpha Modes}}.</param>
        /// <param name="options">A value that specifies whether the new render target must be compatible with GDI.</param>
        /// <unmanaged>HRESULT CreateCompatibleRenderTarget([In, Optional] const D2D1_SIZE_F* desiredSize,[In, Optional] const D2D1_SIZE_U* desiredPixelSize,[In, Optional] const D2D1_PIXEL_FORMAT* desiredFormat,[None] D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS options,[Out] ID2D1BitmapRenderTarget** bitmapRenderTarget)</unmanaged>
        public BitmapRenderTarget(RenderTarget renderTarget, SharpDX.Direct2D1.CompatibleRenderTargetOptions options, SharpDX.Direct2D1.PixelFormat? desiredFormat)
            : this(renderTarget, options, null, null, desiredFormat)
        {
        }

        /// <summary>	
        ///  Creates a bitmap render target for use during intermediate offscreen drawing that is compatible with the current render target.	
        /// </summary>	
        /// <remarks>	
        /// The pixel size and DPI of the new render target can be altered by specifying values for desiredSize or desiredPixelSize:  If desiredSize is specified but desiredPixelSize is not, the pixel size is computed from the desired size using the parent target DPI. If the desiredSize maps to a integer-pixel size, the DPI of the compatible render target is the same as the DPI of the parent target.  If desiredSize maps to a fractional-pixel size, the pixel size is rounded up to the nearest integer and the DPI for the compatible render target is slightly higher than the DPI of the parent render target. In all cases, the coordinate (desiredSize.width, desiredSize.height) maps to the lower-right corner of the compatible render target.If the desiredPixelSize is specified and desiredSize is not, the DPI of the new render target is the same as the original render target.If both desiredSize and desiredPixelSize are specified, the DPI of the new render target is computed to account for the difference in scale.If neither desiredSize nor desiredPixelSize is specified, the new render target size and DPI match the original render target. 	
        /// </remarks>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="desiredSize">The desired size of the new render target in device-independent pixels if it should be different from the original render target. For more information, see the Remarks section.</param>
        /// <param name="desiredPixelSize">The desired size of the new render target in pixels if it should be different from the original render target. For more information, see the Remarks section.</param>
        /// <param name="desiredFormat">The desired pixel format and alpha mode of the new render target. If the pixel format is set to DXGI_FORMAT_UNKNOWN, the new render target uses the same pixel format as the original render target. If the alpha mode is <see cref="SharpDX.Direct2D1.AlphaMode.Unknown"/>, the alpha mode of the new render target defaults to D2D1_ALPHA_MODE_PREMULTIPLIED. For information about supported pixel formats, see  {{Supported Pixel  Formats and Alpha Modes}}.</param>
        /// <param name="options">A value that specifies whether the new render target must be compatible with GDI.</param>
        /// <unmanaged>HRESULT CreateCompatibleRenderTarget([In, Optional] const D2D1_SIZE_F* desiredSize,[In, Optional] const D2D1_SIZE_U* desiredPixelSize,[In, Optional] const D2D1_PIXEL_FORMAT* desiredFormat,[None] D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS options,[Out] ID2D1BitmapRenderTarget** bitmapRenderTarget)</unmanaged>
        public BitmapRenderTarget(RenderTarget renderTarget, SharpDX.Direct2D1.CompatibleRenderTargetOptions options, Size2F? desiredSize, Size2? desiredPixelSize, SharpDX.Direct2D1.PixelFormat? desiredFormat) : base(IntPtr.Zero)
        {
            renderTarget.CreateCompatibleRenderTarget(desiredSize, desiredPixelSize, desiredFormat, options, this);
        }
    }
}
