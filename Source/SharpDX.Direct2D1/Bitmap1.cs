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
    public partial class Bitmap1
    {
        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="deviceContext">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmap([In] D2D_SIZE_U size,[In, Buffer, Optional] const void* sourceData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out, Fast] ID2D1Bitmap1** bitmap)</unmanaged>	
        public Bitmap1(DeviceContext deviceContext, Size2 size)
            : this(deviceContext, size, null, 0, new BitmapProperties1(new PixelFormat(Format.Unknown, AlphaMode.Unknown)))
        {
        }

        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="deviceContext">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <param name="bitmapProperties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmap([In] D2D_SIZE_U size,[In, Buffer, Optional] const void* sourceData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out, Fast] ID2D1Bitmap1** bitmap)</unmanaged>	
        public Bitmap1(DeviceContext deviceContext, Size2 size, SharpDX.Direct2D1.BitmapProperties1 bitmapProperties)
            : this(deviceContext, size, null, 0, bitmapProperties)
        {
        }

        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="deviceContext">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <param name="dataStream">A pointer to the memory location of the image data, or NULL to create an uninitialized bitmap.</param>
        /// <param name="pitch">The byte count of each scanline, which is equal to (the image width in pixels * the number of bytes per pixel) + memory padding. If srcData is NULL, this value is ignored. (Note that pitch is also sometimes called stride.)</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmap([In] D2D_SIZE_U size,[In, Buffer, Optional] const void* sourceData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out, Fast] ID2D1Bitmap1** bitmap)</unmanaged>	
        public Bitmap1(DeviceContext deviceContext, Size2 size, DataStream dataStream, int pitch)
            : this(deviceContext, size, dataStream, pitch, new BitmapProperties1(new PixelFormat(Format.Unknown, AlphaMode.Unknown)))
        {
        }

        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="deviceContext">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <param name="dataStream">A pointer to the memory location of the image data, or NULL to create an uninitialized bitmap.</param>
        /// <param name="pitch">The byte count of each scanline, which is equal to (the image width in pixels * the number of bytes per pixel) + memory padding. If srcData is NULL, this value is ignored. (Note that pitch is also sometimes called stride.)</param>
        /// <param name="bitmapProperties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmap([In] D2D_SIZE_U size,[In, Buffer, Optional] const void* sourceData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out, Fast] ID2D1Bitmap1** bitmap)</unmanaged>	
        public Bitmap1(DeviceContext deviceContext, Size2 size, DataStream dataStream, int pitch, SharpDX.Direct2D1.BitmapProperties1 bitmapProperties)
            : base(IntPtr.Zero)
        {
            deviceContext.CreateBitmap(size, dataStream == null ? IntPtr.Zero : dataStream.PositionPointer, pitch, bitmapProperties, this);
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.Bitmap"/> whose data is shared with another resource.	
        /// </summary>	
        /// <param name="deviceContext">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="surface">An <see cref="SharpDX.DXGI.Surface"/> that contains the data to share with the new ID2D1Bitmap. For more information, see the Remarks section.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmapFromDxgiSurface([In] IDXGISurface* surface,[In, Optional] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out, Fast] ID2D1Bitmap1** bitmap1)</unmanaged>	
        public Bitmap1(DeviceContext deviceContext, Surface surface)
            : base(IntPtr.Zero)
        {
            deviceContext.CreateBitmapFromDxgiSurface(surface, null, this);
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.Bitmap"/> whose data is shared with another resource.	
        /// </summary>	
        /// <param name="deviceContext">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="surface">An <see cref="SharpDX.DXGI.Surface"/> that contains the data to share with the new ID2D1Bitmap. For more information, see the Remarks section.</param>
        /// <param name="bitmapProperties">The pixel format  and DPI of the bitmap to create . The <see cref="SharpDX.DXGI.Format"/> portion of the pixel format  must match the <see cref="SharpDX.DXGI.Format"/> of data or the method will fail, but the alpha modes don't have to match. To prevent a  mismatch, you can pass NULL or the value obtained from the {{D2D1::PixelFormat}} helper function. The DPI settings do not have to match those of data. If both dpiX and dpiY are  0.0f, the default DPI, 96, is used.</param>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmapFromDxgiSurface([In] IDXGISurface* surface,[In, Optional] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out, Fast] ID2D1Bitmap1** bitmap1)</unmanaged>	
        public Bitmap1(DeviceContext deviceContext, Surface surface, SharpDX.Direct2D1.BitmapProperties1 bitmapProperties)
            : base(IntPtr.Zero)
        {
            deviceContext.CreateBitmapFromDxgiSurface(surface, bitmapProperties, this);
        }

        /// <summary>
        /// Creates a Bitmap from a WIC bitmap.
        /// </summary>
        /// <param name="deviceContext">The render target.</param>
        /// <param name="wicBitmapSource">A reference to a <see cref="SharpDX.WIC.BitmapSource"/> WIC bitmap.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmapFromWicBitmap([In] IWICBitmapSource* wicBitmapSource,[In, Optional] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out] ID2D1Bitmap1** bitmap)</unmanaged>	
        public static Bitmap1 FromWicBitmap(DeviceContext deviceContext, WIC.BitmapSource wicBitmapSource)
        {
            Bitmap1 bitmap;
            deviceContext.CreateBitmapFromWicBitmap(wicBitmapSource, null, out bitmap);
            return bitmap;
        }

        /// <summary>
        /// Creates a Bitmap from a WIC bitmap.
        /// </summary>
        /// <param name="deviceContext">The render target.</param>
        /// <param name="wicBitmap">The WIC bitmap.</param>
        /// <param name="bitmapProperties">The bitmap properties.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateBitmapFromWicBitmap([In] IWICBitmapSource* wicBitmapSource,[In, Optional] const D2D1_BITMAP_PROPERTIES1* bitmapProperties,[Out] ID2D1Bitmap1** bitmap)</unmanaged>	
        public static Bitmap1 FromWicBitmap(DeviceContext deviceContext, WIC.BitmapSource wicBitmap, SharpDX.Direct2D1.BitmapProperties1 bitmapProperties)
        {
            Bitmap1 bitmap;
            deviceContext.CreateBitmapFromWicBitmap(wicBitmap, bitmapProperties, out bitmap);
            return bitmap;
        }

        /// <summary>
        /// Maps the given bitmap into memory.
        /// </summary>
        /// <param name="options"><para>The options used in mapping the bitmap into memory.</para></param>	
        /// <returns>a reference to the rectangle that is mapped into memory</returns>	
        /// <remarks>	
        /// The bitmap must have been created with the <see cref="SharpDX.Direct2D1.MapOptions.Read"/> flag specified.The caller should try to unmap the memory as quickly as is feasible to release occupied DMA aperture memory.	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1Bitmap1::Map']/*"/>	
        /// <unmanaged>HRESULT ID2D1Bitmap1::Map([In] D2D1_MAP_OPTIONS options,[Out] D2D1_MAPPED_RECT* mappedRect)</unmanaged>	
        public DataRectangle Map(SharpDX.Direct2D1.MapOptions options)
        {
            MappedRectangle mappedRect;
            Map(options, out mappedRect);
            return new DataRectangle(mappedRect.Bits, mappedRect.Pitch);
        }
    }
}