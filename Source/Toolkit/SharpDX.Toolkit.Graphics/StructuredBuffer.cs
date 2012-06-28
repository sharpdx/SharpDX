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

using System;

using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A Structured Buffer frontend to <see cref="SharpDX.Direct3D11.Buffer"/>.
    /// </summary>
    public class StructuredBuffer : BufferBase
    {

        protected StructuredBuffer(BufferDescription description, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None)
            : this(GraphicsDevice.Current, description, viewFlags)
        {
        }

        protected StructuredBuffer(Direct3D11.Buffer buffer, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None)
            : this(GraphicsDevice.Current, buffer, viewFlags)
        {
        }

        protected StructuredBuffer(GraphicsDevice deviceLocal, BufferDescription description, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None)
            : base(deviceLocal, description)
        {
            ViewFlags = viewFlags;
            Count = description.SizeInBytes / description.StructureByteStride;
        }

        protected StructuredBuffer(GraphicsDevice deviceLocal, Direct3D11.Buffer nativeBuffer, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None)
            : base(deviceLocal, nativeBuffer)
        {
            var description = nativeBuffer.Description;
            ViewFlags = viewFlags;
            Count = description.SizeInBytes / description.StructureByteStride;
        }

        public readonly UnorderedAccessViewBufferFlags ViewFlags;

        public readonly int Count;

        public StructuredBuffer Clone()
        {
            return new StructuredBuffer(GraphicsDevice, Description, ViewFlags);
        }

        public static StructuredBuffer New(BufferDescription description, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None)
        {
            return new StructuredBuffer(description, viewFlags);
        }

        public static StructuredBuffer New(SharpDX.Direct3D11.Buffer buffer, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None)
        {
            return new StructuredBuffer(buffer, viewFlags);
        }

        public static StructuredBuffer New(int sizeInBytes, int structureSizeInBytes, bool isReadWrite = false, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None)
        {
            var description = NewDescription(sizeInBytes, BindFlags.ShaderResource, isReadWrite, structureSizeInBytes, ResourceUsage.Default, ResourceOptionFlags.BufferStructured);
            return new StructuredBuffer(description, viewFlags);
        }

        public static StructuredBuffer New<T>(int countElements, bool isReadWrite = false, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None) where T : struct
        {
            int sizeOfStruct = Utilities.SizeOf<T>();
            var description = NewDescription(sizeOfStruct * countElements, BindFlags.ShaderResource, isReadWrite, sizeOfStruct, ResourceUsage.Default, ResourceOptionFlags.BufferStructured);
            return new StructuredBuffer(description, viewFlags);
        }

        public static StructuredBuffer New<T>(T[] elements, bool isReadWrite = false, UnorderedAccessViewBufferFlags viewFlags = UnorderedAccessViewBufferFlags.None) where T : struct
        {
            int sizeOfStruct = Utilities.SizeOf<T>();
            var description = NewDescription(sizeOfStruct * elements.Length, BindFlags.ShaderResource, isReadWrite, sizeOfStruct, ResourceUsage.Default, ResourceOptionFlags.BufferStructured);
            var nativeBuffer = Direct3D11.Buffer.Create(GraphicsDevice.Current, elements, description);
            return new StructuredBuffer(nativeBuffer, viewFlags);
        }

        protected override void InitializeViews()
        {
            var bindFlags = Description.BindFlags;

            if ((bindFlags & BindFlags.ShaderResource) != 0)
            {
                var description = new ShaderResourceViewDescription
                {
                    Format = Format.Unknown,
                    Dimension = ShaderResourceViewDimension.Buffer,
                    Buffer =
                    {
                        ElementCount = this.Description.SizeInBytes / this.Description.StructureByteStride,
                        ElementOffset = 0
                    }
                };

                this.ShaderResourceView = ToDispose(new ShaderResourceView(this.GraphicsDevice, this.Resource, description));
            }

            if ((bindFlags & BindFlags.UnorderedAccess) != 0)
            {
                var description = new UnorderedAccessViewDescription()
                {
                    Format = Format.Unknown,
                    Dimension = UnorderedAccessViewDimension.Buffer,
                    Buffer =
                    {
                        ElementCount = this.Description.SizeInBytes / this.Description.StructureByteStride,
                        FirstElement = 0,
                        Flags = ViewFlags
                    },
                };

                this.UnorderedAccessView = ToDispose(new UnorderedAccessView(this.GraphicsDevice, this.Resource, description));
            }
        }

        public override BufferBase ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new StructuredBuffer(this.GraphicsDevice, stagingDesc, ViewFlags);
        }
    }
}