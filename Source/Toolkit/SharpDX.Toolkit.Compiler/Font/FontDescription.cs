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
// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//--------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
    //    <Asset Type="Graphics:FontDescription">
  // <FontName>Kootenay</FontName>
  // <Size>14</Size>
  // <Spacing>0</Spacing>
  // <UseKerning>true</UseKerning>
  // <Style>Regular</Style>
  // <CharacterRegions>
  //    <CharacterRegion>
  //      <Start>&#32;</Start>
  //      <End>&#126;</End>
  //    </CharacterRegion>
  //  </CharacterRegions>
  //</Asset>


    // Options telling the tool what to do.
    [XmlRoot("TkFont")]
    public class FontDescription
    {
        /// <summary>
        /// Input can be either a system (TrueType) font or a specially marked bitmap file.
        /// </summary>
        public string FontName;

        /// <summary>
        ///  Size and style for TrueType fonts (ignored when converting a bitmap font).
        /// </summary>
        public float Size = 23;

        /// <summary>
        /// Character spacing overrides. Zero is default spacing, negative closer together, positive further apart
        /// </summary>
        public float Spacing = 0;

        /// <summary>
        /// Line spacing overrides. Zero is default spacing, negative closer together, positive further apart
        /// </summary>
        public float LineSpacing = 0;

        /// <summary>
        /// Specifies whether to use kerning information when rendering the font. Default value is false (NOT SUPPORTED YET).
        /// </summary>
        public bool UseKerning = false;

        /// <summary>
        /// Format of the output texture. Values: 'auto', 'rgba32', 'bgra4444', 'compressedmono'. Default is 'auto'
        /// </summary>
        public FontTextureFormat Format = FontTextureFormat.Auto;

        /// <summary>
        /// Which characters to include in the font (eg. "/CharacterRegion:0x20-0x7F /CharacterRegion:0x123")
        /// </summary>
        public readonly List<CharacterRegion> CharacterRegions = new List<CharacterRegion>();

        /// <summary>
        /// Fallback character used when asked to render a codepoint that is not
        /// included in the font. If zero, missing characters throw exceptions.
        /// </summary>
        public char DefaultCharacter = (char)0;

        /// <summary>
        /// Style for the font. 'regular', 'bold', 'italic', 'underline', 'strikeout'. Default is 'regular
        /// </summary>
        public FontStyle Style = FontStyle.Regular;

        /// <summary>
        /// By default, font textures use premultiplied alpha format. Set this if you want interpolative alpha instead.
        /// </summary>
        public bool NoPremultiply = false;

        /// <summary>
        /// By default, font textures is a grey. To generate ClearType textures, turn this flag to true 
        /// </summary>
        public FontAntiAliasMode AntiAlias = FontAntiAliasMode.Default;

        public static FontDescription Load(string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read)) return Load(stream);
        }

        private static XmlSerializer serializer = new XmlSerializer(typeof(FontDescription));

        public static FontDescription Load(Stream stream)
        {
            return (FontDescription)serializer.Deserialize(stream);
        }

        public void Save(string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Create, NativeFileAccess.Write)) Save(stream);
        }

        public void Save(Stream stream)
        {
            var settings = new XmlWriterSettings { Indent = true };
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, this, ns);
            }
        }
    }
}
