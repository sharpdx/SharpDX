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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SharpDX.DXGI;

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
                throw new NotSupportedException("Install DirectX SDK June 2010 to run this test (DXSDK_DIR env variable is missing).");            
        }

        [Test]
        public void TestConstructors()
        {
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

            const int Count = 1000;
            for (int i = 0; i < Count; i++)
            {
                ProcessFiles(files, ImageFileType.Dds);
                ProcessFiles(files, ImageFileType.Jpg);
                ProcessFiles(files, ImageFileType.Png);
                //ProcessFiles(files, ImageFileType.Gif);
                //ProcessFiles(files, ImageFileType.Bmp);
                ProcessFiles(files, ImageFileType.Tiff);
            }
        }

        private void ProcessFiles(IEnumerable<string>  files, ImageFileType intermediateFormat)
        {

            Console.WriteLine("Testing {0}", intermediateFormat);
            int imageCount = 0;
            foreach (var file in files)
            {
                // Load an image from a file and dispose it.
                var image = Image.Load(file);
                //Console.WriteLine("Loading file {0} : {1}", file, image.Description);
                //Console.Out.Flush();
                //Console.WriteLine("Image [{0}] => {1}", file, image.Description);
                image.Dispose();

                // Load an image from a buffer
                var buffer = File.ReadAllBytes(file);
                image = Image.Load(buffer);

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
                    Assert.True(FormatHelper.IsCompressed(image.Description.Format) && intermediateFormat != ImageFileType.Dds);
                }

                if (intermediateFormat == ImageFileType.Dds)
                {
                    // Reload the image from the memory stream.
                    var image2 = Image.Load(tempStream);

                    // Check that description is identical to original image loaded from the disk.
                    Assert.AreEqual(image.Description, image2.Description, "Image description is different for image [{0}]", file);

                    // Check that number of buffers are identical.
                    Assert.AreEqual(image.PixelBuffers.Length, image2.PixelBuffers.Length, "PixelBuffer size is different for image [{0}]", file);

                    // Compare each pixel buffer
                    for (int j = 0; j < image.PixelBuffers.Length; j++)
                    {
                        var srcPixelBuffer = image.PixelBuffers[j];
                        var dstPixelBuffer = image2.PixelBuffers[j];

                        // Check only row and slice pitchs
                        Assert.AreEqual(srcPixelBuffer.RowPitch, dstPixelBuffer.RowPitch, "RowPitch are different for index [{0}], image [{1}]", j, file);
                        Assert.AreEqual(srcPixelBuffer.SlicePitch, dstPixelBuffer.SlicePitch, "SlicePitch are different for index [{0}], image [{1}]", j, file);

                        var isSameBuffer = Utilities.CompareMemory(srcPixelBuffer.Pixels, dstPixelBuffer.Pixels, srcPixelBuffer.SlicePitch);
                        Assert.True(isSameBuffer, "Content of PixelBuffer is different for index [{0}], image [{1}]", j, file);
                    }
                    image2.Dispose();
                }


                image.Dispose();
                imageCount++;
            }


            GC.Collect();
            GC.WaitForFullGCComplete();
            var testMemoryAfter = GC.GetTotalMemory(true);
            Console.WriteLine("Loaded {0} and convert to (Dds, Jpg, Png, Gif, Bmp, Tiff) image from DirectXSDK test Memory: {1} bytes", imageCount, testMemoryAfter - testMemoryBefore);
        }
    }
}
