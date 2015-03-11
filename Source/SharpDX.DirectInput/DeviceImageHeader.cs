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

namespace SharpDX.DirectInput
{
    public partial class DeviceImageHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceImageHeader"/> class.
        /// </summary>
        public DeviceImageHeader()
        {
            unsafe
            {
                Size = sizeof(__Native);
                SizeImageInfo = sizeof(DeviceImage.__Native);
            }
        }

        public DeviceImage[] Images { get; private set; }

        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.Size = sizeof(__Native);
                temp.SizeImageInfo = sizeof(DeviceImage.__Native);
                return temp;
            }
        }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public int Size;
            public int SizeImageInfo;
            public int ViewCount;
            public int ButtonCount;
            public int AxeCount;
            public int PovCount;
            public int BufferSize;
            public int BufferUsed;
            public System.IntPtr ImageInfoArrayPointer;

        }

        // Method to free native struct
        internal unsafe void __MarshalFree(ref __Native @ref)
        {
        }
        
        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Size = @ref.Size;
            this.SizeImageInfo = @ref.SizeImageInfo;
            this.ViewCount = @ref.ViewCount;
            this.ButtonCount = @ref.ButtonCount;
            this.AxeCount = @ref.AxeCount;
            this.PovCount = @ref.PovCount;
            this.BufferSize = @ref.BufferSize;
            this.BufferUsed = @ref.BufferUsed;
            this.ImageInfoArrayPointer = @ref.ImageInfoArrayPointer;

            if (this.BufferSize > 0 && this.ImageInfoArrayPointer != IntPtr.Zero)
            {
                int nbImageInfoElements = BufferSize/ sizeof(DeviceImage.__Native);
                Images = new DeviceImage[nbImageInfoElements];
                var pImageInfo = (DeviceImage.__Native*)this.ImageInfoArrayPointer;
                for (int i = 0; i < Images.Length; i++)
                {
                    var image = new DeviceImage();
                    image.__MarshalFrom(ref *pImageInfo);
                    pImageInfo++;
                }
            }
        }
         
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Size = this.Size;
            @ref.SizeImageInfo = this.SizeImageInfo;
            @ref.ViewCount = this.ViewCount;
            @ref.ButtonCount = this.ButtonCount;
            @ref.AxeCount = this.AxeCount;
            @ref.PovCount = this.PovCount;
            @ref.BufferSize = this.BufferSize;
            @ref.BufferUsed = this.BufferUsed;
            @ref.ImageInfoArrayPointer = this.ImageInfoArrayPointer;

        }

    }
}