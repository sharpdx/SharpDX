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
using System.Collections.ObjectModel;

namespace SharpDX.Collections
{
    /// <summary>The observable collection class.</summary>
    /// <typeparam name="T">Type of a collection item.</typeparam>
    public class ObservableCollection<T> : Collection<T>
    {
        /// <summary>Raised when an item is added to this instance.</summary>
        public event EventHandler<ObservableCollectionEventArgs<T>> ItemAdded;

        /// <summary>Raised when a item is removed from this instance.</summary>
        public event EventHandler<ObservableCollectionEventArgs<T>> ItemRemoved;

        /// <summary>Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1" />.</summary>
        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
                OnComponentRemoved(new ObservableCollectionEventArgs<T>(base[i]));

            base.ClearItems();
        }

        /// <summary>Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1" /> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentException">This item is already added</exception>
        protected override void InsertItem(int index, T item)
        {
            if (base.Contains(item))
                throw new ArgumentException("This item is already added");

            base.InsertItem(index, item);

            if (item != null)
                OnComponentAdded(new ObservableCollectionEventArgs<T>(item));
        }

        /// <summary>Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1" />.</summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            T item = base[index];
            base.RemoveItem(index);
            if (item != null)
                OnComponentRemoved(new ObservableCollectionEventArgs<T>(item));
        }

        /// <summary>Replaces the element at the specified index.</summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="System.NotSupportedException">Cannot set item into this instance</exception>
        protected override void SetItem(int index, T item)
        {
            throw new NotSupportedException("Cannot set item into this instance");
        }

        /// <summary>Called when [component added].</summary>
        /// <param name="e">The decimal.</param>
        private void OnComponentAdded(ObservableCollectionEventArgs<T> e)
        {
            EventHandler<ObservableCollectionEventArgs<T>> handler = ItemAdded;
            if (handler != null) handler(this, e);
        }

        /// <summary>Called when [component removed].</summary>
        /// <param name="e">The decimal.</param>
        private void OnComponentRemoved(ObservableCollectionEventArgs<T> e)
        {
            EventHandler<ObservableCollectionEventArgs<T>> handler = ItemRemoved;
            if (handler != null) handler(this, e);
        }
    }
}