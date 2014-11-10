// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// The <see cref="MouseManager"/> component provides access to mouse state
    /// </summary>
    public class MouseManager : GameSystem, IMouseService
    {
        // as the MouseState structure is immutable - keep a state from which the structure can be rebuilt
        private MouseState state;
        private MouseState nextState;
        private bool isFirstUpdateDone;

        // provides platform-specific binding to mouse functionality
        private MousePlatform platform;

        /// <summary>
        /// Initializes a new instance of <see cref="MouseManager"/> class
        /// </summary>
        /// <param name="game">The <see cref="Game"/> instance whose window is used as source of mouse input events</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="game"/> is null</exception>
        public MouseManager(Game game) : base(game)
        {
            Enabled = true;
            // register the mouse service instance
            game.Services.AddService(typeof(IMouseService), this);
            // register the mouse manager as game system
            game.GameSystems.Add(this);
        }

        /// <summary>
        /// Initializes this instance and starts listening to mouse input events
        /// </summary>
        /// <exception cref="NotSupportedException">Is thrown if mouse manager is used on an unsupported platform.</exception>
        public override void Initialize()
        {
            base.Initialize();
            // create platform-specific instance
            platform = MousePlatform.Create(Game.Window.NativeWindow);

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
            return state;
        }

        /// <summary>
        /// Sets the position of mouse pointer
        /// </summary>
        /// <param name="point">The desired position in the range X/Y [0,1]</param>
        /// <exception cref="InvalidOperationException">Is thrown when <see cref="MouseManager"/> is not initialized</exception>
        public void SetPosition(Vector2 point)
        {
            if (platform == null)
                throw new InvalidOperationException("MouseManager is not initialized.");

            platform.SetLocation(point);
        }

        /// <summary>
        /// Updates the mouse states.
        /// </summary>
        /// <param name="gameTime">Not used.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var position = platform.GetLocation();
            nextState.x = position.X;
            nextState.y = position.Y;

            if(!isFirstUpdateDone)
            {
                state = nextState;
            }

            var previousState = state;
            state = nextState;
            state.deltaX = state.X - previousState.X;
            state.deltaY = state.Y - previousState.Y;

            nextState.wheelDelta = 0;
            nextState.leftButton.ResetEvents();
            nextState.middleButton.ResetEvents();
            nextState.rightButton.ResetEvents();
            nextState.xButton1.ResetEvents();
            nextState.xButton2.ResetEvents();
            isFirstUpdateDone = true;
        }

        /// <summary>
        /// Handler for <see cref="MousePlatform.MouseDown"/> event
        /// </summary>
        /// <param name="button">The pressed button</param>
        private void HandleMouseDown(MouseButton button)
        {
            SetButtonStateTo(button, true);
        }

        /// <summary>
        /// Handler for <see cref="MousePlatform.MouseUp"/> event
        /// </summary>
        /// <param name="button">The pressed button</param>
        private void HandleMouseUp(MouseButton button)
        {
            SetButtonStateTo(button, false);
        }

        /// <summary>
        /// Handler for <see cref="MousePlatform.MouseWheelDelta"/> event
        /// </summary>
        /// <param name="wheelDelta">The pressed button</param>
        private void HandleWheelDelta(int wheelDelta)
        {
            nextState.wheelDelta += wheelDelta;
        }

        /// <summary>
        /// Sets the state of specified mouse button
        /// </summary>
        /// <param name="button">The button whose state needs to be set.</param>
        /// <param name="isDown">if set to <c>true</c> [is pressed].</param>
        /// <exception cref="System.ArgumentOutOfRangeException">button</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown if the <paramref name="button" /> has an unknown value.</exception>
        private void SetButtonStateTo(MouseButton button, bool isDown)
        {
            switch (button)
            {
                case MouseButton.None:
                    break;
                case MouseButton.Left:
                    HandleState(ref nextState.leftButton, isDown);
                    break;
                case MouseButton.Middle:
                    HandleState(ref nextState.middleButton, isDown);
                    break;
                case MouseButton.Right:
                    HandleState(ref nextState.rightButton, isDown);
                    break;
                case MouseButton.XButton1:
                    HandleState(ref nextState.xButton1, isDown);
                    break;
                case MouseButton.XButton2:
                    HandleState(ref nextState.xButton2, isDown);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }

        /// <summary>
        /// Computes the next state from the provided button sate.
        /// </summary>
        /// <param name="state">The current button state.</param>
        /// <param name="isDown">A value indicating whether the button is pressed or not.</param>
        private static void HandleState(ref ButtonState state, bool isDown)
        {
            if(isDown)
            {
                if(!state.Down)
                {
                    state.Pressed = true;
                }
                state.Down = true;
                state.Released = false;
            }
            else
            {
                state.Released = state.Down;
                state.Down = false;
                state.Pressed = false;
            }
        }
    }
}