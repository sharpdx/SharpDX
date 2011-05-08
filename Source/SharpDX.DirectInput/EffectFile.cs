// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
    public partial class EffectFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFile"/> class.
        /// </summary>
        public EffectFile()
        {
            unsafe
            {
                Size = sizeof(__Native);
            }
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public EffectParameters Parameters { get; set; }
      
        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.Size = sizeof(__Native);
                return temp;
            }
        }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal unsafe partial struct __Native
        {
            public int Size;
            public System.Guid Guid;
            public System.IntPtr EffectParametersPointer;
            public fixed byte Name[260];
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (EffectParametersPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(EffectParametersPointer);
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            // Free Parameters
            if (Parameters != null && @ref.EffectParametersPointer != IntPtr.Zero)
                Parameters.__MarshalFree(ref *((EffectParameters.__Native*)@ref.EffectParametersPointer));

            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Size = @ref.Size;
            this.Guid = @ref.Guid;
            this.EffectParametersPointer = @ref.EffectParametersPointer;
            fixed (void* __ptr = @ref.Name) this.Name = Utilities.PtrToStringAnsi((IntPtr)__ptr, 260);

            if (this.EffectParametersPointer != IntPtr.Zero)
            {
                Parameters = new EffectParameters();
                Parameters.__MarshalFrom(ref *(EffectParameters.__Native*)EffectParametersPointer);
                EffectParametersPointer = IntPtr.Zero;
            }
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Size = this.Size;
            @ref.Guid = this.Guid;
            IntPtr effectParameters = IntPtr.Zero;
            if ( Parameters != null)
            {
                effectParameters = Marshal.AllocHGlobal(sizeof (EffectParameters.__Native));
                var nativeParameters = default(EffectParameters.__Native);
                Parameters.__MarshalTo(ref nativeParameters);
                *((EffectParameters.__Native*) effectParameters) = nativeParameters;
            }

            @ref.EffectParametersPointer = effectParameters;
            IntPtr Name_ = Marshal.StringToHGlobalAnsi(this.Name);
            fixed (void* __ptr = @ref.Name) Utilities.CopyMemory((IntPtr)__ptr, Name_, this.Name.Length);
            Marshal.FreeHGlobal(Name_);
        }
    }
}