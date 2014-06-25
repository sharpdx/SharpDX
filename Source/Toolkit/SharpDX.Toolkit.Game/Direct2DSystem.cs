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

#if DIRECTX11_1 && !WP8

namespace SharpDX.Toolkit
{
    using System;
    using Direct3D11;
    using System.Collections.Generic;
    using Graphics;

    /// <summary>
    /// A game system which provides <see cref="Direct2DSurface"/> management.
    /// </summary>
    public class Direct2DSystem : GameSystem
    {
        private readonly List<Direct2DSurface> surfaces = new List<Direct2DSurface>();

        private IDirect2DService d2DService;
        private SpriteBatch spriteBatch;
        private Rectangle surfaceDestinationRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct2DSystem"/> class.
        /// </summary>
        /// <param name="game">The game where this system should be attached.</param>
        public Direct2DSystem(Game game)
            : base(game)
        {
            Visible = true;
            Enabled = true;

            // it will be drawn last, can be overriden
            DrawOrder = int.MaxValue;

            Game.GameSystems.Add(this);
        }

        /// <summary>
        /// Exposes the surfaces collection. To properly initialize surfaces and load their content, it is recommended to populate this collection before calling the <see cref="Initialize"/> method.
        /// </summary>
        public ICollection<Direct2DSurface> Surfaces { get { return surfaces; } }

        /// <summary>
        /// Initializes this game system instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">Is thrown when the underlying game doesn't have registered a <see cref="IDirect2DService"/>.</exception>
        /// <remarks>The surfaces must be registered before this method call, otherwise they may not be initialized correctly.</remarks>
        public override void Initialize()
        {
            base.Initialize();

            d2DService = this.GetService<IDirect2DService>();

            foreach (var surface in surfaces)
                surface.Initialize();
        }

        /// <summary>
        /// Updates all visible surfaces which are registed in this instance.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var surface in surfaces)
            {
                if (surface.IsVisible)
                    surface.Update(gameTime);
            }
        }

        /// <summary>
        /// Performs redrawing of dirty surfaces and draws them on the currently assigned backbuffer using an <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="gameTime">The actual game time.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (DoesAnySurfaceNeedRedraw())
            {
                using (GraphicsDevice.Performance.CreateEvent("Draw surfaces"))
                {
                    var ctx = d2DService.DeviceContext;

                    ctx.BeginDraw();

                    foreach (var surface in surfaces)
                    {
                        if (surface.IsDirty && surface.IsVisible)
                            surface.Draw();
                    }

                    ctx.EndDraw();
                }
            }

            using (GraphicsDevice.Performance.CreateEvent("Draw render targets"))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, GraphicsDevice.BlendStates.AlphaBlend, GraphicsDevice.SamplerStates.PointClamp);
                foreach (var surface in surfaces)
                {
                    if (surface.IsVisible)
                        spriteBatch.Draw(surface.Surface, surfaceDestinationRectangle, Color.White);
                }
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Determines if any registered surface needs to be redrawn to avoid creating useless performance marker events.
        /// </summary>
        /// <returns></returns>
        private bool DoesAnySurfaceNeedRedraw()
        {
            foreach (var surface in surfaces)
            {
                if (surface.IsDirty && surface.IsVisible)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the content in all surfaces and prepares backbuffer-dependent resources.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));
            surfaceDestinationRectangle = new Rectangle(0, 0, GraphicsDevice.BackBuffer.Width, GraphicsDevice.BackBuffer.Height);

            foreach (var surface in surfaces)
                surface.LoadContent();
        }

        /// <summary>
        /// Unloads the associated content and all registered surfaces.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            foreach (var surface in surfaces)
                surface.UnloadContent();
        }
    }
}

#endif