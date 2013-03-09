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

namespace MouseInput
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using System.Text;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    /// <summary>
    /// Simple SpriteBatchAndFont application using SharpDX.Toolkit.
    /// The purpose of this application is to use SpriteBatch and SpriteFont.
    /// </summary>
    public class MouseInputGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private MouseManager mouseManager;
        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;
        private MouseState mouseState;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchAndFontGame" /> class.
        /// </summary>
        public MouseInputGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Create the mouse manager
            mouseManager = new MouseManager(this);

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
            Window.Title = "MouseInput demo";
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // print the current mouse state
            var sb = new StringBuilder();
            sb.AppendFormat("Left button  : {0}\n", mouseState.Left);
            sb.AppendFormat("Middle button: {0}\n", mouseState.Middle);
            sb.AppendFormat("Right button : {0}\n", mouseState.Right);
            sb.AppendFormat("XButton1     : {0}\n", mouseState.XButton1);
            sb.AppendFormat("XButton2     : {0}\n", mouseState.XButton2);
            sb.AppendFormat("X            : {0}\n", mouseState.X);
            sb.AppendFormat("Y            : {0}\n", mouseState.Y);
            sb.AppendFormat("Wheel        : {0}\n", mouseState.WheelDelta);

            // Render the text
            spriteBatch.Begin();
            spriteBatch.DrawString(arial16BMFont, sb.ToString(), new Vector2(8, 32), Color.White);
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            // read the current mouse state
            mouseState = mouseManager.GetState();

            base.Update(gameTime);
        }
    }
}
