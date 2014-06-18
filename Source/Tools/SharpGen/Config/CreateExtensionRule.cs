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
using SharpGen.Model;

namespace SharpGen.Config
{
    [XmlType("create")]
    public class CreateExtensionRule : ExtensionBaseRule
    {
        [XmlIgnore]
        public Visibility? Visibility { get; set; }
        [XmlAttribute("visibility")]
        public Visibility _Visibility_ { get { return Visibility.Value; } set { Visibility = value; } } public bool ShouldSerialize_Visibility_() { return Visibility != null; }


        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} visibility:{1}", base.ToString(), Visibility.HasValue ? Visibility.Value.ToString() : "undef");
        }
    }

    [XmlType("create-cpp")]
    public class CreateCppExtensionRule : CreateExtensionRule
    {
        [XmlAttribute("macro")]
        public string Macro { get; set; }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} macro:{1}", base.ToString(), Macro);
        }
    }
}