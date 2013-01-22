// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using SharpDX;
using SharpDX.IO;
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
            const int width = 512;
            const int height = 512;
            const string filename = "output.jpg";

            var factory = new ImagingFactory();

            WICStream stream = null;

            // ------------------------------------------------------
            // Encode a JPG image
            // ------------------------------------------------------

            // Create a WIC outputstream 
            if (File.Exists(filename))
                File.Delete(filename);

            stream = new WICStream(factory, filename, NativeFileAccess.Write);

            // Initialize a Jpeg encoder with this stream
            var encoder = new JpegBitmapEncoder(factory);
            encoder.Initialize(stream);

            // Create a Frame encoder
            var bitmapFrameEncode = new BitmapFrameEncode(encoder);
            bitmapFrameEncode.Options.ImageQuality = 0.8f;
            bitmapFrameEncode.Initialize();
            bitmapFrameEncode.SetSize(width, height);
            var guid = PixelFormat.Format24bppBGR;
            bitmapFrameEncode.SetPixelFormat(ref guid);

            // Write a pseudo-plasma to a buffer
            int stride = PixelFormat.GetStride(PixelFormat.Format24bppBGR, width);
            var bufferSize = height * stride;
            var buffer = new DataStream(bufferSize, true, true);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    buffer.WriteByte((byte)(x / 2.0 + 20.0 * Math.Sin(y / 40.0)));
                    buffer.WriteByte((byte)(y / 2.0 + 30.0 * Math.Sin(x / 80.0)));
                    buffer.WriteByte((byte)(x / 2.0));
                }
            }

            // Copy the pixels from the buffer to the Wic Bitmap Frame encoder
            bitmapFrameEncode.WritePixels(512, new DataRectangle(buffer.DataPointer, stride));

            // Commit changes
            bitmapFrameEncode.Commit();
            encoder.Commit();
            bitmapFrameEncode.Dispose();
            encoder.Dispose();
            stream.Dispose();

            // ------------------------------------------------------
            // Decode the previous JPG image
            // ------------------------------------------------------

            // Read input
            stream = new WICStream(factory, filename, NativeFileAccess.Read);
            var decoder = new JpegBitmapDecoder(factory);
            decoder.Initialize(stream, DecodeOptions.CacheOnDemand);
            var bitmapFrameDecode = decoder.GetFrame(0);
            var queryReader = bitmapFrameDecode.MetadataQueryReader;

            // Dump MetadataQueryreader
            queryReader.Dump(Console.Out);
            queryReader.Dispose();

            bitmapFrameDecode.Dispose();
            decoder.Dispose();
            stream.Dispose();

            // Dispose
            factory.Dispose();

            System.Diagnostics.Process.Start(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, filename)));
        }
    }
}
