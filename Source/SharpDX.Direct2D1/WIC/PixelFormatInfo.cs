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
    public partial class PixelFormatInfo
    {
        /// <summary>
        /// Gets the channel mask.
        /// </summary>
        /// <param name="channelIndex">Index of the channel.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT IWICPixelFormatInfo::GetChannelMask([In] unsigned int uiChannelIndex,[In] unsigned int cbMaskBuffer,[In] void* pbMaskBuffer,[Out] unsigned int* pcbActual)</unmanaged>
        public byte[] GetChannelMask(int channelIndex)
        {
            unsafe
            {
                int actualcount = 0;
                GetChannelMask(channelIndex, actualcount, null, out actualcount);
                if (actualcount == 0)
                    return new byte[0];

                var temp = new byte[actualcount];
                GetChannelMask(channelIndex, actualcount, temp, out actualcount);
                return temp;
            }
        }
    }
}