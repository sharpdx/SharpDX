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

namespace SharpDX
{
    /// <summary>
    /// A disposable component base class.
    /// </summary>
    public abstract class Component : ComponentBase, IDisposable
    {
        /// <summary>
        /// Gets or sets the disposables.
        /// </summary>
        /// <value>The disposables.</value>
        private DisposeCollector Disposables { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        protected internal Component()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component" /> class with an immutable name.
        /// </summary>
        /// <param name="name">The name.</param>
        protected Component(string name) : base(name)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is attached to a collector.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is attached to a collector; otherwise, <c>false</c>.
        /// </value>
        internal bool IsAttached { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        protected internal bool IsDisposed { get; private set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(true);
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Disposes of object resources.
        /// </summary>
        /// <param name="disposeManagedResources">If true, managed resources should be
        /// disposed of in addition to unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                // Dispose all ComObjects
                if (Disposables != null)
                    Disposables.Dispose();
                Disposables = null;
            }
        }

        /// <summary>
        /// Adds a disposable object to the list of the objects to dispose.
        /// </summary>
        /// <param name="toDisposeArg">To dispose.</param>
        protected internal T ToDispose<T>(T toDisposeArg) where T : class, IDisposable
        {
            if (toDisposeArg  != null)
            {
                if (Disposables == null)
                    Disposables = new DisposeCollector();
                return Disposables.Collect(toDisposeArg);
            }
            return null;
        }

        /// <summary>
        /// Dispose a disposable object and set the reference to null. Removes this object from the ToDispose list.
        /// </summary>
        /// <param name="objectToDispose">Object to dispose.</param>
        protected internal void RemoveAndDispose<T>(ref T objectToDispose) where T : class, IDisposable
        {
            if (objectToDispose != null && Disposables != null)
                Disposables.RemoveAndDispose(ref objectToDispose);
        }

        /// <summary>
        /// Removes a disposable object to the list of the objects to dispose.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toDisposeArg">To dispose.</param>
        protected internal void RemoveToDispose<T>(T toDisposeArg) where T : class, IDisposable
        {
            if (toDisposeArg != null && Disposables != null)
                Disposables.Remove(toDisposeArg);

        }
    }
}