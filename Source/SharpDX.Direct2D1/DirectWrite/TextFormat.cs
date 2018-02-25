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
    public partial class TextFormat
    {

        /// <summary>	
        ///  Creates a text format object used for text layout with normal weight, style and stretch.
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="fontFamilyName">An array of characters that contains the name of the font family</param>
        /// <param name="fontSize">The logical size of the font in DIP ("device-independent pixel") units. A DIP equals 1/96 inch.</param>
        /// <unmanaged>HRESULT CreateTextFormat([In] const wchar* fontFamilyName,[None] IDWriteFontCollection* fontCollection,[None] DWRITE_FONT_WEIGHT fontWeight,[None] DWRITE_FONT_STYLE fontStyle,[None] DWRITE_FONT_STRETCH fontStretch,[None] FLOAT fontSize,[In] const wchar* localeName,[Out] IDWriteTextFormat** textFormat)</unmanaged>
        public TextFormat(Factory factory, string fontFamilyName, float fontSize)
            : this(factory, fontFamilyName, null, DirectWrite.FontWeight.Normal, DirectWrite.FontStyle.Normal, DirectWrite.FontStretch.Normal, fontSize, "")
        {
        }

        /// <summary>	
        ///  Creates a text format object used for text layout with normal stretch.
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="fontFamilyName">An array of characters that contains the name of the font family</param>
        /// <param name="fontWeight">A value that indicates the font weight for the text object created by this method.</param>
        /// <param name="fontStyle">A value that indicates the font style for the text object created by this method.</param>
        /// <param name="fontSize">The logical size of the font in DIP ("device-independent pixel") units. A DIP equals 1/96 inch.</param>
        /// <unmanaged>HRESULT CreateTextFormat([In] const wchar* fontFamilyName,[None] IDWriteFontCollection* fontCollection,[None] DWRITE_FONT_WEIGHT fontWeight,[None] DWRITE_FONT_STYLE fontStyle,[None] DWRITE_FONT_STRETCH fontStretch,[None] FLOAT fontSize,[In] const wchar* localeName,[Out] IDWriteTextFormat** textFormat)</unmanaged>
        public TextFormat(Factory factory, string fontFamilyName, SharpDX.DirectWrite.FontWeight fontWeight, SharpDX.DirectWrite.FontStyle fontStyle, float fontSize)
            : this(factory, fontFamilyName, null, fontWeight, fontStyle, DirectWrite.FontStretch.Normal, fontSize, "")
        {
        }

        /// <summary>	
        ///  Creates a text format object used for text layout. 	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="fontFamilyName">An array of characters that contains the name of the font family</param>
        /// <param name="fontWeight">A value that indicates the font weight for the text object created by this method.</param>
        /// <param name="fontStyle">A value that indicates the font style for the text object created by this method.</param>
        /// <param name="fontStretch">A value that indicates the font stretch for the text object created by this method.</param>
        /// <param name="fontSize">The logical size of the font in DIP ("device-independent pixel") units. A DIP equals 1/96 inch.</param>
        /// <unmanaged>HRESULT CreateTextFormat([In] const wchar* fontFamilyName,[None] IDWriteFontCollection* fontCollection,[None] DWRITE_FONT_WEIGHT fontWeight,[None] DWRITE_FONT_STYLE fontStyle,[None] DWRITE_FONT_STRETCH fontStretch,[None] FLOAT fontSize,[In] const wchar* localeName,[Out] IDWriteTextFormat** textFormat)</unmanaged>
        public TextFormat(Factory factory, string fontFamilyName, SharpDX.DirectWrite.FontWeight fontWeight, SharpDX.DirectWrite.FontStyle fontStyle, SharpDX.DirectWrite.FontStretch fontStretch, float fontSize)
            : this(factory, fontFamilyName, null, fontWeight, fontStyle, fontStretch, fontSize, "")
        {
        }

        /// <summary>	
        ///  Creates a text format object used for text layout. 	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="fontFamilyName">An array of characters that contains the name of the font family</param>
        /// <param name="fontCollection">A pointer to a font collection object. When this is NULL, indicates the system font collection.</param>
        /// <param name="fontWeight">A value that indicates the font weight for the text object created by this method.</param>
        /// <param name="fontStyle">A value that indicates the font style for the text object created by this method.</param>
        /// <param name="fontStretch">A value that indicates the font stretch for the text object created by this method.</param>
        /// <param name="fontSize">The logical size of the font in DIP ("device-independent pixel") units. A DIP equals 1/96 inch.</param>
        /// <unmanaged>HRESULT CreateTextFormat([In] const wchar* fontFamilyName,[None] IDWriteFontCollection* fontCollection,[None] DWRITE_FONT_WEIGHT fontWeight,[None] DWRITE_FONT_STYLE fontStyle,[None] DWRITE_FONT_STRETCH fontStretch,[None] FLOAT fontSize,[In] const wchar* localeName,[Out] IDWriteTextFormat** textFormat)</unmanaged>
        public TextFormat(Factory factory, string fontFamilyName, SharpDX.DirectWrite.FontCollection fontCollection, SharpDX.DirectWrite.FontWeight fontWeight, SharpDX.DirectWrite.FontStyle fontStyle, SharpDX.DirectWrite.FontStretch fontStretch, float fontSize)
            : this(factory, fontFamilyName, fontCollection, fontWeight, fontStyle, fontStretch, fontSize,"")
        {
        }

        /// <summary>	
        ///  Creates a text format object used for text layout. 	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="fontFamilyName">An array of characters that contains the name of the font family</param>
        /// <param name="fontCollection">A pointer to a font collection object. When this is NULL, indicates the system font collection.</param>
        /// <param name="fontWeight">A value that indicates the font weight for the text object created by this method.</param>
        /// <param name="fontStyle">A value that indicates the font style for the text object created by this method.</param>
        /// <param name="fontStretch">A value that indicates the font stretch for the text object created by this method.</param>
        /// <param name="fontSize">The logical size of the font in DIP ("device-independent pixel") units. A DIP equals 1/96 inch.</param>
        /// <param name="localeName">An array of characters that contains the locale name.</param>
        /// <unmanaged>HRESULT CreateTextFormat([In] const wchar* fontFamilyName,[None] IDWriteFontCollection* fontCollection,[None] DWRITE_FONT_WEIGHT fontWeight,[None] DWRITE_FONT_STYLE fontStyle,[None] DWRITE_FONT_STRETCH fontStretch,[None] FLOAT fontSize,[In] const wchar* localeName,[Out] IDWriteTextFormat** textFormat)</unmanaged>
        public TextFormat(Factory factory, string fontFamilyName, SharpDX.DirectWrite.FontCollection fontCollection, SharpDX.DirectWrite.FontWeight fontWeight, SharpDX.DirectWrite.FontStyle fontStyle, SharpDX.DirectWrite.FontStretch fontStretch, float fontSize, string localeName) : base(IntPtr.Zero)
        {
            factory.CreateTextFormat(fontFamilyName, fontCollection, fontWeight, fontStyle, fontStretch, fontSize, localeName, this);
        }

        /// <summary>	
        /// Gets a copy of the font family name. 	
        /// </summary>	
        /// <returns>the current font family name. </returns>
        /// <unmanaged>HRESULT IDWriteTextFormat::GetFontFamilyName([Out, Buffer] wchar_t* fontFamilyName,[None] int nameSize)</unmanaged>
        public string FontFamilyName
        {
            get
            {
                unsafe
                {
                    int fontFamilyNameLength = GetFontFamilyNameLength();
                    char* fontFamilyName = stackalloc char[fontFamilyNameLength + 1];
                    GetFontFamilyName(new IntPtr(fontFamilyName), fontFamilyNameLength + 1);
                    return new string(fontFamilyName, 0, fontFamilyNameLength);
                }
            }
        }

        /// <summary>	
        /// Gets a copy of the locale name. 	
        /// </summary>	
        /// <returns>the current locale name.</returns>
        /// <unmanaged>HRESULT IDWriteTextFormat::GetLocaleName([Out, Buffer] wchar_t* localeName,[None] int nameSize)</unmanaged>
        public string LocaleName
        {
            get
            {
                unsafe
                {
                    int localNameLength = GetLocaleNameLength();
                    char* localName = stackalloc char[localNameLength + 1];
                    GetLocaleName(new IntPtr(localName), localNameLength + 1);
                    return new string(localName, 0, localNameLength);
                }
            }
        }
    }
}
