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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A class used to shader input resource parameters and prepare them (i.e. take their NativePointer ASAP).
    /// </summary>
    class EffectResourceLinker : IDisposable
    {
        /// <summary>
        /// Real object resources, as they were set on the parameter.
        /// </summary>
        private object[] resources;

        /// <summary>
        /// Real object resources, as they were set on the parameter.
        /// </summary>
        public Buffer[] ConstantBuffers;

        /// <summary>
        /// Total number of resources.
        /// </summary>
        public int Count;

        /// <summary>
        /// Pointer to native pointers.
        /// </summary>
        public unsafe IntPtr* Pointers;

        /// <summary>
        /// Pointer to native pointers.
        /// </summary>
        public unsafe int* UAVCounts;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            resources = new object[Count];
            ConstantBuffers = new Buffer[Count];
            unsafe
            {
                // Initialize UAV counts to -1
                UAVCounts = (int*)Utilities.AllocateMemory(Count * sizeof(int));
                for (int i = 0; i < Count; i++)
                {
                    UAVCounts[i] = -1;
                }

                Pointers = (IntPtr*)Utilities.AllocateClearedMemory(Count * sizeof(IntPtr));
            }
        }

        public void Dispose()
        {
            unsafe
            {
                if ((IntPtr)Pointers != IntPtr.Zero)
                {
                    Utilities.FreeMemory((IntPtr)Pointers);
                    Pointers = (IntPtr*)IntPtr.Zero;
                }
            }
        }

        public T GetResource<T>(int resourceIndex) where T : class
        {
            return (T)resources[resourceIndex];
        }

        public void SetResource(int resourceIndex, EffectResourceType type, Direct3D11.UnorderedAccessView view, int uavInitialCount)
        {
            resources[resourceIndex] = view;
            unsafe
            {
                UAVCounts[resourceIndex] = uavInitialCount;
                Pointers[resourceIndex] = GetNativePointer(resourceIndex, type, view);
            }
        }

        public void SetResource<T>(int resourceIndex, EffectResourceType type, T value)
        {
            resources[resourceIndex] = value;
            unsafe
            {
                Pointers[resourceIndex] = GetNativePointer(resourceIndex, type, value);
            }
        }

        public void SetResourcePointer(int resourceIndex, EffectResourceType type, IntPtr value)
        {
            // Don't setup the resources at the global level, only setup the native pointer
            //resources[resourceIndex] = value;
            unsafe
            {
                Pointers[resourceIndex] = value;
            }
        }

        public void SetResource<T>(int resourceIndex, EffectResourceType type, params T[] valueArray) where T : class
        {
            foreach (var value in valueArray)
            {
                resources[resourceIndex] = value;
                unsafe
                {
                    Pointers[resourceIndex] = GetNativePointer(resourceIndex, type, value);
                }
                resourceIndex++;
            }
        }

        public void SetResource(int resourceIndex, EffectResourceType type, Direct3D11.UnorderedAccessView[] valueArray, int[] uavInitialCount)
        {
            for (int i = 0; i < valueArray.Length; i++, resourceIndex++)
            {
                var value = valueArray[i];
                resources[resourceIndex] = value;
                unsafe
                {
                    UAVCounts[resourceIndex] = uavInitialCount[i];
                    Pointers[resourceIndex] = GetNativePointer(resourceIndex, type, value);
                }
            }
        }

        private IntPtr GetNativePointer(int resourceIndex, EffectResourceType type, object value)
        {
            if (value == null)
                return IntPtr.Zero;

            if (value is IntPtr)
                return (IntPtr) value;

            switch (type)
            {
                case EffectResourceType.ConstantBuffer:
                    {
                        var constantBuffer = value as EffectConstantBuffer;
                        if (constantBuffer != null)
                        {
                            var rawBuffer = constantBuffer;
                            ConstantBuffers[resourceIndex] = rawBuffer;
                            return ((Direct3D11.Buffer)rawBuffer).NativePointer;
                        }
                        var buffer = value as Buffer;
                        if (buffer != null)
                        {
                            var rawBuffer = buffer;
                            ConstantBuffers[resourceIndex] = rawBuffer;
                            return ((Direct3D11.Buffer)rawBuffer).NativePointer;
                        }
                    }
                    break;
                case EffectResourceType.ShaderResourceView:
                    {
                        var buffer = value as Buffer;
                        if (buffer != null)
                            return ((Direct3D11.ShaderResourceView) buffer).NativePointer;
                        var texture = value as Texture;
                        if (texture != null)
                            return ((Direct3D11.ShaderResourceView) texture).NativePointer;
                        var srv = value as Direct3D11.ShaderResourceView;
                        if (srv != null)
                            return srv.NativePointer;
                    }
                    break;
                case EffectResourceType.UnorderedAccessView:
                    {
                        var buffer = value as Buffer;
                        if (buffer != null)
                            return ((Direct3D11.UnorderedAccessView) buffer).NativePointer;
                        var texture = value as Texture;
                        if (texture != null)
                            return ((Direct3D11.UnorderedAccessView)texture).NativePointer;
                        var srv = value as Direct3D11.UnorderedAccessView;
                        if (srv != null)
                            return srv.NativePointer;
                    }
                    break;
                case EffectResourceType.SamplerState:
                    var samplerState = value as Graphics.SamplerState;
                    if (samplerState != null)
                        return ((Direct3D11.SamplerState) samplerState).NativePointer;
                    var samplerStateD3D11 = value as Direct3D11.SamplerState;
                    if (samplerStateD3D11 != null)
                        return samplerStateD3D11.NativePointer;
                    break;
            }

            // Throws an exception if the resource is not supported.
            throw new NotSupportedException(string.Format("Unsupported resource type [{0}/{1}]", type, value.GetType().Name));
        }
    }
}