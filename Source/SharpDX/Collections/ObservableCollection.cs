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
    /// <summary>
    /// An observable collection.
    /// </summary>
    /// <typeparam name="T">Type of a collection item</typeparam>
    public class ObservableCollection<T> : Collection<T>
    {
        /// <summary>
        /// Raised when an item is added to this instance.
        /// </summary>
        /// <param name="" />
        public event EventHandler<ObservableCollectionEventArgs<T>> ItemAdded;

        /// <summary>
        /// Raised when a item is removed from this instance.
        /// </summary>
        /// <param name="" />
        public event EventHandler<ObservableCollectionEventArgs<T>> ItemRemoved;

        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
                OnComponentRemoved(new ObservableCollectionEventArgs<T>(base[i]));

            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            if (base.Contains(item))
                throw new ArgumentException("This item is already added");

            base.InsertItem(index, item);

            if (item != null)
                OnComponentAdded(new ObservableCollectionEventArgs<T>(item));
        }

        protected override void RemoveItem(int index)
        {
            T item = base[index];
            base.RemoveItem(index);
            if (item != null)
                OnComponentRemoved(new ObservableCollectionEventArgs<T>(item));
        }

        protected override void SetItem(int index, T item)
        {
            throw new NotSupportedException("Cannot set item into this instance");
        }

        private void OnComponentAdded(ObservableCollectionEventArgs<T> e)
        {
            EventHandler<ObservableCollectionEventArgs<T>> handler = ItemAdded;
            if (handler != null) handler(this, e);
        }

        private void OnComponentRemoved(ObservableCollectionEventArgs<T> e)
        {
            EventHandler<ObservableCollectionEventArgs<T>> handler = ItemRemoved;
            if (handler != null) handler(this, e);
        }
    }
}