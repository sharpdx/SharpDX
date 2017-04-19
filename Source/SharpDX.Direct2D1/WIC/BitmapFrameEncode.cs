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
using System.ComponentModel;
using SharpDX.Mathematics.Interop;

namespace SharpDX.WIC
{
    public partial class BitmapFrameEncode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFrameEncode"/> class.
        /// </summary>
        /// <param name="encoder">The encoder.</param>
        /// <unmanaged>HRESULT IWICBitmapEncoder::CreateNewFrame([Out] IWICBitmapFrameEncode** ppIFrameEncode,[Out] IPropertyBag2** ppIEncoderOptions)</unmanaged>	
        public BitmapFrameEncode(BitmapEncoder encoder)
        {
            Options = new BitmapEncoderOptions(IntPtr.Zero);

            encoder.CreateNewFrame(this, Options);
        }

        /// <summary>
        /// Gets the properties to setup before <see cref="Initialize()"/>.
        /// </summary>
        public BitmapEncoderOptions Options { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::Initialize([In, Optional] IPropertyBag2* pIEncoderOptions)</unmanaged>
        public void Initialize()
        {
            Initialize(Options);
        }

        /// <summary>
        /// Sets the <see cref="ColorContext"/> objects for this frame encoder.
        /// </summary>
        /// <param name="colorContextOut">The color contexts to set for the encoder.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::SetColorContexts([In] unsigned int cCount,[In, Buffer] IWICColorContext** ppIColorContext)</unmanaged>	
        public void SetColorContexts(SharpDX.WIC.ColorContext[] colorContextOut)
        {
            SetColorContexts(colorContextOut != null ? colorContextOut.Length : 0, colorContextOut);
        }

        /// <summary>	
        /// <p>Encodes the frame scanlines.</p>	
        /// </summary>	
        /// <param name="lineCount"><dd>  <p>The number of lines to encode.</p> </dd></param>	
        /// <param name="buffer">A data buffer containing the pixels to copy from.</param>	
        /// <param name="totalSizeInBytes">Total size in bytes of pixels to write. If == 0, size is calculated with lineCount * rowStride.</param>
        /// <remarks>	
        /// <p>Successive <strong>WritePixels</strong> calls are assumed to be sequential scanline access in the output image.</p>	
        /// </remarks>	
        /// <msdn-id>ee690158</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::WritePixels([In] unsigned int lineCount,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In, Buffer] void* pbPixels)</unmanaged>	
        /// <unmanaged-short>IWICBitmapFrameEncode::WritePixels</unmanaged-short>	
        public void WritePixels(int lineCount, DataRectangle buffer, int totalSizeInBytes = 0)
        {
            WritePixels(lineCount, buffer.DataPointer, buffer.Pitch, totalSizeInBytes);
        }

        /// <summary>	
        /// <p>Encodes the frame scanlines.</p>	
        /// </summary>	
        /// <param name="lineCount"><dd>  <p>The number of lines to encode.</p> </dd></param>	
        /// <param name="buffer">A data buffer containing the pixels to copy from.</param>
        /// <param name="rowStride">The stride of one row.</param>
        /// <param name="totalSizeInBytes">Total size in bytes of pixels to write. If == 0, size is calculated with lineCount * rowStride.</param>
        /// <remarks>	
        /// <p>Successive <strong>WritePixels</strong> calls are assumed to be sequential scanline access in the output image.</p>	
        /// </remarks>	
        /// <msdn-id>ee690158</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::WritePixels([In] unsigned int lineCount,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In, Buffer] void* pbPixels)</unmanaged>	
        /// <unmanaged-short>IWICBitmapFrameEncode::WritePixels</unmanaged-short>	
        public void WritePixels(int lineCount, IntPtr buffer, int rowStride, int totalSizeInBytes = 0)
        {
            if (totalSizeInBytes == 0)
                totalSizeInBytes = lineCount * rowStride;
            WritePixels(lineCount, rowStride, totalSizeInBytes, buffer);
        }

        /// <summary>	
        /// <p>Encodes the frame scanlines.</p>	
        /// </summary>	
        /// <param name="lineCount"><dd>  <p>The number of lines to encode.</p> </dd></param>	
        /// <param name="stride"><dd>  <p>The <em>stride</em> of the image pixels.</p> </dd></param>	
        /// <param name="pixelBuffer"><dd>  <p>A reference to the pixel buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>Successive <strong>WritePixels</strong> calls are assumed to be sequential scanline access in the output image.</p>	
        /// </remarks>	
        /// <msdn-id>ee690158</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::WritePixels([In] unsigned int lineCount,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In, Buffer] void* pbPixels)</unmanaged>	
        /// <unmanaged-short>IWICBitmapFrameEncode::WritePixels</unmanaged-short>	
        public unsafe void WritePixels<T>(int lineCount, int stride, T[] pixelBuffer) where T : struct
        {
            if ((lineCount * stride) > (Utilities.SizeOf<T>() * pixelBuffer.Length))
                throw new ArgumentException("lineCount * stride must be <= to sizeof(pixelBuffer)");

            WritePixels(lineCount, stride, lineCount * stride, (IntPtr)Interop.Fixed(pixelBuffer));
        }

        /// <summary>	
        /// <p>Encodes a bitmap source.</p>	
        /// </summary>	
        /// <param name="bitmapSource"><dd>  <p>The bitmap source to encode.</p> </dd></param>	
        /// <remarks>	
        /// <p>If <strong>SetSize</strong> is not called prior to calling <strong>WriteSource</strong>, the size given in <em>prc</em> is used if not <strong><c>null</c></strong>. Otherwise, the size of the <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> given in <em>pIBitmapSource</em> is used. </p><p>If <strong>SetPixelFormat</strong> is not called prior to calling <strong>WriteSource</strong>, the pixel format of the <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> given in <em>pIBitmapSource</em> is used.</p><p>If <strong>SetResolution</strong> is not called prior to calling <strong>WriteSource</strong>, the pixel format of <em>pIBitmapSource</em> is used.</p><p>If <strong>SetPalette</strong> is not called prior to calling <strong>WriteSource</strong>, the target pixel format is indexed, and the pixel format of <em>pIBitmapSource</em> matches the encoder frame's pixel format, then the <em>pIBitmapSource</em> pixel format is used.</p><p>When encoding a GIF image, if the global palette is set and the frame level palette is not set directly by the user or by a custom independent software vendor (ISV) GIF codec, <strong>WriteSource</strong> will use the global palette to encode the frame even when <em>pIBitmapSource</em> has a frame level palette.</p><p><strong>Windows Vista:</strong>The source rect width must match the width set through SetSize. Repeated <strong>WriteSource</strong> calls can be made as long as the total accumulated source rect height is the same as set through SetSize.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IWICBitmapFrameEncode::WriteSource']/*"/>	
        /// <msdn-id>ee690159</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::WriteSource([In, Optional] IWICBitmapSource* pIBitmapSource,[In, Optional] WICRect* prc)</unmanaged>	
        /// <unmanaged-short>IWICBitmapFrameEncode::WriteSource</unmanaged-short>	
        public void WriteSource(SharpDX.WIC.BitmapSource bitmapSource)
        {
            WriteSource(bitmapSource, IntPtr.Zero);
        }

        /// <summary>	
        /// <p>Encodes a bitmap source.</p>	
        /// </summary>	
        /// <param name="bitmapSourceRef"><dd>  <p>The bitmap source to encode.</p> </dd></param>	
        /// <param name="rectangleRef"><dd>  <p>The size rectangle of the bitmap source.</p> </dd></param>	
        /// <remarks>	
        /// <p>If <strong>SetSize</strong> is not called prior to calling <strong>WriteSource</strong>, the size given in <em>prc</em> is used if not <strong><c>null</c></strong>. Otherwise, the size of the <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> given in <em>pIBitmapSource</em> is used. </p><p>If <strong>SetPixelFormat</strong> is not called prior to calling <strong>WriteSource</strong>, the pixel format of the <strong><see cref="SharpDX.WIC.BitmapSource"/></strong> given in <em>pIBitmapSource</em> is used.</p><p>If <strong>SetResolution</strong> is not called prior to calling <strong>WriteSource</strong>, the pixel format of <em>pIBitmapSource</em> is used.</p><p>If <strong>SetPalette</strong> is not called prior to calling <strong>WriteSource</strong>, the target pixel format is indexed, and the pixel format of <em>pIBitmapSource</em> matches the encoder frame's pixel format, then the <em>pIBitmapSource</em> pixel format is used.</p><p>When encoding a GIF image, if the global palette is set and the frame level palette is not set directly by the user or by a custom independent software vendor (ISV) GIF codec, <strong>WriteSource</strong> will use the global palette to encode the frame even when <em>pIBitmapSource</em> has a frame level palette.</p><p><strong>Windows Vista:</strong>The source rect width must match the width set through SetSize. Repeated <strong>WriteSource</strong> calls can be made as long as the total accumulated source rect height is the same as set through SetSize.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IWICBitmapFrameEncode::WriteSource']/*"/>	
        /// <msdn-id>ee690159</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::WriteSource([In, Optional] IWICBitmapSource* pIBitmapSource,[In, Optional] WICRect* prc)</unmanaged>	
        /// <unmanaged-short>IWICBitmapFrameEncode::WriteSource</unmanaged-short>	
        public unsafe void WriteSource(SharpDX.WIC.BitmapSource bitmapSourceRef, RawBox rectangleRef)
        {
            WriteSource(bitmapSourceRef, new IntPtr(&rectangleRef));
        }

        protected override unsafe void Dispose(bool disposing)
        {
            if (disposing)
            {
                Options.Dispose();
                Options = null;
            }

            base.Dispose(disposing);
        }
    }
}