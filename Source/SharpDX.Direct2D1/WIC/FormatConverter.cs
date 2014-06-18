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
    public partial class FormatConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatConverter"/> class.
        /// </summary>
        /// <param name="converterInfo">The converter info.</param>
        public FormatConverter(FormatConverterInfo converterInfo) : base(IntPtr.Zero)
        {
            converterInfo.CreateInstance(this);
        }

        /// <summary>
        /// Initializes this instance with the specified bitmap source and format
        /// </summary>
        /// <param name="sourceRef">The source ref.</param>
        /// <param name="dstFormat">The destination format.</param>
        /// <returns></returns>
        public void Initialize(SharpDX.WIC.BitmapSource sourceRef, System.Guid dstFormat)
        {
            Initialize(sourceRef, dstFormat, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatConverter"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public FormatConverter(ImagingFactory factory) : base(IntPtr.Zero)
        {
            factory.CreateFormatConverter(this);
        }
    }
}