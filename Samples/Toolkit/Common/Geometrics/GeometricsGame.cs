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
using System.Collections.Generic;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Geometrics
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Simple Geometrics application using SharpDX.Toolkit.
    /// The purpose of this application is to display primitive builtin geometries (Cube, Plane, Torus, Teapot...etc.) 
    /// using <see cref="GeometricPrimitive"/> with a <see cref="BasicEffect"/>.
    /// </summary>
    public class GeometricsGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private BasicEffect basicEffect;
        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;
        private PointerManager pointer;

        private Texture2D texture;

        private List<GeometricPrimitive> primitives;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricsGame" /> class.
        /// </summary>
        public GeometricsGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Creates the pointer manager
            pointer = new PointerManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Creates a basic effect
            basicEffect = ToDisposeContent(new BasicEffect(GraphicsDevice)
                {
                    View = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY),
                    Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
                    World = Matrix.Identity
                });

            basicEffect.PreferPerPixelLighting = true;
            basicEffect.EnableDefaultLighting();

            // Creates all primitives
            primitives = new List<GeometricPrimitive>
                             {
                                 ToDisposeContent(GeometricPrimitive.Plane.New(GraphicsDevice)),
                                 ToDisposeContent(GeometricPrimitive.Cube.New(GraphicsDevice)),
                                 ToDisposeContent(GeometricPrimitive.Sphere.New(GraphicsDevice)),
                                 ToDisposeContent(GeometricPrimitive.GeoSphere.New(GraphicsDevice)),
                                 ToDisposeContent(GeometricPrimitive.Cylinder.New(GraphicsDevice)),
                                 ToDisposeContent(GeometricPrimitive.Torus.New(GraphicsDevice)),
                                 ToDisposeContent(GeometricPrimitive.Teapot.New(GraphicsDevice))
                             };

            // Load a SpriteFont
            arial16BMFont = Content.Load<SpriteFont>("Arial16");

            // Instantiate a SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the texture
            texture = Content.Load<Texture2D>("GeneticaMortarlessBlocks");
            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;

            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Geometrics demo";

            base.Initialize();
        }


        protected override void Update(GameTime gameTime)
        {
            var pointerState = pointer.GetState();
            if (pointerState.Points.Count > 0 && pointerState.Points[0].EventType == PointerEventType.Released)
            {
                basicEffect.TextureEnabled = !basicEffect.TextureEnabled;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Render the text
            spriteBatch.DrawString(arial16BMFont, string.Format("Press the pointer\nto switch the\ntexture mode.\n\nTexture Mode: ({0})", basicEffect.TextureEnabled ? "On" : "Off"), new Vector2(40, 40), Color.White);

            // Render each primitive
            for (int i = 0; i < primitives.Count; i++)
            {
                var primitive = primitives[i];

                // Calculate the translation
                float dx = ((i + 1) % 4);
                float dy = ((i + 1) / 4);

                float x = (dx - 1.5f) * 1.7f;
                float y = 1.0f - 2.0f * dy;

                var time = (float)gameTime.TotalGameTime.TotalSeconds + i;

                // Setup the World matrice for this primitive
                basicEffect.World = Matrix.Scaling((float)Math.Sin(time*1.5f) * 0.2f + 1.0f) * Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f) * Matrix.Translation(x, y, 0);

                // Render the name of the primitive
                spriteBatch.DrawString(arial16BMFont, primitive.Name, new Vector2(GraphicsDevice.BackBuffer.Width * (0.08f + dx / 4.0f), GraphicsDevice.BackBuffer.Height * (0.47f + dy / 2.2f)), Color.White);

                // Disable Cull only for the plane primitive, otherwise use standard culling
                GraphicsDevice.SetRasterizerState(i == 0 ? GraphicsDevice.RasterizerStates.CullNone : GraphicsDevice.RasterizerStates.CullBack);

                // Draw the primitive using BasicEffect
                primitive.Draw(basicEffect);
            }
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
