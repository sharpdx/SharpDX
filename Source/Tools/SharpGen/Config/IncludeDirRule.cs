// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpGen.Config
{
    /// <summary>
    /// An Include directive
    /// </summary>
    public class IncludeDirRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncludeDirRule"/> class.
        /// </summary>
        public IncludeDirRule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncludeDirRule"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public IncludeDirRule(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [XmlText]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the is override.
        /// </summary>
        /// <value>
        /// The is override.
        /// </value>
        [XmlAttribute("override")]
        public bool IsOverride { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("include-dir: {0} override: {1}", Path, IsOverride);
        }
    }
}