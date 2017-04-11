using System;

namespace SharpDX.Direct3D11
{
    public partial class Device5
    {
        /// <summary>
        /// Opens a handle for a shared fence by using resourceHandle.
        /// </summary>
        /// <param name="resourceHandle">The handle that was returned by a call to ID3D11Fence::CreateSharedHandle or ID3D12Device::CreateSharedHandle</param>
        /// <returns>Fence</returns>
        public Fence OpenSharedFence(IntPtr resourceHandle)
        {
            IntPtr temp;
            OpenSharedFence(resourceHandle, Utilities.GetGuidFromType(typeof(Fence)), out temp);
            return CppObject.FromPointer<Fence>(temp);
        }
    }
}