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
using System.IO;

namespace SharpDX.WIC
{
    /// <summary>
    /// Gif bitmap encoder using <see cref="BitmapEncoder"/> initialized with default guid <see cref="ContainerFormatGuids.Gif"/>.
    /// </summary>
    public class GifBitmapEncoder : BitmapEncoder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GifBitmapEncoder"/> class.
        /// </summary>
        /// <param name="nativePtr">The native PTR.</param>
        public GifBitmapEncoder(IntPtr nativePtr)
            : base(nativePtr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GifBitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public GifBitmapEncoder(ImagingFactory factory)
            : base(factory, ContainerFormatGuids.Gif)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GifBitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="stream">The output stream.</param>
        public GifBitmapEncoder(ImagingFactory factory, Stream stream = null)
            : base(factory, ContainerFormatGuids.Gif, stream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GifBitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="stream">The output stream.</param>
        public GifBitmapEncoder(ImagingFactory factory, Guid guidVendorRef, Stream stream = null)
            : base(factory, ContainerFormatGuids.Gif, guidVendorRef, stream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GifBitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="stream">The output stream.</param>
        public GifBitmapEncoder(ImagingFactory factory, WICStream stream = null)
            : base(factory, ContainerFormatGuids.Gif, stream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GifBitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="stream">The output stream.</param>
        public GifBitmapEncoder(ImagingFactory factory, Guid guidVendorRef, WICStream stream = null)
            : base(factory, ContainerFormatGuids.Gif, guidVendorRef, stream)
        {
        }
    }
}