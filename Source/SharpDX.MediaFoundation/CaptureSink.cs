#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CaptureSink
    {
        /// <summary>	
        /// <p>Queries the underlying Sink Writer object for an interface.</p>	
        /// </summary>	
        /// <param name="dwSinkStreamIndex">No documentation.</param>	
        /// <param name="rguidService">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCaptureSink::GetService']/*"/>	
        /// <msdn-id>hh447885</msdn-id>	
        /// <unmanaged>HRESULT IMFCaptureSink::GetService([In] unsigned int dwSinkStreamIndex,[In] const GUID&amp; rguidService,[In] const GUID&amp; riid,[Out, Optional] IUnknown** ppUnknown)</unmanaged>	
        /// <unmanaged-short>IMFCaptureSink::GetService</unmanaged-short>	
        T GetService<T>(int sinkStreamIndex, Guid serviceGuid)
            where T : ComObject
        {
            return (T)GetService(sinkStreamIndex, serviceGuid, Utilities.GetGuidFromType(typeof(T)));
        }
    }
}

#endif