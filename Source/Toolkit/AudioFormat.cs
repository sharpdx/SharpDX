// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
//
// AudioFormat originally created by Microsoft for XNA.

using System;
using System.IO;

namespace SharpDX.Toolkit.Audio
{ 
    internal class AudioFormat
    {
        public readonly byte[] RawBuffer;
        public readonly short FormatTag;
        public readonly short Channels;
        public readonly int SampleRate;
        public readonly int AvgBytesPerSec;
        public readonly short BlockAlign;
        public readonly short BitsPerSample;

        public AudioFormat(byte[] buffer)
        {
            BinaryReader binaryReader = new BinaryReader((Stream)new MemoryStream(buffer));
            this.FormatTag = binaryReader.ReadInt16();
            this.Channels = binaryReader.ReadInt16();
            this.SampleRate = binaryReader.ReadInt32();
            this.AvgBytesPerSec = binaryReader.ReadInt32();
            this.BlockAlign = binaryReader.ReadInt16();
            this.BitsPerSample = binaryReader.ReadInt16();
            binaryReader.Close();
            this.RawBuffer = AudioHelper.LocalizeFormat(buffer);
        }

        public TimeSpan DurationFromSize(int sizeInBytes)
        {
            return TimeSpan.FromMilliseconds((double)(sizeInBytes / (int)this.BlockAlign) * 1000.0 / (double)this.SampleRate);
        }

        public int SizeFromDuration(TimeSpan duration)
        {
            int num = checked((int)unchecked(duration.TotalMilliseconds * (double)this.SampleRate / 1000.0));
            return checked(num + unchecked(num % (int)this.Channels) * (int)this.BlockAlign);
        }

        public static AudioFormat Create(int sampleRate, int channels, short bitDepth)
        {
            return new AudioFormat(AudioHelper.MakeFormat(sampleRate, channels, bitDepth));
        }
    }
}
