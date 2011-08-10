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
using SharpDX.Win32;

namespace SharpDX.WIC
{
    public partial class BitmapDecoder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class.
        /// </summary>
        /// <param name="bitmapDecoderInfo">The bitmap decoder info.</param>
        /// <unmanaged>HRESULT IWICBitmapDecoderInfo::CreateInstance([Out, Fast] IWICBitmapDecoder** ppIBitmapDecoder)</unmanaged>
        public BitmapDecoder(BitmapDecoderInfo bitmapDecoderInfo)
        {
            bitmapDecoderInfo.CreateInstance(this);
        }

        /// <summary>
        /// Gets the <see cref="ColorContext"/> objects of the image.
        /// </summary>
        /// <unmanaged>HRESULT IWICBitmapDecoder::GetColorContexts([In] unsigned int cCount,[Out, Buffer, Optional] IWICColorContext** ppIColorContexts,[Out] unsigned int* pcActualCount)</unmanaged>	
        public ColorContext[] ColorContexts
        {
            get
            {
                int count = 0;
                GetColorContexts(0, null, out count);
                if (count == 0)
                    return new ColorContext[0];

                var temp = new ColorContext[count];
                GetColorContexts(count, temp, out count);

                return temp;
            }
        }

        /// <summary>
        /// Queries the capabilities of the decoder based on the specified stream.
        /// </summary>
        /// <param name="stream">The stream to retrieve the decoder capabilities from..</param>
        /// <returns>Capabilities of the decoder</returns>
        /// <unmanaged>HRESULT IWICBitmapDecoder::QueryCapability([In, Optional] IStream* pIStream,[Out] WICBitmapDecoderCapabilities* pdwCapability)</unmanaged>
        public SharpDX.WIC.BitmapDecoderCapabilities QueryCapability(IStream stream)
        {
            return QueryCapability_(ComStream.ToComPointer(stream));
        }


        /// <summary>
        /// Initializes the decoder with the provided stream.
        /// </summary>
        /// <param name="stream">The stream to use for initialization.</param>
        /// <param name="cacheOptions">The cache options.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapDecoder::Initialize([In, Optional] IStream* pIStream,[In] WICDecodeOptions cacheOptions)</unmanaged>
        public SharpDX.Result Initialize(IStream stream, SharpDX.WIC.DecodeOptions cacheOptions)
        {
            return Initialize_(ComStream.ToComPointer(stream), cacheOptions);
        }
    }
}