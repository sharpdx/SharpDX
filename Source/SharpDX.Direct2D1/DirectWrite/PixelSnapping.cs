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

namespace SharpDX.DirectWrite
{
    [ShadowAttribute(typeof(PixelSnappingShadow))]
    public partial interface PixelSnapping
    {
        /// <summary>
        /// Determines whether pixel snapping is disabled. The recommended default is FALSE,
        /// unless doing animation that requires subpixel vertical placement.
        /// </summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <returns>Receives TRUE if pixel snapping is disabled or FALSE if it not. </returns>
        /// <unmanaged>HRESULT IsPixelSnappingDisabled([None] void* clientDrawingContext,[Out] BOOL* isDisabled)</unmanaged>
        bool IsPixelSnappingDisabled(object clientDrawingContext);

        /// <summary>	
        ///  Gets a transform that maps abstract coordinates to DIPs. 	
        /// </summary>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>a structure which has transform information for  pixel snapping.</returns>
        /// <unmanaged>HRESULT GetCurrentTransform([None] void* clientDrawingContext,[Out] DWRITE_MATRIX* transform)</unmanaged>
        RawMatrix3x2 GetCurrentTransform(object clientDrawingContext);

        /// <summary>	
        ///  Gets the number of physical pixels per DIP. 	
        /// </summary>	
        /// <remarks>	
        ///  Because a DIP (device-independent pixel) is 1/96 inch,  the pixelsPerDip value is the number of logical pixels per inch divided by 96.	
        /// </remarks>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>the number of physical pixels per DIP</returns>
        /// <unmanaged>HRESULT GetPixelsPerDip([None] void* clientDrawingContext,[Out] FLOAT* pixelsPerDip)</unmanaged>
        float GetPixelsPerDip(object clientDrawingContext);
    }
}
