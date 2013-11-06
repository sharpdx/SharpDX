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
using System;
using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    /// <summary>The device object unique identifier struct.</summary>
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public partial struct DeviceObjectId
    {
        /// <summary>The _raw type.</summary>
        private int _rawType;

        /// <summary>The instance number maximum.</summary>
        private const int InstanceNumberMax = 0xFFFF - 1;

        /// <summary>Any instance mask.</summary>
        private const int AnyInstanceMask = 0x00FFFF00;

        /// <summary>Initializes a new instance of the <see cref="DeviceObjectId"/> struct.</summary>
        /// <param name="typeFlags">The type flags.</param>
        /// <param name="instanceNumber">The instance number.</param>
        public DeviceObjectId(DeviceObjectTypeFlags typeFlags, int instanceNumber) : this()
        {
            // Clear anyInstance flags and use instanceNumber
            _rawType = ((int)typeFlags & ~AnyInstanceMask) | ((instanceNumber < 0 | instanceNumber > InstanceNumberMax) ? 0 : ((instanceNumber & 0xFFFF) << 8));
        }

        /// <summary>Gets the flags.</summary>
        /// <value>The flags.</value>
        public DeviceObjectTypeFlags Flags
        {
            get
            {
                return (DeviceObjectTypeFlags) (_rawType & ~AnyInstanceMask);
            }
        }

        /// <summary>Gets the instance number.</summary>
        /// <value>The instance number.</value>
        public int InstanceNumber
        {
            get { return (_rawType >> 8) & 0xFFFF; }
        }

        /// <summary>Performs an explicit conversion from <see cref="DeviceObjectId"/> to <see cref="System.Int32"/>.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator int(DeviceObjectId type)
        {
            return type._rawType;
        }

        /// <summary>Equalses the specified other.</summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(DeviceObjectId other)
        {
            return other._rawType == _rawType;
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (DeviceObjectId)) return false;
            return Equals((DeviceObjectId) obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _rawType;
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Flags: {0} InstanceNumber: {1} RawId: 0x{2:X8}", Flags, InstanceNumber, _rawType);
        }
    }
}