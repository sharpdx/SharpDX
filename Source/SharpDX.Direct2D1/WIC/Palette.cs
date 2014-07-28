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
    public partial class Palette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Palette"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <msdn-id>ee690319</msdn-id>	
        /// <unmanaged>HRESULT IWICImagingFactory::CreatePalette([Out, Fast] IWICPalette** ppIPalette)</unmanaged>	
        /// <unmanaged-short>IWICImagingFactory::CreatePalette</unmanaged-short>	
        public Palette(ImagingFactory factory)
            : base(IntPtr.Zero)
        {
            factory.CreatePalette(this);
        }

        /// <summary>
        /// Initializes with the specified colors.
        /// </summary>
        /// <typeparam name="T">Type of the color (must be 4 bytes, RGBA)</typeparam>
        /// <param name="colors">The colors.</param>
        /// <exception cref="System.ArgumentException">Color type must be 4 bytes</exception>
        /// <msdn-id>ee719750</msdn-id>
        ///   <unmanaged>HRESULT IWICPalette::InitializeCustom([In, Buffer] void* pColors,[In] unsigned int cCount)</unmanaged>
        ///   <unmanaged-short>IWICPalette::InitializeCustom</unmanaged-short>
        public unsafe void Initialize<T>(T[] colors) where T : struct
        {
            if (Utilities.SizeOf<T>() != 4)
                throw new ArgumentException("Color type must be 4 bytes RGBA");

            void* pColors = Interop.Fixed(colors);
            Initialize((IntPtr)pColors, colors.Length);
        }

        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <msdn-id>ee719744</msdn-id>	
        /// <unmanaged>HRESULT IWICPalette::GetColors([In] unsigned int cCount,[Out, Buffer] void* pColors,[Out] unsigned int* pcActualColors)</unmanaged>	
        /// <unmanaged-short>IWICPalette::GetColors</unmanaged-short>	
        public T[] GetColors<T>() where T : struct
        {
            if (Utilities.SizeOf<T>() != 4)
                throw new ArgumentException("Color type must be 4 bytes RGBA");

            unsafe
            {
                //http://msdn.microsoft.com/en-us/library/windows/desktop/ee719741(v=vs.85).aspx
                int actualCount;
                int count = this.ColorCount;
                var rawColors = new T[count];
                {
                    void* pColors = Interop.Fixed(rawColors);
                    GetColors(count, (IntPtr)pColors, out actualCount);
                }
                if (actualCount == 0)
                    return new T[0];
                    
                if (count != actualCount)
                {
                    rawColors = new T[actualCount];
                    void* pColors = Interop.Fixed(rawColors);
                    GetColors(actualCount, (IntPtr)pColors, out actualCount);
                }
                return rawColors;
            }
        }
    }
}