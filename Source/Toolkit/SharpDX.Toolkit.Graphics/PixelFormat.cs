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
using System.Runtime.InteropServices;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// PixelFormat is equivalent to <see cref="SharpDX.DXGI.Format"/>.
    /// </summary>
    /// <remarks>
    /// Usage is slightly different from <see cref="SharpDX.DXGI.Format"/>, as you have to select the type of the pixel format first (<see cref="Typeless"/>, <see cref="SInt"/>...etc)
    /// and then access the available pixel formats for this type. Example: PixelFormat.UNorm.R8.
    /// Because <see cref="PixelFormat"/> is directly convertible to <see cref="SharpDX.DXGI.Format"/>, you can use it inplace where <see cref="SharpDX.DXGI.Format"/> is required
    /// and vice-versa.
    /// </remarks>
    /// <msdn-id>bb173059</msdn-id>	
    /// <unmanaged>DXGI_FORMAT</unmanaged>	
    /// <unmanaged-short>DXGI_FORMAT</unmanaged-short>	
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct PixelFormat
    {
        /// <summary>
        /// Gets the value as a <see cref="SharpDX.DXGI.Format"/> enum.
        /// </summary>
        public readonly DXGI.Format Value;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="format"></param>
        private PixelFormat(DXGI.Format format)
        {
            Value = format;
        }

        public int SizeInBytes
        {
            get { return (int)DXGI.FormatHelper.SizeOfInBytes(this); }
        }

        public static readonly PixelFormat Unknown = new PixelFormat(DXGI.Format.Unknown);

        /// <summary>
        /// Typeless pixel format.
        /// </summary>
        public static class Typeless
        {
            public static readonly PixelFormat R32G32B32A32 = new PixelFormat(DXGI.Format.R32G32B32A32_Typeless);
            public static readonly PixelFormat R32G32B32 = new PixelFormat(DXGI.Format.R32G32B32_Typeless);
            public static readonly PixelFormat R16G16B16A16 = new PixelFormat(DXGI.Format.R16G16B16A16_Typeless);
            public static readonly PixelFormat R32G32 = new PixelFormat(DXGI.Format.R32G32_Typeless);
            public static readonly PixelFormat R10G10B10A2 = new PixelFormat(DXGI.Format.R10G10B10A2_Typeless);
            public static readonly PixelFormat R8G8B8A8 = new PixelFormat(DXGI.Format.R8G8B8A8_Typeless);
            public static readonly PixelFormat R16G16 = new PixelFormat(DXGI.Format.R16G16_Typeless);
            public static readonly PixelFormat R32 = new PixelFormat(DXGI.Format.R32_Typeless);
            public static readonly PixelFormat R8G8 = new PixelFormat(DXGI.Format.R8G8_Typeless);
            public static readonly PixelFormat R16 = new PixelFormat(DXGI.Format.R16_Typeless);
            public static readonly PixelFormat R8 = new PixelFormat(DXGI.Format.R8_Typeless);
            public static readonly PixelFormat BC1 = new PixelFormat(DXGI.Format.BC1_Typeless);
            public static readonly PixelFormat BC2 = new PixelFormat(DXGI.Format.BC2_Typeless);
            public static readonly PixelFormat BC3 = new PixelFormat(DXGI.Format.BC3_Typeless);
            public static readonly PixelFormat BC4 = new PixelFormat(DXGI.Format.BC4_Typeless);
            public static readonly PixelFormat BC5 = new PixelFormat(DXGI.Format.BC5_Typeless);
            public static readonly PixelFormat B8G8R8A8 = new PixelFormat(DXGI.Format.B8G8R8A8_Typeless);
            public static readonly PixelFormat B8G8R8X8 = new PixelFormat(DXGI.Format.B8G8R8X8_Typeless);
            public static readonly PixelFormat BC6H = new PixelFormat(DXGI.Format.BC6H_Typeless);
            public static readonly PixelFormat BC7 = new PixelFormat(DXGI.Format.BC7_Typeless);
        }

        /// <summary>
        /// Float pixel formats.
        /// </summary>
        public static class Float
        {
            public static readonly PixelFormat R32G32B32A32 = new PixelFormat(DXGI.Format.R32G32B32A32_Float);
            public static readonly PixelFormat R32G32B32 = new PixelFormat(DXGI.Format.R32G32B32_Float);
            public static readonly PixelFormat R16G16B16A16 = new PixelFormat(DXGI.Format.R16G16B16A16_Float);
            public static readonly PixelFormat R32G32 = new PixelFormat(DXGI.Format.R32G32_Float);
            public static readonly PixelFormat R11G11B10 = new PixelFormat(DXGI.Format.R11G11B10_Float);
            public static readonly PixelFormat R16G16 = new PixelFormat(DXGI.Format.R16G16_Float);
            public static readonly PixelFormat R32 = new PixelFormat(DXGI.Format.R32_Float);
            public static readonly PixelFormat R16 = new PixelFormat(DXGI.Format.R16_Float);
        }

        /// <summary>
        /// Unsigned Int pixel formats.
        /// </summary>
        public static class UInt
        {
            public static readonly PixelFormat R32G32B32A32 = new PixelFormat(DXGI.Format.R32G32B32A32_UInt);
            public static readonly PixelFormat R32G32B32 = new PixelFormat(DXGI.Format.R32G32B32_UInt);
            public static readonly PixelFormat R16G16B16A16 = new PixelFormat(DXGI.Format.R16G16B16A16_UInt);
            public static readonly PixelFormat R32G32 = new PixelFormat(DXGI.Format.R32G32_UInt);
            public static readonly PixelFormat R10G10B10A2 = new PixelFormat(DXGI.Format.R10G10B10A2_UInt);
            public static readonly PixelFormat R8G8B8A8 = new PixelFormat(DXGI.Format.R8G8B8A8_UInt);
            public static readonly PixelFormat R16G16 = new PixelFormat(DXGI.Format.R16G16_UInt);
            public static readonly PixelFormat R32 = new PixelFormat(DXGI.Format.R32_UInt);
            public static readonly PixelFormat R8G8 = new PixelFormat(DXGI.Format.R8G8_UInt);
            public static readonly PixelFormat R16 = new PixelFormat(DXGI.Format.R16_UInt);
            public static readonly PixelFormat R8 = new PixelFormat(DXGI.Format.R8_UInt);
        }

        /// <summary>
        /// Signed Int pixel formats.
        /// </summary>
        public static class SInt
        {
            public static readonly PixelFormat R32G32B32A32 = new PixelFormat(DXGI.Format.R32G32B32A32_SInt);
            public static readonly PixelFormat R32G32B32 = new PixelFormat(DXGI.Format.R32G32B32_SInt);
            public static readonly PixelFormat R16G16B16A16 = new PixelFormat(DXGI.Format.R16G16B16A16_SInt);
            public static readonly PixelFormat R32G32 = new PixelFormat(DXGI.Format.R32G32_SInt);
            public static readonly PixelFormat R8G8B8A8 = new PixelFormat(DXGI.Format.R8G8B8A8_SInt);
            public static readonly PixelFormat R16G16 = new PixelFormat(DXGI.Format.R16G16_SInt);
            public static readonly PixelFormat R32 = new PixelFormat(DXGI.Format.R32_SInt);
            public static readonly PixelFormat R8G8 = new PixelFormat(DXGI.Format.R8G8_SInt);
            public static readonly PixelFormat R16 = new PixelFormat(DXGI.Format.R16_SInt);
            public static readonly PixelFormat R8 = new PixelFormat(DXGI.Format.R8_SInt);
        }

        /// <summary>
        /// Unsigned normalized pixel formats.
        /// </summary>
        public static class UNorm
        {
            public static readonly PixelFormat R16G16B16A16 = new PixelFormat(DXGI.Format.R16G16B16A16_UNorm);
            public static readonly PixelFormat R10G10B10A2 = new PixelFormat(DXGI.Format.R10G10B10A2_UNorm);
            public static readonly PixelFormat R8G8B8A8 = new PixelFormat(DXGI.Format.R8G8B8A8_UNorm);
            public static readonly PixelFormat R8G8B8A8_SRgb = new PixelFormat(DXGI.Format.R8G8B8A8_UNorm_SRgb);
            public static readonly PixelFormat R16G16 = new PixelFormat(DXGI.Format.R16G16_UNorm);
            public static readonly PixelFormat R8G8 = new PixelFormat(DXGI.Format.R8G8_UNorm);
            public static readonly PixelFormat R16 = new PixelFormat(DXGI.Format.R16_UNorm);
            public static readonly PixelFormat R8 = new PixelFormat(DXGI.Format.R8_UNorm);
            public static readonly PixelFormat A8 = new PixelFormat(DXGI.Format.A8_UNorm);
            public static readonly PixelFormat BC1 = new PixelFormat(DXGI.Format.BC1_UNorm);
            public static readonly PixelFormat BC1_SRgb = new PixelFormat(DXGI.Format.BC1_UNorm_SRgb);
            public static readonly PixelFormat BC2 = new PixelFormat(DXGI.Format.BC2_UNorm);
            public static readonly PixelFormat BC2_SRgb = new PixelFormat(DXGI.Format.BC2_UNorm_SRgb);
            public static readonly PixelFormat BC3 = new PixelFormat(DXGI.Format.BC3_UNorm);
            public static readonly PixelFormat BC3_SRgb = new PixelFormat(DXGI.Format.BC3_UNorm_SRgb);
            public static readonly PixelFormat BC4 = new PixelFormat(DXGI.Format.BC4_UNorm);
            public static readonly PixelFormat BC5 = new PixelFormat(DXGI.Format.BC5_UNorm);
            public static readonly PixelFormat B5G6R5 = new PixelFormat(DXGI.Format.B5G6R5_UNorm);
            public static readonly PixelFormat B5G5R5A1 = new PixelFormat(DXGI.Format.B5G5R5A1_UNorm);
            public static readonly PixelFormat B8G8R8A8 = new PixelFormat(DXGI.Format.B8G8R8A8_UNorm);
            public static readonly PixelFormat B8G8R8X8 = new PixelFormat(DXGI.Format.B8G8R8X8_UNorm);
            public static readonly PixelFormat B8G8R8A8_SRgb = new PixelFormat(DXGI.Format.B8G8R8A8_UNorm_SRgb);
            public static readonly PixelFormat B8G8R8X8_SRgb = new PixelFormat(DXGI.Format.B8G8R8X8_UNorm_SRgb);
            public static readonly PixelFormat BC7 = new PixelFormat(DXGI.Format.BC7_UNorm);
            public static readonly PixelFormat BC7_SRgb = new PixelFormat(DXGI.Format.BC7_UNorm_SRgb);
        }

        /// <summary>
        /// Signed normalized pixel formats.
        /// </summary>
        public static class SNorm
        {
            public static readonly PixelFormat R16G16B16A16 = new PixelFormat(DXGI.Format.R16G16B16A16_SNorm);
            public static readonly PixelFormat R8G8B8A8 = new PixelFormat(DXGI.Format.R8G8B8A8_SNorm);
            public static readonly PixelFormat R16G16 = new PixelFormat(DXGI.Format.R16G16_SNorm);
            public static readonly PixelFormat R8G8 = new PixelFormat(DXGI.Format.R8G8_SNorm);
            public static readonly PixelFormat R16 = new PixelFormat(DXGI.Format.R16_SNorm);
            public static readonly PixelFormat R8 = new PixelFormat(DXGI.Format.R8_SNorm);
            public static readonly PixelFormat BC4 = new PixelFormat(DXGI.Format.BC4_SNorm);
            public static readonly PixelFormat BC5 = new PixelFormat(DXGI.Format.BC5_SNorm);
        }

        public static implicit operator DXGI.Format(PixelFormat from)
        {
            return from.Value;
        }

        public static implicit operator PixelFormat(DXGI.Format from)
        {
            return new PixelFormat(from);
        }
    }
}