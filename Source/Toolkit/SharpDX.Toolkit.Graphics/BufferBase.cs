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
    public abstract class BufferBase : GraphicsResource
    {
        protected internal ShaderResourceView shaderResourceView;
        protected internal UnorderedAccessView unorderedAccessView;

        public readonly BufferDescription Description;

        protected BufferBase(GraphicsDevice deviceLocal, BufferDescription description)
        {
            Description = description;
            Initialize(deviceLocal, new Direct3D11.Buffer(deviceLocal, description));
        }

        protected BufferBase(GraphicsDevice deviceLocal, Direct3D11.Buffer nativeBuffer, int structureSize = 0)
        {
            Description = nativeBuffer.Description;
            Description.StructureByteStride = structureSize;
            Initialize(deviceLocal, nativeBuffer);
        }

        protected void Initialize(GraphicsDevice deviceArg, Direct3D11.Buffer resource)
        {
            base.Initialize(deviceArg, resource);
            this.InitializeViews();
        }

        protected abstract void InitializeViews();

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Direct3D11.Resource(BufferBase from)
        {
            return from.Resource;
        }

        public static implicit operator Direct3D11.Buffer(BufferBase from)
        {
            return (Direct3D11.Buffer)from.Resource;
        }

        public static implicit operator ShaderResourceView(BufferBase from)
        {
            return from.shaderResourceView;
        }

        public static implicit operator UnorderedAccessView(BufferBase from)
        {
            return from.unorderedAccessView;
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public abstract BufferBase ToStaging();

        protected static BufferDescription NewDescription(int sizeInBytes, BindFlags flags, bool isReadWrite = false, int structureByteStride = 0, ResourceUsage usage = ResourceUsage.Default, ResourceOptionFlags optionFlags = ResourceOptionFlags.None)
        {
            var desc = new BufferDescription()
            {
                SizeInBytes = sizeInBytes,
                StructureByteStride = structureByteStride,
                CpuAccessFlags = GetCputAccessFlagsFromUsage(usage),
                BindFlags = flags,
                OptionFlags = optionFlags,
                Usage = usage,
            };

            if (isReadWrite)
            {
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            return desc;
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                this.Resource.DebugName = Name;
                if (shaderResourceView != null)
                    shaderResourceView.DebugName = Name == null ? null : string.Format("{0} SRV", Name);
                if (unorderedAccessView != null)
                    unorderedAccessView.DebugName = Name == null ? null : string.Format("{0} UAV", Name);
            }
        }
    }
}