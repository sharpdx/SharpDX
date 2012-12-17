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
    /// Represents a texel in ARGB8888 format.
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct Texel : IEquatable<Texel> {
        /// <summary>
        /// Blue component.
        /// </summary>
        public byte B;

        /// <summary>
        /// Green component.
        /// </summary>
        public byte G;

        /// <summary>
        /// Red component.
        /// </summary>
        public byte R;

        /// <summary>
        /// Alpha component.
        /// </summary>
        public byte A;

        /// <summary>
        /// Constructs a new Texel.
        /// </summary>
        /// <param name="b">Blue component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="r">Red component.</param>
        /// <param name="a">Alpha component.</param>
        public Texel(byte b, byte g, byte r, byte a) {
            B = b;
            G = g;
            R = r;
            A = a;
        }

        /// <summary>
        /// Tests equality between two texels.
        /// </summary>
        /// <param name="a">First texel</param>
        /// <param name="b">Second texel</param>
        /// <returns>True if the texels are equal, false otherwise.</returns>
        public static bool operator==(Texel a, Texel b) {
            return (a.B == b.B) && (a.G == b.G) && (a.R == b.R) && (a.A == b.A);
        }

        /// <summary>
        /// Tests inequality between two texels.
        /// </summary>
        /// <param name="a">First texel</param>
        /// <param name="b">Second texel</param>
        /// <returns>True if the texels are not equal, false otherwise.</returns>
        public static bool operator!=(Texel a, Texel b) {
            return (a.B != b.B) && (a.G != b.G) && (a.R != b.R) && (a.A != b.A);
        }

        /// <summary>
        /// Implicitly converts a texel to a Color4D.
        /// </summary>
        /// <param name="texel">Texel to convert</param>
        /// <returns>Converted Color4D</returns>
        public static implicit operator Color4D(Texel texel) {
            return new Color4D(texel.R / 255.0f, texel.G / 255.0f, texel.B / 255.0f, texel.A / 255.0f);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if(obj is Texel) {
                return Equals((Texel) obj);
            }
            return false;
        }

        /// <summary>
        /// Tests equality between this key and another.
        /// </summary>
        /// <param name="other">Other key to test</param>
        /// <returns>True if their indices are equal</returns>
        public bool Equals(Texel other) {
            return (B == other.B) && (G == other.G) && (R == other.R) && (A == other.A);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return B.GetHashCode() + G.GetHashCode() + R.GetHashCode() + A.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            CultureInfo info = CultureInfo.CurrentCulture;
            return String.Format(info, "{{B:{0} G:{1} R:{2} A:{3}}}",
                new Object[] { B.ToString(info), G.ToString(info), R.ToString(info), A.ToString(info) });
        }
    }
}
