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
using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Device"/> and <see cref="SharpDX.Direct3D11.DeviceContext"/>
    /// </summary>
    public class GraphicsDevice : Component
    {
        internal Device Device;
        internal DeviceContext Context;
        private readonly Dictionary<PixelFormat,int> maximumMSAASampleCount = new Dictionary<PixelFormat, int>();

        [ThreadStatic]
        private static GraphicsDevice current;

        internal readonly StageStatus CurrentStage;
        internal struct StageStatus
        {
            internal VertexShader VertexShader;
            internal DomainShader DomainShader;
            internal HullShader HullShader;
            internal GeometryShader GeometryShader;
            internal PixelShader PixelShader;
            internal ComputeShader ComputeShader;
        };

        protected GraphicsDevice(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            Device = ToDispose(featureLevels.Length > 0 ? new Device(type, flags, featureLevels) : new Device(type, flags));
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            FeatureLevel = Device.FeatureLevel;
            AttachToCurrentThread();
        }

        protected GraphicsDevice(Device device)
        {
            Device = ToDispose(device);
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            FeatureLevel = Device.FeatureLevel;
            AttachToCurrentThread();
        }

        protected GraphicsDevice(GraphicsDevice mainDevice, DeviceContext deferredContext)
        {
            Device = mainDevice.Device;
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = mainDevice;
            Context = deferredContext;
            FeatureLevel = Device.FeatureLevel;
        }

        /// <summary>
        /// Gets the maximum MSAA sample count for a particular <see cref="PixelFormat"/>.
        /// </summary>
        /// <param name="pixelFormat">The pixelFormat.</param>
        /// <returns>The maximum multisample count for this pixel pixelFormat</returns>
        /// <msdn-id>ff476499</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CheckMultisampleQualityLevels([In] DXGI_FORMAT Format,[In] unsigned int SampleCount,[Out] unsigned int* pNumQualityLevels)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CheckMultisampleQualityLevels</unmanaged-short>	
        public int GetMaximumMSAASampleCount(PixelFormat pixelFormat)
        {
            int maxCount;
            if (!maximumMSAASampleCount.TryGetValue(pixelFormat, out maxCount))
            {
                maxCount = 1;
                for (int i = 1; i < 32; i++)
                    if (Device.CheckMultisampleQualityLevels(pixelFormat, i) != 0)
                        maxCount = i;

                maximumMSAASampleCount[pixelFormat] = maxCount;
            }
            return maxCount;
        }

        /// <summary>	
        /// <p>Clears an unordered access resource with bit-precise values.</p>	
        /// </summary>	
        /// <param name="view">The buffer to clear.</param>	
        /// <param name="value">The value used to clear.</param>	
        /// <remarks>	
        /// <p>This API copies the lower ni bits from each array element i to the corresponding channel, where ni is the number of bits in  the ith channel of the resource format (for example, R8G8B8_FLOAT has 8 bits for the first 3 channels). This works on any UAV with no format conversion.  For a raw or structured buffer view, only the first array element value is used.</p>	
        /// </remarks>	
        /// <msdn-id>ff476391</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearUnorderedAccessViewUint([In] ID3D11UnorderedAccessView* pUnorderedAccessView,[In] const unsigned int* Values)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearUnorderedAccessViewUint</unmanaged-short>	
        public void Clear(UnorderedAccessView view, int value)
        {
            Context.ClearUnorderedAccessView(view, value);
        }

        /// <summary>	
        /// Clears an unordered access resource with a float value.
        /// </summary>	
        /// <param name="view">The buffer to clear.</param>	
        /// <param name="value">The value used to clear.</param>	
        /// <remarks>	
        /// <p>This API works on FLOAT, UNORM, and SNORM unordered access views (UAVs), with format conversion from FLOAT to *NORM where appropriate. On other UAVs, the operation is invalid and the call will not reach the driver.</p>	
        /// </remarks>	
        /// <msdn-id>ff476390</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearUnorderedAccessViewFloat([In] ID3D11UnorderedAccessView* pUnorderedAccessView,[In] const float* Values)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearUnorderedAccessViewFloat</unmanaged-short>	
        public void Clear(UnorderedAccessView view, float value)
        {
            Context.ClearUnorderedAccessView(view, value);
        }

        /// <summary>
        /// Copies the content of this resource to another <see cref="GraphicsResource" />.
        /// </summary>
        /// <param name="fromResource">The resource to copy from.</param>
        /// <param name="toResource">The resource to copy to.</param>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476392</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::CopyResource([In] ID3D11Resource* pDstResource,[In] ID3D11Resource* pSrcResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::CopyResource</unmanaged-short>
        public void Copy(Direct3D11.Resource fromResource, Direct3D11.Resource toResource)
        {
            Context.CopyResource(fromResource, toResource);
        }

        /// <summary>
        /// Creates a new device from a <see cref="SharpDX.Direct3D11.Device"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/></returns>
        public static GraphicsDevice New(Device device)
        {
            return new GraphicsDevice(device);
        }

        /// <summary>
        /// Creates a new <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/></returns>
        public static GraphicsDevice New(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            return new GraphicsDevice(type, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new deferred <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <returns>A deferred <see cref="GraphicsDevice"/></returns>
        public GraphicsDevice NewDeferred()
        {
            return new GraphicsDevice(this, new DeviceContext(Device));
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> for immediate rendering.
        /// </summary>
        public readonly GraphicsDevice MainDevice;

        /// <summary>
        /// Gets the <see cref="FeatureLevel"/> for this device.
        /// </summary>
        public readonly FeatureLevel FeatureLevel;

        /// <summary>
        /// Gets whether this <see cref="GraphicsDevice"/> is running in debug.
        /// </summary>
        public readonly bool IsDebugMode;

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> attached to the current thread.
        /// </summary>
        public static GraphicsDevice Current
        {
            get { return current; }
        }

        /// <summary>
        /// Attach this <see cref="GraphicsDevice"/> to the current thread.
        /// </summary>
        public void AttachToCurrentThread()
        {
            current = this;
        }

        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <remarks>
        /// This method creates internally a stagging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public TData[] GetData<TData>(BufferBase buffer, int subResourceIndex = 0) where TData : struct
        {
            var toData = new TData[buffer.Description.SizeInBytes / Utilities.SizeOf<TData>()];
            GetData(buffer, toData, subResourceIndex);
            return toData;
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture datas.</param>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <remarks>
        /// This method creates internally a stagging resource if this texture is not already a stagging resouce, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetData<TData>(BufferBase buffer, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Get data from this resource
            if (buffer.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(buffer, buffer, toData, subResourceIndex);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = buffer.ToStaging())
                    GetData(buffer, throughStaging, toData, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// See the unmanaged documentation for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetData<TData>(BufferBase buffer, BufferBase stagingTexture, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((toData.Length * Utilities.SizeOf<TData>()) > buffer.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // Copy the texture to a staging resource
            Context.CopyResource(buffer, stagingTexture);

            // Map the staging resource to a CPU accessible memory
            var box = Context.MapSubresource(stagingTexture, subResourceIndex, MapMode.Read, Direct3D11.MapFlags.None);
            Utilities.Read(box.DataPointer, toData, 0, toData.Length);
            // Make sure that we unmap the resource in case of an exception
            Context.UnmapSubresource(stagingTexture, subResourceIndex);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetData<TData>(BufferBase buffer, ref TData fromData, int offsetInBytes = 0, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if (Utilities.SizeOf<TData>() > buffer.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // Offset in bytes is set to 0 for constant buffers
            offsetInBytes = (buffer.Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? 0 : offsetInBytes;

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (buffer.Description.Usage == ResourceUsage.Default || buffer.Description.Usage == ResourceUsage.Immutable)
            {
                // Setup the dest region inside the buffer
                var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + Utilities.SizeOf<TData>(), 1, 1);
                Context.UpdateSubresource(ref fromData, buffer, subResourceIndex, 0, 0, (buffer.Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? (ResourceRegion?)null : destRegion);
            }
            else
            {
                try
                {
                    var box = Context.MapSubresource(buffer, subResourceIndex, MapMode.WriteDiscard, Direct3D11.MapFlags.None);
                    Utilities.Write((IntPtr)((byte*)box.DataPointer + offsetInBytes), ref fromData);
                }
                finally
                {
                    Context.UnmapSubresource(buffer, subResourceIndex);
                }
            }
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetData<TData>(BufferBase buffer, TData[] fromData, int offsetInBytes = 0, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((fromData.Length * Utilities.SizeOf<TData>()) > buffer.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (buffer.Description.Usage == ResourceUsage.Default || buffer.Description.Usage == ResourceUsage.Immutable)
            {
                // Setup the dest region inside the buffer
                var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + fromData.Length * Utilities.SizeOf<TData>(), 1, 1);
                Context.UpdateSubresource(fromData, buffer, subResourceIndex, 0, 0, (buffer.Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? (ResourceRegion?)null : destRegion);
            }
            else
            {
                try
                {
                    var box = Context.MapSubresource(buffer, subResourceIndex, MapMode.WriteDiscard, Direct3D11.MapFlags.None);
                    Utilities.Write((IntPtr)((byte*)box.DataPointer + offsetInBytes), fromData, 0, fromData.Length);
                }
                finally
                {
                    Context.UnmapSubresource(buffer, subResourceIndex);
                }
            }
        }

        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture.</param>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <returns></returns>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>This method creates internally a stagging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        public TData[] GetData<TData>(Texture2DBase texture, int subResourceIndex = 0) where TData : struct
        {
            var toData = new TData[CalculateElementWidth<TData>(texture) * texture.Description.Height];
            GetData(texture, toData, subResourceIndex);
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
        public void GetData<TData>(Texture2DBase texture, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Get data from this resource
            if (texture.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(texture, texture, toData, subResourceIndex);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = texture.ToStaging())
                    GetData(texture, throughStaging, toData, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture to get the data from.</param>
        /// <param name="stagingTexture">The staging texture used to transfer the texture to.</param>
        /// <param name="toData">To data.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void GetData<TData>(Texture2DBase texture, Texture2DBase stagingTexture, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((toData.Length * Utilities.SizeOf<TData>()) != (texture.StrideInBytes * texture.Description.Height))
                throw new ArgumentException("Length of TData is not compatible with Width * Height * Pixel size in bytes");

            // Copy the texture to a staging resource
            Context.CopyResource(texture, stagingTexture);

            try
            {
                int width = CalculateElementWidth<TData>(texture);

                // Map the staging resource to a CPU accessible memory
                var box = Context.MapSubresource(stagingTexture, subResourceIndex, MapMode.Read, MapFlags.None);

                // The fast way: If same stride, we can directly copy the whole texture in one shot
                if (box.RowPitch == texture.StrideInBytes)
                {
                    Utilities.Read(box.DataPointer, toData, 0, toData.Length);
                }
                else
                {
                    // Otherwise, the long way by copying each scanline
                    int offsetStride = 0;
                    var sourcePtr = (byte*)box.DataPointer;

                    for (int i = 0; i < texture.Description.Height; i++)
                    {
                        Utilities.Read((IntPtr)sourcePtr, toData, offsetStride, width);
                        sourcePtr += box.RowPitch;
                        offsetStride += width;
                    }
                }
            }
            finally
            {
                // Make sure that we unmap the resource in case of an exception
                Context.UnmapSubresource(stagingTexture, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void SetData<TData>(Texture2DBase texture, TData[] fromData, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((fromData.Length * Utilities.SizeOf<TData>()) != (texture.StrideInBytes * texture.Description.Height))
                throw new ArgumentException("Length of TData is not compatible with Width * Height * Pixel size in bytes");

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (texture.Description.Usage == ResourceUsage.Default || texture.Description.Usage == ResourceUsage.Immutable)
            {
                Context.UpdateSubresource(fromData, texture, subResourceIndex, texture.StrideInBytes);
            }
            else
            {
                try
                {
                    int width = CalculateElementWidth<TData>(texture);
                    var box = Context.MapSubresource(texture, subResourceIndex, MapMode.WriteDiscard,
                                                     MapFlags.None);
                    // The fast way: If same stride, we can directly copy the whole texture in one shot
                    if (box.RowPitch == texture.StrideInBytes)
                    {
                        Utilities.Write(box.DataPointer, fromData, 0, fromData.Length);
                    }
                    else
                    {
                        // Otherwise, the long way by copying each scanline
                        int offsetStride = 0;
                        var destPtr = (byte*)box.DataPointer;

                        for (int i = 0; i < texture.Description.Height; i++)
                        {
                            Utilities.Write((IntPtr)destPtr, fromData, offsetStride, width);
                            destPtr += box.RowPitch;
                            offsetStride += width;
                        }

                    }
                }
                finally
                {
                    Context.UnmapSubresource(texture, subResourceIndex);
                }
            }
        }

        public static implicit operator Device(GraphicsDevice from)
        {
            return from.Device;
        }

        public static implicit operator DeviceContext(GraphicsDevice from)
        {
            return from.Context;
        }

        /// <summary>
        /// Calculates the width of the element.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <returns>The width</returns>
        /// <exception cref="System.ArgumentException">If the size is invalid</exception>
        private int CalculateElementWidth<TData>(Texture2DBase texture) where TData : struct
        {
            var dataStrideInBytes = Utilities.SizeOf<TData>() * texture.Description.Width;
            var width = ((double)texture.StrideInBytes / dataStrideInBytes) * texture.Description.Width;
            if (Math.Abs(width - (int)width) > double.Epsilon)
                throw new ArgumentException("sizeof(TData) / sizeof(Format) * Width is not an integer");
            return (int)width;
        }
    }
}
