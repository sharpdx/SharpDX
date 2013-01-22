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
using System;
using System.Collections.Generic;

namespace SharpDoc.Model
{
    /// <summary>
    /// A Generic Parameter description.
    /// </summary>
    public class NGenericParameter : NTypeReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NGenericParameter"/> class.
        /// </summary>
        public NGenericParameter()
        {
            Constraints = new List<NTypeReference>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has constraints.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has constraints; otherwise, <c>false</c>.
        /// </value>
        public bool HasConstraints { get; set; }


        /// <summary>
        /// Gets or sets the constraints.
        /// </summary>
        /// <value>The constraints.</value>
        public List<NTypeReference> Constraints { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has custom attributes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has custom attributes; otherwise, <c>false</c>.
        /// </value>
        public bool HasCustomAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has default constructor constraint.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has default constructor constraint; otherwise, <c>false</c>.
        /// </value>
        public bool HasDefaultConstructorConstraint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has not nullable value type constraint.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has not nullable value type constraint; otherwise, <c>false</c>.
        /// </value>
        public bool HasNotNullableValueTypeConstraint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has reference type constraint.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has reference type constraint; otherwise, <c>false</c>.
        /// </value>
        public bool HasReferenceTypeConstraint { get; set; }
    }
}