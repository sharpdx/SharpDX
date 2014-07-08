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
    public partial class Chorus
    {
        /// <summary>
        /// Default number of milliseconds the input is delayed before it is played back. The default value is 50. 
        /// </summary>
        public const float DelayDefault = 16f;
        /// <summary>
        /// Maximum number of milliseconds the input is delayed before it is played back. 
        /// </summary>
        public const float DelayMax = 20f;
        /// <summary>
        /// Minimum number of milliseconds the input is delayed before it is played back. 
        /// </summary>
        public const float DelayMin = 0f;
        /// <summary>
        /// Default percentage by which the delay time is modulated by the low-frequency oscillator, in hundredths of a percentage point. The default value is 10.
        /// </summary>
        public const float DepthDefault = 10f;
        /// <summary>
        /// Maximum percentage by which the delay time is modulated by the low-frequency oscillator, in hundredths of a percentage point.
        /// </summary>
        public const float DepthMax = 100f;
        /// <summary>
        /// Minimum percentage by which the delay time is modulated by the low-frequency oscillator, in hundredths of a percentage point.
        /// </summary>
        public const float DepthMin = 0f;
        /// <summary>
        /// Default percentage of output signal to feed back into the effect's input. The default value is 25.
        /// </summary>
        public const float FeedbackDefault = 25f;
        /// <summary>
        /// Maximum percentage of output signal to feed back into the effect's input.
        /// </summary>
        public const float FeedbackMax = 99f;
        /// <summary>
        /// Minimum percentage of output signal to feed back into the effect's input.
        /// </summary>
        public const float FeedbackMin = -99f;
        /// <summary>
        /// Default frequency of the LFO. The default value is 1.1. 
        /// </summary>
        public const float FrequencyDefault = 1.1f;
        /// <summary>
        /// Maximum frequency of the LFO.
        /// </summary>
        public const float FrequencyMax = 10f;
        /// <summary>
        /// Minimum frequency of the LFO.
        /// </summary>
        public const float FrequencyMin = 0f;
        /// <summary>
        /// Positive 180 phase differential between left and right LFOs.
        /// </summary>
        public const int Phase180 = 4;
        /// <summary>
        /// Positive 90 phase differential between left and right LFOs.
        /// </summary>
        public const int Phase90 = 3;
        /// <summary>
        /// Default phase differential between left and right LFOs. The default value is Phase90.
        /// </summary>
        public const int PhaseDefault = 3;
        /// <summary>
        /// Maximum phase differential between left and right LFOs.
        /// </summary>
        public const int PhaseMax = 4;
        /// <summary>
        /// Minimum phase differential between left and right LFOs.
        /// </summary>
        public const int PhaseMin = 0;
        /// <summary>
        /// Negative 180 phase differential between left and right LFOs.
        /// </summary>
        public const int PhaseNegative180 = 0;
        /// <summary>
        /// Negative 90 phase differential between left and right LFOs.
        /// </summary>
        public const int PhaseNegative90 = 1;
        /// <summary>
        /// Zero phase differential between left and right LFOs.
        /// </summary>
        public const int PhaseZero = 2;
        /// <summary>
        /// Default waveform shape of the LFO. By default, the waveform is a sine.
        /// </summary>
        public const int WaveformDefault = 1;
        /// <summary>
        /// Sine waveform shape of the LFO.
        /// </summary>
        public const int WaveformSin = 1;
        /// <summary>
        /// Triangle waveform shape of the LFO.
        /// </summary>
        public const int WaveformTriangle = 0;
        /// <summary>
        /// Default ratio of wet (processed) signal to dry (unprocessed) signal.
        /// </summary>
        public const float WetDryMixDefault = 50f;
        /// <summary>
        /// Maximum ratio of wet (processed) signal to dry (unprocessed) signal.
        /// </summary>
        public const float WetDryMixMax = 100f;
        /// <summary>
        /// Minimum ratio of wet (processed) signal to dry (unprocessed) signal.
        /// </summary>
        public const float WetDryMixMin = 0f;        
    }
}