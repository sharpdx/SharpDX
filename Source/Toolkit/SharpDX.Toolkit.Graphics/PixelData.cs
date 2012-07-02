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

            public PixelFormat Format
            {
                get { return PixelFormat.R8.UNorm; }
            }

            public void Write(byte red, byte green, byte blue, byte alpha)
            {
                R = red;
            }

            public void Write(byte alpha)
            {
            }

            public void Write(Color4 color)
            {
                R = ToByte(color.Red);
            }

            public void Write(float alpha)
            {
            }

            public float Red { get { return R/255.0f; } }

            public float Green { get { return 0.0f; } }

            public float Blue { get { return 0.0f; } }

            public float Alpha { get { return 1.0f; } }

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

            public void Write(byte red, byte green, byte blue, byte alpha)
            {
                R = red;
                G = green;
            }

            public void Write(byte alpha)
            {
            }

            public void Write(Color4 color)
            {
                R = ToByte(color.Red);
                G = ToByte(color.Green);
            }

            public void Write(float alpha)
            {
            }

            public float Red { get { return R / 255.0f; } }

            public float Green { get { return G / 255.0f; } }

            public float Blue { get { return 0.0f; } }

            public float Alpha { get { return 1.0f; } }

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

            public void Write(byte red, byte green, byte blue, byte alpha)
            {
                R = red;
                G = green;
                B = blue;
                A = alpha;
            }

            public void Write(byte alpha)
            {
                A = alpha;
            }

            public void Write(Color4 color)
            {
                R = ToByte(color.Red);
                G = ToByte(color.Green);
                B = ToByte(color.Blue);
                A = ToByte(color.Alpha);
            }

            public void Write(float alpha)
            {
            }

            public float Red { get { return R / 255.0f; } }

            public float Green { get { return G / 255.0f; } }

            public float Blue { get { return B / 255.0f; } }

            public float Alpha { get { return A / 255.0f; } }

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

            public void Write(byte red, byte green, byte blue, byte alpha)
            {
                R = new Half(red / 255.0f);
            }

            public void Write(byte alpha)
            {
            }

            public void Write(Color4 color)
            {
                R = new Half(color.Red);
            }

            public void Write(float alpha)
            {
            }

            public float Red { get { return R / 255.0f; } }

            public float Green { get { return 0.0f; } }

            public float Blue { get { return 0.0f; } }

            public float Alpha { get { return 1.0f; } }

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

            public void Write(byte red, byte green, byte blue, byte alpha)
            {
                R = new Half(red / 255.0f);
                G = new Half(green / 255.0f);
            }

            public void Write(byte alpha)
            {
            }

            public void Write(Color4 color)
            {
                R = new Half(color.Red);
                G = new Half(color.Green);
            }

            public void Write(float alpha)
            {
            }

            public float Red { get { return R / 255.0f; } }

            public float Green { get { return G / 255.0f; } }

            public float Blue { get { return 0.0f; } }

            public float Alpha { get { return 1.0f; } }

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

            public void Write(byte red, byte green, byte blue, byte alpha)
            {
                R = new Half(red / 255.0f);
                G = new Half(green / 255.0f);
                B = new Half(blue / 255.0f);
                A = new Half(alpha / 255.0f);
            }

            public void Write(byte alpha)
            {
                A = new Half(alpha / 255.0f);
            }

            public void Write(Color4 color)
            {
                R = new Half(color.Red);
                G = new Half(color.Green);
                B = new Half(color.Blue);
                A = new Half(color.Alpha);
            }

            public void Write(float alpha)
            {
            }

            public float Red { get { return R / 255.0f; } }

            public float Green { get { return G / 255.0f; } }

            public float Blue { get { return B / 255.0f; } }

            public float Alpha { get { return A / 255.0f; } }

            public override string ToString()
            {
                return string.Format("R:{0}, G:{1}, B:{2}, A:{3}", R, G, B, A);
            }
        }
    }
}