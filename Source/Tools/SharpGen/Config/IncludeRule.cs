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

using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SharpGen.Config
{
    /// <summary>
    /// An Include directive
    /// </summary>
    public class IncludeRule
    {
        public IncludeRule()
        {
            AttachTypes = new List<string>();
            FilterErrors = new List<string>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [XmlIgnore]
        public string Id { get { return Path.GetFileNameWithoutExtension(File).ToLower(); } }

        /// <summary>
        /// Gets or sets the file to be included.
        /// </summary>
        /// <value>The file.</value>
        [XmlAttribute("file")]
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        [XmlAttribute("namespace")]
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>The output.</value>
        [XmlAttribute("output")]
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the file must be attached for mapping to the current Namespace/Assembly
        /// </summary>
        /// <value>The attach.</value>
        [XmlIgnore]
        public bool? Attach { get; set; }

        /// <summary>
        /// Internal method to serialize nullable <c cref="Attach"/> property.
        /// </summary>
        /// <value><c>true</c> if [_ attach_]; otherwise, <c>false</c>.</value>
        [XmlAttribute("attach")]
        public bool _Attach_
        {
            get { return Attach.Value; }
            set { Attach = value; }
        }

        /// <summary>
        /// Should Attach be serialized?
        /// </summary>
        /// <returns><c>true</c> if Attach property has a value; otherwise, <c>false</c></returns>
        public bool ShouldSerialize_Attach_()
        {
            return Attach != null;
        }

        /// <summary>
        /// Gets or sets the pre-include-code to insert before performing the include.
        /// </summary>
        /// <value>The pre.</value>
        [XmlElement("pre")]
        public string Pre { get; set; }

        /// <summary>
        /// Gets or sets the post-include-code to insert after performing the include.
        /// </summary>
        /// <value>The post.</value>
        [XmlElement("post")]
        public string Post{ get; set; }

        /// <summary>
        /// Gets or sets single types to attach.
        /// </summary>
        /// <value>The post.</value>
        [XmlElement("attach")]
        public List<string> AttachTypes{ get; set; }

        /// <summary>
        /// Gets or sets the ignore errors.
        /// </summary>
        /// <value>The ignore errors.</value>
        [XmlElement("filter-error")]
        public List<string> FilterErrors { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "include: {0}", File);
        }
    }
}