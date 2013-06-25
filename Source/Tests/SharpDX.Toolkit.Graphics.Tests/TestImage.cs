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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using SharpDX.DXGI;
using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics.Tests
{
    /// <summary>
    /// Tests for <see cref="Texture2DBase"/>
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.Toolkit.Graphics.Image")]
    public unsafe class TestImage
    {
        private string dxsdkDir;

        [TestFixtureSetUp] 
        public void Initialize()
        {
            dxsdkDir = Environment.GetEnvironmentVariable("DXSDK_DIR");

            if (string.IsNullOrEmpty(dxsdkDir))
                throw new NotSupportedException("Install DirectX SDK June 2010 to run this test (DXSDK_DIR environment variable is missing).");            
        }

        /// <summary>
        /// Tests Image TKTX format
        /// </summary>
        [Test]
        public void TestTKTX()
        {
            var image = Image.Load("logo.png");
            image.Save("logo.tktx", ImageFileType.Tktx);
            
            var image2 = Image.Load("logo.tktx");
            var buffer = new MemoryStream();
            image2.Save(buffer, ImageFileType.Tktx);
            
            buffer.Position = 0;
            var image3 = Image.Load(buffer);
            CompareImage(image, image3);

            image3.Save("logo-tktx.png", ImageFileType.Png);

            image2.Dispose();
            image3.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// Tests Image 1D.
        /// </summary>
        [Test]
        public void TestImage1D()
        {
            const int Size = 256; // must be > 256 to work
            int BufferCount = (int)Math.Log(Size, 2) + 1;
            int ExpectedSizePerArraySlice = Size * 2 - 1;

            var source = Image.New1D(Size, true, PixelFormat.R8.UNorm);

            // 9 buffers: 256 + 128 + 64 + 32 + 16 + 8 + 4 + 2 + 1
            Assert.AreEqual(source.TotalSizeInBytes, ExpectedSizePerArraySlice);
            Assert.AreEqual(source.PixelBuffer.Count, BufferCount);

            // Check with array size
            var dest = Image.New1D(Size, true, PixelFormat.R8.UNorm, 6);
            Assert.AreEqual(dest.TotalSizeInBytes, ExpectedSizePerArraySlice * 6);
            Assert.AreEqual(dest.PixelBuffer.Count, BufferCount * 6);

            ManipulateImage(source, dest, 5, 0, 0);

            // Dispose images
            source.Dispose();
            dest.Dispose();
        }

        /// <summary>
        /// Tests Image 2D.
        /// </summary>
        [Test]
        public void TestImage2D()
        {
            const int Size = 256; // must be > 256 to work
            int BufferCount = (int)Math.Log(Size, 2) + 1;
            int ExpectedSizePerArraySlice = ((int)Math.Pow(4, BufferCount) - 1) / 3;

            var source = Image.New2D(Size, Size, true, PixelFormat.R8.UNorm);

            // 9 buffers: 256 + 128 + 64 + 32 + 16 + 8 + 4 + 2 + 1
            Assert.AreEqual(source.TotalSizeInBytes, ExpectedSizePerArraySlice);
            Assert.AreEqual(source.PixelBuffer.Count, BufferCount);

            // Check with array size
            var dest = Image.New2D(Size, Size, true, PixelFormat.R8.UNorm, 6);
            Assert.AreEqual(dest.TotalSizeInBytes, ExpectedSizePerArraySlice * 6);
            Assert.AreEqual(dest.PixelBuffer.Count, BufferCount * 6);

            ManipulateImage(source, dest, 5, 0, 0);

            // Dispose images
            source.Dispose();
            dest.Dispose();
        }

        /// <summary>
        /// Tests Image 2D.
        /// </summary>
        [Test]
        public void TestImage3D()
        {
            const int Size = 64; // must be > 256 to work
            int BufferCount = (int)Math.Log(Size, 2) + 1;
            // BufferSize(x) = 1/7 (8^(x+1)-1)
            int ExpectedTotalBufferCount = (int)Math.Pow(2, BufferCount) - 1;
            int ExpectedSizePerArraySlice = ((int)Math.Pow(8, BufferCount) - 1) / 7;

            var source = Image.New3D(Size, Size, Size, true, PixelFormat.R8.UNorm);

            // 9 buffers: 256 + 128 + 64 + 32 + 16 + 8 + 4 + 2 + 1
            Assert.AreEqual(source.TotalSizeInBytes, ExpectedSizePerArraySlice);
            Assert.AreEqual(source.PixelBuffer.Count, ExpectedTotalBufferCount);

            var dest = Image.New3D(Size, Size, Size, true, PixelFormat.R8.UNorm);

            ManipulateImage(source, dest, 0, 5, 0);

            // Dispose images
            source.Dispose();
            dest.Dispose();
        }

        private void ManipulateImage(Image source, Image dest, int arrayIndex, int zIndex, int mipIndex)
        {
            // Use Set Pixel
            var fromPixelBuffer = source.PixelBuffer[0];
            var toPixelBuffer = dest.PixelBuffer[arrayIndex, zIndex, mipIndex];

            fromPixelBuffer.SetPixel(0, 0, new PixelData.R8(255));
            fromPixelBuffer.SetPixel(16, 0, new PixelData.R8(128));
            fromPixelBuffer.CopyTo(toPixelBuffer);

            Assert.True(Utilities.CompareMemory(fromPixelBuffer.DataPointer, toPixelBuffer.DataPointer, fromPixelBuffer.BufferStride));

            // Use Get Pixels
            var fromPixels = fromPixelBuffer.GetPixels<byte>();
            Assert.AreEqual(fromPixels.Length, source.Description.Width * source.Description.Height);

            // Check values
            Assert.AreEqual(fromPixels[0], 255);
            Assert.AreEqual(fromPixels[16], 128);

            // Use Set Pixels
            fromPixels[0] = 1;
            fromPixels[16] = 2;
            fromPixelBuffer.SetPixels(fromPixels);

            // Use Get Pixel
            Assert.AreEqual(fromPixelBuffer.GetPixel<byte>(0, 0), 1);
            Assert.AreEqual(fromPixelBuffer.GetPixel<byte>(16, 0), 2);
        }

        [Test]
        public void TestPerfLoadSave()
        {
            var image = Image.Load("map.bmp");

            const int Count = 100; // Change this to perform memory benchmarks
            var types = new ImageFileType[] {
                ImageFileType.Dds,
                ImageFileType.Jpg,
                ImageFileType.Png,
                ////ImageFileType.Gif,
                ////ImageFileType.Bmp,
                ImageFileType.Tiff,
                ImageFileType.Tktx,
            };
            var clock = Stopwatch.StartNew();
            foreach (var imageFileType in types)
            {
                clock.Restart();
                for (int i = 0; i < Count; i++)
                {
                    image.Save("map2.bin", imageFileType);
                }
                Console.WriteLine("Save [{0}] {1} in {2}ms", Count, imageFileType, clock.ElapsedMilliseconds);
            }

            image.Dispose();
        }

        private long testMemoryBefore;

        [Test]
        public void TestLoadAndSave()
        {

            GC.Collect();
            GC.WaitForFullGCComplete();
            testMemoryBefore = GC.GetTotalMemory(true);

            var files = new List<string>();
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.dds", SearchOption.AllDirectories));
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.bmp", SearchOption.AllDirectories));

            const int Count = 1; // Change this to perform memory benchmarks
            var types = new ImageFileType[] {
                ImageFileType.Dds,
                ImageFileType.Jpg,
                ImageFileType.Png,
                ////ImageFileType.Gif,
                ////ImageFileType.Bmp,
                ImageFileType.Tiff,
                ImageFileType.Tktx,
            };

            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < types.Length; j++)
                {
                    Console.Write("[{0}] ", i);
                    ProcessFiles(files, types[j]);
                }
            }
        }

        private void ProcessFiles(IEnumerable<string>  files, ImageFileType intermediateFormat)
        {

            Console.WriteLine("Testing {0}", intermediateFormat);
            Console.Out.Flush();
            int imageCount = 0;
            var clock = Stopwatch.StartNew();
            foreach (var file in files)
            {
                // Load an image from a file and dispose it.
                var image = Image.Load(file);
                image.Dispose();

                // Load an image from a buffer
                var buffer = File.ReadAllBytes(file);

                using (image = Image.Load(buffer))
                {

                    // Write this image to a memory stream using DDS format.
                    var tempStream = new MemoryStream();
                    try
                    {
                        image.Save(tempStream, intermediateFormat);
                        tempStream.Position = 0;

                        // Save to a file on disk
                        var name = Enum.GetName(typeof(ImageFileType), intermediateFormat).ToLower();
                        image.Save(Path.ChangeExtension(Path.GetFileName(file), name), intermediateFormat);
                    }
                    catch (NotSupportedException)
                    {
                        Assert.True(FormatHelper.IsCompressed(image.Description.Format) && intermediateFormat != ImageFileType.Dds && intermediateFormat != ImageFileType.Tktx);
                    }

                    if (intermediateFormat == ImageFileType.Dds)
                    {
                        // Reload the image from the memory stream.
                        var image2 = Image.Load(tempStream);
                        CompareImage(image, image2, file);
                        image2.Dispose();
                    }
                }

                imageCount++;
            }
            var time = clock.ElapsedMilliseconds;
            clock.Stop();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            var testMemoryAfter = GC.GetTotalMemory(true);
            Console.WriteLine("Loaded {0} and convert to (Dds, Jpg, Png, Gif, Bmp, Tiff) image from DirectXSDK test Memory: {1} bytes, in {2}ms", imageCount, testMemoryAfter - testMemoryBefore, time);
        }

        internal static void CompareImage(Image from, Image to, string file = null)
        {
            // Check that description is identical to original image loaded from the disk.
            Assert.AreEqual(from.Description, to.Description, "Image description is different for image [{0}]", file);

            // Check that number of buffers are identical.
            Assert.AreEqual(from.PixelBuffer.Count, to.PixelBuffer.Count, "PixelBuffer size is different for image [{0}]", file);

            // Compare each pixel buffer
            for (int j = 0; j < from.PixelBuffer.Count; j++)
            {
                var srcPixelBuffer = from.PixelBuffer[j];
                var dstPixelBuffer = to.PixelBuffer[j];

                // Check only row and slice pitch
                Assert.AreEqual(srcPixelBuffer.RowStride, dstPixelBuffer.RowStride, "RowPitch is different for index [{0}], image [{1}]", j, file);
                Assert.AreEqual(srcPixelBuffer.BufferStride, dstPixelBuffer.BufferStride, "SlicePitch is different for index [{0}], image [{1}]", j, file);

                var isSameBuffer = Utilities.CompareMemory(srcPixelBuffer.DataPointer, dstPixelBuffer.DataPointer, srcPixelBuffer.BufferStride);
                if (!isSameBuffer)
                {
                    var stream = new NativeFileStream("test_from.dds", NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write);
                    stream.Write(srcPixelBuffer.DataPointer, 0, srcPixelBuffer.BufferStride);
                    stream.Close();
                    stream = new NativeFileStream("test_to.dds", NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write);
                    stream.Write(dstPixelBuffer.DataPointer, 0, dstPixelBuffer.BufferStride);
                    stream.Close();
                }

                Assert.True(isSameBuffer, "Content of PixelBuffer is different for index [{0}], image [{1}]", j, file);
            }
        }
    }
}
