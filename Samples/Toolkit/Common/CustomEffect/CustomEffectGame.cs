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
using SharpDX.Direct3D11;
using SharpDX.Toolkit;

namespace CustomEffect
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Simple CustomEffect application using SharpDX.Toolkit.
    /// The purpose of this application is to use a custom Effect.
    /// </summary>
    public class CustomEffectGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private Effect metaTunnelEffect;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomEffectGame" /> class.
        /// </summary>
        public CustomEffectGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredDepthStencilFormat = DepthFormat.None;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "MetaTunnel Effect by XT95/Frequency";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Loads the effect
            metaTunnelEffect = Content.Load<Effect>("metatunnel.fxo");

            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            // As the shader is writing to the screen, we don't need to clear it.
            metaTunnelEffect.Parameters["w"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);

            // Draw a full screen quad using the specified effect.
            GraphicsDevice.DrawQuad(metaTunnelEffect);

            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
