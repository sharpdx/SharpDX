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

using System.Drawing;

namespace SharpDX.WIC
{
    public partial class BitmapSource
    {
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <unmanaged>HRESULT IWICBitmapSource::GetSize([Out] unsigned int* puiWidth,[Out] unsigned int* puiHeight)</unmanaged>
        public System.Drawing.Size Size
        {
            get
            {
                int width, height;
                GetSize(out width, out height);
                return new System.Drawing.Size(width,height);
            }
        }

        /// <summary>
        /// Copies the pixels.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="stride">The stride.</param>
        /// <param name="output">The output stream.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>
        public SharpDX.Result CopyPixels(System.Drawing.Rectangle rectangle, int stride, DataStream output)
        {
            return CopyPixels(rectangle, stride, (int) output.Length, output.DataPointer);
        }

        /// <summary>
        /// Copies the pixels.
        /// </summary>
        /// <param name="stride">The stride.</param>
        /// <param name="output">The output stream.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT IWICBitmapSource::CopyPixels([In, Optional] const WICRect* prc,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>
        public SharpDX.Result CopyPixels(int stride, DataStream output)
        {
            return CopyPixels(null, stride, (int)output.Length, output.DataPointer);
        }
    }
}