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
using System.IO;
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
        private byte[] copyTempBuffer;

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

        public static GraphicsDevice New(Device device)
        {
            return new GraphicsDevice(device);
        }

        public static GraphicsDevice New(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            return new GraphicsDevice(type, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new deferred <see cref="GraphicsDeviceContext"/>.
        /// </summary>
        /// <returns>A deferred <see cref="GraphicsDeviceContext"/></returns>
        public GraphicsDevice NewDeferred()
        {
            return new GraphicsDevice(this, new DeviceContext(Device));
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDeviceContext"/> for immediate rendering.
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
        /// Copy the entire contents of the source resource to the destination resource using the GPU.
        /// </summary>
        /// <param name="fromResource">The resource to copy from.</param>
        /// <param name="toResource">The resource to copy to.</param>
        /// <remarks>
        /// This method is unusual in that it causes the GPU to perform the copy operation (similar to a memcpy by the CPU). As a result, it has a few restrictions designed for improving performance. 
        /// For instance, the source and destination resources:  
        /// - Must be different resources. 
        /// - Must be the same type. 
        /// - Must have identical dimensions (including width, height, depth, and size as appropriate). 
        /// Will only be copied. CopyResource does not support any stretch, color key, blend, or format conversions. 
        /// Must have compatible DXGI formats, which means the formats must be identical or at least from the same type group. For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. Might not be currently mapped.  
        /// </remarks>
        public void Copy(GraphicsResource fromResource, GraphicsResource toResource)
        {
            Context.CopyResource(fromResource, toResource);
        }

        public void Copy<TData>(GraphicsResource fromResource, ref TData toData, int subResourceIndex = 0) where TData : struct
        {
            using (var throughStaging = fromResource.ToStaging())
                Copy(fromResource, throughStaging, ref toData, subResourceIndex);
        }

        public void Copy<TData>(GraphicsResource fromResource, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            using (var throughStaging = fromResource.ToStaging())
                Copy(fromResource, throughStaging, toData, subResourceIndex);
        }

        public void Copy<TData>(GraphicsResource fromResource, GraphicsResource throughStaging, ref TData toData, int subResourceIndex = 0) where TData : struct
        {
            Context.CopyResource(fromResource, throughStaging);
            // TODO HANDLE stride
            var box = Context.MapSubresource(throughStaging, 0, MapMode.Read, MapFlags.None);
            Utilities.Read(box.DataPointer, ref toData);
            Context.UnmapSubresource(throughStaging, 0);
        }

        public void Copy<TData>(GraphicsResource fromResource, GraphicsResource throughStaging, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            Context.CopyResource(fromResource, throughStaging);
            // TODO HANDLE stride
            var box = Context.MapSubresource(throughStaging, subResourceIndex, MapMode.Read, MapFlags.None);
            Utilities.Read(box.DataPointer, toData, 0, toData.Length);
            Context.UnmapSubresource(throughStaging, subResourceIndex);
        }

        public void Copy<TData>(GraphicsResource fromResource, GraphicsResource throughStaging, TData[,] toData, int subResourceIndex = 0) where TData : struct
        {
            Context.CopyResource(fromResource, throughStaging);
            // TODO HANDLE stride
            var box = Context.MapSubresource(throughStaging, subResourceIndex, MapMode.Read, MapFlags.None);
            Utilities.Read(box.DataPointer, toData, 0, toData.Length);
            Context.UnmapSubresource(throughStaging, subResourceIndex);
        }

        public void Copy<TData>(ref TData fromData, GraphicsResource toResource, int offset = 0, int subResourceIndex = 0) where TData : struct
        {
            var box = Context.MapSubresource(toResource, subResourceIndex, MapMode.WriteDiscard, MapFlags.None);
            // TODO HANDLE stride
            Utilities.Write(Utilities.IntPtrAdd(box.DataPointer, offset), ref fromData);
            Context.UnmapSubresource(toResource, subResourceIndex);
        }

        public void Copy<TData>(TData[] fromData, GraphicsResource toResource, int offset = 0, int subResourceIndex = 0) where TData : struct
        {
            var box = Context.MapSubresource(toResource, subResourceIndex, MapMode.WriteDiscard, MapFlags.None);
            // TODO HANDLE stride
            Utilities.Write(Utilities.IntPtrAdd(box.DataPointer, offset), fromData, 0, fromData.Length);
            Context.UnmapSubresource(toResource, subResourceIndex);
        }

        public unsafe void Copy(Stream fromStream, GraphicsResource toResource, int sizeInBytesToCopy = 0, int offset = 0, int subResourceIndex = 0)
        {
            // TODO HANDLE stride
            var box = Context.MapSubresource(toResource, subResourceIndex, MapMode.WriteDiscard, MapFlags.None);

            // If Stream is a DataStream, copy it directly
            if (fromStream is DataStream)
            {
                Utilities.CopyMemory(Utilities.IntPtrAdd(box.DataPointer, offset), ((DataStream)fromStream).PositionPointer,
                                     sizeInBytesToCopy == 0 ? (int)((DataStream)fromStream).RemainingLength : sizeInBytesToCopy);
            }
            else
            {
                // Else Batch copy
                if (copyTempBuffer == null)
                    copyTempBuffer = new byte[16384];

                // Calculate the number of byte to copy if it is not specified
                sizeInBytesToCopy = sizeInBytesToCopy == 0
                                        ? (int)(fromStream.Length - fromStream.Position)
                                        : sizeInBytesToCopy;

                var outputPtr = Utilities.IntPtrAdd(box.DataPointer, offset);

                fixed (void* pBuffer = copyTempBuffer)
                {
                    do
                    {
                        // Calculate the minimum amount of data to read from the stream
                        int dataToRead = Math.Min(copyTempBuffer.Length, sizeInBytesToCopy);

                        // Read the data from the stream
                        int readLength = fromStream.Read(copyTempBuffer, 0, dataToRead);

                        if (readLength == 0)
                            break;

                        // Copy the data from the stream to the mapped buffer
                        Utilities.CopyMemory(outputPtr, (IntPtr)pBuffer, readLength);

                        // Go to next position in buffer
                        outputPtr = Utilities.IntPtrAdd(outputPtr, readLength);
                        sizeInBytesToCopy -= readLength;
                    } while (sizeInBytesToCopy > 0);
                }
            }

            Context.UnmapSubresource(toResource, subResourceIndex);
        }

        public static implicit operator Device(GraphicsDevice from)
        {
            return from.Device;
        }

        public static implicit operator DeviceContext(GraphicsDevice from)
        {
            return from.Context;
        }
    }
}
