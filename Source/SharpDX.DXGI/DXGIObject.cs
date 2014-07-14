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

namespace SharpDX.DXGI
{
    public partial class DXGIObject
    {
        /// <summary>
        /// Gets the parent of the object.
        /// </summary>
        /// <typeparam name="T">Type of the parent object</typeparam>
        /// <returns>Returns the parent object based on the GUID of the type of the parent object.</returns>
        /// <msdn-id>bb174542</msdn-id>
        /// <unmanaged>HRESULT IDXGIObject::GetParent([In] const GUID&amp; riid,[Out] void** ppParent)</unmanaged>
        /// <unmanaged-short>IDXGIObject::GetParent</unmanaged-short>
        public T GetParent<T>() where T : ComObject
        {
            IntPtr temp;
            this.GetParent(Utilities.GetGuidFromType(typeof (T)), out temp);
            return FromPointer<T>(temp);
        }
    }
}