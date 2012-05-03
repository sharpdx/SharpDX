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
using System.Linq;
using System.Text;

namespace SharpDoc.Model
{
    /// <summary>
    /// An abstract member of a <see cref="NType"/>.
    /// </summary>
    public abstract class NMember : NModelBase, INMemberReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NMember"/> class.
        /// </summary>
        protected NMember()
        {
            Members = new List<NMember>();
            Attributes = new List<string>();
            GenericParameters = new List<NGenericParameter>();
            GenericArguments = new List<NTypeReference>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an array.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is an array; otherwise, <c>false</c>.
        /// </value>
        public bool IsArray {get;set;}

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
        /// Gets or sets the type that is declaring this member.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public NTypeReference DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is static.
        /// </summary>
        /// <value><c>true</c> if this instance is static; otherwise, <c>false</c>.</value>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is final.
        /// </summary>
        /// <value><c>true</c> if this instance is final; otherwise, <c>false</c>.</value>
        public bool IsFinal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is abstract.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is abstract; otherwise, <c>false</c>.
        /// </value>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        /// <value>The visibility.</value>
        public NVisibility Visibility { get; set; }

        /// <summary>
        /// Gets the name of the visibility.
        /// </summary>
        /// <value>The name of the visibility.</value>
        public string VisibilityName
        {
            get
            {
                switch (Visibility)
                {
                    case NVisibility.Public:
                        return "public";
                    case NVisibility.Protected:
                        return "protected";
                    case NVisibility.Private:
                        return "private";
                    case NVisibility.ProtectedInternal:
                        return "protected internal";
                    case NVisibility.Internal:
                        return "internal";
                }
                return "";
            }
        }

        /// <summary>
        /// Gets the name of the visibility.
        /// </summary>
        /// <value>
        /// The name of the visibility.
        /// </value>
        public string VisibilityFullName
        {
            get
            {
                var text = new StringBuilder(VisibilityName);
                if (IsStatic) text.Append(" static");
                if (IsAbstract) text.Append(" abstract");
                if (IsFinal) text.Append(" sealed");
                return text.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the namespace this member is attached.
        /// </summary>
        /// <value>The namespace.</value>
        public NNamespace Namespace { get; set; }

        /// <summary>
        /// Gets or sets the type of the member.
        /// </summary>
        /// <value>The type of the member.</value>
        public NMemberType MemberType { get; set; }

        /// <summary>
        /// Gets or sets the parent. This is null for root type.
        /// </summary>
        /// <value>The parent.</value>
        public NMember Parent { get; set; }

        /// <summary>
        /// Gets or sets all the members.
        /// </summary>
        /// <value>The members.</value>
        public List<NMember> Members { get; private set; }

        /// <summary>
        /// Gets or sets the attributes declaration (as string).
        /// </summary>
        /// <value>The attributes.</value>
        public List<string> Attributes { get; private set; }

        /// <summary>
        /// Gets the type member.
        /// </summary>
        /// <value>The type member.</value>
        public virtual string TypeMember
        {
            get { return GetType().Name.Substring(1); }
        }

        /// <summary>
        /// Helper method to return a particular collection of members.
        /// </summary>
        /// <typeparam name="T">A member</typeparam>
        /// <returns>A collection ot <paramref name="T"/></returns>
        protected IEnumerable<T> MembersAs<T>() where T : NMember
        {
            return Members.OfType<T>();
        }

        /// <summary>
        /// Adds a member.
        /// </summary>
        /// <param name="member">The member.</param>
        public void AddMember(NMember member)
        {
            member.Parent = this;
            Members.Add(member);
        }

        public override string ToString()
        {
            return string.Format("{0}", FullName);
        }
    }
}