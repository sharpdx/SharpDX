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

using System.IO;
using SharpDX.IO;
using SharpDX.Serialization;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
    [ContentReader(typeof(SpriteFontDataContentReader))]
    public class SpriteFontData : IDataSerializable
    {
        const string spriteFontMagic = "DXTKfont";

        public Glyph[] Glyphs;

        public float LineSpacing;

        public int DefaultCharacter;

        public Bitmap Image;

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An <see cref="EffectData"/>. Null if the stream is not a serialized <see cref="EffectData"/>.</returns>
        /// <remarks>
        /// </remarks>
        public static SpriteFontData Load(Stream stream)
        {
            var serializer = new BinarySerializer(stream, SerializerMode.Read, Text.Encoding.ASCII) {ArrayLengthType = ArrayLengthType.Int};
            var data = serializer.Load<SpriteFontData>();

            // Glyps are null, then this is not a SpriteFondData
            if (data.Glyphs == null)
                return null;
            return data;
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static SpriteFontData Load(byte[] buffer)
        {
            return Load(new MemoryStream(buffer));
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified file.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static SpriteFontData Load(string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(stream);
        }

        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            string magic = spriteFontMagic;
            serializer.Serialize(ref magic, spriteFontMagic.Length);

            if (magic != spriteFontMagic)
            {
                // Make sure that Glyphs is null
                Glyphs = null;
                return;
            }

            serializer.Serialize(ref Glyphs);
            serializer.Serialize(ref LineSpacing);
            serializer.Serialize(ref DefaultCharacter);

            serializer.Serialize(ref Image);
        }

        public struct Glyph : IDataSerializable
        {
            // Unicode codepoint.
            public int Character;

            // Glyph image data (may only use a portion of a larger bitmap).
            public Rectangle Subrect;

            // Layout information.
            public Vector2 Offset;

            // Advance X
            public float XAdvance;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Character);
                serializer.Serialize(ref Subrect);
                serializer.Serialize(ref Offset);
                serializer.Serialize(ref XAdvance);
            }
        }

        public class Bitmap : IDataSerializable
        {
            public int Width;

            public int Height;

            public DXGI.Format PixelFormat;

            public int RowStride;

            public int CompressedHeight;

            public byte[] Data;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Width);
                serializer.Serialize(ref Height);
                serializer.SerializeEnum(ref PixelFormat);
                serializer.Serialize(ref RowStride);
                serializer.Serialize(ref CompressedHeight);
                serializer.Serialize(ref Data, RowStride * CompressedHeight);
            }
        }
    }
}