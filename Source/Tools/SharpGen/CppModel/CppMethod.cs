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
using System.Text;
using System.Xml.Serialization;

namespace SharpGen.CppModel
{
    /// <summary>
    /// A C++ method.
    /// </summary>
    [XmlType("method")]
    public class CppMethod : CppElement
    {
        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>The type of the return.</value>
        [XmlElement("return")]
        public CppType ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the calling convention.
        /// </summary>
        /// <value>The calling convention.</value>
        [XmlAttribute("call-conv")]
        public CppCallingConvention CallingConvention { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        [XmlAttribute("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        [XmlIgnore]
        public IEnumerable<CppParameter> Parameters
        {
            get { return Iterate<CppParameter>(); }
        }

        protected override IEnumerable<CppElement> AllItems
        {
            get
            {
                var allElements = new List<CppElement>(Iterate<CppElement>());
                allElements.Add(ReturnType);
                return allElements;
            }            
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(ReturnType);
            builder.Append(" ");
            if (Parent is CppInterface)
            {
                builder.Append(Parent.Name);
                builder.Append("::");
            }
            builder.Append(Name);
            builder.Append("(");
            int i = 0, count = Parameters.Count();
            foreach (var cppParameter in Parameters)
            {
                builder.Append(cppParameter);
                if ((i + 1) < count)
                {
                    builder.Append(",");
                }
                i++;
            }
            builder.Append(")");
            return builder.ToString();
        }
    }
}