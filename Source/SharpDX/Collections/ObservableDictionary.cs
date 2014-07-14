// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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

namespace SharpDX.Collections
{
    /// <summary>
    /// An observable dictionary.
    /// </summary>
    /// <typeparam name="TKey">The dictionary's key type.</typeparam>
    /// <typeparam name="TValue">The dictionary's value type.</typeparam>
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary;

        /// <inheritdoc/>
        public ObservableDictionary()
            : this(0, null)
        {
        }

        /// <inheritdoc/>
        public ObservableDictionary(int capacity)
            : this(capacity, null)
        {
        }

        /// <inheritdoc/>
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        /// <inheritdoc/>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }

        /// <inheritdoc/>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : this(dictionary != null ? dictionary.Count : 0, comparer)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            foreach (var pair in dictionary)
                Add(pair.Key, pair.Value);
        }

        /// <inheritdoc/>
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        /// <summary>
        /// Returns the collection of the keys present in dictionary.
        /// </summary>
        public ICollection<TKey> Keys { get { return dictionary.Keys; } }

        /// <summary>
        /// Gets the collection of the values present in dictionary.
        /// </summary>
        public ICollection<TValue> Values { get { return dictionary.Values; } }

        /// <summary>
        /// Gets the cound of items present in dictionary.
        /// </summary>
        public int Count { get { return dictionary.Count; } }

        /// <summary>
        /// Is raised when a new item is added to the dictionary.
        /// </summary>
        public event EventHandler<ObservableDictionaryEventArgs<TKey, TValue>> ItemAdded;

        /// <summary>
        /// Is raised when an item is removed from the dictionary.
        /// </summary>
        public event EventHandler<ObservableDictionaryEventArgs<TKey, TValue>> ItemRemoved;

        /// <summary>
        /// Gets the enumerator of this dictionary.
        /// </summary>
        /// <returns>The enumerator instance of the dictionary.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        /// <summary>
        /// Removes all items from the dictionary.
        /// </summary>
        public void Clear()
        {
            var args = new List<ObservableDictionaryEventArgs<TKey, TValue>>();
            // event should be raised _after_ collection modification
            foreach (var pair in dictionary)
                args.Add(new ObservableDictionaryEventArgs<TKey, TValue>(pair));

            dictionary.Clear();

            foreach (var e in args)
                OnItemRemoved(e);
        }

        /// <summary>
        /// Adds a new value with the specified key to dictionary.
        /// </summary>
        /// <param name="key">The added key.</param>
        /// <param name="value">The added value.</param>
        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
            OnItemAdded(new ObservableDictionaryEventArgs<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Checks whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to check for presence.</param>
        /// <returns>true if the dictionary contains the provided key, false - otherwise.</returns>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the value corresponding to the specified key from dictionary.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>true if the item was removed, false - otherwise.</returns>
        public bool Remove(TKey key)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
                return false;

            dictionary.Remove(key);
            OnItemRemoved(new ObservableDictionaryEventArgs<TKey, TValue>(key, value));

            return true;
        }

        /// <summary>
        /// Tries to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">Contains the returned value on success.</param>
        /// <returns>true if the value was returned successfuly, false - otherwise.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets a value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The associated value.</returns>
        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
            set
            {
                TValue oldValue;
                var isReplace = dictionary.TryGetValue(key, out oldValue);
                dictionary[key] = value;

                if (isReplace)
                    OnItemRemoved(new ObservableDictionaryEventArgs<TKey, TValue>(key, oldValue));

                OnItemAdded(new ObservableDictionaryEventArgs<TKey, TValue>(key, value));
            }
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly; } }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);
            OnItemAdded(new ObservableDictionaryEventArgs<TKey, TValue>(item));
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            var removed = ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);
            if (removed)
                OnItemRemoved(new ObservableDictionaryEventArgs<TKey, TValue>(item));

            return removed;
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        protected virtual void OnItemAdded(ObservableDictionaryEventArgs<TKey, TValue> args)
        {
            var handler = ItemAdded;
            if (handler != null)
                handler(this, args);
        }

        /// <inheritdoc/>
        protected virtual void OnItemRemoved(ObservableDictionaryEventArgs<TKey, TValue> args)
        {
            var handler = ItemRemoved;
            if (handler != null)
                handler(this, args);
        }
    }
}