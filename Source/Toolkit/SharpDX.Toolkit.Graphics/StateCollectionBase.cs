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
using SharpDX.Toolkit;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base collection for Graphics device states (BlendState, DepthStencilState, RasterizerState).
    /// </summary>
    /// <typeparam name="T">Type of the state.</typeparam>
    public abstract class StateCollectionBase<T> : ComponentCollection<T>, IDisposable where T : ComponentBase
    {
        /// <summary>
        /// An allocator of state.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="name">The name of the state to create.</param>
        /// <returns>An instance of T or null if not supported.</returns>
        public delegate T StateAllocatorDelegate(GraphicsDevice device, string name);

        /// <summary>
        /// Gets the graphics device associated with this collection.
        /// </summary>
        protected readonly GraphicsDevice GraphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateCollectionBase{T}" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        protected StateCollectionBase(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }

        /// <summary>
        /// Sets this callback to create a state when a state with a particular name is not found.
        /// </summary>
        public StateAllocatorDelegate StateAllocatorCallback;

        void IDisposable.Dispose()
        {
            for(int i = Items.Count - 1; i >= 0; i--)
            {
                ((IDisposable)Items[i]).Dispose();
            }
            Items.Clear();
        }

        /// <summary>
        /// Registers the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <remarks>
        /// The name of the state must be defined.
        /// </remarks>
        public void Register(T state)
        {
            Add(state);
        }

        protected override T TryToGetOnNotFound(string name)
        {
            var handler = StateAllocatorCallback;
            if (handler != null) return handler(GraphicsDevice, name);
            return default(T);
        }
    }
}