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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in Histogram effect.
    /// </summary>
    public class Histogram : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Histogram"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Histogram(DeviceContext context) : base(context, Effect.Histogram)
        {
        }

        /// <summary>
        /// Specifies the number of bins used for the histogram. The range of intensity values that fall into a particular bucket depend on the number of specified buckets. 
        /// </summary>
        public int NumBins
        {
            get
            {
                return unchecked((int)GetUIntValue((int)HistogramProperties.NumBins));
            }
            set
            {
                SetValue((int)HistogramProperties.NumBins, unchecked((uint)value));
            }
        }

        /// <summary>
        /// Specifies the channel used to generate the histogram. This effect has a single data output corresponding to the specified channel. See Channel selectors for more info.
        /// </summary>
        public ChannelSelector ChannelSelect
        {
            get
            {
                return GetEnumValue<ChannelSelector>((int)HistogramProperties.ChannelSelect);
            }
            set
            {
                SetEnumValue((int)HistogramProperties.ChannelSelect, value);
            }
        }

        /// <summary>
        /// The output array.
        /// </summary>
        public unsafe float[] HistogramOutput
        {
            get
            {
                var array = new float[NumBins];
                fixed (void* pArray = array)
                    GetValue((int)HistogramProperties.HistogramOutput, PropertyType.Blob, (IntPtr)pArray, sizeof(float) * array.Length);
                return array;
            }
        }
    }
}