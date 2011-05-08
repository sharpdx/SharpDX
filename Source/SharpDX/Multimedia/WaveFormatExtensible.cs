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
using System.Runtime.InteropServices;

namespace SharpDX.Multimedia
{
    /// <summary>
    /// WaveFormatExtensible
    /// http://www.microsoft.com/whdc/device/audio/multichaud.mspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public class WaveFormatExtensible : WaveFormat
    {
        short wValidBitsPerSample; // bits of precision, or is wSamplesPerBlock if wBitsPerSample==0
        int dwChannelMask; // which channels are present in stream
        Guid subFormat;

        /// <summary>
        /// Parameterless constructor for marshalling
        /// </summary>
        internal WaveFormatExtensible()
        {
        }

        /// <summary>
        /// Creates a new WaveFormatExtensible for PCM or IEEE
        /// </summary>
        public WaveFormatExtensible(int rate, int bits, int channels)
            : base(rate, bits, channels)
        {
            waveFormatTag = WaveFormatEncoding.Extensible;
            extraSize = 22;
            wValidBitsPerSample = (short)bits;
            for (int n = 0; n < channels; n++)
            {
                dwChannelMask |= (1 << n);
            }
            if (bits == 32)
            {
                // KSDATAFORMAT_SUBTYPE_IEEE_FLOAT
                subFormat = new Guid("00000003-0000-0010-8000-00aa00389b71"); // AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT;
            }
            else
            {
                // KSDATAFORMAT_SUBTYPE_PCM
                subFormat = new Guid("00000001-0000-0010-8000-00aa00389b71"); // AudioMediaSubtypes.MEDIASUBTYPE_PCM; //
            }

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        internal new struct __Native
        {
            public WaveFormat.__Native waveFormat;
            public short wValidBitsPerSample; // bits of precision, or is wSamplesPerBlock if wBitsPerSample==0
            public int dwChannelMask; // which channels are present in stream
            public Guid subFormat;

            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                waveFormat.__MarshalFree();
            }
        }
        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.__MarshalFrom(ref @ref.waveFormat);
            this.wValidBitsPerSample = @ref.wValidBitsPerSample;
            this.dwChannelMask = @ref.dwChannelMask;
            this.subFormat = @ref.subFormat;
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            this.__MarshalTo(ref @ref.waveFormat);
            @ref.wValidBitsPerSample = this.wValidBitsPerSample;
            @ref.dwChannelMask = this.dwChannelMask;
            @ref.subFormat = this.subFormat;
        }

        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.waveFormat.extraSize = 22;
                return temp;
            }
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} wBitsPerSample:{1} dwChannelMask:{2} subFormat:{3} extraSize:{4}",
                base.ToString(),
                wValidBitsPerSample,
                dwChannelMask,
                subFormat,
                extraSize);
        }
    }
}