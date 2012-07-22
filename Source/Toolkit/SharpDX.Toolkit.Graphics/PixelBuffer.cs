using System;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// 
    /// </summary>
    public struct PixelBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PixelBuffer" /> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">The format.</param>
        /// <param name="rowPitch">The row pitch.</param>
        /// <param name="slicePitch">The slice pitch.</param>
        /// <param name="pixels">The pixels.</param>
        public PixelBuffer(int width, int height, int depth, Format format, int rowPitch, int slicePitch, IntPtr pixels)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Format = format;
            RowPitch = rowPitch;
            SlicePitch = slicePitch;
            Pixels = pixels;
            PixelSizeInBytes = (int)FormatHelper.SizeOfInBytes(Format);
        }

        public readonly int Width;

        public readonly int Height;

        public readonly int Depth;

        public readonly DXGI.Format Format;

        public readonly int RowPitch;

        public readonly int SlicePitch;

        public readonly IntPtr Pixels;

        private readonly int PixelSizeInBytes;

        /*
        public unsafe T GetPixelAt<T>(int x, int y) where T : struct
        {
            //T data = default(T);

            //(byte*) Pixels + RowPitch * y + x * PixelSizeInBytes;

        }

        public void SetPixelAt<T>(int x, int y, T value) where T : struct
        {

        }

        public T[] ReadPixels<T>() where T : struct
        {

        }


        public void ReadPixels<T>(T[] pixels) where T : struct
        {

        }

        public void WritePixels<T>(T[] pixels) where T : struct
        {

        }
         * */

    }
}