/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Assimp {
    /// <summary>
    /// Time-value pair specifying a 3D vector for a given time.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct VectorKey : IEquatable<VectorKey> {
        /// <summary>
        /// The time of this key.
        /// </summary>
        public double Time;

        /// <summary>
        /// The 3D vector value of this key.
        /// </summary>
        public Vector3D Value;

        /// <summary>
        /// Constructs a new VectorKey.
        /// </summary>
        /// <param name="time">The time of this key.</param>
        /// <param name="vector">The 3D vector value of this key.</param>
        public VectorKey(double time, Vector3D vector) {
            Time = time;
            Value = vector;
        }

        /// <summary>
        /// Tests equality between two keys.
        /// </summary>
        /// <param name="a">The first key</param>
        /// <param name="b">The second key</param>
        /// <returns>True if the key's 3D vectors are the same, false otherwise.</returns>
        public static bool operator==(VectorKey a, VectorKey b) {
            return a.Value == b.Value;
        }

        /// <summary>
        /// Tests inequality between two keys.
        /// </summary>
        /// <param name="a">The first key</param>
        /// <param name="b">The second key</param>
        /// <returns>True if the key's 3D vectors are not the same, false otherwise.</returns>
        public static bool operator!=(VectorKey a, VectorKey b) {
            return a.Value != b.Value;
        }

        /// <summary>
        /// Tests inequality between two keys.
        /// </summary>
        /// <param name="a">The first key</param>
        /// <param name="b">The second key</param>
        /// <returns>True if the first key's time is less than the second key's.</returns>
        public static bool operator <(VectorKey a, VectorKey b) {
            return a.Time < b.Time;
        }

        /// <summary>
        /// Tests inequality between two keys.
        /// </summary>
        /// <param name="a">The first key</param>
        /// <param name="b">The second key</param>
        /// <returns>True if the first key's time is greater than the second key's.</returns>
        public static bool operator >(VectorKey a, VectorKey b) {
            return a.Time > b.Time;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if(obj is VectorKey) {
                return Equals((VectorKey) obj);
            }
            return false;
        }

        /// <summary>
        /// Tests equality between this key and another.
        /// </summary>
        /// <param name="key">Other key to test</param>
        /// <returns>True if their 3D vectors are equal.</returns>
        public bool Equals(VectorKey key) {
            return Value == key.Value;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            CultureInfo info = CultureInfo.CurrentCulture;
            return String.Format(info, "{{Time:{0} Vector:{1}}}",
                new Object[] { Time.ToString(info), Value.ToString() });
        }
    }
}
