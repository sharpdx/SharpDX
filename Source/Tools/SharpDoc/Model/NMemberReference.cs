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
using System;
using System.Collections.Generic;

namespace SharpDoc.Model
{
    /// <summary>
    /// Member reference
    /// </summary>
    public class NMemberReference : NReference, INMemberReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NMemberReference"/> class.
        /// </summary>
        public NMemberReference()
        {
            GenericParameters = new List<NGenericParameter>();
            GenericArguments = new List<NTypeReference>();
        }

        /// <summary>
        /// Gets or sets the type that is declaring this member.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public NTypeReference DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an array.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is an array; otherwise, <c>false</c>.
        /// </value>
        public bool IsArray { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pointer.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is pointer; otherwise, <c>false</c>.
        /// </value>
        public bool IsPointer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sentinel.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is sentinel; otherwise, <c>false</c>.
        /// </value>
        public bool IsSentinel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generic instance.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is generic instance; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenericInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generic parameter.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is generic parameter; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenericParameter { get; set; }

        /// <summary>
        /// Gets or sets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public NTypeReference ElementType { get; set; }

        /// <summary>
        /// Gets or sets the generic parameters.
        /// </summary>
        /// <value>The generic parameters.</value>
        public List<NGenericParameter> GenericParameters { get; set; }

        /// <summary>
        /// Gets or sets the generic arguments.
        /// </summary>
        /// <value>The generic arguments.</value>
        public List<NTypeReference> GenericArguments {get;set;}

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(NMemberReference other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (NMemberReference)) return false;
            return Equals((NMemberReference) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", FullName);
        }
    }
}