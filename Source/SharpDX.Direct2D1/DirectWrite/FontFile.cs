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
    public partial class FontFile
    {
        private FontFileLoaderShadow fontLoaderShadow;

        /// <summary>	
        /// Creates a font file reference object from a local font file. 	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="filePath">An array of characters that contains the absolute file path for the font file. Subsequent operations on the constructed object may fail if the user provided filePath doesn't correspond to a valid file on the disk. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateFontFileReference([In] const wchar_t* filePath,[In, Optional] const __int64* lastWriteTime,[Out] IDWriteFontFile** fontFile)</unmanaged>
        public FontFile(Factory factory, string filePath) : this(factory, filePath, null) {
        }

        /// <summary>	
        /// Creates a font file reference object from a local font file. 	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="filePath">An array of characters that contains the absolute file path for the font file. Subsequent operations on the constructed object may fail if the user provided filePath doesn't correspond to a valid file on the disk. </param>
        /// <param name="lastWriteTime">The last modified time of the input file path. If the parameter is omitted, the function will access the font file to obtain its last write time. You should specify this value to avoid extra disk access. Subsequent operations on the constructed object may fail if the user provided lastWriteTime doesn't match the file on the disk. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateFontFileReference([In] const wchar_t* filePath,[In, Optional] const __int64* lastWriteTime,[Out] IDWriteFontFile** fontFile)</unmanaged>
        public FontFile(Factory factory, string filePath, long? lastWriteTime)
        {
            factory.CreateFontFileReference(filePath, lastWriteTime, this);
        }

        /// <summary>
        /// Creates a reference to an application-specific font file resource.
        /// </summary>
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="fontFileReferenceKey">A font file reference key that uniquely identifies the font file resource during the lifetime of fontFileLoader.</param>
        /// <param name="fontFileReferenceKeySize">The size of the font file reference key in bytes.</param>
        /// <param name="fontFileLoader">The font file loader that will be used by the font system to load data from the file identified by fontFileReferenceKey.</param>
        /// <remarks>
        /// This function is provided for cases when an application or a document needs to use a private font without having to install it on the system. fontFileReferenceKey has to be unique only in the scope of the fontFileLoader used in this call.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFactory::CreateCustomFontFileReference([In, Buffer] const void* fontFileReferenceKey,[None] int fontFileReferenceKeySize,[None] IDWriteFontFileLoader* fontFileLoader,[Out] IDWriteFontFile** fontFile)</unmanaged>
        public FontFile(Factory factory, IntPtr fontFileReferenceKey, int fontFileReferenceKeySize, FontFileLoader fontFileLoader)
        {
            factory.CreateCustomFontFileReference(fontFileReferenceKey, fontFileReferenceKeySize, fontFileLoader, this);
        }

        /// <summary>	
        /// Obtains the file loader associated with a font file object. 	
        /// </summary>	
        /// <unmanaged>HRESULT IDWriteFontFile::GetLoader([Out] IDWriteFontFileLoader** fontFileLoader)</unmanaged>
        public SharpDX.DirectWrite.FontFileLoader Loader
        {
            get
            {
                if (fontLoaderShadow != null)
                    return (FontFileLoader)fontLoaderShadow.Callback;
                
                SharpDX.DirectWrite.FontFileLoader __output__; 
                GetLoader(out __output__); 
                return __output__;
            }
        }

        /// <summary>	
        /// Obtains the reference to the reference key of a font file. The returned reference is valid until the font file object is released.  	
        /// </summary>	
        /// <returns>the reference to the reference key of a font file. </returns>
        /// <unmanaged>HRESULT IDWriteFontFile::GetReferenceKey([Out, Buffer] const void** fontFileReferenceKey,[Out] int* fontFileReferenceKeySize)</unmanaged>
        public DataPointer GetReferenceKey()
        {
            unsafe
            {
                int keySize;
                IntPtr keyPtr;
                GetReferenceKey(new IntPtr(&keyPtr), out keySize);
                return new DataPointer(keyPtr, keySize);
            }
        }
    }
}