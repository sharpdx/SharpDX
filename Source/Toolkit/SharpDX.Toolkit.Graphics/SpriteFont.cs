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
    /// <summary>Represents a font texture.</summary>
    [ContentReader(typeof(SpriteFontContentReader))]
    public sealed class SpriteFont : GraphicsResource
    {
        private readonly Dictionary<char, int> characterMap;
        private readonly SpriteFontData.Glyph[] glyphs;
        private Texture2D texture;

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
        /// <param name="stream">The stream.</param>
        /// <returns>An <see cref="EffectData"/>. Null if the stream is not a serialized <see cref="EffectData"/>.</returns>
        /// <remarks>
        /// </remarks>
        public static SpriteFont Load(GraphicsDevice device, Stream stream)
        {
            var spriteFontData = SpriteFontData.Load(stream);
            if (spriteFontData == null)
                return null;
            return New(device, spriteFontData);
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static SpriteFont Load(GraphicsDevice device, byte[] buffer)
        {
            return Load(device, new MemoryStream(buffer));
        }

        /// <summary>
        /// Loads an <see cref="EffectData"/> from the specified file.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>An <see cref="EffectData"/> </returns>
        public static SpriteFont Load(GraphicsDevice device, string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(device, stream);
        }
        
        internal SpriteFont(GraphicsDevice device, SpriteFontData spriteFontData)
            : base(device)
        {
            // Read the glyph data.
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

            Characters = new ReadOnlyCollection<char>(characterList);

            // Read font properties.
            LineSpacing = spriteFontData.LineSpacing;

            DefaultCharacter = (char)spriteFontData.DefaultCharacter;

            // Read the texture data.
            var image = spriteFontData.Image;

            // Create the texture
            texture =  ToDispose(Texture2D.New(device, image.Width, image.Height, image.PixelFormat, image.Data));
        }

        internal void InternalDraw(ref StringProxy text, SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, ref Vector2 scale, SpriteEffects spriteEffects, float depth)
        {
            var baseOffset = origin;

            // If the text is mirrored, offset the start position accordingly.
            if (spriteEffects != SpriteEffects.None)
            {
                baseOffset -= MeasureString(ref text)*axisIsMirroredTable[(int) spriteEffects & 3];
            }

            var localScale = scale;


            // Draw each character in turn.
            ForEachGlyph(ref text, (ref SpriteFontData.Glyph glyph, float x, float y) =>
                                       {
                                           var offset = new Vector2(x, y + glyph.Offset.Y);
                                           Vector2.Modulate(ref offset, ref axisDirectionTable[(int) spriteEffects & 3], out offset);
                                           Vector2.Add(ref offset, ref baseOffset, out offset);


                                           if (spriteEffects != SpriteEffects.None)
                                           {
                                               // For mirrored characters, specify bottom and/or right instead of top left.
                                               var glyphRect = new Vector2(glyph.Subrect.Right - glyph.Subrect.Left, glyph.Subrect.Top - glyph.Subrect.Bottom);
                                               Vector2.Modulate(ref glyphRect, ref axisIsMirroredTable[(int) spriteEffects & 3], out offset);
                                           }
                                           var destination = new DrawingRectangleF(position.X, position.Y, localScale.X, localScale.Y);
                                           DrawingRectangle? sourceRectangle = glyph.Subrect;
                                           spriteBatch.DrawSprite(texture, ref destination, true, ref sourceRectangle, color, rotation, ref offset, spriteEffects, depth);
                                       });
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

        private Vector2 MeasureString(ref StringProxy text)
        {
            var result = Vector2.Zero;
            ForEachGlyph(ref text, (ref SpriteFontData.Glyph glyph, float x, float y) =>
            {
                float w = x + (glyph.Subrect.Right - glyph.Subrect.Left);
                float h = y + Math.Max((glyph.Subrect.Bottom - glyph.Subrect.Top) + glyph.Offset.Y, LineSpacing);
                if (w > result.X) result.X = w;
                if (h > result.Y) result.Y = h;
            });

            return result;
        }

        /// <summary>Gets a collection of all the characters that are included in the font.</summary>
        public ReadOnlyCollection<char> Characters { get; private set; }

        /// <summary>Gets or sets the default character for the font.</summary>
        public char? DefaultCharacter { get; set; }

        /// <summary>Gets or sets the vertical distance (in pixels) between the base lines of two consecutive lines of text. Line spacing includes the blank space between lines as well as the height of the characters.</summary>
        public float LineSpacing { get; set; }

        /// <summary>Gets or sets the spacing of the font characters.</summary>
        public float Spacing { get; set; }

        private delegate void GlyphAction(ref SpriteFontData.Glyph glyph, float x, float y);

        private unsafe void ForEachGlyph(ref StringProxy text, GlyphAction action)
        {
            float x = 0;
            float y = 0;

            fixed (void* pGlyph = glyphs)
            {
                for (int i =  0; i < text.Length; i++)
                {
                    char character = text[i];

                    switch (character)
                    {
                        case '\r':
                            // Skip carriage returns.
                            continue;

                        case '\n':
                            // New line.
                            x = 0;
                            y += LineSpacing;
                            break;

                        default:
                            // Output this character.
                            int glyphIndex;
                            if (!characterMap.TryGetValue(character, out glyphIndex))
                                throw new ArgumentException(string.Format("Character '{0}' is not available in the SpriteFont character map", character), "text");

                            var glyph = (SpriteFontData.Glyph*) pGlyph + glyphIndex;


                            x += glyph->Offset.X;

                            if (x < 0)
                                x = 0;

                            if (!char.IsWhiteSpace(character))
                            {
                                action(ref *glyph, x, y);
                            }

                            x += glyph->Subrect.Right - glyph->Subrect.Left + glyph->XAdvance;
                            break;
                    }
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
    }


}