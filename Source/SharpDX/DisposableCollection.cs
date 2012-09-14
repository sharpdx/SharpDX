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
using System.Collections;
using System.Collections.Generic;

namespace SharpDX
{
    /// <summary>
    /// A class to dispose several <see cref="IDisposable"/> instances.
    /// </summary>
    public class DisposableCollection : DisposeBase, IEnumerable<IDisposable>
    {
        private List<IDisposable> disposables;

        /// <summary>
        /// Gets the number of elements to dispose.
        /// </summary>
        /// <value>The number of elements to dispose.</value>
        public int Count
        {
            get { return disposables.Count; }
        }

        /// <summary>
        /// Disposes of object resources.
        /// </summary>
        /// <param name="disposeManagedResources">If true, managed resources should be
        /// disposed of in addition to unmanaged resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                // Dispose all ComObjects
                if (disposables != null)
                    for (int i = disposables.Count - 1; i >= 0; i--)
                    {
                        disposables[i].Dispose();
                        Remove(disposables[i]);
                    }
                disposables = null;
            }
        }

        /// <summary>
        /// Adds a disposable object to the list of the objects to dispose.
        /// </summary>
        /// <param name="toDisposeArg">To dispose.</param>
        public T Add<T>(T toDisposeArg) where T : class, IDisposable
        {
            IDisposable toDispose = toDisposeArg;
            var toDisposeComponent = toDispose as Component;

            // If this is a component and It's already attached, don't add it
            if (toDisposeComponent != null && toDisposeComponent.IsAttached)
                return toDisposeArg;

            if (toDispose != null)
            {
                if (disposables == null)
                    disposables = new List<IDisposable>();

                if (!disposables.Contains(toDispose))
                {
                    disposables.Add(toDispose);

                    // Set attached flag for Component
                    if (toDisposeComponent != null)
                        toDisposeComponent.IsAttached = true;
                }
            }
            return toDisposeArg;
        }

        /// <summary>
        /// Dispose a disposable object and set the reference to null. Removes this object from this instance..
        /// </summary>
        /// <param name="objectToDispose">Object to dispose.</param>
        public void RemoveAndDispose<T>(ref T objectToDispose) where T : class, IDisposable
        {
            if (objectToDispose != null && disposables != null)
            {
                Remove(objectToDispose);
                // Dispose the comonent
                objectToDispose.Dispose();
                objectToDispose = null;
            }
        }

        /// <summary>
        /// Removes a disposable object to the list of the objects to dispose.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toDisposeArg">To dispose.</param>
        public void Remove<T>(T toDisposeArg) where T : class, IDisposable
        {
            if (disposables != null && disposables.Contains(toDisposeArg))
            {
                disposables.Remove(toDisposeArg);

                // Set not attached flag
                var toDisposeComponent = toDisposeArg as Component;
                if (toDisposeComponent != null)
                    toDisposeComponent.IsAttached = false;
            }
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            return disposables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}