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
using System.Runtime.InteropServices;

using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>PixelFormat is equivalent to <see cref="SharpDX.DXGI.Format" />.</summary>
    /// <msdn-id>bb173059</msdn-id>
    ///   <unmanaged>DXGI_FORMAT</unmanaged>
    ///   <unmanaged-short>DXGI_FORMAT</unmanaged-short>
    /// <remarks>This structure is implicitly castable to and from <see cref="SharpDX.DXGI.Format" />, you can use it inplace where <see cref="SharpDX.DXGI.Format" /> is required
    /// and vice-versa.
    /// Usage is slightly different from <see cref="SharpDX.DXGI.Format" />, as you have to select the type of the pixel format first (<see cref="Typeless" />, <see cref="SInt" />...etc)
    /// and then access the available pixel formats for this type. Example: PixelFormat.UNorm.R8.</remarks>
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct PixelFormat : IEquatable<PixelFormat>
    {
        /// <summary>Gets the value as a <see cref="SharpDX.DXGI.Format" /> enum.</summary>
        public readonly Format Value;

        /// <summary>Internal constructor.</summary>
        /// <param name="format">The format.</param>
        private PixelFormat(Format format)
        {
            this.Value = format;
        }

        /// <summary>Gets the size information bytes.</summary>
        /// <value>The size information bytes.</value>
        public int SizeInBytes { get { return (int)FormatHelper.SizeOfInBytes(this); } }

        /// <summary>The unknown.</summary>
        public static readonly PixelFormat Unknown = new PixelFormat(Format.Unknown);

        /// <summary>The a8 class.</summary>
        public static class A8
        {
            #region Constants and Fields

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.A8_UNorm);

            #endregion
        }

        /// <summary>The b5 g5 r5 a1 class.</summary>
        public static class B5G5R5A1
        {
            #region Constants and Fields

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.B5G5R5A1_UNorm);

            #endregion
        }

        /// <summary>The b5 g6 r5 class.</summary>
        public static class B5G6R5
        {
            #region Constants and Fields

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.B5G6R5_UNorm);

            #endregion
        }

        /// <summary>The b8 g8 r8 a8 class.</summary>
        public static class B8G8R8A8
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.B8G8R8A8_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.B8G8R8A8_UNorm);

            /// <summary>The authentication norm arguments RGB.</summary>
            public static readonly PixelFormat UNormSRgb = new PixelFormat(Format.B8G8R8A8_UNorm_SRgb);

            #endregion
        }

        /// <summary>The b8 g8 r8 x8 class.</summary>
        public static class B8G8R8X8
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.B8G8R8X8_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.B8G8R8X8_UNorm);

            /// <summary>The authentication norm arguments RGB.</summary>
            public static readonly PixelFormat UNormSRgb = new PixelFormat(Format.B8G8R8X8_UNorm_SRgb);

            #endregion
        }

        /// <summary>The bc1 class.</summary>
        public static class BC1
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.BC1_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.BC1_UNorm);

            /// <summary>The authentication norm arguments RGB.</summary>
            public static readonly PixelFormat UNormSRgb = new PixelFormat(Format.BC1_UNorm_SRgb);

            #endregion
        }

        /// <summary>The bc2 class.</summary>
        public static class BC2
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.BC2_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.BC2_UNorm);

            /// <summary>The authentication norm arguments RGB.</summary>
            public static readonly PixelFormat UNormSRgb = new PixelFormat(Format.BC2_UNorm_SRgb);

            #endregion
        }

        /// <summary>The bc3 class.</summary>
        public static class BC3
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.BC3_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.BC3_UNorm);

            /// <summary>The authentication norm arguments RGB.</summary>
            public static readonly PixelFormat UNormSRgb = new PixelFormat(Format.BC3_UNorm_SRgb);

            #endregion
        }

        /// <summary>The bc4 class.</summary>
        public static class BC4
        {
            #region Constants and Fields

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.BC4_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.BC4_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.BC4_UNorm);

            #endregion
        }

        /// <summary>The bc5 class.</summary>
        public static class BC5
        {
            #region Constants and Fields

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.BC5_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.BC5_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.BC5_UNorm);

            #endregion
        }

        /// <summary>The bc6h class.</summary>
        public static class BC6H
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.BC6H_Typeless);

            #endregion
        }

        /// <summary>The bc7 class.</summary>
        public static class BC7
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.BC7_Typeless);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.BC7_UNorm);

            /// <summary>The authentication norm arguments RGB.</summary>
            public static readonly PixelFormat UNormSRgb = new PixelFormat(Format.BC7_UNorm_SRgb);

            #endregion
        }

        /// <summary>The R10 G10 B10 a2 class.</summary>
        public static class R10G10B10A2
        {
            #region Constants and Fields

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R10G10B10A2_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R10G10B10A2_UInt);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.R10G10B10A2_UNorm);

            #endregion
        }

        /// <summary>The R11 G11 B10 class.</summary>
        public static class R11G11B10
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R11G11B10_Float);

            #endregion
        }

        /// <summary>The R16 class.</summary>
        public static class R16
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R16_Float);

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R16_SInt);

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.R16_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R16_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R16_UInt);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.R16_UNorm);

            #endregion
        }

        /// <summary>The R16 G16 class.</summary>
        public static class R16G16
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R16G16_Float);

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R16G16_SInt);

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.R16G16_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R16G16_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R16G16_UInt);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.R16G16_UNorm);

            #endregion
        }

        /// <summary>The R16 G16 B16 a16 class.</summary>
        public static class R16G16B16A16
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R16G16B16A16_Float);

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R16G16B16A16_SInt);

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.R16G16B16A16_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R16G16B16A16_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R16G16B16A16_UInt);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.R16G16B16A16_UNorm);

            #endregion
        }

        /// <summary>The R32 class.</summary>
        public static class R32
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R32_Float);

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R32_SInt);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R32_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R32_UInt);

            #endregion
        }

        /// <summary>The R32 G32 class.</summary>
        public static class R32G32
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R32G32_Float);

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R32G32_SInt);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R32G32_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R32G32_UInt);

            #endregion
        }

        /// <summary>The R32 G32 B32 class.</summary>
        public static class R32G32B32
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R32G32B32_Float);

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R32G32B32_SInt);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R32G32B32_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R32G32B32_UInt);

            #endregion
        }

        /// <summary>The R32 G32 B32 a32 class.</summary>
        public static class R32G32B32A32
        {
            #region Constants and Fields

            /// <summary>The pixel format float.</summary>
            public static readonly PixelFormat Float = new PixelFormat(Format.R32G32B32A32_Float);

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R32G32B32A32_SInt);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R32G32B32A32_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R32G32B32A32_UInt);

            #endregion
        }

        /// <summary>The r8 class.</summary>
        public static class R8
        {
            #region Constants and Fields

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R8_SInt);

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.R8_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R8_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R8_UInt);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.R8_UNorm);

            #endregion
        }

        /// <summary>The r8 g8 class.</summary>
        public static class R8G8
        {
            #region Constants and Fields

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R8G8_SInt);

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.R8G8_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R8G8_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R8G8_UInt);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.R8G8_UNorm);

            #endregion
        }

        /// <summary>The r8 g8 b8 a8 class.</summary>
        public static class R8G8B8A8
        {
            #region Constants and Fields

            /// <summary>The pixel format int.</summary>
            public static readonly PixelFormat SInt = new PixelFormat(Format.R8G8B8A8_SInt);

            /// <summary>The pixel format S norm.</summary>
            public static readonly PixelFormat SNorm = new PixelFormat(Format.R8G8B8A8_SNorm);

            /// <summary>The pixel format typeless.</summary>
            public static readonly PixelFormat Typeless = new PixelFormat(Format.R8G8B8A8_Typeless);

            /// <summary>The pixel format unsigned int.</summary>
            public static readonly PixelFormat UInt = new PixelFormat(Format.R8G8B8A8_UInt);

            /// <summary>The pixel format U norm.</summary>
            public static readonly PixelFormat UNorm = new PixelFormat(Format.R8G8B8A8_UNorm);

            /// <summary>The pixel format norm S RGB.</summary>
            public static readonly PixelFormat UNormSRgb = new PixelFormat(Format.R8G8B8A8_UNorm_SRgb);

            #endregion
        }

        /// <summary>Performs an implicit conversion from <see cref="PixelFormat" /> to <see cref="Format" />.</summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Format(PixelFormat from)
        {
            return from.Value;
        }

        /// <summary>Performs an implicit conversion from <see cref="Format" /> to <see cref="PixelFormat" />.</summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PixelFormat(Format from)
        {
            return new PixelFormat(from);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(PixelFormat other)
        {
            return Value == other.Value;
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PixelFormat && Equals((PixelFormat) obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>Implements the ==.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PixelFormat left, PixelFormat right)
        {
            return left.Equals(right);
        }

        /// <summary>Implements the !=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PixelFormat left, PixelFormat right)
        {
            return !left.Equals(right);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}", Value);
        }
    }
}