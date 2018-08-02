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

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Reflection;

namespace SharpDX.MediaFoundation
{
    public partial class MediaAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributes"/> class.
        /// </summary>
        /// <param name="initialSizeInBytes">The initial number of elements allocated for the attribute store. The attribute store grows as needed. Default is 0</param>
        /// <remarks>	
        /// <p>Attributes are used throughout Microsoft Media Foundation to configure objects, describe media formats, query object properties, and other purposes. For more information, see Attributes in Media Foundation.</p><p>For a complete list of all the defined attribute GUIDs in Media Foundation, see Media Foundation Attributes.</p>	
        /// </remarks>	
        /// <msdn-id>ms701878</msdn-id>
        /// <unmanaged>HRESULT MFCreateAttributes([Out] IMFAttributes** ppMFAttributes,[In] unsigned int cInitialSize)</unmanaged>
        /// <unmanaged-short>MFCreateAttributes</unmanaged-short>
        public MediaAttributes(int initialSizeInBytes = 0)
            : base(IntPtr.Zero)
        {
            MediaFactory.CreateAttributes(this, initialSizeInBytes);
        }

        /// <summary>	
        /// Gets an item value
        /// </summary>	
        /// <param name="guidKey">GUID of the key.</param>	
        /// <returns>The value associated to this key.</returns>	
        /// <msdn-id>ms704598</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::GetItem([In] const GUID&amp; guidKey,[In] void* pValue)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::GetItem</unmanaged-short>	
        public object Get(System.Guid guidKey)
        {
            var itemType = GetItemType(guidKey);
            switch (itemType)
            {
                case AttributeType.UInt32:
                    return Get<int>(guidKey);
                case AttributeType.UInt64:
                    return Get<long>(guidKey);
                case AttributeType.Double:
                    return Get<double>(guidKey);
                case AttributeType.Guid:
                    return Get<Guid>(guidKey);
                case AttributeType.Blob:
                    return Get<byte[]>(guidKey);
                case AttributeType.String:
                    return Get<string>(guidKey);
                case AttributeType.IUnknown:
                    return Get<ComObject>(guidKey);
            }
            throw new ArgumentException("The type of the value is not supported");
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Retrieves an attribute at the specified index.</p>	
        /// </summary>	
        /// <param name="index"><dd> <p>Index of the attribute to retrieve. To get the number of attributes, call <strong><see cref="SharpDX.MediaFoundation.MediaAttributes.GetCount"/></strong>.</p> </dd></param>	
        /// <param name="guidKey"><dd> <p>Receives the <see cref="System.Guid"/> that identifies this attribute.</p> </dd></param>	
        /// <returns>The value associated to this index</returns>	
        /// <remarks>	
        /// <p>To enumerate all of an object's attributes in a thread-safe way, do the following:</p><ol> <li> <p>Call <strong><see cref="SharpDX.MediaFoundation.MediaAttributes.LockStore"/></strong> to prevent another thread from adding or deleting attributes.</p> </li> <li> <p>Call <strong><see cref="SharpDX.MediaFoundation.MediaAttributes.GetCount"/></strong> to find the number of attributes.</p> </li> <li> <p>Call <strong>GetItemByIndex</strong> to get each attribute by index.</p> </li> <li> <p>Call <strong><see cref="SharpDX.MediaFoundation.MediaAttributes.UnlockStore"/></strong> to unlock the attribute store.</p> </li> </ol><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>bb970331</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::GetItemByIndex([In] unsigned int unIndex,[Out] GUID* pguidKey,[InOut, Optional] PROPVARIANT* pValue)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::GetItemByIndex</unmanaged-short>	
        public object GetByIndex(int index, out System.Guid guidKey)
        {
            GetItemByIndex(index, out guidKey, IntPtr.Zero);
            return Get(guidKey);
        }

        /// <summary>	
        /// Gets an item value
        /// </summary>	
        /// <param name="guidKey">GUID of the key.</param>	
        /// <returns>The value associated to this key.</returns>	
        /// <msdn-id>ms704598</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::GetItem([In] const GUID&amp; guidKey,[In] void* pValue)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::GetItem</unmanaged-short>	
        public unsafe T Get<T>(System.Guid guidKey)
        {
            // Perform conversions to supported types
            // int
            // long
            // string
            // byte[]
            // double
            // ComObject
            // Guid

            if (typeof(T) == typeof(int) || typeof(T) == typeof(bool) || typeof(T) == typeof(byte) || typeof(T) == typeof(uint) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
            {
                return (T)Convert.ChangeType(GetInt(guidKey), typeof(T));
            }

            if (typeof(T).GetTypeInfo().IsEnum )
            {
                return (T)Enum.ToObject(typeof(T), GetInt(guidKey));
            }

            if (typeof(T) == typeof(IntPtr))
            {
                return (T)(object)new IntPtr(GetLong(guidKey));
            }

            if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                return (T)Convert.ChangeType(GetLong(guidKey), typeof(T));
            }

            if (typeof(T) == typeof(System.Guid))
            {
                return (T)(object)GetGUID(guidKey);
            }

            if (typeof(T) == typeof(string))
            {
                int length = GetStringLength(guidKey);
                char* wstr = stackalloc char[length + 1];
                GetString(guidKey, new IntPtr(wstr), length + 1, IntPtr.Zero);
                return (T)(object)Marshal.PtrToStringUni(new IntPtr(wstr));
            }

            if (typeof(T) == typeof(double) || typeof(T) == typeof(float))
            {
                return (T)Convert.ChangeType(GetDouble(guidKey), typeof(T));
            }

            if (typeof(T) == typeof(byte[]))
            {
                int length = GetBlobSize(guidKey);
                var buffer = new byte[length];
                fixed (void *pBuffer = buffer)
                    GetBlob(guidKey, (IntPtr)pBuffer, buffer.Length, IntPtr.Zero);
                return (T)(object)buffer;
            }

            if (typeof(T).GetTypeInfo().IsValueType)
            {
                int length = GetBlobSize(guidKey);
                if ( length != SharpDX.Interop.SizeOf<T>())
                {
                    throw new ArgumentException("Size of the structure doesn't match the size of stored value");
                }
                var value = default(T);
                GetBlob(guidKey, (IntPtr)Interop.Fixed(ref value), SharpDX.Interop.SizeOf<T>(), IntPtr.Zero);
                return value;
            }

            if (typeof(T) == typeof(ComObject))
            {
                IntPtr ptr;
                GetUnknown(guidKey, Utilities.GetGuidFromType(typeof(IUnknown)), out ptr);
                return (T)(object)new ComObject(ptr);
            }

            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(ComObject)))
            {
                IntPtr ptr;
                GetUnknown(guidKey, Utilities.GetGuidFromType(typeof(T)), out ptr);
                return AsUnsafe<T>(ptr);
            }

            throw new ArgumentException("The type of the value is not supported");
        }

        /// <summary>	
        /// Gets an item value
        /// </summary>	
        /// <param name="guidKey">GUID of the key.</param>	
        /// <returns>The value associated to this key.</returns>	
        /// <msdn-id>ms704598</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::GetItem([In] const GUID&amp; guidKey,[In] void* pValue)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::GetItem</unmanaged-short>	
        public unsafe T Get<T>(MediaAttributeKey<T> guidKey)
        {
            return Get<T>(guidKey.Guid);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Adds an attribute value with a specified key. </p>	
        /// </summary>	
        /// <param name="guidKey"><dd> <p> A <see cref="System.Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value. </p> </dd></param>	
        /// <param name="value"><dd> <p> A <strong><see cref="SharpDX.Win32.Variant"/></strong> that contains the attribute value. The method copies the value. The <strong><see cref="SharpDX.Win32.Variant"/></strong> type must be one of the types listed in the <strong><see cref="SharpDX.MediaFoundation.AttributeType"/></strong> enumeration. </p> </dd></param>	
        /// <returns><p> The method returns an <strong><see cref="SharpDX.Result"/></strong>. Possible values include, but are not limited to, those in the following table. </p><table> <tr><th>Return code</th><th>Description</th></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.Result.Ok"/></strong></dt> </dl> </td><td> <p> The method succeeded. </p> </td></tr> <tr><td> <dl> <dt><strong>E_OUTOFMEMORY</strong></dt> </dl> </td><td> <p> Insufficient memory. </p> </td></tr> <tr><td> <dl> <dt><strong>MF_E_INVALIDTYPE</strong></dt> </dl> </td><td> <p> Invalid attribute type. </p> </td></tr> </table><p>?</p></returns>	
        /// <remarks>	
        /// <p> This method checks whether the <strong><see cref="SharpDX.Win32.Variant"/></strong> type is one of the attribute types defined in <strong><see cref="SharpDX.MediaFoundation.AttributeType"/></strong>, and fails if an unsupported type is used. However, this method does not check whether the <strong><see cref="SharpDX.Win32.Variant"/></strong> is the correct type for the specified attribute <see cref="System.Guid"/>. (There is no programmatic way to associate attribute GUIDs with property types.) For a list of Media Foundation attributes and their data types, see Media Foundation Attributes. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>bb970346</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::SetItem([In] const GUID&amp; guidKey,[In] const PROPVARIANT&amp; Value)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::SetItem</unmanaged-short>	
        public unsafe void Set<T>(System.Guid guidKey, T value)
        {
            // Perform conversions to supported types
            // int
            // long
            // string
            // byte[]
            // double
            // ComObject
            // Guid

            if (typeof(T) == typeof(int) || typeof(T) ==  typeof(bool) || typeof(T) == typeof(byte) || typeof(T) == typeof(uint) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte)
                || typeof(T).GetTypeInfo().IsEnum )
            {
                Set(guidKey, Convert.ToInt32(value));
                return;
            }

            if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                Set(guidKey, Convert.ToInt64(value));
                return;
            }

            if (typeof(T) == typeof(IntPtr))
            {
                Set(guidKey, ((IntPtr)(object)value).ToInt64());
                return;
            }

            if (typeof(T) == typeof(System.Guid))
            {
                Set(guidKey, (System.Guid)(object)value);
                return;
            }

            if (typeof(T) == typeof(string))
            {
                Set(guidKey, value.ToString());
                return;
            }

            if (typeof(T) == typeof(double) || typeof(T) == typeof(float))
            {
                Set(guidKey, Convert.ToDouble(value));
                return;
            }

            if (typeof(T) == typeof(byte[]))
            {
                var arrayValue = ((byte[])(object)value);
                fixed (void* pBuffer = arrayValue)
                    SetBlob(guidKey, (IntPtr)pBuffer, arrayValue.Length);
                return;
            }


            if (typeof(T).GetTypeInfo().IsValueType)
            {
                SetBlob(guidKey, (IntPtr)Interop.Fixed(ref value), SharpDX.Interop.SizeOf<T>());
                return;
            }

            if (typeof(T) == typeof(ComObject) || typeof(IUnknown).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                Set(guidKey, ((IUnknown)(object)value));
                return;
            }

            throw new ArgumentException("The type of the value is not supported");
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Adds an attribute value with a specified key. </p>	
        /// </summary>	
        /// <param name="guidKey"><dd> <p> A <see cref="System.Guid"/> that identifies the value to set. If this key already exists, the method overwrites the old value. </p> </dd></param>	
        /// <param name="value"><dd> <p> A <strong><see cref="SharpDX.Win32.Variant"/></strong> that contains the attribute value. The method copies the value. The <strong><see cref="SharpDX.Win32.Variant"/></strong> type must be one of the types listed in the <strong><see cref="SharpDX.MediaFoundation.AttributeType"/></strong> enumeration. </p> </dd></param>	
        /// <returns><p> The method returns an <strong><see cref="SharpDX.Result"/></strong>. Possible values include, but are not limited to, those in the following table. </p><table> <tr><th>Return code</th><th>Description</th></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.Result.Ok"/></strong></dt> </dl> </td><td> <p> The method succeeded. </p> </td></tr> <tr><td> <dl> <dt><strong>E_OUTOFMEMORY</strong></dt> </dl> </td><td> <p> Insufficient memory. </p> </td></tr> <tr><td> <dl> <dt><strong>MF_E_INVALIDTYPE</strong></dt> </dl> </td><td> <p> Invalid attribute type. </p> </td></tr> </table><p>?</p></returns>	
        /// <remarks>	
        /// <p> This method checks whether the <strong><see cref="SharpDX.Win32.Variant"/></strong> type is one of the attribute types defined in <strong><see cref="SharpDX.MediaFoundation.AttributeType"/></strong>, and fails if an unsupported type is used. However, this method does not check whether the <strong><see cref="SharpDX.Win32.Variant"/></strong> is the correct type for the specified attribute <see cref="System.Guid"/>. (There is no programmatic way to associate attribute GUIDs with property types.) For a list of Media Foundation attributes and their data types, see Media Foundation Attributes. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>bb970346</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::SetItem([In] const GUID&amp; guidKey,[In] const PROPVARIANT&amp; Value)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::SetItem</unmanaged-short>	
        public unsafe void Set<T>(MediaAttributeKey<T> guidKey, T value)
        {
            Set(guidKey.Guid, value);
        }
    }
}