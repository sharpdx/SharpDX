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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
    /// <summary>
    /// Implementation of OLE IPropertyBag2.
    /// </summary>
    /// <unmanaged>IPropertyBag2</unmanaged>
    public class PropertyBag : ComObject
    {
        private IPropertyBag2 nativePropertyBag;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        /// <param name="propertyBagPointer">The property bag pointer.</param>
        public PropertyBag(IntPtr propertyBagPointer) : base(propertyBagPointer)
        {
        }

        protected override void NativePointerUpdated(IntPtr oldNativePointer)
        {
            base.NativePointerUpdated(oldNativePointer);
            if (NativePointer != IntPtr.Zero)
                nativePropertyBag = (IPropertyBag2)Marshal.GetObjectForIUnknown(NativePointer);
            else
                nativePropertyBag = null;
        }

        private void CheckIfInitialized()
        {
            if (nativePropertyBag == null)
                throw new InvalidOperationException("This instance is not bound to an unmanaged IPropertyBag2");
        }

        /// <summary>
        /// Gets the number of properties.
        /// </summary>
        public int Count
        {
            get
            {
                CheckIfInitialized();
                int propertyCount;
                nativePropertyBag.CountProperties(out propertyCount);
                return propertyCount;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        public string[] Keys
        {
            get
            {
                CheckIfInitialized();
                var keys = new List<string>();
                for (int i = 0; i < Count; i++)
                {
                    PROPBAG2 propbag2;
                    int temp;
                    nativePropertyBag.GetPropertyInfo(i, 1, out propbag2, out temp);
                    keys.Add(propbag2.Name);
                }
                return keys.ToArray();
            }
        }

        /// <summary>
        /// Gets the value of the property with this name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Value of the property</returns>
        public object Get(string name)
        {
            CheckIfInitialized();
            object value;
            var propbag2 = new PROPBAG2() {Name = name};
            Result error;
            // Gets the property
            var result = nativePropertyBag.Read(1, ref propbag2, IntPtr.Zero, out value, out error);
            if (result.Failure || error.Failure)
                throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Property with name [{0}] is not valid for this instance", name));
            propbag2.Dispose();
            return value;
        }

        /// <summary>
        /// Gets the value of the property by using a <see cref="PropertyBagKey{T1,T2}"/>
        /// </summary>
        /// <typeparam name="T1">The public type of this property.</typeparam>
        /// <typeparam name="T2">The marshaling type of this property.</typeparam>
        /// <param name="propertyKey">The property key.</param>
        /// <returns>Value of the property</returns>
        public T1 Get<T1, T2>(PropertyBagKey<T1, T2> propertyKey)
        {
            var value = Get(propertyKey.Name);
            return (T1) Convert.ChangeType(value, typeof (T1));
        }

        /// <summary>
        /// Sets the value of the property with this name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Set(string name, object value)
        {
            CheckIfInitialized();
            // In order to set a property in the property bag
            // we need to convert the value to the destination type
            var previousValue = Get(name);
            value = Convert.ChangeType(value, previousValue==null?value.GetType() : previousValue.GetType());

            // Set the property
            var propbag2 = new PROPBAG2() { Name = name };
            var result = nativePropertyBag.Write(1, ref propbag2, value);
            result.CheckError();
            propbag2.Dispose();
        }

        /// <summary>
        /// Sets the value of the property by using a <see cref="PropertyBagKey{T1,T2}"/>
        /// </summary>
        /// <typeparam name="T1">The public type of this property.</typeparam>
        /// <typeparam name="T2">The marshaling type of this property.</typeparam>
        /// <param name="propertyKey">The property key.</param>
        /// <param name="value">The value.</param>
        public void Set<T1,T2>(PropertyBagKey<T1,T2> propertyKey, T1 value)
        {
            Set(propertyKey.Name, value);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct PROPBAG2 : IDisposable
        {
            internal uint type;
            internal ushort vt;
            internal ushort cfType;
            internal IntPtr dwHint;
            internal IntPtr pstrName;
            internal Guid clsid;

            public string Name
            {
                get
                {
                    unsafe
                    {
                        return Marshal.PtrToStringUni(pstrName);
                    }
                }
                set
                {
                    pstrName = Marshal.StringToCoTaskMemUni(value);
                }
            }

            public void Dispose()
            {
                if (pstrName != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pstrName);
                    pstrName = IntPtr.Zero;
                }
            }
        }
        
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("22F55882-280B-11D0-A8A9-00A0C90C2004")]
        private interface IPropertyBag2
        {
            [PreserveSig()]
            Result Read([In] int cProperties, [In] ref PROPBAG2 pPropBag, IntPtr pErrLog, [Out] out object pvarValue, out Result phrError);
            [PreserveSig()]
            Result Write([In] int cProperties, [In] ref PROPBAG2 pPropBag, ref object value);
            [PreserveSig()]
            Result CountProperties(out int pcProperties);
            [PreserveSig()]
            Result GetPropertyInfo([In] int iProperty, [In] int cProperties, out PROPBAG2 pPropBag, out int pcProperties);
            [PreserveSig()]
            Result LoadObject([In, MarshalAs(UnmanagedType.LPWStr)] string pstrName, [In] uint dwHint, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnkObject, IntPtr pErrLog);
        }
    }
}