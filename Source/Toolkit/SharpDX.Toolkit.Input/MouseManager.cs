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

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// The <see cref="MouseManager"/> component provides access to mouse state
    /// </summary>
    public class MouseManager : Component, IGameSystem, IMouseService
    {
        private readonly Game game; // keep a reference to game to get access to native window during initialization

        // as the MouseState structure is inmutable - keep a state from which the structure can be rebuilt
        private ButtonState left;
        private ButtonState middle;
        private ButtonState right;
        private ButtonState xButton1;
        private ButtonState xButton2;
        private int wheelDelta;

        // provides platform-specific binding to mouse functionality
        private MousePlatform platform;

        /// <summary>
        /// Initializes a new instance of <see cref="MouseManager"/> class
        /// </summary>
        /// <param name="game">The <see cref="Game"/> instance whose window is used as source of mouse input events</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="game"/> is null</exception>
        public MouseManager(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            this.game = game;

            // register the mouse service instance
            game.Services.AddService(typeof(IMouseService), this);
            // register the mouse manager as game system
            game.GameSystems.Add(this);
        }

        /// <summary>
        /// Initializes this instance and starts listening to mouse input events
        /// </summary>
        /// <exception cref="NotSupportedException">Is thrown if mouse manager is used on an usupported platform.</exception>
        public void Initialize()
        {
            // create platform-specific instance
            platform = MousePlatform.Create(game.Window.NativeWindow);

            // platform will report state changes trough these events:
            platform.MouseDown += HandleMouseDown;
            platform.MouseUp += HandleMouseUp;
            platform.MouseWheelDelta += HandleWheelDelta;
        }

        /// <summary>
        /// Gets current mouse state.
        /// </summary>
        /// <returns>A snapshot of current mouse state</returns>
        /// <exception cref="NullReferenceException">Is thrown if <see cref="Initialize"/> is not called.</exception>
        public MouseState GetState()
        {
            // read the mouse position information
            var position = platform.GetLocation();

            return new MouseState(left, middle, right, xButton1, xButton2, position.X, position.Y, wheelDelta);
        }

        /// <summary>
        /// Handler for <see cref="MousePlatform.MouseDown"/> event
        /// </summary>
        /// <param name="button">The pressed button</param>
        private void HandleMouseDown(MouseButton button)
        {
            SetButtonStateTo(button, ButtonState.Pressed);
        }

        /// <summary>
        /// Handler for <see cref="MousePlatform.MouseUp"/> event
        /// </summary>
        /// <param name="button">The pressed button</param>
        private void HandleMouseUp(MouseButton button)
        {
            SetButtonStateTo(button, ButtonState.Released);
        }

        /// <summary>
        /// Handler for <see cref="MousePlatform.MouseWheelDelta"/> event
        /// </summary>
        /// <param name="wheelDelta">The pressed button</param>
        private void HandleWheelDelta(int wheelDelta)
        {
            this.wheelDelta = wheelDelta;
        }

        /// <summary>
        /// Sets the state of specified mouse button
        /// </summary>
        /// <param name="button">The button whose state needs to be set.</param>
        /// <param name="state">The new state of the button.</param>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown if the <paramref name="button"/> has an unknown value.</exception>
        private void SetButtonStateTo(MouseButton button, ButtonState state)
        {
            switch (button)
            {
                case MouseButton.None:
                    break;
                case MouseButton.Left:
                    left = state;
                    break;
                case MouseButton.Middle:
                    middle = state;
                    break;
                case MouseButton.Right:
                    right = state;
                    break;
                case MouseButton.XButton1:
                    xButton1 = state;
                    break;
                case MouseButton.XButton2:
                    xButton2 = state;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }
    }
}