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

namespace SharpGen.Config
{
    public abstract class MappingBaseRule : ConfigBaseRule, ITypeRule
    {
        [XmlAttribute("class")]
        public string NewClass { get; set; }
        [XmlAttribute("enum")]
        public string Enum { get; set; }
        [XmlAttribute("enum-item")]
        public string EnumItem { get; set; }
        [XmlAttribute("struct")]
        public string Struct { get; set; }
        [XmlAttribute("field")]
        public string Field { get; set; }
        [XmlAttribute("interface")]
        public string Interface { get; set; }
        [XmlAttribute("function")]
        public string Function { get; set; }
        [XmlAttribute("method")]
        public string Method { get; set; }
        [XmlAttribute("param")]
        public string Parameter { get; set; }
        [XmlAttribute("typedef")]
        public string Typedef { get; set; }
        [XmlAttribute("element")]
        public string Element { get; set; }
        [XmlAttribute("variable")]
        public string Variable { get; set; }
        [XmlAttribute("doc")]
        public string DocItem { get; set; }

        public override string ToString()
        {
            string type = "";

            foreach(var property in typeof(MappingBaseRule).GetProperties())
            {
                if (property.GetValue(this, null) != null)
                {
                    type = ((XmlAttributeAttribute) (property.GetCustomAttributes(typeof (XmlAttributeAttribute), false)[0])).AttributeName + ":" +
                           property.GetValue(this, null);
                    break;

                }                
            }
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} {1}", base.ToString(), type);
        }
    }
}