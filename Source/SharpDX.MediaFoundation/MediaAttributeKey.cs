// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Associate an attribute key with a type used to retrieve keys from a <see cref="MediaAttributes"/> instance.
    /// </summary>
    public class MediaAttributeKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey"/> struct.
        /// </summary>
        /// <param name="guid">The attribute GUID.</param>
        /// <param name="type">The attribute type.</param>
        public MediaAttributeKey(Guid guid, Type type)
            : this(guid, type, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey"/> struct.
        /// </summary>
        /// <param name="guid">The attribute GUID.</param>
        /// <param name="type">The attribute type.</param>
        /// <param name="name">The attribute name, useful for debugging.</param>
        public MediaAttributeKey(Guid guid, Type type, string name)
        {
            Guid = guid;
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Gets  the attribute GUID.
        /// </summary>
        /// <value>
        /// The attribute GUID.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets  the attribute type.
        /// </summary>
        /// <value>
        /// The attribute type.
        /// </value>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the attribute name.
        /// </summary>
        public string Name { get; private set; }
    }


    /// <summary>
    /// Generic version of <see cref="MediaAttributeKey"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value of this key.</typeparam>
    public class MediaAttributeKey<T> : MediaAttributeKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The attribute GUID.</param>
        public MediaAttributeKey(string guid)
            : this(guid, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="name">The attribute name, useful for debugging.</param>
        public MediaAttributeKey(string guid, string name)
            : this(new Guid(guid), name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public MediaAttributeKey(Guid guid)
            : this(guid, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// /// <param name="name">The attribute name, useful for debugging.</param>
        public MediaAttributeKey(Guid guid, string name)
            : base(guid, typeof(T), name)
        {
        }
    }
}