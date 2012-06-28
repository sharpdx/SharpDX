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
    /// <summary>
    /// Base class extension for <see cref="SharpDX.Direct3D11.Buffer"/>.
    /// </summary>
    /// <msdn-id>ff476351</msdn-id>	
    /// <unmanaged>ID3D11Buffer</unmanaged>	
    /// <unmanaged-short>ID3D11Buffer</unmanaged-short>	
    public abstract class BufferBase : GraphicsResource
    {
        protected internal ShaderResourceView ShaderResourceView;

        protected internal UnorderedAccessView UnorderedAccessView;

        /// <summary>
        /// Gets the description of this buffer.
        /// </summary>
        public readonly BufferDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferBase" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="description">The description.</param>
        protected BufferBase(GraphicsDevice deviceLocal, BufferDescription description)
        {
            Description = description;
            Initialize(deviceLocal, new Direct3D11.Buffer(deviceLocal, description));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferBase" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="nativeBuffer">The native buffer.</param>
        protected BufferBase(GraphicsDevice deviceLocal, Direct3D11.Buffer nativeBuffer)
        {
            Description = nativeBuffer.Description;
            Initialize(deviceLocal, nativeBuffer);
        }

        /// <summary>
        /// Initializes the specified device arg.
        /// </summary>
        /// <param name="deviceArg">The device arg.</param>
        /// <param name="resource">The resource.</param>
        protected void Initialize(GraphicsDevice deviceArg, Direct3D11.Buffer resource)
        {
            base.Initialize(deviceArg, resource);
            this.InitializeViews();
        }

        /// <summary>
        /// Initializes the views.
        /// </summary>
        protected abstract void InitializeViews();

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns>A new instance of this buffer as a staging resource</returns>
        public abstract BufferBase ToStaging();

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <typeparam name="T">Type of the staging buffer.</typeparam>
        /// <returns>A new instance of this buffer as a staging resource</returns>
        public T ToStaging<T>() where T : BufferBase
        {
            return (T) ToStaging();
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                this.Resource.DebugName = Name;
                if (ShaderResourceView != null)
                    ShaderResourceView.DebugName = Name == null ? null : string.Format("{0} SRV", Name);
                if (UnorderedAccessView != null)
                    UnorderedAccessView.DebugName = Name == null ? null : string.Format("{0} UAV", Name);
            }
        }

        /// <summary>
        /// Creates a <see cref="BufferDescription"/>.
        /// </summary>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="isReadWrite">if set to <c>true</c> [is read write].</param>
        /// <param name="structureByteStride">The structure byte stride.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="optionFlags">The option flags.</param>
        /// <returns>An instance of <see cref="BufferDescription"/></returns>
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

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Direct3D11.Resource(BufferBase from)
        {
            return from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator Direct3D11.Buffer(BufferBase from)
        {
            return (Direct3D11.Buffer)from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator ShaderResourceView(BufferBase from)
        {
            return from.ShaderResourceView;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator UnorderedAccessView(BufferBase from)
        {
            return from.UnorderedAccessView;
        }        
    }
}