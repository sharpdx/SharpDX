// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Diagnostics;

using SharpDX;
using SharpDX.Toolkit;

namespace SpriteBatchAndFont
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Simple SpriteBatchAndFont application using SharpDX.Toolkit.
    /// The purpose of this application is to use SpriteBatch and SpriteFont.
    /// </summary>
    public class SpriteBatchAndFontGame : Game
    {
        private readonly Stopwatch fpsClock;
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;
        private Texture2D ballsTexture;
        private int frameCount;
        private string fpsText;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchAndFontGame" /> class.
        /// </summary>
        public SpriteBatchAndFontGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this); 

            // Force no vsync and use real timestep to print actual FPS
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Variable used for FPS
            fpsClock = new Stopwatch();
            fpsText = string.Empty;
        }

        protected override void LoadContent()
        {
            // Loads the balls texture (32 textures (32x32) stored vertically => 32 x 1024 ).
            ballsTexture = Content.Load<Texture2D>("balls.dds");

            // SpriteFont supports the following font file format:
            // - DirectX Toolkit MakeSpriteFont or SharpDX Toolkit tkfont
            // - BMFont from Angelcode http://www.angelcode.com/products/bmfont/
            arial16BMFont = Content.Load<SpriteFont>("Arial16.tkfnt");

            // Instantiate a SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void BeginRun()
        {
            // Starts the FPS clock
            fpsClock.Start();
        }

        protected override void Initialize()
        {
            Window.Title = "SpriteBatch and Font demo";
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(GraphicsDevice.BackBuffer, Color.CornflowerBlue);

            // Precalculate some constants
            int textureHalfSize = ballsTexture.Width / 2;
            int spriteSceneWidth = GraphicsDevice.BackBuffer.Width / 2;
            int spriteSceneHeight = GraphicsDevice.BackBuffer.Height / 2;
            int spriteSceneRadiusWidth = GraphicsDevice.BackBuffer.Width / 2 - textureHalfSize;
            int spriteSceneRadiusHeight = GraphicsDevice.BackBuffer.Height / 2 - textureHalfSize;

            // Time used to animate the balls
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Draw sprites on the screen
            const int SpriteCount = 5000;
            var random = new Random(0);
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);  // Use NonPremultiplied, as this sprite texture is not premultiplied
            for (int i = 0; i < SpriteCount; i++)
            {
                var angleOffset = (float)random.NextDouble() * Math.PI * 2.0f;
                var radiusOffset = (float)random.NextDouble() * 0.8f + 0.2f;
                var spriteSpeed = (float)random.NextDouble() + 0.1f;

                var position = new Vector2
                {
                    X = spriteSceneWidth + spriteSceneRadiusWidth * radiusOffset * (float)Math.Cos(time * spriteSpeed + angleOffset),
                    Y = spriteSceneHeight + spriteSceneRadiusHeight * radiusOffset * (float)Math.Sin(time * spriteSpeed + angleOffset)
                };

                var sourceRectangle = new DrawingRectangle(0, (int)((float)random.NextDouble() * 31) * 32, 32, 32);
                spriteBatch.Draw(ballsTexture, position, sourceRectangle, Color.White, 0.0f, new Vector2(textureHalfSize, textureHalfSize), Vector2.One, SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            // Update the FPS text
            frameCount++;
            if (fpsClock.ElapsedMilliseconds > 1000.0f)
            {
                fpsText = string.Format("{0:F2} FPS", (float)frameCount * 1000 / fpsClock.ElapsedMilliseconds);
                frameCount = 0;
                fpsClock.Restart();
            }

            // Render the text
            spriteBatch.Begin();
            spriteBatch.DrawString(arial16BMFont, "  " + SpriteCount + "\nSprites", new Vector2(spriteSceneWidth - 32, spriteSceneHeight- 24), Color.White);
            spriteBatch.DrawString(arial16BMFont, fpsText, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
