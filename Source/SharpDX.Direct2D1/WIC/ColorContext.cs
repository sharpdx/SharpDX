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
    public partial class ColorContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorContext"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateColorContext([Out, Fast] IWICColorContext** ppIWICColorContext)</unmanaged>	
        public ColorContext(ImagingFactory factory)
            : base(IntPtr.Zero)
        {
            factory.CreateColorContext(this);
        }

        /// <summary>
        /// Initializes from memory.
        /// </summary>
        /// <param name="dataPointer">The data pointer.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT IWICColorContext::InitializeFromMemory([In] const void* pbBuffer,[In] unsigned int cbBufferSize)</unmanaged>
        public void InitializeFromMemory(DataPointer dataPointer)
        {
            InitializeFromMemory(dataPointer.Pointer, dataPointer.Size);
        }

        /// <summary>
        /// Gets the color context profile.
        /// </summary>
        public DataStream Profile
        {
            get
            {
                int actualSize;
                GetProfileBytes(0, IntPtr.Zero, out actualSize);
                if (actualSize == 0)
                    return null;
                var buffer = new DataStream(actualSize, true, true);
                GetProfileBytes(actualSize, buffer.DataPointer, out actualSize);
                return buffer;
            }
        }
    }
}