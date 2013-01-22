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

using System.Xml.Serialization;
using SharpGen.CppModel;

namespace SharpGen.Config
{
    interface ITypeRule
    {
        [XmlAttribute("class")]
        string NewClass { get; set; }
        [XmlAttribute("enum")]
        string Enum { get; set; }
        [XmlAttribute("enum-item")]
        string EnumItem { get; set; }
        [XmlAttribute("struct")]
        string Struct { get; set; }
        [XmlAttribute("field")]
        string Field { get; set; }
        [XmlAttribute("interface")]
        string Interface { get; set; }
        [XmlAttribute("function")]
        string Function { get; set; }
        [XmlAttribute("method")]
        string Method { get; set; }
        [XmlAttribute("param")]
        string Parameter { get; set; }
        [XmlAttribute("typedef")]
        string Typedef { get; set; }
        [XmlAttribute("element")]
        string Element { get; set; }
        [XmlAttribute("variable")]
        string Variable { get; set; }
    }
}