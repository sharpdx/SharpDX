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
    public partial class Compressor
    {
        /// <summary>
        /// Default time before compression reaches its full value, in decibels (dB). The default value is 10 ms.
        /// </summary>
        public const float AttackDefault = 10f;
        /// <summary>
        /// Maximum time before compression reaches its full value, in decibels (dB).
        /// </summary>
        public const float AttackMax = 500f;
        /// <summary>
        /// Minimum time before compression reaches its full value, in decibels (dB).
        /// </summary>
        public const float AttackMin = 0.01f;
        /// <summary>
        /// Default output gain of signal after compression, in decibels (dB). The default value is 0 dB. 
        /// </summary>
        public const float GainDefault = 0f;
        /// <summary>
        /// Maximum output gain of signal after compression, in decibels (dB). 
        /// </summary>
        public const float GainMax = 60f;
        /// <summary>
        /// Minimum output gain of signal after compression, in decibels (dB). 
        /// </summary>
        public const float GainMin = -60f;
        /// <summary>
        /// Default time after threshold is reached before attack phase is started, in milliseconds. The default value is 4 ms. 
        /// </summary>
        public const float PreDelayDefault = 4f;
        /// <summary>
        /// Maximum time after threshold is reached before attack phase is started, in milliseconds. 
        /// </summary>
        public const float PreDelayMax = 4f;
        /// <summary>
        /// Minimum time after threshold is reached before attack phase is started, in milliseconds. 
        /// </summary>
        public const float PreDelayMin = 0f;
        /// <summary>
        /// Default compression ratio. The default value is 3, which means 3:1 compression. 
        /// </summary>
        public const float RatioDefault = 3f;
        /// <summary>
        /// Maximum compression ratio.  
        /// </summary>
        public const float RatioMax = 100f;
        /// <summary>
        /// Minimum compression ratio. 
        /// </summary>
        public const float RatioMin = 1f;
        /// <summary>
        /// Default speed at which compression is stopped after input drops below Threshold, in milliseconds. The default value is 200 ms.
        /// </summary>
        public const float ReleaseDefault = 200f;
        /// <summary>
        /// Maximum speed at which compression is stopped after input drops below Threshold, in milliseconds. 
        /// </summary>
        public const float ReleaseMax = 3000f;
        /// <summary>
        /// Minimum speed at which compression is stopped after input drops below Threshold, in milliseconds. 
        /// </summary>
        public const float ReleaseMin = 50f;
        /// <summary>
        /// Default point at which compression begins, in decibels, in decibels (dB). The default value is -20 dB.
        /// </summary>
        public const float ThresholdDefault = -20f;
        /// <summary>
        /// Maximum point at which compression begins, in decibels, in decibels (dB). 
        /// </summary>
        public const float ThresholdMax = 0f;
        /// <summary>
        /// Minimum point at which compression begins, in decibels, in decibels (dB).
        /// </summary>
        public const float ThresholdMin = -60f;        
    }
}