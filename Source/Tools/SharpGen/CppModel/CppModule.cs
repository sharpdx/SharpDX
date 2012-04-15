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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SharpGen.CppModel
{
    /// <summary>
    /// A C++ module contains includes.
    /// </summary>
    [XmlRoot("cpp-module", Namespace= NS)]
    public class CppModule : CppElement
    {
        internal const string NS = "urn:SharpGen.CppModel";

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        [XmlIgnore]
        public override string FullName
        {
            get { return ""; }
        }

        /// <summary>
        /// Gets the includes.
        /// </summary>
        /// <value>The includes.</value>
        [XmlIgnore]
        public IEnumerable<CppInclude> Includes
        {
            get { return Iterate<CppInclude>(); }
        }

        /// <summary>
        /// Finds the include.
        /// </summary>
        /// <param name="includeName">Name of the include.</param>
        /// <returns></returns>
        public CppInclude FindInclude(string includeName)
        {
            return (from cppElement in Iterate<CppInclude>()
                    where cppElement.Name == includeName
                    select cppElement).FirstOrDefault();
        }

        /// <summary>
        /// Reads the module from the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>A C++ module</returns>
        public static CppModule Read(string file)
        {
            var input = new FileStream(file, FileMode.Open);
            var result = Read(input);
            input.Close();
            return result;
        }

        /// <summary>
        /// Reads the module from the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A C++ module</returns>
        public static CppModule Read(Stream input)
        {
            var ds = new XmlSerializer(typeof (CppModule));

            CppModule module = null;
            using (XmlReader w = XmlReader.Create(input))
            {
                module = ds.Deserialize(w) as CppModule;
            }
            if (module != null)
                module.ResetParents();
            return module;
        }

        /// <summary>
        /// Writes this instance to the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Write(string file)
        {
            var output = new FileStream(file, FileMode.Create);
            Write(output);
            output.Close();
        }

        /// <summary>
        /// Writes this instance to the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        public void Write(Stream output)
        {
            var ds = new XmlSerializer(typeof (CppModule));

            var settings = new XmlWriterSettings() {Indent = true};
            using (XmlWriter w = XmlWriter.Create(output, settings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", NS);
                ds.Serialize(w, this, ns);
            }
        }    
    }
}