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
// -----------------------------------------------------------------------------
// The following code is a port of SpriteBatch from DirectXTk
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SharpDX.IO;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Represents a font texture.
    /// </summary>
    [ContentReader(typeof(SpriteFontContentReader))]
    public sealed class SpriteFont : GraphicsResource
    {
        private readonly float globalBaseOffsetY;
        private readonly Dictionary<char, int> characterMap;
        private readonly Dictionary<int, float> kerningMap;
        private readonly SpriteFontData.Glyph[] glyphs;
        private Texture2D[] textures;
        private readonly int defaultGlyphIndex;

        // cache delegates only once to avoid their construction at runtime which generates garbage
        private readonly GlyphAction<InternalDrawCommand> drawGlyphDelegate;
        private readonly GlyphAction<Vector2> measureGlyphDelegate;

        // Lookup table indicates which way to move along each axis per SpriteEffects enum value.
        private static readonly Vector2[] axisDirectionTable = new[]
                                                                   {
                                                                       new Vector2(-1, -1),
                                                                       new Vector2(1, -1),
                                                                       new Vector2(-1, 1),
                                                                       new Vector2(1, 1),
                                                                   };

        // Lookup table indicates which axes are mirrored for each SpriteEffects enum value.
        private static readonly Vector2[] axisIsMirroredTable = new[]
                                                                    {
                                                                        new Vector2(0, 0),
                                                                        new Vector2(1, 0),
                                                                        new Vector2(0, 1),
                                                                        new Vector2(1, 1),
                                                                    };

        public static SpriteFont New(GraphicsDevice device, SpriteFontData spriteFontData)
        {
            return new SpriteFont(device, spriteFontData);
        }


        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified stream.
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="stream">The stream.</param>
        /// <param name="bitmapDataLoader">A delegate to load bitmap data that are not stored in the buffer.</param>
        /// <returns>An <see cref="EffectData"/>. Null if the stream is not a serialized <see cref="EffectData"/>.</returns>
        /// <remarks>
        /// </remarks>
        public static SpriteFont Load(GraphicsDevice device, Stream stream, SpriteFontBitmapDataLoaderDelegate bitmapDataLoader = null)
        {
            var spriteFontData = SpriteFontData.Load(stream, bitmapDataLoader);
            if (spriteFontData == null)
                return null;
            return New(device, spriteFontData);
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified buffer.
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bitmapDataLoader">A delegate to load bitmap data that are not stored in the buffer.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static SpriteFont Load(GraphicsDevice device, byte[] buffer, SpriteFontBitmapDataLoaderDelegate bitmapDataLoader = null)
        {
            return Load(device, new MemoryStream(buffer), bitmapDataLoader);
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified file.
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="fileName">The filename.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static SpriteFont Load(GraphicsDevice device, string fileName)
        {
            var fileDirectory = Path.GetDirectoryName(fileName);
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(device, stream, bitmapName => Texture2D.Load(device, Path.Combine(fileDirectory, bitmapName)));
        }
       
        internal SpriteFont(GraphicsDevice device, SpriteFontData spriteFontData)
            : base(device)
        {
            drawGlyphDelegate = InternalDrawGlyph;
            measureGlyphDelegate = MeasureStringGlyph;

            // Read the glyph data.
            globalBaseOffsetY = spriteFontData.BaseOffset;
            glyphs = spriteFontData.Glyphs;
            characterMap = new Dictionary<char, int>(glyphs.Length * 2);

            // Prebuild the character map
            var characterList = new List<char>(glyphs.Length);
            for (int i = 0; i < glyphs.Length; i++)
            {
                var charItem = (char)glyphs[i].Character;
                characterMap.Add(charItem, i);
                characterList.Add(charItem);
            }

            // Prepare kernings if they are available.
            var kernings = spriteFontData.Kernings;
            if (kernings != null)
            {
                kerningMap = new Dictionary<int, float>(spriteFontData.Kernings.Length);
                for (int i = 0; i < kernings.Length; i++)
                {
                    int key = (kernings[i].First << 16) | kernings[i].Second;
                    kerningMap.Add(key, kernings[i].Offset);
                }
            }

            Characters = new ReadOnlyCollection<char>(characterList);

            // Read font properties.
            LineSpacing = spriteFontData.LineSpacing;

            if (spriteFontData.DefaultCharacter > 0)
            {
                DefaultCharacter = (char)spriteFontData.DefaultCharacter;
                if (!characterMap.TryGetValue(DefaultCharacter.Value, out defaultGlyphIndex))
                {
                    defaultGlyphIndex = -1;
                }
            }

            // Read the texture data.
            textures = new Texture2D[spriteFontData.Bitmaps.Length];
            for(int i = 0; i < textures.Length; i++)
            {
                var bitmap = spriteFontData.Bitmaps[i];
                if (bitmap.Data is SpriteFontData.BitmapData)
                {
                    var image = (SpriteFontData.BitmapData) bitmap.Data;
                    textures[i] = ToDispose(Texture2D.New(device, image.Width, image.Height, image.PixelFormat, image.Data));
                }
                else if (bitmap.Data is Texture2D)
                {
                    textures[i] = (Texture2D) bitmap.Data;
                }
                else
                {
                    throw new NotSupportedException(string.Format("SpriteFontData.Bitmap of type [{0}] is not supported. Only SpriteFontData.BitmapData or Texture2D", bitmap == null ? "null" : bitmap.GetType().Name));
                }
            }
        }

        internal void InternalDraw(ref StringProxy text, ref InternalDrawCommand drawCommand)
        {
            //origin.Y += globalBaseOffsetY;

            // If the text is mirrored, offset the start position accordingly.
            if (drawCommand.spriteEffects != SpriteEffects.None)
            {
                drawCommand.origin -= MeasureString(ref text) * axisIsMirroredTable[(int)drawCommand.spriteEffects & 3];
            }

            // Draw each character in turn.
            ForEachGlyph(ref text, drawGlyphDelegate, ref drawCommand);
        }

        internal void InternalDrawGlyph(ref InternalDrawCommand parameters, ref SpriteFontData.Glyph glyph, float x, float y)
        {
            var spriteEffects = parameters.spriteEffects;

            var offset = new Vector2(x, y + glyph.Offset.Y);
            Vector2.Modulate(ref offset, ref axisDirectionTable[(int)spriteEffects & 3], out offset);
            Vector2.Add(ref offset, ref parameters.origin, out offset);


            if (spriteEffects != SpriteEffects.None)
            {
                // For mirrored characters, specify bottom and/or right instead of top left.
                var glyphRect = new Vector2(glyph.Subrect.Right - glyph.Subrect.Left, glyph.Subrect.Top - glyph.Subrect.Bottom);
                Vector2.Modulate(ref glyphRect, ref axisIsMirroredTable[(int)spriteEffects & 3], out offset);
            }
            var destination = new RectangleF(parameters.position.X, parameters.position.Y, parameters.scale.X, parameters.scale.Y);
            Rectangle? sourceRectangle = glyph.Subrect;
            parameters.spriteBatch.DrawSprite(textures[glyph.BitmapIndex], ref destination, true, ref sourceRectangle, parameters.color, parameters.rotation, ref offset, spriteEffects, parameters.depth);            
        }

        /// <summary>Returns the width and height of a string as a Vector2.</summary>
        /// <param name="text">The string to measure.</param>
        public Vector2 MeasureString(string text)
        {
            var proxyText = new StringProxy(text);
            return MeasureString(ref proxyText);
        }

        /// <summary>Returns the width and height of a string as a Vector2.</summary>
        /// <param name="text">The string to measure.</param>
        public Vector2 MeasureString(StringBuilder text)
        {
            var proxyText = new StringProxy(text);
            return MeasureString(ref proxyText);
        }

        /// <summary>
        /// Checks whether the provided character is present in the character map of the current <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>true if the <paramref name="c"/> is pesent in the character map, false - otherwise.</returns>
        public bool IsCharPresent(char c)
        {
            return characterMap.ContainsKey(c);
        }

        private Vector2 MeasureString(ref StringProxy text)
        {
            var result = Vector2.Zero;
            ForEachGlyph(ref text, measureGlyphDelegate, ref result);
            return result;
        }

        private void MeasureStringGlyph(ref Vector2 result, ref SpriteFontData.Glyph glyph, float x, float y)
        {
            float w = x + (glyph.Subrect.Right - glyph.Subrect.Left) + Spacing;
            float h = y + Math.Max((glyph.Subrect.Bottom - glyph.Subrect.Top) + glyph.Offset.Y, LineSpacing);
            if (w > result.X)
            {
                result.X = w;
            }
            if (h > result.Y)
            {
                result.Y = h;
            }
        }

        /// <summary>Gets a collection of all the characters that are included in the font.</summary>
        public ReadOnlyCollection<char> Characters { get; private set; }

        /// <summary>Gets or sets the default character for the font.</summary>
        public char? DefaultCharacter { get; set; }

        /// <summary>Gets or sets the vertical distance (in pixels) between the base lines of two consecutive lines of text. Line spacing includes the blank space between lines as well as the height of the characters.</summary>
        public float LineSpacing { get; set; }

        /// <summary>Gets or sets the spacing of the font characters.</summary>
        public float Spacing { get; set; }

        private delegate void GlyphAction<T>(ref T parameters, ref SpriteFontData.Glyph glyph, float x, float y);

        private unsafe void ForEachGlyph<T>(ref StringProxy text, GlyphAction<T> action, ref T parameters)
        {
            float x = 0;
            float y = 0;
            // TODO: Not sure how to handle globalBaseOffsetY from AngelCode BMFont

            fixed (void* pGlyph = glyphs)
            {
                var key = 0;
                for (int i =  0; i < text.Length; i++)
                {
                    char character = text[i];					

                    switch (character)
                    {
                        case '\r':
                            // Skip carriage returns.
                            key |= character;
                            continue;

                        case '\n':
                            // New line.
                            x = 0;
                            y += LineSpacing;
                            key |= character;
                            break;

                        default:
                            // Output this character.
                            int glyphIndex;
                            if (!characterMap.TryGetValue(character, out glyphIndex))
                            {
                                if(DefaultCharacter.HasValue && defaultGlyphIndex >= 0)
                                {
                                    character = DefaultCharacter.Value;
                                    glyphIndex = defaultGlyphIndex;
                                }
                                else
                                {
                                    throw new ArgumentException(string.Format("Character '{0}' is not available in the SpriteFont character map", character), "text");
                                }
                            }
                            key |= character;

                            var glyph = (SpriteFontData.Glyph*) pGlyph + glyphIndex;

                            // do not offset the first character, otherwise it is impossible to compute correct alignment
                            // using MeasureString results
                            if (x > 0f) x += glyph->Offset.X;

                            // reset negative offset (it can happen only for first character)
                            if(x < 0f) x = 0f;

                            // Offset the kerning
                            float kerningOffset;
                            if (kerningMap != null && kerningMap.TryGetValue(key, out kerningOffset))
                                x += kerningOffset;

                            if (!char.IsWhiteSpace(character))
                            {
                                action(ref parameters, ref *glyph, x, y);
                            }

                            x += glyph->XAdvance + Spacing;
                            break;
                    }

                    // Shift the kerning key
                    key  =  (key << 16);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct StringProxy
        {
            private string textString;
            private StringBuilder textBuilder;
            public readonly int Length;
            public StringProxy(string text)
            {
                this.textString = text;
                this.textBuilder = null;
                this.Length = text.Length;
            }

            public StringProxy(StringBuilder text)
            {
                this.textBuilder = text;
                this.textString = null;
                this.Length = text.Length;
            }

            public char this[int index]
            {
                get
                {
                    if (this.textString != null)
                    {
                        return this.textString[index];
                    }
                    return this.textBuilder[index];
                }
            }
        }

        /// <summary>
        /// Struct InternalDrawCommand used to pass parameters to InternalDrawGlyph
        /// </summary>
        internal struct InternalDrawCommand
        {
            public InternalDrawCommand(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float depth)
            {
                this.spriteBatch = spriteBatch;
                this.position = position;
                this.color = color;
                this.rotation = rotation;
                this.origin = origin;
                this.scale = scale;
                this.spriteEffects = spriteEffects;
                this.depth = depth;
            }

            public SpriteBatch spriteBatch;

            public Vector2 position;

            public Color color;

            public float rotation;

            public Vector2 origin;

            public Vector2 scale;

            public SpriteEffects spriteEffects;

            public float depth;
        }
    }
}