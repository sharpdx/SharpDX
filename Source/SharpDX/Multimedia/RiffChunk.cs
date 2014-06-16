// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.IO;

namespace SharpDX.Multimedia
{
    /// <summary>
    /// A chunk of a Riff stream.
    /// </summary>
    public class RiffChunk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RiffChunk"/> class.
        /// </summary>
        /// <param name="stream">The stream holding this chunk</param>
        /// <param name="type">The type.</param>
        /// <param name="size">The size.</param>
        /// <param name="dataPosition">The data offset.</param>
        /// <param name="isList">if set to <c>true</c> [is list].</param>
        /// <param name="isHeader">if set to <c>true</c> [is header].</param>
        public RiffChunk(Stream stream, FourCC type, uint size, uint dataPosition, bool isList = false, bool isHeader = false)
        {
            Stream = stream;
            Type = type;
            Size = size;
            DataPosition = dataPosition;
            IsList = isList;
            IsHeader = isHeader;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public Stream Stream{ get; private set; }

        /// <summary>
        /// Gets the <see cref="FourCC"/> of this chunk.
        /// </summary>
        public FourCC Type { get; private set; }

        /// <summary>
        /// Gets the size of the data embedded by this chunk.
        /// </summary>
        public uint Size { get; private set; }

        /// <summary>
        /// Gets the position of the data embedded by this chunk relative to the stream.
        /// </summary>
        public uint DataPosition { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a list chunk.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is list; otherwise, <c>false</c>.
        /// </value>
        public bool IsList { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is a header chunk.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a header; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeader { get; private set; }

        /// <summary>
        /// Gets the raw data contained in this chunk.
        /// </summary>
        /// <returns></returns>
        public byte[] GetData()
        {
            var data = new byte[Size];
            Stream.Position = DataPosition;
            Stream.Read(data, 0, (int)Size);
            return data;
        }

        /// <summary>
        /// Gets structured data contained in this chunk.
        /// </summary>
        /// <typeparam name="T">The type of the data to return</typeparam>
        /// <returns>
        /// A structure filled with the chunk data
        /// </returns>
        public unsafe T GetDataAs<T>() where T : struct
        {
            var value = new T();
            var data = GetData();
            fixed (void* ptr = data)
            {
                Utilities.Read((IntPtr) ptr, ref value);
            }
            return value;
        }

        /// <summary>
        /// Gets structured data contained in this chunk.
        /// </summary>
        /// <typeparam name="T">The type of the data to return</typeparam>
        /// <returns>A structure filled with the chunk data</returns>
        public unsafe T[] GetDataAsArray<T>() where T : struct
        {
            int sizeOfT = Utilities.SizeOf<T>();
            if ((Size % sizeOfT) != 0)
                throw new ArgumentException("Size of T is incompatible with size of chunk");

            var values = new T[Size/sizeOfT];
            var data = GetData();
            fixed (void* ptr = data)
            {
                Utilities.Read((IntPtr)ptr, values, 0, values.Length);
            }
            return values;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Type: {0}, Size: {1}, Position: {2}, IsList: {3}, IsHeader: {4}", Type, Size, DataPosition, IsList, IsHeader);
        }
    }
}