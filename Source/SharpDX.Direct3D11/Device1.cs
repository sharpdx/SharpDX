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
    public partial class Device1
    {
        /// <summary>	
        /// Creates a context state object that holds all Microsoft Direct3D state and some Direct3D behavior.
        /// </summary>	
        /// <typeparam name="T">The type of the emulated interface. This value specifies the behavior of the device when the context state object is active. Valid values are  <see cref="SharpDX.Direct3D10.Device"/>, <see cref="SharpDX.Direct3D10.Device1"/>, <see cref="SharpDX.Direct3D11.Device"/>, and <see cref="SharpDX.Direct3D11.Device1"/>. See Remarks.</typeparam>	
        /// <param name="flags"><para>A combination of <see cref="SharpDX.Direct3D11.CreateDeviceContextStateFlags"/> values that are combined by using a bitwise OR operation. The resulting value specifies how to create the context state object. The <see cref="SharpDX.Direct3D11.CreateDeviceContextStateFlags.Singlethreaded"/> flag is currently the only defined flag. If the original device was created with <see cref="SharpDX.Direct3D11.DeviceCreationFlags.SingleThreaded"/>, you must create all context state objects from that device with the <see cref="SharpDX.Direct3D11.CreateDeviceContextStateFlags.Singlethreaded"/> flag. </para> <para>The context state object that  CreateDeviceContextState creates  inherits the threading model of its associated device context. By default the context state object is rent-threaded, so that an application synchronizes access to the device context, typically by use of critical sections. In contrast, the context state object is free-threaded if you used the ID3D10Multithread interface to turn on thread protection for the device context.</para> <para>If you set the single-threaded flag for both the context state object and the device, you guarantee that you will   call the whole set of context methods and device methods only from one thread. You therefore do not need to use critical sections to synchronize access to the device context, and the runtime can avoid working with those processor-intensive critical sections.</para></param>	
        /// <param name="featureLevelsRef"><para>A reference to an array of <see cref="SharpDX.Direct3D.FeatureLevel"/> values. The array determines the order of feature levels for which creation is attempted.  To get the greatest feature level available, set pFeatureLevels to <c>null</c>,  so that CreateDeviceContextState uses the following array of feature level values:</para>  <code> { <see cref="SharpDX.Direct3D.FeatureLevel.Level_11_1"/>, <see cref="SharpDX.Direct3D.FeatureLevel.Level_11_0"/>, <see cref="SharpDX.Direct3D.FeatureLevel.Level_10_1"/>, <see cref="SharpDX.Direct3D.FeatureLevel.Level_10_0"/>, <see cref="SharpDX.Direct3D.FeatureLevel.Level_9_3"/>, <see cref="SharpDX.Direct3D.FeatureLevel.Level_9_2"/>, <see cref="SharpDX.Direct3D.FeatureLevel.Level_9_1"/>,}; </code></param>	
        /// <param name="chosenFeatureLevelRef"><para>A reference to a variable that receives a <see cref="SharpDX.Direct3D.FeatureLevel"/> value from the pFeatureLevels array. This is the first array value with which CreateDeviceContextState succeeded in creating the context state object. If the call to CreateDeviceContextState fails, the variable pointed to by pChosenFeatureLevel is set to zero.</para></param>	
        /// <returns>A <see cref="SharpDX.Direct3D11.DeviceContextState"/> object that represents the state of a Direct3D device.</returns>	
        /// <remarks>	
        /// The  REFIID value of the emulated interface is a <see cref="System.Guid"/> obtained by use of the __uuidof operator. For example, __uuidof(<see cref="SharpDX.Direct3D11.Device"/>) gets the <see cref="System.Guid"/> of the interface to a Microsoft Direct3D?11 device.Call the <see cref="SharpDX.Direct3D11.DeviceContext1.SwapDeviceContextState"/> method to activate the context state object. When the context state object is active, the device behaviors that are associated with both the context state object's feature level and its compatible interface are activated on the Direct3D device until the next call to SwapDeviceContextState.When a context state object is active, the runtime disables certain methods on the device and context interfaces. For example, a context state object that is created with __uuidof(<see cref="SharpDX.Direct3D11.Device"/>) will cause the runtime to turn off most of the Microsoft Direct3D?10 device interfaces, and a context state object that is created with __uuidof(<see cref="SharpDX.Direct3D10.Device1"/>) or __uuidof(<see cref="SharpDX.Direct3D10.Device"/>) will cause the runtime to turn off most of the <see cref="SharpDX.Direct3D11.DeviceContext"/> methods.	
        /// This behavior ensures that a user of either emulated interface cannot set device state that the other emulated interface is unable to express. This restriction helps guarantee that the <see cref="SharpDX.Direct3D10.Device1"/> emulated interface accurately reflects the full state of the pipeline and that the emulated interface will not operate contrary to its original interface definition.For example, suppose the tessellation stage is made active through the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface	
        /// when you create the device through <see cref="SharpDX.Direct3D11.D3D11.CreateDevice"/> or D3D11CreateDeviceAndSwapChain,  instead of through the Direct3D?10 equivalents. Because  the Direct3D?11 context is active, a Direct3D?10 interface is inactive when you first retrieve it via QueryInterface. This means that you cannot  immediately pass a Direct3D?10 interface that you retrieved from a Direct3D?11 device to a function. You must first call SwapDeviceContextState to activate a Direct3D?10-compatible context state object.The following table shows the methods that are active and inactive for each emulated interface.Emulated interface Active device or immediate context  interfaces Inactive device or immediate context  interfaces  <para> <see cref="SharpDX.Direct3D11.Device"/> or</para>	
        ///  <para> <see cref="SharpDX.Direct3D11.Device1"/> </para>	
        ///  <para> <see cref="SharpDX.Direct3D11.Device"/> </para>	
        ///  <para> <see cref="SharpDX.DXGI.Device"/> +</para>	
        ///  <para> <see cref="SharpDX.DXGI.Device1"/> +</para>	
        ///  <para> <see cref="SharpDX.DXGI.Device2"/> </para>	
        ///  <para> ID3D10Multithread </para>	
        ///   <see cref="SharpDX.Direct3D10.Device"/>   <para> <see cref="SharpDX.Direct3D10.Device1"/> or</para>	
        ///  <para> <see cref="SharpDX.Direct3D10.Device"/> </para>	
        ///  <para> <see cref="SharpDX.Direct3D10.Device"/> </para>	
        ///  <para> <see cref="SharpDX.Direct3D10.Device1"/> </para>	
        ///  <para> <see cref="SharpDX.DXGI.Device"/> +</para>	
        ///  <para> <see cref="SharpDX.DXGI.Device1"/> </para>	
        ///  <para> ID3D10Multithread </para>	
        ///  <para> <see cref="SharpDX.Direct3D11.Device"/> </para>	
        ///  <para> <see cref="SharpDX.Direct3D11.DeviceContext"/> (As published by the immediate context. The Direct3D?10 or Microsoft Direct3D?10.1 emulated interface has no effect on deferred contexts.)</para>?The following table shows the immediate context methods that the runtime disables when the indicated context state objects are active.Methods of <see cref="SharpDX.Direct3D11.DeviceContext"/> when __uuidof(<see cref="SharpDX.Direct3D10.Device1"/>) or __uuidof(<see cref="SharpDX.Direct3D10.Device"/>) is active Methods of <see cref="SharpDX.Direct3D10.Device"/> when __uuidof(<see cref="SharpDX.Direct3D11.Device"/>) is active    <para> Begin </para>	
        ///  <para> ClearDepthStencilView </para>	
        ///  <para> ClearDepthStencilView </para>	
        ///  <para> ClearRenderTargetView </para>	
        ///  <para> ClearRenderTargetView </para>	
        ///  <para> ClearState </para>	
        ///  <para> ClearState </para>	
        ///  <para> ClearUnorderedAccessViewUint </para>	
        ///  <para> ClearUnorderedAccessViewFloat </para>	
        ///  <para> CopyResource </para>	
        ///  <para> CopyResource </para>	
        ///  <para> CopyStructureCount </para>	
        ///  <para> CopySubresourceRegion </para>	
        ///  <para> CopySubresourceRegion </para>	
        ///  <para> CSGetConstantBuffers </para>	
        ///  <para> CSGetSamplers </para>	
        ///  <para> CSGetShader </para>	
        ///  <para> CSGetShaderResources </para>	
        ///  <para> CSGetUnorderedAccessViews </para>	
        ///  <para> CSSetConstantBuffers </para>	
        ///  <para> CSSetSamplers </para>	
        ///  <para> CSSetShader </para>	
        ///  <para> CSSetShaderResources </para>	
        ///  <para> CSSetUnorderedAccessViews </para>	
        ///  <para> Dispatch </para>	
        ///  <para> DispatchIndirect </para>	
        ///  <para> CreateBlendState </para>	
        ///  <para> Draw </para>	
        ///  <para> Draw </para>	
        ///  <para> DrawAuto </para>	
        ///  <para> DrawAuto </para>	
        ///  <para> DrawIndexed </para>	
        ///  <para> DrawIndexed </para>	
        ///  <para> DrawIndexedInstanced </para>	
        ///  <para> DrawIndexedInstanced </para>	
        ///  <para> DrawIndexedInstancedIndirect </para>	
        ///  <para> DrawInstanced </para>	
        ///  <para> DrawInstanced </para>	
        ///  <para> DrawInstancedIndirect </para>	
        ///  <para> DSGetConstantBuffers </para>	
        ///  <para> DSGetSamplers </para>	
        ///  <para> DSGetShader </para>	
        ///  <para> DSGetShaderResources </para>	
        ///  <para> DSSetConstantBuffers </para>	
        ///  <para> DSSetSamplers </para>	
        ///  <para> DSSetShader </para>	
        ///  <para> DSSetShaderResources </para>	
        ///  <para> End </para>	
        ///  <para> ExecuteCommandList </para>	
        ///  <para> FinishCommandList </para>	
        ///  <para> Flush </para>	
        ///  <para> Flush </para>	
        ///  <para> GenerateMips </para>	
        ///  <para> GenerateMips </para>	
        ///  <para> GetData </para>	
        ///  <para> GetPredication </para>	
        ///  <para> GetPredication </para>	
        ///  <para> GetResourceMinLOD </para>	
        ///  <para> GetType </para>	
        ///  <para> GetTextFilterSize </para>	
        ///  <para> GSGetConstantBuffers </para>	
        ///  <para> GSGetConstantBuffers </para>	
        ///  <para> GSGetSamplers </para>	
        ///  <para> GSGetSamplers </para>	
        ///  <para> GSGetShader </para>	
        ///  <para> GSGetShader </para>	
        ///  <para> GSGetShaderResources </para>	
        ///  <para> GSGetShaderResources </para>	
        ///  <para> GSSetConstantBuffers </para>	
        ///  <para> GSSetConstantBuffers </para>	
        ///  <para> GSSetSamplers </para>	
        ///  <para> GSSetSamplers </para>	
        ///  <para> GSSetShader </para>	
        ///  <para> GSSetShader </para>	
        ///  <para> GSSetShaderResources </para>	
        ///  <para> GSSetShaderResources </para>	
        ///  <para> HSGetConstantBuffers </para>	
        ///  <para> HSGetSamplers </para>	
        ///  <para> HSGetShader </para>	
        ///  <para> HSGetShaderResources </para>	
        ///  <para> HSSetConstantBuffers </para>	
        ///  <para> HSSetSamplers </para>	
        ///  <para> HSSetShader </para>	
        ///  <para> HSSetShaderResources </para>	
        ///  <para> IAGetIndexBuffer </para>	
        ///  <para> IAGetIndexBuffer </para>	
        ///  <para> IAGetInputLayout </para>	
        ///  <para> IAGetInputLayout </para>	
        ///  <para> IAGetPrimitiveTopology </para>	
        ///  <para> IAGetPrimitiveTopology </para>	
        ///  <para> IAGetVertexBuffers </para>	
        ///  <para> IASetIndexBuffer </para>	
        ///  <para> IASetInputLayout </para>	
        ///  <para> IASetPrimitiveTopology </para>	
        ///  <para> IASetVertexBuffers </para>	
        ///  <para> OMGetBlendState </para>	
        ///  <para> OMGetBlendState </para>	
        ///  <para> OMGetDepthStencilState </para>	
        ///  <para> OMGetDepthStencilState </para>	
        ///  <para> OMGetRenderTargets </para>	
        ///  <para> OMGetRenderTargets </para>	
        ///  <para> OMGetRenderTargetsAndUnorderedAccessViews </para>	
        ///  <para> OMSetBlendState </para>	
        ///  <para> OMSetBlendState </para>	
        ///  <para> OMSetDepthStencilState </para>	
        ///  <para> OMSetDepthStencilState </para>	
        ///  <para> OMSetRenderTargets </para>	
        ///  <para> OMSetRenderTargets </para>	
        ///  <para> OMSetRenderTargetsAndUnorderedAccessViews </para>	
        ///  <para> PSGetConstantBuffers </para>	
        ///  <para> PSGetConstantBuffers </para>	
        ///  <para> PSGetSamplers </para>	
        ///  <para> PSGetSamplers </para>	
        ///  <para> PSGetShader </para>	
        ///  <para> PSGetShader </para>	
        ///  <para> PSGetShaderResources </para>	
        ///  <para> PSGetShaderResources </para>	
        ///  <para> PSSetConstantBuffers </para>	
        ///  <para> PSSetConstantBuffers </para>	
        ///  <para> PSSetSamplers </para>	
        ///  <para> PSSetSamplers </para>	
        ///  <para> PSSetShader </para>	
        ///  <para> PSSetShader </para>	
        ///  <para> PSSetShaderResources </para>	
        ///  <para> PSSetShaderResources </para>	
        ///  <para> ResolveSubresource </para>	
        ///  <para> ResolveSubresource </para>	
        ///  <para> RSGetScissorRects </para>	
        ///  <para> RSGetScissorRects </para>	
        ///  <para> RSGetState </para>	
        ///  <para> RSGetState </para>	
        ///  <para> RSGetViewports </para>	
        ///  <para> RSGetViewports </para>	
        ///  <para> RSSetScissorRects </para>	
        ///  <para> RSSetScissorRects </para>	
        ///  <para> RSSetState </para>	
        ///  <para> RSSetState </para>	
        ///  <para> RSSetViewports </para>	
        ///  <para> RSSetViewports </para>	
        ///  <para> SetPredication </para>	
        ///  <para> SetPredication </para>	
        ///  <para> SetResourceMinLOD </para>	
        ///  <para> SetTextFilterSize </para>	
        ///  <para> SOGetTargets </para>	
        ///  <para> SOGetTargets </para>	
        ///  <para> SOSetTargets </para>	
        ///  <para> SOSetTargets </para>	
        ///  <para> UpdateSubresource </para>	
        ///  <para> UpdateSubresource </para>	
        ///  <para> VSGetConstantBuffers </para>	
        ///  <para> VSGetConstantBuffers </para>	
        ///  <para> VSGetSamplers </para>	
        ///  <para> VSGetSamplers </para>	
        ///  <para> VSGetShader </para>	
        ///  <para> VSGetShader </para>	
        ///  <para> VSGetShaderResources </para>	
        ///  <para> VSGetShaderResources </para>	
        ///  <para> VSSetConstantBuffers </para>	
        ///  <para> VSSetConstantBuffers </para>	
        ///  <para> VSSetSamplers </para>	
        ///  <para> VSSetSamplers </para>	
        ///  <para> VSSetShader </para>	
        ///  <para> VSSetShader </para>	
        ///  <para> VSSetShaderResources </para>	
        ///  <para> VSSetShaderResources </para>?The following table shows the immediate context methods that the runtime does not disable when the indicated context state objects are active.Methods of <see cref="SharpDX.Direct3D11.DeviceContext"/> when __uuidof(<see cref="SharpDX.Direct3D10.Device1"/>) or __uuidof(<see cref="SharpDX.Direct3D10.Device"/>) is active Methods of <see cref="SharpDX.Direct3D10.Device"/> when __uuidof(<see cref="SharpDX.Direct3D11.Device"/>) is active   <para> GetCreationFlags </para>	
        ///  <para> GetPrivateData </para>	
        ///  <para> GetContextFlags </para>	
        ///  <para> Map </para>	
        ///  <para> Unmap </para>?The following table shows the <see cref="SharpDX.Direct3D10.Device"/> interface methods that the runtime does not disable because they are not immediate context methods.Methods of <see cref="SharpDX.Direct3D10.Device"/>   <para> CheckCounter </para>	
        ///  <para> CheckCounterInfo </para>	
        ///  <para>Create*, like CreateQuery </para>	
        ///  <para> GetDeviceRemovedReason </para>	
        ///  <para> GetExceptionMode </para>	
        ///  <para> OpenSharedResource </para>	
        ///  <para> SetExceptionMode </para>	
        ///  <para> SetPrivateData </para>	
        ///  <para> SetPrivateDataInterface </para>?	
        /// </remarks>	
        /// <unmanaged>HRESULT ID3D11Device1::CreateDeviceContextState([In] D3D11_1_CREATE_DEVICE_CONTEXT_STATE_FLAG Flags,[In, Buffer] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In] unsigned int SDKVersion,[In] const GUID&amp; EmulatedInterface,[Out, Optional] D3D_FEATURE_LEVEL* pChosenFeatureLevel,[Out, Fast] ID3DDeviceContextState** ppContextState)</unmanaged>	
        public SharpDX.Direct3D11.DeviceContextState CreateDeviceContextState<T>(SharpDX.Direct3D11.CreateDeviceContextStateFlags flags, SharpDX.Direct3D.FeatureLevel[] featureLevelsRef, out SharpDX.Direct3D.FeatureLevel chosenFeatureLevelRef) where T : ComObject
        {
            var deviceContextState = new SharpDX.Direct3D11.DeviceContextState(IntPtr.Zero);
            CreateDeviceContextState(flags, featureLevelsRef, featureLevelsRef.Length, D3D11.SdkVersion, Utilities.GetGuidFromType(typeof(T)), out chosenFeatureLevelRef, deviceContextState);
            return deviceContextState;
        }

        /// <summary>	
        /// Gives a device access to a shared resource that is referenced by name and that was created on a different device. You must have previously created the resource as shared and specified that it uses NT handles (that is, you set the <see cref="ResourceOptionFlags.SharedNthandle"/> flag).
        /// </summary>	
        /// <typeparam name="T">Type of the resource</typeparam>
        /// <param name="resourceHandle">The resource handle.</param>	
        /// <returns>An instance of T</returns>	
        /// <remarks>	
        /// The behavior of OpenSharedResource1 is similar to the behavior of the <see cref="SharpDX.Direct3D11.Device.OpenSharedResource"/> method; each call to OpenSharedResource1 to access a resource creates a new resource object.  In other words, if you call OpenSharedResource1 twice and pass the same resource handle to hResource, you receive two resource  objects with different <see cref="SharpDX.ComObject"/> references.To share a resource between two devicesCreate the resource as shared and specify that it uses NT handles, by setting the <see cref="SharpDX.Direct3D11.ResourceOptionFlags.SharedNthandle"/> flag. Obtain the REFIID, or <see cref="System.Guid"/>, of the interface to the resource by using the __uuidof() macro. For example, __uuidof(<see cref="SharpDX.Direct3D11.Texture2D"/>) retrieves the <see cref="System.Guid"/> of the interface to a 2D texture. Query the resource for the <see cref="SharpDX.DXGI.Resource1"/> interface. Call the <see cref="SharpDX.DXGI.Resource1.CreateSharedHandle"/> method to obtain the unique handle to the resource.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID3D11Device1::OpenSharedResource1([In] void* hResource,[In] const GUID&amp; returnedInterface,[Out] void** ppResource)</unmanaged>	
        public T OpenSharedResource1<T>(IntPtr resourceHandle) where T : ComObject
        {
            IntPtr temp;
            OpenSharedResource1(resourceHandle, Utilities.GetGuidFromType(typeof(T)), out temp);
            return FromPointer<T>(temp);
        }

        /// <summary>	
        /// Gives a device access to a shared resource that is referenced by name and that was created on a different device. You must have previously created the resource as shared and specified that it uses NT handles (that is, you set the <see cref="ResourceOptionFlags.SharedNthandle"/> flag).
        /// </summary>	
        /// <typeparam name="T">Type of the resource</typeparam>
        /// <param name="name">Name of the resource to open for sharing.</param>	
        /// <param name="desiredAccess">The requested access rights to the resource.</param>
        /// <returns>An instance of T.</returns>	
        /// <remarks>	
        /// The behavior of OpenSharedResourceByName is similar to the behavior of the <see cref="SharpDX.Direct3D11.Device1.OpenSharedResource1"/> method; each call to OpenSharedResourceByName to access a resource creates a new resource object.  In other words, if you call OpenSharedResourceByName twice and pass the same resource name to lpName, you receive two resource  objects with different <see cref="SharpDX.ComObject"/> references.To share a resource between two devicesCreate the resource as shared and specify that it uses NT handles, by setting the <see cref="SharpDX.Direct3D11.ResourceOptionFlags.SharedNthandle"/> flag. Obtain the REFIID, or <see cref="System.Guid"/>, of the interface to the resource by using the __uuidof() macro. For example, __uuidof(<see cref="SharpDX.Direct3D11.Texture2D"/>) retrieves the <see cref="System.Guid"/> of the interface to a 2D texture. Query the resource for the <see cref="SharpDX.DXGI.Resource1"/> interface. Call the <see cref="SharpDX.DXGI.Resource1.CreateSharedHandle"/> method to obtain the unique handle to the resource. In this <see cref="SharpDX.DXGI.Resource1.CreateSharedHandle"/> call, you must pass a name for the resource if you want to subsequently call OpenSharedResourceByName to access the resource by name.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID3D11Device1::OpenSharedResourceByName([In] const wchar_t* lpName,[In] unsigned int dwDesiredAccess,[In] const GUID&amp; returnedInterface,[Out] void** ppResource)</unmanaged>	
        public T OpenSharedResource1<T>(string name, SharpDX.DXGI.SharedResourceFlags desiredAccess) where T : ComObject
        {
            IntPtr temp;
            OpenSharedResourceByName(name, desiredAccess, Utilities.GetGuidFromType(typeof(T)), out temp);
            return FromPointer<T>(temp);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ImmediateContext1__ != null)
                {
                    ImmediateContext1__.Dispose();
                    ImmediateContext1__ = null;
                }
            }
            
            base.Dispose(disposing);
        }
    }
}