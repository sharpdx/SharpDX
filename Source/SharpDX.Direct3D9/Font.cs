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

using SharpDX.Mathematics.Interop;
using System;
using SharpDX;

namespace SharpDX.Direct3D9
{
    public partial class Font
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fontDescription">The font description.</param>
        public Font(Device device, FontDescription fontDescription) : base(IntPtr.Zero)
        {
            D3DX9.CreateFontIndirect(device, ref fontDescription, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="mipLevels">The mip levels.</param>
        /// <param name="isItalic">if set to <c>true</c> [is italic].</param>
        /// <param name="characterSet">The character set.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="pitchAndFamily">The pitch and family.</param>
        /// <param name="faceName">Name of the face.</param>
        public Font(Device device, int height, int width, FontWeight weight, int mipLevels, bool isItalic, FontCharacterSet characterSet, FontPrecision precision, FontQuality quality, FontPitchAndFamily pitchAndFamily, string faceName)
        {
            D3DX9.CreateFont(device, height, width, (int)weight, mipLevels, isItalic, (int)characterSet, (int)precision, (int)quality, (int)pitchAndFamily,
                              faceName, this);
        }

        /// <summary>	
        /// Load formatted text into video memory to improve the efficiency of rendering to the device. This method supports ANSI and Unicode strings.	
        /// </summary>	
        /// <remarks>	
        /// The compiler setting also determines the function version. If Unicode is defined, the function call resolves to PreloadTextW. Otherwise, the function call resolves to PreloadTextA because ANSI strings are being used. This method generates textures that contain glyphs that represent the input text. The glyphs are drawn as a series of triangles. Text will not be rendered to the device; ID3DX10Font::DrawText must still be called to render the text. However, by preloading text into video memory, ID3DX10Font::DrawText will use substantially fewer CPU resources. This method internally converts characters to glyphs using the GDI function {{GetCharacterPlacement}}. 	
        /// </remarks>	
        /// <param name="stringRef">Pointer to a string of characters to be loaded into video memory. If the compiler settings require Unicode, the data type LPCTSTR resolves to LPCWSTR; otherwise, the data type resolves to LPCSTR. See Remarks. </param>
        /// <returns>If the method succeeds, the return value is S_OK. If the method fails, the return value can be one of the following: D3DERR_INVALIDCALL, D3DXERR_INVALIDDATA. </returns>
        /// <unmanaged>HRESULT ID3DX10Font::PreloadTextW([None] const wchar_t* pString,[None] int Count)</unmanaged>
        public void PreloadText(string stringRef)
        {
            PreloadText(stringRef, stringRef.Length);
        }

        /// <summary>	
        /// Draws formatted text. 
        /// </summary>	
        /// <param name="sprite"><para>Pointer to an <see cref="SharpDX.Direct3D9.Sprite"/> object that contains the string. Can be <c>null</c>, in which case Direct3D will render the string with its own sprite object. To improve efficiency, a sprite object should be specified if DrawText is to be called more than once in a row.</para></param>	
        /// <param name="text"><para>Pointer to a string to draw. If the Count parameter is -1, the string must be null-terminated.</para></param>	
        /// <param name="rect"><para>Pointer to a <see cref="RawRectangle"/> structure that contains the rectangle, in logical coordinates, in which the text is to be formatted. The coordinate value of the rectangle's right side must be greater than that of its left side. Likewise, the coordinate value of the bottom must be greater than that of the top.</para></param>	
        /// <param name="drawFlags"><para>Specifies the method of formatting the text. It can be any combination of the following values:</para>  ValueMeaning <list> <item><term>DT_BOTTOM</term> </item></list>  <para>Justifies the text to the bottom of the rectangle. This value must be combined with DT_SINGLELINE.</para>  <list> <item><term>DT_CALCRECT</term></item> </list>  <para>Determines the width and height of the rectangle. If there are multiple lines of text, DrawText uses the width of the rectangle pointed to by the pRect parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, DrawText modifies the right side of the rectangle so that it bounds the last character in the line. In either case, DrawText returns the height of the formatted text but does not draw the text.</para>  <list> <item><term>DT_CENTER</term></item> </list>  <para>Centers text horizontally in the rectangle.</para>  <list> <item><term>DT_EXPANDTABS</term></item> </list>  <para>Expands tab characters. The default number of characters per tab is eight.</para>  <list> <item><term>DT_LEFT</term></item> </list>  <para>Aligns text to the left.</para>  <list> <item><term>DT_NOCLIP</term></item> </list>  <para>Draws without clipping. DrawText is somewhat faster when DT_NOCLIP is used.</para>  <list> <item><term>DT_RIGHT</term></item> </list>  <para>Aligns text to the right.</para>  <list> <item><term>DT_RTLREADING</term></item> </list>  <para>Displays text in right-to-left reading order for bidirectional text when a Hebrew or Arabic font is selected. The default reading order for all text is left-to-right.</para>  <list> <item><term>DT_SINGLELINE</term></item> </list>  <para>Displays text on a single line only. Carriage returns and line feeds do not break the line.</para>  <list> <item><term>DT_TOP</term></item> </list>  <para>Top-justifies text.</para>  <list> <item><term>DT_VCENTER</term></item> </list>  <para>Centers text vertically (single line only).</para>  <list> <item><term>DT_WORDBREAK</term></item> </list>  <para>Breaks words. Lines are automatically broken between words if a word would extend past the edge of the rectangle specified by the pRect parameter. A carriage return/line feed sequence also breaks the line.</para>   <para>?</para></param>	
        /// <param name="color"><para>Color of the text. For more information, see <see cref="RawColor4"/>.</para></param>	
        /// <returns>If the function succeeds, the return value is the height of the text in logical units. If DT_VCENTER or DT_BOTTOM is specified, the return value is the offset from pRect (top to the bottom) of the drawn text. If the function fails, the return value is zero.</returns>	
        /// <remarks>	
        /// The parameters of this method are very similar to those of the GDI DrawText function.This method supports both ANSI and Unicode strings.This method must be called inside a  BeginScene ... EndScene block. The only exception is when an application calls DrawText with DT_CALCRECT to calculate the size of a given block of text.Unless the DT_NOCLIP format is used, this method clips the text so that it does not appear outside the specified rectangle. All formatting is assumed to have multiple lines unless the DT_SINGLELINE format is specified.If the selected font is too large for the rectangle, this method does not attempt to substitute a smaller font.This method supports only fonts whose escapement and orientation are both zero.	
        /// </remarks>	
        /// <unmanaged>int ID3DXFont::DrawTextW([In] ID3DXSprite* pSprite,[In] const wchar_t* pString,[In] int Count,[In] void* pRect,[In] unsigned int Format,[In] D3DCOLOR Color)</unmanaged>	
        public unsafe int DrawText(SharpDX.Direct3D9.Sprite sprite, string text, RawRectangle rect, FontDrawFlags drawFlags, RawColorBGRA color)
        {

            int value = DrawText(sprite, text, text.Length, new IntPtr(&rect), (int) drawFlags, color);
            if (value == 0)
                throw new SharpDXException("Draw failed");
            return value;
        }

        /// <summary>
        /// Draws formatted text.
        /// </summary>
        /// <param name="sprite">Pointer to an <see cref="SharpDX.Direct3D9.Sprite"/> object that contains the string. Can be <c>null</c>, in which case Direct3D will render the string with its own sprite object. To improve efficiency, a sprite object should be specified if DrawText is to be called more than once in a row.</param>
        /// <param name="text">Pointer to a string to draw. If the Count parameter is -1, the string must be null-terminated.</param>
        /// <param name="x">The x position to draw the text.</param>
        /// <param name="y">The y position to draw the text.</param>
        /// <param name="color">Color of the text. For more information, see <see cref="RawColor4"/>.</param>
        /// <returns>
        /// If the function succeeds, the return value is the height of the text in logical units. If DT_VCENTER or DT_BOTTOM is specified, the return value is the offset from pRect (top to the bottom) of the drawn text. If the function fails, the return value is zero.
        /// </returns>
        /// <unmanaged>int ID3DXFont::DrawTextW([In] ID3DXSprite* pSprite,[In] const wchar_t* pString,[In] int Count,[In] void* pRect,[In] unsigned int Format,[In] D3DCOLOR Color)</unmanaged>
        /// <remarks>
        /// The parameters of this method are very similar to those of the GDI DrawText function.This method supports both ANSI and Unicode strings.This method must be called inside a  BeginScene ... EndScene block. The only exception is when an application calls DrawText with DT_CALCRECT to calculate the size of a given block of text.Unless the DT_NOCLIP format is used, this method clips the text so that it does not appear outside the specified rectangle. All formatting is assumed to have multiple lines unless the DT_SINGLELINE format is specified.If the selected font is too large for the rectangle, this method does not attempt to substitute a smaller font.This method supports only fonts whose escapement and orientation are both zero.
        /// </remarks>
        public int DrawText(Sprite sprite, string text, int x, int y, RawColorBGRA color)
        {
            return DrawText(sprite, text, new RawRectangle(x, y, 0, 0), FontDrawFlags.NoClip, color);
        }

        /// <summary>
        /// Measures the specified sprite.
        /// </summary>
        /// <param name="sprite">Pointer to an <see cref="SharpDX.Direct3D9.Sprite"/> object that contains the string. Can be <c>null</c>, in which case Direct3D will render the string with its own sprite object. To improve efficiency, a sprite object should be specified if DrawText is to be called more than once in a row.</param>
        /// <param name="text"><para>Pointer to a string to draw. If the Count parameter is -1, the string must be null-terminated.</para></param>	
        /// <param name="drawFlags"><para>Specifies the method of formatting the text. It can be any combination of the following values:</para>  ValueMeaning <list> <item><term>DT_BOTTOM</term></item> </list>  <para>Justifies the text to the bottom of the rectangle. This value must be combined with DT_SINGLELINE.</para>  <list> <item><term>DT_CALCRECT</term></item> </list>  <para>Determines the width and height of the rectangle. If there are multiple lines of text, DrawText uses the width of the rectangle pointed to by the pRect parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, DrawText modifies the right side of the rectangle so that it bounds the last character in the line. In either case, DrawText returns the height of the formatted text but does not draw the text.</para>  <list> <item><term>DT_CENTER</term></item> </list>  <para>Centers text horizontally in the rectangle.</para>  <list> <item><term>DT_EXPANDTABS</term></item> </list>  <para>Expands tab characters. The default number of characters per tab is eight.</para>  <list> <item><term>DT_LEFT</term></item> </list>  <para>Aligns text to the left.</para>  <list> <item><term>DT_NOCLIP</term></item> </list>  <para>Draws without clipping. DrawText is somewhat faster when DT_NOCLIP is used.</para>  <list> <item><term>DT_RIGHT</term></item> </list>  <para>Aligns text to the right.</para>  <list> <item><term>DT_RTLREADING</term></item> </list>  <para>Displays text in right-to-left reading order for bidirectional text when a Hebrew or Arabic font is selected. The default reading order for all text is left-to-right.</para>  <list> <item><term>DT_SINGLELINE</term></item> </list>  <para>Displays text on a single line only. Carriage returns and line feeds do not break the line.</para>  <list> <item><term>DT_TOP</term></item> </list>  <para>Top-justifies text.</para>  <list> <item><term>DT_VCENTER</term></item> </list>  <para>Centers text vertically (single line only).</para>  <list> <item><term>DT_WORDBREAK</term></item> </list>  <para>Breaks words. Lines are automatically broken between words if a word would extend past the edge of the rectangle specified by the pRect parameter. A carriage return/line feed sequence also breaks the line.</para>   <para>?</para></param>	
        /// <returns>Determines the width and height of the rectangle. If there are multiple lines of text, this function uses the width of the rectangle pointed to by the rect parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, this method modifies the right side of the rectangle so that it bounds the last character in the line. </returns>
        public unsafe RawRectangle MeasureText(Sprite sprite, string text, FontDrawFlags drawFlags)
        {
            return MeasureText(sprite, text, new RawRectangle(), drawFlags);
        }

        /// <summary>
        /// Measures the specified sprite.
        /// </summary>
        /// <param name="sprite">Pointer to an <see cref="SharpDX.Direct3D9.Sprite"/> object that contains the string. Can be <c>null</c>, in which case Direct3D will render the string with its own sprite object. To improve efficiency, a sprite object should be specified if DrawText is to be called more than once in a row.</param>
        /// <param name="text"><para>Pointer to a string to draw. If the Count parameter is -1, the string must be null-terminated.</para></param>	
        /// <param name="rect"><para>Pointer to a <see cref="RawRectangle"/> structure that contains the rectangle, in logical coordinates, in which the text is to be formatted. The coordinate value of the rectangle's right side must be greater than that of its left side. Likewise, the coordinate value of the bottom must be greater than that of the top.</para></param>	
        /// <param name="drawFlags"><para>Specifies the method of formatting the text. It can be any combination of the following values:</para>  ValueMeaning <list> <item><term>DT_BOTTOM</term></item> </list>  <para>Justifies the text to the bottom of the rectangle. This value must be combined with DT_SINGLELINE.</para>  <list> <item><term>DT_CALCRECT</term></item> </list>  <para>Determines the width and height of the rectangle. If there are multiple lines of text, DrawText uses the width of the rectangle pointed to by the pRect parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, DrawText modifies the right side of the rectangle so that it bounds the last character in the line. In either case, DrawText returns the height of the formatted text but does not draw the text.</para>  <list> <item><term>DT_CENTER</term></item> </list>  <para>Centers text horizontally in the rectangle.</para>  <list> <item><term>DT_EXPANDTABS</term></item> </list>  <para>Expands tab characters. The default number of characters per tab is eight.</para>  <list> <item><term>DT_LEFT</term></item> </list>  <para>Aligns text to the left.</para>  <list> <item><term>DT_NOCLIP</term></item> </list>  <para>Draws without clipping. DrawText is somewhat faster when DT_NOCLIP is used.</para>  <list> <item><term>DT_RIGHT</term></item> </list>  <para>Aligns text to the right.</para>  <list> <item><term>DT_RTLREADING</term></item> </list>  <para>Displays text in right-to-left reading order for bidirectional text when a Hebrew or Arabic font is selected. The default reading order for all text is left-to-right.</para>  <list> <item><term>DT_SINGLELINE</term></item> </list>  <para>Displays text on a single line only. Carriage returns and line feeds do not break the line.</para>  <list> <item><term>DT_TOP</term></item> </list>  <para>Top-justifies text.</para>  <list> <item><term>DT_VCENTER</term></item> </list>  <para>Centers text vertically (single line only).</para>  <list> <item><term>DT_WORDBREAK</term></item> </list>  <para>Breaks words. Lines are automatically broken between words if a word would extend past the edge of the rectangle specified by the pRect parameter. A carriage return/line feed sequence also breaks the line.</para>   <para>?</para></param>	
        /// <returns>Determines the width and height of the rectangle. If there are multiple lines of text, this function uses the width of the rectangle pointed to by the rect parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, this method modifies the right side of the rectangle so that it bounds the last character in the line. </returns>
        public unsafe RawRectangle MeasureText(Sprite sprite, string text, RawRectangle rect, FontDrawFlags drawFlags)
        {
            // DT_CALCRECT
            int whiteColor = -1;
            DrawText(sprite, text, text.Length, new IntPtr(&rect), ((int)drawFlags) | 0x400, *(RawColorBGRA*)&whiteColor);
            return rect;
        }

        /// <summary>
        /// Measures the specified sprite.
        /// </summary>
        /// <param name="sprite">Pointer to an <see cref="SharpDX.Direct3D9.Sprite"/> object that contains the string. Can be <c>null</c>, in which case Direct3D will render the string with its own sprite object. To improve efficiency, a sprite object should be specified if DrawText is to be called more than once in a row.</param>
        /// <param name="text"><para>Pointer to a string to draw. If the Count parameter is -1, the string must be null-terminated.</para></param>	
        /// <param name="rect"><para>Pointer to a <see cref="RawRectangle"/> structure that contains the rectangle, in logical coordinates, in which the text is to be formatted. The coordinate value of the rectangle's right side must be greater than that of its left side. Likewise, the coordinate value of the bottom must be greater than that of the top.</para></param>	
        /// <param name="drawFlags"><para>Specifies the method of formatting the text. It can be any combination of the following values:</para>  ValueMeaning <list> <item><term>DT_BOTTOM</term></item> </list>  <para>Justifies the text to the bottom of the rectangle. This value must be combined with DT_SINGLELINE.</para>  <list> <item><term>DT_CALCRECT</term></item> </list>  <para>Determines the width and height of the rectangle. If there are multiple lines of text, DrawText uses the width of the rectangle pointed to by the pRect parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, DrawText modifies the right side of the rectangle so that it bounds the last character in the line. In either case, DrawText returns the height of the formatted text but does not draw the text.</para>  <list> <item><term>DT_CENTER</term></item> </list>  <para>Centers text horizontally in the rectangle.</para>  <list> <item><term>DT_EXPANDTABS</term></item> </list>  <para>Expands tab characters. The default number of characters per tab is eight.</para>  <list> <item><term>DT_LEFT</term></item> </list>  <para>Aligns text to the left.</para>  <list> <item><term>DT_NOCLIP</term></item> </list>  <para>Draws without clipping. DrawText is somewhat faster when DT_NOCLIP is used.</para>  <list> <item><term>DT_RIGHT</term></item> </list>  <para>Aligns text to the right.</para>  <list> <item><term>DT_RTLREADING</term></item> </list>  <para>Displays text in right-to-left reading order for bidirectional text when a Hebrew or Arabic font is selected. The default reading order for all text is left-to-right.</para>  <list> <item><term>DT_SINGLELINE</term></item> </list>  <para>Displays text on a single line only. Carriage returns and line feeds do not break the line.</para>  <list> <item><term>DT_TOP</term></item> </list>  <para>Top-justifies text.</para>  <list> <item><term>DT_VCENTER</term></item> </list>  <para>Centers text vertically (single line only).</para>  <list> <item><term>DT_WORDBREAK</term></item> </list>  <para>Breaks words. Lines are automatically broken between words if a word would extend past the edge of the rectangle specified by the pRect parameter. A carriage return/line feed sequence also breaks the line.</para>   <para>?</para></param>
        /// <param name="textHeight">The height of the formatted text but does not draw the text.</param>	
        /// <returns>Determines the width and height of the rectangle. If there are multiple lines of text, this function uses the width of the rectangle pointed to by the rect parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, this method modifies the right side of the rectangle so that it bounds the last character in the line. </returns>
        public unsafe RawRectangle MeasureText(Sprite sprite, string text, RawRectangle rect, FontDrawFlags drawFlags, out int textHeight)
        {
            // DT_CALCRECT
            int whiteColor = -1;
            textHeight = DrawText(sprite, text, text.Length, new IntPtr(&rect), ((int)drawFlags) | 0x400, *(RawColorBGRA*)&whiteColor);
            return rect;
        }

        
    }
}