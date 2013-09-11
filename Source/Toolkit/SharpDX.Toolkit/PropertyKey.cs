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

namespace SharpDX.Toolkit
{
    /// <summary>
    /// A Name describing a property attribute.
    /// </summary>
    public class PropertyKey : IEquatable<PropertyKey>
    {
        private readonly string name;

        private readonly int hashcode;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyKey"/> class.
        /// </summary>
        /// <param name="name">The Name.</param>
        public PropertyKey(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
            this.hashcode = name.GetHashCode();
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        /// <value>The Name.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

        public bool Equals(PropertyKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var materialKey = obj as PropertyKey;
            if (materialKey == null)
            {
                return false;
            }
            return Equals(materialKey);
        }

        public override int GetHashCode()
        {
            return hashcode;
        }

        public static bool operator ==(PropertyKey left, PropertyKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PropertyKey left, PropertyKey right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }
    }

    public class PropertyKey<T> : PropertyKey
    {
        public PropertyKey(string name) : base(name)
        {
        }
    }
}