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
using System.Xml;

namespace SharpDoc.Model
{
    /// <summary>
    /// Base class for <see cref="IModelReference"/>.
    /// </summary>
    public abstract class NModelBase : IModelReference, IEquatable<NModelBase>
    {
        private XmlNode _docNode;

        protected NModelBase()
        {
            SeeAlsos = new List<NSeeAlso>();
            Groups = new List<string>();
        }

        public int Index { get; set; }

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
        public string Category { get; protected set; }

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
        /// Gets or sets the un managed API.
        /// </summary>
        /// <value>
        /// The un managed API.
        /// </value>
        public string UnManagedApi { get; set; }

        /// <summary>
        /// Gets or sets the un managed API.
        /// </summary>
        /// <value>
        /// The un managed API.
        /// </value>
        public string UnManagedShortApi { get; set; }

        /// <summary>
        /// Gets or sets the MSDN id.
        /// </summary>
        /// <value>
        /// The MSDN id.
        /// </value>
        public string MsdnId { get; set; }

        /// <summary>
        /// Gets or sets the remarks extracted from the &lt;remarks&gt; tag of the <see cref="IComment.DocNode"/>.
        /// </summary>
        /// <value>The remarks.</value>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the topic link.
        /// </summary>
        /// <value>The topic link.</value>
        public NTopic TopicLink { get; set; }

        /// <summary>
        /// Gets or sets the see alsos.
        /// </summary>
        /// <value>
        /// The see alsos.
        /// </value>
        public List<NSeeAlso> SeeAlsos { get; set; }

        /// <summary>
        /// Gets or sets the group apis.
        /// </summary>
        /// <value>
        /// The group apis.
        /// </value>
        public List<string> Groups { get; private set; }

        /// <summary>
        /// Sets the API group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="state">if set to <c>true</c> [state].</param>
        public void SetApiGroup(string name, bool state)
        {
            if (state)
            {
                if (!Groups.Contains(name))
                {
                    Groups.Add(name);
                }
            }
            else
            {
                Groups.Remove(name);
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is obsolete.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is obsolete; otherwise, <c>false</c>.
        /// </value>
        public bool IsObsolete { get; set; }

        /// <summary>
        /// Gets or sets the obsolete message.
        /// </summary>
        /// <value>
        /// The obsolete message.
        /// </value>
        public string ObsoleteMessage { get; set; }

        /// <summary>
        /// Called when <see cref="DocNode"/> is updated.
        /// </summary>
        protected internal virtual void OnDocNodeUpdate()
        {
            if (DocNode != null)
            {
                Description = NDocumentApi.GetTag(DocNode, DocTag.Summary);
                Remarks = NDocumentApi.GetTag(DocNode, DocTag.Remarks);
                UnManagedApi = NDocumentApi.GetTag(DocNode, "unmanaged");
                UnManagedShortApi = NDocumentApi.GetTag(DocNode, "unmanaged-short");
                MsdnId = NDocumentApi.GetTag(DocNode, "msdn-id");
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(NModelBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (NModelBase)) return false;
            return Equals((NModelBase) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(NModelBase left, NModelBase right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(NModelBase left, NModelBase right)
        {
            return !Equals(left, right);
        }
    }
}