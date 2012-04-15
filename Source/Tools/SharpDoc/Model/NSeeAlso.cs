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

namespace SharpDoc.Model
{
    /// <summary>
    /// A See Also reference
    /// </summary>
    public class NSeeAlso
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NSeeAlso"/> class.
        /// </summary>
        public NSeeAlso() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NSeeAlso"/> class.
        /// </summary>
        /// <param name="linkItem">The link item.</param>
        public NSeeAlso(IModelReference linkItem)
            : this("Reference", linkItem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NSeeAlso"/> class.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="linkItem">The link item.</param>
        public NSeeAlso(string groupName, IModelReference linkItem)
        {
            GroupName = groupName;
            LinkItem = linkItem;
        }

        /// <summary>
        /// Gets or sets the name of the group. Default is "Reference"
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the linked item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public IModelReference LinkItem { get; set; }
    }
}

