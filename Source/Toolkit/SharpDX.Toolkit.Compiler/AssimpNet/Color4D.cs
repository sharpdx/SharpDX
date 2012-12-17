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
    /// Represents a Red-Green-Blue-Alpha (RGBA) color.
    /// Color values range from 0 to 1.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct Color4D : IEquatable<Color4D> {

        /// <summary>
        /// Red component.
        /// </summary>
        public float R;

        /// <summary>
        /// Green component.
        /// </summary>
        public float G;

        /// <summary>
        /// Blue component.
        /// </summary>
        public float B;

        /// <summary>
        /// Alpha component.
        /// </summary>
        public float A;

        /// <summary>
        /// Gets or sets the component value at the specified zero-based index
        /// in the order of RGBA (index 0 access R, 1 access G, etc). If
        /// the index is not in range, a value of zero is returned.
        /// </summary>
        /// <param name="index">Zero-based index.</param>
        /// <returns>The component value</returns>
        public float this[int index] {
            get {
                switch(index) {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    case 3:
                        return A;
                    default:
                        return 0;
                }
            }
            set {
                switch(index) {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    case 3:
                        A = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Constructs a Color4D.
        /// </summary>
        /// <param name="r">Red component</param>
        /// <param name="g">Green component</param>
        /// <param name="b">Blue component</param>
        /// <param name="a">Alpha component</param>
        public Color4D(float r, float g, float b, float a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Constructs a Color4D. Alpha is set to 1.0.
        /// </summary>
        /// <param name="r">Red component</param>
        /// <param name="g">Green component</param>
        /// <param name="b">Blue component</param>
        public Color4D(float r, float g, float b) {
            R = r;
            G = g;
            B = b;
            A = 1.0f;
        }

        /// <summary>
        /// Constructs a Color4D where each component is
        /// set to the same value.
        /// </summary>
        /// <param name="value">Value to set R, G, B, A components</param>
        public Color4D(float value) {
            R = value;
            G = value;
            B = value;
            A = value;
        }

        /// <summary>
        /// Determines if the color is black, or close to being black.
        /// </summary>
        /// <returns>True if the color is black/nearly block, false otherwise.</returns>
        public bool IsBlack() {
            //Don't care about alpha
            float epsilon = 10e-3f;
            return (float)Math.Abs(R) < epsilon 
                && (float)Math.Abs(G) < epsilon 
                && (float)Math.Abs(B) < epsilon;
        }

        /// <summary>
        /// Adds the two colors together.
        /// </summary>
        /// <param name="a">First color</param>
        /// <param name="b">Second color</param>
        /// <returns>Added color</returns>
        public static Color4D operator+(Color4D a, Color4D b) {
            Color4D c;
            c.R = a.R + b.R;
            c.G = a.G + b.G;
            c.B = a.B + b.B;
            c.A = a.A + b.A;
            return c;
        }

        /// <summary>
        /// Adds the value to each of the components of the color.
        /// </summary>
        /// <param name="color">Source color</param>
        /// <param name="value">Value to add to each component</param>
        /// <returns>Added color</returns>
        public static Color4D operator+(Color4D color, float value) {
            Color4D c;
            c.R = color.R + value;
            c.G = color.G + value;
            c.B = color.B + value;
            c.A = color.A + value;
            return c;
        }

        /// <summary>
        /// Adds the value to each of the components of the color.
        /// </summary>
        /// <param name="value">Value to add to each component</param>
        /// <param name="color">Source color</param>
        /// <returns>Added color</returns>
        public static Color4D operator+(float value, Color4D color) {
            Color4D c;
            c.R = color.R + value;
            c.G = color.G + value;
            c.B = color.B + value;
            c.A = color.A + value;
            return c;
        }

        /// <summary>
        /// Subtracts the second color from the first color.
        /// </summary>
        /// <param name="a">First color</param>
        /// <param name="b">Second color</param>
        /// <returns>Resulting color</returns>
        public static Color4D operator-(Color4D a, Color4D b) {
            Color4D c;
            c.R = a.R - b.R;
            c.G = a.G - b.G;
            c.B = a.B - b.B;
            c.A = a.A - b.A;
            return c;
        }

        /// <summary>
        /// Subtracts the value from each of the color's components.
        /// </summary>
        /// <param name="color">Source color</param>
        /// <param name="value">Value to subtract from each component</param>
        /// <returns>Resulting color</returns>
        public static Color4D operator-(Color4D color, float value) {
            Color4D c;
            c.R = color.R - value;
            c.G = color.G - value;
            c.B = color.B - value;
            c.A = color.A - value;
            return c;
        }

        /// <summary>
        /// Subtracts the color's components from the value, returning
        /// the result as a new color. Same as <c>new Color4D(value) - color</c>
        /// </summary>
        /// <param name="value">Value for each component of the first color</param>
        /// <param name="color">Second color</param>
        /// <returns>Resulting color</returns>
        public static Color4D operator-(float value, Color4D color) {
            Color4D c;
            c.R = value - color.R;
            c.G = value - color.G;
            c.B = value - color.B;
            c.A = value - color.A;
            return c;
        }

        /// <summary>
        /// Multiplies the two colors.
        /// </summary>
        /// <param name="a">First color</param>
        /// <param name="b">Second color</param>
        /// <returns>Multiplied color.</returns>
        public static Color4D operator*(Color4D a, Color4D b) {
            Color4D c;
            c.R = a.R * b.R;
            c.G = a.G * b.G;
            c.B = a.B * b.B;
            c.A = a.A * b.A;
            return c;
        }

        /// <summary>
        /// Multiplies the color by a scalar value, component wise.
        /// </summary>
        /// <param name="value">Source color</param>
        /// <param name="scale">Scalar value</param>
        /// <returns>Resulting color</returns>
        public static Color4D operator*(Color4D value, float scale) {
            Color4D c;
            c.R = value.R * scale;
            c.G = value.G * scale;
            c.B = value.B * scale;
            c.A = value.A * scale;
            return c;
        }

        /// <summary>
        /// Multiplies the color by a scalar value, component wise.
        /// </summary>
        /// <param name="scale">Scalar value</param>
        /// <param name="value">Source color</param>
        /// <returns>Resulting color</returns>
        public static Color4D operator*(float scale, Color4D value) {
            Color4D c;
            c.R = value.R * scale;
            c.G = value.G * scale;
            c.B = value.B * scale;
            c.A = value.A * scale;
            return c;
        }

        /// <summary>
        /// Divides the first color by the second color, component wise.
        /// </summary>
        /// <param name="a">First color</param>
        /// <param name="b">Second color</param>
        /// <returns>Resulting color</returns>
        public static Color4D operator/(Color4D a, Color4D b) {
            Color4D c;
            c.R = a.R / b.R;
            c.G = a.G / b.G;
            c.B = a.B / b.B;
            c.A = a.A / b.A;
            return c;
        }

        /// <summary>
        /// Divides the color by a divisor value.
        /// </summary>
        /// <param name="color">Source color</param>
        /// <param name="divisor">Divisor</param>
        /// <returns>Resulting color</returns>
        public static Color4D operator/(Color4D color, float divisor) {
            float invDivisor = 1.0f / divisor;
            Color4D c;
            c.R = color.R * invDivisor;
            c.G = color.G * invDivisor;
            c.B = color.B * invDivisor;
            c.A = color.A * invDivisor;
            return c;
        }

        /// <summary>
        /// Tets equality between two colors.
        /// </summary>
        /// <param name="a">First color</param>
        /// <param name="b">Second color</param>
        /// <returns>True if the colors are equal, false otherwise</returns>
        public static bool operator==(Color4D a, Color4D b) {
            return (a.R == b.R) && (a.G == b.G) && (a.B == b.B) && (a.A == b.A);
        }

        /// <summary>
        /// Tets inequality between two colors.
        /// </summary>
        /// <param name="a">First color</param>
        /// <param name="b">Second color</param>
        /// <returns>True if the colors are not equal, false otherwise</returns>
        public static bool operator!=(Color4D a, Color4D b) {
            return (a.R != b.R) || (a.G != b.G) || (a.B != b.B) || (a.A != b.A);
        }

        /// <summary>
        /// Tests equality between this color and another color
        /// </summary>
        /// <param name="other">Color to test against</param>
        /// <returns>True if components are equal</returns>
        public bool Equals(Color4D other) {
            return (R == other.R) && (G == other.G) && (B == other.B) && (A == other.A);
        }

        /// <summary>
        /// Tests equality between this color and another object.
        /// </summary>
        /// <param name="obj">Object to test against</param>
        /// <returns>True if the object is a color and the components are equal</returns>
        public override bool Equals(object obj) {
            if(obj is Color4D) {
                return Equals((Color4D) obj);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return R.GetHashCode() + G.GetHashCode() + B.GetHashCode() + A.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            CultureInfo info = CultureInfo.CurrentCulture;
            return String.Format(info, "{{R:{0} G:{1} B:{2} A:{3}}}",
                new Object[] { R.ToString(info), G.ToString(info), B.ToString(info), A.ToString(info) });
        }
    }
}
