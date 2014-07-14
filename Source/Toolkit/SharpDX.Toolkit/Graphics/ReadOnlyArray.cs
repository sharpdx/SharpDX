// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Exposes an array as readonly with readonly elements with support for improved performance for equality.
    /// </summary>
    public class ReadOnlyArray<T> : IEnumerable<T>, IEquatable<ReadOnlyArray<T>>
    {
        private readonly int hashCode;
        internal T[] Elements;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyArray{T}" /> class.
        /// </summary>
        /// <param name="elements">The elements.</param>
        public ReadOnlyArray(T[] elements)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");

            Elements = elements;
            
            // precompute the hashcode
            hashCode = elements.Length;
            for(int i = 0; i < elements.Length; i++)
            {
                hashCode = (hashCode * 397) ^ elements[i].GetHashCode();
            }
        }

        /// <summary>
        /// Gets number of elements.
        /// </summary>
        /// <value>The number of elements.</value>
        public int Length
        {
            get { return Elements.Length; }
        }

        /// <summary>Gets a specific element in the collection by using an index value.</summary>
        /// <param name="index">Index of the value to get.</param>
        public T this[int index]
        {
            get
            {
                return Elements[index];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Elements.Length; i++)
                yield return Elements[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(ReadOnlyArray<T> other)
        {
            if (hashCode != other.hashCode)
                return false;

            if (ReferenceEquals(Elements, other.Elements))
                return true;

            if (Elements.Length != other.Elements.Length)
                return false;

            for (int i = 0; i < Elements.Length; i++)
            {
                if (!Equals(Elements[i], other.Elements[i]))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ReadOnlyArray<T>)obj);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ReadOnlyArray<T> left, ReadOnlyArray<T> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ReadOnlyArray<T> left, ReadOnlyArray<T> right)
        {
            return !Equals(left, right);
        }
    }
}