#if DESKTOP_APP
using System;
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    internal class CaptureEngineOnEventCallbackShadow : SharpDX.ComObjectShadow
    {
        private static readonly CaptureEngineOnEventCallbackVtbl Vtbl = new CaptureEngineOnEventCallbackVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(CaptureEngineOnEventCallback callback)
        {
            return ToCallbackPtr<CaptureEngineOnEventCallback>(callback);
        }

        public class CaptureEngineOnEventCallbackVtbl : ComObjectVtbl
        {
            public CaptureEngineOnEventCallbackVtbl() : base(1)
            {
                AddMethod(new OnEventCallback(OnEventImpl));
            }

            /// <unmanaged>HRESULT IMFCaptureEngineOnEventCallback::OnEvent([In] IMFMediaEvent* pEvent)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int OnEventCallback(IntPtr thisPtr, IntPtr mediaEvent);
            private static int OnEventImpl(IntPtr thisPtr, IntPtr mediaEvent)
            {
                try
                {
                    var shadow = ToShadow<MediaEngineNotifyShadow>(thisPtr);
                    var callback = (CaptureEngineOnEventCallback)shadow.Callback;
                    var eventRef = new MediaEvent(mediaEvent);
                    callback.OnEvent(eventRef);
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
            get { return Vtbl; }
        }
    }
}
#endif