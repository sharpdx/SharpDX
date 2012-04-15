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
    public partial class Palette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Palette"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreatePalette([Out, Fast] IWICPalette** ppIPalette)</unmanaged>	
        public Palette(ImagingFactory factory)
            : base(IntPtr.Zero)
        {
            factory.CreatePalette(this);
        }

        /// <summary>
        /// Initializes with the specified colors.
        /// </summary>
        /// <param name="colors">The colors.</param>
        /// <unmanaged>HRESULT IWICPalette::InitializeCustom([In, Buffer] unsigned int* pColors,[In] unsigned int cCount)</unmanaged>	
        public void Initialize(Color4[] colors)
        {
            var rawColors = new int[colors.Length];
            for (int i = 0; i < colors.Length; i++)
                rawColors[i] = colors[i].ToArgb();
            Initialize(rawColors, rawColors.Length);
        }

        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <unmanaged>HRESULT IWICPalette::GetColors([In] unsigned int cCount,[Out, Buffer] unsigned int* pColors,[Out] unsigned int* pcActualColors)</unmanaged>	
        public Color4[] Colors
        {
            get
            {
                unsafe
                {
                    int actualCount = 0;
                    GetColors(actualCount, null, out actualCount);
                    if (actualCount == 0)
                        return new Color4[0];

                    var rawColors = new int[actualCount];
                    GetColors(actualCount, rawColors, out actualCount);
                    var result = new Color4[actualCount];
                    for (int i = 0; i < rawColors.Length; i++)
                        result[i] = new Color4(rawColors[i]);
                    return result;
                }
            }
        }
    }
}