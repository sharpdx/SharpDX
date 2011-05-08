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
using System.Xml;

namespace SharpDoc.Model
{
    /// <summary>
    /// Base class for <see cref="IModelReference"/>.
    /// </summary>
    public abstract class NModelBase : IModelReference, IComment
    {
        private XmlNode _docNode;

        /// <summary>
        /// Gets or sets the XML generated commment ID.
        /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x.aspx for more information.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the file id. THis is a version of the <see cref="IModelReference.Id"/> that
        /// can be used for filename.
        /// </summary>
        /// <value>The file id.</value>
        public string NormalizedId { get; set; }

        /// <summary>
        /// Gets or sets the name of this instance.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full name of this instance.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="XmlNode"/> extracted from the code comments 
        /// for a particular member.
        /// </summary>
        /// <value>The XmlNode doc.</value>
        public XmlNode DocNode
        {
            get { return _docNode; }
            set
            {
                _docNode = value;
                OnDocNodeUpdate();
            }
        }

        /// <summary>
        /// Gets or sets the description extracted from the &lt;summary&gt; tag of the <see cref="IComment.DocNode"/>.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the remarks extracted from the &lt;remarks&gt; tag of the <see cref="IComment.DocNode"/>.
        /// </summary>
        /// <value>The remarks.</value>
        public string Remarks { get; set; }

        /// <summary>
        /// Called when <see cref="DocNode"/> is updated.
        /// </summary>
        protected virtual void OnDocNodeUpdate()
        {
            if (DocNode != null)
            {
                Description = DocFromTag(DocTag.Summary);
                Remarks = DocFromTag(DocTag.Remarks);
            }
        }

        /// <summary>
        /// Extract a comment from tag inside the <see cref="DocNode"/> associated
        /// to this element.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        public string DocFromTag(string tagName)
        {
            if (DocNode != null)
            {
                var selectedNode = DocNode.SelectSingleNode(tagName);
                if (selectedNode != null)
                    return selectedNode.InnerXml.Trim();
            }
            return null;
        }
    }
}