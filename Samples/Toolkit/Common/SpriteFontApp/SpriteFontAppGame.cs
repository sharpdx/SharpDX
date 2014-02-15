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
using System.Diagnostics;

using SharpDX;
using SharpDX.Toolkit;

namespace SpriteFontApp
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Simple SpriteFontApp application using SharpDX.Toolkit.
    /// The purpose of this application is to use SpriteFont.
    /// </summary>
    public class SpriteFontApp : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private SpriteFont arial13;
        private SpriteFont msSansSerif10;
        private SpriteFont arial16;
        private SpriteFont arial16ClearType;
        private SpriteFont arial16Bold;
        private SpriteFont courrierNew10;
        private SpriteFont calibri64;
        private Texture2D colorTexture;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFontApp" /> class.
        /// </summary>
        public SpriteFontApp()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this); 

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Load fonts
            arial13 = ToDisposeContent(Content.Load<SpriteFont>("Arial13"));
            msSansSerif10 = ToDisposeContent(Content.Load<SpriteFont>("MicrosoftSansSerif10"));
            arial16 = ToDisposeContent(Content.Load<SpriteFont>("Arial16"));
            arial16ClearType = ToDisposeContent(Content.Load<SpriteFont>("Arial16ClearType"));
            arial16Bold = ToDisposeContent(Content.Load<SpriteFont>("Arial16Bold"));
            calibri64 = ToDisposeContent(Content.Load<SpriteFont>("Calibri64"));
            courrierNew10 = ToDisposeContent(Content.Load<SpriteFont>("CourierNew10"));

            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));
            colorTexture = ToDisposeContent(Texture2D.New(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8.UNorm, new [] {Color.White}));

            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "SpriteFont demo";
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.Black);

            // Render the text
            spriteBatch.Begin();

            var text = "This text is in Arial 16 with anti-alias\nand multiline...";
            var dim = arial16.MeasureString(text);

            int x = 20, y = 20;
            spriteBatch.Draw(colorTexture, new SharpDX.Rectangle(x, y, (int)dim.X, (int)dim.Y), SharpDX.Color.Green);
            spriteBatch.DrawString(arial16, text, new SharpDX.Vector2(x, y), SharpDX.Color.White);
            spriteBatch.DrawString(courrierNew10, "Measured: " + dim, new SharpDX.Vector2(x, y + dim.Y + 5), SharpDX.Color.GreenYellow);

            text = @"
-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
Text using Courier New 10 fixed font
0123456789 - 0123456789 - 0123456789
ABCDEFGHIJ - ABCDEFGHIJ - A1C3E5G7I9
-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_";

            spriteBatch.DrawString(courrierNew10, text, new SharpDX.Vector2(x, y + dim.Y + 8), SharpDX.Color.White);

            spriteBatch.DrawString(arial13, "Arial 13, font with no-antialias when size <= 13.", new SharpDX.Vector2(x, y + 150), SharpDX.Color.White);
            spriteBatch.DrawString(msSansSerif10, "Microsoft Sans Serif 10, font with no-antialias when size <= 13.", new SharpDX.Vector2(x, y + 175), SharpDX.Color.White);

            spriteBatch.DrawString(arial16Bold, "Font is in bold - Arial 16", new SharpDX.Vector2(x, y + 190), SharpDX.Color.White);

            text = "Bigger font\nCalibri 64";
            y = 240;
            dim = calibri64.MeasureString(text);
            spriteBatch.Draw(colorTexture, new SharpDX.Rectangle(x, y, (int)dim.X, (int)dim.Y), SharpDX.Color.Red);
            spriteBatch.DrawString(calibri64, text, new SharpDX.Vector2(x, y), SharpDX.Color.White);

            text = "Rendering test\nRotated On Center";
            dim = arial16.MeasureString(text);
            spriteBatch.DrawString(arial16, text, new SharpDX.Vector2(600, 120), SharpDX.Color.White, -(float)gameTime.TotalGameTime.TotalSeconds, new Vector2(dim.X/2.0f, dim.Y/2.0f), 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.DrawString(arial16ClearType, "Arial16 - ClearType\nAbc /\\Z Ghi SWy {}:;=&%@", new SharpDX.Vector2(470, 250), SharpDX.Color.White);
            spriteBatch.DrawString(arial16, "Abc /\\Z Ghi SWy {}:;=&%@\nArial16 - Standard", new SharpDX.Vector2(470, 300), SharpDX.Color.White);

            spriteBatch.DrawString(arial16, "Arial16 simulate shadow", new SharpDX.Vector2(471, 391), SharpDX.Color.Red);
            spriteBatch.DrawString(arial16, "Arial16 simulate shadow", new SharpDX.Vector2(470, 390), SharpDX.Color.White);

            spriteBatch.DrawString(arial16, "Arial16 scaled x1.5", new SharpDX.Vector2(470, 420), SharpDX.Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);

            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
