#if DESKTOP_APP
using System;
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    internal class CaptureEngineOnSampleCallback2Shadow : ComObjectShadow
    {
        private static readonly CaptureEngineOnSampleCallback2Vtbl Vtbl = new CaptureEngineOnSampleCallback2Vtbl();

        public class CaptureEngineOnSampleCallback2Vtbl : CaptureEngineOnSampleCallbackShadow.CaptureEngineOnSampleCallbackVtbl
        {
            public CaptureEngineOnSampleCallback2Vtbl()
                :base(1)
            {
                AddMethod(new OnSynchronizedEventCallback(OnSynchronizedEventImpl));
            }

            /// <unmanaged>HRESULT IMFCaptureEngineOnEventCallback::OnEvent([In] IMFMediaEvent* pEvent)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int OnSynchronizedEventCallback(IntPtr thisPtr, IntPtr mediaEvent);
            private static int OnSynchronizedEventImpl(IntPtr thisPtr, IntPtr mediaEvent)
            {
                try
                {
                    var shadow = ToShadow<CaptureEngineOnSampleCallback2Shadow>(thisPtr);
                    var callback = (CaptureEngineOnSampleCallback2)shadow.Callback;
                    var eventRef = new MediaEvent(mediaEvent);
                    callback.OnSynchronizedEvent(eventRef);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }


        protected override CppObjectVtbl GetVtbl
        {
            get
            {
                return Vtbl;
            }
        }

    }
}
#endif