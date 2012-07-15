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
using System.Collections.Generic;
using System.Security.Policy;

namespace SharpDX.DXGI
{
    /// <summary>
    /// Helper to use with <see cref="Format"/>.
    /// </summary>
    public static class FormatHelper
    {
        private static readonly int[] sizeOfInBits = new int[256];

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
            return sizeOfInBits[(int) format];
        }

        /// <summary>
        /// Static initializer to speed up size calculation (not sure the JIT is enough "smart" for this kind of thing).
        /// </summary>
        static FormatHelper()
        {
            InitFormat(new[] { Format.R1_UNorm }, 1);

            InitFormat(new[] { Format.A8_UNorm, Format.R8_SInt, Format.R8_SNorm, Format.R8_Typeless, Format.R8_UInt, Format.R8_UNorm }, 8);

            InitFormat(new[] { 
                Format.B5G5R5A1_UNorm,
                Format.B5G6R5_UNorm,
                Format.D16_UNorm,
                Format.R16_Float,
                Format.R16_SInt,
                Format.R16_SNorm,
                Format.R16_Typeless,
                Format.R16_UInt,
                Format.R16_UNorm,
                Format.R8G8_SInt,
                Format.R8G8_SNorm,
                Format.R8G8_Typeless,
                Format.R8G8_UInt,
                Format.R8G8_UNorm
            }, 16);

            InitFormat(new[] { 
                Format.B8G8R8X8_Typeless,
                Format.B8G8R8X8_UNorm,
                Format.B8G8R8X8_UNorm_SRgb,
                Format.D24_UNorm_S8_UInt,
                Format.D32_Float,
                Format.D32_Float_S8X24_UInt,
                Format.G8R8_G8B8_UNorm,
                Format.R10G10B10_Xr_Bias_A2_UNorm,
                Format.R10G10B10A2_Typeless,
                Format.R10G10B10A2_UInt,
                Format.R10G10B10A2_UNorm,
                Format.R11G11B10_Float,
                Format.R16G16_Float,
                Format.R16G16_SInt,
                Format.R16G16_SNorm,
                Format.R16G16_Typeless,
                Format.R16G16_UInt,
                Format.R16G16_UNorm,
                Format.R24_UNorm_X8_Typeless,
                Format.R24G8_Typeless,
                Format.R32_Float,
                Format.R32_Float_X8X24_Typeless,
                Format.R32_SInt,
                Format.R32_Typeless,
                Format.R32_UInt,
                Format.R8G8_B8G8_UNorm,
                Format.R8G8B8A8_SInt,
                Format.R8G8B8A8_SNorm,
                Format.R8G8B8A8_Typeless,
                Format.R8G8B8A8_UInt,
                Format.R8G8B8A8_UNorm,
                Format.R8G8B8A8_UNorm_SRgb,
                Format.B8G8R8A8_Typeless,
                Format.B8G8R8A8_UNorm,
                Format.B8G8R8A8_UNorm_SRgb,
                Format.R9G9B9E5_Sharedexp,
                Format.X24_Typeless_G8_UInt,
                Format.X32_Typeless_G8X24_UInt,
            }, 32);

            InitFormat(new[] { 
                Format.R16G16B16A16_Float,
                Format.R16G16B16A16_SInt,
                Format.R16G16B16A16_SNorm,
                Format.R16G16B16A16_Typeless,
                Format.R16G16B16A16_UInt,
                Format.R16G16B16A16_UNorm,
                Format.R32G32_Float,
                Format.R32G32_SInt,
                Format.R32G32_Typeless,
                Format.R32G32_UInt,
                Format.R32G8X24_Typeless,
            }, 64);

            InitFormat(new[] { 
                Format.R32G32B32_Float,
                Format.R32G32B32_SInt,
                Format.R32G32B32_Typeless,
                Format.R32G32B32_UInt,
            }, 96);

            InitFormat(new[] { 
                Format.R32G32B32A32_Float,
                Format.R32G32B32A32_SInt,
                Format.R32G32B32A32_Typeless,
                Format.R32G32B32A32_UInt,
            }, 128);

            InitFormat(new[] { 
                Format.BC1_Typeless,
                Format.BC1_UNorm,
                Format.BC1_UNorm_SRgb,
                Format.BC4_SNorm,
                Format.BC4_Typeless,
                Format.BC4_UNorm,
            }, 4);

            InitFormat(new[] { 
                Format.BC2_Typeless,
                Format.BC2_UNorm,
                Format.BC2_UNorm_SRgb,
                Format.BC3_Typeless,
                Format.BC3_UNorm,
                Format.BC3_UNorm_SRgb,
                Format.BC5_SNorm,
                Format.BC5_Typeless,
                Format.BC5_UNorm,
                Format.BC6H_Sf16,
                Format.BC6H_Typeless,
                Format.BC6H_Uf16,
                Format.BC7_Typeless,
                Format.BC7_UNorm,
                Format.BC7_UNorm_SRgb,
            }, 8);
        }

        private static void InitFormat(IEnumerable<Format> formats, int bitCount)
        {
            foreach (var format in formats)
                sizeOfInBits[(int)format] = bitCount;
        }
    }
}