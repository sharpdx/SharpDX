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

using System.Runtime.InteropServices;

namespace SharpDX.Direct3D12
{
    public partial struct RootParameter
    {
        public RootParameterType ParameterType;

        private Union union;

        public RootDescriptorTable DescriptorTable
        {
            get { return union.DescriptorTable; }
            set { union.DescriptorTable = value; }
        }

        public RootConstants Constants
        {
            get { return union.Constants; }
            set { union.Constants = value; }
        }

        public RootDescriptor Descriptor
        {
            get { return union.Descriptor; }
            set { union.Descriptor = value; }
        }

        public ShaderVisibility ShaderVisibility;

        /// <summary>
        /// Because this union contains pointers, it is aligned on 8 bytes boundary, making the field ResourceBarrierDescription.Type 
        /// to be aligned on 8 bytes instead of 4 bytes, so we can't use directly Explicit layout on ResourceBarrierDescription
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private partial struct Union
        {
            [FieldOffset(0)]
            public RootDescriptorTable DescriptorTable;

            [FieldOffset(0)]
            public RootConstants Constants;

            [FieldOffset(0)]
            public RootDescriptor Descriptor;
        }
    }
}