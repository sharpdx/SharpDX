// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

using System.Collections.Generic;
using System.IO;

namespace SharpDX.Multimedia
{
    /// <summary>
    /// A stream able to load a xWma.
    /// </summary>
    public class XWMAStream : RiffStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XWMAStream"/> class.
        /// </summary>
        /// <param name="xwmaFile">The xwma file.</param>
        public XWMAStream(string xwmaFile) : base(xwmaFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XWMAStream"/> class.
        /// </summary>
        /// <param name="stream">The xwma stream.</param>
        public XWMAStream(Stream stream) : base(stream)
        {
        }

        /// <summary>
        /// Initializes the specified stream.
        /// </summary>
        protected override unsafe void Initialize(IEnumerable<RiffChunk> chunks)
        {
            // Check for a "fmt" chunk
            var fmtChunk = Chunk(chunks, "fmt ");

            if (fmtChunk.Size < sizeof(WaveFormat.__Native))
                ThrowInvalidFileFormat();

            // Get the WaveFormat from the fmt chunk
            if (fmtChunk.Size == sizeof(WaveFormat.__Native))
            {
                WaveFormat = new WaveFormat();
                var waveFormatNative = fmtChunk.GetDataAs<WaveFormat.__Native>();
                WaveFormat.__MarshalFrom(ref waveFormatNative);
            }
            else if (fmtChunk.Size < sizeof(WaveFormatExtensible.__Native))
            {
                ThrowInvalidFileFormat();
            }
            else
            {
                // TODO better check Extrasize before marshaling?
                var waveFormatEx = new WaveFormatExtensible();
                var waveFormatNative = fmtChunk.GetDataAs<WaveFormatExtensible.__Native>();
                waveFormatEx.__MarshalFrom(ref waveFormatNative);
                WaveFormat = waveFormatEx;
            }

            // Check that format is Wma
            if (WaveFormat.Encoding != WaveFormatEncoding.Wmaudio2 &&  WaveFormat.Encoding != WaveFormatEncoding.Wmaudio3)
                ThrowInvalidFileFormat();

            // Check for "dpds" chunk
            // Get the dpds decoded packed cumulative bytes
            var dpdsChunk = Chunk(chunks, "dpds");
            DecodedPacketsInfo = dpdsChunk.GetDataAsArray<uint>();
        }

        protected override string FileFormatName
        {
            get { return "XMWA"; }
        }

        /// <summary>
        /// Gets the decoded packets info.
        /// </summary>
        public uint[] DecodedPacketsInfo { get; private set; }
    }
}