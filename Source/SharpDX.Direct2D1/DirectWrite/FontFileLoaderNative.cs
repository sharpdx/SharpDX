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
    public partial class FontFileLoaderNative
    {
        /// <summary>	
        /// Creates a font file stream object that encapsulates an open file resource. 	
        /// </summary>	
        /// <remarks>	
        /// The resource is closed when the last reference to fontFileStream is released.  	
        /// </remarks>	
        /// <param name="fontFileReferenceKey">A reference to a font file reference key that uniquely identifies the font file resource within the scope of the font loader being used. The buffer allocated for this key must at least be the size, in bytes, specified by  fontFileReferenceKeySize. </param>
        /// <returns>a reference to the newly created <see cref="SharpDX.DirectWrite.FontFileStream"/> object. </returns>
        /// <unmanaged>HRESULT IDWriteFontFileLoader::CreateStreamFromKey([In, Buffer] const void* fontFileReferenceKey,[None] int fontFileReferenceKeySize,[Out] IDWriteFontFileStream** fontFileStream)</unmanaged>
        public FontFileStream CreateStreamFromKey(DataPointer fontFileReferenceKey)
        {
            FontFileStream temp;
            CreateStreamFromKey_(fontFileReferenceKey.Pointer, fontFileReferenceKey.Size, out temp);
            return temp;
        }
    }
}