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
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// FunctionCallback
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class FunctionCallback
    {
        public IntPtr Pointer;

        public FunctionCallback(IntPtr pointer)
        {
            Pointer = pointer;
        }

        public unsafe FunctionCallback(void* pointer)
        {
            Pointer = new IntPtr(pointer);
        }

        public static explicit operator IntPtr(FunctionCallback value)
        {
            return value.Pointer;
        }

        public static implicit operator FunctionCallback(IntPtr value)
        {
            return new FunctionCallback(value);
        }

        public unsafe static implicit operator void*(FunctionCallback value)
        {
            return (void*)value.Pointer;
        }

        public static unsafe explicit operator FunctionCallback(void* value)
        {
            return new FunctionCallback(value);
        }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}", Pointer);
        }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <param name = "format">The format.</param>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "{0}", Pointer.ToString(format));
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Pointer.ToInt32();
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "FunctionCallback" /> is equal to this instance.
        /// </summary>
        /// <param name = "other">The <see cref = "FunctionCallback" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "FunctionCallback" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(FunctionCallback other)
        {
            return Pointer == other.Pointer;
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "value">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            if (!ReferenceEquals(value.GetType(), typeof(FunctionCallback)))
                return false;

            return Equals((FunctionCallback)value);
        }
    }
}
