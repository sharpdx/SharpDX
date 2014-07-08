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
    internal partial class DataFormat
    {
        public DataFormat(DataFormatFlag flags)
        {
            Size = Utilities.SizeOf<__Native>();
            ObjectSize = Utilities.SizeOf<DataObjectFormat.__Native>();
            Flags = flags;
        }

        public DataFormat()
        {
            unsafe
            {
                Size = sizeof(__Native);
                ObjectSize = sizeof(DataObjectFormat.__Native);
            }
        }

        internal static __Native __NewNative()
        {
            unsafe
            {
                __Native temp = default(__Native);
                temp.Size = sizeof(__Native);
                temp.ObjectSize = sizeof(DataObjectFormat.__Native);
                return temp;
            }
        }

        public DataObjectFormat[] ObjectsFormat { get; set; }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public int Size;
            public int ObjectSize;
            public SharpDX.DirectInput.DataFormatFlag Flags;
            public int DataSize;
            public int ObjectArrayCount;
            public System.IntPtr ObjectArrayPointer;
            // Method to free unmanaged allocation
            internal unsafe void __MarshalFree()
            {
                //if (ObjectArrayPointer != IntPtr.Zero)
                //    GCHandle.FromIntPtr(ObjectArrayPointer).Free();
            }
        }

        // Method to free unmanaged allocation
        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Size = @ref.Size;
            this.ObjectSize = @ref.ObjectSize;
            this.Flags = @ref.Flags;
            this.DataSize = @ref.DataSize;
            this.ObjectArrayCount = @ref.ObjectArrayCount;
            this.ObjectArrayPointer = @ref.ObjectArrayPointer;
        }

        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Size = this.Size;
            @ref.ObjectSize = this.ObjectSize;
            @ref.Flags = this.Flags;
            @ref.DataSize = this.DataSize;

            @ref.ObjectArrayCount = 0;
            @ref.ObjectArrayPointer = IntPtr.Zero;

            if (ObjectsFormat != null && ObjectsFormat.Length > 0)
            {
                @ref.ObjectArrayCount = ObjectsFormat.Length;
                var nativeDataFormats = new DataObjectFormat.__Native[ObjectsFormat.Length];
                for(int i = 0; i < ObjectsFormat.Length; i++ )
                    ObjectsFormat[i].__MarshalTo(ref nativeDataFormats[i]);

                var handle = GCHandle.Alloc(nativeDataFormats, GCHandleType.Pinned);
                @ref.ObjectArrayPointer = handle.AddrOfPinnedObject();
            }
        }
    }
}