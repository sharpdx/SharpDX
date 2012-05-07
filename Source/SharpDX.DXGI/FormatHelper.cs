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
using System;

namespace SharpDX.DXGI
{
    /// <summary>
    /// Helper to use with <see cref="Format"/>.
    /// </summary>
    public static class FormatHelper
    {

        /// <summary>
        /// Calculates the size of a <see cref="Format"/> in bytes.
        /// </summary>
        /// <param name="format">The dxgi format.</param>
        /// <returns>size of in bytes</returns>
        public static float SizeOfInBytes(Format format)
        {
            return (float) SizeOfInBits(format)/8;
        }

        /// <summary>
        /// Calculates the size of a <see cref="Format"/> in bits.
        /// </summary>
        /// <param name="format">The dxgi format.</param>
        /// <returns>size of in bits</returns>
        public static int SizeOfInBits(Format format)
        {
            // Size from doc http://msdn.microsoft.com/en-us/library/bb173059%28VS.85%29.aspx
            switch (format)
            {
                case Format.R1_UNorm:
                    return 1;
                case Format.A8_UNorm:
                case Format.R8_SInt:
                case Format.R8_SNorm:
                case Format.R8_Typeless:
                case Format.R8_UInt:
                case Format.R8_UNorm:
                    return 8;
                case Format.B5G5R5A1_UNorm:
                case Format.B5G6R5_UNorm:
                case Format.D16_UNorm:
                case Format.R16_Float:
                case Format.R16_SInt:
                case Format.R16_SNorm:
                case Format.R16_Typeless:
                case Format.R16_UInt:
                case Format.R16_UNorm:
                case Format.R8G8_SInt:
                case Format.R8G8_SNorm:
                case Format.R8G8_Typeless:
                case Format.R8G8_UInt:
                case Format.R8G8_UNorm:
                    return 16;
                case Format.B8G8R8X8_Typeless:
                case Format.B8G8R8X8_UNorm:
                case Format.B8G8R8X8_UNorm_SRgb:
                case Format.D24_UNorm_S8_UInt:
                case Format.D32_Float:
                case Format.D32_Float_S8X24_UInt:
                case Format.G8R8_G8B8_UNorm:
                case Format.R10G10B10_Xr_Bias_A2_UNorm:
                case Format.R10G10B10A2_Typeless:
                case Format.R10G10B10A2_UInt:
                case Format.R10G10B10A2_UNorm:
                case Format.R11G11B10_Float:
                case Format.R16G16_Float:
                case Format.R16G16_SInt:
                case Format.R16G16_SNorm:
                case Format.R16G16_Typeless:
                case Format.R16G16_UInt:
                case Format.R16G16_UNorm:
                case Format.R24_UNorm_X8_Typeless:
                case Format.R24G8_Typeless:
                case Format.R32_Float:
                case Format.R32_Float_X8X24_Typeless:
                case Format.R32_SInt:
                case Format.R32_Typeless:
                case Format.R32_UInt:
                case Format.R8G8_B8G8_UNorm:
                case Format.R8G8B8A8_SInt:
                case Format.R8G8B8A8_SNorm:
                case Format.R8G8B8A8_Typeless:
                case Format.R8G8B8A8_UInt:
                case Format.R8G8B8A8_UNorm:
                case Format.R8G8B8A8_UNorm_SRgb:
                case Format.B8G8R8A8_Typeless:
                case Format.B8G8R8A8_UNorm:
                case Format.B8G8R8A8_UNorm_SRgb:
                case Format.R9G9B9E5_Sharedexp:
                case Format.X24_Typeless_G8_UInt:
                case Format.X32_Typeless_G8X24_UInt:
                    return 32;
                case Format.BC1_Typeless:
                case Format.BC1_UNorm:
                case Format.BC1_UNorm_SRgb:
                case Format.BC4_SNorm:
                case Format.BC4_Typeless:
                case Format.BC4_UNorm:
                case Format.R16G16B16A16_Float:
                case Format.R16G16B16A16_SInt:
                case Format.R16G16B16A16_SNorm:
                case Format.R16G16B16A16_Typeless:
                case Format.R16G16B16A16_UInt:
                case Format.R16G16B16A16_UNorm:
                case Format.R32G32_Float:
                case Format.R32G32_SInt:
                case Format.R32G32_Typeless:
                case Format.R32G32_UInt:
                case Format.R32G8X24_Typeless:
                    return 64;
                case Format.R32G32B32_Float:
                case Format.R32G32B32_SInt:
                case Format.R32G32B32_Typeless:
                case Format.R32G32B32_UInt:
                    return 96;
                case Format.BC2_Typeless:
                case Format.BC2_UNorm:
                case Format.BC2_UNorm_SRgb:
                case Format.BC3_Typeless:
                case Format.BC3_UNorm:
                case Format.BC3_UNorm_SRgb:
                case Format.BC5_SNorm:
                case Format.BC5_Typeless:
                case Format.BC5_UNorm:
                case Format.BC6H_Sf16:
                case Format.BC6H_Typeless:
                case Format.BC6H_Uf16:
                case Format.BC7_Typeless:
                case Format.BC7_UNorm:
                case Format.BC7_UNorm_SRgb:
                case Format.R32G32B32A32_Float:
                case Format.R32G32B32A32_SInt:
                case Format.R32G32B32A32_Typeless:
                case Format.R32G32B32A32_UInt:
                    return 128;
            }
            throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Unknown size for DXGI Format [{0}] ", format));
        }
    }
}