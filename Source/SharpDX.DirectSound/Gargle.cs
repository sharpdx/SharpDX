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
    public partial class Gargle
    {
        /// <summary>
        /// Default rate of modulation, in Hertz.
        /// </summary>
        public const int RateDefault = 20;
        /// <summary>
        /// Maximum rate of modulation, in Hertz.
        /// </summary>
        public const int RateMax = 0x3e8;
        /// <summary>
        /// Minimum rate of modulation, in Hertz.
        /// </summary>
        public const int RateMin = 1;
        /// <summary>
        /// Default shape of the modulation waveform.
        /// </summary>
        public const int WaveShapeDefault = 0;
        /// <summary>
        /// Square shape of the modulation waveform.
        /// </summary>
        public const int WaveShapeSquare = 1;
        /// <summary>
        /// Triangular shape of the modulation waveform.
        /// </summary>
        public const int WaveShapeTriangle = 0;        
    }
}