using System;

namespace SharpDX.Direct3D11
{
    public partial class ResourceView
    {


        /// <summary>	
        /// <p>Get the resource that is accessed through this view.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>This function increments the reference count of the resource by one, so it is necessary to call <strong>Release</strong> on the returned reference when the application is done with it. Destroying (or losing) the returned reference before <strong>Release</strong> is called will result in a memory leak.</p>	
        /// </remarks>	
        /// <msdn-id>ff476643</msdn-id>	
        /// <unmanaged>GetResource</unmanaged>	
        /// <unmanaged-short>GetResource</unmanaged-short>	
        /// <unmanaged>void ID3D11View::GetResource([Out] ID3D11Resource** ppResource)</unmanaged>
        public SharpDX.Direct3D11.Resource Resource
        {
            get
            {
                IntPtr __output__; 
                GetResource(out __output__); 
                return new Resource(__output__);
            }
        }

    }
}