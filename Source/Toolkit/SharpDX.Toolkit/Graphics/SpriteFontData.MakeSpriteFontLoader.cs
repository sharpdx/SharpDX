// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

using SharpDX.Multimedia;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class SpriteFontData
    {
        /// <summary>
        ///  This method for loading/saving a font file generated from MakeSpriteFont.
        /// </summary>
        /// <param name="serializer">The binaryserializer to use.</param>
        /// <returns></returns>
        private void SerializeMakeSpriteFont(BinarySerializer serializer)
        {
            FourCC magicCode2 = "font";
            serializer.Serialize(ref magicCode2);
            if (magicCode2 != "font")
                return;

            // Writes the version
            if (serializer.Mode == SerializerMode.Read)
            {
                int version = serializer.Reader.ReadInt32();
                if (version != Version)
                {
                    throw new NotSupportedException(string.Format("SpriteFontData version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, Version));
                }
            }
            else
            {
                serializer.Writer.Write(Version);
            }
            
            // Deserialize Glyphs
            int glyphCount = 0;
            serializer.Serialize(ref glyphCount);

            // For MakeSpriteFont, there is only one GlyphPage.
            Glyphs = new Glyph[glyphCount];
            for (int i = 0; i < glyphCount; i++)
            {
                serializer.Serialize(ref Glyphs[i].Character);
                serializer.Serialize(ref Glyphs[i].Subrect);
                serializer.Serialize(ref Glyphs[i].Offset);
                serializer.Serialize(ref Glyphs[i].XAdvance);

                // Fix XAdvance with Right/Left for MakeSpriteFont
                Glyphs[i].XAdvance += Glyphs[i].Subrect.Right - Glyphs[i].Subrect.Left;
            }

            serializer.Serialize(ref LineSpacing);
            serializer.Serialize(ref DefaultCharacter);

            var image = new BitmapData();
            Bitmaps = new Bitmap[1] {new Bitmap()};
            Bitmaps[0].Data = image;

            serializer.Serialize(ref image.Width);
            serializer.Serialize(ref image.Height);
            serializer.SerializeEnum(ref image.PixelFormat);
            serializer.Serialize(ref image.RowStride);
            serializer.Serialize(ref image.CompressedHeight);
            serializer.Serialize(ref image.Data, image.RowStride * image.CompressedHeight);
        }
    }
}