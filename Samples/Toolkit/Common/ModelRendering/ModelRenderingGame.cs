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
using SharpDX.Direct3D11;
using SharpDX.Toolkit;

namespace ModelRendering
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Simple SpriteBatchAndFont application using SharpDX.Toolkit.
    /// The purpose of this application is to use SpriteBatch and SpriteFont.
    /// </summary>
    public class ModelRenderingGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;

        private BasicEffect effect;

        private Model model;
        private int frameCount;
        private string fpsText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRenderingGame" /> class.
        /// </summary>
        public ModelRenderingGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this); 
            graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Load the fonts
            arial16BMFont = Content.Load<SpriteFont>("Arial16");

            // Load the model
            model = Content.Load<Model>("duck");

            var texture = model.Materials[0].GetProperty(MaterialKeys.DiffuseTexture)[0].Texture;

            effect = ToDisposeContent(new BasicEffect(GraphicsDevice)
                         {
                             Texture = (Texture2D)texture,
                             World = Matrix.Identity,
                             View = Matrix.Identity,
                             Projection = Matrix.Identity,
                         });

            effect.View = Matrix.LookAtLH(new Vector3(0, 0, -25), new Vector3(0, 0, 0), Vector3.UnitY);
            effect.Projection = Matrix.PerspectiveFovLH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f);

            effect.TextureEnabled = true;

            effect.EnableDefaultLighting();
            effect.PreferPerPixelLighting = true;

            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            primitiveBatch = new PrimitiveBatch<VertexPositionNormalTexture>(GraphicsDevice);

            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Model Rendering Demo";
            base.Initialize();
        }

        private PrimitiveBatch<VertexPositionNormalTexture> primitiveBatch;

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);



            effect.World = Matrix.Scaling(0.1f) * Matrix.RotationY((float)gameTime.TotalGameTime.TotalSeconds) * Matrix.Translation(0, -8, 0);

            effect.CurrentTechnique.Passes[0].Apply();
            model.Meshes[0].MeshParts[0].Draw(GraphicsDevice);

            // Render the text
            spriteBatch.Begin();
            spriteBatch.DrawString(arial16BMFont, "Model rendering", new Vector2(0, 0), Color.White);
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
