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
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D9
{
    /// <summary>
    /// EffectHandle
    /// </summary>
    public class EffectHandle : DisposeBase
    {
        private IntPtr Pointer;
        private bool isString;


        public EffectHandle(IntPtr pointer)
        {
            Pointer = pointer;
        }

        public unsafe EffectHandle(void* pointer)
        {
            Pointer = new IntPtr(pointer);
        }

        public EffectHandle(string name)
        {
            Pointer = Marshal.StringToHGlobalAnsi(name);
            isString = true;
        }

        public static implicit operator IntPtr(EffectHandle value)
        {
            return value.Pointer;
        }

        public static implicit operator EffectHandle(IntPtr value)
        {
            return new EffectHandle(value);
        }

        public unsafe static implicit operator void*(EffectHandle value)
        {
            return (void*)value.Pointer;
        }

        public static unsafe implicit operator EffectHandle(void* value)
        {
            return new EffectHandle(value);
        }

        public static implicit operator EffectHandle(string name)
        {
            return new EffectHandle(name);
        }

        protected override void Dispose(bool disposing)
        {
            if (isString)
            {
                Marshal.FreeHGlobal(Pointer);
                Pointer = IntPtr.Zero;
                isString = false;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct __Native
        {
            public IntPtr Pointer;
            internal void __MarshalFree()
            {
            }

            public static implicit operator IntPtr(__Native value)
            {
                return value.Pointer;
            }

            public static implicit operator __Native(IntPtr value)
            {
                return new __Native() { Pointer = value };
            }

            public unsafe static implicit operator void*(__Native value)
            {
                return (void*)value.Pointer;
            }

            public static unsafe implicit operator __Native(void* value)
            {
                return new __Native() { Pointer = (IntPtr)value };
            }
        }

        // Method to free unmanaged allocation
        internal void __MarshalFree(ref __Native @ref)
        {
            // @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal void __MarshalFrom(ref __Native @ref)
        {
            this.Pointer = @ref.Pointer;
        }
        // Method to marshal from managed struct tot native
        internal void __MarshalTo(ref __Native @ref)
        {
            @ref.Pointer = Pointer;
        }
    }
}
