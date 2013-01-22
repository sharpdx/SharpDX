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

using System.Collections;
using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A collection of attributes.
    /// </summary>
    public sealed class AttributeCollection : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> mapNameToValue;

        internal AttributeCollection(IEnumerable<AttributeData> attributes)
        {
            mapNameToValue = new Dictionary<string, object>();
            foreach (var attribute in attributes)
            {
                mapNameToValue[attribute.Name] = attribute.Value;
            }
        }

        /// <summary>
        /// Gets a specific element in the collection by using a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The value of the attribute. Null if not found.</returns>
        public object this[string name]
        {
            get
            {
                object value;
                mapNameToValue.TryGetValue(name, out value);
                return value;
            }
            set { mapNameToValue[name] = value; }
        }

        /// <summary>
        /// Gets the number of attributes.
        /// </summary>
        /// <value>The count of attributes.</value>
        public int Count
        {
            get { return mapNameToValue.Count; }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public IEnumerable<string> Keys
        {
            get { return mapNameToValue.Keys; }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<object> Values
        {
            get
            {
                return mapNameToValue.Values;
            }
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return mapNameToValue.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}