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
    public partial class TextureLoadInformation
    {
        /// <summary>	
        /// Source texture box (see <see cref="SharpDX.Direct3D10.ResourceRegion"/>). 	
        /// </summary>	
        /// <unmanaged>D3D11_BOX* pSrcBox</unmanaged>
        public ResourceRegion SourceRegion;

        /// <summary>	
        /// Destination texture box (see <see cref="SharpDX.Direct3D10.ResourceRegion"/>). 	
        /// </summary>	
        /// <unmanaged>D3D11_BOX* pDstBox</unmanaged>
        public ResourceRegion DestinationRegion;

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public IntPtr SourceRegionPointer;
            public IntPtr DestinationRegionPointer;
            public int FirstSourceMip;
            public int FirstDestinationMip;
            public int MipCount;
            public int FirstSourceElement;
            public int FirstDestinationElement;
            public int ElementCount;
            public SharpDX.Direct3D10.FilterFlags Filter;
            public SharpDX.Direct3D10.FilterFlags MipFilter;
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (SourceRegionPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(SourceRegionPointer);
                if (DestinationRegionPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(DestinationRegionPointer);
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.SourceRegionPointer = @ref.SourceRegionPointer;
            this.DestinationRegionPointer = @ref.DestinationRegionPointer;
            this.FirstSourceMip = @ref.FirstSourceMip;
            this.FirstDestinationMip = @ref.FirstDestinationMip;
            this.MipCount = @ref.MipCount;
            this.FirstSourceElement = @ref.FirstSourceElement;
            this.FirstDestinationElement = @ref.FirstDestinationElement;
            this.ElementCount = @ref.ElementCount;
            this.Filter = @ref.Filter;
            this.MipFilter = @ref.MipFilter;
            this.SourceRegion = new ResourceRegion();
            if (@ref.SourceRegionPointer != IntPtr.Zero)
                Utilities.Read<ResourceRegion>(@ref.SourceRegionPointer, ref this.SourceRegion);
            this.DestinationRegion = new ResourceRegion();
            if (@ref.DestinationRegionPointer != IntPtr.Zero)
                Utilities.Read<ResourceRegion>(@ref.DestinationRegionPointer, ref this.DestinationRegion);
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.SourceRegionPointer = Marshal.AllocHGlobal(Utilities.SizeOf<ResourceRegion>());
            @ref.DestinationRegionPointer = Marshal.AllocHGlobal(Utilities.SizeOf<ResourceRegion>());
            @ref.FirstSourceMip = this.FirstSourceMip;
            @ref.FirstDestinationMip = this.FirstDestinationMip;
            @ref.MipCount = this.MipCount;
            @ref.FirstSourceElement = this.FirstSourceElement;
            @ref.FirstDestinationElement = this.FirstDestinationElement;
            @ref.ElementCount = this.ElementCount;
            @ref.Filter = this.Filter;
            @ref.MipFilter = this.MipFilter;
            Utilities.Write(@ref.SourceRegionPointer, ref this.SourceRegion);
            Utilities.Write(@ref.DestinationRegionPointer, ref this.DestinationRegion);
        } 
    }
}