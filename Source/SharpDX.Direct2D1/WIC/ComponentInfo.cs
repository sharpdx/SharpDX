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

namespace SharpDX.WIC
{
    public partial class ComponentInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentInfo"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="clsidComponent">The CLSID component.</param>
        public ComponentInfo(ImagingFactory factory, Guid clsidComponent) : base(IntPtr.Zero)
        {
            factory.CreateComponentInfo(clsidComponent, this);
        }

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <unmanaged>HRESULT IWICComponentInfo::GetAuthor([In] unsigned int cchAuthor,[InOut, Buffer, Optional] wchar_t* wzAuthor,[Out] unsigned int* pcchActual)</unmanaged>
        public string Author
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetAuthor(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetAuthor(count, (IntPtr) temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <unmanaged>HRESULT IWICComponentInfo::GetVersion([In] unsigned int cchVersion,[InOut, Buffer, Optional] wchar_t* wzVersion,[Out] unsigned int* pcchActual)</unmanaged>
        public string Version
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetVersion(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetVersion(count, (IntPtr) temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }

        /// <summary>
        /// Gets the spec version.
        /// </summary>
        /// <unmanaged>HRESULT IWICComponentInfo::GetSpecVersion([In] unsigned int cchSpecVersion,[InOut, Buffer, Optional] wchar_t* wzSpecVersion,[Out] unsigned int* pcchActual)</unmanaged>
        public string SpecVersion
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetSpecVersion(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetSpecVersion(count, (IntPtr) temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }

        /// <summary>
        /// Gets the friendly name.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        /// <unmanaged>HRESULT IWICComponentInfo::GetFriendlyName([In] unsigned int cchFriendlyName,[InOut, Buffer, Optional] wchar_t* wzFriendlyName,[Out] unsigned int* pcchActual)</unmanaged>
        public string FriendlyName
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetFriendlyName(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetFriendlyName(count, (IntPtr) temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }
    }
}