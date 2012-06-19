// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    public class StructuredBuffer : Buffer
    {
        protected StructuredBuffer(BufferDescription description)
            : this(GraphicsDevice.Current, description)
        {
        }

        protected StructuredBuffer(Direct3D11.Buffer buffer, int structureStride)
            : this(GraphicsDevice.Current, buffer, structureStride)
        {
        }

        protected StructuredBuffer(GraphicsDevice deviceLocal, BufferDescription description)
            : base(deviceLocal, description, PixelFormat.Unknown)
        {
            Count = description.SizeInBytes / description.StructureByteStride;
        }

        protected StructuredBuffer(GraphicsDevice deviceLocal, Direct3D11.Buffer nativeBuffer, int structureStride)
            : base(deviceLocal, nativeBuffer, PixelFormat.Unknown, structureStride)
        {
            var description = nativeBuffer.Description;
            Count = description.SizeInBytes / description.StructureByteStride;
        }

        public int Count { get; private set; }

        public StructuredBuffer Clone()
        {
            return new StructuredBuffer(GraphicsDevice, Description);
        }

        public static StructuredBuffer New(BufferDescription description)
        {
            return new StructuredBuffer(description);
        }

        public static StructuredBuffer New(int sizeInBytes, int structureSizeInBytes, bool isReadWrite = false)
        {
            var description = NewDescription(sizeInBytes, BindFlags.ShaderResource, isReadWrite, structureSizeInBytes, ResourceUsage.Dynamic, ResourceOptionFlags.BufferStructured);
            return new StructuredBuffer(description);
        }

        public static StructuredBuffer New<T>(int countElements, bool isReadWrite = false) where T : struct
        {
            int sizeOfStruct = Utilities.SizeOf<T>();
            var description = NewDescription(sizeOfStruct * countElements, BindFlags.ShaderResource, isReadWrite, sizeOfStruct, ResourceUsage.Default, ResourceOptionFlags.BufferStructured);
            return new StructuredBuffer(description);
        }

        public static StructuredBuffer New<T>(T[] elements, bool isReadWrite = false) where T : struct
        {
            int sizeOfStruct = Utilities.SizeOf<T>();
            var description = NewDescription(sizeOfStruct * elements.Length, BindFlags.ShaderResource, isReadWrite, sizeOfStruct, ResourceUsage.Default, ResourceOptionFlags.BufferStructured);
            var nativeBuffer = Direct3D11.Buffer.Create(GraphicsDevice.Current, elements, description);
            return new StructuredBuffer(nativeBuffer, sizeOfStruct);
        }

        public override GraphicsResource ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new StructuredBuffer(this.GraphicsDevice, stagingDesc);
        }
    }
}