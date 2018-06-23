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

namespace SharpDX.Direct2D1
{
    public partial class BitmapProperties1
    {
        private ColorContext colorContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapProperties1"/> class.
        /// </summary>
        public BitmapProperties1()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapProperties"/> struct.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        public BitmapProperties1(PixelFormat pixelFormat) : this(pixelFormat, 96, 96)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapProperties"/> struct.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <param name="dpiX">The dpi X.</param>
        /// <param name="dpiY">The dpi Y.</param>
        public BitmapProperties1(PixelFormat pixelFormat, float dpiX, float dpiY) : this(pixelFormat, dpiX, dpiY, BitmapOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapProperties1"/> class.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <param name="dpiX">The dpi X.</param>
        /// <param name="dpiY">The dpi Y.</param>
        /// <param name="bitmapOptions">The bitmap options.</param>
        public BitmapProperties1(PixelFormat pixelFormat, float dpiX, float dpiY, BitmapOptions bitmapOptions) : this(pixelFormat, dpiX, dpiY, bitmapOptions, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapProperties1"/> class.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <param name="dpiX">The dpi X.</param>
        /// <param name="dpiY">The dpi Y.</param>
        /// <param name="bitmapOptions">The bitmap options.</param>
        /// <param name="colorContext">The color context.</param>
        public BitmapProperties1(PixelFormat pixelFormat, float dpiX, float dpiY, BitmapOptions bitmapOptions, ColorContext colorContext)
        {
            PixelFormat = pixelFormat;
            DpiX = dpiX;
            DpiY = dpiY;
            BitmapOptions = bitmapOptions;
            ColorContext = colorContext;
        }
    }
}