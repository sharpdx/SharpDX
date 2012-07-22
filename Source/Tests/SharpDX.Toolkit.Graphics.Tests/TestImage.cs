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
using System.IO;
using NUnit.Framework;

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


        [Test]
        public void TestLoadDDS()
        {
            var testMemoryBefore = GC.GetTotalMemory(false);
            const int Count = 100;
            for (int i = 0; i < Count; i++)
            {
                int imageCount = 0;
                foreach (var file in Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.dds", SearchOption.AllDirectories))
                {
                    var image = Image.Load(file);
                    //Console.WriteLine("{0}: {1}", file, image.Description);
                    image.Dispose();

                    var buffer = File.ReadAllBytes(file);
                    image = Image.Load(buffer);
                    image.Dispose();

                    imageCount++;
                }
                GC.Collect();
                GC.WaitForFullGCComplete();
                var testMemoryAfter = GC.GetTotalMemory(true);
                Console.WriteLine("Loaded {0} x 2 DDS image from DirectXSDK test {1}/{2} Memory: {3} bytes", imageCount, i, Count, testMemoryAfter - testMemoryBefore);
            }
        }
    }
}
