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

namespace MinMaxGPUApp
{
    public class BlendMinMax : Component, IMinMaxProcessor
    {
        private VertexShader vertexShader;
        private PixelShader pixelShaderMinMax;
        private Device device;
        private Texture2D texture2DMinMax;
        private ShaderResourceView texture2DMinMaxResourceView;
        private RenderTargetView texture2DMinMaxRenderView;

        private BlendState blendState;

        private DepthStencilState depthStencilState;

        public Vector2 MinMaxFactor { get { return new Vector2(-1.0f, 1.0f);} }

        public void Initialize(Device device)
        {
            this.device = device;
            // Compile Vertex and Pixel shaders
            var bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MinMaxBlendVS", "vs_4_0", ShaderFlags.Debug);
            vertexShader = ToDispose(new VertexShader(device, bytecode));
            bytecode.Dispose();

            bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MinMaxBlendPS", "ps_4_0", ShaderFlags.Debug);
            pixelShaderMinMax = ToDispose(new PixelShader(device, bytecode));
            bytecode.Dispose();

            var blendStateDesc = new BlendStateDescription();
            blendStateDesc.AlphaToCoverageEnable = false;
            blendStateDesc.IndependentBlendEnable = false;
            blendStateDesc.RenderTarget[0].BlendOperation = BlendOperation.Maximum;
            blendStateDesc.RenderTarget[0].DestinationBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].IsBlendEnabled = true;
            blendStateDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green;
            blendStateDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].SourceBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].DestinationBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Maximum;
            
            blendState = ToDispose(new BlendState(device, blendStateDesc));

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


            // Create shader resource views and render target views
            texture2DMinMaxRenderView = ToDispose(new RenderTargetView(device, texture2DMinMax));
        }

        public Size Size { get; set; }

        public void Copy(DeviceContext context, Texture2D destination)
        {
            context.CopySubresourceRegion(texture2DMinMax, 0, null, destination, 0, 0, 0, 0);
        }

        public void Reduce(DeviceContext context, ShaderResourceView from)
        {
            PixHelper.BeginEvent("MinMax Blend");
            context.InputAssembler.InputLayout = null;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(null, 0, 0));
            context.VertexShader.SetShaderResource(0, from);
            context.VertexShader.Set(vertexShader);
            context.PixelShader.Set(pixelShaderMinMax);
            context.Rasterizer.SetViewports(new Viewport(0, 0, 1, 1));
            context.ClearRenderTargetView(texture2DMinMaxRenderView, new Color4(float.MinValue, float.MinValue, 0.0f, 1.0f));
            context.OutputMerger.SetTargets(texture2DMinMaxRenderView);
            context.OutputMerger.SetBlendState(blendState, Colors.Black, -1);
            context.OutputMerger.SetDepthStencilState(depthStencilState, 0);
            // context.Draw(Size.Width * Size.Height, 0);
            context.Draw(Size.Width * Size.Height / 128, 0);
            context.VertexShader.SetShaderResource(0, null);
            context.OutputMerger.ResetTargets();
            PixHelper.EndEvent();
        }
    }
}