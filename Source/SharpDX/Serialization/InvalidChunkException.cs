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
using SharpDX.Multimedia;

namespace SharpDX.Serialization
{
    /// <summary>
    /// Exceptions thrown when an invalid chunk is decoded.
    /// </summary>
    public class InvalidChunkException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.
        /// </summary>
        /// <param name="chunkId">The chunk id.</param>
        /// <param name="expectedChunkId">The expected chunk id.</param>
        public InvalidChunkException(FourCC chunkId, FourCC expectedChunkId)
            : base(string.Format("Unexpected chunk [{0}/0x{1:X}] instead of [{2}/0x{3:X}]", chunkId, (int)chunkId, expectedChunkId, (int)expectedChunkId))
        {
            this.ChunkId = chunkId;
            this.ExpectedChunkId = expectedChunkId;
        }

        /// <summary>
        /// Gets the chunk id.
        /// </summary>
        /// <value>The chunk id.</value>
        public FourCC ChunkId { get; private set; }

        /// <summary>
        /// Gets the expected chunk id.
        /// </summary>
        /// <value>The expected chunk id.</value>
        public FourCC ExpectedChunkId { get; private set; }
    }
}