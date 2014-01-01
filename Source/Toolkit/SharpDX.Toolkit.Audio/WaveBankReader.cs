// Copyright (c) 2010-2013 SharpDX - SharpDX Team
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
//-------------------------------------------------------------------------------------
// This is a port of the wave bank reader for the DirectX toolkit 
//-------------------------------------------------------------------------------------
// Functions for loading audio data from Wave Banks
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// http://go.microsoft.com/fwlink/?LinkId=248929
//-------------------------------------------------------------------------------------
using SharpDX.Multimedia;
using SharpDX.Text;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Text;

namespace SharpDX.Toolkit.Audio
{
    internal sealed class WaveBankReader : IDisposable
    {
        private BinaryReader reader;
        private bool isBigEndian;
        private Header header;
        private BankData bankData;
        string[] names;
        EntryCompact[] compactMetadata;
        Entry[] metaData;
        uint[][] seekTable;
        byte[] waveData;

        public WaveBankReader(Stream stream)
        {
            reader = new BinaryReader(stream);
            isBigEndian = false;

            ReadHeader();
            ReadBankData();
            ReadNames();
            ReadMetadata();
            ReadSeekTable();
            ReadWaveData();
        }

        public int Count { get { return bankData == null ? 0 : (int)bankData.EntryCount; } }
        public bool IsStreaming { get { return bankData == null ? false : (bankData.Flags & BankDataFlags.DataTypeStreaming) == BankDataFlags.DataTypeStreaming; } }

        private void ReadHeader()
        {
            header = new Header();
            header.Signature = ReadUInt32();

            if (header.Signature != Header.LittleEndianSignature && header.Signature != Header.BigEndianSignature)
            {
                throw new Exception("Not a valid xwb file.");
            }

            //switch to reading big Endian;
            if (header.Signature == Header.BigEndianSignature)
            {
                isBigEndian = true;
                header.Signature = Header.LittleEndianSignature;
            }

            header.Version = ReadUInt32();
            header.HeaderVersion = ReadUInt32();

            if (header.HeaderVersion != Header.WaveBankVersion)
            {
                throw new NotSupportedException("This file version is not supported.");
            }

            for (int i = 0; i < header.Segments.Length; i++)
            {
                Region region = new Region();
                region.Offset = ReadUInt32();
                region.Length = ReadUInt32();
                header.Segments[i] = region;
            }
        }

        private void ReadBankData()
        {
            bankData = new BankData();

            SeekSegment(SegmentIndex.BankData);

            bankData.Flags = (BankDataFlags)ReadUInt32();
            bankData.EntryCount = ReadUInt32();
            bankData.BankName = Encoding.ASCII.GetString(reader.ReadBytes(BankData.BankNameLength)).Trim('\0');
            bankData.EntryMetaDataElementSize = ReadUInt32();
            bankData.EntryNameElementSize = ReadUInt32();
            bankData.Alignment = ReadUInt32();
            bankData.CompactFormat = (MiniWaveFormat)ReadUInt32();
            bankData.BuildTime = new FILETIME { LowDateTime = ReadUInt32(), HighDateTime = ReadUInt32() };


            if (bankData.EntryCount <= 0)
                throw new Exception("No Data.");

            if ((bankData.Flags & BankDataFlags.Compact) == BankDataFlags.Compact)
            {
                if (bankData.EntryMetaDataElementSize != EntryCompact.Size)
                {
                    throw new Exception("Entry Meta Data Element Size is not valid for compact wavebank.");
                }

                if (header.Segments[SegmentIndex.EntryWaveData].Length > MaxCompactDataSegmentSize)
                {
                    throw new Exception("Data segment is too large to be valid compact wavebank.");
                }
            }
            else
            {
                if (bankData.EntryMetaDataElementSize != Entry.Size)
                {
                    throw new Exception("Entry Meta Data Element Size is not valid for wavebank.");
                }
            }

            uint metadataBytes = header.Segments[SegmentIndex.EntryMetaData].Length;
            if (metadataBytes != (bankData.EntryCount * bankData.EntryMetaDataElementSize))
            {
                throw new Exception("Entry Meta Data Size does not match segment length.");
            }
        }

        private void ReadNames()
        {
            names = new string[((int)bankData.EntryCount)];
            var nameBytes = header.Segments[SegmentIndex.EntryNames].Length;
            if (nameBytes > 0)
            {
                if (nameBytes >= bankData.EntryNameElementSize * bankData.EntryCount)
                {
                    SeekSegment(SegmentIndex.EntryNames);
                    for (int i = 0; i < bankData.EntryCount; i++)
                    {
                        names[i] = Encoding.ASCII.GetString(reader.ReadBytes((int)bankData.EntryNameElementSize)).Trim('\0');
                    }
                }
            }
        }

        private void ReadMetadata()
        {
            SeekSegment(SegmentIndex.EntryMetaData);

            if ((bankData.Flags & BankDataFlags.Compact) == BankDataFlags.Compact)
            {
                compactMetadata = new EntryCompact[(int)bankData.EntryCount];

                for (int i = 0; i < bankData.EntryCount; i++)
                {
                    compactMetadata[i] = (EntryCompact)ReadUInt32();
                }
            }
            else
            {
                metaData = new Entry[(int)bankData.EntryCount];
                for (int i = 0; i < bankData.EntryCount; i++)
                {
                    Entry metdataEntry = new Entry();
                    metdataEntry.FlagsAndDuration = (FlagsAndDuration)ReadUInt32();
                    metdataEntry.Format = (MiniWaveFormat)ReadUInt32();
                    metdataEntry.PlayRegion = new Region { Offset = ReadUInt32(), Length = ReadUInt32() };
                    metdataEntry.LoopRegion = new SampleRegion { StartSample = ReadUInt32(), TotalSamples = ReadUInt32() };
                    metaData[i] = metdataEntry;
                }
            }
        }

        private void ReadSeekTable()
        {
            var seekLength = header.Segments[SegmentIndex.SeekTables].Length;

            if (seekLength > 0)
            {
                SeekSegment(SegmentIndex.SeekTables);

                seekLength /= sizeof(uint);
                var seekTableRaw = new uint[seekLength];

                for (int i = 0; i < seekLength; i++)
                {
                    seekTableRaw[i] = ReadUInt32();
                }

                seekTable = new uint[bankData.EntryCount][];

                for (int i = 0; i < bankData.EntryCount; i++)
                {
                    var offset = seekTableRaw[i];
                    if (offset != unchecked((uint)(-1)))
                    {
                        offset += bankData.EntryCount;
                        if (offset <= seekLength)
                        {
                            var count = seekTableRaw[offset];

                            if (count > 0)
                            {
                                seekTable[i] = new uint[count];
                                Array.Copy(seekTableRaw, (int)offset + 1, seekTable[i], 0, (int)count);
                            }

                        }
                    }
                    seekTable[i] = new uint[0];

                }
            }
        }

        private void ReadWaveData()
        {
            if (header.Segments[SegmentIndex.EntryWaveData].Length == 0)
            {
                throw new Exception("No Wave Data.");
            }

            if ((bankData.Flags & BankDataFlags.DataTypeStreaming) == BankDataFlags.DataTypeStreaming)
            {
                //Not sure what to do here just leave the binary reader / stream open??
            }
            else
            {
                //TODO Handle Endianness.  This might depend on the bitrate of each of the sounds.
                SeekSegment(SegmentIndex.EntryWaveData);
                waveData = reader.ReadBytes((int)header.Segments[SegmentIndex.EntryWaveData].Length);
            }
        }

        public WaveFormat GetWaveFormat(uint index)
        {
            if (index >= bankData.EntryCount)
                throw new ArgumentOutOfRangeException("index");

            var minFormat = GetMiniWaveFormat(index);

            switch (minFormat.FormatTag)
            {
                case MiniWaveFormat.Tag.PCM:
                    return new WaveFormat((int)minFormat.SamplesPerSecond, (int)minFormat.GetBitsPerSample(), (int)minFormat.Channels);
                    break;
                case MiniWaveFormat.Tag.ADPCM:
                    return new WaveFormat((int)minFormat.SamplesPerSecond, (int)minFormat.Channels, (int)minFormat.GetBlockAlign());
                    break;
                case MiniWaveFormat.Tag.WMA: // xWMA is supported by XAudio 2.7 and by Xbox One
                    return WaveFormat.CreateCustomFormat((minFormat.BitsPerSample & 0x1) > 0 ? WaveFormatEncoding.Wmaudio3 : WaveFormatEncoding.Wmaudio2,
                    (int)minFormat.SamplesPerSecond,
                    (int)minFormat.Channels,
                    (int)minFormat.GetAvgerageBytesPerSecond(),
                    (int)minFormat.GetBlockAlign(),
                    (int)minFormat.GetBitsPerSample());
                    break;
                case MiniWaveFormat.Tag.XMA:
                default:
                    throw new NotSupportedException("Wave format not suppoted.");
                    break;
            }


            return null;
        }

        public byte[] GetWaveData(uint index)
        {
            if (index >= bankData.EntryCount)
                throw new ArgumentOutOfRangeException("index");

            if ((bankData.Flags & BankDataFlags.DataTypeStreaming) == BankDataFlags.DataTypeStreaming)
            {
                throw new InvalidOperationException("Operation is invalid for streaming wave banks.");
            }

            if ((bankData.Flags & BankDataFlags.Compact) == BankDataFlags.Compact)
            {
                var entry = compactMetadata[index];
                uint offset, length;

                entry.ComputeLocations(out offset, out length, index, header, bankData, compactMetadata);
                if ((offset + length) > header.Segments[SegmentIndex.EntryWaveData].Length)
                {
                    throw new Exception("Wave entry exceeds wave data buffer.");
                }
                var result = new byte[length];
                Array.Copy(waveData, (int)offset, result, 0, (int)length);
                return result;
            }
            else
            {
                var entry = metaData[index];

                if ((entry.PlayRegion.Offset + entry.PlayRegion.Length) > header.Segments[SegmentIndex.EntryWaveData].Length)
                {
                    throw new Exception("Wave entry exceeds wave data buffer.");
                }

                var result = new byte[entry.PlayRegion.Length];
                Array.Copy(waveData, (int)entry.PlayRegion.Offset, result, 0, (int)entry.PlayRegion.Length);
                return result;
            }
        }

        public MetaData GetMetadata(uint index)
        {
            if (index >= bankData.EntryCount)
                throw new ArgumentOutOfRangeException("index");

            var metadata = new MetaData();

            if ((bankData.Flags & BankDataFlags.Compact) == BankDataFlags.Compact)
            {
                var entry = compactMetadata[index];
                uint offset, length;

                entry.ComputeLocations(out offset, out length, index, header, bankData, compactMetadata);

                metadata.Duration = EntryCompact.GetDuration(length, bankData, FindSeekTable(index));
                metadata.LoopStart = metadata.LoopLength = 0;
                metadata.OffsetBytes = offset;
                metadata.LengthBytes = length;

            }
            else
            {
                var entry = metaData[index];

                metadata.Duration = entry.FlagsAndDuration.Duration;
                metadata.LoopStart = entry.LoopRegion.StartSample;
                metadata.LoopLength = entry.LoopRegion.TotalSamples;
                metadata.OffsetBytes = entry.PlayRegion.Offset;
                metadata.LengthBytes = entry.PlayRegion.Length;

            }

            return metadata;
        }

        public uint[] GetSeekTable(uint index, out WaveFormatEncoding tag)
        {
            if (index >= bankData.EntryCount)
                throw new ArgumentOutOfRangeException("index");
            
            tag = 0;

            var minFormat = GetMiniWaveFormat(index);
            var seekTable = FindSeekTable(index);
            switch (minFormat.FormatTag)
            {
                case MiniWaveFormat.Tag.WMA: // xWMA is supported by XAudio 2.7 and by Xbox One                    
                    tag = (minFormat.BitsPerSample & 0x1) > 0 ? WaveFormatEncoding.Wmaudio3 : WaveFormatEncoding.Wmaudio2;
                    break;
                case MiniWaveFormat.Tag.XMA:                    
                    tag = (WaveFormatEncoding)0x166 /* WAVE_FORMAT_XMA2 */;
                    break;
                default:
                    return null;
            }

            return null;
        }

        public string GetName(uint index)
        {
            if (index >= bankData.EntryCount)
                throw new ArgumentOutOfRangeException("index");

            if (names == null || names.Length == 0)
                return null;

            return names[index];
        }

        private MiniWaveFormat GetMiniWaveFormat(uint index)
        {
            return (bankData.Flags & BankDataFlags.Compact) == BankDataFlags.Compact ? bankData.CompactFormat : metaData[index].Format;
        }

        private uint[] FindSeekTable(uint index)
        {
            if (seekTable == null || index >= bankData.EntryCount || index >= seekTable.Length)
                return null;

            return seekTable[index];
        }


        private uint ReadUInt32()
        {
            if (!isBigEndian)
                return reader.ReadUInt32();

            var bytes = reader.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private void SeekSegment(int segmentIndex)
        {
            reader.BaseStream.Seek(header.Segments[segmentIndex].Offset, SeekOrigin.Begin);
        }

        public void Dispose()
        {
#if !NET35Plus
            reader.Close();
#else            
            reader.Dispose();
#endif
        }

        const uint DVDSectorSize = 2048;
        const uint DVDBlockSize = DVDSectorSize * 16;
        const uint AlignmentMin = 4;
        const uint AlignmentDVD = DVDSectorSize;

        const uint MaxDataSegmentSize = 0xFFFFFFFF;
        const uint MaxCompactDataSegmentSize = 0x001FFFFF;

        struct Region
        {
            public uint Offset; // Region offset, in bytes.
            public uint Length; // Region length, in bytes.
        }

        struct SampleRegion
        {
            public uint StartSample;  // Start sample for the region;
            public uint TotalSamples; // Region length in samples;
        }

        static class SegmentIndex
        {
            public const int BankData = 0;       // Bank data
            public const int EntryMetaData = 1;      // Entry meta-data
            public const int SeekTables = 2;         // Storage for seek tables for the encoded waves.
            public const int EntryNames = 3;         // Entry friendly names
            public const int EntryWaveData = 4;      // Entry wave data
            public const int Count = 5;
        };

        class Header
        {
            //seems to be opposite to whats in DirectXtk
            public static readonly uint LittleEndianSignature = BitConverter.ToUInt32(ASCIIEncoding.ASCII.GetBytes("WBND"), 0);
            public static readonly uint BigEndianSignature = BitConverter.ToUInt32(ASCIIEncoding.ASCII.GetBytes("DNBW"), 0);
            public const uint WaveBankVersion = 44;



            public uint Signature;            // File signature
            public uint Version;              // Version of the tool that created the file
            public uint HeaderVersion;        // Version of the file format
            public Region[] Segments;           // Segment lookup table

            public Header()
            {
                Segments = new Region[(int)SegmentIndex.Count];
            }

        }

        struct MiniWaveFormat
        {
            public enum Tag : uint
            {
                PCM = 0x0,
                XMA = 0x1,
                ADPCM = 0x2,
                WMA = 0x3,
            }

            public enum BitDepth : uint
            {
                Depth8 = 0x0, // PCM only
                Depth16 = 0x1, // PCM only
            }

            private const uint ADPCMBlockAlignConversionOffset = 22;
            private static readonly uint[] WMABlockAlign = new uint[] { 929, 1487, 1280, 2230, 8917, 8192, 4459, 5945, 2304, 1536, 1485, 1008, 2731, 4096, 6827, 5462, 1280 };
            private static readonly uint[] WMAAvgBytesPerSec = new uint[] { 12000, 24000, 4000, 6000, 8000, 20000, 2500 };

            private uint miniWaveFormat;

            // Format tag
            public Tag FormatTag
            {
                get { return (Tag)BitField.Get(miniWaveFormat, 2, 0); }
                set { BitField.Set((uint)value, 2, 0, ref miniWaveFormat); }
            }

            // Channel count (1 - 6)
            public uint Channels
            {
                get { return BitField.Get(miniWaveFormat, 3, 2); }
                set { BitField.Set(value, 3, 2, ref miniWaveFormat); }
            }

            // Sampling rate
            public uint SamplesPerSecond
            {
                get { return BitField.Get(miniWaveFormat, 18, 5); }
                set { BitField.Set(value, 18, 5, ref miniWaveFormat); }
            }

            // Block alignment.  For WMA, lower 6 bits block alignment index, upper 2 bits bytes-per-second index.
            public uint BlockAlign
            {
                get { return BitField.Get(miniWaveFormat, 8, 23); }
                set { BitField.Set(value, 8, 23, ref miniWaveFormat); }
            }

            // Bits per sample (8 vs. 16, PCM only); WMAudio2/WMAudio3 (for WMA)
            public uint BitsPerSample
            {
                get { return BitField.Get(miniWaveFormat, 1, 31); }
                set { BitField.Set((uint)value, 1, 31, ref miniWaveFormat); }
            }

            public uint GetBitsPerSample()
            {
                switch (FormatTag)
                {
                    case Tag.WMA:
                    case Tag.XMA:
                        return 16;
                    case Tag.ADPCM:
                        return 4;
                    default:
                        return (uint)((BitsPerSample == (uint)BitDepth.Depth16) ? 16 : 8);
                }

            }

            public uint GetBlockAlign()
            {

                switch (FormatTag)
                {
                    case Tag.PCM:
                        return BlockAlign;
                    case Tag.XMA:
                        return (Channels * 16 / 8); // XMA_OUTPUT_SAMPLE_BITS = 16
                    case Tag.ADPCM:
                        return (BlockAlign + ADPCMBlockAlignConversionOffset) * Channels;
                    case Tag.WMA:
                        uint blockAlignIndex = BlockAlign & 0x1F;
                        if (blockAlignIndex < WMABlockAlign.Length)
                            return WMABlockAlign[blockAlignIndex];
                        break;
                    default:
                        break;
                }
                return 0;

            }

            public uint GetAvgerageBytesPerSecond()
            {
                switch (FormatTag)
                {
                    case Tag.PCM:
                    case Tag.XMA:
                        return SamplesPerSecond * GetBlockAlign(); // XMA_OUTPUT_SAMPLE_BITS = 16
                    case Tag.ADPCM:
                        {
                            uint blockAlign = GetBlockAlign();
                            uint samplesPerAdpcmBlock = GetAdpcmSamplesPerBlock();
                            return blockAlign * SamplesPerSecond / samplesPerAdpcmBlock;
                        }
                    case Tag.WMA:
                        uint bytesPerSecondIndex = this.BlockAlign >> 5;
                        if (bytesPerSecondIndex < WMAAvgBytesPerSec.Length)
                            return WMAAvgBytesPerSec[bytesPerSecondIndex];
                        break;
                    default:
                        break;
                }
                return 0;

            }

            public uint GetAdpcmSamplesPerBlock()
            {

                uint blockAlign = (this.BlockAlign + ADPCMBlockAlignConversionOffset) * Channels;
                return blockAlign * 2 / Channels - 12;

            }

            public static explicit operator MiniWaveFormat(uint value)
            {
                MiniWaveFormat result = new MiniWaveFormat();
                result.miniWaveFormat = value;
                return result;
            }
        }

        public enum BankDataFlags : uint
        {
            EntryNames = 0x00010000,
            Compact = 0x00020000,
            SyncDisabled = 0x00040000,
            SeekTables = 0x00080000,
            Mask = 0x000F0000,
            DataTypeBuffer = 0x00000000,
            DataTypeStreaming = 0x00000001,
            DataTypeMask = 0x00000001,
        }


        struct FILETIME { public uint LowDateTime; public uint HighDateTime;}

        class BankData
        {
            public const int BankNameLength = 64;

            public BankDataFlags Flags;                   // Bank flags
            public uint EntryCount;              // Number of entries in the bank
            public string BankName;                // Bank friendly name
            public uint EntryMetaDataElementSize;// Size of each entry meta-data element, in bytes
            public uint EntryNameElementSize;    // Size of each entry name element, in bytes
            public uint Alignment;               // Entry alignment, in bytes
            public MiniWaveFormat CompactFormat;           // Format data for compact bank
            public FILETIME BuildTime;             // Build timestamp
        }


        public enum EntryFlags
        {
            ReadAHead = 0x00000001,     // Enable stream read-ahead
            LoopCache = 0x00000002,     // One or more looping sounds use this wave
            RemoveLoopTail = 0x00000004,// Remove data after the end of the loop region
            IgnoreLoop = 0x00000008,    // Used internally when the loop region can't be used
            Mask = 0x00000008,
        }

        struct FlagsAndDuration
        {
            private uint flagsAndDuration;

            public EntryFlags Flags
            {
                get { return (EntryFlags)BitField.Get(flagsAndDuration, 4, 0); }
                set { BitField.Set((uint)value, 28, 4, ref flagsAndDuration); }
            }


            public uint Duration
            {
                get { return BitField.Get(flagsAndDuration, 3, 2); }
                set { BitField.Set(value, 3, 2, ref flagsAndDuration); }
            }

            public static explicit operator FlagsAndDuration(uint value)
            {
                FlagsAndDuration result = new FlagsAndDuration();
                result.flagsAndDuration = value;
                return result;
            }
        }

        struct Entry
        {
            public FlagsAndDuration FlagsAndDuration;
            public MiniWaveFormat Format;
            public Region PlayRegion;
            public SampleRegion LoopRegion;


            public const int Size = 4 + 4 + 8 + 8;

        }

        struct EntryCompact
        {
            private uint entryCompact;

            // Data offset, in multiplies of the bank alignment.
            public uint Offset
            {
                get { return BitField.Get(entryCompact, 21, 0); }
                set { BitField.Set(value, 3, 2, ref entryCompact); }
            }

            // Data length deviation, in bytes.
            public uint LengthDeviation
            {
                get { return BitField.Get(entryCompact, 11, 21); }
                set { BitField.Set(value, 3, 2, ref entryCompact); }
            }

            public const int Size = 4 + 4;

            public static explicit operator EntryCompact(uint value)
            {
                EntryCompact result = new EntryCompact();
                result.entryCompact = value;
                return result;
            }

            public void ComputeLocations(out uint offset, out uint length, uint index, Header header, BankData data, EntryCompact[] entries)
            {
                offset = this.Offset * data.Alignment;

                if (index < (data.EntryCount - 1))
                {
                    length = (entries[index + 1].Offset * data.Alignment) - offset - LengthDeviation;
                }
                else
                {
                    length = header.Segments[SegmentIndex.EntryWaveData].Length - offset - LengthDeviation;
                }
            }

            public static uint GetDuration(uint length, BankData data, uint[] seekTable)
            {
                switch (data.CompactFormat.FormatTag)
                {
                    case MiniWaveFormat.Tag.ADPCM:
                        {
                            uint duration = (length / data.CompactFormat.GetBlockAlign()) * data.CompactFormat.GetAdpcmSamplesPerBlock();
                            uint partial = length % data.CompactFormat.GetBlockAlign();
                            if (partial != 0)
                            {
                                if (partial >= (7 * data.CompactFormat.Channels))
                                    duration += (partial * 2 / data.CompactFormat.Channels - 12);
                            }
                            return duration;
                        }

                    case MiniWaveFormat.Tag.WMA:
                        if (seekTable != null && seekTable.Length > 0)
                        {
                            return seekTable[seekTable.Length - 1] / (2 * data.CompactFormat.Channels);
                        }
                        return 0;

                    case MiniWaveFormat.Tag.XMA:
                        if (seekTable != null && seekTable.Length > 0)
                        {
                            return seekTable[seekTable.Length - 1];
                        }
                        return 0;

                    default:
                        return (length * 8) / (data.CompactFormat.GetBitsPerSample() * data.CompactFormat.Channels);
                }
            }


        }


        public struct MetaData
        {
            public uint Duration { get; set; }
            public uint LoopStart { get; set; }
            public uint LoopLength { get; set; }
            public uint OffsetBytes { get; set; }
            public uint LengthBytes { get; set; }
        }
    }
}
