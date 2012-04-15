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

namespace SharpGen.Model
{
    /// <summary>
    /// Type used for template
    /// </summary>
    public class InteropType
    {
        private Type _type;
        private string _typeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteropType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public InteropType(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteropType"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        public InteropType(string typeName)
        {
            _typeName = typeName;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Type"/> to <see cref="SharpDX.TypeWrapper"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InteropType(Type input)
        {
            return new InteropType(input);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="SharpDX.TypeWrapper"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InteropType(string input)
        {
            return new InteropType(input);
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName
        {
            get
            {
                Type type = Type;

                if (type != null)
                {
                    if (type == typeof(int))
                        return "int";
                    if (type == typeof(short))
                        return "short";
                    if (type == typeof(void*))
                        return "void*";
                    if (type == typeof(void))
                        return "void";
                    if (type == typeof(float))
                        return "float";
                    if (type == typeof(double))
                        return "double";
                    if (type == typeof(long))
                        return "long";

                    return type.FullName;
                }
                return _typeName;
            }
        }

        /// <summary>
        /// Gets the name of the param type of.
        /// </summary>
        /// <value>The name of the param type of.</value>
        public string ParamTypeOfName
        {
            get
            {
                return "typeof(" + TypeName + ")";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is primitive.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is primitive; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimitive
        {
            get
            {
                if (Type != null)
                    return Type.IsPrimitive;
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is value type.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is value type; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueType
        {
            get
            {
                if (Type != null)
                    return Type.IsValueType;
                // If typename != null is necessary a ValueType
                return true;
            }
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
            InteropType against = obj as InteropType;
            if (against == null)
                return false;
            return Type == against.Type && _typeName == against._typeName;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(InteropType a, Type b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Type == b;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(InteropType a, Type b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(InteropType a, InteropType b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(InteropType a, InteropType b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _type.GetHashCode();
        }
    }
}