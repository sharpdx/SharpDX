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
    public class PixelShaderMinMax : Component, IMinMaxProcessor
    {
        private VertexShader vertexShader;

        private PixelShader pixelShaderMinMaxBegin;

        private PixelShader pixelShaderMinMax;

        private InputLayout layout;

        private Buffer vertices;

        private SamplerState sampler;

        private Device device;

        private Size size = Size.Empty;

        private Size previousSize = Size.Empty;

        private Texture2D texture2DMinMax;

        private ShaderResourceView[] texture2DMinMaxResourceView;

        private RenderTargetView[] texture2DMinMaxRenderView;

        public Vector2 MinMaxFactor { get { return new Vector2(1.0f, 1.0f); } }

        public void Initialize(Device device)
        {
            this.device = device;
            // Compile Vertex and Pixel shaders
            var bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MinMaxVS", "vs_4_0", ShaderFlags.Debug);
            vertexShader = ToDispose(new VertexShader(device, bytecode));
            // Layout from VertexShader input signature
            layout = ToDispose(new InputLayout(device,ShaderSignature.GetInputSignature(bytecode), new[] {
                            new InputElement("POSITION", 0, Format.R32G32_Float, 0, 0)
                        }));
            bytecode.Dispose();

            bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MinMaxBeginPS", "ps_4_0", ShaderFlags.Debug);
            pixelShaderMinMaxBegin = ToDispose(new PixelShader(device, bytecode));
            bytecode.Dispose();

            bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MinMaxPS", "ps_4_0", ShaderFlags.Debug);
            pixelShaderMinMax = ToDispose(new PixelShader(device, bytecode));
            bytecode.Dispose();

            // Instantiate Vertex buiffer from vertex data
            vertices = ToDispose(Buffer.Create(device,BindFlags.VertexBuffer, new[] { -1.0f, 1.0f, 1.0f, 1.0f, -1.0f, -1.0f, 1.0f, -1.0f, }));

            sampler = ToDispose(new SamplerState(device, new SamplerStateDescription()
                        {
                            Filter = Filter.MinMagMipPoint,
                            AddressU = TextureAddressMode.Wrap,
                            AddressV = TextureAddressMode.Wrap,
                            AddressW = TextureAddressMode.Wrap,
                            BorderColor = Colors.Black,
                            ComparisonFunction = Comparison.Never,
                            MaximumAnisotropy = 16,
                            MipLodBias = 0,
                            MinimumLod = 0,
                            MaximumLod = 16,
                        }));
        }

        public Size Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        private void UpdateMinMaxTextures()
        {
            if (size == previousSize)
                return;
            previousSize = size;

            Size nextSize = size;
            int numberOfLevels = (int)(Math.Log(nextSize.Width) / Math.Log(2));

            // Create permutation 2D texture 
            texture2DMinMax = ToDispose(new Texture2D(device, new Texture2DDescription
                        {
                            ArraySize = 1,
                            BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                            CpuAccessFlags = CpuAccessFlags.None,
                            Format = Format.R32G32_Float,
                            Width = nextSize.Width / 2,
                            Height = nextSize.Height / 2,
                            MipLevels = numberOfLevels,
                            OptionFlags = ResourceOptionFlags.None,
                            SampleDescription = new SampleDescription(1, 0),
                            Usage = ResourceUsage.Default
                        }));

            // Create shader resource views and render target views
            texture2DMinMaxResourceView = new ShaderResourceView[numberOfLevels];
            texture2DMinMaxRenderView = new RenderTargetView[numberOfLevels];

            for (int i = 0; i < numberOfLevels; i++)
            {
                texture2DMinMaxResourceView[i] = ToDispose(new ShaderResourceView(device,texture2DMinMax,new ShaderResourceViewDescription() 
                {
                                Format = Format.R32G32_Float,
                                Dimension = ShaderResourceViewDimension.Texture2D,
                                Texture2D = { MipLevels = 1, MostDetailedMip = i }
                            }));

                texture2DMinMaxRenderView[i] = ToDispose( new RenderTargetView( device, texture2DMinMax, new RenderTargetViewDescription() { Format = Format.R32G32_Float, Dimension = RenderTargetViewDimension.Texture2D, Texture2D = { MipSlice = i } }));
            }
        }

        public ShaderResourceView MinMaxView
        {
            get
            {
                return texture2DMinMaxResourceView[texture2DMinMaxResourceView.Length - 1];
            }
        }

        public void Copy(DeviceContext context, Texture2D destination)
        {
            context.CopySubresourceRegion(texture2DMinMax, texture2DMinMaxRenderView.Length - 1, null, destination, 0, 0, 0, 0);
        }

        public void Reduce(DeviceContext context, ShaderResourceView from)
        {
            UpdateMinMaxTextures();

            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, sizeof(float) * 2, 0));
            context.VertexShader.Set(vertexShader);

            var viewport = new Viewport(0, 0, Size.Width, Size.Height);
            for (int i = 0; i < texture2DMinMaxResourceView.Length; i++)
            {
                viewport.Width = Math.Max(((int)viewport.Width) / 2, 1);
                viewport.Height = Math.Max(((int)viewport.Height) / 2, 1);

                PixHelper.BeginEvent("MinMax Level {0} Size: ({1},{2})", i, viewport.Width, viewport.Height);

                context.PixelShader.Set(i == 0 ? pixelShaderMinMaxBegin : pixelShaderMinMax);
                context.PixelShader.SetSampler(0, sampler);
                context.PixelShader.SetShaderResource(0, from);
                context.Rasterizer.SetViewports(viewport);
                context.ClearRenderTargetView(texture2DMinMaxRenderView[i], Colors.Black);
                context.OutputMerger.SetTargets(texture2DMinMaxRenderView[i]);
                context.Draw(4, 0);
                context.PixelShader.SetShaderResource(0, null);
                context.OutputMerger.ResetTargets();
                from = texture2DMinMaxResourceView[i];

                PixHelper.EndEvent();
            }
        }
    }
}