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
// AudioHelper originally created by Microsoft for XNA.

using System;
using System.IO;

namespace SharpDX.Toolkit.Audio
{
    internal static class AudioHelper
    {
        internal static short Byteswap(short value)
        {
            if (!BitConverter.IsLittleEndian)
                value = (short)(((int)value & 65280) >> 8 | ((int)value & (int)byte.MaxValue) << 8);
            return value;
        }

        internal static int Byteswap(int value)
        {
            if (!BitConverter.IsLittleEndian)
                value = value >> 24 | value << 24 | (value & 65280) << 8 | (value & 16711680) >> 8;
            return value;
        }

        public static byte[] MakeFormat(int sampleRate, int channels, short bitDepth)
        {
            byte[] buffer = new byte[18];
            BinaryWriter binaryWriter = new BinaryWriter((Stream)new MemoryStream(buffer));
            binaryWriter.Write((short)1);
            binaryWriter.Write((short)channels);
            binaryWriter.Write(sampleRate);
            binaryWriter.Write(sampleRate * (int)bitDepth / 8 * (int)(short)channels);
            binaryWriter.Write((short)((int)(short)channels * (int)bitDepth / 8));
            binaryWriter.Write(bitDepth);
            binaryWriter.Write((short)0);
            binaryWriter.Close();
            return buffer;
        }

        public static byte[] LocalizeFormat(byte[] source)
        {
            BinaryReader binaryReader = new BinaryReader((Stream)new MemoryStream(source));
            byte[] buffer = source;
            if (source.Length < 18)
                buffer = new byte[Math.Max(18, source.Length)];
            BinaryWriter binaryWriter = new BinaryWriter((Stream)new MemoryStream(buffer));
            binaryWriter.Write(AudioHelper.Byteswap(binaryReader.ReadInt16()));
            binaryWriter.Write(AudioHelper.Byteswap(binaryReader.ReadInt16()));
            binaryWriter.Write(AudioHelper.Byteswap(binaryReader.ReadInt32()));
            binaryWriter.Write(AudioHelper.Byteswap(binaryReader.ReadInt32()));
            binaryWriter.Write(AudioHelper.Byteswap(binaryReader.ReadInt16()));
            binaryWriter.Write(AudioHelper.Byteswap(binaryReader.ReadInt16()));
            binaryReader.Close();
            binaryWriter.Close();
            return buffer;
        }

        public static TimeSpan GetSampleDuration(int sizeInBytes, int sampleRate, int channels)
        {
            return AudioFormat.Create(sampleRate, channels, (short)16).DurationFromSize(sizeInBytes);
        }

        public static int GetSampleSizeInBytes(TimeSpan duration, int sampleRate, int channels)
        {
            return AudioFormat.Create(sampleRate, channels, (short)16).SizeFromDuration(duration);
        }
    }
}
