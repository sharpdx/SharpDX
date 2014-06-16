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
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
    /// <summary>
    /// Security attributes.
    /// </summary>
    /// <unmanaged>SECURITY_ATTRIBUTES</unmanaged>	    
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct SecurityAttributes
    {
        /// <summary>
        /// Length.
        /// </summary>
        public int Length;

        /// <summary>
        /// Descriptor.
        /// </summary>
        public IntPtr Descriptor;

        private int inheritHandle;
        /// <summary>
        /// Gets or sets a value indicating whether [inherit handle].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [inherit handle]; otherwise, <c>false</c>.
        /// </value>
        public bool InheritHandle
        {
            get
            {
                return inheritHandle != 0;
            }
            set
            {
                inheritHandle = value ? 1 : 0;
            }
        }
    }
}
