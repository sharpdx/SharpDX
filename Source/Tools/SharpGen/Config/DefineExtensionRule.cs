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

namespace SharpGen.Config
{
    [XmlType("define")]
    public class DefineExtensionRule : ExtensionBaseRule
    {
        public DefineExtensionRule()
        {
        }

        public DefineExtensionRule(int sizeOf)
        {
            SizeOf = sizeOf;
        }

        public DefineExtensionRule(int sizeOf, int align)
        {
            SizeOf = sizeOf;
            Align = align;
        }


        [XmlIgnore]
        public int? SizeOf { get; set; }
        [XmlAttribute("sizeof")]
        public int _SizeOf_ { get { return SizeOf.Value; } set { SizeOf = value; } } public bool ShouldSerialize_SizeOf_() { return SizeOf != null; }
            
        [XmlIgnore]
        public int? Align { get; set; }
        [XmlAttribute("align")]
        public int _Align_ { get { return Align.Value; } set { Align = value; } } public bool ShouldSerialize_Align_() { return Align != null; }

        [XmlIgnore]
        public bool? HasCustomMarshal { get; set; }
        [XmlAttribute("marshal")]
        public bool _HasCustomMarshal_ { get { return HasCustomMarshal.Value; } set { HasCustomMarshal = value; } } public bool ShouldSerialize_HasCustomMarshal_() { return HasCustomMarshal != null; }

        [XmlIgnore]
        public bool? IsStaticMarshal { get; set; }
        [XmlAttribute("static-marshal")]
        public bool _IsStaticMarshal_ { get { return IsStaticMarshal.Value; } set { IsStaticMarshal = value; } } public bool ShouldSerialize_IsStaticMarshal_() { return IsStaticMarshal != null; }

        [XmlIgnore]
        public bool? HasCustomNew { get; set; }
        [XmlAttribute("custom-new")]
        public bool _HasCustomNew_ { get { return HasCustomNew.Value; } set { HasCustomNew = value; } } public bool ShouldSerialize_HasCustomNew_() { return HasCustomNew != null; }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} {1} {2}", base.ToString(), SizeOf.HasValue ? "sizeof:" + SizeOf.Value : "", Align.HasValue ? "align:" + Align.Value : "");
        }
    }
}