﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Primitive quad use to draw an effect on a quad (fullscreen by default). This is directly accessible from the <see cref="GraphicsDevice.DrawQuad"/> method.
    /// </summary>
    public partial class PrimitiveQuad : Component
    {
        private readonly Effect quadEffect;
        private readonly EffectPass quadPass;
        private readonly EffectPass textureCopyPass;
        private readonly EffectParameter matrixParameter;
        private readonly EffectParameter textureParameter;
        private readonly EffectParameter colorParameter;
        private readonly EffectParameter textureSamplerParameter;
        private readonly SharedData sharedData;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveQuad" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public PrimitiveQuad(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            quadEffect = ToDispose(new Effect(GraphicsDevice, effectBytecode));
            quadPass = quadEffect.CurrentTechnique.Passes[0];
            matrixParameter = quadEffect.Parameters["MatrixTransform"];

            textureCopyPass = quadEffect.CurrentTechnique.Passes[1];
            textureParameter = quadEffect.Parameters["Texture"];
            colorParameter = quadEffect.Parameters["Color"];
            textureSamplerParameter = quadEffect.Parameters["TextureSampler"];

            // Default LinearClamp
            textureSamplerParameter.SetResource(GraphicsDevice.SamplerStates.LinearClamp);

            Transform = Matrix.Identity;
            Color = new Color4(1.0f);

            sharedData = GraphicsDevice.GetOrCreateSharedData(SharedDataType.PerDevice, "Toolkit::PrimitiveQuad::VertexBuffer", () => new SharedData(GraphicsDevice));
        }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Gets or sets the color. Default is <see cref="SharpDX.Color.White"/>.
        /// </summary>
        /// <value>The color.</value>
        public Color4 Color
        {
            get
            {
                return colorParameter.GetValue<Color4>();
            }

            set
            {
                colorParameter.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the transform. Default is <see cref="Matrix.Identity"/>.
        /// </summary>
        /// <value>The transform.</value>
        public Matrix Transform
        {
            get
            {
                return matrixParameter.GetMatrix();
            }

            set
            {
                matrixParameter.SetValue(value);
            }
        }

        /// <summary>
        /// Draws a raw quad requiring a Vertex and Pixel shader to be setup before calling this method.
        /// </summary>
        /// <param name="fullScreenTriangle">if set to <c>true</c> to draw an optimized full screen triangle as a full screen quad.</param>
        public void DrawRaw(bool fullScreenTriangle)
        {
            GraphicsDevice.SetVertexBuffer(fullScreenTriangle ? sharedData.VertexBufferFullQuad : sharedData.VertexBuffer);
            GraphicsDevice.SetVertexInputLayout(sharedData.VertexInputLayout);

            // Make sure that we are using our vertex shader
            GraphicsDevice.Draw(PrimitiveType.TriangleStrip, fullScreenTriangle ? 3 : 4);

            // Reset the vertex buffer
            GraphicsDevice.SetVertexBuffer(0, null, 0);
            GraphicsDevice.InputAssemblerStage.SetInputLayout(null);
        }

        /// <summary>
        /// Draws a quad. The effect must have been applied before calling this method with pixel shader having the signature float2:TEXCOORD.
        /// </summary>
        public void Draw()
        {
            Draw(false);
        }

        /// <summary>
        /// Draws a quad. The effect must have been applied before calling this method with pixel shader having the signature float2:TEXCOORD.
        /// </summary>
        /// <param name="fullScreenTriangle">if set to <c>true</c> to draw an optimized full screen triangle as a full screen quad.</param>
        public void Draw(bool fullScreenTriangle)
        {
            ResetShaderStages();
            // Make sure that we are using our vertex shader
            quadPass.Apply();
            DrawRaw(fullScreenTriangle);
        }

        /// <summary>
        /// Draws a quad with a texture. This Draw method is using a simple pixel shader that is sampling the texture.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="samplerState">State of the sampler. If null, default sampler is <see cref="SamplerStateCollection.LinearClamp" />.</param>
        /// <param name="fullScreenTriangle">if set to <c>true</c> to draw an optimized full screen triangle as a full screen quad.</param>
        public void Draw(SharpDX.Direct3D11.ShaderResourceView texture, SharpDX.Direct3D11.SamplerState samplerState = null, bool fullScreenTriangle = false)
        {
            GraphicsDevice.SetVertexBuffer(fullScreenTriangle ? sharedData.VertexBufferFullQuad : sharedData.VertexBuffer);
            GraphicsDevice.SetVertexInputLayout(sharedData.VertexInputLayout);

            ResetShaderStages();

            // Make sure that we are using our vertex shader
            textureParameter.SetResource(texture);
            textureSamplerParameter.SetResource(samplerState ?? GraphicsDevice.SamplerStates.LinearClamp);
            textureCopyPass.Apply();
            GraphicsDevice.Draw(PrimitiveType.TriangleStrip, fullScreenTriangle ? 3 : 4);

            // Reset the vertex buffer
            GraphicsDevice.SetVertexBuffer(0, null, 0);
            GraphicsDevice.InputAssemblerStage.SetInputLayout(null);
            GraphicsDevice.Context.PixelShader.SetShaderResource(0, null);
        }

        /// <summary>
        /// Draws the specified effect onto the quad. The effect must have a pixel shader with the signature float2:TEXCOORD.
        /// </summary>
        /// <param name="effect">The effect.</param>
        /// <param name="fullScreenTriangle">if set to <c>true</c> to draw an optimized full screen triangle as a full screen quad.</param>
        public void Draw(Effect effect, bool fullScreenTriangle = false)
        {
            GraphicsDevice.SetVertexBuffer(fullScreenTriangle ? sharedData.VertexBufferFullQuad : sharedData.VertexBuffer);
            GraphicsDevice.SetVertexInputLayout(sharedData.VertexInputLayout);
            ResetShaderStages();

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                // Apply the Effect pass
                pass.Apply();

                // Make sure that we are using our vertex shader
                quadPass.Apply();

                GraphicsDevice.Draw(PrimitiveType.TriangleStrip, fullScreenTriangle ? 3 : 4);

                // Reset the quadPass and custom pass
                quadPass.UnApply();
                pass.UnApply();
            }

            // Reset the vertex buffer
            GraphicsDevice.SetVertexBuffer(0, null, 0);
            GraphicsDevice.InputAssemblerStage.SetInputLayout(null);
        }

        /// <summary>
        /// Draws the specified effect pass onto the quad. The effect pass must have a pixel shader with the signature float2:TEXCOORD.
        /// </summary>
        /// <param name="effectPass">The effect pass.</param>
        /// <param name="fullScreenTriangle">if set to <c>true</c> to draw an optimized full screen triangle as a full screen quad.</param>
        public void Draw(EffectPass effectPass, bool fullScreenTriangle = false)
        {
            ResetShaderStages();

            // Apply the Effect pass
            effectPass.Apply();
            Draw(fullScreenTriangle);

            // Unapply this effect
            effectPass.UnApply();
        }

        private void ResetShaderStages()
        {
            // Make sure that domain, hull and geometry shaders are disable.
            GraphicsDevice.DomainShaderStage.Set(null);
            GraphicsDevice.HullShaderStage.Set(null);
            GraphicsDevice.GeometryShaderStage.Set(null);
        }

        /// <summary>
        /// Internal structure used to store VertexBuffer and VertexInputLayout.
        /// </summary>
        private class SharedData : Component
        {
            /// <summary>
            /// The vertex buffer
            /// </summary>
            public readonly Buffer<VertexPositionTexture> VertexBuffer;

            /// <summary>
            /// The vertex buffer
            /// </summary>
            public readonly Buffer<VertexPositionTexture> VertexBufferFullQuad;

            /// <summary>
            /// The vertex input layout
            /// </summary>
            public readonly VertexInputLayout VertexInputLayout;

            private static readonly VertexPositionTexture[] QuadsVertices = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1)),
            };

            private static readonly VertexPositionTexture[] FullQuadsVertices = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3( 3, 1, 0), new Vector2(2, 0)),
                new VertexPositionTexture(new Vector3(-1,-3, 0), new Vector2(0, 2)),
            };

            public SharedData(GraphicsDevice device)
            {
                VertexBuffer = ToDispose(Buffer.Vertex.New(device, QuadsVertices));
                VertexBufferFullQuad = ToDispose(Buffer.Vertex.New(device, FullQuadsVertices));
                VertexInputLayout = VertexInputLayout.FromBuffer(0, VertexBuffer);
            }
        }
    }
}