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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharpDoc.Model
{
    /// <summary>
    /// Documentation topic store in an external file.
    /// </summary>
    public class NTopic : IModelReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NTopic"/> class.
        /// </summary>
        public NTopic()
        {
            SubTopics = new List<NTopic>();
        }

        /// <summary>
        /// Gets or sets the XML generated commment ID.
        /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x.aspx for more information.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the normalized id. This is a normalized version of the <see cref="IModelReference.Id"/> that
        /// can be used for filename.
        /// </summary>
        /// <value>The file id.</value>
        [XmlIgnore]
        public string NormalizedId { get; set; }

        /// <summary>
        /// Gets or sets the name of this instance.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full name of this instance.
        /// </summary>
        /// <value>The full name.</value>
        [XmlAttribute("fullname")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file that contains the documentation.
        /// </summary>
        /// <value>The name of the file.</value>
        [XmlAttribute("filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the html content. This is loaded from the filename.
        /// </summary>
        /// <value>The content.</value>
        [XmlIgnore]
        public string Content { get; set;  }

        /// <summary>
        /// Gets or sets the parent topic.
        /// </summary>
        /// <value>The parent topic.</value>
        [XmlIgnore]
        public NTopic Parent { get; set; }

        /// <summary>
        /// Gets or sets the sub topics.
        /// </summary>
        /// <value>The sub topics.</value>
        [XmlElement("topic")]
        public List<NTopic> SubTopics { get; set; }

        /// <summary>
        /// Id for the default class library topic
        /// </summary>
        public const string DefaultClassLibraryTopicId = "X:ClassLibrary";

        /// <summary>
        /// Gets the default class library topic.
        /// </summary>
        /// <value>The default class library topic.</value>
        public static NTopic DefaultClassLibraryTopic
        {
            get
            {
                return new NTopic()
                           {
                               Id = DefaultClassLibraryTopicId,
                               NormalizedId = "ClassLibrary",
                               Name = "Class Library"
                           };
            }
        } 
    }
}