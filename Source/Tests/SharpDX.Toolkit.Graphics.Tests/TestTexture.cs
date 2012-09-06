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
using System.Text;
using NUnit.Framework;
using SharpDX.D3DCompiler;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics.Tests
{
    /// <summary>
    /// Tests for all Textures
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.Toolkit.Graphics")]
    public unsafe class TestTexture
    {
        private string dxsdkDir;

        [TestFixtureSetUp]
        public void Initialize()
        {
            dxsdkDir = Environment.GetEnvironmentVariable("DXSDK_DIR");

            if (string.IsNullOrEmpty(dxsdkDir))
                throw new NotSupportedException("Install DirectX SDK June 2010 to run this test (DXSDK_DIR env variable is missing).");
        }

        /// <summary>
        /// Tests the load save.
        /// </summary>
        /// <remarks>
        /// This test loads several images using <see cref="Texture.Load"/> (on the GPU) and save them to the disk using <see cref="Texture.Save"/>.
        /// The saved image is then compared with the original image to check that the whole chain (CPU -> GPU, GPU -> CPU) is passing correctly
        /// the textures.
        /// </remarks>
        [Test]
        public void TestLoadSave()
        {
            var device = GraphicsDevice.New(DriverType.Reference, DeviceCreationFlags.Debug);
            var files = new List<string>();
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.dds", SearchOption.AllDirectories));
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.bmp", SearchOption.AllDirectories));

            var excludeList = new List<string>()
                                  {
                                      "RifleStock1Bump.dds"  // This file is in BC1 format but size is not a multiple of 4, so It can't be loaded as a texture, so we skip it.
                                  };

            foreach (var file in files)
            {
                if (excludeList.Contains(Path.GetFileName(file), StringComparer.InvariantCultureIgnoreCase))
                    continue;

                Console.WriteLine("Process file {0}", file);

                // Load an image from a file and dispose it.
                var texture = Texture.Load(file);

                var localPath = Path.GetFileName(file);
                texture.Save(localPath, ImageFileType.Dds);
                texture.Dispose();

                var originalImage = Image.Load(file);
                var textureImage = Image.Load(localPath);

                TestImage.CompareImage(originalImage, textureImage, file);

                originalImage.Dispose();
                textureImage.Dispose();
            }
        }
    }
}
