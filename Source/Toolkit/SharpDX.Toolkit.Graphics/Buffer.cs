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
using System.Collections.Generic;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// All-in-One Buffer class linked <see cref="SharpDX.Direct3D11.Buffer"/>.
    /// </summary>
    /// <remarks>
    /// This class is able to create constant buffers, index buffers, vertex buffers, structured buffer, raw buffers, argument buffers.
    /// </remarks>
    /// <msdn-id>ff476351</msdn-id>	
    /// <unmanaged>ID3D11Buffer</unmanaged>	
    /// <unmanaged-short>ID3D11Buffer</unmanaged-short>	
    public partial class Buffer : GraphicsResource
    {
        private readonly Dictionary<ShaderResourceKey, ShaderResourceView> shaderResourceViews = new Dictionary<ShaderResourceKey, ShaderResourceView>();

        private readonly Dictionary<RenderTargetKey, RenderTargetView> renderTargetViews = new Dictionary<RenderTargetKey, RenderTargetView>();

        private ShaderResourceView shaderResourceView;

        private UnorderedAccessView unorderedAccessView;

        /// <summary>
        /// Gets the description of this buffer.
        /// </summary>
        public readonly BufferDescription Description;

        /// <summary>
        /// Gets the number of elements.
        /// </summary>
        /// <remarks>
        /// This value is valid for structured buffers, raw buffers and index buffers that are used as a SharedResourceView.
        /// </remarks>
        public readonly int ElementCount;

        /// <summary>
        /// Gets the size of element T.
        /// </summary>
        public readonly int ElementSize;

        /// <summary>
        /// Gets the type of this buffer.
        /// </summary>
        public readonly BufferFlags BufferFlags;

        /// <summary>
        /// Gets the default view format of this buffer.
        /// </summary>
        public readonly PixelFormat ViewFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffer" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <param name="bufferFlags">Type of the buffer.</param>
        /// <param name="viewFormat">The view format.</param>
        /// <param name="dataPointer">The data pointer.</param>
        protected Buffer(GraphicsDevice device, BufferDescription description, BufferFlags bufferFlags, PixelFormat viewFormat, IntPtr dataPointer) : base(device.MainDevice)
        {
            Description = description;
            BufferFlags = bufferFlags;
            ViewFormat = viewFormat;
            InitCountAndViewFormat(out this.ElementCount, ref ViewFormat);
            ElementSize = Description.SizeInBytes / this.ElementCount;
            Initialize(new Direct3D11.Buffer(device.MainDevice, dataPointer, Description));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffer" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="nativeBuffer">The native buffer.</param>
        /// <param name="bufferFlags">Type of the buffer.</param>
        /// <param name="viewFormat">The view format.</param>
        protected Buffer(GraphicsDevice device, Direct3D11.Buffer nativeBuffer, BufferFlags bufferFlags, PixelFormat viewFormat) : base(device.MainDevice)
        {
            Description = nativeBuffer.Description;
            BufferFlags = bufferFlags;
            ViewFormat = viewFormat;
            InitCountAndViewFormat(out this.ElementCount, ref ViewFormat);
            ElementSize = Description.SizeInBytes / this.ElementCount;
            Initialize(nativeBuffer);
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns>A new instance of this buffer as a staging resource</returns>
        public Buffer ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new Buffer(GraphicsDevice, stagingDesc, BufferFlags, ViewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance</returns>
        /// <remarks>
        /// This method will not copy the content of the buffer to the clone
        /// </remarks>
        public Buffer Clone()
        {
            return new Buffer(GraphicsDevice, Description, BufferFlags, ViewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Gets a <see cref="ShaderResourceView"/> for a particular <see cref="PixelFormat"/>.
        /// </summary>
        /// <param name="viewFormat">The view format.</param>
        /// <param name="firstElement">The first element of the view.</param>
        /// <param name="elementCount">The number of elements accessible from the view.</param>
        /// <returns>A <see cref="ShaderResourceView"/> for the particular view format.</returns>
        /// <remarks>
        /// The buffer must have been declared with <see cref="Graphics.BufferFlags.ShaderResource"/>. 
        /// The ShaderResourceView instance is kept by this buffer and will be disposed when this buffer is disposed.
        /// </remarks>
        /// <msdn-id>ff476519</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateShaderResourceView([In] ID3D11Resource* pResource,[In, Optional] const D3D11_SHADER_RESOURCE_VIEW_DESC* pDesc,[Out, Fast] ID3D11ShaderResourceView** ppSRView)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateShaderResourceView</unmanaged-short>	
        public ShaderResourceView GetShaderResourceView(PixelFormat viewFormat, int firstElement, int elementCount)
        {
            ShaderResourceView srv = null;
            if ((Description.BindFlags & BindFlags.ShaderResource) != 0)
            {
                lock (shaderResourceViews)
                {
                    var key = new ShaderResourceKey(viewFormat, firstElement, elementCount);

                    if (!shaderResourceViews.TryGetValue(key, out srv))
                    {
                        var description = new ShaderResourceViewDescription {
                            Format = viewFormat,
                            Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                            BufferEx = {
                                ElementCount = elementCount,
                                FirstElement = firstElement,
                                Flags = ShaderResourceViewExtendedBufferFlags.None
                            }
                        };

                        if (((BufferFlags & BufferFlags.RawBuffer) == BufferFlags.RawBuffer))
                            description.BufferEx.Flags |= ShaderResourceViewExtendedBufferFlags.Raw;

                        srv = ToDispose(new ShaderResourceView(this.GraphicsDevice, (Direct3D11.Resource)this.Resource, description));

                        srv.Tag = this;

                        shaderResourceViews.Add(key, srv);
                    }
                }
            }
            return srv;
        }

        /// <summary>
        /// Gets a <see cref="RenderTargetView" /> for a particular <see cref="PixelFormat" />.
        /// </summary>
        /// <param name="pixelFormat">The view format.</param>
        /// <param name="width">The width in pixels of the render target.</param>
        /// <returns>A <see cref="RenderTargetView" /> for the particular view format.</returns>
        /// <remarks>The buffer must have been declared with <see cref="Graphics.BufferFlags.RenderTarget" />.
        /// The RenderTargetView instance is kept by this buffer and will be disposed when this buffer is disposed.</remarks>
        public RenderTargetView GetRenderTargetView(PixelFormat pixelFormat, int width)
        {
            RenderTargetView rtv = null;
            if ((Description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                lock (renderTargetViews)
                {
                    var renderTargetKey = new RenderTargetKey(pixelFormat, width);

                    if (!renderTargetViews.TryGetValue(renderTargetKey, out rtv))
                    {
                        var description = new RenderTargetViewDescription() {
                            Format = pixelFormat,
                            Dimension = RenderTargetViewDimension.Buffer,
                            Buffer = {
                                ElementWidth = pixelFormat.SizeInBytes * width,
                                ElementOffset = 0
                            }
                        };

                        rtv = ToDispose(new RenderTargetView(this.GraphicsDevice, (Direct3D11.Resource)this.Resource, description));

                        rtv.Tag = this;

                        renderTargetViews.Add(renderTargetKey, rtv);
                    }
                }
            }
            return rtv;
        }

        /// <summary>
        /// Gets the content of this buffer to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// This method creates internally a staging resource if this texture is not already a staging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public TData[] GetData<TData>() where TData : struct
        {
            var toData = new TData[this.Description.SizeInBytes / Utilities.SizeOf<TData>()];
            GetData(toData);
            return toData;
        }

        /// <summary>
        /// Copies the content of this buffer to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture data.</param>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// This method creates internally a staging resource if this texture is not already a staging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetData<TData>(TData[] toData) where TData : struct
        {
            // Get data from this resource
            if (this.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(this, toData);
            }
            else
            {
                // Inefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = this.ToStaging())
                    GetData(throughStaging, toData);
            }
        }

        /// <summary>
        /// Copies the content of this buffer to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture data.</param>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// This method creates internally a staging resource if this texture is not already a staging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetData<TData>(ref TData toData) where TData : struct
        {
            // Get data from this resource
            if (this.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(this, ref toData);
            }
            else
            {
                // Inefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = this.ToStaging())
                    GetData(throughStaging, ref toData);
            }
        }

        /// <summary>
        /// Copies the content of this buffer from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public unsafe void GetData<TData>(Buffer stagingTexture, ref TData toData) where TData : struct
        {
            GetData(stagingTexture, new DataPointer(Interop.Fixed(ref toData), Utilities.SizeOf<TData>()));
        }

        /// <summary>
        /// Copies the content of this buffer from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public unsafe void GetData<TData>(Buffer stagingTexture, TData[] toData) where TData : struct
        {
            GetData(stagingTexture, new DataPointer(Interop.Fixed(toData), toData.Length * Utilities.SizeOf<TData>()));
        }

        /// <summary>
        /// Copies the content of this buffer from GPU memory to a CPU memory using a specific staging resource.
        /// </summary>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data pointer.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetData(Buffer stagingTexture, DataPointer toData)
        {
            var deviceContext = (Direct3D11.DeviceContext)GraphicsDevice;

            // Check size validity of data to copy to
            if (toData.Size > this.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // Copy the texture to a staging resource
            if (!ReferenceEquals(this, stagingTexture))
                deviceContext.CopyResource(this, stagingTexture);

            // Map the staging resource to a CPU accessible memory
            var box = deviceContext.MapSubresource(stagingTexture, 0, MapMode.Read, Direct3D11.MapFlags.None);
            Utilities.CopyMemory(toData.Pointer, box.DataPointer, toData.Size);
            // Make sure that we unmap the resource in case of an exception
            deviceContext.UnmapSubresource(stagingTexture, 0);
        }

        /// <summary>
        /// Copies the content of a single structure data from CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Data writing behavior</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>. See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public void SetData<TData>(ref TData fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard) where TData : struct
        {
            SetData(GraphicsDevice, ref fromData, offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content an array of data from CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="startIndex">Index to begin setting data from.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>. See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetData<TData>(TData[] fromData, int startIndex = 0, int elementCount = 0, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard) where TData : struct
        {
            SetData(GraphicsDevice, fromData, startIndex, elementCount, offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <param name="fromData">A data pointer.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>. See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        public void SetData(DataPointer fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            SetData(GraphicsDevice, fromData, offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>
        /// See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetData<TData>(GraphicsDevice device, ref TData fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard) where TData : struct
        {
            SetData(device, new DataPointer(Interop.Fixed(ref fromData), Utilities.SizeOf<TData>()), offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="startIndex">The starting index to begin setting data from.</param>
        /// <param name="elementCount">The number of elements to set.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>
        /// See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetData<TData>(GraphicsDevice device, TData[] fromData, int startIndex = 0, int elementCount = 0, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard) where TData : struct
        {
            var sizeOfT = Interop.SizeOf<TData>();
            var sourcePtr = (IntPtr)((byte*) Interop.Fixed(fromData) + startIndex*sizeOfT);
            var sizeOfData = (elementCount == 0 ? fromData.Length : elementCount)*sizeOfT;
            SetData(device, new DataPointer(sourcePtr, sizeOfData), offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">A data pointer.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>
        /// See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        public unsafe void SetData(GraphicsDevice device, DataPointer fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            // Check size validity of data to copy to
            if ((fromData.Size + offsetInBytes) > this.Description.SizeInBytes)
                throw new ArgumentException("Size of data to upload + offset is larger than size of buffer");

            var deviceContext = (Direct3D11.DeviceContext)device;

            // If this bufefer is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (this.Description.Usage == ResourceUsage.Default)
            {
                // Setup the dest region inside the buffer
                if ((this.Description.BindFlags & BindFlags.ConstantBuffer) != 0)
                {
                    deviceContext.UpdateSubresource(new DataBox(fromData.Pointer, 0, 0), this);
                }
                else
                {
                    var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + fromData.Size, 1, 1);
                    deviceContext.UpdateSubresource(new DataBox(fromData.Pointer, 0, 0), this, 0, destRegion);
                }
            }
            else
            {
                try
                {
                    var box = deviceContext.MapSubresource(this, 0, SetDataOptionsHelper.ConvertToMapMode(options), Direct3D11.MapFlags.None);
                    Utilities.CopyMemory((IntPtr)((byte*)box.DataPointer + offsetInBytes), fromData.Pointer, fromData.Size);
                }
                finally
                {
                    deviceContext.UnmapSubresource(this, 0);
                }
            }
        }

        /// <summary>
        /// Directly sets a CPU memory region.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">A data pointer.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>
        /// See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        public unsafe void SetDynamicData(GraphicsDevice device, Action<IntPtr> fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            var deviceContext = (Direct3D11.DeviceContext)device;
            try
            {
                var box = deviceContext.MapSubresource(this, 0, SetDataOptionsHelper.ConvertToMapMode(options), Direct3D11.MapFlags.None);
                fromData((IntPtr)((byte*)box.DataPointer + offsetInBytes));
            }
            finally
            {
                deviceContext.UnmapSubresource(this, 0);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Buffer"/> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="buffer">The original buffer to duplicate the definition from.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <returns>An instance of a new <see cref="Buffer"/></returns>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public static Buffer New(GraphicsDevice device, Buffer buffer, DXGI.Format viewFormat = SharpDX.DXGI.Format.Unknown)
        {
            var bufferType = GetBufferFlagsFromDescription(buffer.Description);
            return new Buffer(device, buffer, bufferType, viewFormat);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description of the buffer.</param>
        /// <param name="viewFormat">View format used if the buffer is used as a shared resource view.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(GraphicsDevice device, BufferDescription description, DXGI.Format viewFormat = SharpDX.DXGI.Format.Unknown)
        {
            var bufferType = GetBufferFlagsFromDescription(description);
            return new Buffer(device, description, bufferType, viewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(GraphicsDevice device, int bufferSize, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, bufferSize, 0, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="elementCount">Number of T elements in this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer<T> New<T>(GraphicsDevice device, int elementCount, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            int bufferSize = Utilities.SizeOf<T>() * elementCount;
            int elementSize = Utilities.SizeOf<T>();

            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer<T>(device, description, bufferFlags, PixelFormat.Unknown, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(GraphicsDevice device, int bufferSize, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, bufferSize, 0, bufferFlags, viewFormat, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="elementSize">Size of an element in the buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(GraphicsDevice device, int bufferSize, int elementSize, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, bufferSize, elementSize, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="elementSize">Size of an element in the buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(GraphicsDevice device, int bufferSize, int elementSize, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default)
        {
            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);
            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer(device, description, bufferFlags, viewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="value">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer<T> New<T>(GraphicsDevice device, ref T value, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            return New(device, ref value, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="value">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static unsafe Buffer<T> New<T>(GraphicsDevice device, ref T value, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            int bufferSize = Utilities.SizeOf<T>();
            int elementSize = ((bufferFlags & BufferFlags.StructuredBuffer) != 0) ? Utilities.SizeOf<T>() : 0;

            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);

            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer<T>(device, description, bufferFlags, viewFormat, (IntPtr)Interop.Fixed(ref value));
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="initialValue">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer<T> New<T>(GraphicsDevice device, T[] initialValue, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            return New(device, initialValue, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="initialValue">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static unsafe Buffer<T> New<T>(GraphicsDevice device, T[] initialValue, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            int bufferSize = Utilities.SizeOf<T>() * initialValue.Length;
            int elementSize = Utilities.SizeOf<T>();
            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);

            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer<T>(device, description, bufferFlags, viewFormat, (IntPtr)Interop.Fixed(initialValue));
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance from a byte array.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="initialValue">The initial value of this buffer.</param>
        /// <param name="elementSize">Size of an element. Must be equal to 2 or 4 for an index buffer, or to the size of a struct for a structured/typed buffer. Can be set to 0 for other buffers.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static unsafe Buffer New(GraphicsDevice device, byte[] initialValue, int elementSize, BufferFlags bufferFlags, DXGI.Format viewFormat = DXGI.Format.Unknown, ResourceUsage usage = ResourceUsage.Default)
        {
            int bufferSize = initialValue.Length;
            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);

            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer(device, description, bufferFlags, viewFormat, (IntPtr)Interop.Fixed(initialValue));
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="dataPointer">The data pointer.</param>
        /// <param name="elementSize">Size of the element.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(GraphicsDevice device, DataPointer dataPointer, int elementSize, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, dataPointer, elementSize, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="dataPointer">The data pointer.</param>
        /// <param name="elementSize">Size of the element.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(GraphicsDevice device, DataPointer dataPointer, int elementSize, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default)
        {
            int bufferSize = dataPointer.Size;
            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);
            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer(device, description, bufferFlags, viewFormat, dataPointer.Pointer);
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                if (shaderResourceView != null)
                    shaderResourceView.DebugName = Name == null ? null : string.Format("{0} SRV", Name);

                if (unorderedAccessView != null)
                    unorderedAccessView.DebugName = Name == null ? null : string.Format("{0} UAV", Name);
            }
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
            {
                // Clear all views
                shaderResourceView = null;
                unorderedAccessView = null;
                renderTargetViews.Clear();
                shaderResourceViews.Clear();
            }
        }

        /// <summary>
        /// Initializes the specified device arg.
        /// </summary>
        /// <param name="resource">The resource.</param>
        protected override void Initialize(Direct3D11.DeviceChild resource)
        {
            base.Initialize(resource);

            // Staging resource don't have any views
            if (Description.Usage != ResourceUsage.Staging)
                this.InitializeViews();
        }

        private void InitCountAndViewFormat(out int count, ref PixelFormat viewFormat)
        {
            if (Description.StructureByteStride == 0)
            {
                if ((BufferFlags & BufferFlags.RawBuffer) != 0)
                    count = Description.SizeInBytes / sizeof(int);
                else if ((BufferFlags & BufferFlags.IndexBuffer) != 0)
                {
                    count = (BufferFlags & BufferFlags.ShaderResource) != 0 ? Description.SizeInBytes / ViewFormat.SizeInBytes : 0;
                }
                else if (viewFormat != DXGI.Format.Unknown)
                {
                    count = Description.SizeInBytes / viewFormat.SizeInBytes;
                }
                else
                {
                    count = 1;
                }
            }
            else
            {
                // Element count
                count = Description.SizeInBytes / Description.StructureByteStride;
            }
        }

        private static BufferFlags GetBufferFlagsFromDescription(BufferDescription description)
        {
            var bufferType = (BufferFlags)0;

            if ((description.BindFlags & BindFlags.ConstantBuffer) != 0)
                bufferType |= BufferFlags.ConstantBuffer;

            if ((description.BindFlags & BindFlags.IndexBuffer) != 0)
                bufferType |= BufferFlags.IndexBuffer;

            if ((description.BindFlags & BindFlags.VertexBuffer) != 0)
                bufferType |= BufferFlags.VertexBuffer;

            if ((description.BindFlags & BindFlags.UnorderedAccess) != 0)
                bufferType |= BufferFlags.UnorderedAccess;

            if ((description.BindFlags & BindFlags.RenderTarget) != 0)
                bufferType |= BufferFlags.RenderTarget;

            if ((description.OptionFlags & ResourceOptionFlags.BufferStructured) != 0)
                bufferType |= BufferFlags.StructuredBuffer;

            if ((description.OptionFlags & ResourceOptionFlags.BufferAllowRawViews) != 0)
                bufferType |= BufferFlags.RawBuffer;

            if ((description.OptionFlags & ResourceOptionFlags.DrawIndirectArguments) != 0)
                bufferType |= BufferFlags.ArgumentBuffer;

            return bufferType;
        }

        private static PixelFormat CheckPixelFormat(BufferFlags bufferFlags, int elementSize, PixelFormat viewFormat)
        {
            if ((bufferFlags & BufferFlags.IndexBuffer) != 0 && (bufferFlags & BufferFlags.ShaderResource) != 0)
            {
                if (elementSize != 2 && elementSize != 4)
                    throw new ArgumentException("Element size must be set to sizeof(short) = 2 or sizeof(int) = 4 for index buffer if index buffer is bound to a ShaderResource", "elementSize");

                viewFormat = elementSize == 2 ? PixelFormat.R16.UInt : PixelFormat.R32.UInt;
            }
            return viewFormat;
        }

        private static BufferDescription NewDescription(int bufferSize, int elementSize, BufferFlags bufferFlags, ResourceUsage usage)
        {
            var desc = new BufferDescription() {
                SizeInBytes = bufferSize,
                StructureByteStride = elementSize, // We keep the element size in the structure byte stride, even if it is not a structured buffer
                CpuAccessFlags = GetCpuAccessFlagsFromUsage(usage),
                BindFlags = BindFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = usage,
            };

            if ((bufferFlags & BufferFlags.ConstantBuffer) != 0)
                desc.BindFlags |= BindFlags.ConstantBuffer;

            if ((bufferFlags & BufferFlags.IndexBuffer) != 0)
                desc.BindFlags |= BindFlags.IndexBuffer;

            if ((bufferFlags & BufferFlags.VertexBuffer) != 0)
                desc.BindFlags |= BindFlags.VertexBuffer;

            if ((bufferFlags & BufferFlags.RenderTarget) != 0)
                desc.BindFlags |= BindFlags.RenderTarget;

            if ((bufferFlags & BufferFlags.ShaderResource) != 0)
                desc.BindFlags |= BindFlags.ShaderResource;

            if ((bufferFlags & BufferFlags.UnorderedAccess) != 0)
                desc.BindFlags |= BindFlags.UnorderedAccess;

            if ((bufferFlags & BufferFlags.StreamOutput) != 0)
                desc.BindFlags |= BindFlags.StreamOutput;

            if ((bufferFlags & BufferFlags.StructuredBuffer) != 0)
            {
                desc.OptionFlags |= ResourceOptionFlags.BufferStructured;
                if (elementSize == 0)
                    throw new ArgumentException("Element size cannot be set to 0 for structured buffer");
            }

            if ((bufferFlags & BufferFlags.RawBuffer) == BufferFlags.RawBuffer)
                desc.OptionFlags |= ResourceOptionFlags.BufferAllowRawViews;

            if ((bufferFlags & BufferFlags.ArgumentBuffer) == BufferFlags.ArgumentBuffer)
                desc.OptionFlags |= ResourceOptionFlags.DrawIndirectArguments;

            return desc;
        }

        /// <summary>
        /// Initializes the views.
        /// </summary>
        private void InitializeViews()
        {
            var bindFlags = Description.BindFlags;

            var srvFormat = ViewFormat;
            var uavFormat = ViewFormat;

            if (((BufferFlags & BufferFlags.RawBuffer) != 0))
            {
                srvFormat = PixelFormat.R32.Typeless;
                uavFormat = PixelFormat.R32.Typeless;
            }

            if ((bindFlags & BindFlags.ShaderResource) != 0)
            {
                this.shaderResourceView = GetShaderResourceView(srvFormat, 0, ElementCount);
            }

            if ((bindFlags & BindFlags.UnorderedAccess) != 0)
            {
                var description = new UnorderedAccessViewDescription() {
                    Format = uavFormat,
                    Dimension = UnorderedAccessViewDimension.Buffer,
                    Buffer = {
                        ElementCount = this.ElementCount,
                        FirstElement = 0,
                        Flags = UnorderedAccessViewBufferFlags.None
                    },
                };

                if (((BufferFlags & BufferFlags.RawBuffer) == BufferFlags.RawBuffer))
                    description.Buffer.Flags |= UnorderedAccessViewBufferFlags.Raw;

                if (((BufferFlags & BufferFlags.StructuredAppendBuffer) == BufferFlags.StructuredAppendBuffer))
                    description.Buffer.Flags |= UnorderedAccessViewBufferFlags.Append;

                if (((BufferFlags & BufferFlags.StructuredCounterBuffer) == BufferFlags.StructuredCounterBuffer))
                    description.Buffer.Flags |= UnorderedAccessViewBufferFlags.Counter;

                this.unorderedAccessView = ToDispose(new UnorderedAccessView(this.GraphicsDevice, (Direct3D11.Resource)this.Resource, description));
            }
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Direct3D11.Resource(Buffer from)
        {
            return from == null ? null : (Direct3D11.Resource)from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator Direct3D11.Buffer(Buffer from)
        {
            return (Direct3D11.Buffer)(from == null ? null : from.Resource);
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator ShaderResourceView(Buffer from)
        {
            return from == null ? null : from.shaderResourceView;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator UnorderedAccessView(Buffer from)
        {
            return from == null ? null : from.unorderedAccessView;
        }

        private struct RenderTargetKey : IEquatable<RenderTargetKey>
        {
            public RenderTargetKey(Format pixelFormat, int width)
            {
                PixelFormat = pixelFormat;
                Width = width;
            }

            public DXGI.Format PixelFormat;

            public int Width;

            public bool Equals(RenderTargetKey other)
            {
                return PixelFormat.Equals(other.PixelFormat) && Width == other.Width;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is RenderTargetKey && Equals((RenderTargetKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (PixelFormat.GetHashCode() * 397) ^ Width;
                }
            }

            public static bool operator ==(RenderTargetKey left, RenderTargetKey right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(RenderTargetKey left, RenderTargetKey right)
            {
                return !left.Equals(right);
            }
        }

        private struct ShaderResourceKey : IEquatable<ShaderResourceKey>
        {
            public ShaderResourceKey(Format viewFormat, int offset, int count)
            {
                this.ViewFormat = viewFormat;
                this.Offset = offset;
                this.Count = count;
            }

            public DXGI.Format ViewFormat;

            public int Offset;

            public int Count;

            public bool Equals(ShaderResourceKey other)
            {
                return this.ViewFormat.Equals(other.ViewFormat) && this.Offset == other.Offset && this.Count == other.Count;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is ShaderResourceKey && Equals((ShaderResourceKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = this.ViewFormat.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.Offset;
                    hashCode = (hashCode * 397) ^ this.Count;
                    return hashCode;
                }
            }

            public static bool operator ==(ShaderResourceKey left, ShaderResourceKey right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(ShaderResourceKey left, ShaderResourceKey right)
            {
                return !left.Equals(right);
            }
        }
    }

    /// <summary>
    /// A buffer with typed information.
    /// </summary>
    /// <typeparam name="T">Type of an element of this buffer.</typeparam>
    public class Buffer<T> : Buffer where T : struct
    {
        protected internal Buffer(GraphicsDevice device, BufferDescription description, BufferFlags bufferFlags, PixelFormat viewFormat, IntPtr dataPointer) : base(device, description, bufferFlags, viewFormat, dataPointer)
        {
        }

        protected internal Buffer(GraphicsDevice device, Direct3D11.Buffer nativeBuffer, BufferFlags bufferFlags, PixelFormat viewFormat)
            : base(device, nativeBuffer, bufferFlags, viewFormat)
        {
        }


        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <returns>An array of data.</returns>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice" />.
        /// This method creates internally a staging resource if this texture is not already a staging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        public T[] GetData()
        {
            return GetData<T>();
        }

        /// <summary>
        /// Copies the content of a single structure data from CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>. See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public void SetData(ref T fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            base.SetData(ref fromData, offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content an array of data from CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="startIndex">Index to start copying from.</param>
        /// <param name="elementCount">Number of elements to copy.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior.</param>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>. See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public void SetData(T[] fromData, int startIndex = 0, int elementCount = 0, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            base.SetData(fromData, startIndex, elementCount, offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content of a single structure data from CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>. See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public void SetData(GraphicsDevice device, ref T fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            base.SetData(device, ref fromData, offsetInBytes, options);
        }

        /// <summary>
        /// Copies the content an array of data from CPU memory to this buffer into GPU memory.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="startIndex">Buffer index to begin copying from.</param>
        /// <param name="elementCount">Number of elements to copy.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="options">Buffer data behavior.</param>
        /// <remarks>
        /// This method is only working when called from the main thread that is accessing the main <see cref="GraphicsDevice"/>. See the unmanaged documentation about Map/UnMap for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public void SetData(GraphicsDevice device, T[] fromData, int startIndex = 0, int elementCount = 0, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            base.SetData(device, fromData, startIndex, elementCount, offsetInBytes, options);
        }
    }
}