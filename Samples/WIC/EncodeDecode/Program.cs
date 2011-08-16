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
using SharpDX;
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
            const string filename = "output.jpg";

            var factory = new ImagingFactory();

            if (File.Exists(filename))
                File.Delete(filename);

            var stream = new WICStream(factory, filename, FileAccess.Write);

            var encoder = new JpegBitmapEncoder(factory);
            encoder.Initialize(stream);

            var bitmapFrameEncode = new BitmapFrameEncode(encoder);
            bitmapFrameEncode.Options.ImageQuality = 0.8f;
            bitmapFrameEncode.Initialize();

            const int width = 512;
            const int height = 512;

            bitmapFrameEncode.SetSize(width, height);

            bitmapFrameEncode.PixelFormat = PixelFormat.Format24bppBGR;

            int stride = (width * 24 + 7) / 8/***WICGetStride***/;
            var bufferSize = height * stride;

            var buffer = new DataStream(bufferSize, true, true);
            for(int i = 0; i < bufferSize; i++)
                buffer.WriteByte((byte)i);

            bitmapFrameEncode.WritePixels(512, new DataRectangle(buffer.DataPointer, stride));

            bitmapFrameEncode.Commit();

            encoder.Commit();

            bitmapFrameEncode.Dispose();
            encoder.Dispose();
            stream.Dispose();
            factory.Dispose();
        }
    }
}
