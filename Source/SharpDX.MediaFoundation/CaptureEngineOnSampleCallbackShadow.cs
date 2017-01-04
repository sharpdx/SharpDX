#if DESKTOP_APP
using System;
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    internal class CaptureEngineOnSampleCallbackShadow : ComObjectShadow
    {
        private static readonly CaptureEngineOnSampleCallbackVtbl Vtbl = new CaptureEngineOnSampleCallbackVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(CaptureEngineOnSampleCallback callback)
        {
            return ToCallbackPtr<CaptureEngineOnSampleCallback>(callback);
        }

        public class CaptureEngineOnSampleCallbackVtbl : ComObjectVtbl
        {
            public CaptureEngineOnSampleCallbackVtbl(int numberOfMethods) : base(numberOfMethods + 1)
            {
                AddMethod(new OnSampleCallback(OnSampleImpl));
            }

            /// <unmanaged>HRESULT IMFCaptureEngineOnEventCallback::OnEvent([In] IMFMediaEvent* pEvent)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int OnSampleCallback(IntPtr thisPtr, IntPtr sample);
            private static int OnSampleImpl(IntPtr thisPtr, IntPtr sample)
            {
                try
                {
                    var shadow = ToShadow<CaptureEngineOnSampleCallbackShadow>(thisPtr);
                    var callback = (CaptureEngineOnSampleCallback)shadow.Callback;
                    var sampleRef = new Sample(sample);
                    callback.OnSample(sampleRef);
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