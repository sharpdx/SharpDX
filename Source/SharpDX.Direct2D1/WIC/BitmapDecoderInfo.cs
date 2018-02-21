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

using SharpDX.Win32;

namespace SharpDX.WIC
{
    public partial class BitmapDecoderInfo
    {
        /// <summary>
        /// Gets the file pattern signatures supported by the decoder.
        /// </summary>
        /// <unmanaged>HRESULT IWICBitmapDecoderInfo::GetPatterns([In] unsigned int cbSizePatterns,[Out, Buffer, Optional] WICBitmapPattern* pPatterns,[Out] unsigned int* pcPatterns,[Out] unsigned int* pcbPatternsActual)</unmanaged>	
        public BitmapPattern[] Patterns
        {
            get
            {
                int actualCount = 0;
                int count = 0;
                GetPatterns(0, null, out count, out actualCount);
                if (actualCount == 0)
                    return new BitmapPattern[0];

                count = actualCount;
                var temp = new BitmapPattern[actualCount];
                GetPatterns(count, temp, out count, out actualCount);

                return temp;
            }
        }
    }
}