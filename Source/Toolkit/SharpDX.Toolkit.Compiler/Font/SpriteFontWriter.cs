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
using System;
using System.IO;

using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
    using System.Drawing;
    using System.Drawing.Imaging;

    // Writes the output spritefont binary file.
    internal static class SpriteFontWriter
    {
        const string spriteFontMagic = "DXTKfont";

        const int DXGI_FORMAT_R8G8B8A8_UNORM = 28;
        const int DXGI_FORMAT_B4G4R4A4_UNORM = 115;
        const int DXGI_FORMAT_BC2_UNORM = 74;


        public static void WriteSpriteFont(FontDescription options, string outputFilename, Glyph[] glyphs, float lineSpacing, Bitmap bitmap)
        {
            using (var stream = new NativeFileStream(outputFilename, NativeFileMode.Create, NativeFileAccess.Write))
            {
                WriteSpriteFont(options, stream, glyphs, lineSpacing, bitmap);
            }
        }

        public static void WriteSpriteFont(FontDescription options, Stream outputStream, Glyph[] glyphs, float lineSpacing, Bitmap bitmap)
        {
            using (var writer = new BinaryWriter(outputStream))
            {
                WriteMagic(writer);
                WriteGlyphs(writer, glyphs);

                writer.Write(lineSpacing);
                writer.Write(options.DefaultCharacter);
                
                WriteBitmap(writer, options, bitmap);
            }
        }

        static void WriteMagic(BinaryWriter writer)
        {
            foreach (char magic in spriteFontMagic)
            {
                writer.Write((byte)magic);
            }
        }


        static void WriteGlyphs(BinaryWriter writer, Glyph[] glyphs)
        {
            writer.Write(glyphs.Length);

            foreach (Glyph glyph in glyphs)
            {
                writer.Write((int)glyph.Character);

                writer.Write(glyph.Subrect.Left);
                writer.Write(glyph.Subrect.Top);
                writer.Write(glyph.Subrect.Right);
                writer.Write(glyph.Subrect.Bottom);

                writer.Write(glyph.XOffset);
                writer.Write(glyph.YOffset);
                writer.Write(glyph.XAdvance);
            }
        }


        static void WriteBitmap(BinaryWriter writer, FontDescription options, Bitmap bitmap)
        {
            writer.Write(bitmap.Width);
            writer.Write(bitmap.Height);

            switch (options.Format)
            {
                case FontTextureFormat.Rgba32:
                    WriteRgba32(writer, bitmap);
                    break;
             
                case FontTextureFormat.Bgra4444:
                    WriteBgra4444(writer, bitmap);
                    break;
                
                case FontTextureFormat.CompressedMono:
                    WriteCompressedMono(writer, bitmap, options);
                    break;
                
                default:
                    throw new NotSupportedException();
            }
        }


        // Writes an uncompressed 32 bit font texture.
        static void WriteRgba32(BinaryWriter writer, Bitmap bitmap)
        {
            writer.Write(DXGI_FORMAT_R8G8B8A8_UNORM);

            writer.Write(bitmap.Width * 4);
            writer.Write(bitmap.Height);

            using (var bitmapData = new BitmapUtils.PixelAccessor(bitmap, ImageLockMode.ReadOnly))
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color color = bitmapData[x, y];

                        writer.Write(color.R);
                        writer.Write(color.G);
                        writer.Write(color.B);
                        writer.Write(color.A);
                    }
                }
            }
        }


        // Writes a 16 bit font texture.
        static void WriteBgra4444(BinaryWriter writer, Bitmap bitmap)
        {
            writer.Write(DXGI_FORMAT_B4G4R4A4_UNORM);

            writer.Write(bitmap.Width * sizeof(ushort));
            writer.Write(bitmap.Height);

            using (var bitmapData = new BitmapUtils.PixelAccessor(bitmap, ImageLockMode.ReadOnly))
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color color = bitmapData[x, y];

                        int r = color.R >> 4;
                        int g = color.G >> 4;
                        int b = color.B >> 4;
                        int a = color.A >> 4;

                        int packed = b | (g << 4) | (r << 8) | (a << 12);

                        writer.Write((ushort)packed);
                    }
                }
            }
        }


        // Writes a block compressed monochromatic font texture.
        static void WriteCompressedMono(BinaryWriter writer, Bitmap bitmap, FontDescription options)
        {
            if ((bitmap.Width & 3) != 0 ||
                (bitmap.Height & 3) != 0)
            {
                throw new ArgumentException("Block compression requires texture size to be a multiple of 4.");
            }

            writer.Write(DXGI_FORMAT_BC2_UNORM);

            writer.Write(bitmap.Width * 4);
            writer.Write(bitmap.Height / 4);

            using (var bitmapData = new BitmapUtils.PixelAccessor(bitmap, ImageLockMode.ReadOnly))
            {
                for (int y = 0; y < bitmap.Height; y += 4)
                {
                    for (int x = 0; x < bitmap.Width; x += 4)
                    {
                        CompressBlock(writer, bitmapData, x, y, options);
                    }
                }
            }
        }


        // We want to compress our font textures, because, like, smaller is better, 
        // right? But a standard DXT compressor doesn't do a great job with fonts that 
        // are in premultiplied alpha format. Our font data is greyscale, so all of the 
        // RGBA channels have the same value. If one channel is compressed differently 
        // to another, this causes an ugly variation in brightness of the rendered text. 
        // Also, fonts are mostly either black or white, with grey values only used for 
        // antialiasing along their edges. It is very important that the black and white 
        // areas be accurately represented, while the precise value of grey is less 
        // important.
        //
        // Trouble is, your average DXT compressor knows nothing about these 
        // requirements. It will optimize to minimize a generic error metric such as 
        // RMS, but this will often sacrifice crisp black and white in exchange for 
        // needless accuracy of the antialiasing pixels, or encode RGB differently to 
        // alpha. UGLY!
        //
        // Fortunately, encoding monochrome fonts turns out to be trivial. Using DXT3, 
        // we can fix the end colors as black and white, which gives guaranteed exact 
        // encoding of the font inside and outside, plus two fractional values for edge 
        // antialiasing. Also, these RGB values (0, 1/3, 2/3, 1) map exactly to four of 
        // the possible 16 alpha values available in DXT3, so we can ensure the RGB and 
        // alpha channels always exactly match.

        static void CompressBlock(BinaryWriter writer, BitmapUtils.PixelAccessor bitmapData, int blockX, int blockY, FontDescription options)
        {
            long alphaBits = 0;
            int rgbBits = 0;

            int pixelCount = 0;

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    long alpha;
                    int rgb;

                    int value = bitmapData[blockX + x, blockY + y].A;

                    if (options.NoPremultiply)
                    {
                        // If we are not premultiplied, RGB is always white and we have 4 bit alpha.
                        alpha = value >> 4;
                        rgb = 0;
                    }
                    else
                    {
                        // For premultiplied encoding, quantize the source value to 2 bit precision.
                        if (value < 256 / 6)
                        {
                            alpha = 0;
                            rgb = 1;
                        }
                        else if (value < 256 / 2)
                        {
                            alpha = 5;
                            rgb = 3;
                        }
                        else if (value < 256 * 5 / 6)
                        {
                            alpha = 10;
                            rgb = 2;
                        }
                        else
                        {
                            alpha = 15;
                            rgb = 0;
                        }
                    }

                    // Add this pixel to the alpha and RGB bit masks.
                    alphaBits |= alpha << (pixelCount * 4);
                    rgbBits |= rgb << (pixelCount * 2);

                    pixelCount++;
                }
            }

            // Output the alpha bit mask.
            writer.Write(alphaBits);

            // Output the two endpoint colors (black and white in 5.6.5 format).
            writer.Write((ushort)0xFFFF);
            writer.Write((ushort)0);

            // Output the RGB bit mask.
            writer.Write(rgbBits);
        }
    }
}
