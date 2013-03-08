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

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A Name describing a material attribute.
    /// </summary>
    public class MaterialKey : IDataSerializable, IEquatable<MaterialKey>
    {
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialKey"/> class.
        /// </summary>
        public MaterialKey()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialKey"/> class.
        /// </summary>
        /// <param name="name">The Name.</param>
        public MaterialKey(string name)
        {
            this.name = name;
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


        public bool Equals(MaterialKey other)
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
            var materialKey = obj as MaterialKey;
            if (materialKey == null)
            {
                return false;
            }
            return Equals(materialKey);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public static bool operator ==(MaterialKey left, MaterialKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MaterialKey left, MaterialKey right)
        {
            return !Equals(left, right);
        }

        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref name);
        }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }
    }

    public class MaterialKey<T> : MaterialKey
    {
        public MaterialKey(string name) : base(name)
        {
        }
    }
}