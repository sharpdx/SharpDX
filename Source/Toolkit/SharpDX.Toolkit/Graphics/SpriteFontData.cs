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
using System.IO;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.Serialization;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A delegate to load binary bitmap data from a bitmap name (currently used to load external bitmap referenced in AngelCode Bitmap data).
    /// </summary>
    /// <param name="bitmapName">The name of the bitmap data to load.</param>
    /// <returns>A bitmap data object.</returns>
    public delegate object SpriteFontBitmapDataLoaderDelegate(string bitmapName);

    /// <summary>
    /// Data for a SpriteFont object that supports kerning.
    /// </summary>
    /// <remarks>
    /// Loading of SpriteFontData supports DirectXTk "MakeSpriteFont" format and AngelCode Bitmap Font Maker (binary format).
    /// </remarks>
    [ContentReader(typeof(SpriteFontDataContentReader))]
    public partial class SpriteFontData : IDataSerializable
    {
        public const string FontMagicCode = "TKFT";

        public const int Version = 0x100;

        /// <summary>
        /// The number of pixels from the absolute top of the line to the base of the characters.
        /// </summary>
        public float BaseOffset;

        /// <summary>
        /// This is the distance in pixels between each line of text.
        /// </summary>
        public float LineSpacing;

        /// <summary>
        /// The default character fallback.
        /// </summary>
        public char DefaultCharacter;

        /// <summary>
        /// An array of <see cref="Glyph"/> data.
        /// </summary>
        public Glyph[] Glyphs;

        /// <summary>
        /// An array of <see cref="Bitmap"/> data.
        /// </summary>
        public Bitmap[] Bitmaps;

        /// <summary>
        /// An array of <see cref="Kerning"/> data.
        /// </summary>
        public Kerning[] Kernings;

        /// <summary>
        /// Loads a <see cref="SpriteFontData"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bitmapDataLoader">A delegate to load bitmap data that are not stored in the buffer.</param>
        /// <returns>An <see cref="SpriteFontData"/>. Null if the stream is not a serialized <see cref="SpriteFontData"/>.</returns>
        public static SpriteFontData Load(Stream stream, SpriteFontBitmapDataLoaderDelegate bitmapDataLoader = null)
        {
            var serializer = new BinarySerializer(stream, SerializerMode.Read, Text.Encoding.ASCII) {ArrayLengthType = ArrayLengthType.Int};

            var data = new SpriteFontData();

            var magicCode = FourCC.Empty;
            serializer.Serialize(ref magicCode);

            if (magicCode == "DXTK")
            {
                data.SerializeMakeSpriteFont(serializer);
            }
            else if (magicCode == new FourCC(0x03464D42)) // BMF\3
            {
                data.SerializeBMFFont(serializer);
            }
            else
            {
                return null;
            }

            // Glyphs are null, then this is not a SpriteFondData
            if (data.Glyphs == null)
                return null;

            if (bitmapDataLoader != null)
            {
                foreach (var bitmap in data.Bitmaps)
                {
                    // If the bitmap data is a string, then this is a texture to load
                    if (bitmap.Data is string)
                    {
                        bitmap.Data = bitmapDataLoader((string) bitmap.Data);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Loads a <see cref="SpriteFontData"/> from the specified stream.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bitmapDataLoader">A delegate to load bitmap data that are not stored in the buffer.</param>
        /// <returns>An <see cref="SpriteFontData"/>. Null if the stream is not a serialized <see cref="SpriteFontData"/>.</returns>
        public static SpriteFontData Load(byte[] buffer, SpriteFontBitmapDataLoaderDelegate bitmapDataLoader = null)
        {
            return Load(new MemoryStream(buffer), bitmapDataLoader);
        }

        /// <summary>
        /// Loads a <see cref="SpriteFontData"/> from the specified stream.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <param name="bitmapDataLoader">A delegate to load bitmap data that are not stored in the buffer.</param>
        /// <returns>An <see cref="SpriteFontData"/>. Null if the stream is not a serialized <see cref="SpriteFontData"/>.</returns>
        public static SpriteFontData Load(string fileName, SpriteFontBitmapDataLoaderDelegate bitmapDataLoader = null)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(stream, bitmapDataLoader);
        }

        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // TODO implement a custom serial here
            throw new NotImplementedException();
        //    string magic = FontMagicCode;
        //    serializer.Serialize(ref magic, FontMagicCode.Length);

        //    if (magic != FontMagicCode)
        //    {
        //        // Make sure that Glyphs is null
        //        //Glyphs = null;
        //        return;
        //    }

        //    //serializer.Serialize(ref Glyphs);
        //    serializer.Serialize(ref LineSpacing);
        //    serializer.Serialize(ref DefaultCharacter);

        //    //serializer.Serialize(ref Image);
        }
    }
}