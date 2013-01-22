// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of bytege, to any person obtaining a copy
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
using System.Runtime.InteropServices;

namespace SharpDX.Toolkit.Graphics
{
    internal class TGAHelper
    {
        enum ImageType : byte
        {
            NoImage = 0,
            ColorMapped = 1,
            TrueColor = 2,
            BlackAndWhite = 3,
            ColorMappedRLE = 9,
            TrueColorRLE = 10,
            BlackAndWhiteRLE = 11,
        };

        enum DescriptorFlags : byte
        {
            InvertX = 0x10,
            InvertY = 0x20,
            Interleaved2Way = 0x40, // Deprecated
            Interleaved4Way = 0x80, // Deprecated
        };

        const string g_TGA20_Signature = "TRUEVISION-XFILE.";

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Header
        {
            public byte     IDLength;
            public byte     ColorMapType;
            public ImageType ImageType;
            public ushort   ColorMapFirst;
            public ushort   ColorMapLength;
            public byte     ColorMapSize;
            public ushort   XOrigin;
            public ushort   YOrigin;
            public ushort   Width;
            public ushort   Height;
            public byte     BitsPerPixel;
            public DescriptorFlags Descriptor;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Footer
        {
            public ushort    ExtensionOffset;
            public ushort    DeveloperOffset;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)] 
            public string    Signature;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Extension
        {
            public ushort    Size;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)] 
            public string    AuthorName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 324)] 
            public string    AuthorComment;
            public ushort    StampMonth;
            public ushort    StampDay;
            public ushort    StampYear;
            public ushort    StampHour;
            public ushort    StampMinute;
            public ushort    StampSecond;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)] 
            public string    JobName;
            public ushort    JobHour;
            public ushort    JobMinute;
            public ushort    JobSecond;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)] 
            public string    SoftwareId;
            public ushort    VersionNumber;
            public byte      VersionLetter;
            public uint      KeyColor;
            public ushort    PixelNumerator;
            public ushort    PixelDenominator;
            public ushort    GammaNumerator;
            public ushort    GammaDenominator;
            public uint      ColorOffset;
            public uint      StampOffset;
            public uint      ScanOffset;
            public byte      AttributesType;
        };

        [Flags]
        public enum ConversionFlags
        {
            None = 0x0,
            Expand   = 0x1,      // Conversion requires expanded pixel size
            InvertX = 0x2,      // If set, scanlines are right-to-left
            InvertY = 0x4,      // If set, scanlines are top-to-bottom
            RLE     = 0x8,      // Source data is RLE compressed

            Swizzle  = 0x10000,  // Swizzle BGR<->RGB data
            Format888  = 0x20000,  // 24bpp format
        };


        //-------------------------------------------------------------------------------------
        // Decodes TGA header
        //-------------------------------------------------------------------------------------
        public static bool DecodeTGAHeader(IntPtr pSource, int size, out ImageDescription metadata, out int offset, out ConversionFlags convFlags)
        {
            metadata = new ImageDescription();
            offset = 0;
            convFlags = ConversionFlags.None;

            int sizeOfTGAHeader = Marshal.SizeOf(typeof(Header));
            if (size < sizeOfTGAHeader)
                return false;

            var header = (Header) Marshal.PtrToStructure(pSource, typeof (Header));

            if (header.ColorMapType != 0 || header.ColorMapLength != 0)
            {
                throw new NotSupportedException();
            }

            if ((header.Descriptor & (DescriptorFlags.Interleaved2Way | DescriptorFlags.Interleaved4Way)) != 0)
            {
                throw new NotSupportedException();
            }

            if (header.Width == 0 || header.Height == 0)
            {
                throw new NotSupportedException("TGA Invalid Width or Height (=0)");
            }

            switch (header.ImageType)
            {
                case ImageType.TrueColor:
                case ImageType.TrueColorRLE:
                    switch (header.BitsPerPixel)
                    {
                        case 16:
                            metadata.Format = DXGI.Format.B5G5R5A1_UNorm;
                            break;

                        case 24:
                            metadata.Format = DXGI.Format.R8G8B8A8_UNorm;
                            convFlags |= ConversionFlags.Expand;

                            break;

                        case 32:
                            metadata.Format = DXGI.Format.R8G8B8A8_UNorm;
                            // We could use DXGI.Format.B8G8R8A8_UNORM, but we prefer DXGI 1.0 formats
                            break;
                    }

                    if (header.ImageType == ImageType.TrueColorRLE)
                    {
                        convFlags |= ConversionFlags.RLE;
                    }
                    break;

                case ImageType.BlackAndWhite:
                case ImageType.BlackAndWhiteRLE:
                    switch (header.BitsPerPixel)
                    {
                        case 8:
                            metadata.Format = DXGI.Format.R8_UNorm;
                            break;

                        default:
                            throw new NotSupportedException();
                    }

                    if (header.ImageType == ImageType.BlackAndWhiteRLE)
                    {
                        convFlags |= ConversionFlags.RLE;
                    }
                    break;

                case ImageType.NoImage:
                case ImageType.ColorMapped:
                case ImageType.ColorMappedRLE:
                    //throw new NotSupportedException();
                    return false;
                default:
                    throw new NotSupportedException("TGA Invalid Data");
            }

            metadata.Width = header.Width;
            metadata.Height = header.Height;
            metadata.Depth = metadata.ArraySize = metadata.MipLevels = 1;
            metadata.Dimension = TextureDimension.Texture2D;

            if ((header.Descriptor & DescriptorFlags.InvertX) != 0)
                convFlags |= ConversionFlags.InvertX;

            if ((header.Descriptor & DescriptorFlags.InvertY) != 0)
                convFlags |= ConversionFlags.InvertY;

            offset = sizeOfTGAHeader;

            if (header.IDLength != 0)
            {
                offset += header.IDLength;
            }

            return true;
        }

    }
}