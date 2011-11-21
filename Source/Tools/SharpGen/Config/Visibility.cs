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
using System;
using System.Xml.Serialization;

namespace SharpGen.Config
{
    [Flags]
    public enum Visibility
    {        
        [XmlEnum("public")] 
        Public = 0x01,
        [XmlEnum("internal")]
        Internal = 0x02,
        [XmlEnum("protected")] 
        Protected = 0x04,
        [XmlEnum("public-protected")] 
        PublicProtected = 0x08,
        [XmlEnum("private")]
        Private = 0x10,
        [XmlEnum("override")]
        Override = 0x20,
        [XmlEnum("abstract")]
        Abstract = 0x40,
        [XmlEnum("partial")]
        Partial = 0x80,
        [XmlEnum("static")]
        Static = 0x100,
        [XmlEnum("const")]
        Const = 0x200,
        [XmlEnum("virtual")]
        Virtual = 0x400,
        [XmlEnum("readonly")]
        Readonly = 0x800,
        [XmlEnum("sealed")]
        Sealed = 0x1000,
    }
}