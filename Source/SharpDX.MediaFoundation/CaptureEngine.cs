using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CaptureEngine
    {
        /// <summary>	
        /// <p>Creates an instance of the capture engine.</p>	
        /// </summary>	
        /// <param name="clsid"><dd> <p>The CLSID of the object to create. Currently, this parameter must equal <strong><see cref="SharpDX.MediaFoundation.CaptureEngine.ClsidMFCaptureEngine"/></strong>.</p> </dd></param>	
        /// <param name="riid"><dd> <p>The IID of the requested interface. The capture engine supports the <strong><see cref="SharpDX.MediaFoundation.CaptureEngine"/></strong> interface.</p> </dd></param>	
        /// <param name="vObjectOut"><dd> <p>Receives a reference to the requested interface. The caller must release the interface.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>Before calling this method, call the <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Startup"/></strong> function.</p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCaptureEngineClassFactory::CreateInstance']/*"/>	
        /// <msdn-id>hh447848</msdn-id>	
        /// <unmanaged>HRESULT IMFCaptureEngineClassFactory::CreateInstance([In] const GUID&amp; clsid,[In] const GUID&amp; riid,[Out] void** ppvObject)</unmanaged>	
        /// <unmanaged-short>IMFCaptureEngineClassFactory::CreateInstance</unmanaged-short>	
        public CaptureEngine(CaptureEngineClassFactory factory)
        {
            IntPtr native;
            factory.CreateInstance(ClsidMFCaptureEngine, Utilities.GetGuidFromType(typeof(CaptureEngine)), out native);
            NativePointer = native;
        }
    }
}
