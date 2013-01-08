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
