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
    public partial class FontCollection
    {
        /// <summary>	
        /// Creates a font collection using a custom font collection loader. 	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="collectionLoader">An application-defined font collection loader, which must have been previously registered using <see cref="Factory.RegisterFontCollectionLoader_"/>. </param>
        /// <param name="collectionKey">The key used by the loader to identify a collection of font files.  The buffer allocated for this key should at least be the size of collectionKeySize. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateCustomFontCollection([None] IDWriteFontCollectionLoader* collectionLoader,[In, Buffer] const void* collectionKey,[None] int collectionKeySize,[Out] IDWriteFontCollection** fontCollection)</unmanaged>
        public FontCollection(Factory factory, FontCollectionLoader collectionLoader, DataPointer collectionKey)
        {
            factory.CreateCustomFontCollection(collectionLoader, collectionKey.Pointer, collectionKey.Size, this);
        }
    }
}