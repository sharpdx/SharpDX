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

namespace KeyboardInput
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using System.Text;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    /// <summary>
    /// Simple KeyboardInput application using SharpDX.Toolkit.
    /// The purpose of this application is to use SpriteBatch and SpriteFont.
    /// </summary>
    public class KeyboardInputGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private KeyboardManager keyboarManager;
        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;
        private KeyboardState keyboardState;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchAndFontGame" /> class.
        /// </summary>
        public KeyboardInputGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Create the keyboard manager
            keyboarManager = new KeyboardManager(this);

            // Force no vsync and use real timestep to print actual FPS
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            // SpriteFont supports the following font file format:
            // - DirectX Toolkit MakeSpriteFont or SharpDX Toolkit tkfont
            // - BMFont from Angelcode http://www.angelcode.com/products/bmfont/
            arial16BMFont = Content.Load<SpriteFont>("Arial16");

            // Instantiate a SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

            base.UnloadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "KeyboardInput demo";
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // print the current mouse state
            var sb = new StringBuilder();
            sb.AppendLine("Pressed keys:");

            foreach(var key in keyboardState.GetPressedKeys())
                sb.AppendFormat("Key: {0}, Code: {1}\n", key, (int)key);

            // Render the text
            spriteBatch.Begin();
            spriteBatch.DrawString(arial16BMFont, sb.ToString(), new Vector2(8, 32), Color.White);
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            // read the current keyboard state
            keyboardState = keyboarManager.GetState();

            base.Update(gameTime);
        }
    }
}
