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

namespace SharpDX.Direct3D10
{
    public partial struct Message
    {
        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public SharpDX.Direct3D10.MessageCategory Category;
            public SharpDX.Direct3D10.MessageSeverity Severity;
            public SharpDX.Direct3D10.MessageId Id;
            public System.IntPtr PDescription;
            public SharpDX.PointerSize DescriptionByteLength;
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Category = @ref.Category;
            this.Severity = @ref.Severity;
            this.Id = @ref.Id;
            this.Description = (@ref.PDescription == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(@ref.PDescription);
            this.DescriptionByteLength = @ref.DescriptionByteLength;
        }
    }
}

