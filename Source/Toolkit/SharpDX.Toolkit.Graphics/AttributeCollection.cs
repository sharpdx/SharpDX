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

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A collection of attributes.
    /// </summary>
    public sealed class AttributeCollection : IEnumerable<AttributeData>, IDataSerializable
    {
        private Dictionary<string, object> mapNameToValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
        /// </summary>
        public AttributeCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public AttributeCollection(int capacity)
        {
            mapNameToValue = new Dictionary<string, object>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public AttributeCollection(IEnumerable<AttributeData> attributes)
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
        public IEnumerator<AttributeData> GetEnumerator()
        {
            return new AttributeDataEnumerator(mapNameToValue.GetEnumerator());
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            var reader = serializer.ReaderOnly(this);

            int count = reader.ReadInt32();

            if (mapNameToValue == null)
            {
                mapNameToValue = new Dictionary<string, object>(count);
            }

            var data = new AttributeData();
            for (int i = 0; i < count; i++)
            {
                data.Serialize(serializer);
                mapNameToValue.Add(data.Name, data.Value);
            }
        }

        private class AttributeDataEnumerator : IEnumerator<AttributeData>
        {
            private readonly IEnumerator<KeyValuePair<string, object>> values;

            public AttributeDataEnumerator(IEnumerator<KeyValuePair<string, object>> values)
            {
                this.values = values;
            }

            public void Dispose()
            {
                values.Dispose();
            }

            public bool MoveNext()
            {
                return values.MoveNext();
            }

            public void Reset()
            {
                values.Reset();
            }

            public AttributeData Current
            {
                get
                {
                    var data = values.Current;
                    return new AttributeData(data.Key,  data.Value);
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }
        }
    }
}