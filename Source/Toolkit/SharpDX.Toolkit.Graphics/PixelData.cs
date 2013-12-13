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

using System.Runtime.InteropServices;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Provides typed structure to read and write to some <see cref="PixelFormat"/>.
    /// </summary>
    public static class PixelData
    {
        internal static byte ToByte(float component)
        {
            var value = (int) (component*255.0f);
            return (byte)(value < 0 ? 0 : value > 255 ? 255 : value);
        }

        /// <summary>
        /// Pixel format associated to <see cref="PixelFormat.R8.UNorm"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct R8 : IPixelData
        {
            public byte R;

            public R8(byte r)
            {
                this.R = r;
            }

            public PixelFormat Format
            {
                get { return PixelFormat.R8.UNorm; }
            }

            public Color4 Value
            {
                get { return new Color4(R/255.0f, 0, 0, 1.0f); }
                set { R = ToByte(value.Red); }
            }

            public Color Value32Bpp
            {
                get { return new Color(R, (byte)0, (byte)0, (byte)255); }
                set { R = value.R; }
            }

            public override string ToString()
            {
                return string.Format("R:{0}", R);
            }
        }

        /// <summary>
        /// Pixel format associated to <see cref="PixelFormat.R8G8.UNorm"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 2)]
        public struct R8G8 : IPixelData
        {
            public byte R, G;

            public PixelFormat Format
            {
                get { return PixelFormat.R8G8.UNorm; }
            }

            public Color4 Value
            {
                get { return new Color4(R / 255.0f, G / 255.0f, 0, 1.0f); }
                set
                {
                    R = ToByte(value.Red);
                    G = ToByte(value.Green);
                }
            }

            public Color Value32Bpp
            {
                get { return new Color(R, G, (byte)0, (byte)255); }
                set
                {
                    R = value.R;
                    G = value.G;
                }
            }

            public override string ToString()
            {
                return string.Format("R:{0}, G:{1}", R, G);
            }
        }

        /// <summary>
        /// Pixel format associated to <see cref="PixelFormat.R8G8B8A8.UNorm"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 4)]
        public struct R8G8B8A8 : IPixelData
        {
            public byte R, G, B, A;

            public PixelFormat Format
            {
                get { return PixelFormat.R8G8B8A8.UNorm; }
            }

            public Color4 Value
            {
                get { return new Color4(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f); }
                set
                {
                    R = ToByte(value.Red);
                    G = ToByte(value.Green);
                    B = ToByte(value.Blue);
                    A = ToByte(value.Alpha);
                }
            }

            public Color Value32Bpp
            {
                get { return new Color(R, G, B, A); }
                set
                {
                    R = value.R;
                    G = value.G;
                    B = value.B;
                    A = value.A;
                }
            }

            public override string ToString()
            {
                return string.Format("R:{0}, G:{1}, B:{2}, A:{3}", R, G, B, A);
            }
        }

        /// <summary>
        /// Pixel format associated to <see cref="PixelFormat.R16.UNorm"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct R16 : IPixelData
        {
            public Half R;

            public PixelFormat Format
            {
                get { return PixelFormat.R16.UNorm; }
            }

            public Color4 Value
            {
                get { return new Color4(R / 255.0f, 0, 0, 1.0f); }
                set { R = new Half(value.Red); }
            }

            public Color Value32Bpp
            {
                get { return new Color(R, 0, 0, 1); }
                set { R = new Half(value.R / 255.0f); }
            }

            public override string ToString()
            {
                return string.Format("R:{0}", R);
            }
        }

        /// <summary>
        /// Pixel format associated to <see cref="PixelFormat.R16G16.UNorm"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 2)]
        public struct R16G16 : IPixelData
        {
            public Half R, G;

            public PixelFormat Format
            {
                get { return PixelFormat.R16G16.UNorm; }
            }

            public Color4 Value
            {
                get { return new Color4(R / 255.0f, G / 255.0f, 0, 1.0f); }
                set
                {
                    R = new Half(value.Red);
                    G = new Half(value.Green);
                }

            }

            public Color Value32Bpp
            {
                get { return new Color(R, G, 0, 1); }
                set
                {
                    R = new Half(value.R / 255.0f);
                    G = new Half(value.G / 255.0f);
                }
            }

            public override string ToString()
            {
                return string.Format("R:{0}, G:{1}", R, G);
            }
        }

        /// <summary>
        /// Pixel format associated to <see cref="PixelFormat.R16G16B16A16.UNorm"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 4)]
        public struct R16G16B16A16 : IPixelData
        {
            public Half R, G, B, A;

            public PixelFormat Format
            {
                get { return PixelFormat.R16G16B16A16.UNorm; }
            }

            public Color4 Value
            {
                get { return new Color4(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f); }
                set
                {
                    R = new Half(value.Red);
                    G = new Half(value.Green);
                    B = new Half(value.Blue);
                    A = new Half(value.Alpha);
                }

            }

            public Color Value32Bpp
            {
                get { return new Color(R, G, B, A); }
                set
                {
                    R = new Half(value.R / 255.0f);
                    G = new Half(value.G / 255.0f);
                    B = new Half(value.B / 255.0f);
                    A = new Half(value.A / 255.0f);
                }
            }

            public override string ToString()
            {
                return string.Format("R:{0}, G:{1}, B:{2}, A:{3}", R, G, B, A);
            }
        }
    }
}