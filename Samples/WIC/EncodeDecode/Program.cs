// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

using System.IO;
using SharpDX.WIC;

namespace EncodeDecode
{
    static class Program
    {
        /// <summary>
        /// SharpDX WIC sample. Encode to JPG and decode.
        /// </summary>
        static void Main()
        {
            var factory = new ImagingFactory();

            var stream = new WICStream(factory, "output.jpg", FileAccess.Write);

            var encoder = new JpegBitmapEncoder(factory);
            encoder.Initialize(stream);

            var bitmapFrameEncode = new BitmapFrameEncode(encoder);
            bitmapFrameEncode.Options.ImageQuality = 0.8f;
            bitmapFrameEncode.Initialize();

            bitmapFrameEncode.SetSize(512, 512);

            bitmapFrameEncode.PixelFormat = PixelFormat.Format24bppBGR;

            //bitmapFrameEncode.WritePixels()

            bitmapFrameEncode.Commit();

            encoder.Commit();
        }
    }
}
