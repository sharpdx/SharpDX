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
using System.Collections.Generic;

namespace SharpDX.Toolkit.Collections
{
    /// <summary>
    /// Event arguments for the <see cref="ObservableDictionary{TKey,TValue}.ItemAdded"/> and <see cref="ObservableDictionary{TKey,TValue}.ItemRemoved"/> events.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    public class ObservableDictionaryEventArgs<TKey, TValue> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionaryEventArgs{TKey,TValue}"/> class from the provided <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="pair">The <see cref="KeyValuePair{TKey,TValue}"/> that contains the event arguments.</param>
        public ObservableDictionaryEventArgs(KeyValuePair<TKey, TValue> pair)
            : this(pair.Key, pair.Value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionaryEventArgs{TKey,TValue}"/> class from the provided key and value.
        /// </summary>
        /// <param name="key">The event's key argument.</param>
        /// <param name="value">The event's value argument.</param>
        public ObservableDictionaryEventArgs(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the event's key argument.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// Gets the event's value argument.
        /// </summary>
        public TValue Value { get; private set; }
    }
}