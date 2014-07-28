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
    public partial class Echo
    {
        /// <summary>
        /// Default percentage of output fed back into input.
        /// </summary>
        public const float FeedbackDefault = 50f;
        /// <summary>
        /// Maximum percentage of output fed back into input.
        /// </summary>
        public const float FeedbackMax = 100f;
        /// <summary>
        /// Minimum percentage of output fed back into input.
        /// </summary>
        public const float FeedbackMin = 0f;
        /// <summary>
        /// Default delay for left channel, in milliseconds.
        /// </summary>
        public const float LeftDelayDefault = 500f;
        /// <summary>
        /// Maximum delay for left channel, in milliseconds.
        /// </summary>
        public const float LeftDelayMax = 2000f;
        /// <summary>
        /// Minimum delay for left channel, in milliseconds.
        /// </summary>
        public const float LeftDelayMin = 1f;
        /// <summary>
        /// Default value that specifies whether to swap left and right delays with each successive echo. The default value is zero, meaning no swap.
        /// </summary>
        public const int PanDelayDefault = 0;
        /// <summary>
        /// Maximum value that specifies whether to swap left and right delays with each successive echo. The default value is zero, meaning no swap.
        /// </summary>
        public const int PanDelayMax = 1;
        /// <summary>
        /// Minimum value that specifies whether to swap left and right delays with each successive echo. The default value is zero, meaning no swap.
        /// </summary>
        public const int PanDelayMin = 0;
        /// <summary>
        /// Default delay for right channel, in milliseconds.
        /// </summary>
        public const float RightDelayDefault = 500f;
        /// <summary>
        /// Maximum delay for right channel, in milliseconds.
        /// </summary>
        public const float RightDelayMax = 2000f;
        /// <summary>
        /// Minimum delay for right channel, in milliseconds.
        /// </summary>
        public const float RightDelayMin = 1f;
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