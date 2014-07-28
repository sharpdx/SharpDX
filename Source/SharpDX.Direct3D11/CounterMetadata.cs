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

namespace SharpDX.Direct3D11
{
    /// <summary>
    /// Counter metadata that contains the type, name, units of measure, and a description of an existing counter.
    /// </summary>
    public partial class CounterMetadata
    {
        /// <summary>
        /// Gets the data type of a counter (see <see cref="SharpDX.Direct3D11.CounterType"/>).
        /// </summary>
        /// <value>The type.</value>
        public CounterType Type { get; internal set;}

        /// <summary>
        /// Gets the number of hardware counters that are needed for this counter type to be created. All instances of the same counter type use the same hardware counters.
        /// </summary>
        /// <value>The hardware counter count.</value>
        public int HardwareCounterCount { get; internal set; }

        /// <summary>
        /// Gets a brief name for the counter.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the units a counter measures.
        /// </summary>
        /// <value>The units.</value>
        public string Units { get; internal set; }

        /// <summary>
        /// Gets a description of the counter.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; internal set; }
    }
}