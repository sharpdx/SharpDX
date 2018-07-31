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

namespace SharpDX.Direct3D12
{
    public partial struct ShaderBytecode
    {
        private readonly byte[] managedData;

        public ShaderBytecode(byte[] buffer) : this()
        {
            managedData = buffer;
        }

        public ShaderBytecode(IntPtr pointer, PointerSize size)
        {
            managedData = null;
            Pointer = pointer;
            Size = size;
        }

        public ShaderBytecode(DataPointer dataPointer) : this()
        {
            managedData = null;
            Pointer = dataPointer.Pointer;
            Size = dataPointer.Size;
        }

        public byte[] Buffer
        {
            get { return managedData; }
        }

        public DataPointer BufferPointer
        {
            get
            {
                return new DataPointer(Pointer, Size);
            }
        }

        public static implicit operator ShaderBytecode(DataPointer data)
        {
            return new ShaderBytecode(data);
        }

        public static implicit operator ShaderBytecode(byte[] buffer)
        {
            return new ShaderBytecode(buffer);
        }

        internal void UpdateNative(ref __Native native, IntPtr pinBuffer)
        {
            native.Pointer = pinBuffer;
            if (managedData != null)
            {
                native.Size = (IntPtr)managedData.Length;
            }
        }
    }
}