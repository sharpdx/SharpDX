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

namespace MultiWindow
{
    public class MiniTriRenderer : GameWindowRenderer
    {
        private PrimitiveBatch<VertexPositionColor> primitiveBatch;
        private BasicEffect basicEffect;

        public MiniTriRenderer(Game game, object windowContext = null)
            : base(game, windowContext)
        {
            BackgroundColor = Color.CornflowerBlue;
            ForegroundColor = Color.Red;
            Visible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniTriRenderer" /> class.
        /// </summary>
        protected override void LoadContent()
        {
            // Creates a basic effect
            Utilities.Dispose(ref basicEffect);
            basicEffect = new BasicEffect(GraphicsDevice)
                              {
                                  VertexColorEnabled = true,
                                  View = Matrix.Identity,
                                  Projection = Matrix.Identity,
                                  World = Matrix.Identity
                              };

            // Creates primitive bag
            Utilities.Dispose(ref primitiveBatch);
            primitiveBatch = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);

            Window.AllowUserResizing = true;
        }

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            basicEffect.CurrentTechnique.Passes[0].Apply();
            primitiveBatch.Begin();
            primitiveBatch.DrawTriangle(
                new VertexPositionColor(new Vector3(-0.8f, -0.8f, 0.0f), ForegroundColor), 
                new VertexPositionColor(new Vector3( 0.0f,  0.8f, 0.0f), ForegroundColor), 
                new VertexPositionColor(new Vector3( 0.8f, -0.8f, 0.0f), ForegroundColor)                
                );
            primitiveBatch.End();
        }
    }
}