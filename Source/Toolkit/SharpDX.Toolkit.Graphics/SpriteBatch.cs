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
// -----------------------------------------------------------------------------
// The following code is a port of SpriteBatch from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Renders a group of sprites.
    /// </summary>
    public partial class SpriteBatch : GraphicsResource
    {
        private const int MaxBatchSize = 2048;
        private const int MinBatchSize = 128;
        private const int InitialQueueSize = 64;
        private const int VerticesPerSprite = 4;
        private const int IndicesPerSprite = 6;
        private const int MaxVertexCount = MaxBatchSize * VerticesPerSprite;
        private const int MaxIndexCount = MaxBatchSize * IndicesPerSprite;

        private static readonly Vector2[] CornerOffsets = { Vector2.Zero, Vector2.UnitX, Vector2.UnitY, Vector2.One };
        private static readonly short[] indices;
        private static Vector2 vector2Zero = Vector2.Zero;
        private static Rectangle? nullRectangle;
        
        private readonly BackToFrontComparer backToFrontComparer = new BackToFrontComparer();
        private readonly EffectParameter effectMatrixTransform;
        private readonly EffectParameter effectSampler;
        private readonly FrontToBackComparer frontToBackComparer = new FrontToBackComparer();
        private readonly Buffer<short> indexBuffer;
        private readonly Effect spriteEffect;
        private readonly EffectPass spriteEffectPass;
        private readonly TextureComparer textureComparer = new TextureComparer();
        private readonly ResourceContext resourceContext;
        private readonly VertexInputLayout vertexInputLayout;
        private readonly Dictionary<ShaderResourceView, TextureInfo> textureInfos = new Dictionary<ShaderResourceView, TextureInfo>(128);
        private readonly Resource tempResource = new Resource(IntPtr.Zero);
        private readonly SharpDX.Direct3D11.Texture1D tempTexture1D = new SharpDX.Direct3D11.Texture1D(IntPtr.Zero);
        private readonly SharpDX.Direct3D11.Texture2D tempTexture2D = new SharpDX.Direct3D11.Texture2D(IntPtr.Zero);
        private readonly SharpDX.Direct3D11.Texture3D tempTexture3D = new SharpDX.Direct3D11.Texture3D(IntPtr.Zero);
        private BlendState blendState;

        private Effect customEffect;
        private EffectParameter customEffectMatrixTransform;
        private EffectParameter customEffectSampler;
        private EffectParameter customEffectTexture;
        private DepthStencilState depthStencilState;

        private bool isBeginCalled;
        private RasterizerState rasterizerState;
        private SamplerState samplerState;
        private int[] sortIndices;
        private SpriteInfo[] sortedSprites;
        private SpriteInfo[] spriteQueue;
        private int spriteQueueCount;
        private SpriteSortMode spriteSortMode;
        private TextureInfo[] spriteTextures;
        private DataBuffer x64TempBuffer;

        private Matrix transformMatrix;

        static SpriteBatch()
        {
            indices = new short[MaxIndexCount];
            int k = 0;
            for (int i = 0; i < MaxIndexCount; k += VerticesPerSprite)
            {
                indices[i++] = (short)(k + 0);
                indices[i++] = (short)(k + 1);
                indices[i++] = (short)(k + 2);
                indices[i++] = (short)(k + 1);
                indices[i++] = (short)(k + 3);
                indices[i++] = (short)(k + 2);
            }            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatch" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="batchCapacity">The batch capacity default to 2048 (minimum 2048).</param>
        public SpriteBatch(GraphicsDevice graphicsDevice, int batchCapacity = MaxBatchSize) : base(graphicsDevice)
        {
            if (batchCapacity < MaxBatchSize) batchCapacity = MaxBatchSize;
            spriteQueue = new SpriteInfo[batchCapacity];
            spriteTextures = new TextureInfo[batchCapacity];

            spriteEffect = ToDispose(new Effect(graphicsDevice, effectBytecode, graphicsDevice.DefaultEffectPool));
            spriteEffect.CurrentTechnique = spriteEffect.Techniques[0];
            spriteEffectPass = spriteEffect.CurrentTechnique.Passes[0];

            effectMatrixTransform = spriteEffect.Parameters["MatrixTransform"];
            effectSampler = spriteEffect.Parameters["TextureSampler"];

            // Creates the vertex buffer (shared by within a device context).
            resourceContext = GraphicsDevice.GetOrCreateSharedData(SharedDataType.PerContext, "SpriteBatch.VertexBuffer", () => new ResourceContext(GraphicsDevice));

            // Creates the vertex input layout (we don't need to cache them as they are already cached).
            vertexInputLayout = VertexInputLayout.New(VertexBufferLayout.New<VertexPositionColorTexture>(0));

            // Creates the index buffer (shared within a Direct3D11 Device)
            indexBuffer =  GraphicsDevice.GetOrCreateSharedData(SharedDataType.PerDevice, "SpriteBatch.IndexBuffer", () => Buffer.Index.New(GraphicsDevice, indices));
        }

        /// <summary>
        /// Begins a sprite batch operation using deferred sort and default state objects (BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise).
        /// </summary>
        public void Begin(SpriteSortMode spritemode = SpriteSortMode.Deferred, Effect effect = null)
        {
            Begin(spritemode, null, null, null, null, effect, Matrix.Identity);
        }

        /// <summary>
        /// Begins a sprite batch rendering using the specified sorting mode and blend state, sampler, depth stencil and rasterizer state objects, plus a custom effect. Passing null for any of the state objects selects the default default state objects (BlendState.AlphaBlend, DepthStencilState.None, RasterizerState.CullCounterClockwise, SamplerState.LinearClamp). Passing a null effect selects the default SpriteBatch Class shader.
        /// </summary>
        /// <param name="sortMode">Sprite drawing order.</param>
        /// <param name="blendState">Blending options.</param>
        /// <param name="samplerState">Texture sampling options.</param>
        /// <param name="depthStencilState">Depth and stencil options.</param>
        /// <param name="rasterizerState">Rasterization options.</param>
        /// <param name="effect">Effect state options.</param>
        public void Begin(SpriteSortMode sortMode, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null)
        {
            Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);
        }

        /// <summary>
        /// Begins a sprite batch rendering using the specified sorting mode and blend state, sampler, depth stencil, rasterizer state objects, plus a custom effect and a 2D transformation matrix. Passing null for any of the state objects selects the default default state objects (BlendState.AlphaBlend, DepthStencilState.None, RasterizerState.CullCounterClockwise, SamplerState.LinearClamp). Passing a null effect selects the default SpriteBatch Class shader. 
        /// </summary>
        /// <param name="sortMode">Sprite drawing order.</param>
        /// <param name="blendState">Blending options.</param>
        /// <param name="samplerState">Texture sampling options.</param>
        /// <param name="depthStencilState">Depth and stencil options.</param>
        /// <param name="rasterizerState">Rasterization options.</param>
        /// <param name="effect">Effect state options.</param>
        /// <param name="transformMatrix">Transformation matrix for scale, rotate, translate options.</param>
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            if (isBeginCalled)
            {
                throw new InvalidOperationException("End must be called before begin");
            }

            this.spriteSortMode = sortMode;
            this.blendState = blendState;
            this.samplerState = samplerState;
            this.depthStencilState = depthStencilState;
            this.rasterizerState = rasterizerState;
            this.customEffect = effect;
            this.transformMatrix = transformMatrix;

            // If custom effect is not null, get all its potential default parameters
            if (customEffect != null)
            {
                customEffectMatrixTransform = customEffect.Parameters["MatrixTransform"];
                customEffectTexture = customEffect.Parameters["Texture"];
                customEffectSampler = customEffect.Parameters["TextureSampler"];
            }

            // Immediate mode, then prepare for rendering here instead of End()
            if (sortMode == SpriteSortMode.Immediate)
            {
                if (resourceContext.IsInImmediateMode)
                {
                    throw new InvalidOperationException("Only one SpriteBatch at a time can use SpriteSortMode.Immediate");
                }

                PrepareForRendering();

                resourceContext.IsInImmediateMode = true;
            }

            // Sets to true isBeginCalled
            isBeginCalled = true;
        }

        /// <summary>
        /// Adds a sprite to a batch of sprites for rendering using the specified texture, destination rectangle, and color. 
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">A rectangle that specifies (in screen coordinates) the destination for drawing the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <remarks>
        /// Before making any calls to Draw, you must call Begin. Once all calls to Draw are complete, call End. 
        /// </remarks>
        public void Draw(ShaderResourceView texture, Rectangle destinationRectangle, Color color)
        {
            var destination = new RectangleF(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height);
            DrawSprite(texture, ref destination, false, ref nullRectangle, color, 0f, ref vector2Zero, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Adds a sprite to a batch of sprites for rendering using the specified texture, position and color. 
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public void Draw(ShaderResourceView texture, Vector2 position, Color color)
        {
            var destination = new RectangleF(position.X, position.Y, 1f, 1f);
            DrawSprite(texture, ref destination, true, ref nullRectangle, color, 0f, ref vector2Zero, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Adds a sprite to a batch of sprites for rendering using the specified texture, destination rectangle, source rectangle, color, rotation, origin, effects and layer. 
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">A rectangle that specifies (in screen coordinates) the destination for drawing the sprite. If this rectangle is not the same size as the source rectangle, the sprite will be scaled to fit.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public void Draw(ShaderResourceView texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            var destination = new RectangleF(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height);
            DrawSprite(texture, ref destination, false, ref sourceRectangle, color, rotation, ref origin, effects, layerDepth);
        }

        /// <summary>
        /// Adds a sprite to a batch of sprites for rendering using the specified texture, position, source rectangle, and color. 
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public void Draw(ShaderResourceView texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            var destination = new RectangleF(position.X, position.Y, 1f, 1f);
            DrawSprite(texture, ref destination, true, ref sourceRectangle, color, 0f, ref vector2Zero, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Adds a sprite to a batch of sprites for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer. 
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public void Draw(ShaderResourceView texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            var destination = new RectangleF(position.X, position.Y, scale, scale);
            DrawSprite(texture, ref destination, true, ref sourceRectangle, color, rotation, ref origin, effects, layerDepth);
        }

        /// <summary>
        /// Adds a sprite to a batch of sprites for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer. 
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public void Draw(ShaderResourceView texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            var destination = new RectangleF(position.X, position.Y, scale.X, scale.Y);
            DrawSprite(texture, ref destination, true, ref sourceRectangle, color, rotation, ref origin, effects, layerDepth);
        }

        /// <summary>Adds a string to a batch of sprites for rendering using the specified font, text, position, and color.</summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var proxy = new SpriteFont.StringProxy(text);

            var drawCommand = new SpriteFont.InternalDrawCommand(this, position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            spriteFont.InternalDraw(ref proxy, ref drawCommand);
        }

        /// <summary>Adds a string to a batch of sprites for rendering using the specified font, text, position, and color.</summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var proxy = new SpriteFont.StringProxy(text);
            var drawCommand = new SpriteFont.InternalDrawCommand(this, position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            spriteFont.InternalDraw(ref proxy, ref drawCommand);
        }

        /// <summary>Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.</summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var proxy = new SpriteFont.StringProxy(text);
            var drawCommand = new SpriteFont.InternalDrawCommand(this, position, color, rotation, origin, scale, effects, layerDepth);
            spriteFont.InternalDraw(ref proxy, ref drawCommand);
        }

        /// <summary>Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.</summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var proxy = new SpriteFont.StringProxy(text);
            var drawCommand = new SpriteFont.InternalDrawCommand(this, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
            spriteFont.InternalDraw(ref proxy, ref drawCommand);
        }

        /// <summary>Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.</summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var proxy = new SpriteFont.StringProxy(text);
            var drawCommand = new SpriteFont.InternalDrawCommand(this, position, color, rotation, origin, scale, effects, layerDepth);
            spriteFont.InternalDraw(ref proxy, ref drawCommand);
        }

        /// <summary>Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.</summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var proxy = new SpriteFont.StringProxy(text);
            var drawCommand = new SpriteFont.InternalDrawCommand(this, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
            spriteFont.InternalDraw(ref proxy, ref drawCommand);
        }

        /// <summary>
        /// Flushes the sprite batch and restores the device state to how it was before Begin was called. 
        /// </summary>
        public void End()
        {
            if (!isBeginCalled)
            {
                throw new InvalidOperationException("Begin must be called before End");
            }

            if (spriteSortMode == SpriteSortMode.Immediate)
            {
                resourceContext.IsInImmediateMode = false;
            }
            else if (spriteQueueCount > 0)
            {
                  // Draw the queued sprites now.
                if (resourceContext.IsInImmediateMode)
                {
                    throw new InvalidOperationException("Cannot end one SpriteBatch while another is using SpriteSortMode.Immediate");
                }

                // If not immediate, then setup and render all sprites
                PrepareForRendering();
                FlushBatch();
            }

            // Clear the custom effect so that it won't be used next Begin/End
            if (customEffect != null)
            {
                customEffectMatrixTransform = null;
                customEffectTexture = null;
                customEffectSampler = null;
                customEffect = null;
            }

            // Clear stored texture infos
            textureInfos.Clear();

            // We are with begin pair
            isBeginCalled = false;
        }

        private void FlushBatch()
        {
            SpriteInfo[] spriteQueueForBatch;

            // If Deferred, then sprites are displayed in the same order they arrived
            if (spriteSortMode == SpriteSortMode.Deferred)
            {
                spriteQueueForBatch = spriteQueue;
            }
            else
            {
                // Else Sort all sprites according to their sprite order mode.
                SortSprites();
                spriteQueueForBatch = sortedSprites;
            }

            // Iterate on all sprites and group batch per texture.
            int offset = 0;
            var previousTexture = default(TextureInfo);
            for (int i = 0; i < spriteQueueCount; i++)
            {
                TextureInfo texture;

                if (spriteSortMode == SpriteSortMode.Deferred)
                {
                    texture = spriteTextures[i];
                }
                else
                {
                    // Copy ordered sprites to the queue to batch
                    int index = sortIndices[i];
                    spriteQueueForBatch[i] = spriteQueue[index];

                    // Get the texture indirectly
                    texture = spriteTextures[index];
                }

                if (texture.ShaderResourceView != previousTexture.ShaderResourceView)
                {
                    if (i > offset)
                    {
                        DrawBatchPerTexture(ref previousTexture, spriteQueueForBatch, offset, i - offset);
                    }

                    offset = i;
                    previousTexture = texture;
                }
            }

            // Draw the last batch
            DrawBatchPerTexture(ref previousTexture, spriteQueueForBatch, offset, spriteQueueCount - offset);

            // Reset the queue.
            Array.Clear(spriteTextures, 0, spriteQueueCount);
            spriteQueueCount = 0;

            // When sorting is disabled, we persist mSortedSprites data from one batch to the next, to avoid
            // unnecessary work in GrowSortedSprites. But we never reuse these when sorting, because re-sorting
            // previously sorted items gives unstable ordering if some sprites have identical sort keys.
            if (spriteSortMode != SpriteSortMode.Deferred)
            {
                Array.Clear(sortedSprites, 0, sortedSprites.Length);
            }
        }

        private void SortSprites()
        {
            IComparer<int> comparer;

            switch (spriteSortMode)
            {
                case SpriteSortMode.Texture:
                    textureComparer.SpriteTextures = spriteTextures;
                    comparer = textureComparer;
                    break;

                case SpriteSortMode.BackToFront:
                    backToFrontComparer.SpriteQueue = spriteQueue;
                    comparer = backToFrontComparer;
                    break;

                case SpriteSortMode.FrontToBack:
                    frontToBackComparer.SpriteQueue = spriteQueue;
                    comparer = frontToBackComparer;
                    break;
                default:
                    throw new NotSupportedException();
            }

            if ((sortIndices == null) || (sortIndices.Length < spriteQueueCount))
            {
                sortIndices = new int[spriteQueueCount];
                sortedSprites = new SpriteInfo[spriteQueueCount];
            }

            // Reset all indices to the original order
            for (int i = 0; i < spriteQueueCount; i++)
            {
                sortIndices[i] = i;
            }

            Array.Sort(sortIndices, 0, spriteQueueCount, comparer);
        }

        internal unsafe void DrawSprite(ShaderResourceView texture, ref RectangleF destination, bool scaleDestination, ref Rectangle? sourceRectangle, Color color, float rotation, ref Vector2 origin, SpriteEffects effects, float depth)
        {
            // Check that texture is not null
            if (texture == null || texture.NativePointer == IntPtr.Zero)
            {
                throw new ArgumentNullException("texture");
            }

            // Make sure that Begin was called
            if (!isBeginCalled)
            {
                throw new InvalidOperationException("Begin must be called before draw");
            }

            // Resize the buffer of SpriteInfo
            if (spriteQueueCount >= spriteQueue.Length)
            {
                Array.Resize(ref spriteQueue, spriteQueue.Length*2);
            }

            // Gets the resource information from the view (width, height).
            // Cache the result in order to avoid this request if the texture is reused 
            // inside a same Begin/End block.
            TextureInfo textureInfo;
            if (!textureInfos.TryGetValue(texture, out textureInfo))
            {
                // otherwise go to the slow path
                textureInfo.ShaderResourceView = texture.NativePointer;

                // If this is a shader resource view from the toolkit
                // go the fast path
                var tkTexture = texture.Tag as Texture;
                if (tkTexture != null)
                {
                    textureInfo.Width = tkTexture.Width;
                    textureInfo.Height = tkTexture.Height;
                }
                else
                {
                    IntPtr resourcePtr;
                    texture.GetResource(out resourcePtr);
                    tempResource._nativePointer = (void*)resourcePtr;
                    switch (tempResource.Dimension)
                    {
                        case ResourceDimension.Texture1D:
                            tempTexture1D._nativePointer = (void*)resourcePtr;
                            textureInfo.Width = tempTexture1D.Description.Width;
                            textureInfo.Height = 1;
                            break;
                        case ResourceDimension.Texture2D:
                            tempTexture2D._nativePointer = (void*)resourcePtr;
                            var description2D = tempTexture2D.Description;
                            textureInfo.Width = description2D.Width;
                            textureInfo.Height = description2D.Height;
                            break;
                        case ResourceDimension.Texture3D:
                            tempTexture3D._nativePointer = (void*)resourcePtr;
                            var description3D = tempTexture3D.Description;
                            textureInfo.Width = description3D.Width;
                            textureInfo.Height = description3D.Height;
                            break;
                        default:
                            throw new ArgumentException("Invalid resource for texture. Must be Texture1D/2D/3D", "texture");
                    }
                }

                // Then calculate the actual width of the view
                // Use the first MostDetailedMip to calculate the actual size of this view
                var mipIndex = texture.Description.Texture1D.MostDetailedMip;
                textureInfo.Width = Math.Max(1, textureInfo.Width >> mipIndex);
                textureInfo.Height = Math.Max(1, textureInfo.Height >> mipIndex);

                textureInfos.Add(texture, textureInfo);
            }

            // Put values in next SpriteInfo
            fixed (SpriteInfo* spriteInfo = &(spriteQueue[spriteQueueCount]))
            {
                float width;
                float height;

                // If the source rectangle has a value, then use it.
                if (sourceRectangle.HasValue)
                {
                    Rectangle rectangle = sourceRectangle.Value;
                    spriteInfo->Source.X = rectangle.X;
                    spriteInfo->Source.Y = rectangle.Y;
                    width = rectangle.Width;
                    height = rectangle.Height;
                }
                else
                {
                    // Else, use directly the size of the texture
                    spriteInfo->Source.X = 0.0f;
                    spriteInfo->Source.Y = 0.0f;
                    width = textureInfo.Width;
                    height = textureInfo.Height;
                }

                // Sets the width and height
                spriteInfo->Source.Width = width;
                spriteInfo->Source.Height = height;

                // Scale the destination box
                if (scaleDestination)
                {
                    destination.Width *= width;
                    destination.Height *= height;
                }

                // Sets the destination
                spriteInfo->Destination = destination;

                // Copy all other values.
                spriteInfo->Origin.X = origin.X;
                spriteInfo->Origin.Y = origin.Y;
                spriteInfo->Rotation = rotation;
                spriteInfo->Depth = depth;
                spriteInfo->SpriteEffects = effects;
                spriteInfo->Color = color;
            }

            // If we are in immediate mode, render the sprite directly
            if (spriteSortMode == SpriteSortMode.Immediate)
            {
                DrawBatchPerTexture(ref textureInfo, spriteQueue, 0, 1);
            }
            else
            {
                if (spriteTextures.Length < spriteQueue.Length)
                {
                    Array.Resize(ref spriteTextures, spriteQueue.Length);
                }
                spriteTextures[spriteQueueCount] = textureInfo;
                spriteQueueCount++;
            }
        }

        private void DrawBatchPerTexture(ref TextureInfo texture, SpriteInfo[] sprites, int offset, int count)
        {
            var nativeShaderResourceViewPointer = texture.ShaderResourceView;

            if (customEffect != null)
            {
                var currentTechnique = customEffect.CurrentTechnique;

                int passCount = currentTechnique.Passes.Count;
                for (int i = 0; i < passCount; i++)
                {
                    // Sets the texture on the custom effect if the parameter exist
                    if (customEffectTexture != null)
                    {
                        customEffectTexture.SetResourcePointer(nativeShaderResourceViewPointer);
                    }

                    var currentPass = currentTechnique.Passes[i];

                    // Apply the current pass
                    currentPass.Apply();

                    // Draw the batch of sprites
                    DrawBatchPerTextureAndPass(ref texture, sprites, offset, count);

                    // unbind all pass resources as the texture can be used later as a render target
                    currentPass.UnApply();
                }
            }
            else
            {
                unsafe
                {
                    // Sets the texture for this sprite effect.
                    // Use an optimized version in order to avoid to reapply the sprite effect here just to change texture
                    // We are calling directly the PixelShaderStage. We assume that the texture is on slot 0 as it is
                    // setup in the original BasicEffect.fx shader.
                    GraphicsDevice.PixelShaderStage.SetShaderResources(0, 1, new IntPtr(&nativeShaderResourceViewPointer));
                }

                DrawBatchPerTextureAndPass(ref texture, sprites, offset, count);

                // unbind the texture from pass as it can be used later as a render target
                GraphicsDevice.PixelShaderStage.SetShaderResources(0, 1, GraphicsDevice.ResetSlotsPointers);
            }
        }

        private unsafe void DrawBatchPerTextureAndPass(ref TextureInfo texture, SpriteInfo[] sprites, int offset, int count)
        {
            float deltaX = 1f/(texture.Width);
            float deltaY = 1f/(texture.Height);
            while (count > 0)
            {
                // How many sprites do we want to draw?
                int batchSize = count;

                // How many sprites does the D3D vertex buffer have room for?
                int remainingSpace = MaxBatchSize - resourceContext.VertexBufferPosition;
                if (batchSize > remainingSpace)
                {
                    if (remainingSpace < MinBatchSize)
                    {
                        resourceContext.VertexBufferPosition = 0;
                        batchSize = (count < MaxBatchSize) ? count : MaxBatchSize;
                    }
                    else
                    {
                        batchSize = remainingSpace;
                    }
                }

                // Sets the data directly to the buffer in memory
                int offsetInBytes = resourceContext.VertexBufferPosition * VerticesPerSprite * Utilities.SizeOf<VertexPositionColorTexture>();

                var noOverwrite = resourceContext.VertexBufferPosition == 0 ? SetDataOptions.Discard : SetDataOptions.NoOverwrite;

                // ------------------------------------------------------------------------------------------------------------
                // CAUTION: Performance problem under x64 resolved by this special codepath:
                // For some unknown reasons, It seems that writing directly to the pointer returned by the MapSubresource is 
                // extremely inefficient using x64 but using a temporary buffer and performing a mempcy to the locked region
                // seems to be running at the same speed than x86
                // ------------------------------------------------------------------------------------------------------------
                if (IntPtr.Size == 8)
                {
                    if (x64TempBuffer == null)
                    {
                        x64TempBuffer = ToDispose(new DataBuffer(Utilities.SizeOf<VertexPositionColorTexture>() * MaxBatchSize * VerticesPerSprite));
                    }

                    // Perform the update of all vertices on a temporary buffer
                    var texturePtr = (VertexPositionColorTexture*)x64TempBuffer.DataPointer;
                    for (int i = 0; i < batchSize; i++)
                    {
                        UpdateVertexFromSpriteInfo(ref sprites[offset + i], ref texturePtr, deltaX, deltaY);
                    }

                    // Then copy this buffer in one shot
                    resourceContext.VertexBuffer.SetData(GraphicsDevice, new DataPointer(x64TempBuffer.DataPointer, batchSize * VerticesPerSprite * Utilities.SizeOf<VertexPositionColorTexture>()), offsetInBytes, noOverwrite);
                }
                else
                {
                    // For x86 version, It seems that we can write directly to the buffer
                    // TODO: Need to check that this behavior is also running fine under WP8
                    var deviceContext = (DeviceContext)GraphicsDevice;
                    try
                    {
                        var box = deviceContext.MapSubresource(resourceContext.VertexBuffer, 0, SetDataOptionsHelper.ConvertToMapMode(noOverwrite), MapFlags.None);
                        var pointer = (IntPtr)((byte*)box.DataPointer + offsetInBytes);
                        var texturePtr = (VertexPositionColorTexture*)pointer;
                        for(var i = 0; i < batchSize; i++)
                            UpdateVertexFromSpriteInfo(ref sprites[offset + i], ref texturePtr, deltaX, deltaY);
                    }
                    finally
                    {
                        deviceContext.UnmapSubresource(resourceContext.VertexBuffer, 0);
                    }
                }

                // Draw from the specified index
                int startIndex = resourceContext.VertexBufferPosition * IndicesPerSprite;
                int indexCount = batchSize * IndicesPerSprite;
                GraphicsDevice.DrawIndexed(PrimitiveType.TriangleList, indexCount, startIndex);

                // Update position, offset and remaining count
                resourceContext.VertexBufferPosition += batchSize;
                offset += batchSize;
                count -= batchSize;
            }
        }

        private unsafe void UpdateVertexFromSpriteInfo(ref SpriteInfo spriteInfo, ref VertexPositionColorTexture* vertex, float deltaX, float deltaY)
        {
            var rotation = spriteInfo.Rotation != 0f ? new Vector2((float) Math.Cos(spriteInfo.Rotation), (float) Math.Sin(spriteInfo.Rotation)) : Vector2.UnitX;

            // Origin scale down to the size of the source texture 
            var origin = spriteInfo.Origin;
            origin.X /= spriteInfo.Source.Width == 0f ? float.Epsilon : spriteInfo.Source.Width;
            origin.Y /= spriteInfo.Source.Height == 0f ? float.Epsilon : spriteInfo.Source.Height;

            for (int j = 0; j < 4; j++)
            {
                // Gets the corner and take into account the Flip mode.
                var corner = CornerOffsets[j];
                // Calculate position on destination
                var position = new Vector2((corner.X - origin.X) * spriteInfo.Destination.Width, (corner.Y - origin.Y) * spriteInfo.Destination.Height);

                // Apply rotation and destination offset
                vertex->Position.X = spriteInfo.Destination.X + (position.X * rotation.X) - (position.Y * rotation.Y);
                vertex->Position.Y = spriteInfo.Destination.Y + (position.X * rotation.Y) + (position.Y * rotation.X);
                vertex->Position.Z = spriteInfo.Depth;
                vertex->Color = spriteInfo.Color;

                corner = CornerOffsets[j ^ (int)spriteInfo.SpriteEffects];
                vertex->TextureCoordinate.X = (spriteInfo.Source.X + corner.X * spriteInfo.Source.Width) * deltaX;
                vertex->TextureCoordinate.Y = (spriteInfo.Source.Y + corner.Y * spriteInfo.Source.Height) * deltaY;
                vertex++;
            }
        }

        private void PrepareForRendering()
        {
            // Setup states (Blend, DepthStencil, Rasterizer)
            GraphicsDevice.SetBlendState(blendState ?? GraphicsDevice.BlendStates.AlphaBlend);
            GraphicsDevice.SetDepthStencilState(depthStencilState ?? GraphicsDevice.DepthStencilStates.None);
            GraphicsDevice.SetRasterizerState(rasterizerState ?? GraphicsDevice.RasterizerStates.CullBack);

            // Clear all shaders
            GraphicsDevice.ResetShaderStages();

            // Build ortho-projection matrix
            ViewportF viewport = GraphicsDevice.Viewport;
            float xRatio = (viewport.Width > 0) ? (1f/(viewport.Width)) : 0f;
            float yRatio = (viewport.Height > 0) ? (-1f/(viewport.Height)) : 0f;
            var matrix = new Matrix { M11 = xRatio * 2f, M22 = yRatio * 2f, M33 = 1f, M44 = 1f, M41 = -1f, M42 = 1f };

            Matrix finalMatrix;
            Matrix.Multiply(ref transformMatrix, ref matrix, out finalMatrix);

            // Use LinearClamp for sampler state
            var localSamplerState = samplerState ?? GraphicsDevice.SamplerStates.LinearClamp;

            // Setup effect states and parameters: SamplerState and MatrixTransform
            // Sets the sampler state
            if (customEffect != null)
            {
                if (customEffect.CurrentTechnique == null)
                    throw new InvalidOperationException("CurrentTechnique is not set on custom effect");

                if (customEffectSampler != null)
                    customEffectSampler.SetResource(localSamplerState);

                if (customEffectMatrixTransform != null)
                    customEffectMatrixTransform.SetValue(finalMatrix);
            }
            else
            {
                effectSampler.SetResource(localSamplerState);
                effectMatrixTransform.SetValue(finalMatrix);

                // Apply the sprite effect globally
                spriteEffectPass.Apply();
            }

            // Set VertexInputLayout
            GraphicsDevice.SetVertexInputLayout(vertexInputLayout);

            // VertexBuffer
            GraphicsDevice.SetVertexBuffer(resourceContext.VertexBuffer);

            // Index buffer
            GraphicsDevice.SetIndexBuffer(indexBuffer, false);

            // If this is a deferred D3D context, reset position so the first Map call will use D3D11_MAP_WRITE_DISCARD.
            if (GraphicsDevice.IsDeferred)
            {
                resourceContext.VertexBufferPosition = 0;
            }
        }

        #region Nested type: BackToFrontComparer

        private class BackToFrontComparer : IComparer<int>
        {
            public SpriteInfo[] SpriteQueue;

            #region IComparer<int> Members

            public int Compare(int left, int right)
            {
                return SpriteQueue[right].Depth.CompareTo(SpriteQueue[left].Depth);
            }

            #endregion
        }

        #endregion

        #region Nested type: FrontToBackComparer

        private class FrontToBackComparer : IComparer<int>
        {
            public SpriteInfo[] SpriteQueue;

            #region IComparer<int> Members

            public int Compare(int left, int right)
            {
                return SpriteQueue[left].Depth.CompareTo(SpriteQueue[right].Depth);
            }

            #endregion
        }

        #endregion

        #region Nested type: TextureComparer

        private class TextureComparer : IComparer<int>
        {
            public TextureInfo[] SpriteTextures;

            #region IComparer<int> Members

            public int Compare(int left, int right)
            {
                return SpriteTextures[left].ShaderResourceView.ToInt64().CompareTo(SpriteTextures[right].ShaderResourceView.ToInt64());
            }

            #endregion
        }

        #endregion

        #region Nested type: SpriteInfo

        [StructLayout(LayoutKind.Sequential)]
        private struct SpriteInfo
        {
            public RectangleF Source;
            public RectangleF Destination;
            public Vector2 Origin;
            public float Rotation;
            public float Depth;
            public SpriteEffects SpriteEffects;
            public Color Color;
        }

        #endregion


        /// <summary>
        /// Use a ResourceContext per GraphicsDevice (DeviceContext)
        /// </summary>
        private class ResourceContext : Component
        {
            public readonly Buffer<VertexPositionColorTexture> VertexBuffer;

            public int VertexBufferPosition;

            public bool IsInImmediateMode;

            public ResourceContext(GraphicsDevice device)
            {
                VertexBuffer = ToDispose(Buffer.Vertex.New<VertexPositionColorTexture>(device, MaxVertexCount, ResourceUsage.Dynamic));
            }
        }

        /// <summary>
        /// Internal structure used to store texture information.
        /// </summary>
        private struct TextureInfo
        {
            public IntPtr ShaderResourceView;

            public int Width;

            public int Height;
        }
    }
}