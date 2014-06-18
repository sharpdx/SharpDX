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
using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public partial struct DeviceObjectId
    {
        private int _rawType;

        private const int InstanceNumberMax = 0xFFFF - 1;
        private const int AnyInstanceMask = 0x00FFFF00;

        public DeviceObjectId(DeviceObjectTypeFlags typeFlags, int instanceNumber) : this()
        {
            // Clear anyInstance flags and use instanceNumber
            _rawType = ((int)typeFlags & ~AnyInstanceMask) | ((instanceNumber < 0 | instanceNumber > InstanceNumberMax) ? 0 : ((instanceNumber & 0xFFFF) << 8));
        }

        public DeviceObjectTypeFlags Flags
        {
            get
            {
                return (DeviceObjectTypeFlags) (_rawType & ~AnyInstanceMask);
            }
        }

        public int InstanceNumber
        {
            get { return (_rawType >> 8) & 0xFFFF; }
        }

        public static explicit operator int(DeviceObjectId type)
        {
            return type._rawType;
        }

        public bool Equals(DeviceObjectId other)
        {
            return other._rawType == _rawType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (DeviceObjectId)) return false;
            return Equals((DeviceObjectId) obj);
        }

        public override int GetHashCode()
        {
            return _rawType;
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Flags: {0} InstanceNumber: {1} RawId: 0x{2:X8}", Flags, InstanceNumber, _rawType);
        }
    }
}