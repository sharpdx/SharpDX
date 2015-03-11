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

namespace SharpDX.WIC
{
    public partial class BitmapFrameDecode 
    {
        /// <summary>
        /// Get the <see cref="ColorContext"/> of the image (if any)
        /// </summary>
        /// <param name="imagingFactory">The factory for creating new color contexts</param>
        /// <param name="colorContexts">The color context array, or null</param>
        /// <remarks>
        /// When the image format does not support color contexts,
        /// <see cref="ResultCode.UnsupportedOperation"/> is returned.
        /// </remarks>
        /// <unmanaged>HRESULT IWICBitmapDecoder::GetColorContexts([In] unsigned int cCount,[Out, Buffer, Optional] IWICColorContext** ppIColorContexts,[Out] unsigned int* pcActualCount)</unmanaged>	
        public Result TryGetColorContexts(ImagingFactory imagingFactory, out ColorContext[] colorContexts)
        {
            return ColorContextsHelper.TryGetColorContexts(GetColorContexts, imagingFactory, out colorContexts);
        }

        /// <summary>
        /// Get the <see cref="ColorContext"/> of the image (if any)
        /// </summary>
        /// <returns>
        /// null if the decoder does not support color contexts;
        /// otherwise an array of zero or more ColorContext objects
        /// </returns>
        /// <unmanaged>HRESULT IWICBitmapDecoder::GetColorContexts([In] unsigned int cCount,[Out, Buffer, Optional] IWICColorContext** ppIColorContexts,[Out] </unmanaged>
        public ColorContext[] TryGetColorContexts(ImagingFactory imagingFactory)
        {
            return ColorContextsHelper.TryGetColorContexts(GetColorContexts, imagingFactory);
        }

        [Obsolete("Use TryGetColorContexts instead")]
        public ColorContext[] ColorContexts
        {
            get { return new ColorContext[0]; }
        }
    }
}