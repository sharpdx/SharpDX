// Copyright (c) 2010-2013 SharpDX - SharpDX Team
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

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// A base class for a pool of items.
    /// </summary>
    /// <typeparam name="TItem">The item type that will be managed by the pool.</typeparam>
    internal abstract class Pool<TItem>
    {
        private readonly Queue<TItem> freeItems;
        private List<TItem> activeItems;
        private List<TItem> swap;

        protected Pool()
        {
            activeItems = new List<TItem>();
            swap = new List<TItem>();
            freeItems = new Queue<TItem>();
        }

        /// <summary>
        /// Tries to either reuse an existing item or to create a new one.
        /// </summary>
        /// <param name="trackItem">Indicates whether to track the new item as active or not.</param>
        /// <returns>The created or recycled item.</returns>
        /// <exception cref="InvalidOperationException">Is thrown when there is no available item to recycle and new one cannot be created.</exception>
        public TItem Acquire(bool trackItem)
        {
            TItem item;
            if (TryAcquire(trackItem, out item))
            {
                return item;
            }

            throw new InvalidOperationException(string.Format("Unable to create item of type {0}.", typeof(TItem).Name));
        }

        /// <summary>
        /// Clears this pool from all active and free items.
        /// </summary>
        public void Clear()
        {
            lock (freeItems)
            {
                foreach (var item in activeItems)
                {
                    ClearItem(item);
                }

                activeItems.Clear();

                while (freeItems.Count > 0)
                {
                    ClearItem(freeItems.Dequeue());
                }
            }
        }

        /// <summary>
        /// Returns an used item for recycling.
        /// </summary>
        /// <param name="item">The item to return for recycling.</param>
        public void Return(TItem item)
        {
            lock (freeItems)
            {
                freeItems.Enqueue(item);
            }
        }

        /// <summary>
        /// Tries to acquire a free item or create a new one.
        /// </summary>
        /// <param name="trackItem">Indicates whether to track the new item as active or not.</param>
        /// <param name="item">The acquired item or default(TItem) if the operation did not succeed.</param>
        /// <returns>true if the operation succeeded, false - otherwise.</returns>
        public bool TryAcquire(bool trackItem, out TItem item)
        {
            lock (freeItems)
            {
                if (freeItems.Count == 0)
                    AuditItems();

                if (freeItems.Count == 0)
                {
                    if (!TryCreate(trackItem, out item))
                        return false;
                }
                else
                {
                    item = freeItems.Dequeue();
                    if (!TryReset(trackItem, item))
                        return false;
                }

                if (trackItem)
                    activeItems.Add(item);

                return true;
            }
        }

        /// <summary>
        /// Allows derived classes to execute additional logic when an item is cleared from this pool.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void ClearItem(TItem item)
        {
        }

        /// <summary>
        /// Derived classes must implement this method to determine if an item is active or not.
        /// </summary>
        /// <param name="item">The item to determine if it is active.</param>
        /// <returns>true if the item is active, false - otherwise.</returns>
        protected abstract bool IsActive(TItem item);

        /// <summary>
        /// Derived classes must implement this method to allow creation of a new item.
        /// </summary>
        /// <param name="trackItem">Indicates whether to track the new item as active or not.</param>
        /// <param name="item">The created item.</param>
        /// <returns>true if creation succeeded, false - otherwise.</returns>
        protected abstract bool TryCreate(bool trackItem, out TItem item);

        /// <summary>
        /// Tries to recycle an item. The default implementation doesn't do anything.
        /// </summary>
        /// <param name="trackItem">Indicates whether to track the new item as active or not.</param>
        /// <param name="item">The item to recycle.</param>
        /// <returns>true if recycling succeeded, false - otherwise. The default implementation always returns true.</returns>
        protected virtual bool TryReset(bool trackItem, TItem item)
        {
            return true;
        }

        /// <summary>
        /// Recomputes the lists of active and free items.
        /// </summary>
        private void AuditItems()
        {
            foreach (var item in activeItems)
            {
                if (IsActive(item))
                {
                    swap.Add(item);
                }
                else
                {
                    freeItems.Enqueue(item);
                }
            }

            var temp = activeItems;
            activeItems = swap;
            swap = temp;
            swap.Clear();
        }
    }
}