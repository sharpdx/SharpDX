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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SharpDoc.Model
{
    /// <summary>
    /// A .Net namespace.
    /// </summary>
    public class NNamespace : NModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NNamespace"/> class.
        /// </summary>
        /// <param name="name">The name of this namespace.</param>
        public NNamespace(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
            FullName = name;
            Types = new List<NType>();
            Category = "Namespace";
        }

        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public NAssembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>The types.</value>
        public List<NType> Types { get; private set; }

        /// <summary>
        /// Adds the type to this namespace
        /// </summary>
        /// <param name="type">The member.</param>
        public void AddType(NType type)
        {
            type.Namespace = this;
            Types.Add(type);
        }

        /// <summary>
        /// Gets a value indicating whether this instance has structures.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has structures; otherwise, <c>false</c>.
        /// </value>
        public bool HasStructures { get { return Structures.Any(); } }

            /// <summary>
        /// Gets the structures.
        /// </summary>
        /// <value>The structures.</value>
        public IEnumerable<NStruct> Structures
        {
            get { return Types.OfType<NStruct>(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has classes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has classes; otherwise, <c>false</c>.
        /// </value>
        public bool HasClasses { get { return Classes.Any(); } }

        /// <summary>
        /// Gets the classes.
        /// </summary>
        /// <value>The classes.</value>
        public IEnumerable<NClass> Classes
        {
            get { return Types.OfType<NClass>(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has interfaces.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has interfaces; otherwise, <c>false</c>.
        /// </value>
        public bool HasInterfaces { get { return Interfaces.Any(); } }

        /// <summary>
        /// Gets the interfaces.
        /// </summary>
        /// <value>The interfaces.</value>
        public IEnumerable<NInterface> Interfaces
        {
            get { return Types.OfType<NInterface>(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has enums.
        /// </summary>
        /// <value><c>true</c> if this instance has enums; otherwise, <c>false</c>.</value>
        public bool HasEnums { get { return Enums.Any(); } }

        /// <summary>
        /// Gets the enums.
        /// </summary>
        /// <value>The enums.</value>
        public IEnumerable<NEnum> Enums
        {
            get { return Types.OfType<NEnum>(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has delegates.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has delegates; otherwise, <c>false</c>.
        /// </value>
        public bool HasDelegates { get { return Delegates.Any(); } }

        /// <summary>
        /// Gets the enums.
        /// </summary>
        /// <value>The enums.</value>
        public IEnumerable<NDelegate> Delegates
        {
            get { return Types.OfType<NDelegate>(); }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(NNamespace other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Assembly, Assembly);
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
            if (obj.GetType() != typeof (NNamespace)) return false;
            return Equals((NNamespace) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Assembly != null ? Assembly.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}