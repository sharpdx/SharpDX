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
using System;
using SharpDX;
//using SharpDX.D3DCompiler;
//using SharpDX.Direct3D;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
//using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;

namespace Win8SharpDXApp
{
    /// <summary>
    ///   SharpDX port of SharpDX-MiniTri Direct3D 11 Sample
    /// </summary>
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // Enable compatibility with Direct2D
            var defaultDevice = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

            // Retrieve the Direct3D 11.1 device amd device context
            var device = defaultDevice.QueryInterface<Device1>();

            var context = defaultDevice.ImmediateContext.QueryInterface<DeviceContext1>();

            // SwapChain description
            var desc = new SwapChainDescription1()
                {
                    Width = 0,
                    // Automatic sizing
                    Height = 0,
                    BufferCount = 2,
                    // Use two buffers to enable flip effect.
                    SampleDescription = new SampleDescription(1, 0),
                    // Don't use multi-sampling.
                    SwapEffect = SwapEffect.FlipSequential,
                    Scaling = Scaling.None,
                    Usage = Usage.RenderTargetOutput
                };

            var dxgiDevice2 = device.QueryInterface<Device2>();
            var dxgiAdapter = dxgiDevice2.Adapter;
            var dxgiFactory2 = dxgiAdapter.GetParent<Factory2>();

            dxgiDevice2.MaximumFrameLatency = 1;

            // Creates the swap chain
            var swapChain = dxgiFactory2.CreateSwapChainForImmersiveWindow(device, null, ref desc, null);

            // New RenderTargetView from the backbuffer
            var backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            var renderView = new RenderTargetView(device, backBuffer);

            var backBufferDesc = backBuffer.Description;

            // Compile Vertex and Pixel shaders
            //var vertexShaderByteCode = ShaderBytecode.CompileFromFile("MiniTri.fx", "VS", "vs_4_0", ShaderFlags.None,
            //                                                          EffectFlags.None);
            //var vertexShader = new VertexShader(device, vertexShaderByteCode);

            //var pixelShaderByteCode = ShaderBytecode.CompileFromFile("MiniTri.fx", "PS", "ps_4_0", ShaderFlags.None,
            //                                                         EffectFlags.None);
            //var pixelShader = new PixelShader(device, pixelShaderByteCode);

            //// Layout from VertexShader input signature
            //var layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), new[] { 
            //    new InputElement("POSITION",0,Format.R32G32B32A32_Float,0,0),
            //    new InputElement("COLOR",0,Format.R32G32B32A32_Float,16,0)
            //});

            //// Write vertex data to a datastream
            //var stream = new DataStream(32*3, true, true);
            //stream.WriteRange(new[]
            //                      {
            //                          new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
            //                          new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
            //                          new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
            //                      });
            //stream.Position = 0;

            //// Instantiate Vertex buiffer from vertex data
            //var vertices = new Buffer(device, stream, new BufferDescription()
            //                                              {
            //                                                  BindFlags = BindFlags.VertexBuffer,
            //                                                  CpuAccessFlags = CpuAccessFlags.None,
            //                                                  OptionFlags = ResourceOptionFlags.None,
            //                                                  SizeInBytes = 32*3,
            //                                                  Usage = ResourceUsage.Default,
            //                                                  StructureByteStride = 0
            //                                              });
            //stream.Dispose();

            // Prepare All the stages
            //context.InputAssembler.InputLayout = layout;
            //context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            //context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));
            context.Rasterizer.SetViewports(new Viewport(0, 0, backBufferDesc.Width, backBufferDesc.Height, 0.0f, 1.0f));
            //context.VertexShader.Set(vertexShader);
            //context.PixelShader.Set(pixelShader);
            context.OutputMerger.SetTargets(renderView);


            while (true)
            {

                context.ClearRenderTargetView(renderView, new Color4(1.0f, 0.0f, 0.0f, 0.0f));
                //context.Draw(3, 0);
                swapChain.Present(0, PresentFlags.None);
            }
            // Release all resources
            //vertexShaderByteCode.Dispose();
            //vertexShader.Dispose();
            //pixelShaderByteCode.Dispose();
            //pixelShader.Dispose();
            //vertices.Dispose();
            //layout.Dispose();
            renderView.Dispose();
            backBuffer.Dispose();
            context.ClearState();
            context.Flush();
            device.Dispose();
            context.Dispose();
            swapChain.Dispose();
            dxgiAdapter.Dispose();
            dxgiDevice2.Dispose();
            dxgiFactory2.Dispose();
        }
    }
}