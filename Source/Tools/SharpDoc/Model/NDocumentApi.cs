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
using System.IO;
using System.Xml;

namespace SharpDoc.Model
{
    /// <summary>
    /// Front end to a XML generated documentation, used to find documentation about a type reference.
    /// </summary>
    public class NDocumentApi
    {
        private Dictionary<string,XmlNode> cacheNodes = new Dictionary<string, XmlNode>();

        /// <summary>
        /// Gets or sets the XML document that contains code comments.
        /// </summary>
        /// <value>The document.</value>
        public XmlDocument Document { get; set; }

        /// <summary>
        /// Loads the xml documentation from the specified path..
        /// </summary>
        /// <param name="path">The path to a xml doc file.</param>
        /// <returns>A NDocumentApi or null if failed to read</returns>
        public static NDocumentApi Load(string path)
        {
            NDocumentApi doc = null;

            if (File.Exists(path))
            {
                try
                {

                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(path);
                    doc = new NDocumentApi { Document = xmlDoc };
                    doc.Initialize();
                }
                catch (Exception ex)
                {
                }
            }
            return doc;
        }
      
        /// <summary>
        /// Finds a member doc from the <see cref="Document"/>.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <returns>A node of the member if found or otherwise, null.</returns>
        public XmlNode FindMemberDoc(string memberId)
        {
            XmlNode node;
            cacheNodes.TryGetValue(memberId, out node);
            return node;
        }

        /// <summary>
        /// Extract a comment from tag inside the <see cref="NModelBase.DocNode"/> associated
        /// to this element.
        /// </summary>
        /// <param name="docNode">The doc node.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns>
        /// The content of the tag or null if empty or not found
        /// </returns>
        public static string GetTag(XmlNode docNode, string tagName)
        {
            if (docNode != null)
            {
                var selectedNode = docNode.SelectSingleNode(tagName);
                if (selectedNode != null)
                    return selectedNode.InnerXml.Trim();
            }

            return null;
        }

        private void Initialize()
        {
            var items = this.Document.SelectNodes("/doc/members/member");
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var node = items[i];
                    if (node.Attributes != null)
                    {
                        var nameAttr = node.Attributes["name"];
                        if (nameAttr != null)
                            cacheNodes.Add(nameAttr.Value, node);
                    }
                }
            }
        }
    }
}