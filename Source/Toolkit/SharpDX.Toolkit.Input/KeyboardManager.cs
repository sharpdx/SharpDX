// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Provides access to keyboard state
    /// </summary>
    public class KeyboardManager : Component, IGameSystem, IKeyboardService
    {
        // a reference to game is kept to access the native window in the Initialize method
        private readonly Game game;

        // a list of pressed keys:

#if WinFormsInterop && !NET35Plus // .NET 2.0
        private readonly List<Keys> pressedKeys = new List<Keys>();
#else
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
#endif

        // provides platform-specific bindings to keyboard events
        private KeyboardPlatform platform;

        /// <summary>
        /// Creates a new instance of <see cref="KeyboardManager"/> class.
        /// </summary>
        /// <param name="game">The <see cref="Game"/> instance whose window is used as source of keyboard input events</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="game"/> is null</exception>
        public KeyboardManager(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            this.game = game;

            // register the keyboard service instance
            game.Services.AddService(typeof(IKeyboardService), this);
            // register the keyboard manager as game system
            game.GameSystems.Add(this);
        }

        /// <summary>
        /// Initializes this instance and starts listening to keyboard input events
        /// </summary>
        /// <exception cref="NotSupportedException">Is thrown if keyboard manager is used on an usupported platform.</exception>
        public void Initialize()
        {
            // create the platform-specific instance
            platform = KeyboardPlatform.Create(game.Window.NativeWindow);

            // bind to platform-independent events
            platform.KeyPressed += HandleKeyPressed;
            platform.KeyReleased += HandleKeyReleased;
        }

        /// <summary>
        /// Gets current keyboard state.
        /// </summary>
        /// <returns>A snapshot of current keyboard state</returns>
        public KeyboardState GetState()
        {
            return new KeyboardState(pressedKeys);
        }

        /// <summary>
        /// Handles the <see cref="KeyboardPlatform.KeyPressed"/> event
        /// </summary>
        /// <param name="key">The pressed key</param>
        private void HandleKeyPressed(Keys key)
        {
            // need to remove pressed key only on .NET 2.0 platform, as others are using HashSet
            // for storage which disallows duplicates
#if WinFormsInterop && !NET35Plus // .NET 2.0
            pressedKeys.Remove(key);
#endif
            pressedKeys.Add(key);
        }

        /// <summary>
        /// Handles the <see cref="KeyboardPlatform.KeyReleased"/> event
        /// </summary>
        /// <param name="key">The released key</param>
        private void HandleKeyReleased(Keys key)
        {
            pressedKeys.Remove(key);
        }
    }
}