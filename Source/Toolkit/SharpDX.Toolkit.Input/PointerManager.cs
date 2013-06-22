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
    using System.Collections.Generic;

    /// <summary>
    /// Provides cross-platform access to pointer events
    /// </summary>
    public class PointerManager : Component, IGameSystem, IUpdateable, IPointerService
    {
        private readonly Game game; // keep a reference to game object to be able to initialize correctly
        private PointerPlatform platform; // keep a reference to pointer platform to be able to manipulate pointer

        private List<PointerPoint> pointerPoints = new List<PointerPoint>(); // the list of currently collected pointer points
        private List<PointerPoint> statePointerPoints = new List<PointerPoint>(); // the list of pointer points that will be copied to state

        private readonly object pointerPointLock = new object(); // keep a separate object for lock operations as "lock(this)" is a bad practice

        private bool enabled = true;
        private int updateOrder;

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

        /// <inheritdoc/>
        public bool Enabled
        {
            get { return enabled; }
            private set
            {
                if (value == enabled) return;
                enabled = value;
                RaiseEvent(EnabledChanged);
            }
        }

        /// <inheritdoc/>
        public int UpdateOrder
        {
            get { return updateOrder; }
            private set
            {
                if (value == updateOrder) return;
                updateOrder = value;
                RaiseEvent(UpdateOrderChanged);
            }
        }

        /// <inheritdoc/>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Initializes this instance of <see cref="PointerManager"/> class.
        /// </summary>
        /// <exception cref="NotSupportedException">Is thrown when this functionality is not supported on current platform</exception>
        public void Initialize()
        {
            // TODO: assume Initialize is called only once. Implement cleanup in case if it is called several times.
            platform =  ToDispose(PointerPlatform.Create(game.Window.NativeWindow, this));
        }

        /// <inheritdoc/>
        public PointerState GetState()
        {
            var state = new PointerState();

            GetState(state);

            return state;
        }

        /// <inheritdoc/>
        public void GetState(PointerState state)
        {
            if(state == null) throw new ArgumentNullException("state");

            state.Points.Clear();

            foreach(var point in statePointerPoints)
                state.Points.Add(point);
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            lock(pointerPointLock)
            {
                // swap the lists and clear the list that will be used to collect next pointer points

                var tmp = pointerPoints;
                pointerPoints = statePointerPoints;
                statePointerPoints = tmp;

                pointerPoints.Clear();
            }
        }

        /// <summary>
        /// Adds a pointer point to the raised events collection. It will be copied to pointer state at next update.
        /// </summary>
        /// <param name="point">The raised pointer event</param>
        internal void AddPointerEvent(ref PointerPoint point)
        {
            // use a simple lock at this time, to avoid excessive code complexity
            lock(pointerPointLock)
                pointerPoints.Add(point);
        }

        /// <summary>
        /// Raises a simple event in a thread-safe way due to stack-copy of delegate reference
        /// </summary>
        /// <param name="handler">The event handler that needs to be raised</param>
        private void RaiseEvent(EventHandler<EventArgs> handler)
        {
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}