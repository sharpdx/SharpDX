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

namespace SharpDX.Direct3D12
{
    public partial struct RootParameter
    {
        internal __Native native;
        private DescriptorRange[] descriptorTable;

        public RootParameter(DescriptorRange[] descriptorTable) : this()
        {
            DescriptorTable = descriptorTable;
        }

        public RootParameter(RootConstants rootConstants)
            : this()
        {
            Constants = rootConstants;
        }

        public RootParameter(RootDescriptor rootDescriptor, RootParameterType type)
            : this()
        {
            if(type == RootParameterType.Constant32Bits || type == RootParameterType.DescriptorTable)
            {
                throw new ArgumentException(string.Format("Cannot this type [{0}] for a RootDescriptor", type), "type");
            }

            native.ParameterType = type;
            Descriptor = rootDescriptor;
        }

        public static implicit operator RootParameter(DescriptorRange[] descriptorTable)
        {
            return new RootParameter(descriptorTable);
        }

        public static implicit operator RootParameter(RootConstants rootConstants)
        {
            return new RootParameter(rootConstants);
        }

        public DescriptorRange[] DescriptorTable
        {
            get { return descriptorTable; }
            private set
            {
                native.ParameterType = RootParameterType.DescriptorTable;
                descriptorTable = value;
            }
        }

        public RootConstants Constants
        {
            get { return native.Union.Constants; }
            private set
            {
                native.ParameterType = RootParameterType.Constant32Bits;
                native.Union.Constants = value;
            }
        }

        public RootDescriptor Descriptor
        {
            get { return native.Union.Descriptor; }
            private set
            {
                native.Union.Descriptor = value;
            }
        }

        public ShaderVisibility ShaderVisibility
        {
            get { return native.ShaderVisibility; }
            set { native.ShaderVisibility = value; }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct __Native
        {
            public RootParameterType ParameterType;

            public __Union Union;

            public ShaderVisibility ShaderVisibility;
        }

        /// <summary>
        /// Because this union contains pointers, it is aligned on 8 bytes boundary, making the field ResourceBarrierDescription.Type 
        /// to be aligned on 8 bytes instead of 4 bytes, so we can't use directly Explicit layout on ResourceBarrierDescription
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        internal partial struct __Union
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