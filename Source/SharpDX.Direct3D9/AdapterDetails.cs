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

namespace SharpDX.Direct3D9
{
    public partial class AdapterDetails
    {
        /// <summary>
        /// Gets a value indicating whether the adapter is WHQL certified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if certified; otherwise, <c>false</c>.
        /// </value>
        public bool Certified
        {
            get { return WhqlLevel != 0; }
        }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        public Version DriverVersion
        {
            get
            {
                return new Version((int)(RawDriverVersion >> 48) & 0xFFFF, (int)(RawDriverVersion >> 32) & 0xFFFF, (int)(RawDriverVersion >> 16) & 0xFFFF, (int)(RawDriverVersion >> 0) & 0xFFFF);
            }
        }

        /// <summary>
        /// Gets the certification date.
        /// </summary>
        public DateTime CertificationDate
        {
            get
            {
                // Decoding http://msdn.microsoft.com/en-us/library/bb172505%28v=vs.85%29.aspx
                return WhqlLevel == 0
                           ? DateTime.MaxValue
                           : (WhqlLevel == 1
                                  ? DateTime.MinValue
                                  : new DateTime(1999 + (WhqlLevel >> 16), (WhqlLevel & 0xFF00) >> 8, WhqlLevel & 0xFF));
            }
        }
    }
}