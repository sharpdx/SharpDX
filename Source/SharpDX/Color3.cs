// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
// -----------------------------------------------------------------------------
// Original code from SlimMath project. http://code.google.com/p/slimmath/
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
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
using System.ComponentModel;

namespace SharpDX
{
    /// <summary>
    /// Represents a color in the form of rgb.
    /// </summary>
#if !WIN8METRO
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Color3 : IEquatable<Color3>, IFormattable
    {
        /// <summary>
        /// The red component of the color.
        /// </summary>
        public float Red;

        /// <summary>
        /// The green component of the color.
        /// </summary>
        public float Green;

        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public float Blue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Color3"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Color3(float value)
        {
            Red = Green = Blue = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Color3"/> struct.
        /// </summary>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color3(float red, float green, float blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Color3"/> struct.
        /// </summary>
        /// <param name="value">The red, green, and blue components of the color.</param>
        public Color3(Vector3 value)
        {
            Red = value.X;
            Green = value.Y;
            Blue = value.Z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Color3"/> struct.
        /// </summary>
        /// <param name="rgb">A packed integer containing all three color components.
        /// The alpha component is ignored.</param>
        public Color3(int rgb)
        {
            Red = ((rgb >> 16) & 255) / 255.0f;
            Green = ((rgb >> 8) & 255) / 255.0f;
            Blue = (rgb & 255) / 255.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Color3"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the red, green, and blue components of the color. This must be an array with three elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public Color3(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 3)
                throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for Color3.");

            Red = values[0];
            Green = values[1];
            Blue = values[2];
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the red, green, or blue component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the red component, 1 for the green component, and 2 for the blue component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 2].</exception>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Red;
                    case 1: return Green;
                    case 2: return Blue;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Color3 run from 0 to 2, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: Red = value; break;
                    case 1: Green = value; break;
                    case 2: Blue = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Color3 run from 0 to 2, inclusive.");
                }
            }
        }

        /// <summary>
        /// Converts the color into a packed integer.
        /// </summary>
        /// <returns>A packed integer containing all three color components.
        /// The alpha channel is set to 255.</returns>
        public int ToRgb()
        {
            uint a = 255;
            uint r = (uint)(Red * 255.0f);
            uint g = (uint)(Green * 255.0f);
            uint b = (uint)(Blue * 255.0f);

            uint value = b;
            value += g << 8;
            value += r << 16;
            value += a << 24;

            return (int)value;
        }

        /// <summary>
        /// Converts the color into a three component vector.
        /// </summary>
        /// <returns>A three component vector containing the red, green, and blue components of the color.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(Red, Green, Blue);
        }

        /// <summary>
        /// Creates an array containing the elements of the color.
        /// </summary>
        /// <returns>A three-element array containing the components of the color.</returns>
        public float[] ToArray()
        {
            return new float[] { Red, Green, Blue };
        }

        /// <summary>
        /// Adds two colors.
        /// </summary>
        /// <param name="left">The first color to add.</param>
        /// <param name="right">The second color to add.</param>
        /// <param name="result">When the method completes, completes the sum of the two colors.</param>
        public static void Add(ref Color3 left, ref Color3 right, out Color3 result)
        {
            result.Red = left.Red + right.Red;
            result.Green = left.Green + right.Green;
            result.Blue = left.Blue + right.Blue;
        }

        /// <summary>
        /// Adds two colors.
        /// </summary>
        /// <param name="left">The first color to add.</param>
        /// <param name="right">The second color to add.</param>
        /// <returns>The sum of the two colors.</returns>
        public static Color3 Add(Color3 left, Color3 right)
        {
            return new Color3(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue);
        }

        /// <summary>
        /// Subtracts two colors.
        /// </summary>
        /// <param name="left">The first color to subtract.</param>
        /// <param name="right">The second color to subtract.</param>
        /// <param name="result">WHen the method completes, contains the difference of the two colors.</param>
        public static void Subtract(ref Color3 left, ref Color3 right, out Color3 result)
        {
            result.Red = left.Red - right.Red;
            result.Green = left.Green - right.Green;
            result.Blue = left.Blue - right.Blue;
        }

        /// <summary>
        /// Subtracts two colors.
        /// </summary>
        /// <param name="left">The first color to subtract.</param>
        /// <param name="right">The second color to subtract</param>
        /// <returns>The difference of the two colors.</returns>
        public static Color3 Subtract(Color3 left, Color3 right)
        {
            return new Color3(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue);
        }

        /// <summary>
        /// Modulates two colors.
        /// </summary>
        /// <param name="left">The first color to modulate.</param>
        /// <param name="right">The second color to modulate.</param>
        /// <param name="result">When the method completes, contains the modulated color.</param>
        public static void Modulate(ref Color3 left, ref Color3 right, out Color3 result)
        {
            result.Red = left.Red * right.Red;
            result.Green = left.Green * right.Green;
            result.Blue = left.Blue * right.Blue;
        }

        /// <summary>
        /// Modulates two colors.
        /// </summary>
        /// <param name="left">The first color to modulate.</param>
        /// <param name="right">The second color to modulate.</param>
        /// <returns>The modulated color.</returns>
        public static Color3 Modulate(Color3 left, Color3 right)
        {
            return new Color3(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue);
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="value">The color to scale.</param>
        /// <param name="scale">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled color.</param>
        public static void Scale(ref Color3 value, float scale, out Color3 result)
        {
            result.Red = value.Red * scale;
            result.Green = value.Green * scale;
            result.Blue = value.Blue * scale;
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="value">The color to scale.</param>
        /// <param name="scale">The amount by which to scale.</param>
        /// <returns>The scaled color.</returns>
        public static Color3 Scale(Color3 value, float scale)
        {
            return new Color3(value.Red * scale, value.Green * scale, value.Blue * scale);
        }

        /// <summary>
        /// Negates a color.
        /// </summary>
        /// <param name="value">The color to negate.</param>
        /// <param name="result">When the method completes, contains the negated color.</param>
        public static void Negate(ref Color3 value, out Color3 result)
        {
            result.Red = 1.0f - value.Red;
            result.Green = 1.0f - value.Green;
            result.Blue = 1.0f - value.Blue;
        }

        /// <summary>
        /// Negates a color.
        /// </summary>
        /// <param name="value">The color to negate.</param>
        /// <returns>The negated color.</returns>
        public static Color3 Negate(Color3 value)
        {
            return new Color3(1.0f - value.Red, 1.0f - value.Green, 1.0f - value.Blue);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">When the method completes, contains the clamped value.</param>
        public static void Clamp(ref Color3 value, ref Color3 min, ref Color3 max, out Color3 result)
        {
            float red = value.Red;
            red = (red > max.Red) ? max.Red : red;
            red = (red < min.Red) ? min.Red : red;

            float green = value.Green;
            green = (green > max.Green) ? max.Green : green;
            green = (green < min.Green) ? min.Green : green;

            float blue = value.Blue;
            blue = (blue > max.Blue) ? max.Blue : blue;
            blue = (blue < min.Blue) ? min.Blue : blue;

            result = new Color3(red, green, blue);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static Color3 Clamp(Color3 value, Color3 min, Color3 max)
        {
            Color3 result;
            Clamp(ref value, ref min, ref max, out result);
            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two colors.</param>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static void Lerp(ref Color3 start, ref Color3 end, float amount, out Color3 result)
        {
            result.Red = start.Red + amount * (end.Red - start.Red);
            result.Green = start.Green + amount * (end.Green - start.Green);
            result.Blue = start.Blue + amount * (end.Blue - start.Blue);
        }

        /// <summary>
        /// Performs a linear interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two colors.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static Color3 Lerp(Color3 start, Color3 end, float amount)
        {
            return new Color3(
                start.Red + amount * (end.Red - start.Red),
                start.Green + amount * (end.Green - start.Green),
                start.Blue + amount * (end.Blue - start.Blue));
        }

        /// <summary>
        /// Performs a cubic interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the cubic interpolation of the two colors.</param>
        public static void SmoothStep(ref Color3 start, ref Color3 end, float amount, out Color3 result)
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.Red = start.Red + ((end.Red - start.Red) * amount);
            result.Green = start.Green + ((end.Green - start.Green) * amount);
            result.Blue = start.Blue + ((end.Blue - start.Blue) * amount);
        }

        /// <summary>
        /// Performs a cubic interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The cubic interpolation of the two colors.</returns>
        public static Color3 SmoothStep(Color3 start, Color3 end, float amount)
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            return new Color3(
                start.Red + ((end.Red - start.Red) * amount),
                start.Green + ((end.Green - start.Green) * amount),
                start.Blue + ((end.Blue - start.Blue) * amount));
        }

        /// <summary>
        /// Returns a color containing the smallest components of the specified colorss.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <param name="result">When the method completes, contains an new color composed of the largest components of the source colorss.</param>
        public static void Max(ref Color3 left, ref Color3 right, out Color3 result)
        {
            result.Red = (left.Red > right.Red) ? left.Red : right.Red;
            result.Green = (left.Green > right.Green) ? left.Green : right.Green;
            result.Blue = (left.Blue > right.Blue) ? left.Blue : right.Blue;
        }

        /// <summary>
        /// Returns a color containing the largest components of the specified colorss.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <returns>A color containing the largest components of the source colors.</returns>
        public static Color3 Max(Color3 left, Color3 right)
        {
            Color3 result;
            Max(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Returns a color containing the smallest components of the specified colors.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <param name="result">When the method completes, contains an new color composed of the smallest components of the source colors.</param>
        public static void Min(ref Color3 left, ref Color3 right, out Color3 result)
        {
            result.Red = (left.Red < right.Red) ? left.Red : right.Red;
            result.Green = (left.Green < right.Green) ? left.Green : right.Green;
            result.Blue = (left.Blue < right.Blue) ? left.Blue : right.Blue;
        }

        /// <summary>
        /// Returns a color containing the smallest components of the specified colors.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <returns>A color containing the smallest components of the source colors.</returns>
        public static Color3 Min(Color3 left, Color3 right)
        {
            Color3 result;
            Min(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Adjusts the contrast of a color.
        /// </summary>
        /// <param name="value">The color whose contrast is to be adjusted.</param>
        /// <param name="contrast">The amount by which to adjust the contrast.</param>
        /// <param name="result">When the method completes, contains the adjusted color.</param>
        public static void AdjustContrast(ref Color3 value, float contrast, out Color3 result)
        {
            result.Red = 0.5f + contrast * (value.Red - 0.5f);
            result.Green = 0.5f + contrast * (value.Green - 0.5f);
            result.Blue = 0.5f + contrast * (value.Blue - 0.5f);
        }

        /// <summary>
        /// Adjusts the contrast of a color.
        /// </summary>
        /// <param name="value">The color whose contrast is to be adjusted.</param>
        /// <param name="contrast">The amount by which to adjust the contrast.</param>
        /// <returns>The adjusted color.</returns>
        public static Color3 AdjustContrast(Color3 value, float contrast)
        {
            return new Color3(
                0.5f + contrast * (value.Red - 0.5f),
                0.5f + contrast * (value.Green - 0.5f),
                0.5f + contrast * (value.Blue - 0.5f));
        }

        /// <summary>
        /// Adjusts the saturation of a color.
        /// </summary>
        /// <param name="value">The color whose saturation is to be adjusted.</param>
        /// <param name="saturation">The amount by which to adjust the saturation.</param>
        /// <param name="result">When the method completes, contains the adjusted color.</param>
        public static void AdjustSaturation(ref Color3 value, float saturation, out Color3 result)
        {
            float grey = value.Red * 0.2125f + value.Green * 0.7154f + value.Blue * 0.0721f;

            result.Red = grey + saturation * (value.Red - grey);
            result.Green = grey + saturation * (value.Green - grey);
            result.Blue = grey + saturation * (value.Blue - grey);
        }

        /// <summary>
        /// Adjusts the saturation of a color.
        /// </summary>
        /// <param name="value">The color whose saturation is to be adjusted.</param>
        /// <param name="saturation">The amount by which to adjust the saturation.</param>
        /// <returns>The adjusted color.</returns>
        public static Color3 AdjustSaturation(Color3 value, float saturation)
        {
            float grey = value.Red * 0.2125f + value.Green * 0.7154f + value.Blue * 0.0721f;

            return new Color3(
                grey + saturation * (value.Red - grey),
                grey + saturation * (value.Green - grey),
                grey + saturation * (value.Blue - grey));
        }

        /// <summary>
        /// Adds two colors.
        /// </summary>
        /// <param name="left">The first color to add.</param>
        /// <param name="right">The second color to add.</param>
        /// <returns>The sum of the two colors.</returns>
        public static Color3 operator +(Color3 left, Color3 right)
        {
            return new Color3(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue);
        }

        /// <summary>
        /// Assert a color (return it unchanged).
        /// </summary>
        /// <param name="value">The color to assert (unchange).</param>
        /// <returns>The asserted (unchanged) color.</returns>
        public static Color3 operator +(Color3 value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two colors.
        /// </summary>
        /// <param name="left">The first color to subtract.</param>
        /// <param name="right">The second color to subtract.</param>
        /// <returns>The difference of the two colors.</returns>
        public static Color3 operator -(Color3 left, Color3 right)
        {
            return new Color3(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue);
        }

        /// <summary>
        /// Negates a color.
        /// </summary>
        /// <param name="value">The color to negate.</param>
        /// <returns>A negated color.</returns>
        public static Color3 operator -(Color3 value)
        {
            return new Color3(-value.Red, -value.Green, -value.Blue);
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="scale">The factor by which to scale the color.</param>
        /// <param name="value">The color to scale.</param>
        /// <returns>The scaled color.</returns>
        public static Color3 operator *(float scale, Color3 value)
        {
            return new Color3(value.Red * scale, value.Green * scale, value.Blue * scale);
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="value">The factor by which to scale the color.</param>
        /// <param name="scale">The color to scale.</param>
        /// <returns>The scaled color.</returns>
        public static Color3 operator *(Color3 value, float scale)
        {
            return new Color3(value.Red * scale, value.Green * scale, value.Blue * scale);
        }

        /// <summary>
        /// Modulates two colors.
        /// </summary>
        /// <param name="left">The first color to modulate.</param>
        /// <param name="right">The second color to modulate.</param>
        /// <returns>The modulated color.</returns>
        public static Color3 operator *(Color3 left, Color3 right)
        {
            return new Color3(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color3 left, Color3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Color3 left, Color3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.Color3"/> to <see cref="SharpDX.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color4(Color3 value)
        {
            return new Color4(value.Red, value.Green, value.Blue, 1.0f);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.Color3"/> to <see cref="SharpDX.Vector3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3(Color3 value)
        {
            return new Vector3(value.Red, value.Green, value.Blue);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.Vector3"/> to <see cref="SharpDX.Color3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color3(Vector3 value)
        {
            return new Color3(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Int32"/> to <see cref="SharpDX.Color3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color3(int value)
        {
            return new Color3(value);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Red:{1} Green:{2} Blue:{3}", Red, Green, Blue);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "Red:{1} Green:{2} Blue:{3}", Red.ToString(format, CultureInfo.CurrentCulture),
                Green.ToString(format, CultureInfo.CurrentCulture), Blue.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "Red:{1} Green:{2} Blue:{3}", Red, Green, Blue);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                return ToString(formatProvider);

            return string.Format(formatProvider, "Red:{1} Green:{2} Blue:{3}",Red.ToString(format, formatProvider),
                Green.ToString(format, formatProvider), Blue.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Red.GetHashCode() + Green.GetHashCode() + Blue.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="SharpDX.Color3"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SharpDX.Color3"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SharpDX.Color3"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Color3 other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            if (!ReferenceEquals(value.GetType(), typeof(Color3)))
                return false;

            return Equals((Color3)value);
        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Color3"/> to <see cref="SlimDX.Color3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Color3(Color3 value)
        {
            return new SlimDX.Color3(value.Red, value.Green, value.Blue);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Color3"/> to <see cref="SharpDX.Color3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color3(SlimDX.Color3 value)
        {
            return new Color3(value.Red, value.Green, value.Blue);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.Color3"/> to <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator System.Windows.Media.Color(Color3 value)
        {
            return new System.Windows.Media.Color()
            {
                A = 255,
                R = (byte)(255f * value.Red),
                G = (byte)(255f * value.Green),
                B = (byte)(255f * value.Blue)
            };
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Media.Color"/> to <see cref="SharpDX.Color3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color3(System.Windows.Media.Color value)
        {
            return new Color3()
            {
                Red = (float)value.R / 255f,
                Green = (float)value.G / 255f,
                Blue = (float)value.B / 255f
            };
        }
#endif

#if WinFormsInterop
        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.Color3"/> to <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Drawing.Color(Color3 value)
        {
            return System.Drawing.Color.FromArgb(
                (byte)(255f * value.Red),
                (byte)(255f * value.Green),
                (byte)(255f * value.Blue));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Drawing.Color"/> to <see cref="SharpDX.Color3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color3(System.Drawing.Color value)
        {
            return new Color3()
            {
                Red = (float)value.R / 255f,
                Green = (float)value.G / 255f,
                Blue = (float)value.B / 255f
            };
        }
#endif
    }
}
