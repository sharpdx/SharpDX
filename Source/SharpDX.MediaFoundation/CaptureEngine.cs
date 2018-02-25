#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public delegate void CaptureEngineOnEventDelegate(MediaEvent mediaEvent);

    public partial class CaptureEngine
    {
        private CaptureEngineOnEventImpl captureEngineOnEventImpl;

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
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCaptureEngineClassFactory::CreateInstance']/*"/>	
        /// <msdn-id>hh447848</msdn-id>	
        /// <unmanaged>HRESULT IMFCaptureEngineClassFactory::CreateInstance([In] const GUID&amp; clsid,[In] const GUID&amp; riid,[Out] void** ppvObject)</unmanaged>	
        /// <unmanaged-short>IMFCaptureEngineClassFactory::CreateInstance</unmanaged-short>	
        public CaptureEngine(CaptureEngineClassFactory factory)
        {
            IntPtr native;
            factory.CreateInstance(ClsidMFCaptureEngine, Utilities.GetGuidFromType(typeof(CaptureEngine)), out native);
            NativePointer = native;

            captureEngineOnEventImpl = new CaptureEngineOnEventImpl(this);
        }

        /// <summary>	
        /// <p>Initializes the capture engine.</p>	
        /// </summary>	
        /// <param name="eventCallbackRef"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.CaptureEngineOnEventCallback"/></strong> interface. The caller must implement this interface. The capture engine uses this interface to send asynchronous events to the caller.</p> </dd></param>	
        /// <param name="attributesRef"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.MediaAttributes"/></strong> interface. This parameter can be <strong><c>null</c></strong>. </p> <p>You can use this parameter to configure the capture engine. Call <strong><see cref="SharpDX.MediaFoundation.MediaFactory.CreateAttributes"/></strong> to create an attribute store, and then set any of the following attributes.</p> <ul> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.D3DManager"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.DisableDXVA"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.DisableHardwareTransforms"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.EncoderTransformFieldOfUseUnlockAttribute"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.EventGeneratorGuid"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.EventStreamIndex"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.MediaSourceConfig"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.RecordSinkAudioMaxProcessedSamples"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.RecordSinkAudioMaxUnprocessedSamples"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.RecordSinkVideoMaxProcessedSamples"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.RecordSinkVideoMaxUnprocessedSamples"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.UseAudioDeviceOnly"/> </li> <li> <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.UseVideoDeviceOnly"/> </li> </ul> </dd></param>	
        /// <param name="audioSourceRef"><dd> <p>An <strong><see cref="SharpDX.ComObject"/></strong> reference that specifies an audio-capture device. This parameter can be <strong><c>null</c></strong>.</p> <p>If you set the <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.UseVideoDeviceOnly"/> attribute to <strong>TRUE</strong> in <em>pAttributes</em>, the capture engine does not use an audio device, and the <em>pAudioSource</em> parameter is ignored.</p> <p>Otherwise, if <em>pAudioSource</em> is <strong><c>null</c></strong>, the capture engine selects the microphone that is built into the video camera specified by <em>pVideoSource</em>. If the video camera does not have a microphone, the capture engine enumerates the audio-capture devices on the system and selects the first one.</p> <p>To override the default audio device, set <em>pAudioSource</em> to an <strong><see cref="SharpDX.MediaFoundation.MediaSource"/></strong> or <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> reference for the device. For more information, see Audio/Video Capture in Media Foundation.</p> </dd></param>	
        /// <param name="videoSourceRef"><dd> <p>An <strong><see cref="SharpDX.ComObject"/></strong> reference that specifies a video-capture device. This parameter can be <strong><c>null</c></strong>.</p> <p>If you set the <see cref="SharpDX.MediaFoundation.CaptureEngineAttributeKeys.UseAudioDeviceOnly"/> attribute to <strong>TRUE</strong> in <em>pAttributes</em>, the capture engine does not use a video device, and the <em>pVideoSource</em> parameter is ignored.</p> <p>Otherwise, if <em>pVideoSource</em> is <strong><c>null</c></strong>, the capture engine enumerates the video-capture devices on the system and selects the first one.</p> <p>To override the default video device, set <em>pVideoSource</em> to an <strong><see cref="SharpDX.MediaFoundation.MediaSource"/></strong> or <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> reference for the device. For more information, see Enumerating Video Capture Devices.</p> </dd></param>	
        /// <returns><p>This method can return one of these values.</p><table> <tr><th>Return code</th><th>Description</th></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.Result.Ok"/></strong></dt> </dl> </td><td> <p>Success.</p> </td></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.MediaFoundation.ResultCode.InvalidRequest"/></strong></dt> </dl> </td><td> <p>The <strong>Initialize</strong> method was already called.</p> </td></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.MediaFoundation.ResultCode.NoCaptureDevicesAvailable"/></strong></dt> </dl> </td><td> <p>No capture devices are available.</p> </td></tr> </table><p> </p></returns>	
        /// <remarks>	
        /// <p>You must call this method once before using the capture engine. Calling the method a second time returns <strong><see cref="SharpDX.MediaFoundation.ResultCode.InvalidRequest"/></strong>.</p><p>This method is asynchronous. If the method returns a success code, the caller will receive an <strong>MF_CAPTURE_ENGINE_INITIALIZED</strong> event through the <strong><see cref="SharpDX.MediaFoundation.CaptureEngineOnEventCallback.OnEvent"/></strong> method. The operation can fail asynchronously after the method succeeds. If so, the error code is conveyed through the <strong>OnEvent</strong> method.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCaptureEngine::Initialize']/*"/>	
        /// <msdn-id>hh447855</msdn-id>	
        /// <unmanaged>HRESULT IMFCaptureEngine::Initialize([In] IMFCaptureEngineOnEventCallback* pEventCallback,[In, Optional] IMFAttributes* pAttributes,[In, Optional] IUnknown* pAudioSource,[In, Optional] IUnknown* pVideoSource)</unmanaged>	
        /// <unmanaged-short>IMFCaptureEngine::Initialize</unmanaged-short>	
        public void Initialize(MediaAttributes attributesRef, ComObject audioSourceRef, ComObject videoSourceRef)
        {
            Initialize(captureEngineOnEventImpl, attributesRef, audioSourceRef, videoSourceRef);
        }

        public event CaptureEngineOnEventDelegate CaptureEngineEvent;

        private void OnEvent(MediaEvent mediaEvent)
        {
            CaptureEngineEvent?.Invoke(mediaEvent);
        }

        private class CaptureEngineOnEventImpl : CallbackBase, CaptureEngineOnEventCallback
        {
            private CaptureEngine captureEngine;

            public CaptureEngineOnEventImpl(CaptureEngine captureEngine)
            {
                this.captureEngine = captureEngine;
            }

            public void OnEvent(MediaEvent mediaEvent)
            {
                captureEngine.OnEvent(mediaEvent);
            }
        }
    }
}

#endif