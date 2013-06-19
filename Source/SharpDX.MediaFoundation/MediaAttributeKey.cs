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
using System.Globalization;

using SharpDX.Win32;

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
        /// <param name="guid">The GUID.</param>
        /// <param name="type">The type.</param>
        public MediaAttributeKey(Guid guid, Type type)
        {
            Guid = guid;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; private set; }
    }


    /// <summary>
    /// Generic version of <see cref="MediaAttributeKey"/>
    /// </summary>
    /// <typeparam name="T">Type of the value of this key</typeparam>
    public class MediaAttributeKey<T> : MediaAttributeKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public MediaAttributeKey(string guid)
            : base(new Guid(guid), typeof(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public MediaAttributeKey(Guid guid) : base(guid, typeof(T))
        {
        }
    }
}