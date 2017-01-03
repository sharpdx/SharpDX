#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CaptureRecordSink
    {
        /// <summary>	
        /// <p>Specifies a byte stream that will receive the data for the recording.</p>	
        /// </summary>	
        /// <param name="byteStreamRef"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface of a byte stream. The byte stream must be writable.</p> </dd></param>	
        /// <param name="guidContainerType"><dd> <p>A <see cref="System.Guid"/> that specifies the file container type. Possible values are documented in the <see cref="SharpDX.MediaFoundation.TranscodeAttributeKeys.TranscodeContainertype"/> attribute.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>Calling this method overrides any previous call to <strong><see cref="SharpDX.MediaFoundation.CaptureRecordSink.SetOutputFileName"/></strong> or <strong><see cref="SharpDX.MediaFoundation.CaptureRecordSink.SetSampleCallback_"/></strong>.</p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCaptureRecordSink::SetOutputByteStream']/*"/>	
        /// <msdn-id>hh447878</msdn-id>	
        /// <unmanaged>HRESULT IMFCaptureRecordSink::SetOutputByteStream([In] IMFByteStream* pByteStream,[In] const GUID&amp; guidContainerType)</unmanaged>	
        /// <unmanaged-short>IMFCaptureRecordSink::SetOutputByteStream</unmanaged-short>
        public void SetOutputByteStream(ByteStream stream, Guid containerType)
        {
            SetOutputByteStream_(stream.NativePointer, containerType);
        }

        /// <summary>	
        /// <p>Sets a callback to receive the recording data for one stream.</p>	
        /// </summary>	
        /// <param name="dwStreamSinkIndex"><dd> <p>The zero-based index of the stream. The index is returned in the <em>pdwSinkStreamIndex</em> parameter of the <strong><see cref="SharpDX.MediaFoundation.CaptureSink.AddStream"/></strong> method.</p> </dd></param>	
        /// <param name="callbackRef"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.CaptureEngineOnSampleCallback"/></strong> interface. The caller must implement this interface.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>Calling this method overrides any previous call to <strong><see cref="SharpDX.MediaFoundation.CaptureRecordSink.SetOutputByteStream_"/></strong> or  <strong><see cref="SharpDX.MediaFoundation.CaptureRecordSink.SetOutputFileName"/></strong>.</p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCaptureRecordSink::SetSampleCallback']/*"/>	
        /// <msdn-id>hh447881</msdn-id>	
        /// <unmanaged>HRESULT IMFCaptureRecordSink::SetSampleCallback([In] unsigned int dwStreamSinkIndex,[In] IMFCaptureEngineOnSampleCallback* pCallback)</unmanaged>	
        /// <unmanaged-short>IMFCaptureRecordSink::SetSampleCallback</unmanaged-short>	
        public void SetSampleCallback(int streamSinkIndex, CaptureEngineOnSampleCallback callback)
        {
            SetSampleCallback_(streamSinkIndex, CaptureEngineOnSampleCallbackShadow.ToIntPtr(callback));
        }
    }
}

#endif