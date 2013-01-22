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
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace SharpGen.Config
{
    /// <summary>
    /// A simple Xml preprocessor.
    /// </summary>
    internal class Preprocessor
    {
        /// <summary>
        /// Preprocesses the specified XML text.
        /// </summary>
        /// <param name="xmlText">The XML text.</param>
        /// <param name="macros">The macros.</param>
        /// <returns>A preprocessed xml text</returns>
        public static string Preprocess(string xmlText, string[] macros)
        {
            var doc = XDocument.Load(new StringReader(xmlText));

            XNamespace ns = ConfigFile.NS;

            var list = doc.Descendants(ns + "ifndef").ToList();
            // Work on deepest first
            list.Reverse();
            foreach (var ifndef in list)
            {
                var attr = ifndef.Attribute("name");
                if (attr != null && macros.Contains(attr.Value))
                {
                    ifndef.Remove();
                }
                else
                {
                    foreach (var element in ifndef.Elements())
                    {
                        //Console.WriteLine("=============== ifndef Add element {0}", element);
                        ifndef.AddBeforeSelf(element);
                    }

                    ifndef.Remove();
                }
            }

            list = doc.Descendants(ns + "ifdef").ToList();
            // Work on deepest first
            list.Reverse();
            foreach (var ifdef in list)
            {
                var attr = ifdef.Attribute("name");
                if (attr == null || !macros.Contains(attr.Value))
                {
                    ifdef.Remove();
                } 
                else
                {
                    foreach(var element in ifdef.Elements())
                    {
                        //Console.WriteLine("=============== ifdef Add element {0}", element);
                        ifdef.AddBeforeSelf(element);
                    }

                    ifdef.Remove();
                }
            }

            var writer = new StringWriter();
            doc.Save(writer);

            return writer.ToString();
        }
    }
}