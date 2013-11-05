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

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SharpDX
{
    /// <summary>
    /// Equality comparer using the identify of the object.
    /// </summary>
    /// <typeparam name="T">Type of the parameter</typeparam>
    /// <remarks>
    /// From http://stackoverflow.com/questions/8946790/how-to-use-an-objects-identity-as-key-for-dictionaryk-v.
    /// </remarks>
    public class IdentityEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        /// <summary>Returns a hash code for this instance.</summary>
        /// <param name="value">The value.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(T value)
        {
            return RuntimeHelpers.GetHashCode(value);
        }

        /// <summary>Equalses the specified left.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(T left, T right)
        {
            return left == right; // Reference identity comparison
        }
    }
}