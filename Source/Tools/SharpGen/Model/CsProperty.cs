// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Linq;

namespace SharpGen.Model
{
    public class CsProperty : CsMarshalBase
    {
        public CsProperty(string name)
        {
            Name = name;
        }

        public CsMethod Getter { get; set; }

        public CsMethod Setter { get; set; }

        public string CppSignature
        {
            get
            {
                if (Getter != null) return Getter.CppSignature;
                return Setter.CppSignature;
            }
        }

        public bool IsPropertyParam { get; set; }

        public bool IsPersistent { get; set; }

        public override string DocUnmanagedName
        {
            get
            {
                if (Setter != null && Getter != null)
                    return string.Format("{0} / {1}", Getter.CppElement.Name, Setter.CppElement.Name);
                return base.DocUnmanagedName;
            }
        }

        public string PrefixSetterParam
        {
            get
            {
                if (Setter != null)
                {
                    var parameter = Setter.Parameters.First();
                    if (parameter.ParamName.StartsWith("ref"))
                    {
                        return "ref ";
                    }
                }
                return "";
            }
        }
    }
}