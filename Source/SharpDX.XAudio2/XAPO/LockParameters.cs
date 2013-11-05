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
namespace SharpDX.XAPO
{
    using System;
    using System.Runtime.InteropServices;
    using SharpDX.Multimedia;

    /// <summary>The lock parameters struct.</summary>
    public partial struct LockParameters
    {
        /// <summary>Gets or sets the wave format.</summary>
        /// <value>The format.</value>
        public WaveFormat Format { get; set; }

        // Internal native struct used for marshalling
        /// <summary>The __ native struct.</summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1 )]
        internal struct __Native {
            /// <summary>The format pointer.</summary>
            public IntPtr FormatPointer;
            /// <summary>The maximum frame count.</summary>
            public int MaxFrameCount;
            /// <summary>__s the marshal free.</summary>
            internal void __MarshalFree()
            {
                if (FormatPointer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(FormatPointer);
            }
        }

        /// <summary>__s the marshal free.</summary>
        /// <param name="ref">The preference.</param>
        internal void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        /// <summary>__s the marshal automatic.</summary>
        /// <param name="ref">The preference.</param>
        internal void __MarshalTo(ref __Native @ref)
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

        /// <summary>__s the marshal from.</summary>
        /// <param name="ref">The preference.</param>
        internal void __MarshalFrom(ref __Native @ref)
        {
            this.Format = null;
            this.FormatPointer = @ref.FormatPointer;
            if(this.FormatPointer != IntPtr.Zero)
            {
                this.Format = WaveFormat.MarshalFrom(this.FormatPointer);
            }
            this.MaxFrameCount = @ref.MaxFrameCount;
        }


        // Method to marshal from native to managed struct
        /// <summary>__s the marshal from.</summary>
        /// <param name="ref">The preference.</param>
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