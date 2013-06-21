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

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace SpriteBatchCustomEffect
{
    /// <summary>
    /// This sample shows how a custom shader can be used with SpriteBatch
    /// </summary>
    public class SpriteBatchCustomEffectGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Effect customEffect;

        public SpriteBatchCustomEffectGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "SpriteBatch Custom Effect";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));
            texture = Content.Load<Texture2D>("Input");
            customEffect = Content.Load<Effect>("Effect");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // assign the effect to current Begin/End batch
            spriteBatch.Begin(SpriteSortMode.Immediate, customEffect);

            // draw the image with our effect (it will transform it to grayscale) and modulate the result with a light-green color
            spriteBatch.Draw(texture, new Vector2(20, 20), Color.LightGreen);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
