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

namespace SharpDX.DirectSound
{
    public partial class SoundBufferDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundBufferDescription"/> class.
        /// </summary>
        public SoundBufferDescription()
        {
            unsafe
            {
                Size = sizeof (SoundBufferDescription.__Native);
                pFormat = IntPtr.Zero;
            }
        }

        /// <summary>
        /// WaveFormat of this sound buffer description
        /// </summary>
        public WaveFormat Format;

        internal partial struct __Native {
            public int Size;
            public SharpDX.DirectSound.BufferFlags Flags;
            public int BufferBytes;
            public int Reserved;
            public IntPtr pFormat;
            public Guid AlgorithmFor3D;

		    // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (pFormat != IntPtr.Zero)
                    Marshal.FreeHGlobal(pFormat);
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Size = this.Size;
            @ref.Flags = this.Flags;
            @ref.BufferBytes = this.BufferBytes;
            @ref.Reserved = this.Reserved;
            @ref.pFormat = WaveFormat.MarshalToPtr(Format);
            @ref.AlgorithmFor3D = this.AlgorithmFor3D;		
		}

        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.Size = sizeof (__Native);
                return temp;
            }
        }
    }
}