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
    internal abstract class Pool<TItem>
    {
        private List<TItem> activeItems;
        private Queue<TItem> freeItems;
        private List<TItem> swap;

        public Pool()
        {
            activeItems = new List<TItem>();
            swap = new List<TItem>();
            freeItems = new Queue<TItem>();
        }

        public TItem Acquire(bool trackItem)
        {
            TItem item;
            if (TryAcquire(trackItem, out item))
            {
                return item;
            }

            throw new InvalidOperationException(string.Format("Unable to create item of type {0}.", typeof(TItem).Name));
        }

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

        public void Return(TItem item)
        {
            lock (freeItems)
            {
                freeItems.Enqueue(item);
            }
        }

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

        protected virtual void ClearItem(TItem item)
        {
        }

        protected abstract bool IsActive(TItem item);

        protected abstract bool TryCreate(bool trackItem, out TItem item);

        protected virtual bool TryReset(bool trackItem, TItem item)
        {
            return true;
        }

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