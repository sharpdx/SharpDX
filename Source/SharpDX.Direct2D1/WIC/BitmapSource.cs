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

namespace SharpDX.WIC
{
    public partial class BitmapSource
    {
        /// <summary>	
        /// <p>Retrieves the pixel width and height of the bitmap.</p>	
        /// </summary>	
        /// <unmanaged>HRESULT IWICBitmapSource::GetSize([Out] unsigned int* puiWidth,[Out] unsigned int* puiHeight)</unmanaged>
        /// <msdn-id>ee690185</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapSource::GetSize([Out] unsigned int* puiWidth,[Out] unsigned int* puiHeight)</unmanaged>	
        /// <unmanaged-short>IWICBitmapSource::GetSize</unmanaged-short>	
        public Size2 Size
        {
            get
            {
                int width, height;
                GetSize(out width, out height);
                return new Size2(width, height);
            }
        }

        /// <summary>	
        /// <p>Instructs the object to produce pixels.</p>	
        /// </summary>	
        /// <param name="rectangle"><dd>  <p>The rectangle to copy. A <strong><c>null</c></strong> value specifies the entire bitmap.</p> </dd></param>	
        /// <param name="stride"><dd>  <p>The stride of the bitmap</p> </dd></param>	
        /// <param name="dataPointer"><dd>  <p>A reference to the buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p><strong>CopyPixels</strong> is one of the two main image processing routines (the other being <strong>Lock</strong>) triggering the actual processing.  It instructs the object to produce pixels according to its algorithm - this may involve decoding a portion of a JPEG stored on disk, copying a block of memory, or even analytically computing a complex gradient.  The algorithm is completely dependent on the object implementing the interface. </p><p> The caller can restrict the operation to a rectangle of interest (ROI) using the prc parameter.  The ROI sub-rectangle must be fully contained in the bounds of the bitmap.  Specifying a <strong><c>null</c></strong> ROI implies that the whole bitmap should be returned. 	
        /// </p><p> The caller controls the memory management and must provide an output buffer (<em>pbBuffer</em>) for the results of the copy along with the buffer's bounds (<em>cbBufferSize</em>).  The cbStride parameter defines the count of bytes between two vertically adjacent pixels in the output buffer.  The caller must ensure that there is sufficient buffer to complete the call based on the width, height and pixel format of the bitmap and the sub-rectangle provided to the copy method. </p><p> If the caller needs to perform numerous copies of an expensive <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> such as a JPEG, it is recommended to create an in-memory <strong><see cref="SharpDX.WIC.Bitmap"/></strong> first. </p>Codec Developer Remarks<p> The callee must only write to the first (prc-&gt;Width*bitsperpixel+7)/8 bytes of each line of the output buffer (in this case, a line is a consecutive string of <em>cbStride</em> bytes). </p>	
        /// </remarks>	
        /// <msdn-id>ee690179</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>	
        /// <unmanaged-short>IWICBitmapSource::CopyPixels</unmanaged-short>	
        public unsafe void CopyPixels(RawBox rectangle, int stride, DataPointer dataPointer)
        {
            CopyPixels(new IntPtr(&rectangle), stride, (int) dataPointer.Size, dataPointer.Pointer);
        }

        /// <summary>	
        /// <p>Instructs the object to produce pixels.</p>	
        /// </summary>	
        /// <param name="stride"><dd>  <p>The stride of the bitmap</p> </dd></param>	
        /// <param name="dataPointer"><dd>  <p>A reference to the buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p><strong>CopyPixels</strong> is one of the two main image processing routines (the other being <strong>Lock</strong>) triggering the actual processing.  It instructs the object to produce pixels according to its algorithm - this may involve decoding a portion of a JPEG stored on disk, copying a block of memory, or even analytically computing a complex gradient.  The algorithm is completely dependent on the object implementing the interface. </p><p> The caller can restrict the operation to a rectangle of interest (ROI) using the prc parameter.  The ROI sub-rectangle must be fully contained in the bounds of the bitmap.  Specifying a <strong><c>null</c></strong> ROI implies that the whole bitmap should be returned. 	
        /// </p><p> The caller controls the memory management and must provide an output buffer (<em>pbBuffer</em>) for the results of the copy along with the buffer's bounds (<em>cbBufferSize</em>).  The cbStride parameter defines the count of bytes between two vertically adjacent pixels in the output buffer.  The caller must ensure that there is sufficient buffer to complete the call based on the width, height and pixel format of the bitmap and the sub-rectangle provided to the copy method. </p><p> If the caller needs to perform numerous copies of an expensive <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> such as a JPEG, it is recommended to create an in-memory <strong><see cref="SharpDX.WIC.Bitmap"/></strong> first. </p>Codec Developer Remarks<p> The callee must only write to the first (prc-&gt;Width*bitsperpixel+7)/8 bytes of each line of the output buffer (in this case, a line is a consecutive string of <em>cbStride</em> bytes). </p>	
        /// </remarks>	
        /// <msdn-id>ee690179</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>	
        /// <unmanaged-short>IWICBitmapSource::CopyPixels</unmanaged-short>	
        public void CopyPixels(int stride, DataPointer dataPointer)
        {
            CopyPixels(IntPtr.Zero, stride, (int)dataPointer.Size, dataPointer.Pointer);
        }

        /// <summary>	
        /// <p>Instructs the object to produce pixels.</p>	
        /// </summary>	
        /// <param name="stride"><dd>  <p>The stride of the bitmap</p> </dd></param>	
        /// <param name="dataPointer"><dd>  <p>A reference to the buffer.</p> </dd></param>	
        /// <param name="size">Size of the buffer in bytes.</param>	
        /// <remarks>	
        /// <p><strong>CopyPixels</strong> is one of the two main image processing routines (the other being <strong>Lock</strong>) triggering the actual processing.  It instructs the object to produce pixels according to its algorithm - this may involve decoding a portion of a JPEG stored on disk, copying a block of memory, or even analytically computing a complex gradient.  The algorithm is completely dependent on the object implementing the interface. </p><p> The caller can restrict the operation to a rectangle of interest (ROI) using the prc parameter.  The ROI sub-rectangle must be fully contained in the bounds of the bitmap.  Specifying a <strong><c>null</c></strong> ROI implies that the whole bitmap should be returned. 	
        /// </p><p> The caller controls the memory management and must provide an output buffer (<em>pbBuffer</em>) for the results of the copy along with the buffer's bounds (<em>cbBufferSize</em>).  The cbStride parameter defines the count of bytes between two vertically adjacent pixels in the output buffer.  The caller must ensure that there is sufficient buffer to complete the call based on the width, height and pixel format of the bitmap and the sub-rectangle provided to the copy method. </p><p> If the caller needs to perform numerous copies of an expensive <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> such as a JPEG, it is recommended to create an in-memory <strong><see cref="SharpDX.WIC.Bitmap"/></strong> first. </p>Codec Developer Remarks<p> The callee must only write to the first (prc-&gt;Width*bitsperpixel+7)/8 bytes of each line of the output buffer (in this case, a line is a consecutive string of <em>cbStride</em> bytes). </p>	
        /// </remarks>	
        /// <msdn-id>ee690179</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>	
        /// <unmanaged-short>IWICBitmapSource::CopyPixels</unmanaged-short>	
        public void CopyPixels(int stride, IntPtr dataPointer, int size)
        {
            CopyPixels(IntPtr.Zero, stride, size, dataPointer);
        }

        /// <summary>
        /// <p>Instructs the object to produce pixels.</p>
        /// </summary>
        /// <typeparam name="T">Type of a pixel. This parameter must exactly match a pixel like using <see cref="Color"/> for a 32bit RGBA color or <see cref="RawVector4"/> for a 64bits for a RGBA 4 floats color.</typeparam>
        /// <param name="rectangle"><dd>  <p>The rectangle to copy. A <strong><c>null</c></strong> value specifies the entire bitmap.</p> </dd></param>
        /// <param name="output">The destination array. The size of the array must be sizeof(pixel) * rectangle.Width * rectangle.Height</param>
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok" /></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result" /></strong> error code.</p></returns>
        /// <exception cref="System.ArgumentException">output.Length must be equal to Width * Height</exception>
        /// <msdn-id>ee690179</msdn-id>
        ///   <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>
        ///   <unmanaged-short>IWICBitmapSource::CopyPixels</unmanaged-short>
        /// <remarks><p><strong>CopyPixels</strong> is one of the two main image processing routines (the other being <strong>Lock</strong>) triggering the actual processing.  It instructs the object to produce pixels according to its algorithm - this may involve decoding a portion of a JPEG stored on disk, copying a block of memory, or even analytically computing a complex gradient.  The algorithm is completely dependent on the object implementing the interface. </p><p> The caller can restrict the operation to a rectangle of interest (ROI) using the prc parameter.  The ROI sub-rectangle must be fully contained in the bounds of the bitmap.  Specifying a <strong><c>null</c></strong> ROI implies that the whole bitmap should be returned.
        ///   </p><p> The caller controls the memory management and must provide an output buffer (<em>pbBuffer</em>) for the results of the copy along with the buffer's bounds (<em>cbBufferSize</em>).  The cbStride parameter defines the count of bytes between two vertically adjacent pixels in the output buffer.  The caller must ensure that there is sufficient buffer to complete the call based on the width, height and pixel format of the bitmap and the sub-rectangle provided to the copy method. </p><p> If the caller needs to perform numerous copies of an expensive <strong><see cref="SharpDX.WIC.BitmapSource" /></strong> such as a JPEG, it is recommended to create an in-memory <strong><see cref="SharpDX.WIC.Bitmap" /></strong> first. </p>Codec Developer Remarks<p> The callee must only write to the first (prc-&gt;Width*bitsperpixel+7)/8 bytes of each line of the output buffer (in this case, a line is a consecutive string of <em>cbStride</em> bytes). </p></remarks>
        public unsafe void CopyPixels<T>(RawBox rectangle, T[] output) where T : struct
        {
            if ((rectangle.Width * rectangle.Height) != output.Length)
                throw new ArgumentException("output.Length must be equal to Width * Height");

            CopyPixels(new IntPtr(&rectangle), rectangle.Width * Utilities.SizeOf<T>(), (int)output.Length * Utilities.SizeOf<T>(), (IntPtr)Interop.Fixed(output));
        }

        /// <summary>	
        /// <p>Instructs the object to produce pixels.</p>	
        /// </summary>	
        /// <typeparam name="T">Type of a pixel. This parameter must exactly match a pixel like using <see cref="Color"/> for a 32bit RGBA color or <see cref="RawVector4"/> for a 64bits for a RGBA 4 floats color.</typeparam>
        /// <param name="output">The destination array. The size of the array must be sizeof(pixel) * Width * Height</param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p><strong>CopyPixels</strong> is one of the two main image processing routines (the other being <strong>Lock</strong>) triggering the actual processing.  It instructs the object to produce pixels according to its algorithm - this may involve decoding a portion of a JPEG stored on disk, copying a block of memory, or even analytically computing a complex gradient.  The algorithm is completely dependent on the object implementing the interface. </p><p> The caller can restrict the operation to a rectangle of interest (ROI) using the prc parameter.  The ROI sub-rectangle must be fully contained in the bounds of the bitmap.  Specifying a <strong><c>null</c></strong> ROI implies that the whole bitmap should be returned. 	
        /// </p><p> The caller controls the memory management and must provide an output buffer (<em>pbBuffer</em>) for the results of the copy along with the buffer's bounds (<em>cbBufferSize</em>).  The cbStride parameter defines the count of bytes between two vertically adjacent pixels in the output buffer.  The caller must ensure that there is sufficient buffer to complete the call based on the width, height and pixel format of the bitmap and the sub-rectangle provided to the copy method. </p><p> If the caller needs to perform numerous copies of an expensive <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> such as a JPEG, it is recommended to create an in-memory <strong><see cref="SharpDX.WIC.Bitmap"/></strong> first. </p>Codec Developer Remarks<p> The callee must only write to the first (prc-&gt;Width*bitsperpixel+7)/8 bytes of each line of the output buffer (in this case, a line is a consecutive string of <em>cbStride</em> bytes). </p>	
        /// </remarks>	
        /// <msdn-id>ee690179</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>	
        /// <unmanaged-short>IWICBitmapSource::CopyPixels</unmanaged-short>	
        public unsafe void CopyPixels<T>(T[] output) where T : struct
        {
            var size = Size;
            if ( (size.Width * size.Height) != output.Length)
                throw new ArgumentException("output.Length must be equal to Width * Height");

            CopyPixels(IntPtr.Zero, Size.Width * Utilities.SizeOf<T>(), (int)output.Length * Utilities.SizeOf<T>(), (IntPtr)Interop.Fixed(output));
        }

        /// <summary>
        /// <p>Instructs the object to produce pixels.</p>
        /// </summary>
        /// <param name="output">The destination array. The size of the array must be sizeof(pixel) * Width * Height</param>
        /// <param name="stride">The stride (number of bytes per row).</param>
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok" /></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result" /></strong> error code.</p></returns>
        /// <exception cref="System.ArgumentException">output.Length must be equal to Width * Height</exception>
        /// <msdn-id>ee690179</msdn-id>
        ///   <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>
        ///   <unmanaged-short>IWICBitmapSource::CopyPixels</unmanaged-short>
        /// <remarks><p><strong>CopyPixels</strong> is one of the two main image processing routines (the other being <strong>Lock</strong>) triggering the actual processing.  It instructs the object to produce pixels according to its algorithm - this may involve decoding a portion of a JPEG stored on disk, copying a block of memory, or even analytically computing a complex gradient.  The algorithm is completely dependent on the object implementing the interface. </p><p> The caller can restrict the operation to a rectangle of interest (ROI) using the prc parameter.  The ROI sub-rectangle must be fully contained in the bounds of the bitmap.  Specifying a <strong><c>null</c></strong> ROI implies that the whole bitmap should be returned.
        ///   </p><p> The caller controls the memory management and must provide an output buffer (<em>pbBuffer</em>) for the results of the copy along with the buffer's bounds (<em>cbBufferSize</em>).  The cbStride parameter defines the count of bytes between two vertically adjacent pixels in the output buffer.  The caller must ensure that there is sufficient buffer to complete the call based on the width, height and pixel format of the bitmap and the sub-rectangle provided to the copy method. </p><p> If the caller needs to perform numerous copies of an expensive <strong><see cref="SharpDX.WIC.BitmapSource" /></strong> such as a JPEG, it is recommended to create an in-memory <strong><see cref="SharpDX.WIC.Bitmap" /></strong> first. </p>Codec Developer Remarks<p> The callee must only write to the first (prc-&gt;Width*bitsperpixel+7)/8 bytes of each line of the output buffer (in this case, a line is a consecutive string of <em>cbStride</em> bytes). </p></remarks>
        public unsafe void CopyPixels(RawRectangle rectangle, byte[] output, int stride)
        {
            if (output == null) throw new ArgumentNullException("output");
            if (stride <= 0) throw new ArgumentOutOfRangeException("stride", "Must be > 0");
            if ((output.Length % stride) != 0)
                throw new ArgumentException("output.Length must be a modulo of stride");

            CopyPixels(new IntPtr(&rectangle), stride, output.Length, (IntPtr)Interop.Fixed(output));
        }

        /// <summary>
        /// <p>Instructs the object to produce pixels.</p>
        /// </summary>
        /// <param name="output">The destination array. The size of the array must be sizeof(pixel) * Width * Height</param>
        /// <param name="stride">The stride (number of bytes per row).</param>
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok" /></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result" /></strong> error code.</p></returns>
        /// <exception cref="System.ArgumentException">output.Length must be equal to Width * Height</exception>
        /// <msdn-id>ee690179</msdn-id>
        ///   <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>
        ///   <unmanaged-short>IWICBitmapSource::CopyPixels</unmanaged-short>
        /// <remarks><p><strong>CopyPixels</strong> is one of the two main image processing routines (the other being <strong>Lock</strong>) triggering the actual processing.  It instructs the object to produce pixels according to its algorithm - this may involve decoding a portion of a JPEG stored on disk, copying a block of memory, or even analytically computing a complex gradient.  The algorithm is completely dependent on the object implementing the interface. </p><p> The caller can restrict the operation to a rectangle of interest (ROI) using the prc parameter.  The ROI sub-rectangle must be fully contained in the bounds of the bitmap.  Specifying a <strong><c>null</c></strong> ROI implies that the whole bitmap should be returned.
        ///   </p><p> The caller controls the memory management and must provide an output buffer (<em>pbBuffer</em>) for the results of the copy along with the buffer's bounds (<em>cbBufferSize</em>).  The cbStride parameter defines the count of bytes between two vertically adjacent pixels in the output buffer.  The caller must ensure that there is sufficient buffer to complete the call based on the width, height and pixel format of the bitmap and the sub-rectangle provided to the copy method. </p><p> If the caller needs to perform numerous copies of an expensive <strong><see cref="SharpDX.WIC.BitmapSource" /></strong> such as a JPEG, it is recommended to create an in-memory <strong><see cref="SharpDX.WIC.Bitmap" /></strong> first. </p>Codec Developer Remarks<p> The callee must only write to the first (prc-&gt;Width*bitsperpixel+7)/8 bytes of each line of the output buffer (in this case, a line is a consecutive string of <em>cbStride</em> bytes). </p></remarks>
        public unsafe void CopyPixels(byte[] output, int stride)
        {
            if(output == null) throw new ArgumentNullException("output");
            if (stride <= 0) throw new ArgumentOutOfRangeException("stride", "Must be > 0");
            if ((output.Length % stride)!= 0)
                throw new ArgumentException("output.Length must be a modulo of stride");

            CopyPixels(IntPtr.Zero, stride, output.Length, (IntPtr)Interop.Fixed(output));
        }
    }
}