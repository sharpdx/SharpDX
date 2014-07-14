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

namespace SharpDX.DirectSound
{
    public partial class WavesReverb
    {
        /// <summary>
        /// Default high-frequency reverb time ratio.
        /// </summary>
        public const float HighFrequencyRTRatioDefault = 0.001f;
        /// <summary>
        /// Maximum high-frequency reverb time ratio.
        /// </summary>
        public const float HighFrequencyRTRatioMax = 0.999f;
        /// <summary>
        /// Minimum high-frequency reverb time ratio.
        /// </summary>
        public const float HighFrequencyRTRatioMin = 0.001f;
        /// <summary>
        /// Default input gain of signal, in decibels (dB).
        /// </summary>
        public const float InGainDefault = 0f;
        /// <summary>
        /// Maximum input gain of signal, in decibels (dB).
        /// </summary>
        public const float InGainMax = 0f;
        /// <summary>
        /// Minimum input gain of signal, in decibels (dB).
        /// </summary>
        public const float InGainMin = -96f;
        /// <summary>
        /// Default reverb mix, in dB.
        /// </summary>
        public const float ReverbMixDefault = 0f;
        /// <summary>
        /// Maximum reverb mix, in dB.
        /// </summary>
        public const float ReverbMixMax = 0f;
        /// <summary>
        /// Minimum reverb mix, in dB.
        /// </summary>
        public const float ReverbMixMin = -96f;
        /// <summary>
        /// Default reverb time, in milliseconds.
        /// </summary>
        public const float ReverbTimeDefault = 1000f;
        /// <summary>
        /// Maximum reverb time, in milliseconds.
        /// </summary>
        public const float ReverbTimeMax = 3000f;
        /// <summary>
        /// Minimum reverb time, in milliseconds.
        /// </summary>
        public const float ReverbTimeMin = 0.001f;        
    }
}