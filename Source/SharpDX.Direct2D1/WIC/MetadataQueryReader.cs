// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.IO;

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

                    return new string(temp, 0, count-1);
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

                var varianStruct = (Variant*)variant;
                object value = varianStruct->Value;

                // If object is a ComObject, try to instantiate a MetaDataQueryReader
                if (value is ComObject)
                    value = ((ComObject)value).QueryInterfaceOrNull<MetadataQueryReader>();

                return value;
            }
        }

        /// <summary>
        /// Dumps all metadatas.
        /// </summary>
        /// <param name="writer">The text writer output.</param>
        /// <param name="level">The level of tabulations.</param>
        /// <remarks>
        /// This is a simple helper method to dump metadata stored in this instance.
        /// </remarks>
        public void Dump(TextWriter writer, int level = 0)
        {
            // See Native Image Format Metadata Queries : http://msdn.microsoft.com/en-us/library/windows/desktop/ee719904%28v=VS.85%29.aspx
            foreach (var name in Enumerator)
            {
                var value = GetMetadataByName(name);
                for (int i = 0; i < level; i++)
                    writer.Write("    ");
                var valueStr = value is MetadataQueryReader ? "..." : "" + (value is Array ? Utilities.Join(",", ((Array)value).GetEnumerator()) : value is IntPtr ? string.Format("0x{0:X}", value) : value);

                writer.WriteLine("{0} = {1}", name, valueStr);

                if (value is MetadataQueryReader)
                    ((MetadataQueryReader)value).Dump(writer, level + 1);
            }
        }            
    }
}