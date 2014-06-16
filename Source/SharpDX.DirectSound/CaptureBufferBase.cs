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
using SharpDX.Multimedia;

namespace SharpDX.DirectSound
{
    public partial class CaptureBufferBase
    {
        /// <summary>
        /// True if the buffer is currently capturing.
        /// </summary>
        public bool Capturing
        {
            get
            {
                return (CaptureStatus & CaptureBufferStatusFlags.Capturing) != 0;
            }
        }

        /// <summary>
        /// The offset from the start of the buffer, in bytes, of the capture cursor.
        /// </summary>
        public int CurrentCapturePosition
        {
            get
            {
                int capturePosition;
                int realPosition;
                GetCurrentPosition(out capturePosition, out realPosition);
                return capturePosition;
            }
        }

        /// <summary>
        /// The offset from the start of the buffer, in bytes, of the read cursor.
        /// </summary>
        public int CurrentRealPosition
        {
            get
            {
                int capturePosition;
                int realPosition;
                GetCurrentPosition(out capturePosition, out realPosition);
                return realPosition;
            }
        }

        /// <summary>
        /// True if the capture buffer is looping.
        /// </summary>
        public bool Looping
        {

            get
            {
                return (CaptureStatus & CaptureBufferStatusFlags.Looping) != 0;
            }
        }

        /// <summary>
        /// Gets the waveform format of the capture buffer.
        /// </summary>
        /// <value>The format.</value>
        public WaveFormat Format
        {
            get
            {
                unsafe
                {
                    WaveFormatExtensible.__Native native = WaveFormatExtensible.__NewNative();
                    int sizeOfInputStructure = Utilities.SizeOf<WaveFormatExtensible.__Native>();
                    int sizeOfStructureReturned;
                    GetFormat(new IntPtr(&native), sizeOfInputStructure, out sizeOfStructureReturned);
                    return WaveFormat.MarshalFrom(new IntPtr(&native));
                }
            }
        }

        /// <summary>	
        /// The Lock method locks a portion of the buffer. Locking the buffer returns references into the buffer, allowing the application to read or write audio data into memory.	
        /// </summary>	
        /// <param name="offset">Offset, in bytes, from the start of the buffer to the point where the lock begins. </param>
        /// <param name="sizeBytes">Size, in bytes, of the portion of the buffer to lock. Because the buffer is conceptually circular, this number can exceed the number of bytes between dwOffset and the end of the buffer. </param>
        /// <param name="flags"> Flags modifying the lock event. The following flags are defined:  ValueDescription DSBLOCK_FROMWRITECURSORStart the lock at the write cursor. The dwOffset parameter is ignored. DSBLOCK_ENTIREBUFFERLock the entire buffer. The dwBytes parameter is ignored.  </param>
        /// <param name="secondPart"> Address of a variable that receives a pointer to the second locked part of the capture buffer. If NULL is returned, the ppvAudioPtr1 parameter points to the entire locked portion of the capture buffer. </param>
        /// <returns>Address of a variable that receives a pointer to the first locked part of the buffer.</returns>
        /// <unmanaged>HRESULT IDirectSoundCaptureBuffer::Lock([None] int dwOffset,[None] int dwBytes,[Out] void** ppvAudioPtr1,[Out] int* pdwAudioBytes1,[Out] void** ppvAudioPtr2,[Out, Optional] int* pdwAudioBytes2,[None] int dwFlags)</unmanaged>
        public DataStream Lock(int offset, int sizeBytes, LockFlags flags, out DataStream secondPart)
        {
            IntPtr dataPart1;
            int sizePart1;
            IntPtr dataPart2;
            int sizePart2;
            Lock(offset, sizeBytes, out dataPart1, out sizePart1, out dataPart2, out sizePart2, (int)flags);

            secondPart = null;
            if (dataPart2 != IntPtr.Zero)
                secondPart = new DataStream(dataPart2, sizePart2, true, true);

            return new DataStream(dataPart1, sizePart1, true, true);
        }

        /// <summary>	
        /// The Unlock method releases a locked sound buffer.	
        /// </summary>	
        /// <param name="dataPart1"> Address of the value retrieved in the ppvAudioPtr1 parameter of the {{Lock}} method. </param>
        /// <param name="dataPart2"> Address of the value retrieved in the ppvAudioPtr2 parameter of the IDirectSoundBuffer8::Lock method. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IDirectSoundBuffer::Unlock([In, Buffer] void* pvAudioPtr1,[None] int dwAudioBytes1,[In, Buffer, Optional] void* pvAudioPtr2,[None] int dwAudioBytes2)</unmanaged>
        public void Unlock(DataStream dataPart1, DataStream dataPart2)
        {
            if (dataPart2 != null)
                Unlock(dataPart1.DataPointer, (int)dataPart1.Length, dataPart2.DataPointer, (int)dataPart2.Length);
            else
                Unlock(dataPart1.DataPointer, (int)dataPart1.Length, IntPtr.Zero, 0);
        }

        /// <summary>
        /// Writes data to the buffer.
        /// </summary>
        /// <returns />
        public void Read<T>(T[] data, int bufferOffset, LockFlags flags) where T : struct
        {
            this.Read<T>(data, 0, 0, bufferOffset, flags);
        }

        /// <summary>
        /// Writes data to the buffer.
        /// </summary>
        /// <returns />
        public void Read<T>(T[] data, int startIndex, int count, int bufferOffset, LockFlags flags) where T : struct
        {
            DataStream stream2 = null;
            //Utilities.CheckArrayBounds(data, startIndex, ref count);
            int bytes = Utilities.SizeOf<T>() * count;
            DataStream stream1 = this.Lock(bufferOffset, bytes, flags, out stream2);
            int count1 = (int)(((long)((int)stream1.Length)) / (Utilities.SizeOf<T>()));
            stream1.ReadRange(data, startIndex, count1);
            if ((stream2 != null) && (count > count1))
                stream2.ReadRange<T>(data, count1 + startIndex, count - count1);
            Unlock(stream1, stream2);
        }
    }
}