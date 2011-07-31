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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SharpDX.Multimedia
{
    /// <summary>
    /// Riff chunk enumerator.
    /// </summary>
    public class RiffParser : IEnumerator<RiffChunk>, IEnumerable<RiffChunk>
    {
        private readonly Stream input;
        private readonly BinaryReader reader;
        private readonly Stack<RiffChunk> path;
        private bool descendNext;
        private bool isEndOfRiff;
        private bool isErrorState;
        private uint length;

        /// <summary>
        /// Initializes a new instance of the <see cref="RiffParser"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="length"></param>
        public RiffParser(Stream input, uint length=0u)
        {
            this.input = input;
            this.length = (length == 0) ? (uint)input.Length : length;
            this.reader = new BinaryReader(input);
            this.path = new Stack<RiffChunk>();
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            CheckState();

            if (current != null)
            {
                if (descendNext)
                {
                    descendNext = false;
                } else
                {
                    long nextOffset = current.DataOffset + current.Size;

                    // Pad DWORD
                    if ((nextOffset & 1) != 0)
                        nextOffset++;

                    input.Position = nextOffset;
                }

                var currentChunkContainer = path.Peek();
                long endOfOuterChunk = currentChunkContainer.DataOffset + currentChunkContainer.Size;
                if (input.Position >= endOfOuterChunk)
                    path.Pop();

                if (path.Count == 0)
                    return !(isEndOfRiff = true);
            }

            var fourCC = ((FourCC) reader.ReadUInt32());
            bool isList = (fourCC == "LIST");
            bool isHeader = (fourCC == "RIFF");
            uint chunkSize = 0;

            if (input.Position == 4 && !isHeader)
            {
                isErrorState = true;
                throw new InvalidOperationException("Invalid RIFF file format");
            }

            // Read chunk size
            chunkSize = reader.ReadUInt32();

            // If list or header
            if (isList || isHeader)
            {
                // Check filesize
                if (isHeader && chunkSize > (input.Length - 8))
                {
                    isErrorState = true;
                    throw new InvalidOperationException("Invalid RIFF file format");
                }
                chunkSize -= 4;
                fourCC = reader.ReadUInt32();
            }

            // Read RIFF type and create chunk
            current = new RiffChunk(fourCC, chunkSize, (uint)input.Position, isList, isHeader);
            return true;
        }

        private void CheckState()
        {
            if (isEndOfRiff)
                throw new InvalidOperationException("End of Riff. Cannot MoveNext");

            if (isErrorState)
                throw new InvalidOperationException("The enumerator is in an error state");
        }

        public Stack<RiffChunk> CurrentPath { get { return path; } }

        public void Reset()
        {
            CheckState();
            current = null;
            input.Position = 0;
        }

        object IEnumerator.Current
        {
            get
            {
                CheckState();
                return Current;
            }
        }

        public void Ascend()
        {
            CheckState();
            var outerChunk = path.Pop();
            input.Position = outerChunk.DataOffset + outerChunk.Size;
        }

        public void Descend()
        {
            CheckState();
            path.Push(current);
            descendNext = true;
        }

        private RiffChunk current;
        public RiffChunk Current { 
            get
            {
                CheckState();
                return current;
            }         
        }

        public IEnumerator<RiffChunk> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}