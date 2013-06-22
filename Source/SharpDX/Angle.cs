﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using SharpDX.Serialization;


namespace SharpDX
{
    /// <summary>
    /// Represents a unit independent angle using a single-precision floating-point
    /// internal representation.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
#if !W8CORE
    [Serializable]
#endif
    public struct AngleSingle : IComparable, IComparable<AngleSingle>, IEquatable<AngleSingle>, IFormattable, IDataSerializable
    {
        /// <summary>
        /// A value that specifies the size of a single degree.
        /// </summary>
        public const float Degree = 0.002777777777777778f;

        /// <summary>
        /// A value that specifies the size of a single minute.
        /// </summary>
        public const float Minute = 0.000046296296296296f;

        /// <summary>
        /// A value that specifies the size of a single second.
        /// </summary>
        public const float Second = 0.000000771604938272f;

        /// <summary>
        /// A value that specifies the size of a single radian.
        /// </summary>
        public const float Radian = 0.159154943091895336f;

        /// <summary>
        /// A value that specifies the size of a single milliradian.
        /// </summary>
        public const float Milliradian = 0.0001591549431f;

        /// <summary>
        /// A value that specifies the size of a single gradian.
        /// </summary>
        public const float Gradian = 0.0025f;

        /// <summary>
        /// The internal representation of the angle.
        /// </summary>
        [FieldOffset(0)]
        float radians;

        [FieldOffset(0)]
        private int radiansInt;

        /// <summary>
        /// Initializes a new instance of the SharpDX.AngleSingle structure with the
        /// given unit dependant angle and unit type.
        /// </summary>
        /// <param name="angle">A unit dependant measure of the angle.</param>
        /// <param name="type">The type of unit the angle argument is.</param>
        public AngleSingle(float angle, AngleType type)
        {
            radiansInt = 0;
            switch (type)
            {
                case AngleType.Revolution:
                    radians = MathUtil.RevolutionsToRadians(angle);
                    break;

                case AngleType.Degree:
                    radians = MathUtil.DegreesToRadians(angle);
                    break;

                case AngleType.Radian:
                    radians = angle;
                    break;

                case AngleType.Gradian:
                    radians = MathUtil.GradiansToRadians(angle);
                    break;

                default:
                    radians = 0.0f;
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SharpDX.AngleSingle structure using the
        /// arc length formula (θ = s/r).
        /// </summary>
        /// <param name="arcLength">The measure of the arc.</param>
        /// <param name="radius">The radius of the circle.</param>
        public AngleSingle(float arcLength, float radius)
        {
            radiansInt = 0;
            radians = arcLength / radius;
        }

        /// <summary>
        /// Wraps this SharpDX.AngleSingle to be in the range [π, -π].
        /// </summary>
        public void Wrap()
        {
            float newangle = (float)Math.IEEERemainder(radians, MathUtil.TwoPi);

            if (newangle <= -MathUtil.Pi)
                newangle += MathUtil.TwoPi;
            else if (newangle > MathUtil.Pi)
                newangle -= MathUtil.TwoPi;

            radians = newangle;
        }

        /// <summary>
        /// Wraps this SharpDX.AngleSingle to be in the range [0, 2π).
        /// </summary>
        public void WrapPositive()
        {
            float newangle = radians % MathUtil.TwoPi;

            if (newangle < 0.0)
                newangle += MathUtil.TwoPi;

            radians = newangle;
        }

        /// <summary>
        /// Gets or sets the total number of revolutions this SharpDX.AngleSingle represents.
        /// </summary>
        public float Revolutions
        {
            get { return MathUtil.RadiansToRevolutions(radians); }
            set { radians = MathUtil.RevolutionsToRadians(value); }
        }

        /// <summary>
        /// Gets or sets the total number of degrees this SharpDX.AngleSingle represents.
        /// </summary>
        public float Degrees
        {
            get { return MathUtil.RadiansToDegrees(radians); }
            set { radians = MathUtil.DegreesToRadians(value); }
        }

        /// <summary>
        /// Gets or sets the minutes component of the degrees this SharpDX.AngleSingle represents.
        /// When setting the minutes, if the value is in the range (-60, 60) the whole degrees are
        /// not changed; otherwise, the whole degrees may be changed. Fractional values may set
        /// the seconds component.
        /// </summary>
        public float Minutes
        {
            get
            {
                float degrees = MathUtil.RadiansToDegrees(radians);

                if (degrees < 0)
                {
                    float degreesfloor = (float)Math.Ceiling(degrees);
                    return (degrees - degreesfloor) * 60.0f;
                }
                else
                {
                    float degreesfloor = (float)Math.Floor(degrees);
                    return (degrees - degreesfloor) * 60.0f;
                }
            }
            set
            {
                float degrees = MathUtil.RadiansToDegrees(radians);
                float degreesfloor = (float)Math.Floor(degrees);

                degreesfloor += value / 60.0f;
                radians = MathUtil.DegreesToRadians(degreesfloor);
            }
        }

        /// <summary>
        /// Gets or sets the seconds of the degrees this SharpDX.AngleSingle represents.
        /// When setting the seconds, if the value is in the range (-60, 60) the whole minutes
        /// or whole degrees are not changed; otherwise, the whole minutes or whole degrees
        /// may be changed.
        /// </summary>
        public float Seconds
        {
            get
            {
                float degrees = MathUtil.RadiansToDegrees(radians);

                if (degrees < 0)
                {
                    float degreesfloor = (float)Math.Ceiling(degrees);

                    float minutes = (degrees - degreesfloor) * 60.0f;
                    float minutesfloor = (float)Math.Ceiling(minutes);

                    return (minutes - minutesfloor) * 60.0f;
                }
                else
                {
                    float degreesfloor = (float)Math.Floor(degrees);

                    float minutes = (degrees - degreesfloor) * 60.0f;
                    float minutesfloor = (float)Math.Floor(minutes);

                    return (minutes - minutesfloor) * 60.0f;
                }
            }
            set
            {
                float degrees = MathUtil.RadiansToDegrees(radians);
                float degreesfloor = (float)Math.Floor(degrees);

                float minutes = (degrees - degreesfloor) * 60.0f;
                float minutesfloor = (float)Math.Floor(minutes);

                minutesfloor += value / 60.0f;
                degreesfloor += minutesfloor / 60.0f;
                radians = MathUtil.DegreesToRadians(degreesfloor);
            }
        }
        
        /// <summary>
        /// Gets or sets the total number of radians this SharpDX.AngleSingle represents.
        /// </summary>
        public float Radians
        {
            get { return radians; }
            set { radians = value; }
        }

        /// <summary>
        /// Gets or sets the total number of milliradians this SharpDX.AngleSingle represents.
        /// One milliradian is equal to 1/(2000π).
        /// </summary>
        public float Milliradians
        {
            get { return radians / (Milliradian * MathUtil.TwoPi); }
            set { radians = value * (Milliradian * MathUtil.TwoPi); }
        }

        /// <summary>
        /// Gets or sets the total number of gradians this SharpDX.AngleSingle represents.
        /// </summary>
        public float Gradians
        {
            get { return MathUtil.RadiansToGradians(radians); }
            set { radians = MathUtil.RadiansToGradians(value); }
        }

        /// <summary>
        /// Gets a System.Boolean that determines whether this SharpDX.Angle
        /// is a right angle (i.e. 90° or π/2).
        /// </summary>
        public bool IsRight
        {
            get { return radians == MathUtil.PiOverTwo; }
        }

        /// <summary>
        /// Gets a System.Boolean that determines whether this SharpDX.Angle
        /// is a straight angle (i.e. 180° or π).
        /// </summary>
        public bool IsStraight
        {
            get { return radians == MathUtil.Pi; }
        }

        /// <summary>
        /// Gets a System.Boolean that determines whether this SharpDX.Angle
        /// is a full rotation angle (i.e. 360° or 2π).
        /// </summary>
        public bool IsFullRotation
        {
            get { return radians == MathUtil.TwoPi; }
        }

        /// <summary>
        /// Gets a System.Boolean that determines whether this SharpDX.Angle
        /// is an oblique angle (i.e. is not 90° or a multiple of 90°).
        /// </summary>
        public bool IsOblique
        {
            get { return WrapPositive(this).radians != MathUtil.PiOverTwo; }
        }

        /// <summary>
        /// Gets a System.Boolean that determines whether this SharpDX.Angle
        /// is an acute angle (i.e. less than 90° but greater than 0°).
        /// </summary>
        public bool IsAcute
        {
            get { return radians > 0.0 && radians < MathUtil.PiOverTwo; }
        }

        /// <summary>
        /// Gets a System.Boolean that determines whether this SharpDX.Angle
        /// is an obtuse angle (i.e. greater than 90° but less than 180°).
        /// </summary>
        public bool IsObtuse
        {
            get { return radians > MathUtil.PiOverTwo && radians < MathUtil.Pi; }
        }

        /// <summary>
        /// Gets a System.Boolean that determines whether this SharpDX.Angle
        /// is a reflex angle (i.e. greater than 180° but less than 360°).
        /// </summary>
        public bool IsReflex
        {
            get { return radians > MathUtil.Pi && radians < MathUtil.TwoPi; }
        }

        /// <summary>
        /// Gets a SharpDX.AngleSingle instance that complements this angle (i.e. the two angles add to 90°).
        /// </summary>
        public AngleSingle Complement
        {
            get { return new AngleSingle(MathUtil.PiOverTwo - radians, AngleType.Radian); }
        }

        /// <summary>
        /// Gets a SharpDX.AngleSingle instance that supplements this angle (i.e. the two angles add to 180°).
        /// </summary>
        public AngleSingle Supplement
        {
            get { return new AngleSingle(MathUtil.Pi - radians, AngleType.Radian); }
        }

        /// <summary>
        /// Wraps the SharpDX.AngleSingle given in the value argument to be in the range [π, -π].
        /// </summary>
        /// <param name="value">A SharpDX.AngleSingle to wrap.</param>
        /// <returns>The SharpDX.AngleSingle that is wrapped.</returns>
        public static AngleSingle Wrap(AngleSingle value)
        {
            value.Wrap();
            return value;
        }

        /// <summary>
        /// Wraps the SharpDX.AngleSingle given in the value argument to be in the range [0, 2π).
        /// </summary>
        /// <param name="value">A SharpDX.AngleSingle to wrap.</param>
        /// <returns>The SharpDX.AngleSingle that is wrapped.</returns>
        public static AngleSingle WrapPositive(AngleSingle value)
        {
            value.WrapPositive();
            return value;
        }

        /// <summary>
        /// Compares two SharpDX.AngleSingle instances and returns the smaller angle.
        /// </summary>
        /// <param name="left">The first SharpDX.AngleSingle instance to compare.</param>
        /// <param name="right">The second SharpDX.AngleSingle instance to compare.</param>
        /// <returns>The smaller of the two given SharpDX.AngleSingle instances.</returns>
        public static AngleSingle Min(AngleSingle left, AngleSingle right)
        {
            if (left.radians < right.radians)
                return left;

            return right;
        }

        /// <summary>
        /// Compares two SharpDX.AngleSingle instances and returns the greater angle.
        /// </summary>
        /// <param name="left">The first SharpDX.AngleSingle instance to compare.</param>
        /// <param name="right">The second SharpDX.AngleSingle instance to compare.</param>
        /// <returns>The greater of the two given SharpDX.AngleSingle instances.</returns>
        public static AngleSingle Max(AngleSingle left, AngleSingle right)
        {
            if (left.radians > right.radians)
                return left;

            return right;
        }

        /// <summary>
        /// Adds two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The first object to add.</param>
        /// <param name="right">The second object to add.</param>
        /// <returns>The value of the two objects added together.</returns>
        public static AngleSingle Add(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians + right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Subtracts two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The first object to subtract.</param>
        /// <param name="right">The second object to subtract.</param>
        /// <returns>The value of the two objects subtracted.</returns>
        public static AngleSingle Subtract(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians - right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Multiplies two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The first object to multiply.</param>
        /// <param name="right">The second object to multiply.</param>
        /// <returns>The value of the two objects multiplied together.</returns>
        public static AngleSingle Multiply(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians * right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Divides two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The numerator object.</param>
        /// <param name="right">The denominator object.</param>
        /// <returns>The value of the two objects divided.</returns>
        public static AngleSingle Divide(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians / right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Gets a new SharpDX.AngleSingle instance that represents the zero angle (i.e. 0°).
        /// </summary>
        public static AngleSingle ZeroAngle
        {
            get { return new AngleSingle(0.0f, AngleType.Radian); }
        }

        /// <summary>
        /// Gets a new SharpDX.AngleSingle instance that represents the right angle (i.e. 90° or π/2).
        /// </summary>
        public static AngleSingle RightAngle
        {
            get { return new AngleSingle(MathUtil.PiOverTwo, AngleType.Radian); }
        }

        /// <summary>
        /// Gets a new SharpDX.AngleSingle instance that represents the straight angle (i.e. 180° or π).
        /// </summary>
        public static AngleSingle StraightAngle
        {
            get { return new AngleSingle(MathUtil.Pi, AngleType.Radian); }
        }

        /// <summary>
        /// Gets a new SharpDX.AngleSingle instance that represents the full rotation angle (i.e. 360° or 2π).
        /// </summary>
        public static AngleSingle FullRotationAngle
        {
            get { return new AngleSingle(MathUtil.TwoPi, AngleType.Radian); }
        }

        /// <summary>
        /// Returns a System.Boolean that indicates whether the values of two SharpDX.Angle
        /// objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the left and right parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(AngleSingle left, AngleSingle right)
        {
            return left.radians == right.radians;
        }

        /// <summary>
        /// Returns a System.Boolean that indicates whether the values of two SharpDX.Angle
        /// objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the left and right parameters do not have the same value; otherwise, false.</returns>
        public static bool operator !=(AngleSingle left, AngleSingle right)
        {
            return left.radians != right.radians;
        }

        /// <summary>
        /// Returns a System.Boolean that indicates whether a SharpDX.Angle
        /// object is less than another SharpDX.AngleSingle object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if left is less than right; otherwise, false.</returns>
        public static bool operator <(AngleSingle left, AngleSingle right)
        {
            return left.radians < right.radians;
        }

        /// <summary>
        /// Returns a System.Boolean that indicates whether a SharpDX.Angle
        /// object is greater than another SharpDX.AngleSingle object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if left is greater than right; otherwise, false.</returns>
        public static bool operator >(AngleSingle left, AngleSingle right)
        {
            return left.radians > right.radians;
        }

        /// <summary>
        /// Returns a System.Boolean that indicates whether a SharpDX.Angle
        /// object is less than or equal to another SharpDX.AngleSingle object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if left is less than or equal to right; otherwise, false.</returns>
        public static bool operator <=(AngleSingle left, AngleSingle right)
        {
            return left.radians <= right.radians;
        }

        /// <summary>
        /// Returns a System.Boolean that indicates whether a SharpDX.Angle
        /// object is greater than or equal to another SharpDX.AngleSingle object.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if left is greater than or equal to right; otherwise, false.</returns>
        public static bool operator >=(AngleSingle left, AngleSingle right)
        {
            return left.radians >= right.radians;
        }

        /// <summary>
        /// Returns the value of the SharpDX.AngleSingle operand. (The sign of
        /// the operand is unchanged.)
        /// </summary>
        /// <param name="value">A SharpDX.AngleSingle object.</param>
        /// <returns>The value of the value parameter.</returns>
        public static AngleSingle operator +(AngleSingle value)
        {
            return value;
        }

        /// <summary>
        /// Returns the the negated value of the SharpDX.AngleSingle operand.
        /// </summary>
        /// <param name="value">A SharpDX.AngleSingle object.</param>
        /// <returns>The negated value of the value parameter.</returns>
        public static AngleSingle operator -(AngleSingle value)
        {
            return new AngleSingle(-value.radians, AngleType.Radian);
        }

        /// <summary>
        /// Adds two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The first object to add.</param>
        /// <param name="right">The second object to add.</param>
        /// <returns>The value of the two objects added together.</returns>
        public static AngleSingle operator +(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians + right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Subtracts two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The first object to subtract</param>
        /// <param name="right">The second object to subtract.</param>
        /// <returns>The value of the two objects subtracted.</returns>
        public static AngleSingle operator -(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians - right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Multiplies two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The first object to multiply.</param>
        /// <param name="right">The second object to multiply.</param>
        /// <returns>The value of the two objects multiplied together.</returns>
        public static AngleSingle operator *(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians * right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Divides two SharpDX.AngleSingle objects and returns the result.
        /// </summary>
        /// <param name="left">The numerator object.</param>
        /// <param name="right">The denominator object.</param>
        /// <returns>The value of the two objects divided.</returns>
        public static AngleSingle operator /(AngleSingle left, AngleSingle right)
        {
            return new AngleSingle(left.radians / right.radians, AngleType.Radian);
        }

        /// <summary>
        /// Compares this instance to a specified object and returns an integer that
        /// indicates whether the value of this instance is less than, equal to, or greater
        /// than the value of the specified object.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relationship of the current instance
        /// to the obj parameter. If the value is less than zero, the current instance
        /// is less than the other. If the value is zero, the current instance is equal
        /// to the other. If the value is greater than zero, the current instance is
        /// greater than the other.
        /// </returns>
        public int CompareTo(object other)
        {
            if (other == null)
                return 1;

            if (!(other is AngleSingle))
                throw new ArgumentException("Argument must be of type Angle.", "other");

            float radians = ((AngleSingle)other).radians;

            if (this.radians > radians)
                return 1;

            if (this.radians < radians)
                return -1;

            return 0;
        }

        /// <summary>
        /// Compares this instance to a second SharpDX.AngleSingle and returns
        /// an integer that indicates whether the value of this instance is less than,
        /// equal to, or greater than the value of the specified object.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relationship of the current instance
        /// to the obj parameter. If the value is less than zero, the current instance
        /// is less than the other. If the value is zero, the current instance is equal
        /// to the other. If the value is greater than zero, the current instance is
        /// greater than the other.
        /// </returns>
        public int CompareTo(AngleSingle other)
        {
            if (this.radians > other.radians)
                return 1;

            if (this.radians < other.radians)
                return -1;

            return 0;
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance and a specified
        /// SharpDX.AngleSingle object have the same value.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns>
        /// Returns true if this SharpDX.AngleSingle object and another have the same value;
        /// otherwise, false.
        /// </returns>
        public bool Equals(AngleSingle other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, MathUtil.RadiansToDegrees(radians).ToString("0.##°"));
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

            return string.Format(CultureInfo.CurrentCulture, "{0}°", MathUtil.RadiansToDegrees(radians).ToString(format, CultureInfo.CurrentCulture));
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
            return string.Format(formatProvider, MathUtil.RadiansToDegrees(radians).ToString("0.##°"));
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

            return string.Format(formatProvider, "{0}°", MathUtil.RadiansToDegrees(radians).ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a hash code for this SharpDX.AngleSingle instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return radiansInt;
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance and a specified
        /// object have the same value.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// Returns true if the obj parameter is a SharpDX.AngleSingle object or a type
        /// capable of implicit conversion to a SharpDX.AngleSingle value, and
        /// its value is equal to the value of the current SharpDX.Angle
        /// object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is AngleSingle) && (this == (AngleSingle)obj);
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Write optimized version without using Serialize methods
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(radiansInt);
            }
            else
            {
                radiansInt = serializer.Reader.ReadInt32();
            }
        }
    }
}
