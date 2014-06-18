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

namespace SharpDX.WIC
{
    public partial class BitmapLock
    {
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <unmanaged>HRESULT IWICBitmapLock::GetSize([Out] unsigned int* puiWidth,[Out] unsigned int* puiHeight)</unmanaged>	
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
        /// Gets a pointer to the data.
        /// </summary>
        public DataRectangle Data
        {
            get
            {
                int size;
                var pointer = GetDataPointer(out size);
                return new DataRectangle(pointer, Stride);
            }
        }
    }
}