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

using System.Collections.Generic;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Yaml;

namespace CustomLoadingData
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Simple CustomLoadingData application using SharpDX.Toolkit.
    /// The purpose of this application is to demonstrate how to load YAML data into .NET objects
    /// using the Content Manager.
    /// </summary>
    public class CustomLoadingDataGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private YamlManager yamlManager;
        private SpriteBatch spriteBatch;
        private MyData loadedMyData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLoadingDataGame" /> class.
        /// </summary>
        public CustomLoadingDataGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredDepthStencilFormat = DepthFormat.None;


            // In order to support Yaml loading, we need to add the YamlManager
            yamlManager = new YamlManager(this);
            GameSystems.Add(yamlManager);

            // Register that !MyData will map to our local type.
            // We could have used YamlTagAttribute directly on our MyData type from YamlDotNet library.
            yamlManager.RegisterTagMapping("MyData",typeof(MyData));

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "Yaml Data Loading";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            loadedMyData = Content.Load<MyData>("MyData.yml");
            Window.Title = string.Format("Yaml Data Loading: [{0}]", loadedMyData.Name);

            // Instantiate a SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);


            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

            base.UnloadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the color using MyData
            GraphicsDevice.Clear(loadedMyData.Color);

            // Render the text
            spriteBatch.Begin();
            spriteBatch.DrawString(loadedMyData.Font, loadedMyData.Name, new Vector2(0, 0), Color.White);
            var pos = new Vector2(0, loadedMyData.Font.MeasureString(loadedMyData.Name).Y);
            spriteBatch.DrawString(loadedMyData.Font, "And so was this font.", pos, Color.White);
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }


        /// <summary>
        /// A Simple type demonstrating how to load data from YAML to .NET object instance.
        /// </summary>
        public class MyData
        {
            public MyData()
            {
                Vectors = new List<Vector4>();
            }

            public string Name { get; set; }

            public Color Color { get; set; }

            public float SimpleValue { get; set; }

            public List<Vector4> Vectors { get; set; }

            public SpriteFont Font { get; set; }
        }
    }
}
