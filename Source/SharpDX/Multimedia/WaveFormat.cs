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
// -----------------------------------------------------------------------------
// Original code from NAudio project. http://naudio.codeplex.com/
// Greetings to Mark Heath.
// -----------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.Multimedia
{
    /// <summary>
    /// Represents a Wave file format
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public class WaveFormat
    {
        /// <summary>format type</summary>
        protected WaveFormatEncoding waveFormatTag;
        /// <summary>number of channels</summary>
        protected short channels;
        /// <summary>sample rate</summary>
        protected int sampleRate;
        /// <summary>for buffer estimation</summary>
        protected int averageBytesPerSecond;
        /// <summary>block size of data</summary>
        protected short blockAlign;
        /// <summary>number of bits per sample of mono data</summary>
        protected short bitsPerSample;
        /// <summary>number of following bytes</summary>
        protected short extraSize;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        internal struct __Native
        {
            /// <summary>format type</summary>
            public WaveFormatEncoding waveFormatTag;
            /// <summary>number of channels</summary>
            public short channels;
            /// <summary>sample rate</summary>
            public int sampleRate;
            /// <summary>for buffer estimation</summary>
            public int averageBytesPerSecond;
            /// <summary>block size of data</summary>
            public short blockAlign;
            /// <summary>number of bits per sample of mono data</summary>
            public short bitsPerSample;
            /// <summary>number of following bytes</summary>
            public short extraSize;
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.waveFormatTag = @ref.waveFormatTag;
            this.channels = @ref.channels;
            this.sampleRate = @ref.sampleRate;
            this.averageBytesPerSecond = @ref.averageBytesPerSecond;
            this.blockAlign = @ref.blockAlign;
            this.bitsPerSample = @ref.bitsPerSample;
            this.extraSize = @ref.extraSize;            
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.waveFormatTag = this.waveFormatTag;
            @ref.channels = this.channels;
            @ref.sampleRate = this.sampleRate;
            @ref.averageBytesPerSecond = this.averageBytesPerSecond;
            @ref.blockAlign = this.blockAlign;
            @ref.bitsPerSample = this.bitsPerSample;
            @ref.extraSize = this.extraSize;  
        }

        /// <summary>
        /// Creates a new PCM 44.1Khz stereo 16 bit format
        /// </summary>
        public WaveFormat()
            : this(44100, 16, 2)
        {

        }

        /// <summary>
        /// Creates a new 16 bit wave format with the specified sample
        /// rate and channel count
        /// </summary>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of channels</param>
        public WaveFormat(int sampleRate, int channels)
            : this(sampleRate, 16, channels)
        {
        }

        /// <summary>
        /// Gets the size of a wave buffer equivalent to the latency in milliseconds.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns></returns>
        public int ConvertLatencyToByteSize(int milliseconds)
        {
            int bytes = (int)((AverageBytesPerSecond / 1000.0) * milliseconds);
            if ((bytes % BlockAlign) != 0)
            {
                // Return the upper BlockAligned
                bytes = bytes + BlockAlign - (bytes % BlockAlign);
            }
            return bytes;
        }

        /// <summary>
        /// Creates a WaveFormat with custom members
        /// </summary>
        /// <param name="tag">The encoding</param>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of channels</param>
        /// <param name="averageBytesPerSecond">Average Bytes Per Second</param>
        /// <param name="blockAlign">Block Align</param>
        /// <param name="bitsPerSample">Bits Per Sample</param>
        /// <returns></returns>
        public static WaveFormat CreateCustomFormat(WaveFormatEncoding tag, int sampleRate, int channels, int averageBytesPerSecond, int blockAlign, int bitsPerSample)
        {
            WaveFormat waveFormat = new WaveFormat();
            waveFormat.waveFormatTag = tag;
            waveFormat.channels = (short)channels;
            waveFormat.sampleRate = sampleRate;
            waveFormat.averageBytesPerSecond = averageBytesPerSecond;
            waveFormat.blockAlign = (short)blockAlign;
            waveFormat.bitsPerSample = (short)bitsPerSample;
            waveFormat.extraSize = 0;
            return waveFormat;
        }

        /// <summary>
        /// Creates an A-law wave format
        /// </summary>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of Channels</param>
        /// <returns>Wave Format</returns>
        public static WaveFormat CreateALawFormat(int sampleRate, int channels)
        {
            return CreateCustomFormat(WaveFormatEncoding.Alaw, sampleRate, channels, sampleRate * channels, 1, 8);
        }

        /// <summary>
        /// Creates a Mu-law wave format
        /// </summary>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of Channels</param>
        /// <returns>Wave Format</returns>
        public static WaveFormat CreateMuLawFormat(int sampleRate, int channels)
        {
            return CreateCustomFormat(WaveFormatEncoding.Mulaw, sampleRate, channels, sampleRate * channels, 1, 8);
        }

        /// <summary>
        /// Creates a new PCM format with the specified sample rate, bit depth and channels
        /// </summary>
        public WaveFormat(int rate, int bits, int channels)
        {
            if (channels  < 1)
            {
                throw new ArgumentOutOfRangeException("Channels must be 1 or greater", "channels");
            }
            // minimum 16 bytes, sometimes 18 for PCM
            this.waveFormatTag = bits<32?WaveFormatEncoding.Pcm:WaveFormatEncoding.IeeeFloat;
            this.channels = (short)channels;
            this.sampleRate = rate;
            this.bitsPerSample = (short)bits;
            this.extraSize = 0;

            this.blockAlign = (short)(channels * (bits / 8));
            this.averageBytesPerSecond = this.sampleRate * this.blockAlign;
        }

        /// <summary>
        /// Creates a new 32 bit IEEE floating point wave format
        /// </summary>
        /// <param name="sampleRate">sample rate</param>
        /// <param name="channels">number of channels</param>
        public static WaveFormat CreateIeeeFloatWaveFormat(int sampleRate, int channels)
        {
            WaveFormat wf = new WaveFormat();
            wf.waveFormatTag = WaveFormatEncoding.IeeeFloat;
            wf.channels = (short)channels;
            wf.bitsPerSample = 32;
            wf.sampleRate = sampleRate;
            wf.blockAlign = (short)(4 * channels);
            wf.averageBytesPerSecond = sampleRate * wf.blockAlign;
            wf.extraSize = 0;
            return wf;
        }

        /// <summary>
        /// Helper function to retrieve a WaveFormat structure from a pointer
        /// </summary>
        /// <param name="pointer">WaveFormat structure</param>
        /// <returns></returns>
        public static WaveFormat MarshalFromPtr(IntPtr pointer)
        {
            WaveFormat waveFormat = (WaveFormat)Marshal.PtrToStructure(pointer, typeof(WaveFormat));
            switch (waveFormat.Encoding)
            {
                case WaveFormatEncoding.Pcm:
                    // can't rely on extra size even being there for PCM so blank it to avoid reading
                    // corrupt data
                    waveFormat.extraSize = 0;
                    break;
                case WaveFormatEncoding.Extensible:
                    waveFormat = (WaveFormatExtensible)Marshal.PtrToStructure(pointer, typeof(WaveFormatExtensible));
                    break;
            }
            return waveFormat;
        }

        /// <summary>
        /// Helper function to marshal WaveFormat to an IntPtr
        /// </summary>
        /// <param name="format">WaveFormat</param>
        /// <returns>IntPtr to WaveFormat structure (needs to be freed by callee)</returns>
        public static IntPtr MarshalToPtr(WaveFormat format)
        {
            int formatSize = Marshal.SizeOf(format);
            IntPtr formatPointer = Marshal.AllocHGlobal(formatSize);
            Marshal.StructureToPtr(format, formatPointer, false);
            return formatPointer;
        }

        /// <summary>
        /// Reads a new WaveFormat object from a stream
        /// </summary>
        /// <param name="br">A binary reader that wraps the stream</param>
        public WaveFormat(BinaryReader br)
        {
            int formatChunkLength = br.ReadInt32();
            if (formatChunkLength < 16)
                throw new ApplicationException("Invalid WaveFormat Structure");
            this.waveFormatTag = (WaveFormatEncoding)br.ReadUInt16();
            this.channels = br.ReadInt16();
            this.sampleRate = br.ReadInt32();
            this.averageBytesPerSecond = br.ReadInt32();
            this.blockAlign = br.ReadInt16();
            this.bitsPerSample = br.ReadInt16();
            this.extraSize = 0;
            if (formatChunkLength > 16)
            {

                this.extraSize = br.ReadInt16();
                if (this.extraSize > formatChunkLength - 18)
                {
                    Console.WriteLine("Format chunk mismatch");
                    //RRL GSM exhibits this bug. Don't throw an exception
                    //throw new ApplicationException("Format chunk length mismatch");

                    this.extraSize = (short)(formatChunkLength - 18);
                }

                // read any extra data
                // br.ReadBytes(extraSize);

            }
        }

        /// <summary>
        /// Reports this WaveFormat as a string
        /// </summary>
        /// <returns>String describing the wave format</returns>
        public override string ToString()
        {
            switch (this.waveFormatTag)
            {
                case WaveFormatEncoding.Pcm:
                case WaveFormatEncoding.Extensible:
                    // extensible just has some extra bits after the PCM header
                    return String.Format("{0} bit PCM: {1}kHz {2} channels",
                        bitsPerSample, sampleRate / 1000, channels);
                default:
                    return this.waveFormatTag.ToString();
            }
        }

        /// <summary>
        /// Compares with another WaveFormat object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if the objects are the same</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is WaveFormat))
            return false;

            WaveFormat other = (WaveFormat)obj;
            return waveFormatTag == other.waveFormatTag &&
                channels == other.channels &&
                sampleRate == other.sampleRate &&
                averageBytesPerSecond == other.averageBytesPerSecond &&
                blockAlign == other.blockAlign &&
                bitsPerSample == other.bitsPerSample;
        }

        /// <summary>
        /// Provides a Hashcode for this WaveFormat
        /// </summary>
        /// <returns>A hashcode</returns>
        public override int GetHashCode()
        {
            return (int)waveFormatTag ^
                (int)channels ^
                sampleRate ^
                averageBytesPerSecond ^
                (int)blockAlign ^
                (int)bitsPerSample;
        }

        /// <summary>
        /// Returns the encoding type used
        /// </summary>
        public WaveFormatEncoding Encoding
        {
            get
            {
                return waveFormatTag;
            }
        }

        /// <summary>
        /// Returns the number of channels (1=mono,2=stereo etc)
        /// </summary>
        public int Channels
        {
            get
            {
                return channels;
            }
        }

        /// <summary>
        /// Returns the sample rate (samples per second)
        /// </summary>
        public int SampleRate
        {
            get
            {
                return sampleRate;
            }
        }

        /// <summary>
        /// Returns the average number of bytes used per second
        /// </summary>
        public int AverageBytesPerSecond
        {
            get
            {
                return averageBytesPerSecond;
            }
        }

        /// <summary>
        /// Returns the block alignment
        /// </summary>
        public int BlockAlign
        {
            get
            {
                return blockAlign;
            }
        }

        /// <summary>
        /// Returns the number of bits per sample (usually 16 or 32, sometimes 24 or 8)
        /// Can be 0 for some codecs
        /// </summary>
        public int BitsPerSample
        {
            get
            {
                return bitsPerSample;
            }
        }

        /// <summary>
        /// Returns the number of extra bytes used by this waveformat. Often 0,
        /// except for compressed formats which store extra data after the WAVEFORMATEX header
        /// </summary>
        public int ExtraSize
        {
            get
            {
                return extraSize;
            }
        }
    }
}
