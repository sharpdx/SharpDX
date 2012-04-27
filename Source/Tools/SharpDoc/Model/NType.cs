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

namespace SharpDoc.Model
{
    /// <summary>
    /// The base class for class or structure.
    /// </summary>
    public abstract class NType : NMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NType"/> class.
        /// </summary>
        protected NType()
        {
            Bases = new List<INMemberReference>();
            Descendants = new List<INMemberReference>();
            FlattenedHierarchy = new List<Tuple<int, INMemberReference>>();
            Interfaces = new List<NMemberReference>();
            AllMembers = new List<INMemberReference>();
        }

        /// <summary>
        /// Gets or sets all members (members of this instance + inherited members).
        /// </summary>
        /// <value>
        /// All members.
        /// </value>
        public List<INMemberReference> AllMembers { get; set; }

        /// <summary>
        /// Gets or sets all base types for this type.
        /// </summary>
        /// <value>All the base types.</value>
        public List<INMemberReference> Bases { get; set; }

        /// <summary>
        /// Gets or sets the descendants.
        /// </summary>
        /// <value>The descendants.</value>
        public List<INMemberReference> Descendants { get; set; }

        /// <summary>
        /// Gets or sets the flattened hierarchy type, including all base types and all derivated types.
        /// The T1 type as <see cref="System.Int32"/> returned by the <see cref="Tuple{T1,T2}"/> describes
        /// the level (0 for the root level) in the inheritance hierarchy.
        /// Objects are return from the most 
        /// </summary>
        /// <value>The flattened hierarchy.</value>
        public List<Tuple<int, INMemberReference>> FlattenedHierarchy { get; set; }

        /// <summary>
        /// Gets or sets the inherited interfaces.
        /// </summary>
        /// <value>The interfaces.</value>
        public List<NMemberReference> Interfaces { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has constructors.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has constructors; otherwise, <c>false</c>.
        /// </value>
        public bool HasConstructors { get; set;}

        /// <summary>
        /// Gets the constructors.
        /// </summary>
        /// <value>The constructors.</value>
        public IEnumerable<NConstructor> Constructors
        {
            get { return MembersAs<NConstructor>(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has methods.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has methods; otherwise, <c>false</c>.
        /// </value>
        public bool HasMethods { get; set; }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <value>The methods.</value>
        public IEnumerable<NMethod> Methods
        {
            get { return Members.Where(member => member.GetType() == typeof(NMethod)).Cast<NMethod>().ToList(); }
        }

        /// <summary>
        /// Gets the all methods of this instance and inherited).
        /// </summary>
        /// <value>The methods.</value>
        public IEnumerable<NMethod> AllMethods
        {
            get { return AllMembers.Where(member => member.GetType() == typeof(NMethod)).Cast<NMethod>().ToList(); }
        }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <value>The methods.</value>
        public IEnumerable<NMethod> MethodsAndConstructors
        {
            get { return MembersAs<NMethod>(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has operators.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has operators; otherwise, <c>false</c>.
        /// </value>
        public bool HasOperators { get; set; }

        /// <summary>
        /// Gets the operators.
        /// </summary>
        /// <value>The operators.</value>
        public IEnumerable<NOperator> Operators
        {
            get { return MembersAs<NOperator>(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has fields.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has fields; otherwise, <c>false</c>.
        /// </value>
        public bool HasFields { get; set; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>The fields.</value>
        public IEnumerable<NField> Fields
        {
            get { return MembersAs<NField>(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has properties.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has properties; otherwise, <c>false</c>.
        /// </value>
        public bool HasProperties { get; set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IEnumerable<NProperty> Properties
        {
            get { return MembersAs<NProperty>(); }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IEnumerable<NProperty> AllProperties
        {
            get { return AllMembers.OfType<NProperty>(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has events.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has events; otherwise, <c>false</c>.
        /// </value>
        public bool HasEvents { get; set; }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        public IEnumerable<NEvent> Events
        {
            get { return MembersAs<NEvent>(); }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        public IEnumerable<NEvent> AllEvents
        {
            get { return AllMembers.OfType<NEvent>(); }
        }

        /// <summary>
        /// Gets the type name of this type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; protected set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return FullName;
        }
    }
}