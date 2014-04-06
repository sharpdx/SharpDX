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

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Provides access to keyboard state
    /// </summary>
    public class KeyboardManager : GameSystem, IKeyboardService
    {
        private KeyboardState state;
        private KeyboardState nextState;

        // provides platform-specific bindings to keyboard events
        private KeyboardPlatform platform;

        /// <summary>
        /// Creates a new instance of <see cref="KeyboardManager"/> class.
        /// </summary>
        /// <param name="game">The <see cref="Game"/> instance whose window is used as source of keyboard input events</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="game"/> is null</exception>
        public KeyboardManager(Game game) : base(game)
        {
            Enabled = true;
            // register the keyboard service instance
            game.Services.AddService(typeof(IKeyboardService), this);
            // register the keyboard manager as game system
            game.GameSystems.Add(this);
        }

        /// <summary>
        /// Initializes this instance and starts listening to keyboard input events
        /// </summary>
        /// <exception cref="NotSupportedException">Is thrown if keyboard manager is used on an unsupported platform.</exception>
        public override void Initialize()
        {
            base.Initialize();

            // create the platform-specific instance
            platform = KeyboardPlatform.Create(Game.Window.NativeWindow);

            // bind to platform-independent events
            platform.KeyDown += HandleKeyDown;
            platform.KeyUp += HandleKeyUp;
        }

        /// <summary>
        /// Gets current keyboard state.
        /// </summary>
        /// <returns>A snapshot of current keyboard state</returns>
        public KeyboardState GetState()
        {
            return state;
        }

        /// <summary>
        /// Handles the <see cref="KeyboardPlatform.KeyDown"/> event
        /// </summary>
        /// <param name="key">The pressed key</param>
        private void HandleKeyDown(Keys key)
        {
            var keyState = nextState[key];

            if (!keyState.Down)
            {
                keyState.Pressed = true;
            }
            keyState.Down = true;
            nextState[key] = keyState;
        }

        /// <summary>
        /// Handles the <see cref="KeyboardPlatform.KeyUp"/> event
        /// </summary>
        /// <param name="key">The released key</param>
        private void HandleKeyUp(Keys key)
        {
            nextState[key] = ButtonStateFlags.Released;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            state = nextState;

            nextState.ResetPressedReleased();
        }
    }
}