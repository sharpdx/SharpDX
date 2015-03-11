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
    public partial class LocalFontFileLoader
    {
        /// <summary>	
        /// <p>Obtains the absolute font file path from the font file reference key.</p>	
        /// </summary>	
        /// <param name="referenceKey"><dd> <p>The font file reference key that uniquely identifies the local font file within the scope of the font loader being used.</p> </dd></param>	
        /// <returns><p>If this method succeeds, the absolute font file path from the font file reference key.</p></returns>	
        /// <msdn-id>dd371241</msdn-id>	
        /// <unmanaged>HRESULT IDWriteLocalFontFileLoader::GetFilePathFromKey([In, Buffer] const void* fontFileReferenceKey,[In] unsigned int fontFileReferenceKeySize,[Out, Buffer] wchar_t* filePath,[In] unsigned int filePathSize)</unmanaged>	
        /// <unmanaged-short>IDWriteLocalFontFileLoader::GetFilePathFromKey</unmanaged-short>	
        public unsafe string GetFilePath(DataPointer referenceKey)
        {
            if (referenceKey.IsEmpty)
            {
                throw new ArgumentNullException("referenceKey", "DatePointer cannot be null");
            }

            var fileNameSize = GetFilePathLengthFromKey(referenceKey.Pointer, referenceKey.Size);
            var buffer = new char[fileNameSize + 1];
            fixed(void* pBuffer = buffer)
            {
                GetFilePathFromKey(referenceKey.Pointer, referenceKey.Size, new IntPtr(pBuffer), fileNameSize + 1 );
            }
            return new string(buffer, 0, fileNameSize);
        }

        /// <summary>	
        /// <p>Obtains the last write time of the file from the font file reference key.</p>	
        /// </summary>	
        /// <param name="referenceKey"><dd> <p>The font file reference key that uniquely identifies the local font file within the scope of the font loader being used.</p> </dd></param>	
        /// <returns><dd> <p>The time of the last font file modification.</p> </dd></returns>	
        /// <msdn-id>dd371247</msdn-id>	
        /// <unmanaged>HRESULT IDWriteLocalFontFileLoader::GetLastWriteTimeFromKey([In, Buffer] const void* fontFileReferenceKey,[In] unsigned int fontFileReferenceKeySize,[Out] FILETIME* lastWriteTime)</unmanaged>	
        /// <unmanaged-short>IDWriteLocalFontFileLoader::GetLastWriteTimeFromKey</unmanaged-short>	
        public DateTime GetLastWriteTime(DataPointer referenceKey)
        {
            if (referenceKey.IsEmpty)
            {
                throw new ArgumentNullException("referenceKey", "DatePointer cannot be null");
            }

            var fileTime = GetLastWriteTimeFromKey(referenceKey.Pointer, referenceKey.Size);
            return DateTime.FromFileTime(fileTime);
        }
    }
}