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
using System.IO;
using Microsoft.Win32.SafeHandles;
using SharpDX.Win32;

namespace SharpDX.XACT3
{
    public partial class WaveBank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaveBank"/> class from a wave bank stream.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="stream">The wave bank stream.</param>
        /// <unmanaged>HRESULT IXACT3Engine::CreateInMemoryWaveBank([In] const void* pvBuffer,[In] unsigned int dwSize,[In] unsigned int dwFlags,[In] unsigned int dwAllocAttributes,[Out, Fast] IXACT3WaveBank** ppWaveBank)</unmanaged>
        public WaveBank(Engine engine, Stream stream)
        {
            if (stream is DataStream)
            {
                engine.CreateInMemoryWaveBank(((DataStream) stream).DataPointer, (int) stream.Length, 0, 0, this);
                return;
            }

            var data = Utilities.ReadStream(stream);
            unsafe
            {
                fixed (void* pData = data)
                    engine.CreateInMemoryWaveBank((IntPtr)pData, data.Length, 0, 0, this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveBank"/> class from a file for async reading.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="fileName">Name of the file to load the wavebank from.</param>
        /// <param name="offset">The offset into the stream.</param>
        /// <param name="packetSize">Packet size used to load the stream.</param>
        public WaveBank(Engine engine, string fileName, int offset, short packetSize)
        {
            var handle = FileHelper.CreateFile(fileName, NativeFileAccess.GenericRead, NativeFileShare.Read | NativeFileShare.Write,
                                  IntPtr.Zero, NativeFileCreationDisposition.OpenExisting,
                                  NativeFileAttributes.Normal | NativeFileAttributes.NoBuffering |
                                  NativeFileAttributes.Overlapped | NativeFileAttributes.SequentialScan, IntPtr.Zero);


            if (handle == IntPtr.Zero || handle.ToInt32() == -1)
                throw new FileNotFoundException("Unable to open the specified file.", fileName);


            var streamingParameters = new StreamingParameters {File = handle, Flags = 0, Offset = offset, PacketSize = packetSize};

            engine.CreateStreamingWaveBank(streamingParameters, this);
            FileStreamHandle = new SafeFileHandle(handle, true);
        }


        protected override void Dispose(bool disposing)
        {
            if (FileStreamHandle != null && !FileStreamHandle.IsInvalid)
            {
                FileStreamHandle.Dispose();
                FileStreamHandle = null;
            }
            base.Dispose(disposing);
        }

        internal SafeFileHandle FileStreamHandle { get; set; }
    }
}

