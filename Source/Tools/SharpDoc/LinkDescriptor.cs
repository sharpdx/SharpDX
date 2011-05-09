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
using SharpDoc.Model;

namespace SharpDoc
{
    /// <summary>
    /// Describes a URL corresponding to a <see cref="Model.IModelReference"/>.
    /// </summary>
    public class LinkDescriptor
    {
        /// <summary>
        /// Gets or sets the id linked.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the local reference.
        /// </summary>
        /// <value>The local reference.</value>
        public IModelReference LocalReference { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the type of link.
        /// </summary>
        /// <value>The type.</value>
        public LinkType Type { get; set; }

        /// <summary>
        /// Gets or sets the attributes to add to the a href tag.
        /// </summary>
        /// <value>The attributes.</value>
        public string Attributes { get; set; }
    }
}