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

namespace SharpDX.Direct3D12
{
    public partial class BuildRaytracingAccelerationStructureInputs
    {
        public RaytracingAccelerationStructureType Type { get; set; }

        public RaytracingAccelerationStructureBuildFlags Flags { get; set; }

        public int DescriptorsCount { get; set; }

        public ElementsLayout Layout { get; set; }

        public long InstanceDescriptions { get; set; }

        public RaytracingGeometryDescription[] GeometryDescriptions { get; set; }

        // Internal native struct used for marshalling

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public RaytracingAccelerationStructureType Type;
            public RaytracingAccelerationStructureBuildFlags Flags;
            public int NumDescs;
            public ElementsLayout DescsLayout;
            public System.IntPtr Ptr;

            // Method to free unmanaged allocation
            internal unsafe void __MarshalFree()
            {
                if (Ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(Ptr);
                }
            }
        }

        internal unsafe void __MarshalFree(ref BuildRaytracingAccelerationStructureInputs.__Native @ref)
        {
            @ref.__MarshalFree();
        }

        internal unsafe void __MarshalFrom(ref BuildRaytracingAccelerationStructureInputs.__Native @ref)
        {
            Type = @ref.Type;
            Flags = @ref.Flags;
            DescriptorsCount = @ref.NumDescs;
            Layout = @ref.DescsLayout;

            if (@ref.NumDescs > 0)
            {
                if (@ref.Type == RaytracingAccelerationStructureType.TopLevel)
                {
                    InstanceDescriptions = Utilities.Read<long>(@ref.Ptr);
                }
                else
                {
                    GeometryDescriptions = new RaytracingGeometryDescription[@ref.NumDescs];
                    Utilities.Read(@ref.Ptr, this.GeometryDescriptions, 0, @ref.NumDescs);
                }
            }
        }

        internal unsafe void __MarshalTo(ref BuildRaytracingAccelerationStructureInputs.__Native @ref)
        {
            @ref.Type = Type;
            @ref.Flags = Flags;
            @ref.NumDescs = DescriptorsCount;
            @ref.DescsLayout = Layout;

            if (GeometryDescriptions != null
                && GeometryDescriptions.Length > 0)
            {
                @ref.Ptr = Marshal.AllocHGlobal(Utilities.SizeOf<RaytracingGeometryDescription>() * @ref.NumDescs);
                Utilities.Write(@ref.Ptr, GeometryDescriptions, 0, @ref.NumDescs);
            }
            else
            {
                @ref.Ptr = Marshal.AllocHGlobal(Utilities.SizeOf<long>());
                var instanceDescriptions = InstanceDescriptions;
                Utilities.Write(@ref.Ptr, ref instanceDescriptions);
            }
        }
    }
}