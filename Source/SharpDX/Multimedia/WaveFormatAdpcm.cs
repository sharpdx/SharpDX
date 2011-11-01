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
using System.Runtime.InteropServices;

namespace SharpDX.Multimedia
{
    /// <summary>
    /// WaveFormatAdpcm
    /// http://msdn.microsoft.com/en-us/library/microsoft.directx_sdk.xaudio2.adpcmwaveformat%28v=vs.85%29.aspx
    /// </summary>
    /// <unmanaged>WAVEFORMATADPCM</unmanaged>
    public class WaveFormatAdpcm : WaveFormat
    {
        private ushort samplesPerBlock;
        private int[] coefficients;

        /// <summary>
        /// Parameterless constructor for marshalling
        /// </summary>
        internal WaveFormatAdpcm()
        {
        }

        /// <summary>
        /// Creates a new WaveFormatAdpcm for PCM or IEEE
        /// </summary>
        public WaveFormatAdpcm(int rate, int bits, int channels, ushort samplesPerBlock, int[] coefficients )
            : base(rate, bits, channels)
        {
            waveFormatTag = WaveFormatEncoding.Adpcm;
            this.samplesPerBlock = samplesPerBlock;
            this.coefficients = coefficients;
            extraSize = 32;
        }

        /// <summary>
        /// Gets or sets the samples per block.
        /// </summary>
        /// <value>
        /// The samples per block.
        /// </value>
        public ushort SamplesPerBlock
        {
            get { return samplesPerBlock; }
            set { samplesPerBlock = value; }
        }

        /// <summary>
        /// Gets or sets the coefficients.
        /// </summary>
        /// <value>
        /// The coefficients.
        /// </value>
        public int[] Coefficients
        {
            get { return coefficients; }
            set { coefficients = value; }
        }

        protected unsafe override IntPtr MarshalToPtr()
        {
            var result = Marshal.AllocHGlobal(Utilities.SizeOf<WaveFormatAdpcm.__Native>());
            __MarshalTo(ref *(WaveFormatAdpcm.__Native*)result);
            return result;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        internal unsafe new struct __Native
        {
            public WaveFormat.__Native waveFormat;
            public ushort samplesPerBlock;
            public ushort numberOfCoefficients;
            public int coefficients;
            public int coefficients2;
            public int coefficients3;
            public int coefficients4;
            public int coefficients5;
            public int coefficients6;
            public int coefficients7;

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
            this.samplesPerBlock = @ref.samplesPerBlock;
            this.Coefficients = new int[@ref.numberOfCoefficients];
            if (@ref.numberOfCoefficients > 7)
                throw new InvalidOperationException("Unable to read Adpcm format. Too may coefficients (max 7)");
            fixed(int* pCoefs = &@ref.coefficients)
            for (int i = 0; i < @ref.numberOfCoefficients; i++)
                this.Coefficients[i] = pCoefs[i];
            this.extraSize = (short) (4 + 4*Coefficients.Length);
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            if (Coefficients.Length > 7)
                throw new InvalidOperationException("Unable to encode Adpcm format. Too may coefficients (max 7)");

            this.extraSize = (short)(4 + 4 * Coefficients.Length);
            this.__MarshalTo(ref @ref.waveFormat);
            @ref.samplesPerBlock = this.samplesPerBlock;
            @ref.numberOfCoefficients = (ushort)this.Coefficients.Length;
            fixed (int* pCoefs = &@ref.coefficients)
                for (int i = 0; i < @ref.numberOfCoefficients; i++)
                    pCoefs[i] = this.Coefficients[i];
        }

        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.waveFormat.extraSize = 32;
                return temp;
            }
        }
    }
}