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
using System.Collections.Generic;

namespace SharpDX.Toolkit
{
    public class PropertyCollection : Dictionary<PropertyKey, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollection"/> class.
        /// </summary>
        public PropertyCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollection" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="PropertyCollection" /> can contain.</param>
        public PropertyCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollection"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public PropertyCollection(IDictionary<PropertyKey, object> dictionary)
            : base(dictionary)
        {
        }

        public void SetProperty<T>(PropertyKey<T> key, T value)
        {
            if (Utilities.IsEnum(typeof(T)))
            {
                var intValue = Convert.ToInt32(value);
                Add(key, intValue);
            }
            else
            {
                Add(key, value);
            }
        }

        public bool ContainsKey<T>(PropertyKey<T> key)
        {
            return base.ContainsKey(key);
        }

        public T GetProperty<T>(PropertyKey<T> key)
        {
            object value;
            return TryGetValue(key, out value) ? Utilities.IsEnum(typeof(T)) ? (T)Enum.ToObject(typeof(T), (int)value) : (T)value : default(T);
        }

        public virtual PropertyCollection Clone()
        {
            return (PropertyCollection)MemberwiseClone();
        }

    }
}