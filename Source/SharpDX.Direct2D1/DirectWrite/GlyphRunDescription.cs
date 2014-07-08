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

namespace SharpDX.DirectWrite
{
    public partial class GlyphRunDescription
    {
        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public IntPtr LocaleName;
            public IntPtr Text;
            public int TextLength;
            public IntPtr ClusterMap;
            public int TextPosition;
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (this.LocaleName != IntPtr.Zero)
                    Marshal.FreeHGlobal(this.LocaleName);
                if (this.Text != IntPtr.Zero)
                    Marshal.FreeHGlobal(this.Text);
            }
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.LocaleName = (@ref.LocaleName == IntPtr.Zero) ? null : Marshal.PtrToStringUni(@ref.LocaleName);
            this.Text = (@ref.Text == IntPtr.Zero) ? null : Marshal.PtrToStringUni(@ref.Text, @ref.TextLength);
            this.TextLength = @ref.TextLength;
            this.ClusterMap = @ref.ClusterMap;
            this.TextPosition = @ref.TextPosition;
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.LocaleName = (this.LocaleName == null) ? IntPtr.Zero : Marshal.StringToHGlobalUni(this.LocaleName);
            @ref.Text = (this.Text == null) ? IntPtr.Zero : Marshal.StringToHGlobalUni(this.Text);
            @ref.TextLength = (this.Text == null)?0: this.Text.Length;
            @ref.ClusterMap = this.ClusterMap;
            @ref.TextPosition = this.TextPosition;
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }
    }
}