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
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using Lucene.Net.Store;

namespace SharpDocPak
{
    /// <summary>
    /// Archive containing files and parameters for documentation.
    /// This archive is appended to the end of the SharpDocPak executable in view mode.
    /// </summary>
    [Serializable]
    internal class Archive
    {
        public const string DefaultHtmlRoot = "index.htm";
        public const string DefaultHtmlRootAlternate = "index.html";

        /// <summary>
        /// Initializes a new instance of the <see cref="Archive"/> class.
        /// </summary>
        public Archive()
        {
            Tags = new List<TagIndex>();
            Files = new Dictionary<string, byte[]>();
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public List<TagIndex> Tags { get; set; }

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>The files.</value>
        public Dictionary<string, byte[]> Files { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public RAMDirectory Index { get; set; }

        /// <summary>
        /// Gets the default HTML file.
        /// </summary>
        /// <value>The default HTML file.</value>
        public string DefaultHtmlFile
        {
            get
            {
                if (Files.ContainsKey("/" + DefaultHtmlRoot))
                    return DefaultHtmlRoot;
                return DefaultHtmlRootAlternate;
            }
        }

        /// <summary>
        /// Checks if an archive is present at the end of the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An offset from the stream > 0, -1 if the archive is not in the stream</returns>
        public static long CheckArchive(Stream stream)
        {
            var savedPosition = stream.Position;
            try
            {
                var reader = new BinaryReader(stream);
                if (stream.Length < 4)
                    return -1;

                stream.Seek(-4, SeekOrigin.End);

                var signature = reader.ReadInt32();
                if (signature != SignatureCheck)
                    return -1;

                // Retrieve the size of the archive
                stream.Seek(-8, SeekOrigin.End);
                int sizeOfArchiveInBytes = reader.ReadInt32();

                // Jump to the beginning of the archive
                stream.Seek(-sizeOfArchiveInBytes-8, SeekOrigin.End);

                return stream.Position;
            } finally
            {
                stream.Position = savedPosition;
            }
        }

        /// <summary>
        /// Reads an <see cref="Archive"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An instance of <see cref="Archive"/></returns>
        public static Archive Read(Stream stream)
        {
            long offset = CheckArchive(stream);
            if (offset >= 0)
            {
                stream.Position = offset;
                var formatter = new BinaryFormatter();
                var compressedStream = new GZipStream(stream, CompressionMode.Decompress);
                return (Archive)formatter.Deserialize(compressedStream);
            }
            return null;
        }

        /// <summary>
        /// Gets the archive from the current executable.
        /// </summary>
        /// <returns></returns>
        public static Archive GetFromCurrentExecutable()
        {
            var input = new FileStream(typeof(Archive).Assembly.Location, FileMode.Open, FileAccess.Read);
            return Read(input);
        }

        /// <summary>
        /// Appends this instance to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="outputStream">The output stream.</param>
        public void Append(Stream inputStream, Stream outputStream)
        {
            long length = CheckArchive(inputStream);
            if (length < 0)
                length = inputStream.Length;

            // Copy from input stream to output stream
            Utility.CopyStream(inputStream, outputStream, length);

            var formatter = new BinaryFormatter();
            long startOffset = outputStream.Position;
            var compressStream = new GZipStream(outputStream, CompressionMode.Compress, true);
            formatter.Serialize(compressStream, this);
            compressStream.Flush();
            compressStream.Close();

            int sizeOfArchiveInBytes = (int)(outputStream.Position - startOffset);
            Console.WriteLine("Size compressed: {0}", sizeOfArchiveInBytes);
            var writer = new BinaryWriter(outputStream);
            writer.Write(sizeOfArchiveInBytes);
            writer.Write(SignatureCheck);
        }

        private const int SignatureCheck = 0x434F4423;
    }
}