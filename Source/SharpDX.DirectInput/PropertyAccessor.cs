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
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertyAccessor
    {
        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>The device.</value>
        private Device Device { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        private int ObjectCode { get; set; }

        /// <summary>
        /// Gets or sets the how type.
        /// </summary>
        /// <value>The how type.</value>
        private PropertyHowType PropertyType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="code">The code.</param>
        /// <param name="propertyType">The property type.</param>
        internal PropertyAccessor(Device device, int code, PropertyHowType propertyType)
        {
            Device = device;
            ObjectCode = code;
            PropertyType = propertyType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor"/> class by offset inside a structure.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="name">The name of the property inside dataFormat type.</param>
        /// <param name="dataFormat">The data format.</param>
        internal PropertyAccessor( Device device, string name, Type dataFormat )
        {
            Device = device;
            ObjectCode = Marshal.OffsetOf(dataFormat, name).ToInt32();
            PropertyType = PropertyHowType.Byoffset;
	    }


        protected unsafe object GetObject(IntPtr guid)
        {
            // NOT WORKING with APPDATA
            var prop = new PropertyPointer();
            InitHeader<PropertyPointer>(ref prop.Header);
            IntPtr value = IntPtr.Zero;
            prop.Data = new IntPtr(&value);
            Device.GetProperty(guid, new IntPtr(&prop));

            if (prop.Data.ToInt64() == -1)
                return null;

            var handle = GCHandle.FromIntPtr(prop.Data);
            if (!handle.IsAllocated)
                return null;

            return handle.Target;
        }

        protected unsafe void SetObject(IntPtr guid, object value)
        {
            // NOT WORKING with APPDATA
            var prop = new PropertyPointer();
            InitHeader<PropertyPointer>(ref prop.Header);
            var dataValue = IntPtr.Zero;
            prop.Data = new IntPtr(&dataValue);

            // Free previous application data if any
            Device.GetProperty(guid, new IntPtr(&prop));

            GCHandle handle;
            if (prop.Data.ToInt64() != -1)
            {
                handle = GCHandle.FromIntPtr(prop.Data);
                if (handle.IsAllocated)
                    handle.Free();
            }

            // Set new object value
            handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            prop.Data = handle.AddrOfPinnedObject();
            Device.SetProperty(guid, new IntPtr(&prop));
        }

        protected int GetInt(IntPtr guid)
        {
            return GetInt(guid, ObjectCode);
        }

        protected unsafe int GetInt(IntPtr guid, int objCode)
        {
            var prop = new PropertyInt();
            InitHeader<PropertyInt>(ref prop.Header);
            prop.Header.Obj = objCode;
            Device.GetProperty(guid, new IntPtr(&prop));
            return prop.Data;
        }

        protected unsafe void Set(IntPtr guid, int value)
        {
            var prop = new PropertyInt();
            InitHeader<PropertyInt>(ref prop.Header);
            prop.Data = value;
            Device.SetProperty(guid, new IntPtr(&prop));
        }

        protected unsafe Guid GetGuid(IntPtr guid)
        {
            var propNative = new PropertyGuidAndPath.__Native();
            InitHeader<PropertyGuidAndPath.__Native>(ref propNative.Header);            
            Device.GetProperty(guid, new IntPtr(&propNative));
            return propNative.GuidClass;
        }

        protected unsafe string GetPath(IntPtr guid)
        {
            var prop = new PropertyGuidAndPath();
            var propNative = new PropertyGuidAndPath.__Native();
            InitHeader<PropertyGuidAndPath.__Native>(ref propNative.Header);
            Device.GetProperty(guid, new IntPtr(&propNative));
            prop.__MarshalFrom(ref propNative);
            return prop.Path;
        }

        protected string GetString(IntPtr guid)
        {
            return GetString(guid, ObjectCode);
        }

        protected unsafe string GetString(IntPtr guid, int objectCode)
        {
            var prop = new PropertyString();
            var propNative = new PropertyString.__Native();
            InitHeader<PropertyString.__Native>(ref propNative.Header);
            propNative.Header.Obj = objectCode;
            Device.GetProperty(guid, new IntPtr(&propNative));
            prop.__MarshalFrom(ref propNative);
            return prop.Text;
        }

        protected unsafe void Set(IntPtr guid, string value)
        {
            var prop = new PropertyString {Text = value};
            var propNative = new PropertyString.__Native();
            prop.__MarshalTo(ref propNative);
            InitHeader<PropertyString.__Native>(ref propNative.Header);
            Device.SetProperty(guid, new IntPtr(&propNative));
        }

        protected unsafe InputRange GetRange(IntPtr guid)
        {
            var prop = new PropertyRange();
            InitHeader<PropertyRange>(ref prop.Header);
            Device.GetProperty(guid, new IntPtr(&prop));
            return new InputRange(prop);
        }
        
        protected unsafe void Set(IntPtr guid, InputRange value)
        {
            var prop = new PropertyRange();
            InitHeader<PropertyRange>(ref prop.Header);
            value.CopyTo(ref prop);
            Device.SetProperty(guid, new IntPtr(&prop));
        }

        internal unsafe void InitHeader<T>(ref PropertyHeader header) where T : struct
        {
            header.Size = Utilities.SizeOf<T>();
            header.HeaderSize = sizeof(PropertyHeader);
            header.Type = PropertyType;
            header.Obj = ObjectCode;
        }
    }
}