// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.IO;
using SharpDX.DXGI;

namespace SharpDX.Direct2D1
{
    public partial class Bitmap
    {
        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmap([In] D2D_SIZE_U size,[In, Optional] const void* srcData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, DrawingSize size)
            : this(renderTarget, size, null, 0, new BitmapProperties(new PixelFormat(Format.Unknown, AlphaMode.Unknown)))
        {
        }

        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <param name="bitmapProperties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmap([In] D2D_SIZE_U size,[In, Optional] const void* srcData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, DrawingSize size, SharpDX.Direct2D1.BitmapProperties bitmapProperties)
            : this(renderTarget, size, null, 0, bitmapProperties)
        {
        }

        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <param name="dataStream">A pointer to the memory location of the image data, or NULL to create an uninitialized bitmap.</param>
        /// <param name="pitch">The byte count of each scanline, which is equal to (the image width in pixels * the number of bytes per pixel) + memory padding. If srcData is NULL, this value is ignored. (Note that pitch is also sometimes called stride.)</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmap([In] D2D_SIZE_U size,[In, Optional] const void* srcData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, DrawingSize size, DataStream dataStream, int pitch)
            : this(renderTarget, size, dataStream, pitch, new BitmapProperties(new PixelFormat(Format.Unknown, AlphaMode.Unknown)))
        {
        }

        /// <summary>	
        /// Creates a Direct2D bitmap from a pointer to in-memory source data.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="size">The dimension of the bitmap to create in pixels.</param>
        /// <param name="dataStream">A pointer to the memory location of the image data, or NULL to create an uninitialized bitmap.</param>
        /// <param name="pitch">The byte count of each scanline, which is equal to (the image width in pixels * the number of bytes per pixel) + memory padding. If srcData is NULL, this value is ignored. (Note that pitch is also sometimes called stride.)</param>
        /// <param name="bitmapProperties">The pixel format and dots per inch (DPI) of the bitmap to create.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmap([In] D2D_SIZE_U size,[In, Optional] const void* srcData,[In] unsigned int pitch,[In] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, DrawingSize size, DataStream dataStream, int pitch, SharpDX.Direct2D1.BitmapProperties bitmapProperties)
            : base(IntPtr.Zero)
        {
            renderTarget.CreateBitmap(size, dataStream == null ? IntPtr.Zero : dataStream.PositionPointer, pitch, bitmapProperties, this);
        }


        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.Bitmap"/> whose data is shared with another resource.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="bitmap">An <see cref="SharpDX.Direct2D1.Bitmap"/> that contains the data to share with the new ID2D1Bitmap. For more information, see the Remarks section.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateSharedBitmap([In] const GUID&amp; riid,[In] void* data,[In, Optional] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, Bitmap bitmap)
            : this(renderTarget, bitmap, null)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.Bitmap"/> whose data is shared with another resource.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="bitmap">An <see cref="SharpDX.Direct2D1.Bitmap"/> that contains the data to share with the new ID2D1Bitmap. For more information, see the Remarks section.</param>
        /// <param name="bitmapProperties">The pixel format  and DPI of the bitmap to create . The <see cref="SharpDX.DXGI.Format"/> portion of the pixel format  must match the <see cref="SharpDX.DXGI.Format"/> of data or the method will fail, but the alpha modes don't have to match. To prevent a  mismatch, you can pass NULL or the value obtained from the {{D2D1::PixelFormat}} helper function. The DPI settings do not have to match those of data. If both dpiX and dpiY are  0.0f, the default DPI, 96, is used.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateSharedBitmap([In] const GUID&amp; riid,[In] void* data,[In, Optional] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, Bitmap bitmap, SharpDX.Direct2D1.BitmapProperties? bitmapProperties)
            : base(IntPtr.Zero)
        {
            renderTarget.CreateSharedBitmap(Utilities.GetGuidFromType(typeof(Bitmap)), bitmap.NativePointer, bitmapProperties, this);
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.Bitmap"/> whose data is shared with another resource.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="surface">An <see cref="SharpDX.DXGI.Surface"/> that contains the data to share with the new ID2D1Bitmap. For more information, see the Remarks section.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateSharedBitmap([In] const GUID&amp; riid,[In] void* data,[In, Optional] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, Surface surface)
            : this(renderTarget, surface, null)
        {
        }

        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.Bitmap"/> whose data is shared with another resource.	
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="surface">An <see cref="SharpDX.DXGI.Surface"/> that contains the data to share with the new ID2D1Bitmap. For more information, see the Remarks section.</param>
        /// <param name="bitmapProperties">The pixel format  and DPI of the bitmap to create . The <see cref="SharpDX.DXGI.Format"/> portion of the pixel format  must match the <see cref="SharpDX.DXGI.Format"/> of data or the method will fail, but the alpha modes don't have to match. To prevent a  mismatch, you can pass NULL or the value obtained from the {{D2D1::PixelFormat}} helper function. The DPI settings do not have to match those of data. If both dpiX and dpiY are  0.0f, the default DPI, 96, is used.</param>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateSharedBitmap([In] const GUID&amp; riid,[In] void* data,[In, Optional] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out, Fast] ID2D1Bitmap** bitmap)</unmanaged>	
        public Bitmap(RenderTarget renderTarget, Surface surface, SharpDX.Direct2D1.BitmapProperties? bitmapProperties)
            : base(IntPtr.Zero)
        {
            renderTarget.CreateSharedBitmap(Utilities.GetGuidFromType(typeof(Surface)), surface.NativePointer, bitmapProperties, this);
        }


        /// <summary>
        /// Creates a Bitmap from a wic bitmap.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="wicBitmapSource">A reference to a <see cref="SharpDX.WIC.BitmapSource"/> wic bitmap.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmapFromWicBitmap([In] IWICBitmapSource* wicBitmapSource,[In, Optional] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out] ID2D1Bitmap** bitmap)</unmanaged>	
        public static Bitmap FromWicBitmap(RenderTarget renderTarget, WIC.BitmapSource wicBitmapSource)
        {
            Bitmap bitmap;
            renderTarget.CreateBitmapFromWicBitmap(wicBitmapSource, null, out bitmap);
            return bitmap;
        }

        /// <summary>
        /// Creates a Bitmap from a wic bitmap.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="wicBitmap">The wic bitmap.</param>
        /// <param name="bitmapProperties">The bitmap properties.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID2D1RenderTarget::CreateBitmapFromWicBitmap([In] IWICBitmapSource* wicBitmapSource,[In, Optional] const D2D1_BITMAP_PROPERTIES* bitmapProperties,[Out] ID2D1Bitmap** bitmap)</unmanaged>	
        public static Bitmap FromWicBitmap(RenderTarget renderTarget, WIC.BitmapSource wicBitmap, SharpDX.Direct2D1.BitmapProperties bitmapProperties)
        {
            Bitmap bitmap;
            renderTarget.CreateBitmapFromWicBitmap(wicBitmap, bitmapProperties, out bitmap);
            return bitmap;
        }


        /// <summary>	
        /// Copies the specified region from the specified bitmap into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the  current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="sourceBitmap">The bitmap to copy from. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromBitmap([In, Optional] const D2D1_POINT_2U* destPoint,[In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_RECT_U* srcRect)</unmanaged>
        public Result FromBitmap(Bitmap sourceBitmap)
        {
            return CopyFromBitmap(null, sourceBitmap, null);
        }

        /// <summary>	
        /// Copies the specified region from the specified bitmap into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the  current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="sourceBitmap">The bitmap to copy from. </param>
        /// <param name="destinationPoint">In the current bitmap, the upper-left corner of the area to which the region specified by srcRect is copied. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromBitmap([In, Optional] const D2D1_POINT_2U* destPoint,[In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_RECT_U* srcRect)</unmanaged>
        public Result FromBitmap(Bitmap sourceBitmap, DrawingPoint destinationPoint)
        {
            return CopyFromBitmap(destinationPoint, sourceBitmap, null);
        }

        /// <summary>	
        /// Copies the specified region from the specified bitmap into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the  current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="sourceBitmap">The bitmap to copy from. </param>
        /// <param name="sourceArea">The area of bitmap to copy. </param>
        /// <param name="destinationPoint">In the current bitmap, the upper-left corner of the area to which the region specified by srcRect is copied. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromBitmap([In, Optional] const D2D1_POINT_2U* destPoint,[In] ID2D1Bitmap* bitmap,[In, Optional] const D2D1_RECT_U* srcRect)</unmanaged>
        public Result FromBitmap(Bitmap sourceBitmap, DrawingPoint destinationPoint, Rectangle sourceArea)
        {
            return CopyFromBitmap(destinationPoint, sourceBitmap, sourceArea);
        }

        /// <summary>	
        /// Copies the specified region from memory into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion; the two bitmap formats should match.  Passing this method invalid input, such as an invalid destination rectangle, can produce unpredictable results, such as a distorted image or device failure. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="pointer">The data to copy. </param>
        /// <param name="pitch">The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of a scanline (one row of pixels in memory). The stride can be computed from the following formula: pixel width * bytes per pixel + memory padding. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromMemory([In, Optional] const D2D1_RECT_U* dstRect,[In] const void* srcData,[None] int pitch)</unmanaged>
        public Result FromMemory(IntPtr pointer, int pitch)
        {
            return CopyFromMemory(null, pointer, pitch);
        }

        /// <summary>	
        /// Copies the specified region from memory into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion; the two bitmap formats should match.  Passing this method invalid input, such as an invalid destination rectangle, can produce unpredictable results, such as a distorted image or device failure. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="memory">The data to copy. </param>
        /// <param name="pitch">The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of a scanline (one row of pixels in memory). The stride can be computed from the following formula: pixel width * bytes per pixel + memory padding. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromMemory([In, Optional] const D2D1_RECT_U* dstRect,[In] const void* srcData,[None] int pitch)</unmanaged>
        public Result FromMemory(byte[] memory, int pitch)
        {
            unsafe
            {
                fixed (void* pMemory = &memory[0]) return CopyFromMemory(null, new IntPtr(pMemory), pitch);
            }
        }

        /// <summary>	
        /// Copies the specified region from memory into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion; the two bitmap formats should match.  Passing this method invalid input, such as an invalid destination rectangle, can produce unpredictable results, such as a distorted image or device failure. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="pointer">The data to copy. </param>
        /// <param name="pitch">The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of a scanline (one row of pixels in memory). The stride can be computed from the following formula: pixel width * bytes per pixel + memory padding. </param>
        /// <param name="destinationArea">In the current bitmap, the upper-left corner of the area to which the region specified by srcRect is copied. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromMemory([In, Optional] const D2D1_RECT_U* dstRect,[In] const void* srcData,[None] int pitch)</unmanaged>
        public Result FromMemory(IntPtr pointer, int pitch, Rectangle destinationArea)
        {
            return CopyFromMemory(destinationArea, pointer, pitch);
        }

        /// <summary>	
        /// Copies the specified region from memory into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion; the two bitmap formats should match.  Passing this method invalid input, such as an invalid destination rectangle, can produce unpredictable results, such as a distorted image or device failure. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="memory">The data to copy. </param>
        /// <param name="pitch">The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of a scanline (one row of pixels in memory). The stride can be computed from the following formula: pixel width * bytes per pixel + memory padding. </param>
        /// <param name="destinationArea">In the current bitmap, the upper-left corner of the area to which the region specified by srcRect is copied. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromMemory([In, Optional] const D2D1_RECT_U* dstRect,[In] const void* srcData,[None] int pitch)</unmanaged>
        public Result FromMemory(byte[] memory, int pitch, Rectangle destinationArea)
        {
            unsafe
            {
                fixed (void* pMemory = &memory[0]) return CopyFromMemory(destinationArea, new IntPtr(pMemory), pitch);
            }
        }

        /// <summary>	
        /// Copies the specified region from the specified render target into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  All clips and layers must be popped off of the render target before calling this method.  The method returns {{D2DERR_RENDER_TARGET_HAS_LAYER_OR_CLIPRECT}} if any clips or layers are currently applied to the render target. 	
        /// </remarks>	
        /// <param name="renderTarget">The render target that contains the region to copy. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromRenderTarget([In, Optional] const D2D1_POINT_2U* destPoint,[In] ID2D1RenderTarget* renderTarget,[In, Optional] const D2D1_RECT_U* srcRect)</unmanaged>
        public Result FromRenderTarget(RenderTarget renderTarget)
        {
            return CopyFromRenderTarget(null, renderTarget, null);
        }

        /// <summary>	
        /// Copies the specified region from the specified render target into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  All clips and layers must be popped off of the render target before calling this method.  The method returns {{D2DERR_RENDER_TARGET_HAS_LAYER_OR_CLIPRECT}} if any clips or layers are currently applied to the render target. 	
        /// </remarks>	
        /// <param name="renderTarget">The render target that contains the region to copy. </param>
        /// <param name="destinationPoint">In the current bitmap, the upper-left corner of the area to which the region specified by srcRect is copied. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromRenderTarget([In, Optional] const D2D1_POINT_2U* destPoint,[In] ID2D1RenderTarget* renderTarget,[In, Optional] const D2D1_RECT_U* srcRect)</unmanaged>
        public Result FromRenderTarget(RenderTarget renderTarget, DrawingPoint destinationPoint)
        {
            return CopyFromRenderTarget(destinationPoint, renderTarget, null);
        }

        /// <summary>	
        /// Copies the specified region from the specified render target into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion, and will fail if the bitmap formats do not match. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  All clips and layers must be popped off of the render target before calling this method.  The method returns {{D2DERR_RENDER_TARGET_HAS_LAYER_OR_CLIPRECT}} if any clips or layers are currently applied to the render target. 	
        /// </remarks>	
        /// <param name="renderTarget">The render target that contains the region to copy. </param>
        /// <param name="destinationPoint">In the current bitmap, the upper-left corner of the area to which the region specified by srcRect is copied. </param>
        /// <param name="sourceArea">The area of renderTarget to copy. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromRenderTarget([In, Optional] const D2D1_POINT_2U* destPoint,[In] ID2D1RenderTarget* renderTarget,[In, Optional] const D2D1_RECT_U* srcRect)</unmanaged>
        public Result FromRenderTarget(RenderTarget renderTarget, DrawingPoint destinationPoint, Rectangle sourceArea)
        {
            return CopyFromRenderTarget(destinationPoint, renderTarget, sourceArea);
        }

        /// <summary>	
        /// Copies the specified region from a stream into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion; the two bitmap formats should match.  Passing this method invalid input, such as an invalid destination rectangle, can produce unpredictable results, such as a distorted image or device failure. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="stream">The stream to copy the data from. </param>
        /// <param name="length">Length in bytes of the data to copy from the stream.</param>
        /// <param name="pitch">The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of a scanline (one row of pixels in memory). The stride can be computed from the following formula: pixel width * bytes per pixel + memory padding. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromMemory([In, Optional] const D2D1_RECT_U* dstRect,[In] const void* srcData,[None] int pitch)</unmanaged>
        public Result FromStream(Stream stream, int pitch, int length)
        {
            return FromMemory(Utilities.ReadStream(stream, ref length), pitch);
        }

        /// <summary>	
        /// Copies the specified region from a stream into the current bitmap. 	
        /// </summary>	
        /// <remarks>	
        /// This method does not update the size of the current bitmap. If the contents of the source bitmap do not fit in the current bitmap, this method fails. Also, note that this method does not perform format conversion; the two bitmap formats should match.  Passing this method invalid input, such as an invalid destination rectangle, can produce unpredictable results, such as a distorted image or device failure. Calling this method may cause the current batch to flush if the bitmap is active in the batch. If the batch that was flushed does not complete successfully, this method fails. However, this method does not clear the error state of the render target on which the batch was flushed. The failing <see cref="int"/> and tag state will be returned at the next call to {{EndDraw}} or {{Flush}}.  	
        /// </remarks>	
        /// <param name="stream">The stream to copy the data from. </param>
        /// <param name="length">Length in bytes of the data to copy from the stream.</param>
        /// <param name="pitch">The stride, or pitch, of the source bitmap stored in srcData. The stride is the byte count of a scanline (one row of pixels in memory). The stride can be computed from the following formula: pixel width * bytes per pixel + memory padding. </param>
        /// <param name="destinationArea">In the current bitmap, the upper-left corner of the area to which the region specified by srcRect is copied. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT ID2D1Bitmap::CopyFromMemory([In, Optional] const D2D1_RECT_U* dstRect,[In] const void* srcData,[None] int pitch)</unmanaged>
        public Result FromStream(Stream stream, int pitch, int length, Rectangle destinationArea)
        {
            return FromMemory(Utilities.ReadStream(stream, ref length), pitch, destinationArea);
        }

        /// <summary>	
        /// Return the dots per inch (DPI) of the bitmap.	
        /// </summary>	
        /// <value>The dots per inch (DPI) of the bitma.</value>
        public DrawingSizeF DotsPerInch
        {
            get
            {
                float y;
                float x;
                GetDpi(out x, out y);
                return new DrawingSizeF(x, y);
            }
        }
    }
}
