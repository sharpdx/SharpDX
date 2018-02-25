#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CapturePhotoSink
    {
        /// <summary>	
        /// <p>Specifies a byte stream that will receive the still image data.</p>	
        /// </summary>	
        /// <param name="byteStreamRef"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface of a byte stream. The byte stream must be writable.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>Calling this method overrides any previous call to <strong><see cref="SharpDX.MediaFoundation.CapturePhotoSink.SetOutputFileName"/></strong> or <strong><see cref="SharpDX.MediaFoundation.CapturePhotoSink.SetSampleCallback_"/></strong>.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCapturePhotoSink::SetOutputByteStream']/*"/>	
        /// <msdn-id>hh447862</msdn-id>	
        /// <unmanaged>HRESULT IMFCapturePhotoSink::SetOutputByteStream([In] IMFByteStream* pByteStream)</unmanaged>	
        /// <unmanaged-short>IMFCapturePhotoSink::SetOutputByteStream</unmanaged-short>
        public ByteStream OutputByteStream
        {
            set
            {
                SetOutputByteStream(value);
            }
        }

        /// <summary>	
        /// <p>Sets a callback to receive the still-image data.</p>	
        /// </summary>	
        /// <param name="callbackRef"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.CaptureEngineOnSampleCallback"/></strong> interface. The caller must implement this interface.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>Calling this method overrides any previous call to <strong><see cref="SharpDX.MediaFoundation.CapturePhotoSink.SetOutputByteStream_"/></strong> or  <strong><see cref="SharpDX.MediaFoundation.CapturePhotoSink.SetOutputFileName"/></strong>.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCapturePhotoSink::SetSampleCallback']/*"/>	
        /// <msdn-id>hh447864</msdn-id>	
        /// <unmanaged>HRESULT IMFCapturePhotoSink::SetSampleCallback([In] IMFCaptureEngineOnSampleCallback* pCallback)</unmanaged>	
        /// <unmanaged-short>IMFCapturePhotoSink::SetSampleCallback</unmanaged-short>	
        public CaptureEngineOnSampleCallback SampleCallback
        {
            set
            {
                SetSampleCallback(value);
            }
        }
    }
}
#endif
