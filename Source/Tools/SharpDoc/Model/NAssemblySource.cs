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
using System.Xml;

namespace SharpDoc.Model
{
    /// <summary>
    /// This class contains an abstract assembly (either from Mono or Reflection)
    /// and the associated XmlDocument generated from code comments.
    /// </summary>
    public class NAssemblySource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NAssemblySource"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public NAssemblySource(object assembly)
        {
            Assembly = assembly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NAssemblySource"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="document">The document.</param>
        public NAssemblySource(object assembly, XmlDocument document)
        {
            Assembly = assembly;
            Document = document;
        }

        /// <summary>
        /// Gets or sets the assembly definition. This is interpeted by specialization of
        /// <see cref="IModelBuilder"/>.
        /// </summary>
        /// <value>The assembly.</value>
        public object Assembly { get; set; }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
        public string Filename {get; set; }

        /// <summary>
        /// Gets or sets the XML document that contains code comments.
        /// </summary>
        /// <value>The document.</value>
        public XmlDocument Document { get; set; }

        /// <summary>
        /// Finds a member doc from the <see cref="Document"/>.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <returns>A node of the member if found or otherwise, null.</returns>
        public XmlNode FindMemberDoc(string memberId)
        {
            var node = Document.SelectSingleNode("/doc/members/member[@name='" + memberId + "']");
            if (node == null)
                return null;
            return node;
        }
    }


}