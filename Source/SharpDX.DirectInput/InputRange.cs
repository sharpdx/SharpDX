// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.DirectInput
{
    /// <summary>
    /// Structure that describes a range value between a minimum and a maximum
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct InputRange
    {
        /// <summary>Initializes a new instance of the <see cref="InputRange"/> struct.</summary>
        /// <param name="range">The range.</param>
        internal InputRange(PropertyRange range) :this(range.Min, range.Max)
        {            
        }

        /// <summary>Initializes a new instance of the <see cref="InputRange"/> struct.</summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public InputRange(int minimum, int maximum) : this()
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>Copies the automatic.</summary>
        /// <param name="range">The range.</param>
        internal void CopyTo(ref PropertyRange range)
        {
            range.Min = Minimum;
            range.Max = Maximum;
        }

        /// <summary>
        /// Minimum value of this range
        /// </summary>
        public int Minimum;

        /// <summary>
        /// Maximum value of this range
        /// </summary>
        public int Maximum;

        /// <summary>The no minimum.</summary>
        public const int NoMinimum = int.MinValue;

        /// <summary>The no maximum.</summary>
        public const int NoMaximum = int.MaxValue;
    }
}