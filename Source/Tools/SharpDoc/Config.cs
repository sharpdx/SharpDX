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
using System.IO;
using System.Xml.Serialization;
using SharpDoc.Model;

namespace SharpDoc
{
    /// <summary>
    /// Config file for SharpDoc.
    /// </summary>
    [XmlRoot("config", Namespace = NS)]
    public class Config
    {
        internal const string NS = "SharpDoc";

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public Config()
        {
            StyleName = "Standard";
            Topics = new List<NTopic>();
            Sources = new List<string>();
            References = new List<string>();
            Parameters = new List<ConfigParam>();
            StyleParameters = new List<ConfigParam>();
        }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        /// <value>The topic.</value>
        [XmlArray("topics")]
        public List<NTopic> Topics { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        [XmlElement("output")]
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a list of source file (assembly or xml comment file).
        /// </summary>
        /// <value>The sources file.</value>
        [XmlElement("source")]
        public List<string> Sources { get; set; }

        /// <summary>
        /// Gets or sets a list assembly references.
        /// </summary>
        /// <value>The references.</value>
        [XmlElement("reference")]
        public List<string> References { get; set; }

        /// <summary>
        /// Gets or sets parameters.
        /// </summary>
        /// <value>The parameters.</value>
        [XmlElement("param")]
        public List<ConfigParam> Parameters { get; set; }

        /// <summary>
        /// Gets or sets styles override.
        /// </summary>
        /// <value>The styles override.</value>
        [XmlElement("param-style")]
        public List<ConfigParam> StyleParameters { get; set; }

        /// <summary>
        /// Gets or sets the name of the style.
        /// </summary>
        /// <value>The name of the style.</value>
        [XmlElement("style")]
        public string StyleName { get; set; }

        /// <summary>
        /// Loads the specified config file.
        /// </summary>
        /// <param name="file">The config file.</param>
        /// <returns></returns>
        public static Config Load(string file)
        {
            var deserializer = new XmlSerializer(typeof(Config));
            return (Config)deserializer.Deserialize(new StringReader(File.ReadAllText(file)));            
        }

        /// <summary>
        /// Loads the specified config file.
        /// </summary>
        /// <param name="file">The config file.</param>
        /// <returns></returns>
        public void Save(string file)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", NS);
            var deserializer = new XmlSerializer(typeof(Config));
            var output = new FileStream(file, FileMode.Create);
            deserializer.Serialize(output, this, ns);
        }
    }
}