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

namespace SharpDoc
{
    /// <summary>
    /// A defined Style.
    /// </summary>
    [XmlRoot("style", Namespace = NS)]
    public class StyleDefinition
    {
        internal const string NS = "SharpDoc";

        public const string DefaultStyleFilename = "style.xml";

        public const string DefaultBootableTemplateName = "Main";
        public const string DefaultBootableTemplateFilename = DefaultBootableTemplateName + ".cshtml";

        /// <summary>
        /// Gets or sets the name of this style.
        /// </summary>
        /// <value>The name.</value>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the inherited style base if any.
        /// </summary>
        /// <value>The style base.</value>
        [XmlElement("inherit")]
        public string BaseStyle { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [XmlElement("param")]
        public List<ConfigParam> Parameters { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has base style.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has base style; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool HasBaseStyle { get { return !string.IsNullOrEmpty(BaseStyle); } }

        /// <summary>
        /// Gets or sets the directory path of this style
        /// </summary>
        /// <value>The path.</value>
        [XmlIgnore]
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the directory path of this style
        /// </summary>
        /// <value>The path.</value>
        [XmlIgnore]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this style is overriden.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this style is overriden; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsOverriden { get; set; }

        /// <summary>
        /// Gets a value indicating whether this particular style instance is runnable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is runnable; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsRunnable
        {
            get { return File.Exists(System.IO.Path.Combine(DirectoryPath, DefaultBootableTemplateFilename)); }
        }

        /// <summary>
        /// Loads the specified style file.
        /// </summary>
        /// <param name="file">The style file.</param>
        /// <returns></returns>
        public static StyleDefinition Load(string file)
        {
            var deserializer = new XmlSerializer(typeof(StyleDefinition));
            var style = (StyleDefinition)deserializer.Deserialize(new StringReader(File.ReadAllText(file)));
            style.DirectoryPath = System.IO.Path.GetDirectoryName(file);
            return style;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Name: {0}, Path: {1}{2}", Name, DirectoryPath, HasBaseStyle?", Base: " + BaseStyle:"");
        }
    }
}