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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using Windows.UI.Core;

namespace MiniCube
{
    /// <summary>
    /// Simple renderer of a colored rotating cube.
    /// </summary>
    public class CubeRenderer : SharpDX.Win8.DirectXBase
    {
        private SharpDX.Direct3D11.Buffer contantBuffer;

        private Stopwatch clock;

        /// <summary>
        /// Initializes a new instance of <see cref="CubeRenderer"/>
        /// </summary>
        public CubeRenderer()
        {
            Scale = 1.0f;
            ShowCube = true;
        }

        public bool ShowCube { get; set; }

        public float Scale { get; set; }

        /// <inheritdoc/>
        protected override void CreateDeviceResources()
        {
            base.CreateDeviceResources();

            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

            // Compile Vertex and Pixel shaders
            // Because d3dcompiler_44.dll is not in the path, use precompiled fx files
            // var vertexShaderByteCode = ShaderBytecode.CompileFromFile("MiniCube.fx", "VS", "vs_4_0", ShaderFlags.None, EffectFlags.None);
            // vertexShaderByteCode.Save("MiniCube_VS.fxo");
            ShaderBytecode vertexShaderByteCode;
            using (var stream = new NativeFileStream(path + "\\MiniCube_VS.fxo", NativeFileMode.Open, NativeFileAccess.Read))
                vertexShaderByteCode = ShaderBytecode.Load(stream);
            var vertexShader = new VertexShader(d3dDevice, vertexShaderByteCode);

            // Because d3dcompiler_44.dll is not in the path, use precompiled fx files
            // var pixelShaderByteCode = ShaderBytecode.CompileFromFile("MiniCube.fx", "PS", "ps_4_0", ShaderFlags.None, EffectFlags.None);
            // pixelShaderByteCode.Save("MiniCube_PS.fxo");
            ShaderBytecode pixelShaderByteCode;
            using (var stream = new NativeFileStream(path + "\\MiniCube_PS.fxo", NativeFileMode.Open, NativeFileAccess.Read))
                pixelShaderByteCode = ShaderBytecode.Load(stream);
            var pixelShader = new PixelShader(d3dDevice, pixelShaderByteCode);

            // Layout from VertexShader input signature
            var layout = new InputLayout(d3dDevice, vertexShaderByteCode, new[]
                    {
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0)
                    });

            // After the D3D device is created, create additional application resources.
            // CreateWindowSizeDependentResources();

            // Instantiate Vertex buffer from vertex data
            var vertices = SharpDX.Direct3D11.Buffer.Create(d3dDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), // Front
                                      new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),

                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), // BACK
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),

                                      new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), // Top
                                      new Vector4(-1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),

                                      new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), // Bottom
                                      new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),

                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), // Left
                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),

                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), // Right
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                            });

            // Create Constant Buffer
            contantBuffer = new SharpDX.Direct3D11.Buffer(d3dDevice, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

            // Setup all stages that are constant for this scene
            d3dContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, Utilities.SizeOf<Vector4>() * 2, 0));
            d3dContext.InputAssembler.InputLayout = layout;
            d3dContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            d3dContext.VertexShader.SetConstantBuffer(0, contantBuffer);
            d3dContext.VertexShader.Set(vertexShader);
            d3dContext.PixelShader.Set(pixelShader);

            clock = new Stopwatch();
            clock.Start();
        }

        public override void Render()
        {
            float width = (float)renderTargetSize.Width;
            float height = (float)renderTargetSize.Height;

            // Prepare matrices
            var view = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY);
            var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, width / (float)height, 0.1f, 100.0f);
            var viewProj = Matrix.Multiply(view, proj);

            var time = (float)(clock.ElapsedMilliseconds / 1000.0);

            // Set targets (This is mandatory in the loop)
            d3dContext.OutputMerger.SetTargets(depthStencilView, renderTargetView);

            // Clear the views
            d3dContext.ClearRenderTargetView(renderTargetView, Colors.Black);
            d3dContext.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);

            if (ShowCube)
            {
                // Calculate WorldViewProj
                var worldViewProj = Matrix.Scaling(Scale) * Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f) * viewProj;
                worldViewProj.Transpose();

                // Update Constant Buffer
                d3dContext.UpdateSubresource(ref worldViewProj, contantBuffer, 0);

                // Draw the cube
                d3dContext.Draw(36, 0);
            }
        }
    }
}
