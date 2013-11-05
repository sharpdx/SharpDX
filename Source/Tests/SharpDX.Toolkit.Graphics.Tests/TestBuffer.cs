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
using NUnit.Framework;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics.Tests
{
    /// <summary>
    /// Tests for <see cref="SharpDX.Toolkit.Graphics.Buffer"/>
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.Toolkit.Graphics.Buffer")]
    public class TestBuffer
    {
        private GraphicsDevice device;

        public TestBuffer()
        {
            device = GraphicsDevice.New(flags:DeviceCreationFlags.Debug);
        }

        [Test]
        public void ConstantBuffer()
        {
            // -----------------------------------------------------------------------------------
            // Check constant buffer creation
            // -----------------------------------------------------------------------------------
            var constantBuffer = Buffer.New(device, 256, BufferFlags.ConstantBuffer);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(constantBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 256,
                StructureByteStride = 0,
                Usage = ResourceUsage.Default
            });
            
            Assert.AreEqual((ShaderResourceView)constantBuffer, null);
            Assert.AreEqual((UnorderedAccessView)constantBuffer, null);

            constantBuffer.Dispose();
        }

        [Test]
        public void VertexBuffer()
        {
            // -----------------------------------------------------------------------------------
            // Check vertex buffer creation
            // -----------------------------------------------------------------------------------
            var vertexBuffer = Buffer.New(device, 256, BufferFlags.VertexBuffer);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(vertexBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 256,
                StructureByteStride = 0,
                Usage = ResourceUsage.Default
            });

            Assert.AreEqual((ShaderResourceView)vertexBuffer, null);
            Assert.AreEqual((UnorderedAccessView)vertexBuffer, null);

            vertexBuffer.Dispose();
        }


        [Test]
        public void IndexBuffer()
        {
            // -----------------------------------------------------------------------------------
            // Check index buffer creation
            // -----------------------------------------------------------------------------------
            var indexBuffer = Buffer.New(device, 256, BufferFlags.IndexBuffer);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(indexBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 256,
                StructureByteStride = 0,
                Usage = ResourceUsage.Default
            });

            Assert.AreEqual((ShaderResourceView)indexBuffer, null);
            Assert.AreEqual((UnorderedAccessView)indexBuffer, null);

            indexBuffer.Dispose();

            // -----------------------------------------------------------------------------------
            // Check index buffer creation with shader resource view
            // -----------------------------------------------------------------------------------
            indexBuffer = Buffer.New(device, 256, sizeof(int), BufferFlags.IndexBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(indexBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer | BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 256,
                StructureByteStride = 0,
                Usage = ResourceUsage.Default
            });

            // Check SRV description
            Assert.AreNotEqual((ShaderResourceView)indexBuffer, null);

            var srvDescription = ((ShaderResourceView)indexBuffer).Description;
            srvDescription.Texture1DArray.ArraySize = 0; // Clear this value
            var srvDescriptionReference = new ShaderResourceViewDescription()
            {
                Format = Format.R32_UInt,
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                BufferEx =
                {
                    ElementCount = 256/4,
                    FirstElement = 0,
                    Flags = ShaderResourceViewExtendedBufferFlags.None
                }
            };
            Assert.AreEqual(srvDescription, srvDescriptionReference);

            // Check UAV description
            Assert.AreNotEqual((UnorderedAccessView)indexBuffer, null);
            Assert.AreEqual(((UnorderedAccessView)indexBuffer).Description, new UnorderedAccessViewDescription()
            {
                Format = Format.R32_UInt,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer = { ElementCount = 256 / 4, FirstElement = 0, Flags = UnorderedAccessViewBufferFlags.None }
            });


            indexBuffer.Dispose();
        }


        [Test]
        public void StructuredBuffer()
        {
            // -----------------------------------------------------------------------------------
            // Check structured buffer creation with ShaderResourceView
            // -----------------------------------------------------------------------------------
            var structuredBuffer = Buffer.New(device, 512, 16, BufferFlags.StructuredBuffer | BufferFlags.ShaderResource);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(structuredBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferStructured,
                SizeInBytes = 512,
                StructureByteStride = 16,
                Usage = ResourceUsage.Default
            });

            // Check views
            Assert.AreEqual((UnorderedAccessView)structuredBuffer, null);

            // Check SRV description
            Assert.AreNotEqual((ShaderResourceView)structuredBuffer, null);

            var srvDescription = ((ShaderResourceView)structuredBuffer).Description;
            srvDescription.Texture1DArray.ArraySize = 0; // Clear this value
            var srvDescriptionReference = new ShaderResourceViewDescription()
                                              {
                                                  Format = Format.Unknown,
                                                  Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                                                  BufferEx =
                                                      {
                                                          ElementCount = 32,
                                                          FirstElement = 0,
                                                          Flags = ShaderResourceViewExtendedBufferFlags.None
                                                      }
                                              };
            Assert.AreEqual(srvDescription, srvDescriptionReference);
            
            structuredBuffer.Dispose();

            // -----------------------------------------------------------------------------------
            // Check structured buffer creation with UnorderedAccessView
            // -----------------------------------------------------------------------------------
            structuredBuffer = Buffer.New(device, 512, 16, BufferFlags.StructuredBuffer | BufferFlags.UnorderedAccess);
            // -----------------------------------------------------------------------------------
            
            // Check views
            Assert.AreEqual((ShaderResourceView)structuredBuffer, null);
            Assert.AreNotEqual((UnorderedAccessView)structuredBuffer, null);

            // Check UAV description
            Assert.AreEqual(((UnorderedAccessView)structuredBuffer).Description, new UnorderedAccessViewDescription()
            {
                Format = Format.Unknown,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer = { ElementCount = 512 / 16, FirstElement = 0, Flags = UnorderedAccessViewBufferFlags.None }
            });
            
            structuredBuffer.Dispose();

            // -----------------------------------------------------------------------------------
            // Check Append structured buffer creation with UnorderedAccessView
            // -----------------------------------------------------------------------------------
            structuredBuffer = Buffer.New(device, 512, 16, BufferFlags.StructuredAppendBuffer);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(structuredBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferStructured,
                SizeInBytes = 512,
                StructureByteStride = 16,
                Usage = ResourceUsage.Default
            });

            // Check views
            Assert.AreEqual((ShaderResourceView)structuredBuffer, null);
            Assert.AreNotEqual((UnorderedAccessView)structuredBuffer, null);

            // Check UAV description
            Assert.AreEqual(((UnorderedAccessView)structuredBuffer).Description, new UnorderedAccessViewDescription()
            {
                Format = Format.Unknown,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer = { ElementCount = 512 / 16, FirstElement = 0, Flags = UnorderedAccessViewBufferFlags.Append }
            });

            structuredBuffer.Dispose();
        }

        [Test]
        public void RawBuffer()
        {
            // -----------------------------------------------------------------------------------
            // Check raw buffer creation with UnorderedAccess and ShaderResourceView
            // -----------------------------------------------------------------------------------
            var rawBuffer = Buffer.New(device, 512, BufferFlags.RawBuffer | BufferFlags.UnorderedAccess | BufferFlags.ShaderResource);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(rawBuffer.Description, new BufferDescription()
                                                              {
                                                                  BindFlags = BindFlags.UnorderedAccess | BindFlags.ShaderResource,
                                                                  CpuAccessFlags = CpuAccessFlags.None,
                                                                  OptionFlags = ResourceOptionFlags.BufferAllowRawViews,
                                                                  SizeInBytes = 512,
                                                                  StructureByteStride = 0,
                                                                  Usage = ResourceUsage.Default
                                                              });
            // Check SRV description
            Assert.AreNotEqual((ShaderResourceView)rawBuffer, null);

            var srvDescription = ((ShaderResourceView)rawBuffer).Description;
            srvDescription.Texture1DArray.ArraySize = 0; // Clear this value
            var srvDescriptionReference = new ShaderResourceViewDescription()
            {
                Format = Format.R32_Typeless,
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                BufferEx =
                {
                    ElementCount = 512/4,
                    FirstElement = 0,
                    Flags = ShaderResourceViewExtendedBufferFlags.Raw
                }
            };
            Assert.AreEqual(srvDescription, srvDescriptionReference);

            // Check UAV
            Assert.AreNotEqual((UnorderedAccessView)rawBuffer, null);

            // Check UAV description
            Assert.AreEqual(((UnorderedAccessView)rawBuffer).Description, new UnorderedAccessViewDescription()
            {
                Format = Format.R32_Typeless,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer = { ElementCount = 512/4, FirstElement = 0, Flags = UnorderedAccessViewBufferFlags.Raw }
            });
            rawBuffer.Dispose();

            // -----------------------------------------------------------------------------------
            // Check raw buffer creation with IndexBuffer, VertexBuffer, RawBuffer, RenderTarget, UnorderedAccess and ShaderResourceView
            // -----------------------------------------------------------------------------------
            rawBuffer = Buffer.New(device, 512, sizeof(int), BufferFlags.IndexBuffer | BufferFlags.VertexBuffer | BufferFlags.RawBuffer | BufferFlags.RenderTarget | BufferFlags.UnorderedAccess | BufferFlags.ShaderResource);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(rawBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer | BindFlags.VertexBuffer | BindFlags.RenderTarget | BindFlags.UnorderedAccess | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.BufferAllowRawViews,
                SizeInBytes = 512,
                StructureByteStride = 0,
                Usage = ResourceUsage.Default
            });

            // Check SRV description
            Assert.AreNotEqual((ShaderResourceView)rawBuffer, null);

            srvDescription = ((ShaderResourceView)rawBuffer).Description;
            srvDescription.Texture1DArray.ArraySize = 0; // Clear this value
            srvDescriptionReference = new ShaderResourceViewDescription()
            {
                Format = Format.R32_Typeless,
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                BufferEx =
                {
                    ElementCount = 512 / 4,
                    FirstElement = 0,
                    Flags = ShaderResourceViewExtendedBufferFlags.Raw
                }
            };
            Assert.AreEqual(srvDescription, srvDescriptionReference);

            // Check UAV
            Assert.AreNotEqual((UnorderedAccessView)rawBuffer, null);

            // Check UAV description
            Assert.AreEqual(((UnorderedAccessView)rawBuffer).Description, new UnorderedAccessViewDescription()
            {
                Format = Format.R32_Typeless,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer = { ElementCount = 512 / 4, FirstElement = 0, Flags = UnorderedAccessViewBufferFlags.Raw }
            });

            // Check RTV
            var rtv = rawBuffer.GetRenderTargetView(PixelFormat.R32.UInt, 16);
            Assert.AreNotEqual(rtv, null);

            var rtvDescription = rtv.Description;
            rtvDescription.Texture2DArray.ArraySize = 0;
            // Check UAV description
            Assert.AreEqual(rtvDescription, new RenderTargetViewDescription()
            {
                Format = Format.R32_UInt,
                Dimension = RenderTargetViewDimension.Buffer,
                Buffer = { ElementWidth = 16 * sizeof(int), FirstElement = 0}
            });
        }

        [Test]
        public void ArgumentBuffer()
        {
            // -----------------------------------------------------------------------------------
            // Check vertex buffer creation
            // -----------------------------------------------------------------------------------
            var argumentBuffer = Buffer.New(device, 256, BufferFlags.ArgumentBuffer);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(argumentBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.DrawIndirectArguments,
                SizeInBytes = 256,
                StructureByteStride = 0,
                Usage = ResourceUsage.Default
            });

            Assert.AreEqual((ShaderResourceView)argumentBuffer, null);
            Assert.AreEqual((UnorderedAccessView)argumentBuffer, null);

            argumentBuffer.Dispose();


            // -----------------------------------------------------------------------------------
            argumentBuffer = Buffer.New(device, 256, BufferFlags.ArgumentBuffer | BufferFlags.RawBuffer | BufferFlags.UnorderedAccess);
            // -----------------------------------------------------------------------------------

            Assert.AreEqual(argumentBuffer.Description, new BufferDescription()
            {
                BindFlags = BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.DrawIndirectArguments | ResourceOptionFlags.BufferAllowRawViews,
                SizeInBytes = 256,
                StructureByteStride = 0,
                Usage = ResourceUsage.Default
            });

            Assert.AreEqual((ShaderResourceView)argumentBuffer, null);
            Assert.AreNotEqual((UnorderedAccessView)argumentBuffer, null);

            argumentBuffer.Dispose();
        }


        public void AllTest()
        {
            ConstantBuffer();

            IndexBuffer();

            VertexBuffer();

            StructuredBuffer();

            RawBuffer();

            ArgumentBuffer();
        }
    }
}
