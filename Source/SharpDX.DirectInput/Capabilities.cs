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

namespace SharpDX.DirectInput
{
    public partial class Capabilities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        public Capabilities()
        {
            unsafe
            {
                Size = sizeof(__Native);
            }
        }

        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.Size = sizeof(__Native);
                return temp;
            }
        }

        /// <summary>
        /// Gets the type of this device.
        /// </summary>
        /// <value>The type.</value>
        public DeviceType Type
        {
            get { return (DeviceType) (RawType & 0xFF); }
        }

        /// <summary>
        /// Gets the subtype of the device.
        /// </summary>
        /// <value>The subtype.</value>
        public int Subtype
        {
            get
            {
                return RawType >> 8;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is human interface device.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is human interface device; otherwise, <c>false</c>.
        /// </value>
        public bool IsHumanInterfaceDevice
        {
            get
            {
                return ((RawType & 0x10000) != 0);
            }
        }
    }
}