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
using System.Text;
using System.Xml.Serialization;

namespace SharpGen.CppModel
{
    /// <summary>
    /// Type declaration.
    /// </summary>
    [XmlType("type")]
    public class CppType : CppElement
    {
        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        [XmlAttribute("typename")]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the pointer.
        /// </summary>
        /// <value>The pointer.</value>
        [XmlAttribute("ptr")]
        public string Pointer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CppType"/> is const.
        /// </summary>
        /// <value><c>true</c> if const; otherwise, <c>false</c>.</value>
        [XmlAttribute("const")]
        public bool Const { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is array.
        /// </summary>
        /// <value><c>true</c> if this instance is array; otherwise, <c>false</c>.</value>
        [XmlAttribute("array")]
        public bool IsArray { get; set; }

        /// <summary>
        /// Gets or sets the array dimension.
        /// </summary>
        /// <value>The array dimension.</value>
        [XmlAttribute("array-dim")]
        public string ArrayDimension { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Const)
                builder.Append("const ");
            builder.Append(TypeName);
            builder.Append(Pointer);

            if (!string.IsNullOrEmpty(Name))
            {
                builder.Append(" ");
                builder.Append(Name);
            }

            if (IsArray)
            {
                builder.Append("[");
                builder.Append(ArrayDimension);
                builder.Append("]");
            }
            return builder.ToString();
        }

        public bool Equals(CppType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.TypeName, TypeName) && Equals(other.Pointer, Pointer);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TypeName != null ? TypeName.GetHashCode() : 0)*397) ^ (Pointer != null ? Pointer.GetHashCode() : 0);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (CppType)) return false;
            return Equals((CppType) obj);
        }
    }
}