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
using System.Xml;

namespace SharpDoc.Model
{
    /// <summary>
    /// A reference to a documentable element.
    /// </summary>
    public class NReference : IModelReference
    {
        private XmlNode _docNode;

        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the XML generated comment ID.
        /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x.aspx for more information.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the normalized id. This is a normalized version of the <see cref="IModelReference.Id"/> that
        /// can be used for filename.
        /// </summary>
        /// <value>The file id.</value>
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        public string PageTitle { get; set; }

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
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }

        public IModelReference Assembly { get; set; }

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
        protected internal virtual void OnDocNodeUpdate()
        {
            if (DocNode != null)
            {
                Description = NDocumentApi.GetTag(DocNode, DocTag.Summary);
                Remarks = NDocumentApi.GetTag(DocNode, DocTag.Remarks);
            }
        }
    }
}