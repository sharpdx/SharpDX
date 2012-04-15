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
using System.Linq;
using System.Xml.Serialization;
using SharpGen.Config;

namespace SharpGen.Model
{
    [XmlType("enum")]
    public class CsEnum : CsTypeBase
    {
        public CsEnum()
        {
            Type = typeof (int);
            SizeOf = 4;
        }

        public bool IsFlag { get; set; }

        public string TypeName
        {
            get
            {
                if ( Type == typeof(int) )
                    return "int";
                if (Type == typeof(short))
                    return "short";
                if (Type == typeof(byte))
                    return "byte";
                return "UNKNOWN";
            }
        }

        public IEnumerable<CsEnumItem> EnumItems
        {
            get { return Items.OfType<CsEnumItem>(); }
        }

        protected override void UpdateFromTag(MappingRule tag)
        {
            base.UpdateFromTag(tag);
            if (tag.EnumHasFlags.HasValue)
                IsFlag = tag.EnumHasFlags.Value;
        }
    }
}