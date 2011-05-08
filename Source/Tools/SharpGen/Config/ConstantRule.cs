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
using System.Xml.Serialization;

namespace SharpGen.Config
{
    /// <summary>
    /// Create constant associated to a C# class.
    /// Usage: const [from-macro="MACRO_REGEXP"] class="C#_CLASS_NAME" type="C#_TYPE_NAME" name="FIELD_NAME" value="FIELD_VALUE"
    /// </summary>
    [XmlType("const")]
    public class ConstantRule : ConfigBaseRule
    {
        [XmlAttribute("from-macro")]
        public string Macro { get; set; }

        [XmlAttribute("from-guid")]
        public string Guid { get; set; }

        [XmlAttribute("class")]
        public string ClassName { get; set; }

        [XmlAttribute("cpp-type")]
        public string CppType { get; set; }

        [XmlAttribute("cpp-cast")]
        public string CppCast { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// General visibility for Methods
        /// </summary>
        [XmlIgnore]
        public Visibility? Visibility { get; set; }
        [XmlAttribute("visibility")]
        public Visibility _Visibility_ { get { return Visibility.Value; } set { Visibility = value; } } public bool ShouldSerialize_Visibility_() { return Visibility != null; }

        [XmlText]
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} from-{1}:{2} class:{3} cpp-type:{4} cpp-cast:{5} type:{6} name:{7} value:{8}", base.ToString(), Macro!=null?"macro":"guid",  Macro ?? Guid, ClassName, CppType, CppCast, Type, Name, Value);
        }
    }
}