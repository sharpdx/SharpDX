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

    public partial class Distortion
    {
        /// <summary>
        /// Default percentage of distortion intensity.
        /// </summary>
        public const float EdgeDefault = 15f;
        /// <summary>
        /// Maximum percentage of distortion intensity.
        /// </summary>
        public const float EdgeMax = 100f;
        /// <summary>
        /// Minimum percentage of distortion intensity.
        /// </summary>
        public const float EdgeMin = 0f;
        /// <summary>
        /// Default amount of signal change after distortion.
        /// </summary>
        public const float GainDefault = -18f;
        /// <summary>
        /// Maximum amount of signal change after distortion.
        /// </summary>
        public const float GainMax = 0f;
        /// <summary>
        /// Minimum amount of signal change after distortion.
        /// </summary>
        public const float GainMin = -60f;
        /// <summary>
        /// Default width of frequency band that determines range of harmonic content addition.
        /// </summary>
        public const float PostEQBandwidthDefault = 2400f;
        /// <summary>
        /// Maximum width of frequency band that determines range of harmonic content addition.
        /// </summary>
        public const float PostEQBandwidthMax = 8000f;
        /// <summary>
        /// Minimum width of frequency band that determines range of harmonic content addition.
        /// </summary>
        public const float PostEQBandwidthMin = 100f;
        /// <summary>
        /// Default center frequency of harmonic content addition.
        /// </summary>
        public const float PostEQCenterFrequencyDefault = 2400f;
        /// <summary>
        /// Maximum center frequency of harmonic content addition.
        /// </summary>
        public const float PostEQCenterFrequencyMax = 8000f;
        /// <summary>
        /// Minimum center frequency of harmonic content addition.
        /// </summary>
        public const float PostEQCenterFrequencyMin = 100f;
        /// <summary>
        /// Default filter cutoff for high-frequency harmonics attenuation.
        /// </summary>
        public const float PreLowPassCutoffDefault = 8000f;
        /// <summary>
        /// Maximum filter cutoff for high-frequency harmonics attenuation.
        /// </summary>
        public const float PreLowPassCutoffMax = 8000f;
        /// <summary>
        /// Minimum filter cutoff for high-frequency harmonics attenuation.
        /// </summary>
        public const float PreLowPassCutoffMin = 100f;
    }
}