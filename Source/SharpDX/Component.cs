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

namespace SharpDX
{
    /// <summary>
    /// Base class for a framework component. This class can be used as a base component to provide:
    /// <list type="bullet">
    /// <item><description>a way to create named disposable component which can be associated with some user tags.</description></item>
    /// <item><description>a container for disposable objects, being able to dispose dependent disposable.</description></item>
    /// <item><description>an automatic component container with todipose-region that will be able to add newly created components to a list of components to dispose. Use <see cref="PushCollector"/> and <see cref="PopCollector"/> to use this feature in a subclass container component.</description></item>
    /// </list>
    /// </summary>
    public class Component : IDisposable
    {
        /// <summary>
        /// Occurs while this component is disposing and before it is disposed.
        /// </summary>
        //internal event EventHandler<EventArgs> Disposing;
        private string name;

        /// <summary>
        /// Gets or sets the disposables.
        /// </summary>
        /// <value>The disposables.</value>
        private List<IDisposable> Disposables { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is attached to a collector.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is attached to a collector; otherwise, <c>false</c>.
        /// </value>
        private bool IsAttached { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        protected internal Component()
        {
            Disposables = new List<IDisposable>();
        }

        /// <summary>
        /// Gets the name of this component.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnNameChanged();
            }
        }

        protected virtual void OnNameChanged()
        {
        }

        /// <summary>
        /// Gets or sets the tag associated to this object.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

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
            // Notify listeners
            //if (Disposing != null)
            //    Disposing(this, EventArgs.Empty);

            if (disposeManagedResources)
            {
                // Dispose all ComObjects
                if (Disposables != null)
                    for (int i = Disposables.Count - 1; i >= 0; i--)
                    {
                        Disposables[i].Dispose();
                        RemoveToDispose(Disposables[i]);
                    }
                Disposables = null;
            }
        }

        /// <summary>
        /// Adds a disposable object to the list of the objects to dispose.
        /// </summary>
        /// <param name="toDisposeArg">To dispose.</param>
        protected internal T ToDispose<T>(T toDisposeArg) where T : class
        {
            var toDispose = (IDisposable)toDisposeArg;
            var toDisposeComponent = toDispose as Component;

            // If this is a component and It's already attached, don't add it
            if (toDisposeComponent != null && toDisposeComponent.IsAttached)
                return toDisposeArg;

            if (toDispose != null && !Disposables.Contains(toDispose))
            {
                Disposables.Add(toDispose);

                // Set attached flag for Component
                if (toDisposeComponent != null)
                    toDisposeComponent.IsAttached = true;
            }
            return toDisposeArg;
        }

        /// <summary>
        /// Dispose a disposable object and set the reference to null. Removes this object from the ToDispose list.
        /// </summary>
        /// <param name="objectToDispose">Object to dispose.</param>
        protected internal void SafeDispose<T>(ref T objectToDispose) where T : class
        {
            var toDispose = (IDisposable)objectToDispose;

            if (toDispose != null)
            {
                RemoveToDispose(objectToDispose);
                // Dispose the comonent
                toDispose.Dispose();
                objectToDispose = null;
            }
        }

        /// <summary>
        /// Removes a disposable object to the list of the objects to dispose.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toDisposeArg">To dispose.</param>
        protected internal void RemoveToDispose<T>(T toDisposeArg) where T : class
        {
            var toDispose = (IDisposable)toDisposeArg;
            if (Disposables.Contains(toDispose))
            {
                Disposables.Remove(toDispose);

                // Set not attached flag
                var toDisposeComponent = toDispose as Component;
                if (toDisposeComponent != null)
                    toDisposeComponent.IsAttached = false;
            }
        }
    }
}