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

        private GraphicsDevice GraphicsDevice;
        
        [TestFixtureSetUp]
        public void Initialize()
        {
            GraphicsDevice = GraphicsDevice.New(DeviceCreationFlags.Debug);

            dxsdkDir = Environment.GetEnvironmentVariable("DXSDK_DIR");

            if (string.IsNullOrEmpty(dxsdkDir))
                throw new NotSupportedException("Install DirectX SDK June 2010 to run this test (DXSDK_DIR env variable is missing).");
        }

        [Test]
        public void Test1D()
        {
            var textureData = new byte[256];
            for(int i = 0; i < textureData.Length; i++)
                textureData[i] = (byte)i;

            // -------------------------------------------------------
            // General test for a Texture1D
            // -------------------------------------------------------
            
            // Create Texture1D
            var texture = Texture1D.New(GraphicsDevice, textureData.Length, PixelFormat.R8.UNorm);

            // Check description against native description
            var d3d11Texture = (Direct3D11.Texture1D)texture;
            var d3d11SRV = (Direct3D11.ShaderResourceView)texture;

            Assert.AreEqual(d3d11Texture.Description, new Direct3D11.Texture1DDescription() {
                Width = textureData.Length,
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = DXGI.Format.R8_UNorm,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default
            });

            // Check shader resource view.
            var srvDescription = d3d11SRV.Description;
            // Clear those fields that are garbage returned from ShaderResourceView.Description.
            srvDescription.Texture2DArray.ArraySize = 0;
            srvDescription.Texture2DArray.FirstArraySlice = 0;

            Assert.AreEqual(srvDescription, new Direct3D11.ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture1D,
                Format = DXGI.Format.R8_UNorm,
                Texture1D = { MipLevels = 1, MostDetailedMip = 0 },
            });

            // Check mipmap description
            var mipmapDescription = texture.GetMipMapDescription(0);
            var rowStride = textureData.Length * sizeof(byte);
            var refMipmapDescription = new MipMapDescription(textureData.Length, 1, 1, rowStride, rowStride, textureData.Length, 1);
            Assert.AreEqual(mipmapDescription, refMipmapDescription);

            // Check that getting the default SRV is the same as getting the first mip/array
            Assert.AreEqual(texture.ShaderResourceView[ViewType.Full, 0, 0], d3d11SRV);

            // Check GraphicsDevice.GetData/SetData data
            // Upload the textureData to the GPU
            texture.SetData(GraphicsDevice, textureData);

            // Readback data from the GPU
            var readBackData = texture.GetData<byte>();

            // Check that both content are equal
            Assert.True(Utilities.Compare(textureData, readBackData));

            // -------------------------------------------------------
            // Check with Texture1D.Clone and GraphicsDevice.Copy
            // -------------------------------------------------------
            using (var texture2 = texture.Clone<Texture1D>())
            {
                GraphicsDevice.Copy(texture, texture2);

                readBackData = texture2.GetData<byte>();

                // Check that both content are equal
                Assert.True(Utilities.Compare(textureData, readBackData));
            }

            // -------------------------------------------------------
            // Test SetData using a ResourceRegion
            // -------------------------------------------------------
            // Set the last 4 pixels in different orders
            var smallTextureDataRegion = new byte[] { 4, 3, 2, 1 };

            var region = new ResourceRegion(textureData.Length - 4, 0, 0, textureData.Length, 1, 1);
            texture.SetData(GraphicsDevice, smallTextureDataRegion, 0, 0, region);

            readBackData = texture.GetData<byte>();

            Array.Copy(smallTextureDataRegion, 0, textureData, textureData.Length - 4, 4);

            // Check that both content are equal
            Assert.True(Utilities.Compare(textureData, readBackData));

            // -------------------------------------------------------
            // Texture.Dispose()
            // -------------------------------------------------------
            // TODO check that Dispose is implemented correctly
            texture.Dispose();
        }

        [Test]
        public void Test1DArrayAndMipmaps()
        {
            // -----------------------------------------------------------------------------------------
            // Check for a Texture1D as an array of 6 texture with (8+1) mipmaps each, with UAV support
            // -----------------------------------------------------------------------------------------
            var texture = Texture1D.New(GraphicsDevice, 256, true, PixelFormat.R8.UNorm, true, 6);
            var mipcount = (int)Math.Log(256, 2) + 1;

            // Check description against native description
            var d3d11Texture = (Direct3D11.Texture1D)texture;
            var d3d11SRV = (Direct3D11.ShaderResourceView)texture;

            Assert.AreEqual(d3d11Texture.Description, new Direct3D11.Texture1DDescription()
            {
                Width = 256,
                ArraySize = 6,
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = DXGI.Format.R8_UNorm,
                MipLevels = mipcount,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default
            });

            // ---------------------------------------
            // Check default FULL shader resource view.
            // ---------------------------------------
            var srvDescription = d3d11SRV.Description;
            Assert.AreEqual(srvDescription, new Direct3D11.ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture1DArray,
                Format = DXGI.Format.R8_UNorm,
                Texture1DArray = { MipLevels = mipcount, MostDetailedMip = 0, ArraySize = 6, FirstArraySlice = 0},
            
            });

            // Simplified view of mip (mip up to 9) and array (array up to 6)
            //        Array0 Array1 Array2
            //       ______________________
            //  Mip0 |      |      |      |
            //       |------+------+------|
            //  Mip1 |   X  |  X   |  X   |
            //       |------+------+------|
            //  Mip2 |      |      |      |
            //       ----------------------
            srvDescription = texture.ShaderResourceView[ViewType.MipBand, 0, 1].Description;
            Assert.AreEqual(srvDescription, new Direct3D11.ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture1DArray,
                Format = DXGI.Format.R8_UNorm,
                Texture1DArray = { MipLevels = 1, MostDetailedMip = 1, ArraySize = 6, FirstArraySlice = 0 },
            });

            // Simplified view of mip (mip up to 9) and array (array up to 6)
            //        Array0 Array1 Array2
            //       ______________________
            //  Mip0 |      |  X   |      |
            //       |------+------+------|
            //  Mip1 |      |  X   |      |
            //       |------+------+------|
            //  Mip2 |      |  X   |      |
            //       ----------------------
            srvDescription = texture.ShaderResourceView[ViewType.ArrayBand, 1, 0].Description;
            Assert.AreEqual(srvDescription, new Direct3D11.ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture1DArray,
                Format = DXGI.Format.R8_UNorm,
                Texture1DArray = { MipLevels = mipcount, MostDetailedMip = 0, ArraySize = 1, FirstArraySlice = 1 },
            });

            // Simplified view of mip (mip up to 9) and array (array up to 6)
            //        Array0 Array1 Array2
            //       ______________________
            //  Mip0 |      |      |      |
            //       |------+------+------|
            //  Mip1 |      |  X   |      |
            //       |------+------+------|
            //  Mip2 |      |      |      |
            //       ----------------------
            srvDescription = texture.ShaderResourceView[ViewType.Single, 1, 1].Description;
            Assert.AreEqual(srvDescription, new Direct3D11.ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture1DArray,
                Format = DXGI.Format.R8_UNorm,
                Texture1DArray = { MipLevels = 1, MostDetailedMip = 1, ArraySize = 1, FirstArraySlice = 1 },
            });

            // -------------------------------------------------------
            // Test SetData on last mipmap on 2nd array
            // -------------------------------------------------------
            var textureData = new byte[] { 255 };

            texture.SetData(GraphicsDevice, textureData, 1, 8);

            var readbackData = texture.GetData<byte>(1, 8);

            Assert.AreEqual(textureData.Length, readbackData.Length);
            Assert.AreEqual(textureData[0], readbackData[0]);

            // -------------------------------------------------------
            // Clear the content of the texture using UAV
            // -------------------------------------------------------
            var uav = texture.UnorderedAccessView[1, 8];
            var uavDescription = uav.Description;
            Assert.AreEqual(uavDescription, new Direct3D11.UnorderedAccessViewDescription()
            {
                Dimension = UnorderedAccessViewDimension.Texture1DArray,
                Format = DXGI.Format.R8_UNorm,
                Texture1DArray = { ArraySize = 1, FirstArraySlice = 1, MipSlice = 8}
            });

            // Set the value == 1
            GraphicsDevice.Clear(uav, new Int4(1,0,0,0));
            readbackData = texture.GetData<byte>(1, 8);
            Assert.AreEqual(readbackData.Length, 1);
            Assert.AreEqual(readbackData[0], 1);


            texture.Dispose();
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
            var files = new List<string>();
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.dds", SearchOption.AllDirectories));
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.EnumerateFiles(Path.Combine(dxsdkDir, @"Samples\Media"), "*.bmp", SearchOption.AllDirectories));

            var excludeList = new List<string>()
                                  {
                                      "RifleStock1Bump.dds"  // This file is in BC1 format but size is not a multiple of 4, so It can't be loaded as a texture, so we skip it.
                                  };

            for (int i = 0; i < 1; i++)
            {
                foreach (var file in files)
                {
                    if (excludeList.Contains(Path.GetFileName(file), StringComparer.InvariantCultureIgnoreCase))
                        continue;

                    // Load an image from a file and dispose it.
                    var texture = Texture.Load(GraphicsDevice, file);

                    var localPath = Path.GetFileName(file);
                    texture.Save(localPath, ImageFileType.Dds);
                    texture.Dispose();

                    var originalImage = Image.Load(file);
                    var textureImage = Image.Load(localPath);

                    TestImage.CompareImage(originalImage, textureImage, file);

                    originalImage.Dispose();
                    textureImage.Dispose();
                }
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                ////if ((i % 10) == 0 )
                //{
                //    Console.WriteLine("------------------------------------------------------");
                //    Console.WriteLine("Lived Ojbects");
                //    Console.WriteLine("------------------------------------------------------");
                //    deviceDebug.ReportLiveDeviceObjects(ReportingLevel.Detail);
                //}
            }
        }
    }
}
