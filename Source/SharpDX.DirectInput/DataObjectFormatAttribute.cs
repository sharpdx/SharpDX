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

namespace SharpDX.DirectInput
{
    /// <summary>
    /// Attribute to describe a custom format for a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DataObjectFormatAttribute : Attribute
    {
        public DataObjectFormatAttribute()
        {
            Flags = ObjectDataFormatFlags.None;
            InstanceNumber = 0;
            Guid = "";
            TypeFlags = DeviceObjectTypeFlags.All;
        }

        public DataObjectFormatAttribute(string guid, DeviceObjectTypeFlags typeFlags)
        {
            Guid = guid;
            TypeFlags = typeFlags;
            Flags = ObjectDataFormatFlags.None;
            InstanceNumber = 0;
            ArrayCount = 0;
        }

        public DataObjectFormatAttribute(string guid, DeviceObjectTypeFlags typeFlags, ObjectDataFormatFlags flags)
        {
            Guid = guid;
            TypeFlags = typeFlags;
            Flags = flags;
        }

        public DataObjectFormatAttribute(string guid, DeviceObjectTypeFlags typeFlags, ObjectDataFormatFlags flags, int instanceNumber)
        {
            Guid = guid;
            TypeFlags = typeFlags;
            Flags = flags;
            InstanceNumber = instanceNumber;
        }

        public DataObjectFormatAttribute(string guid, int arrayCount, DeviceObjectTypeFlags typeFlags, ObjectDataFormatFlags flags)
        {
            Guid = guid;
            ArrayCount = arrayCount;
            TypeFlags = typeFlags;
            Flags = flags;
        }

        public DataObjectFormatAttribute(string guid, int arrayCount, DeviceObjectTypeFlags typeFlags)
        {
            Guid = guid;
            ArrayCount = arrayCount;
            TypeFlags = typeFlags;
            Flags = ObjectDataFormatFlags.None;
        }

        public DataObjectFormatAttribute(DeviceObjectTypeFlags typeFlags)
        {
            TypeFlags = typeFlags;
            Flags = ObjectDataFormatFlags.None;
            InstanceNumber = 0;
            ArrayCount = 0;
        }

        public DataObjectFormatAttribute(DeviceObjectTypeFlags typeFlags, ObjectDataFormatFlags flags)
        {
            TypeFlags = typeFlags;
            Flags = flags;
        }

        public DataObjectFormatAttribute(DeviceObjectTypeFlags typeFlags, ObjectDataFormatFlags flags, int instanceNumber)
        {
            TypeFlags = typeFlags;
            Flags = flags;
            InstanceNumber = instanceNumber;
        }

        public DataObjectFormatAttribute(int arrayCount, DeviceObjectTypeFlags typeFlags)
        {
            ArrayCount = arrayCount;
            TypeFlags = typeFlags;
            Flags = ObjectDataFormatFlags.None;
        }

        public DataObjectFormatAttribute(int arrayCount, DeviceObjectTypeFlags typeFlags, ObjectDataFormatFlags flags)
        {
            ArrayCount = arrayCount;
            TypeFlags = typeFlags;
            Flags = flags;
        }

        public DataObjectFormatAttribute(int arrayCount, DeviceObjectTypeFlags typeFlags, ObjectDataFormatFlags flags, int instanceNumber)
        {
            ArrayCount = arrayCount;
            TypeFlags = typeFlags;
            Flags = flags;
            InstanceNumber = instanceNumber;
        }


        public DataObjectFormatAttribute(int arrayCount, DeviceObjectTypeFlags typeFlags, int instanceNumber)
        {
            ArrayCount = arrayCount;
            TypeFlags = typeFlags;
            Flags = ObjectDataFormatFlags.None;
            InstanceNumber = instanceNumber;
        }

        /// <summary>
        /// Gets or sets the name of the field. Default is using the field name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets or sets Guid for the axis, button, or other input source. When requesting a data format, making this member empty indicates that any type of object is permissible. 
        /// </summary>
        /// <value>The GUID.</value>
        public string Guid;

        /// <summary>
        /// Gets or sets the array count.
        /// </summary>
        /// <value>The array count.</value>
        public int ArrayCount;

        /// <summary>
        /// Gets or sets the device type that describes the object. 
        /// </summary>
        /// <value>The type.</value>
        public DeviceObjectTypeFlags TypeFlags;

        /// <summary>
        /// Gets or sets the extra flags used to describe the data format.
        /// </summary>
        /// <value>The flags.</value>
        public ObjectDataFormatFlags Flags;

        /// <summary>
        /// Gets or sets the instance number.
        /// </summary>
        /// <value>The instance number.</value>
        public int InstanceNumber;
    }
}