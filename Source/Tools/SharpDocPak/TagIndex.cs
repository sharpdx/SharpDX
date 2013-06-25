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
using System.Xml.Serialization;

namespace SharpDocPak
{
    /// <summary>
    /// Tag index.
    /// </summary>
    [Serializable]
    public class TagIndex : IEquatable<TagIndex>
    {
        public const string TitleId = "title";

        public const string ContentId = "content";

        /// <summary>
        /// Gets the default index of the title.
        /// </summary>
        /// <value>The default index of the title.</value>
        public static TagIndex DefaultTitleIndex
        {
            get
            {
                return new TagIndex(TitleId, "//html/head/title", "Title");
            }
        }

        /// <summary>
        /// Gets the default index of the content.
        /// </summary>
        /// <value>The default index of the content.</value>
        public static TagIndex DefaultContentIndex
        {
            get
            {
                return new TagIndex(ContentId, "//html/body", "Content");
            }
        }

        /// <summary>
        /// Gets the default tags.
        /// </summary>
        /// <value>The default tags.</value>
        public static List<TagIndex> DefaultTags
        {
            get { return new List<TagIndex> {DefaultTitleIndex, DefaultContentIndex}; }
        }

        /// <summary>
        /// Adds the tag index to the tag list.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <param name="tagIndex">Index of the tag.</param>
        public static void AddTagIndex(List<TagIndex> tags, TagIndex tagIndex)
        {
            tags.Remove(tagIndex);
            tags.Add(tagIndex);
        }

        /// <summary>
        /// Parses the tag index and add it to tag list.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <param name="tagId">The tag id.</param>
        /// <param name="xpathAndName">The xpath with an optional name separated by a semi-colon ';'.</param>
        public static void ParseAndAddTagIndex(List<TagIndex> tags, string tagId, string xpathAndName)
        {
            var tagIndex = new TagIndex() {Id = tagId};

            var separatorIndex = xpathAndName.IndexOf(';');
            if (separatorIndex > 0)
            {
                tagIndex.TagPath = xpathAndName.Substring(0, separatorIndex);
                tagIndex.Name = xpathAndName.Substring(separatorIndex + 1, xpathAndName.Length - separatorIndex - 1);
            } else
            {
                tagIndex.TagPath = xpathAndName;
                tagIndex.Name = "" + char.ToUpper(tagId[0]) + tagId.Substring(1);
            }

            AddTagIndex(tags, tagIndex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagIndex"/> class.
        /// </summary>
        public TagIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagIndex"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="tagPath">The tag xpath.</param>
        /// <param name="name">The name.</param>
        public TagIndex(string id, string tagPath, string name)
        {
            Id = id;
            TagPath = tagPath;
            Name = name;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the tag xpath.
        /// </summary>
        /// <value>The tag xpath.</value>
        [XmlAttribute("xpath")]
        public string TagPath { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TagIndex other)
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
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TagIndex)) return false;
            return Equals((TagIndex) obj);
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
        public static bool operator ==(TagIndex left, TagIndex right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TagIndex left, TagIndex right)
        {
            return !Equals(left, right);
        }
    }
}