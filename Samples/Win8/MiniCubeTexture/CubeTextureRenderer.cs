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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDX;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using Windows.UI.Core;

namespace MiniCubeTexture
{
    /// <summary>
    /// Simple renderer of a colored rotating cube.
    /// </summary>
    public class CubeTextureRenderer : Component
    {
        private SharpDX.Direct3D11.Buffer constantBuffer;
        private InputLayout layout;
        private VertexBufferBinding vertexBufferBinding;
        private Stopwatch clock;
        private VertexShader vertexShader;
        private PixelShader pixelShader;
        private ShaderResourceView textureView;
        private SamplerState sampler;

        /// <summary>
        /// Initializes a new instance of <see cref="CubeTextureRenderer"/>
        /// </summary>
        public CubeTextureRenderer()
        {
            Scale = 1.0f;
            ShowCube = true;
            EnableClear = true;
        }

        public bool EnableClear { get; set; }

        public bool ShowCube { get; set; }

        public float Scale { get; set; }

        public virtual void Initialize(DeviceManager devices)
        {
            // Remove previous buffer
            RemoveAndDispose(ref constantBuffer);

            // Setup local variables
            var d3dDevice = devices.DeviceDirect3D;
            var d3dContext = devices.ContextDirect3D;

            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

            // Loads vertex shader bytecode
            var vertexShaderByteCode = NativeFile.ReadAllBytes(path + "\\MiniCubeTexture_VS.fxo");
            vertexShader = new VertexShader(d3dDevice, vertexShaderByteCode);

            // Loads pixel shader bytecode
            pixelShader = new PixelShader(d3dDevice, NativeFile.ReadAllBytes(path + "\\MiniCubeTexture_PS.fxo"));

            // Layout from VertexShader input signature
            layout = new InputLayout(d3dDevice, vertexShaderByteCode, new[]
                    {
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
                    });

            // Instantiate Vertex buiffer from vertex data
            var vertices = SharpDX.Direct3D11.Buffer.Create(d3dDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f, // Front
                                      -1.0f,  1.0f, -1.0f, 1.0f,     0.0f, 0.0f,
                                       1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f,
                                       1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                                       1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 1.0f,

                                      -1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 0.0f, // BACK
                                       1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                                      -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                                      -1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                       1.0f, -1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                                       1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,

                                      -1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 1.0f, // Top
                                      -1.0f, 1.0f,  1.0f,  1.0f,     0.0f, 0.0f,
                                       1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 0.0f,
                                      -1.0f, 1.0f, -1.0f,  1.0f,     0.0f, 1.0f,
                                       1.0f, 1.0f,  1.0f,  1.0f,     1.0f, 0.0f,
                                       1.0f, 1.0f, -1.0f,  1.0f,     1.0f, 1.0f,

                                      -1.0f,-1.0f, -1.0f,  1.0f,     1.0f, 0.0f, // Bottom
                                       1.0f,-1.0f,  1.0f,  1.0f,     0.0f, 1.0f,
                                      -1.0f,-1.0f,  1.0f,  1.0f,     1.0f, 1.0f,
                                      -1.0f,-1.0f, -1.0f,  1.0f,     1.0f, 0.0f,
                                       1.0f,-1.0f, -1.0f,  1.0f,     0.0f, 0.0f,
                                       1.0f,-1.0f,  1.0f,  1.0f,     0.0f, 1.0f,

                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f, // Left
                                      -1.0f, -1.0f,  1.0f, 1.0f,     0.0f, 0.0f,
                                      -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                      -1.0f, -1.0f, -1.0f, 1.0f,     0.0f, 1.0f,
                                      -1.0f,  1.0f,  1.0f, 1.0f,     1.0f, 0.0f,
                                      -1.0f,  1.0f, -1.0f, 1.0f,     1.0f, 1.0f,

                                       1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 0.0f, // Right
                                       1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                                       1.0f, -1.0f,  1.0f, 1.0f,     1.0f, 1.0f,
                                       1.0f, -1.0f, -1.0f, 1.0f,     1.0f, 0.0f,
                                       1.0f,  1.0f, -1.0f, 1.0f,     0.0f, 0.0f,
                                       1.0f,  1.0f,  1.0f, 1.0f,     0.0f, 1.0f,
                            });

            vertexBufferBinding = new VertexBufferBinding(vertices, sizeof(float) * 6, 0);

            // Create Constant Buffer
            constantBuffer = ToDispose(new SharpDX.Direct3D11.Buffer(d3dDevice, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0));

            // Load texture and create sampler
            using (var bitmap = TextureLoader.LoadBitmap(devices.WICFactory, "GeneticaMortarlessBlocks.jpg"))
            using (var texture2D = TextureLoader.CreateTexture2DFromBitmap(d3dDevice, bitmap))
                textureView = new ShaderResourceView(d3dDevice, texture2D);

            sampler = new SamplerState(d3dDevice, new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                BorderColor = Color.Black,
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod =-float.MaxValue,
                MaximumLod = float.MaxValue
            });

            clock = new Stopwatch();
            clock.Start();
        }

        public virtual void Render(TargetBase render)
        {
            var d3dContext = render.DeviceManager.ContextDirect3D;

            float width = (float)render.RenderTargetSize.Width;
            float height = (float)render.RenderTargetSize.Height;

            // Prepare matrices
            var view = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY);
            var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, width / (float)height, 0.1f, 100.0f);
            var viewProj = Matrix.Multiply(view, proj);

            var time = (float)(clock.ElapsedMilliseconds / 1000.0);


            // Set targets (This is mandatory in the loop)
            d3dContext.OutputMerger.SetTargets(render.DepthStencilView, render.RenderTargetView);

            // Clear the views
            d3dContext.ClearDepthStencilView(render.DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
            if (EnableClear)
            {
                d3dContext.ClearRenderTargetView(render.RenderTargetView, Color.Black);
            }

            if (ShowCube)
            {
                // Calculate WorldViewProj
                var worldViewProj = Matrix.Scaling(Scale) * Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f) * viewProj;
                worldViewProj.Transpose();

                // Setup the pipeline
                d3dContext.InputAssembler.SetVertexBuffers(0, vertexBufferBinding);
                d3dContext.InputAssembler.InputLayout = layout;
                d3dContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                d3dContext.VertexShader.SetConstantBuffer(0, constantBuffer);
                d3dContext.VertexShader.Set(vertexShader);
                d3dContext.PixelShader.SetShaderResource(0, textureView);
                d3dContext.PixelShader.SetSampler(0, sampler);
                d3dContext.PixelShader.Set(pixelShader);

                // Update Constant Buffer
                d3dContext.UpdateSubresource(ref worldViewProj, constantBuffer, 0);

                // Draw the cube
                d3dContext.Draw(36, 0);
            }
        }
    }
}
