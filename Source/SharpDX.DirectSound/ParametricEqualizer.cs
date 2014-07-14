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
    public partial class ParametricEqualizer
    {
        /// <summary>
        /// Default bandwidth, in semitones.
        /// </summary>
        public const float BandwidthDefault = 12f;
        /// <summary>
        /// Maximum bandwidth, in semitones.
        /// </summary>
        public const float BandwidthMax = 36f;
        /// <summary>
        /// Minimum bandwidth, in semitones.
        /// </summary>
        public const float BandwidthMin = 1f;
        /// <summary>
        /// Default center frequency, in hertz.
        /// </summary>
        public const float CenterDefault = 8000f;
        /// <summary>
        /// Maximum center frequency, in hertz.
        /// </summary>
        public const float CenterMax = 16000f;
        /// <summary>
        /// Minimum center frequency, in hertz.
        /// </summary>
        public const float CenterMin = 80f;
        /// <summary>
        /// Default gain.
        /// </summary>
        public const float GainDefault = 0f;
        /// <summary>
        /// Maximum gain.
        /// </summary>
        public const float GainMax = 15f;
        /// <summary>
        /// Minimum gain.
        /// </summary>
        public const float GainMin = -15f;        
    }
}