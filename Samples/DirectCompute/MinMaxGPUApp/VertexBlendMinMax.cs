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
using System.Drawing;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace MinMaxGPUApp
{
    public class VertexBlendMinMax : Component, IMinMaxProcessor
    {
        private VertexShader[] vertexShaders;
        private PixelShader pixelShaderMinMax;
        private GeometryShader geometryShaderMinMax;
        private Device device;
        private Texture2D texture2DMinMax;
        private ShaderResourceView texture2DMinMaxResourceView;
        private RenderTargetView texture2DMinMaxRenderView;

        private BlendState blendState;

        private DepthStencilState depthStencilState;

        public Vector2 MinMaxFactor { get { return new Vector2(-1.0f, 1.0f);} }

        private int reduceFactor;

        private Texture2D textureReadback;

        public VertexBlendMinMax()
        {
            ReduceFactor = 4;
        }

        public int ReduceFactor
        {
            get
            {
                return reduceFactor;
            }
            set
            {
                reduceFactor = value;
            }
        }

        public void Initialize(Device device)
        {
            var macros = new[]
                    {
                        new ShaderMacro("WIDTH", Size.Width),
                        new ShaderMacro("HEIGHT", Size.Height),
                        new ShaderMacro("MINMAX_BATCH_COUNT", 0),
                    };

            this.device = device;

            ShaderBytecode bytecode;
            vertexShaders = new VertexShader[10];
            for (int i = 2; i < 6; i++)
            {
                int batchCount = 1 << i;
                macros[2] = new ShaderMacro("MINMAX_BATCH_COUNT", batchCount);

                // Compile Vertex and Pixel shaders
                bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "VertexBlendMinMaxVS", "vs_4_0", ShaderFlags.None, EffectFlags.None, macros);
                vertexShaders[i] = ToDispose(new VertexShader(device, bytecode));
                bytecode.Dispose();
            }

            bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "VertexBlendMinMaxPS", "ps_4_0", ShaderFlags.None, EffectFlags.None, macros);
            pixelShaderMinMax = ToDispose(new PixelShader(device, bytecode));
            bytecode.Dispose();

            bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "VertexBlendMinMaxGS", "gs_5_0", ShaderFlags.None, EffectFlags.None, macros);
            geometryShaderMinMax = ToDispose(new GeometryShader(device, bytecode));
            bytecode.Dispose();

            var blendStateDesc = new BlendStateDescription();
            blendStateDesc.AlphaToCoverageEnable = false;
            blendStateDesc.IndependentBlendEnable = false;
            blendStateDesc.RenderTarget[0].IsBlendEnabled = true;
            blendStateDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green;

            blendStateDesc.RenderTarget[0].BlendOperation = BlendOperation.Maximum;
            blendStateDesc.RenderTarget[0].SourceBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].DestinationBlend = BlendOption.One;

            blendStateDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Maximum;
            blendStateDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
            
            blendState = ToDispose(new BlendState(device, blendStateDesc));

            var depthStateDesc = new DepthStencilStateDescription();
            depthStateDesc.BackFace.Comparison = Comparison.Less;
            depthStateDesc.BackFace.DepthFailOperation = StencilOperation.Zero;
            depthStateDesc.BackFace.FailOperation = StencilOperation.Zero;
            depthStateDesc.BackFace.PassOperation = StencilOperation.Zero;
            depthStateDesc.DepthComparison = Comparison.Less;
            depthStateDesc.DepthWriteMask = DepthWriteMask.All;
            depthStateDesc.IsDepthEnabled = false;
            depthStateDesc.IsStencilEnabled = false;
            depthStateDesc.FrontFace.Comparison = Comparison.Less;
            depthStateDesc.FrontFace.DepthFailOperation = StencilOperation.Zero;
            depthStateDesc.FrontFace.FailOperation = StencilOperation.Zero;
            depthStateDesc.FrontFace.PassOperation = StencilOperation.Zero;
            
            depthStencilState = ToDispose(new DepthStencilState(device, depthStateDesc));


            // Create permutation 2D texture 
            texture2DMinMax = ToDispose(new Texture2D(device, new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.R32G32_Float,
                Width = 1,
                Height = 1,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            }));

            // Create shader resource views and render target views
            texture2DMinMaxRenderView = ToDispose(new RenderTargetView(device, texture2DMinMax));


            // Create result 2D texture to readback by CPU
            textureReadback = ToDispose(new Texture2D(
                device,
                new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.None,
                    CpuAccessFlags = CpuAccessFlags.Read,
                    Format = Format.R32G32_Float,
                    Width = 1,
                    Height = 1,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Staging
                }));
        }

        public void GetResults(DeviceContext context, out float min, out float max)
        {
            context.CopySubresourceRegion(texture2DMinMax, 0, null, textureReadback, 0, 0, 0, 0);
            DataStream result;
            context.MapSubresource(textureReadback, 0, MapMode.Read, MapFlags.None, out result);
            min = -result.ReadFloat();
            max = result.ReadFloat();
            context.UnmapSubresource(textureReadback, 0);            
        }

        public Size Size { get; set; }

        private Size previewSize;

        private int NumberOfVertices
        {
            get
            {
                return Size.Width * Size.Height / ReduceSize ;
            }
        }

        private int ReduceSize
        {
            get
            {
                return 1 << ReduceFactor;
            }
        }

        public void Reduce(DeviceContext context, ShaderResourceView from)
        {
            PixHelper.BeginEvent("MinMax Blend {0}x1 ({1})", 1 << ReduceFactor, NumberOfVertices);
            context.InputAssembler.InputLayout = null;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.PatchListWith32ControlPoints;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(null, 0, 0));
            context.VertexShader.SetShaderResource(0, from);
            context.VertexShader.Set(vertexShaders[ReduceFactor]);
            context.PixelShader.Set(pixelShaderMinMax);
            context.GeometryShader.Set(geometryShaderMinMax);
            context.Rasterizer.SetViewports(new Viewport(0, 0, 1, 1));
            context.ClearRenderTargetView(texture2DMinMaxRenderView, new Color4(float.MinValue, float.MinValue, 0, 0));
            context.OutputMerger.SetTargets(texture2DMinMaxRenderView);
            context.OutputMerger.SetBlendState(blendState, Colors.Black, -1);
            context.OutputMerger.SetDepthStencilState(depthStencilState, 0);
            // context.Draw(Size.Width * Size.Height, 0);
            context.Draw(NumberOfVertices, 0);
            context.GeometryShader.Set(null);
            context.VertexShader.SetShaderResource(0, null);
            context.OutputMerger.ResetTargets();
            PixHelper.EndEvent();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}x{1} (VertexCount {2})", GetType().Name, ReduceSize, NumberOfVertices);
        }
    }
}