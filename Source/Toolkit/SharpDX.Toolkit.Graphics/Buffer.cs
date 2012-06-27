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
    public abstract class Buffer : GraphicsResource
    {
        protected internal ShaderResourceView shaderResourceView;
        protected internal UnorderedAccessView unorderedAccessView;

        public readonly BufferDescription Description;

        protected Buffer(GraphicsDevice deviceLocal, BufferDescription description, PixelFormat viewFormat)
        {
            Description = description;
            Initialize(deviceLocal, new Direct3D11.Buffer(deviceLocal, description), viewFormat);
        }

        protected Buffer(GraphicsDevice deviceLocal, Direct3D11.Buffer nativeBuffer, PixelFormat viewFormat, int structureSize = 0)
        {
            Description = nativeBuffer.Description;
            Description.StructureByteStride = structureSize;
            Initialize(deviceLocal, nativeBuffer, viewFormat);
        }

        protected void Initialize(GraphicsDevice deviceArg, Direct3D11.Buffer resource, PixelFormat viewFormat)
        {
            base.Initialize(deviceArg, resource);

            var bindFlags = Description.BindFlags;

            if ((bindFlags & BindFlags.ShaderResource) != 0)
            {
                var description = new ShaderResourceViewDescription {
                    Format = viewFormat,
                    Dimension = ShaderResourceViewDimension.Buffer, 
                    Buffer = {
                        ElementCount = this.Description.SizeInBytes / this.Description.StructureByteStride, 
                        ElementOffset = 0
                    }
                };

                this.shaderResourceView = ToDispose(new ShaderResourceView(this.GraphicsDevice, this.Resource, description));
            }

            if ((bindFlags & BindFlags.UnorderedAccess) != 0)
            {
                var description = new UnorderedAccessViewDescription() {
                    Format = Format.Unknown,
                    Dimension = UnorderedAccessViewDimension.Buffer,
                    Buffer = {
                        ElementCount = this.Description.SizeInBytes / this.Description.StructureByteStride,
                        FirstElement = 0,
                        Flags = UnorderedAccessViewBufferFlags.None
                    },
                };

                this.unorderedAccessView = ToDispose(new UnorderedAccessView(this.GraphicsDevice, this.Resource, description));
            }
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Direct3D11.Resource(Buffer from)
        {
            return from.Resource;
        }

        public static implicit operator Direct3D11.Buffer(Buffer from)
        {
            return (Direct3D11.Buffer)from.Resource;
        }

        public static implicit operator ShaderResourceView(Buffer from)
        {
            return from.shaderResourceView;
        }

        public static implicit operator UnorderedAccessView(Buffer from)
        {
            return from.unorderedAccessView;
        }

        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// This method creates internally a stagging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        public TData[] GetData<TData>(int subResourceIndex = 0) where TData : struct
        {
            var toData = new TData[Description.SizeInBytes / Utilities.SizeOf<TData>()];
            GetData(toData, subResourceIndex);
            return toData;
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture datas.</param>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// This method creates internally a stagging resource if this texture is not already a stagging resouce, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        public void GetData<TData>(TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Get data from this resource
            if (Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(this, toData, subResourceIndex);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = ToStaging())
                    GetData(throughStaging, toData, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="stagingTexture">The staging texture.</param>
        /// <param name="toData">To data.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void GetData<TData>(Buffer stagingTexture, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((toData.Length * Utilities.SizeOf<TData>()) > Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            var context = (DeviceContext)GraphicsDevice.Current;
            // Copy the texture to a staging resource
            context.CopyResource(this, stagingTexture);

            // Map the staging resource to a CPU accessible memory
            var box = context.MapSubresource(stagingTexture, subResourceIndex, MapMode.Read, Direct3D11.MapFlags.None);
            Utilities.Read(box.DataPointer, toData, 0, toData.Length);
            // Make sure that we unmap the resource in case of an exception
            context.UnmapSubresource(stagingTexture, subResourceIndex);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>See unmanaged documentation for usage and restrictions.</remarks>
        public unsafe void SetData<TData>(ref TData fromData, int offsetInBytes = 0, int subResourceIndex = 0) where TData : struct
        {
            var context = (DeviceContext)GraphicsDevice.Current;

            // Check size validity of data to copy to
            if (Utilities.SizeOf<TData>() > Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // Offset in bytes is set to 0 for constant buffers
            offsetInBytes = (Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? 0 : offsetInBytes;

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (Description.Usage == ResourceUsage.Default || Description.Usage == ResourceUsage.Immutable)
            {
                // Setup the dest region inside the buffer
                var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + Utilities.SizeOf<TData>(), 1, 1);
                context.UpdateSubresource(ref fromData, this, subResourceIndex, 0, 0, (Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? (ResourceRegion?)null : destRegion);
            }
            else
            {
                try
                {
                    var box = context.MapSubresource(this, subResourceIndex, MapMode.WriteDiscard, Direct3D11.MapFlags.None);
                    Utilities.Write((IntPtr)((byte*)box.DataPointer + offsetInBytes), ref fromData);
                }
                finally
                {
                    context.UnmapSubresource(this, subResourceIndex);
                }
            }
        }
        
        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>See unmanaged documentation for usage and restrictions.</remarks>
        public unsafe void SetData<TData>(TData[] fromData, int offsetInBytes = 0, int subResourceIndex = 0) where TData : struct
        {
            var context = (DeviceContext)GraphicsDevice.Current;

            // Check size validity of data to copy to
            if ((fromData.Length * Utilities.SizeOf<TData>()) > Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (Description.Usage == ResourceUsage.Default || Description.Usage == ResourceUsage.Immutable)
            {
                // Setup the dest region inside the buffer
                var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + fromData.Length * Utilities.SizeOf<TData>(), 1, 1);
                context.UpdateSubresource(fromData, this, subResourceIndex, 0, 0, (Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? (ResourceRegion?)null : destRegion);
            }
            else
            {
                try
                {
                    var box = context.MapSubresource(this, subResourceIndex, MapMode.WriteDiscard, Direct3D11.MapFlags.None);
                    Utilities.Write((IntPtr)((byte*)box.DataPointer + offsetInBytes), fromData, 0, fromData.Length);
                }
                finally
                {
                    context.UnmapSubresource(this, subResourceIndex);
                }
            }
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public abstract Buffer ToStaging();

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