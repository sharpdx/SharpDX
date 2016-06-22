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

namespace SharpDX.Direct3D10
{
    /// <summary>
    /// Common Shader class. Provides a common set of methods for a Shader Stage.
    /// TODO: check if usage of abstract is not introducing an unacceptable overhead...
    /// </summary>
    /// <typeparam name = "T">Type of the shader</typeparam>
    public abstract class CommonShaderStage<T> : CppObject
        where T : ComObject
    {

        /// <summary>
        /// Maximum number of bindable constant buffers to a pipeline stage.
        /// </summary>
        public const int MaximumConstantBufferSlotCount = 14;

        /// <summary>
        /// Maximum number of bindable resources to a pipeline stage.
        /// </summary>
        public const int MaximumResourceSlotCount = 128;

        /// <summary>
        /// Maximum number of bindable samplers to a pipeline stage.
        /// </summary>
        public const int MaximumSamplerSlotCount = 16;

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
            SetConstantBuffers(slot, 1, new[] { constantBuffer });
        }

        /// <summary>
        ///   Sets a single sampler to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the sampler.</param>
        /// <param name = "sampler">sampler state to set</param>
        public void SetSampler(int slot, SamplerState sampler)
        {
            SetSamplers(slot, 1, new[] { sampler });
        }

        /// <summary>
        ///   Sets a single shader resource to be used by the shader stage.
        /// </summary>
        /// <param name = "slot">Index into the device's zero-based array to which to set the resource.</param>
        /// <param name = "resourceView">Resource view to attach</param>
        public void SetShaderResource(int slot, ShaderResourceView resourceView)
        {
            SetShaderResources(slot, 1, new[] { resourceView });
        }


        /// <summary>
        ///   Get the shader resources.
        /// </summary>
        /// <remarks>
        ///   Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin getting shader resources from (ranges from 0 to D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "numViews">The number of resources to get from the device. Up to a maximum of 128 slots are available for shader resources (ranges from 0 to D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - StartSlot).</param>
        /// <param name = "shaderResourceViewsRef">Array of {{shader resource view}} interfaces to be returned by the device.</param>
        /// <unmanaged>void PSGetShaderResources([In] UINT StartSlot,[In] UINT NumViews,[Out, Buffer] ID3D10ShaderResourceView** ppShaderResourceViews)</unmanaged>
        internal abstract void GetShaderResources(
            int startSlot,
            int numViews,
            SharpDX.Direct3D10.ShaderResourceView[] shaderResourceViewsRef);

        /// <summary>
        ///   Get an array of sampler states from the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks.
        /// </remarks>
        /// <param name = "startSlot">Index into a zero-based array to begin getting samplers from (ranges from 0 to D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT - 1).</param>
        /// <param name = "numSamplers">Number of samplers to get from a device context. Each pipeline stage has a total of 16 sampler slots available (ranges from 0 to D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT - StartSlot).</param>
        /// <param name = "samplersRef">Array of sampler-state interface pointers (see <see cref = "SharpDX.Direct3D10.SamplerState" />) to be returned by the device.</param>
        /// <unmanaged>void PSGetSamplers([In] UINT StartSlot,[In] UINT NumSamplers,[Out, Buffer] ID3D10SamplerState** ppSamplers)</unmanaged>
        internal abstract void GetSamplers(
            int startSlot,
            int numSamplers,
            SharpDX.Direct3D10.SamplerState[] samplersRef);

        /// <summary>
        ///   Get the constant buffers used by the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin retrieving constant buffers from (ranges from 0 to D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - 1).</param>
        /// <param name = "numBuffers">Number of buffers to retrieve (ranges from 0 to D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - StartSlot).</param>
        /// <param name = "constantBuffersRef">Array of constant buffer interface pointers (see <see cref = "SharpDX.Direct3D10.Buffer" />) to be returned by the method.</param>
        /// <unmanaged>void PSGetConstantBuffers([In] UINT StartSlot,[In] UINT NumBuffers,[Out, Buffer] ID3D10Buffer** ppConstantBuffers)</unmanaged>
        internal abstract void GetConstantBuffers(
            int startSlot,
            int numBuffers,
            SharpDX.Direct3D10.Buffer[] constantBuffersRef);

        /// <summary>
        ///   Bind an array of shader resources to the shader stage.
        /// </summary>
        /// <remarks>
        ///   If an overlapping resource view is already bound to an output slot, such as a render target, then this API will fill the destination shader resource slot with NULL.For information about creating shader-resource views, see <see cref = "SharpDX.Direct3D10.Device.CreateShaderResourceView" />. The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting shader resources to (ranges from 0 to D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "numViews">Number of shader resources to set. Up to a maximum of 128 slots are available for shader resources (ranges from 0 to D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - StartSlot).</param>
        /// <param name = "shaderResourceViewsRef">Array of {{shader resource view}} interfaces to set to the device.</param>
        /// <unmanaged>void PSSetShaderResources([In] UINT StartSlot,[In] UINT NumViews,[In, Buffer] const ID3D10ShaderResourceView** ppShaderResourceViews)</unmanaged>
        public abstract void SetShaderResources(
            int startSlot,
            int numViews,
            SharpDX.Direct3D10.ShaderResourceView[] shaderResourceViewsRef);

        /// <summary>
        ///   Bind an array of shader resources to the shader stage.
        /// </summary>
        /// <remarks>
        ///   If an overlapping resource view is already bound to an output slot, such as a render target, then this API will fill the destination shader resource slot with NULL.For information about creating shader-resource views, see <see cref = "SharpDX.Direct3D10.Device.CreateShaderResourceView" />. The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting shader resources to (ranges from 0 to D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - 1).</param>
        /// <param name = "numViews">Number of shader resources to set. Up to a maximum of 128 slots are available for shader resources (ranges from 0 to D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT - StartSlot).</param>
        /// <param name = "shaderResourceViewsRef">Array of {{shader resource view}} interfaces to set to the device.</param>
        /// <unmanaged>void PSSetShaderResources([In] UINT StartSlot,[In] UINT NumViews,[In, Buffer] const ID3D10ShaderResourceView** ppShaderResourceViews)</unmanaged>
        public abstract void SetShaderResources(
            int startSlot,
            int numViews,
            SharpDX.ComArray<SharpDX.Direct3D10.ShaderResourceView> shaderResourceViewsRef);

        /// <summary>
        ///   Set an array of sampler states to the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any sampler may be set to NULL; this invokes the default state, which is defined to be the following.StateDefault ValueFilterD3D10_FILTER_MIN_MAG_MIP_LINEARAddressUD3D10_TEXTURE_ADDRESS_CLAMPAddressVD3D10_TEXTURE_ADDRESS_CLAMPAddressWD3D10_TEXTURE_ADDRESS_CLAMPMipLODBias0MaxAnisotropy1ComparisonFuncD3D10_COMPARISON_NEVERBorderColor[0]1.0fBorderColor[1]1.0fBorderColor[2]1.0fBorderColor[3]1.0fMinLOD-FLT_MAXMaxLODFLT_MAX  The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting samplers to (ranges from 0 to D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT - 1).</param>
        /// <param name = "numSamplers">Number of samplers in the array. Each pipeline stage has a total of 16 sampler slots available (ranges from 0 to D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT - StartSlot).</param>
        /// <param name = "samplersRef">Pointer to an array of sampler-state interfaces (see <see cref = "SharpDX.Direct3D10.SamplerState" />). See Remarks.</param>
        /// <unmanaged>void PSSetSamplers([In] UINT StartSlot,[In] UINT NumSamplers,[In, Buffer] const ID3D10SamplerState** ppSamplers)</unmanaged>
        public abstract void SetSamplers(
            int startSlot,
            int numSamplers,
            SharpDX.Direct3D10.SamplerState[] samplersRef);

        /// <summary>
        ///   Set an array of sampler states to the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   Any sampler may be set to NULL; this invokes the default state, which is defined to be the following.StateDefault ValueFilterD3D10_FILTER_MIN_MAG_MIP_LINEARAddressUD3D10_TEXTURE_ADDRESS_CLAMPAddressVD3D10_TEXTURE_ADDRESS_CLAMPAddressWD3D10_TEXTURE_ADDRESS_CLAMPMipLODBias0MaxAnisotropy1ComparisonFuncD3D10_COMPARISON_NEVERBorderColor[0]1.0fBorderColor[1]1.0fBorderColor[2]1.0fBorderColor[3]1.0fMinLOD-FLT_MAXMaxLODFLT_MAX  The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting samplers to (ranges from 0 to D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT - 1).</param>
        /// <param name = "numSamplers">Number of samplers in the array. Each pipeline stage has a total of 16 sampler slots available (ranges from 0 to D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT - StartSlot).</param>
        /// <param name = "samplersRef">Pointer to an array of sampler-state interfaces (see <see cref = "SharpDX.Direct3D10.SamplerState" />). See Remarks.</param>
        /// <unmanaged>void PSSetSamplers([In] UINT StartSlot,[In] UINT NumSamplers,[In, Buffer] const ID3D10SamplerState** ppSamplers)</unmanaged>
        public abstract void SetSamplers(
            int startSlot,
            int numSamplers,
            SharpDX.ComArray<SharpDX.Direct3D10.SamplerState> samplersRef);

        /// <summary>
        ///   Set the constant buffers used by the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting constant buffers to (ranges from 0 to D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - 1).</param>
        /// <param name = "numBuffers">Number of buffers to set (ranges from 0 to D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - StartSlot).</param>
        /// <param name = "constantBuffersRef">Array of constant buffers (see <see cref = "SharpDX.Direct3D10.Buffer" />) being given to the device.</param>
        /// <unmanaged>void PSSetConstantBuffers([In] UINT StartSlot,[In] UINT NumBuffers,[In, Buffer] const ID3D10Buffer** ppConstantBuffers)</unmanaged>
        public abstract void SetConstantBuffers(
            int startSlot,
            int numBuffers,
            SharpDX.Direct3D10.Buffer[] constantBuffersRef);

        /// <summary>
        ///   Set the constant buffers used by the shader pipeline stage.
        /// </summary>
        /// <remarks>
        ///   The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.
        /// </remarks>
        /// <param name = "startSlot">Index into the device's zero-based array to begin setting constant buffers to (ranges from 0 to D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - 1).</param>
        /// <param name = "numBuffers">Number of buffers to set (ranges from 0 to D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT - StartSlot).</param>
        /// <param name = "constantBuffersRef">Array of constant buffers (see <see cref = "SharpDX.Direct3D10.Buffer" />) being given to the device.</param>
        /// <unmanaged>void PSSetConstantBuffers([In] UINT StartSlot,[In] UINT NumBuffers,[In, Buffer] const ID3D10Buffer** ppConstantBuffers)</unmanaged>
        public abstract void SetConstantBuffers(int startSlot, int numBuffers, SharpDX.ComArray<SharpDX.Direct3D10.Buffer> constantBuffersRef);
    }
}