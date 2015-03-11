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

namespace SharpDX.DirectWrite
{
    public partial class LocalizedStrings
    {
        /// <summary>	
        /// Get the locale name from the language.	
        /// </summary>	
        /// <param name="index">Zero-based index of the locale name to be retrieved. </param>
        /// <returns>The locale name from the language </returns>
        /// <unmanaged>HRESULT IDWriteLocalizedStrings::GetLocaleName([None] int index,[Out, Buffer] wchar_t* localeName,[None] int size)</unmanaged>
        public string GetLocaleName(int index)
        {
            unsafe
            {
                int localNameLength;
                GetLocaleNameLength(index, out localNameLength);
                char* localName = stackalloc char[localNameLength+1];
                GetLocaleName(index, new IntPtr(localName), localNameLength+1);
                return new string(localName, 0, localNameLength);
            }
        }

        /// <summary>	
        /// Get the string from the language/string pair.
        /// </summary>	
        /// <param name="index">Zero-based index of the string from the language/string pair to be retrieved. </param>
        /// <returns>The locale name from the language </returns>
        /// <unmanaged>HRESULT IDWriteLocalizedStrings::GetLocaleName([None] int index,[Out, Buffer] wchar_t* localeName,[None] int size)</unmanaged>
        public string GetString(int index)
        {
            unsafe
            {
                int stringNameLength;
                GetStringLength(index, out stringNameLength);
                char* stringName = stackalloc char[stringNameLength + 1];
                GetString(index, new IntPtr(stringName), stringNameLength+1);
                return new string(stringName, 0, stringNameLength);
            }
        }
    }
}