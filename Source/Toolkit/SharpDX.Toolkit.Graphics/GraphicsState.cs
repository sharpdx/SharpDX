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

using System.Runtime.InteropServices;

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base class for all <see cref="GraphicsState"/>.
    /// </summary>
    public abstract class GraphicsState : Component
    {
        private readonly object lockCreate = new object();
        protected GraphicsDevice GraphicsDevice;
        protected DeviceChild State;
        private bool isStateInitialized;

        /// <summary>
        /// Initializes the specified device local.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        protected virtual void Initialize(GraphicsDevice deviceLocal)
        {
            GraphicsDevice = deviceLocal;

            // Try to create the blend state immediately
            if (deviceLocal != null)
            {
                State = CreateState();
                isStateInitialized = true;
            }
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator DeviceChild(GraphicsState from)
        {
            return from == null ? null : from.GetOrCreateState();
        }

        protected abstract DeviceChild CreateState();

        protected DeviceChild GetOrCreateState()
        {
            if (isStateInitialized)
                return State;

            lock (lockCreate)
            {
                if (State == null)
                {
                    GraphicsDevice = GraphicsDevice.Current;
                    State = ToDispose(CreateState());
                }
            }
            return State;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
                State = null;
        }

        /// <summary>
        /// Called when name changed for this component.
        /// </summary>
        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
                this.State.DebugName = Name;
        }
    }
}