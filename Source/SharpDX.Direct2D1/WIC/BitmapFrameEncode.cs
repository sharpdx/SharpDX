// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
        /// Encodes the frame scanlines.
        /// </summary>
        /// <param name="lineCount">The line count.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT IWICBitmapFrameEncode::WritePixels([In] unsigned int lineCount,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In, Buffer] unsigned char* pbPixels)</unmanaged>
        public void WritePixels(int lineCount, DataRectangle buffer)
        {
            WritePixels(lineCount, buffer.Pitch, lineCount*buffer.Pitch, buffer.DataPointer);
        }

        public void WriteSource(SharpDX.WIC.BitmapSource bitmapSource)
        {
            WriteSource(bitmapSource, null);
        }
    }
}