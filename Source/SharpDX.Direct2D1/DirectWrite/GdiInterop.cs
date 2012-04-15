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
#if !WIN8METRO
using System.Drawing;
#endif
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{

    public partial class GdiInterop
    {
        /// <summary>	
        /// Creates a font object that matches the properties specified by the LOGFONT structure. 	
        /// </summary>	
        /// <param name="logFont">A structure containing a GDI-compatible font description. </param>
        /// <returns>a  reference to a newly created <see cref="SharpDX.DirectWrite.Font"/>. </returns>
        /// <unmanaged>HRESULT IDWriteGdiInterop::CreateFontFromLOGFONT([In] const LOGFONTW* logFont,[Out] IDWriteFont** font)</unmanaged>
        public Font FromLogFont(object logFont)
        {
            unsafe
            {
                int sizeOfLogFont = Marshal.SizeOf(logFont);
                byte* nativeLogFont = stackalloc byte[sizeOfLogFont];
                Marshal.StructureToPtr(logFont, new IntPtr(nativeLogFont), false);
                Font font;
                CreateFontFromLOGFONT(new IntPtr(nativeLogFont), out font);
                return font;
            }           
        }

        /// <summary>	
        /// Initializes a LOGFONT structure based on the GDI-compatible properties of the specified font. 	
        /// </summary>	
        /// <remarks>	
        /// The conversion to a  LOGFONT by using ConvertFontToLOGFONT operates at the logical font level and does not guarantee that it will map to a specific physical font. It is not guaranteed that GDI will select the same physical font for displaying  text formatted by a LOGFONT as the <see cref="SharpDX.DirectWrite.Font"/> object that was converted. 	
        /// </remarks>	
        /// <param name="font">An <see cref="SharpDX.DirectWrite.Font"/> object to be converted into a GDI-compatible LOGFONT structure. </param>
        /// <param name="logFont">When this method returns, contains a structure that receives a GDI-compatible font description. </param>
        /// <returns> TRUE if the specified font object is part of the system font collection; otherwise, FALSE. </returns>
        /// <unmanaged>HRESULT IDWriteGdiInterop::ConvertFontToLOGFONT([None] IDWriteFont* font,[In] LOGFONTW* logFont,[Out] BOOL* isSystemFont)</unmanaged>
        public bool ToLogFont(Font font, object logFont)
        {
            unsafe
            {
                int sizeOfLogFont = Marshal.SizeOf(logFont);
                byte* nativeLogFont = stackalloc byte[sizeOfLogFont];
                bool isSystemFont;
                ConvertFontToLOGFONT(font, new IntPtr(nativeLogFont), out isSystemFont);
                Marshal.PtrToStructure(new IntPtr(nativeLogFont), logFont);
                return isSystemFont;                
            }
        } 
#if !WIN8METRO
        /// <summary>	
        /// Creates a font object that matches the properties specified by the LOGFONT structure. 	
        /// </summary>	
        /// <param name="font">A <see cref="System.Drawing.Font"/> description. </param>
        /// <returns>a reference to a newly created <see cref="SharpDX.DirectWrite.Font"/>. </returns>
        /// <unmanaged>HRESULT IDWriteGdiInterop::CreateFontFromLOGFONT([In] const LOGFONTW* logFont,[Out] IDWriteFont** font)</unmanaged>
        public Font FromSystemDrawingFont(System.Drawing.Font font)
        {
            var logfontw = new Win32Native.LogFont();
            font.ToLogFont(logfontw);
            return FromLogFont(logfontw);
        }

        /// <summary>
        /// Convert a Direct2D <see cref="Font"/> to a <see cref="System.Drawing.Font"/>.
        /// </summary>
        /// <param name="d2dFont">a Direct2D Font</param>
        /// <param name="font">a <see cref="System.Drawing.Font"/></param>
        /// <returns>true if the specified font object is part of the system font collection; otherwise, false.</returns>
        public bool ToSystemDrawingFont(Font d2dFont, out System.Drawing.Font font)
        {
            var logfontw = new Win32Native.LogFont();
            bool isSystemFont = ToLogFont(d2dFont, logfontw);
            font = System.Drawing.Font.FromLogFont(logfontw);
            return isSystemFont;
        }
#endif
    }
}