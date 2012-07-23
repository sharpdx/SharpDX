using System;
using System.IO;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// An unmanaged pixels buffer .
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

        /// <summary>
        /// Saves this pixel buffer to a file.
        /// </summary>
        /// <param name="fileName">The destination file.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        public void Save(string fileName, ImageFileType fileType)
        {
            using (var imageStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                Save(imageStream, fileType);
            }
        }

        /// <summary>
        /// Saves this pixel buffer to a stream.
        /// </summary>
        /// <param name="imageStream">The destination stream.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        public void Save(Stream imageStream, ImageFileType fileType)
        {
            var description = new ImageDescription()
                {
                    Width = Width,
                    Height = Height,
                    Depth = 1,
                    ArraySize = 1,
                    Dimension = TextureDimension.Texture2D,
                    Format = Format,
                    MipLevels = 1,
                };
            Save(new [] {this}, 1, description, imageStream, fileType);
        }

        /// <summary>
        /// Saves this instance to a stream.
        /// </summary>
        /// <param name="pixelBuffers">The buffers to save.</param>
        /// <param name="count">The number of buffers to save.</param>
        /// <param name="description">Global description of the buffer.</param>
        /// <param name="imageStream">The destination stream.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        internal static void Save(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream, ImageFileType fileType)
        {
            switch (fileType)
            {
                case ImageFileType.Dds:
                    DDSHelper.SaveToDDSStream(pixelBuffers, count, description, DDSFlags.ForceDX10Ext, imageStream);
                    break;
                case ImageFileType.Gif:
                case ImageFileType.Tiff:
                    WICHelper.SaveToWICMemory(pixelBuffers, count, WICFlags.AllFrames, fileType, imageStream);
                    break;
                case ImageFileType.Bmp:
                case ImageFileType.Jpg:
                case ImageFileType.Png:
                case ImageFileType.Wmp:
                    WICHelper.SaveToWICMemory(pixelBuffers, 1, WICFlags.None, fileType, imageStream);
                    break;
                default:
                    throw new NotSupportedException("This file format is not yet implemented.");
            }
        }

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