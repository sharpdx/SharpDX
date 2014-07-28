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
using SharpDX.Multimedia;

namespace SharpDX.XAPO
{

    public partial struct LockParameters
    {
        /// <summary>
        /// Gets or sets the waveformat.
        /// </summary>
        /// <value>The format.</value>
        public WaveFormat Format { get; set; }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 1 )]
        internal partial struct __Native {	
            public IntPtr FormatPointer;
            public int MaxFrameCount;        
            internal unsafe void __MarshalFree()
            {
                if (FormatPointer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(FormatPointer);
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.FormatPointer = IntPtr.Zero;
            if (Format != null)
            {
                int sizeOfFormat = Marshal.SizeOf(Format);
                @ref.FormatPointer = Marshal.AllocCoTaskMem(sizeOfFormat);
                Marshal.StructureToPtr(Format, @ref.FormatPointer, false);
            }
            @ref.MaxFrameCount = this.MaxFrameCount;
        }

        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Format = null;
            this.FormatPointer = @ref.FormatPointer;
            if (this.FormatPointer != IntPtr.Zero)
                this.Format = WaveFormat.MarshalFrom(this.FormatPointer);
            this.MaxFrameCount = @ref.MaxFrameCount;
        }


        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(__Native* @ref)
        {
            this.Format = null;
            this.FormatPointer = @ref->FormatPointer;
            if (this.FormatPointer != IntPtr.Zero)
                this.Format = WaveFormat.MarshalFrom(this.FormatPointer);
            this.MaxFrameCount = @ref->MaxFrameCount;
        }
    }
}