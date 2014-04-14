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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SharpDX.Diagnostics
{
    /// <summary>
    /// Use to provide a debug view on a custom collection. Use this by adding
    /// an attribute <c>[DebuggerTypeProxy(typeof(CollectionDebugView))]</c> to your custom collection.
    /// </summary>
    public class CollectionDebugView
    {
        private readonly ICollection collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionDebugView"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">collection</exception>
        public CollectionDebugView(ICollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            this.collection = collection;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public object[] Items
        {
            get
            {
                var array = new object[this.collection.Count];
                this.collection.CopyTo(array, 0);
                return array;
            }
        }
    }

    /// <summary>
    /// Use to provide a debug view on a custom generic collection. Use this by adding
    /// an attribute <c>[DebuggerTypeProxy(typeof(CollectionDebugView<>))]</c> to your custom generic collection.
    /// </summary>
    public class CollectionDebugView<T>
    {
        private readonly ICollection<T> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionDebugView{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">collection</exception>
        public CollectionDebugView(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            this.collection = collection;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] array = new T[this.collection.Count];
                this.collection.CopyTo(array, 0);
                return array;
            }
        }
    }
}