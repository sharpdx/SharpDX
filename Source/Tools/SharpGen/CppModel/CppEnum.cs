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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharpGen.CppModel
{
    /// <summary>
    /// A C++ enum.
    /// </summary>
    [XmlType("enum")]
    public class CppEnum : CppElement
    {
        /// <summary>
        /// Adds an enum item to this enum.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddEnumItem(string name, string value)
        {
            Add(new CppEnumItem(name, value));
        }

        /// <summary>
        /// Adds the None = 0 enum item.
        /// </summary>
        public void AddNone()
        {
            AddEnumItem("None", "0");
        }

        /// <summary>
        /// Gets the enum items.
        /// </summary>
        /// <value>The enum items.</value>
        [XmlIgnore]
        public IEnumerable<CppEnumItem> EnumItems
        {
            get { return Iterate<CppEnumItem>(); }
        }
    }
}