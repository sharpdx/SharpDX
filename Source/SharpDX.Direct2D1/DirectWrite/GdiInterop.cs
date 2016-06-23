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
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{

    public partial class GdiInterop
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class LogFont
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string lfFaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FontSignature
        {
            public int fsUsb1;
            public int fsUsb2;
            public int fsUsb3;
            public int fsUsb4;
            public int fsCsb1;
            public int fsCsb2;
        }

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
                RawBool isSystemFont;
                ConvertFontToLOGFONT(font, new IntPtr(nativeLogFont), out isSystemFont);
                Marshal.PtrToStructure(new IntPtr(nativeLogFont), logFont);
                return isSystemFont;                
            }
        } 
    }
}