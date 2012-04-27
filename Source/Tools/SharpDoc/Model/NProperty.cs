// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

namespace SharpDoc.Model
{
    /// <summary>
    /// A property document.
    /// </summary>
    public class NProperty : NMember
    {
        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>The type of the return.</value>
        public NTypeReference PropertyType { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has get method.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has get method; otherwise, <c>false</c>.
        /// </value>
        public bool HasGetMethod
        {
            get
            {
                return GetMethod != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has set method.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has set method; otherwise, <c>false</c>.
        /// </value>
        public bool HasSetMethod
        {
            get
            {
                return SetMethod != null;
            }
        }

        /// <summary>
        /// Gets or sets the get method.
        /// </summary>
        /// <value>
        /// The get method.
        /// </value>
        public NMethod GetMethod { get; set; }

        /// <summary>
        /// Gets or sets the set method.
        /// </summary>
        /// <value>
        /// The set method.
        /// </value>
        public NMethod SetMethod { get; set; }

        /// <summary>
        /// Gets or sets the value description.
        /// </summary>
        /// <value>
        /// The value description.
        /// </value>
        public string ValueDescription { get; set; }

        protected internal override void OnDocNodeUpdate()
        {
            base.OnDocNodeUpdate();
            ValueDescription = NDocumentApi.GetTag(DocNode, "value");
        }
    }
}