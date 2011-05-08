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
using System.Collections.Generic;

namespace SharpDoc.Model
{
    /// <summary>
    /// A member reference.
    /// </summary>
    public interface INMemberReference : IModelReference
    {
        /// <summary>
        /// Gets or sets the type that is declaring this member.
        /// </summary>
        /// <value>The type of the declaring.</value>
        NTypeReference DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an array.
        /// </summary>
        /// <value><c>true</c> if this instance is an array; otherwise, <c>false</c>.</value>
        bool IsArray { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pointer.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is pointer; otherwise, <c>false</c>.
        /// </value>
        bool IsPointer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sentinel.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is sentinel; otherwise, <c>false</c>.
        /// </value>
        bool IsSentinel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generic instance.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is generic instance; otherwise, <c>false</c>.
        /// </value>
        bool IsGenericInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generic parameter.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is generic parameter; otherwise, <c>false</c>.
        /// </value>
        bool IsGenericParameter { get; set; }

        /// <summary>
        /// Gets or sets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        NTypeReference ElementType { get; set; }

        /// <summary>
        /// Gets or sets the generic parameters.
        /// </summary>
        /// <value>The generic parameters.</value>
        List<NGenericParameter> GenericParameters { get; set; }

        /// <summary>
        /// Gets or sets the generic arguments.
        /// </summary>
        /// <value>The generic arguments.</value>
        List<NTypeReference> GenericArguments { get; set; }
    }
}