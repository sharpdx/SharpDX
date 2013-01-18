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
    /// Provides cross-platform access to pointer events
    /// </summary>
    public class PointerManager : Component, IGameSystem, IPointerService
    {
        private readonly Game game; // keep a reference to game object to be able to initialize correctly
        private PointerPlatform platform; // keep a reference to pointer platform to be able to manipulate pointer

        /// <summary>
        /// Initializes a new instance of <see cref="PointerManager"/> class
        /// </summary>
        /// <param name="game">The <see cref="Game"/> instance whose window is used as source of pointer input events</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="game"/> is null</exception>
        public PointerManager(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            this.game = game;

            this.game.Services.AddService(typeof(IPointerService), this);
            this.game.GameSystems.Add(this);
        }

        /// <summary>
        /// Raised when <see cref="GameWindow.NativeWindow"/> object loses pointer capture
        /// </summary>
        public event Action<PointerPoint> PointerCaptureLost;

        /// <summary>
        /// Raised when pointer enters <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        public event Action<PointerPoint> PointerEntered;

        /// <summary>
        /// Raised when pointer exits <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        public event Action<PointerPoint> PointerExited;

        /// <summary>
        /// Raised when pointer moves on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        public event Action<PointerPoint> PointerMoved;

        /// <summary>
        /// Raised when pointer is pressed on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        public event Action<PointerPoint> PointerPressed;

        /// <summary>
        /// Raised when pointer is released on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        public event Action<PointerPoint> PointerReleased;

        /// <summary>
        /// Raised when pointer wheel is changed on <see cref="GameWindow.NativeWindow"/> object
        /// </summary>
        public event Action<PointerPoint> PointerWheelChanged;

        /// <summary>
        /// Initializes this instance of <see cref="PointerManager"/> class.
        /// </summary>
        /// <exception cref="NotSupportedException">Is thrown when this functionality is not supported on current platform</exception>
        public void Initialize()
        {
            // TODO: assume Initialize is called only once. Implement cleanup in case if it is called several times.
            platform = PointerPlatform.Create(game.Window.NativeWindow, this);
        }

        /// <summary>
        /// Raises the <see cref="PointerManager.PointerCaptureLost"/> event.
        /// </summary>
        /// <param name="point">Contains the information about pointer event</param>
        internal void RaiseCaptureLost(PointerPoint point)
        {
            RaisePointerEvent(PointerCaptureLost, point);
        }

        /// <summary>
        /// Raises the <see cref="PointerManager.PointerEntered"/> event.
        /// </summary>
        /// <param name="point">Contains the information about pointer event</param>
        internal void RaiseEntered(PointerPoint point)
        {
            RaisePointerEvent(PointerEntered, point);
        }

        /// <summary>
        /// Raises the <see cref="PointerManager.PointerExited"/> event.
        /// </summary>
        /// <param name="point">Contains the information about pointer event</param>
        internal void RaiseExited(PointerPoint point)
        {
            RaisePointerEvent(PointerExited, point);
        }

        /// <summary>
        /// Raises the <see cref="PointerManager.PointerMoved"/> event.
        /// </summary>
        /// <param name="point">Contains the information about pointer event</param>
        internal void RaiseMoved(PointerPoint point)
        {
            RaisePointerEvent(PointerMoved, point);
        }

        /// <summary>
        /// Raises the <see cref="PointerManager.PointerPressed"/> event.
        /// </summary>
        /// <param name="point">Contains the information about pointer event</param>
        internal void RaisePressed(PointerPoint point)
        {
            RaisePointerEvent(PointerPressed, point);
        }

        /// <summary>
        /// Raises the <see cref="PointerManager.PointerReleased"/> event.
        /// </summary>
        /// <param name="point">Contains the information about pointer event</param>
        internal void RaiseReleased(PointerPoint point)
        {
            RaisePointerEvent(PointerReleased, point);
        }
        
        /// <summary>
        /// Raises the <see cref="PointerManager.PointerWheelChanged"/> event.
        /// </summary>
        /// <param name="point">Contains the information about pointer event</param>
        internal void RaiseWheelChanged(PointerPoint point)
        {
            RaisePointerEvent(PointerWheelChanged, point);
        }

        /// <summary>
        /// Raises the specified pointer event
        /// </summary>
        /// <param name="handler">The event handler</param>
        /// <param name="point">Information about pointer event</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="point"/> is null</exception>
        private static void RaisePointerEvent(Action<PointerPoint> handler, PointerPoint point)
        {
            if (point == null) throw new ArgumentNullException("point");

            if (handler != null)
                handler(point);
        }
    }
}