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
using System.Xml.Serialization;

namespace SharpDoc.Model
{
    /// <summary>
    /// Documentation topic store in an external file.
    /// </summary>
    public class ConfigSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSource"/> class.
        /// </summary>
        public ConfigSource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSource"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="mergeGroup">The merge group.</param>
        public ConfigSource(string location, string mergeGroup = null)
        {
            AssemblyPath = location;
            MergeGroup = mergeGroup;
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [XmlText()]
        public string AssemblyPath { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [XmlAttribute("xml")]
        public string DocumentationPath { get; set; }

        /// <summary>
        /// Gets or sets the merge group.
        /// </summary>
        /// <value>
        /// The merge group.
        /// </value>
        [XmlAttribute("api")]
        public string MergeGroup { get; set; }
    }
}