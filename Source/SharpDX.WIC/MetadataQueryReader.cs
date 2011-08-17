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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpDX.Win32;

namespace SharpDX.WIC
{
    public partial class MetadataQueryReader
    {
        /// <summary>
        /// Gets the enumerator on the metadata names.
        /// </summary>
        public IEnumerable<string> Enumerator
        {
            get { return new ComStringEnumerator(GetEnumerator()); }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <unmanaged>HRESULT IWICMetadataQueryReader::GetLocation([In] unsigned int cchMaxLength,[InOut, Buffer, Optional] wchar_t* wzNamespace,[Out] unsigned int* pcchActualLength)</unmanaged>
        public string Location
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetLocation(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetLocation(count, (IntPtr)temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }

        /// <summary>
        /// Gets the metadata value by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Value of the metadata</returns>
        /// <unmanaged>HRESULT IWICMetadataQueryReader::GetMetadataByName([In] const wchar_t* wzName,[InOut, Optional] PROPVARIANT* pvarValue)</unmanaged>
        public object GetMetadataByName(string name)
        {
            unsafe
            {
                byte* variant = stackalloc byte[512];
                var pointer = new IntPtr(variant);
                GetMetadataByName(name, pointer);
                var value = Marshal.GetObjectForNativeVariant(pointer);

                // If object is a ComObject, try to instantiate a MetaDataQueryReader
                if (value is MarshalByRefObject)
                {
                    var temp = new ComObject(Marshal.GetIUnknownForObject(value));
                    try
                    {
                        value = temp.QueryInterface<MetadataQueryReader>();
                    } catch (Exception ex)
                    {
                        return value;
                    }
                }
                return value;
            }
        }
    }
}