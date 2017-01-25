#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CapturePreviewSink
    {
        /// <summary>	
        /// <p>Sets a callback to receive the preview data for one stream.</p>	
        /// </summary>	
        /// <param name="dwStreamSinkIndex"><dd> <p>The zero-based index of the stream. The index is returned in the <em>pdwSinkStreamIndex</em> parameter of the <strong><see cref="SharpDX.MediaFoundation.CaptureSink.AddStream"/></strong> method.</p> </dd></param>	
        /// <param name="callbackRef"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.CaptureEngineOnSampleCallback"/></strong> interface. The caller must implement this interface.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>Calling this method overrides any previous call to <strong><see cref="SharpDX.MediaFoundation.CapturePreviewSink.SetRenderHandle"/></strong>.</p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCapturePreviewSink::SetSampleCallback']/*"/>	
        /// <msdn-id>hh447873</msdn-id>	
        /// <unmanaged>HRESULT IMFCapturePreviewSink::SetSampleCallback([In] unsigned int dwStreamSinkIndex,[In] IMFCaptureEngineOnSampleCallback* pCallback)</unmanaged>	
        /// <unmanaged-short>IMFCapturePreviewSink::SetSampleCallback</unmanaged-short>	
        public void SetSampleCallback(int streamSinkIndex, CaptureEngineOnSampleCallback callback)
        {
            SetSampleCallback_(streamSinkIndex, CaptureEngineOnSampleCallbackShadow.ToIntPtr(callback));
        }
    }
}

#endif