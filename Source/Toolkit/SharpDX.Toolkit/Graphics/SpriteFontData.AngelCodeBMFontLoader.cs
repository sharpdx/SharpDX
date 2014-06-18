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
using System.IO;
using SharpDX.Mathematics;
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class SpriteFontData
    {
        /// <summary>
        /// Type of Channel, not used yet.
        /// </summary>
        private enum ChannelType : byte
        {
            Glyph = 0,
            Outline = 1,
            GlyphOutline = 2,
            Zero = 3,
            One = 4
        }

        /// <summary>
        /// A BMFont common header.
        /// </summary>
        private struct BMFCommon : IDataSerializable
        {
            public ushort LineHeight;
            public ushort Base;
            public ushort ScaleW;
            public ushort ScaleH;
            public ushort PageCount;
            public byte   BitField; // 	1 	bits 	10 	bits 0-6: reserved, bit 7: packed
            public ChannelType Alpha;
            public ChannelType Red;
            public ChannelType Green;
            public ChannelType Blue;
            public void Serialize(BinarySerializer serializer)
            {
                int expectedBlockSize = 0;
                serializer.Serialize(ref expectedBlockSize);

                var blockPosition = serializer.Stream.Position;

                serializer.Serialize(ref LineHeight);
                serializer.Serialize(ref Base);
                serializer.Serialize(ref ScaleW);
                serializer.Serialize(ref ScaleH);
                serializer.Serialize(ref PageCount);
                serializer.Serialize(ref BitField);
                serializer.SerializeEnum(ref Alpha);
                serializer.SerializeEnum(ref Red);
                serializer.SerializeEnum(ref Green);
                serializer.SerializeEnum(ref Blue);

                var blockSize = serializer.Stream.Position - blockPosition;
                if (blockSize != expectedBlockSize)
                    throw new NotSupportedException("Invalid BMF font format");
            }
        }

        /// <summary>
        /// A BMFont glyph  header.
        /// </summary>
        private struct BMFGlyph : IDataSerializable
        {
            public int    Id;   // 	4 	uint 	0+c*20 	These fields are repeated until all characters have been described
            public ushort X; // 	2 	uint 	4+c*20
            public ushort Y; //  	2 	uint 	6+c*20 	
            public ushort Width; // 	2 	uint 	8+c*20 	
            public ushort Height; // 	2 	uint 	10+c*20 	
            public short OffsetX; // 	2 	int 	12+c*20 	
            public short OffsetY; //   2 	int 	14+c*20 	
            public short AdvanceX; // 2 	int 	16+c*20 	
            public byte   PageIndex; // 	1 	uint 	18+c*20 	
            public byte   ChannelIndex; // 1 	uint 	19+c*20 

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Id);
                serializer.Serialize(ref X);
                serializer.Serialize(ref Y);
                serializer.Serialize(ref Width);
                serializer.Serialize(ref Height);
                serializer.Serialize(ref OffsetX);
                serializer.Serialize(ref OffsetY);
                serializer.Serialize(ref AdvanceX);
                serializer.Serialize(ref PageIndex);
                serializer.Serialize(ref ChannelIndex);
            }
        }

        /// <summary>
        ///  This method for loading/saving a font file generated from AngelCode BMFont.
        /// </summary>
        /// <param name="serializer">The binary serializer to use.</param>
        private void SerializeBMFFont(BinarySerializer serializer)
        {
            // ----------------------------------------------------------
            // Read block Info (1)
            // ----------------------------------------------------------
            byte blockType = 1;
            serializer.Serialize(ref blockType);
            if (blockType != 1)
                return;

            // Skip Info block
            int expectedBlockSize = 0;
            serializer.Serialize(ref expectedBlockSize);
            serializer.Stream.Seek(expectedBlockSize, SeekOrigin.Current);

            // ----------------------------------------------------------
            // Read block Common (2)
            // ----------------------------------------------------------
            serializer.Serialize(ref blockType);
            if (blockType != 2)
                return;
            var common = new BMFCommon();
            common.Serialize(serializer);

            // Copy the base offset.
            BaseOffset = common.Base;
            LineSpacing = common.LineHeight;

            // ----------------------------------------------------------
            // Read block page names (3)
            // ----------------------------------------------------------
            serializer.Serialize(ref blockType);
            if (blockType != 3)
                return;
            serializer.Serialize(ref expectedBlockSize);

            // Create bitmap array.
            Bitmaps = new Bitmap[common.PageCount];
            for (int i = 0; i < Bitmaps.Length; i++)
            {
                string name = null;
                serializer.Serialize(ref name, true);
                // Store the name in data
                Bitmaps[i] = new Bitmap { Data = name };
            }

            // ----------------------------------------------------------
            // Read block glyphs (4)
            // ----------------------------------------------------------
            serializer.Serialize(ref blockType);
            if (blockType != 4)
                return;
            serializer.Serialize(ref expectedBlockSize);

            int countChars = expectedBlockSize/20;

            var bmfGlyph = new BMFGlyph();

            Glyphs = new Glyph[countChars];
            for (int i = 0; i < Glyphs.Length; i++)
            {
                bmfGlyph.Serialize(serializer);

                Glyphs[i] = new Glyph
                                {
                                    Character = bmfGlyph.Id, 
                                    Subrect = new Rectangle(bmfGlyph.X, bmfGlyph.Y, bmfGlyph.Width, bmfGlyph.Height), 
                                    Offset = {X = bmfGlyph.OffsetX, Y = bmfGlyph.OffsetY}, 
                                    XAdvance = bmfGlyph.AdvanceX,
                                    BitmapIndex = bmfGlyph.PageIndex
                                };
            }

            // ----------------------------------------------------------
            // Read block kernings (5) optional
            // ----------------------------------------------------------
            if (serializer.Stream.Position < serializer.Stream.Length)
            {
                // If there is still some data to read, there is probably some kernings
                serializer.Serialize(ref blockType);
                if (blockType != 5)
                    return;
                serializer.Serialize(ref expectedBlockSize);
                int kernelCount = expectedBlockSize/10;

                Kernings = new Kerning[kernelCount];
                for (int i = 0; i < Kernings.Length; i++)
                {
                    serializer.Serialize(ref Kernings[i].First);
                    serializer.Serialize(ref Kernings[i].Second);
                    short offset = 0;
                    serializer.Serialize(ref offset);
                    Kernings[i].Offset = offset;
                }
            }
        }
    }
}