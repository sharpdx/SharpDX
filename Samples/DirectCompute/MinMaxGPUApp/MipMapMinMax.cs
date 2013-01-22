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
using System.Drawing;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

using Buffer = SharpDX.Direct3D11.Buffer;
using Color = SharpDX.Color;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace MinMaxGPUApp
{
    public class MipMapMinMax : Component, IMinMaxProcessor
    {
        private VertexShader vertexShader;

        private PixelShader[] pixelShaderMinMaxBegin;

        private PixelShader[] pixelShaderMinMax;

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

        private int reduceFactor;

        private Texture2D textureReadback;

        /// <summary>
        /// Initializes a new instance of the <see cref="MipMapMinMax"/> class.
        /// </summary>
        public MipMapMinMax()
        {
            ReduceFactor = 1;
        }

        /// <summary>
        /// Gets or sets the reduce factor.
        /// </summary>
        /// <value>
        /// The reduce factor.
        /// </value>
        /// <remarks>
        /// This reprensents the number of pixels (2^ReduceFactor * 2^ReduceFactor) that will be used to sample the source texture.
        /// </remarks>
        public int ReduceFactor
        {
            get
            {
                return reduceFactor;
            }
            set
            {
                if (value < 1 && value < 4) throw new ArgumentException("Value must be in the range [1,3]");
                reduceFactor = value;
            }
        }

        /// <inheritdoc/>
        public void Initialize(Device device)
        {
            this.device = device;

            // Compile Vertex and Pixel shaders
            var bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MipMapMinMaxVS", "vs_4_0");
            vertexShader = ToDispose(new VertexShader(device, bytecode));
            // Layout from VertexShader input signature
            layout = ToDispose(new InputLayout(device,ShaderSignature.GetInputSignature(bytecode), new[] {
                            new InputElement("POSITION", 0, Format.R32G32_Float, 0, 0)
                        }));
            bytecode.Dispose();

            pixelShaderMinMaxBegin = new PixelShader[3];
            pixelShaderMinMax = new PixelShader[3];
            for (int i = 0; i < 3; i++)
            {
                bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MipMapMinMaxBegin" + (i + 1) + "PS", "ps_4_0");
                pixelShaderMinMaxBegin[i] = ToDispose(new PixelShader(device, bytecode));
                bytecode.Dispose();

                bytecode = ShaderBytecode.CompileFromFile("minmax.hlsl", "MipMapMinMax" + (i + 1) + "PS", "ps_4_0");
                pixelShaderMinMax[i]= ToDispose(new PixelShader(device, bytecode));
                bytecode.Dispose();
            }

            // Instantiate Vertex buiffer from vertex data
            vertices = ToDispose(Buffer.Create(device,BindFlags.VertexBuffer, new[] { -1.0f, 1.0f, 1.0f, 1.0f, -1.0f, -1.0f, 1.0f, -1.0f, }));

            sampler = ToDispose(new SamplerState(device, new SamplerStateDescription()
                        {
                            Filter = Filter.MinMagMipPoint,
                            AddressU = TextureAddressMode.Wrap,
                            AddressV = TextureAddressMode.Wrap,
                            AddressW = TextureAddressMode.Wrap,
                            BorderColor = Color.Black,
                            ComparisonFunction = Comparison.Never,
                            MaximumAnisotropy = 16,
                            MipLodBias = 0,
                            MinimumLod = 0,
                            MaximumLod = 16,
                        }));
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

            UpdateMinMaxTextures();
        }

        public void GetResults(DeviceContext context, out float min, out float max)
        {
            context.CopySubresourceRegion(texture2DMinMax, texture2DMinMaxResourceView.Length - 1, null, textureReadback, 0, 0, 0, 0);
            DataStream result;
            context.MapSubresource(textureReadback, 0, MapMode.Read, MapFlags.None, out result);
            min = result.ReadFloat();
            max = result.ReadFloat();
            context.UnmapSubresource(textureReadback, 0);
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

        private int passCount = 0;

        public void Reduce(DeviceContext context, ShaderResourceView from)
        {
            PixHelper.BeginEvent(Color.Green, "MinMax {0}x{0}", 1 << ReduceFactor);

            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, sizeof(float) * 2, 0));
            context.VertexShader.Set(vertexShader);

            var viewport = new Viewport(0, 0, Size.Width, Size.Height);

            int maxLevels = texture2DMinMaxResourceView.Length;

            int lastSampleLevel = maxLevels % ReduceFactor;

            int levels = maxLevels / ReduceFactor;

            passCount = levels + (lastSampleLevel > 0 ? 1 : 0);
            for (int i = 0; i < passCount; i++)
            {
                int shaderIndex = ReduceFactor;
                int levelIndex = (i+1) * ReduceFactor - 1;
                viewport.Width = Math.Max(((int)Size.Width) / (1 << (levelIndex + 1)), 1);
                viewport.Height = Math.Max(((int)Size.Height) / (1 << (levelIndex + 1)), 1);

                PixHelper.BeginEvent(Color.GreenYellow, "MinMax Level {0} Size: ({1},{2})", levelIndex, viewport.Width, viewport.Height);

                // Special case when last level is different from ReduceFactor size
                if (i == levels)
                {
                    levelIndex = maxLevels - 1;
                    shaderIndex = lastSampleLevel;
                    viewport.Width = 1;
                    viewport.Height = 1;
                }

                context.PixelShader.Set(i == 0 ? pixelShaderMinMaxBegin[shaderIndex - 1] : pixelShaderMinMax[shaderIndex - 1]);
                context.PixelShader.SetSampler(0, sampler);
                context.PixelShader.SetShaderResource(0, from);
                context.Rasterizer.SetViewports(viewport);
                //context.ClearRenderTargetView(texture2DMinMaxRenderView[levelIndex], Colors.Black);
                context.OutputMerger.SetTargets(texture2DMinMaxRenderView[levelIndex]);
                context.Draw(4, 0);
                context.PixelShader.SetShaderResource(0, null);
                context.OutputMerger.ResetTargets();
                from = texture2DMinMaxResourceView[levelIndex];

                PixHelper.EndEvent();
            }
            PixHelper.EndEvent();
        }

        public override string ToString()
        {
            var samplerSize = 1 << ReduceFactor;
            return string.Format("{0} {1}x{1} (PassCount {2})", GetType().Name, samplerSize, passCount);
        }
    }
}