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

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class SpriteFontData
    {
        /// <summary>
        /// Describes bitmap font data.
        /// </summary>
        public class Bitmap : IDataSerializable
        {
            /// <summary>
            /// The actual data of the bitmap. See remarks.
            /// </summary>
            /// <remarks>
            /// When loading bitmap from a DirectXTk "MakeSpriteFont/tkfont" exe, this field will contain a <see cref="BitmapData"/>.
            /// When loading from an AngelCode BMFont, this field will contain a string representing the name of the external texture to load.
            /// </remarks>
            public object Data;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                if (serializer.Mode == SerializerMode.Read)
                {
                    var data = new BitmapData();
                    serializer.Serialize(ref data);
                    Data = data;
                }
                else
                {
                    var data = (BitmapData)Data;
                    serializer.Serialize(ref data);
                }
            }
        }

        /// <summary>
        /// Bitmap data.
        /// </summary>
        public class BitmapData : IDataSerializable
        {
            /// <summary>
            /// Wisth of the bitmap.
            /// </summary>
            public int Width;

            /// <summary>
            /// Height of the bitmap.
            /// </summary>
            public int Height;

            /// <summary>
            /// Format of the pixel.
            /// </summary>
            public SharpDX.DXGI.Format PixelFormat;

            /// <summary>
            /// Srite in bytes of a row of pixels.
            /// </summary>
            public int RowStride;

            /// <summary>
            /// Number of rowstride (may be less than <see cref="Height"/> when using compressed format.
            /// </summary>
            public int CompressedHeight;

            /// <summary>
            /// Actual raw data stored in <see cref="PixelFormat"/> format.
            /// </summary>
            public byte[] Data;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Width);
                serializer.Serialize(ref Height);
                serializer.SerializeEnum(ref PixelFormat);
                serializer.Serialize(ref RowStride);
                serializer.Serialize(ref CompressedHeight);
                serializer.Serialize(ref Data, RowStride * CompressedHeight);
            }
        }
    }
}