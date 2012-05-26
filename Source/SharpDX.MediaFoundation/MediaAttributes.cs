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

namespace SharpDX.MediaFoundation
{
    public partial class MediaAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributes"/> class.
        /// </summary>
        /// <param name="initialSizeInBytes">The initial number of elements allocated for the attribute store. The attribute store grows as needed. Default is 0</param>
        /// <msdn-id>ms701878</msdn-id>
        /// <unmanaged>HRESULT MFCreateAttributes([Out] IMFAttributes** ppMFAttributes,[In] unsigned int cInitialSize)</unmanaged>
        /// <unmanaged-short>MFCreateAttributes</unmanaged-short>
        /// <remarks>	
        /// <p>Attributes are used throughout Microsoft Media Foundation to configure objects, describe media formats, query object properties, and other purposes. For more information, see Attributes in Media Foundation.</p><p>For a complete list of all the defined attribute GUIDs in Media Foundation, see Media Foundation Attributes.</p>	
        /// </remarks>	
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
        public unsafe object GetItem(System.Guid guidKey)
        {
            var value = new SharpDX.Win32.Variant();
            GetItem(guidKey, new IntPtr(&value));
            return value.Value;
        }

        /// <summary>	
        /// Gets an item value
        /// </summary>	
        /// <param name="guidKey">GUID of the key.</param>	
        /// <returns>The value associated to this key.</returns>	
        /// <msdn-id>ms704598</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::GetItem([In] const GUID&amp; guidKey,[In] void* pValue)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::GetItem</unmanaged-short>	
        public unsafe T GetItem<T>(System.Guid guidKey)
        {
            var value = new SharpDX.Win32.Variant();
            GetItem(guidKey, new IntPtr(&value));
            return (T)Convert.ChangeType(value.Value, typeof(T));
        }

        /// <summary>	
        /// Compares item value.	
        /// </summary>	
        /// <param name="guidKey">No documentation.</param>	
        /// <param name="value">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <msdn-id>ms704598</msdn-id>	
        /// <unmanaged>HRESULT IMFAttributes::CompareItem([In] const GUID&amp; guidKey,[In] const PROPVARIANT&amp; Value,[Out] BOOL* pbResult)</unmanaged>	
        /// <unmanaged-short>IMFAttributes::CompareItem</unmanaged-short>	
        public unsafe bool CompareItem(System.Guid guidKey, object value)
        {
            var variant = new SharpDX.Win32.Variant { Value = value };
            return CompareItem(guidKey, variant);
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
        public void SetItem(System.Guid guidKey, object value)
        {
            var variant = new SharpDX.Win32.Variant { Value = value };
            SetItem(guidKey, variant);
        }
    }
}