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
    [ShadowAttribute(typeof(FontCollectionLoaderShadow))]
    public partial interface FontCollectionLoader
    {
        /// <summary>	
        /// Creates a font file enumerator object that encapsulates a collection of font files. The font system calls back to this interface to create a font collection. 	
        /// </summary>	
        /// <param name="factory">Pointer to the <see cref="SharpDX.DirectWrite.Factory"/> object that was used to create the current font collection. </param>
        /// <param name="collectionKey">A font collection key that uniquely identifies the collection of font files within the scope of the font collection loader being used. The buffer allocated for this key must be at least  the size, in bytes, specified by collectionKeySize. </param>
        /// <returns>a reference to the newly created font file enumerator.</returns>
        /// <unmanaged>HRESULT IDWriteFontCollectionLoader::CreateEnumeratorFromKey([None] IDWriteFactory* factory,[In, Buffer] const void* collectionKey,[None] int collectionKeySize,[Out] IDWriteFontFileEnumerator** fontFileEnumerator)</unmanaged>
        SharpDX.DirectWrite.FontFileEnumerator CreateEnumeratorFromKey(SharpDX.DirectWrite.Factory factory, DataPointer collectionKey);
    }
}