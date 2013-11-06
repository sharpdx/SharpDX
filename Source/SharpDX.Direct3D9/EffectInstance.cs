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

namespace SharpDX.Direct3D9
{
    /// <summary>The effect instance class.</summary>
    public partial class EffectInstance
    {
        /// <summary>Gets or sets the defaults.</summary>
        /// <value>The defaults.</value>
        public EffectDefault[] Defaults { get; set; }

        /// <summary>Internal native struct used for marshalling.</summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            /// <summary>The effect filename.</summary>
            public System.IntPtr EffectFilename;
            /// <summary>The default count.</summary>
            public int DefaultCount;
            /// <summary>The default pointer.</summary>
            public System.IntPtr DefaultPointer;
            /// <summary>Free unmanaged allocation.</summary>
            internal unsafe void __MarshalFree()
            {
                if (this.EffectFilename != IntPtr.Zero)
                    Marshal.FreeHGlobal(this.EffectFilename);
                if (DefaultPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(this.DefaultPointer);
            }
        }

        /// <summary>Free unmanaged allocation.</summary>
        /// <param name="ref">The preference.</param>
        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        /// <summary>Marshal from native to managed struct.</summary>
        /// <param name="ref">The preference.</param>
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.EffectFilename = (@ref.EffectFilename == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(@ref.EffectFilename);
            var defaultsNative = new EffectDefault.__Native[@ref.DefaultCount];
            Utilities.Read(@ref.DefaultPointer, defaultsNative, 0, defaultsNative.Length);
            Defaults = new EffectDefault[defaultsNative.Length];
            for (int i = 0; i < Defaults.Length; i++)
            {
                Defaults[i] = new EffectDefault();
                Defaults[i].__MarshalFrom(ref defaultsNative[i]);
            }
        }

        /// <summary>Marshal from managed struct tot native.</summary>
        /// <param name="ref">The preference.</param>
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.EffectFilename = (this.EffectFilename == null) ? IntPtr.Zero : Marshal.StringToHGlobalAnsi(this.EffectFilename);
            var defaultsNative = (EffectDefault.__Native*)Marshal.AllocHGlobal(Defaults.Length * sizeof(EffectDefault.__Native));
            for (int i = 0; i < Defaults.Length; i++)
            {
                Defaults[i].__MarshalTo(ref defaultsNative[i]);
            }
            @ref.DefaultCount = Defaults.Length;
            @ref.DefaultPointer = (IntPtr)defaultsNative;
        }
    }
}

