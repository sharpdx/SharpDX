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

namespace SharpDX.XAudio2
{
    public partial class AudioBuffer
    {
        private DataStream _dataStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBuffer" /> class.
        /// </summary>
        public AudioBuffer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBuffer" /> class.
        /// </summary>
        /// <param name="stream">The stream to get the audio buffer from.</param>
        public AudioBuffer(DataStream stream)
        {
            Stream = stream;
            Flags = BufferFlags.EndOfStream;
            AudioBytes = (int)stream.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBuffer" /> class.
        /// </summary>
        /// <param name="dataBuffer">The buffer to get the audio buffer from.</param>
        public AudioBuffer(DataPointer dataBuffer)
        {
            AudioDataPointer = dataBuffer.Pointer;
            Flags = BufferFlags.EndOfStream;
            AudioBytes = dataBuffer.Size;
        }

        /// <summary>
        /// Gets or sets the data stream associated to this audio buffer
        /// </summary>
        /// <value>The stream.</value>
        public DataStream Stream
        {
            get
            {
                return _dataStream;
            }
            set
            {
                _dataStream = value;
                this.AudioDataPointer = _dataStream.PositionPointer;
            }
        }
    }
}