// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpDX.Direct3D11
{
    /// <summary>
    /// Common Shader class. Provides a common set of methods for a Shader Stage.
    /// TODO: check if usage of abstract is not introducing an unacceptable overhead...
    /// </summary>
    public abstract partial class CommonShaderStage : CppObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonShaderStage&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        protected CommonShaderStage(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        ///   Gets the constant buffers used by the shader stage.
        /// </summary>
        /// <param name = "startSlot">Index into the device's zero-based array from which to begin retrieving constant buffers.</param>
        /// <param name = "count">Number of buffers to retrieve.</param>
        /// <returns>An array of constant buffers.</returns>
        public Buffer[] GetConstantBuffers(int startSlot, int count)
        {
            var buffers = new Buffer[count];
            GetConstantBuffers(startSlot, count, buffers);
            return buffers;
        }

        /// <summary>
        ///   Gets the sampler states used by the shader stage.
        /// </summary>
        /// <param name = "startSlot">Index into the device's zero-based array from which to begin retrieving samplers.</param>
        /// <param name = "count">Number of samplers to retrieve.</param>
        /// <returns>An array of sampler states.</returns>
        public SamplerState[] GetSamplers(int startSlot, int count)
        {
            var buffers = new SamplerState[count];
            GetSamplers(startSlot, count, buffers);
            return buffers;
        }

        /// <summary>
        ///   Gets the shader resources used by the shader stage.
        /// </summary>
        /// <param name = "startSlot">Index into the device's zero-based array from which to begin retrieving shader resources.</param>
        /// <param name = "count">Number of resources to retrieve.</param>
        /// <returns>An array of shader resources.</returns>
        public ShaderResourceView[] GetShaderResources(int startSlot, int count)
        {
            var buffers = new ShaderResourceView[count];
            GetShaderResources(startSlot, count, buffers);
            return buffers;
        }

        /// <summary>
        ///   Sets a single constant buffer to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the constant buffer.</param>
        /// <param name = "constantBuffer">constant buffer to set</param>
        public void SetConstantBuffer(int slot, Buffer constantBuffer)
        {
            var tempPtr = constantBuffer == null ? IntPtr.Zero : constantBuffer.NativePointer;
            unsafe
            {
                SetConstantBuffers(slot, 1, new IntPtr(&tempPtr));
            }
        }

        /// <summary>
        ///   Sets an array of constant buffers to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the array of constant buffers.</param>
        /// <param name = "constantBuffers">An array of constant buffer to set</param>
        public void SetConstantBuffers(int slot, params Buffer[] constantBuffers)
        {
            SetConstantBuffers(slot, constantBuffers == null ? 0 : constantBuffers.Length, constantBuffers);
        }

        /// <summary>
        ///   Sets an array of constant buffers to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the array of constant buffers.</param>
        /// <param name = "constantBuffers">An array of constant buffer to set</param>
        public void SetConstantBuffers(int slot, ComArray<Buffer> constantBuffers)
        {
            SetConstantBuffers(slot, constantBuffers == null ? 0 : constantBuffers.Length, constantBuffers);
        }

        /// <summary>
        ///   Sets a single sampler to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the sampler.</param>
        /// <param name = "sampler">sampler state to set</param>
        public void SetSampler(int slot, SamplerState sampler)
        {
            var tempPtr = sampler == null ? IntPtr.Zero : sampler.NativePointer;
            unsafe
            {
                SetSamplers(slot, 1, new IntPtr(&tempPtr));
            }
        }

        /// <summary>
        ///   Sets an array of samplers to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the array of sampler states.</param>
        /// <param name = "samplers">An array of sampler state to set</param>
        public void SetSamplers(int slot, params SamplerState[] samplers)
        {
            SetSamplers(slot, samplers == null ? 0 : samplers.Length, samplers);
        }

        /// <summary>
        ///   Sets an array of samplers to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which  to set the array of sampler states.</param>
        /// <param name = "samplers">An array of sampler state to set</param>
        public void SetSamplers(int slot, ComArray<SamplerState> samplers)
        {
            SetSamplers(slot, samplers == null ? 0 : samplers.Length, samplers);
        }

        /// <summary>
        ///   Sets a single shader resource to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the resource.</param>
        /// <param name = "resourceView">Resource view to attach</param>
        public void SetShaderResource(int slot, ShaderResourceView resourceView)
        {
            IntPtr localPointer = resourceView == null ? IntPtr.Zero : resourceView.NativePointer;
            unsafe
            {
                SetShaderResources(slot, 1, new IntPtr(&localPointer));
            }
        }

        /// <summary>
        ///   Bind an array of shader resources to the shader stage.
        /// </summary>
        /// <remarks>
        ///   If an overlapping resource view is already bound to an output slot, such as a render target, then this API will fill the destination shader resource slot with NULL.For information about creating shader-resource views, see <see cref = "SharpDX.Direct3D11.Device.CreateShaderResourceView" />. The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting shader resources to (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "shaderResourceViews">Array of {{shader resource view}} interfaces to set to the device.</param>
        public void SetShaderResources(int startSlot, params SharpDX.Direct3D11.ShaderResourceView[] shaderResourceViews)
        {
            SetShaderResources(startSlot, shaderResourceViews.Length, shaderResourceViews);
        }

        /// <summary>
        ///   Bind an array of shader resources to the shader stage.
        /// </summary>
        /// <remarks>
        ///   If an overlapping resource view is already bound to an output slot, such as a render target, then this API will fill the destination shader resource slot with NULL.For information about creating shader-resource views, see <see cref = "SharpDX.Direct3D11.Device.CreateShaderResourceView" />. The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting shader resources to (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "shaderResourceViews">Array of {{shader resource view}} interfaces to set to the device.</param>
        public void SetShaderResources(int startSlot, ComArray<SharpDX.Direct3D11.ShaderResourceView> shaderResourceViews)
        {
            SetShaderResources(startSlot, shaderResourceViews.Length, shaderResourceViews);
        }

        /// <summary>
        ///   Get the shader resources.
        /// </summary>
        /// <remarks>
        ///   Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin getting shader resources from (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "numViews">The number of resources to get from the device. Up to a maximum of 128 slots are available for shader resources (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - StartSlot).</param>
        /// <param name = "shaderResourceViewsRef">Array of {{shader resource view}} interfaces to be returned by the device.</param>
        /// <unmanaged>void PSGetShaderResources([In] UINT StartSlot,[In] UINT NumViews,[Out, Buffer] ID3D11ShaderResourceView** ppShaderResourceViews)</unmanaged>
        internal abstract void GetShaderResources(
            int startSlot,
            int numViews,
            SharpDX.Direct3D11.ShaderResourceView[] shaderResourceViewsRef);

        /// <summary>
        ///   Get an array of sampler states from the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks.
        /// </remarks>
        /// <param name = "startSlot">Index into a zero-based array to begin getting samplers from (ranges from 0 to D3D11_COMMONSHADER_SAMPLER_SLOT_COUNT - 1).</param>
        /// <param name = "numSamplers">Number of samplers to get from a device context. Each pipeline stage has a total of 16 sampler slots available (ranges from 0 to D3D11_COMMONSHADER_SAMPLER_SLOT_COUNT - StartSlot).</param>
        /// <param name = "samplersRef">Array of sampler-state interface pointers (see <see cref = "SharpDX.Direct3D11.SamplerState" />) to be returned by the device.</param>
        /// <unmanaged>void PSGetSamplers([In] UINT StartSlot,[In] UINT NumSamplers,[Out, Buffer] ID3D11SamplerState** ppSamplers)</unmanaged>
        internal abstract void GetSamplers(
            int startSlot,
            int numSamplers,
            SharpDX.Direct3D11.SamplerState[] samplersRef);

        /// <summary>
        ///   Get the constant buffers used by the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin retrieving constant buffers from (ranges from 0 to D3D11_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - 1).</param>
        /// <param name = "numBuffers">Number of buffers to retrieve (ranges from 0 to D3D11_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - StartSlot).</param>
        /// <param name = "constantBuffersRef">Array of constant buffer interface pointers (see <see cref = "SharpDX.Direct3D11.Buffer" />) to be returned by the method.</param>
        /// <unmanaged>void PSGetConstantBuffers([In] UINT StartSlot,[In] UINT NumBuffers,[Out, Buffer] ID3D11Buffer** ppConstantBuffers)</unmanaged>
        internal abstract void GetConstantBuffers(
            int startSlot,
            int numBuffers,
            SharpDX.Direct3D11.Buffer[] constantBuffersRef);


        /// <summary>
        ///   Bind an array of shader resources to the shader stage.
        /// </summary>
        /// <remarks>
        ///   If an overlapping resource view is already bound to an output slot, such as a render target, then this API will fill the destination shader resource slot with NULL.For information about creating shader-resource views, see <see cref = "SharpDX.Direct3D11.Device.CreateShaderResourceView" />. The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting shader resources to (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "numViews">Number of shader resources to set. Up to a maximum of 128 slots are available for shader resources (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - StartSlot).</param>
        /// <param name = "shaderResourceViews">Array of {{shader resource view}} interfaces to set to the device.</param>
        /// <unmanaged>void PSSetShaderResources([In] UINT StartSlot,[In] UINT NumViews,[In, Buffer] const ID3D11ShaderResourceView** ppShaderResourceViews)</unmanaged>
        public void SetShaderResources(int startSlot, int numViews, params SharpDX.Direct3D11.ShaderResourceView[] shaderResourceViews)
        {
            unsafe
            {
                var shaderResourceViewsPtr = (IntPtr*)0;
                if (numViews > 0)
                {
                    var tempPtr = stackalloc IntPtr[numViews];
                    shaderResourceViewsPtr = tempPtr;
                    for (int i = 0; i < numViews; i++)
                        shaderResourceViewsPtr[i] = (shaderResourceViews[i] == null) ? IntPtr.Zero : shaderResourceViews[i].NativePointer;
                }
                SetShaderResources(startSlot, numViews, (IntPtr)shaderResourceViewsPtr);
            }
        }

        /// <summary>
        ///   Bind an array of shader resources to the shader stage.
        /// </summary>
        /// <remarks>
        ///   If an overlapping resource view is already bound to an output slot, such as a render target, then this API will fill the destination shader resource slot with NULL.For information about creating shader-resource views, see <see cref = "SharpDX.Direct3D11.Device.CreateShaderResourceView" />. The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting shader resources to (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "numViews">Number of shader resources to set. Up to a maximum of 128 slots are available for shader resources (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - StartSlot).</param>
        /// <param name = "shaderResourceViewsRef">Array of {{shader resource view}} interfaces to set to the device.</param>
        /// <unmanaged>void PSSetShaderResources([In] UINT StartSlot,[In] UINT NumViews,[In, Buffer] const ID3D11ShaderResourceView** ppShaderResourceViews)</unmanaged>
        public void SetShaderResources(
            int startSlot,
            int numViews,
            SharpDX.ComArray<SharpDX.Direct3D11.ShaderResourceView> shaderResourceViewsRef)
        {
            SetShaderResources(startSlot, numViews, shaderResourceViewsRef.NativePointer);
        }

        /// <summary>
        ///   Bind an array of shader resources to the shader stage.
        /// </summary>
        /// <remarks>
        ///   If an overlapping resource view is already bound to an output slot, such as a render target, then this API will fill the destination shader resource slot with NULL.For information about creating shader-resource views, see <see cref = "SharpDX.Direct3D11.Device.CreateShaderResourceView" />. The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting shader resources to (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "numViews">Number of shader resources to set. Up to a maximum of 128 slots are available for shader resources (ranges from 0 to D3D11_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - StartSlot).</param>
        /// <param name = "shaderResourceViewsRef">Array of {{shader resource view}} interfaces to set to the device.</param>
        /// <unmanaged>void PSSetShaderResources([In] UINT StartSlot,[In] UINT NumViews,[In, Buffer] const ID3D11ShaderResourceView** ppShaderResourceViews)</unmanaged>
        internal abstract void SetShaderResources(int startSlot, int numViews, IntPtr shaderResourceViewsRef);

        /// <summary>
        ///   Set an array of sampler states to the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any sampler may be set to NULL; this invokes the default state, which is defined to be the following.StateDefault ValueFilterD3D11_FILTER_MIN_MAG_MIP_LINEARAddressUD3D11_TEXTURE_ADDRESS_CLAMPAddressVD3D11_TEXTURE_ADDRESS_CLAMPAddressWD3D11_TEXTURE_ADDRESS_CLAMPMipLODBias0MaxAnisotropy1ComparisonFuncD3D11_COMPARISON_NEVERBorderColor[0]1.0fBorderColor[1]1.0fBorderColor[2]1.0fBorderColor[3]1.0fMinLOD-FLT_MAXMaxLODFLT_MAX  The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting samplers to (ranges from 0 to D3D11_COMMONSHADER_SAMPLER_SLOT_COUNT - 1).</param>
        /// <param name = "numSamplers">Number of samplers in the array. Each pipeline stage has a total of 16 sampler slots available (ranges from 0 to D3D11_COMMONSHADER_SAMPLER_SLOT_COUNT - StartSlot).</param>
        /// <param name = "samplers">Pointer to an array of sampler-state interfaces (see <see cref = "SharpDX.Direct3D11.SamplerState" />). See Remarks.</param>
        /// <unmanaged>void PSSetSamplers([In] UINT StartSlot,[In] UINT NumSamplers,[In, Buffer] const ID3D11SamplerState** ppSamplers)</unmanaged>
        public void SetSamplers(int startSlot, int numSamplers, params SharpDX.Direct3D11.SamplerState[] samplers)
        {
            unsafe
            {
                var samplersPtr = (IntPtr*)0;
                if (numSamplers > 0)
                {
                    var tempPtr = stackalloc IntPtr[numSamplers];
                    samplersPtr = tempPtr;
                    for (int i = 0; i < numSamplers; i++)
                        samplersPtr[i] = (samplers[i] == null) ? IntPtr.Zero : samplers[i].NativePointer;
                }
                SetSamplers(startSlot, numSamplers, (IntPtr)samplersPtr);
            }
        }

        /// <summary>
        ///   Set an array of sampler states to the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any sampler may be set to NULL; this invokes the default state, which is defined to be the following.StateDefault ValueFilterD3D11_FILTER_MIN_MAG_MIP_LINEARAddressUD3D11_TEXTURE_ADDRESS_CLAMPAddressVD3D11_TEXTURE_ADDRESS_CLAMPAddressWD3D11_TEXTURE_ADDRESS_CLAMPMipLODBias0MaxAnisotropy1ComparisonFuncD3D11_COMPARISON_NEVERBorderColor[0]1.0fBorderColor[1]1.0fBorderColor[2]1.0fBorderColor[3]1.0fMinLOD-FLT_MAXMaxLODFLT_MAX  The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting samplers to (ranges from 0 to D3D11_COMMONSHADER_SAMPLER_SLOT_COUNT - 1).</param>
        /// <param name = "numSamplers">Number of samplers in the array. Each pipeline stage has a total of 16 sampler slots available (ranges from 0 to D3D11_COMMONSHADER_SAMPLER_SLOT_COUNT - StartSlot).</param>
        /// <param name = "samplers">Pointer to an array of sampler-state interfaces (see <see cref = "SharpDX.Direct3D11.SamplerState" />). See Remarks.</param>
        /// <unmanaged>void PSSetSamplers([In] UINT StartSlot,[In] UINT NumSamplers,[In, Buffer] const ID3D11SamplerState** ppSamplers)</unmanaged>
        public void SetSamplers(int startSlot, int numSamplers, SharpDX.ComArray<SharpDX.Direct3D11.SamplerState> samplers)
        {
            SetSamplers(startSlot, numSamplers, samplers.NativePointer);
        }

        internal abstract void SetSamplers(int startSlot, int numSamplers, IntPtr samplersRef);

        /// <summary>
        ///   Set the constant buffers used by the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting constant buffers to (ranges from 0 to D3D11_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - 1).</param>
        /// <param name = "numBuffers">Number of buffers to set (ranges from 0 to D3D11_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - StartSlot).</param>
        /// <param name = "constantBuffers">Array of constant buffers (see <see cref = "SharpDX.Direct3D11.Buffer" />) being given to the device.</param>
        /// <unmanaged>void PSSetConstantBuffers([In] UINT StartSlot,[In] UINT NumBuffers,[In, Buffer] const ID3D11Buffer** ppConstantBuffers)</unmanaged>
        public void SetConstantBuffers(int startSlot, int numBuffers, params SharpDX.Direct3D11.Buffer[] constantBuffers)
        {
            unsafe
            {
                var constantBuffersPtr = (IntPtr*)0;
                if (numBuffers > 0)
                {
                    var tempPtr = stackalloc IntPtr[numBuffers];
                    constantBuffersPtr = tempPtr;
                    for (int i = 0; i < numBuffers; i++)
                        constantBuffersPtr[i] = (constantBuffers[i] == null) ? IntPtr.Zero : constantBuffers[i].NativePointer;
                }
                SetConstantBuffers(startSlot, numBuffers, (IntPtr)constantBuffersPtr);
            }
        }

        /// <summary>
        ///   Set the constant buffers used by the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting constant buffers to (ranges from 0 to D3D11_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - 1).</param>
        /// <param name = "numBuffers">Number of buffers to set (ranges from 0 to D3D11_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - StartSlot).</param>
        /// <param name = "constantBuffers">Array of constant buffers (see <see cref = "SharpDX.Direct3D11.Buffer" />) being given to the device.</param>
        /// <unmanaged>void PSSetConstantBuffers([In] UINT StartSlot,[In] UINT NumBuffers,[In, Buffer] const ID3D11Buffer** ppConstantBuffers)</unmanaged>
        public void SetConstantBuffers(int startSlot, int numBuffers, SharpDX.ComArray<SharpDX.Direct3D11.Buffer> constantBuffers)
        {
            SetConstantBuffers(startSlot, numBuffers, constantBuffers.NativePointer);
        }

        internal abstract void SetConstantBuffers(int startSlot, int numBuffers, IntPtr constantBuffersRef);

        public abstract void SetShader(DeviceChild shader, SharpDX.Direct3D11.ClassInstance[] classInstancesOut, int numClassInstances);

        internal abstract void SetUnorderedAccessViews(int startSlot, int numBuffers, IntPtr unorderedAccessBuffer, IntPtr uavCount);
    }

    /// <summary>
    /// Common Shader class. Provides a common set of methods for a Shader Stage.
    /// TODO: check if usage of abstract is not introducing an unacceptable overhead...
    /// </summary>
    /// <typeparam name = "T">Type of the shader</typeparam>
    public abstract class CommonShaderStage<T> : CommonShaderStage where T : DeviceChild
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonShaderStage&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        protected CommonShaderStage(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        ///   Gets the shader currently assigned to the device.
        /// </summary>
        /// <returns>The shader (null if no shader is assigned).</returns>
        public T Get()
        {
            T temp;
            int numClassInstances = 0;
            GetShader(out temp, null, ref numClassInstances);
            return temp;
        }

        /// <summary>
        ///   Gets the shader currently assigned to the device.
        /// </summary>
        /// <param name = "classInstances">An array that will be used to contain any class instances currently active.</param>
        /// <returns>The shader (null if no shader is assigned).</returns>
        public T Get(ClassInstance[] classInstances)
        {
            T temp;
            int numClassInstances = classInstances.Length;
            GetShader(out temp, classInstances, ref numClassInstances);
            return temp;
        }

        /// <summary>
        ///   Assigns a compute shader to the device.
        /// </summary>
        /// <param name = "shader">The shader to assign to the device. Assign null to disable the compute shader.</param>
        public void Set(T shader)
        {
            SetShader(shader, (SharpDX.ComArray<SharpDX.Direct3D11.ClassInstance>)null, 0);
        }

        /// <summary>
        ///   Assigns a compute shader to the device.
        /// </summary>
        /// <param name = "shader">The shader to assign to the device. Assign <c>null</c> to disable the compute shader.</param>
        /// <param name = "classInstances">An array of class-instance interfaces. Each interface used by a shader must have a corresponding class instance or the shader will get disabled.</param>
        public void Set(T shader, ClassInstance[] classInstances)
        {
            SetShader(shader, classInstances, classInstances == null ? 0 : classInstances.Length);
        }

        /// <summary>
        ///   Assigns a compute shader to the device.
        /// </summary>
        /// <param name = "shader">The shader to assign to the device. Assign <c>null</c> to disable the compute shader.</param>
        /// <param name = "classInstances">An array of class-instance interfaces. Each interface used by a shader must have a corresponding class instance or the shader will get disabled.</param>
        public void Set(T shader, SharpDX.ComArray<ClassInstance> classInstances)
        {
            SetShader(shader, classInstances, classInstances == null ? 0 : classInstances.Length);
        }

        public override void SetShader(DeviceChild shader, SharpDX.Direct3D11.ClassInstance[] classInstancesOut, int numClassInstances)
        {
            SetShader((T)shader, classInstancesOut, numClassInstances);
        }

        internal override void SetUnorderedAccessViews(int startSlot, int numBuffers, IntPtr unorderedAccessBuffer, IntPtr uavCount)
        {
            throw new NotSupportedException();
        }

        internal abstract void SetShader(T shaderRef, SharpDX.Direct3D11.ClassInstance[] classInstancesRef, int numClassInstances);

        internal abstract void SetShader(T shaderRef, SharpDX.ComArray<SharpDX.Direct3D11.ClassInstance> classInstancesRef, int numClassInstances);

        internal abstract void GetShader(
            out T pixelShaderRef,
            SharpDX.Direct3D11.ClassInstance[] classInstancesRef,
            ref int numClassInstancesRef);
    }
}