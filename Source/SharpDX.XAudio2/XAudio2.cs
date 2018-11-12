// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Runtime.InteropServices;

namespace SharpDX.XAudio2
{
    public partial class XAudio2
    {
        private EngineCallbackImpl engineCallbackImpl;
        private IntPtr engineShadowPtr;

        ///// <summary>Constant None.</summary>
        private static Guid CLSID_XAudio27 = new Guid("5a508685-a254-4fba-9b82-9a24b00306af");
        
        ///// <summary>Constant None.</summary>
        private static Guid CLSID_XAudio27_Debug = new Guid("db05ea35-0329-4d4b-a53a-6dead03d3852");

        ///// <summary>Constant None.</summary>
        private static Guid IID_IXAudio2 = new Guid("8bcf1f58-9fe7-4583-8ac6-e2adc465c8bb");

        /// <summary>	
        /// Called by XAudio2 just before an audio processing pass begins.	
        /// </summary>	
        public event EventHandler ProcessingPassStart;

        /// <summary>	
        /// Called by XAudio2 just after an audio processing pass ends.	
        /// </summary>	
        public event EventHandler ProcessingPassEnd;

        /// <summary>
        /// Called if a critical system error occurs that requires XAudio2 to be closed down and restarted.
        /// </summary>
        public event EventHandler<ErrorEventArgs> CriticalError;

#if !WINDOWS_UWP
        private const uint RPC_E_CHANGED_MODE = 0x80010106;
        private const uint COINIT_MULTITHREADED = 0x0;
        private const uint COINIT_APARTMENTTHREADED = 0x2;
        
        static XAudio2()
        {
            if (CoInitializeEx(IntPtr.Zero, COINIT_APARTMENTTHREADED) == RPC_E_CHANGED_MODE)
            {
                CoInitializeEx(IntPtr.Zero, COINIT_MULTITHREADED);
            }
        }

        [DllImport("ole32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint CoInitializeEx([In, Optional] IntPtr pvReserved, [In]uint dwCoInit);
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="XAudio2"/> class.
        /// </summary>
        public XAudio2()
            : this(XAudio2Version.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAudio2" /> class.
        /// </summary>
        /// <param name="requestedVersion">The requested version.</param>
        public XAudio2(XAudio2Version requestedVersion)
            : this(XAudio2Flags.None, ProcessorSpecifier.DefaultProcessor, requestedVersion)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAudio2" /> class.
        /// </summary>
        /// <param name="flags">Specify a Debug or Normal XAudio2 instance.</param>
        /// <param name="processorSpecifier">The processor specifier.</param>
        /// <param name="requestedVersion">The requestedVersion to use (auto, 2.7 or 2.8).</param>
        /// <exception cref="System.InvalidOperationException">XAudio2 requestedVersion [ + requestedVersion + ] is not installed</exception>
        public XAudio2(XAudio2Flags flags, ProcessorSpecifier processorSpecifier, XAudio2Version requestedVersion = XAudio2Version.Default)
            : base(IntPtr.Zero)
        {
            var tryVersions = requestedVersion == XAudio2Version.Default
                ? new[] { XAudio2Version.Version29, XAudio2Version.Version28, XAudio2Version.Version27 }
                : new[] { requestedVersion };

            foreach (var tryVersion in tryVersions)
            {
                switch (tryVersion)
                {
#if !WINDOWS_UWP
                    case XAudio2Version.Version27:
                        Guid clsid = ((int)flags == 1) ? CLSID_XAudio27_Debug : CLSID_XAudio27;
                        if ((requestedVersion == XAudio2Version.Default || requestedVersion == XAudio2Version.Version27) && Utilities.TryCreateComInstance(clsid, Utilities.CLSCTX.ClsctxInprocServer, IID_IXAudio2, this))
                        {
                            SetupVtblFor27();
                            // Initialize XAudio2
                            Initialize(0, processorSpecifier);
                            Version = XAudio2Version.Version27;
                        }
                        break;
#endif
                    case XAudio2Version.Version28:
                        try
                        {
                            XAudio28Functions.XAudio2Create(this, 0, (int)processorSpecifier);
                            Version = XAudio2Version.Version28;
                        }
                        catch (DllNotFoundException) { }
                        break;
                    case XAudio2Version.Version29:
                        try
                        {
                            XAudio29Functions.XAudio2Create(this, 0, (int)processorSpecifier);
                            Version = XAudio2Version.Version29;
                        }
                        catch (DllNotFoundException) { }
                        break;
                }

                // Early exit if we found a requestedVersion
                if (Version != XAudio2Version.Default)
                {
                    break;
                }
            }

            if (Version == XAudio2Version.Default)
            {
                throw new DllNotFoundException(string.Format("Unable to find XAudio2 dlls for requested versions [{0}], not installed on this machine", requestedVersion == XAudio2Version.Default ? "2.7, 2.8 or 2.9" : requestedVersion.ToString()));
            }

            // Register engine callback

            engineCallbackImpl = new EngineCallbackImpl(this);
            engineShadowPtr = EngineShadow.ToIntPtr(engineCallbackImpl);
            RegisterForCallbacks(engineCallbackImpl);
        }

        /// <summary>
        /// Gets the requestedVersion of this XAudio2 instance, once a <see cref="XAudio2"/> device has been instanciated.
        /// </summary>
        /// <value>The requestedVersion.</value>
        public XAudio2Version Version { get; private set; }

        // ---------------------------------------------------------------------------------
        // Start handling 2.7 requestedVersion here
        // ---------------------------------------------------------------------------------

        /// <summary>
        /// Setups the VTBL for XAudio 2.7. The 2.7 verions had 3 methods starting at VTBL[3]:
        /// - GetDeviceCount
        /// - GetDeviceDetails
        /// - Initialize
        /// </summary>
        private void SetupVtblFor27()
        {
            RegisterForCallbacks__vtbl_index += 3;
            UnregisterForCallbacks__vtbl_index += 3;
            CreateSourceVoice__vtbl_index += 3;
            CreateSubmixVoice__vtbl_index += 3;
            CreateMasteringVoice__vtbl_index += 3;
            StartEngine__vtbl_index += 3;
            StopEngine__vtbl_index += 3;
            CommitChanges__vtbl_index += 3;
            GetPerformanceData__vtbl_index += 3;
            SetDebugConfiguration__vtbl_index += 3;
        }

        internal void CheckVersion27()
        {
            if (Version != XAudio2Version.Version27)
            {
                throw new InvalidOperationException("This method is only valid on the XAudio 2.7 requestedVersion [Current is: " + Version + "]");
            }
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='IXAudio2::GetDeviceCount']/*" />	
        /// <unmanaged>GetDeviceCount</unmanaged>	
        /// <unmanaged-short>GetDeviceCount</unmanaged-short>	
        /// <unmanaged>HRESULT IXAudio2::GetDeviceCount([Out] unsigned int* pCount)</unmanaged>
        public int DeviceCount
        {
            get
            {
                CheckVersion27();
                int result;
                this.GetDeviceCount(out result);
                return result;
            }
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="countRef">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <!-- No matching elements were found for the following include tag --><include file="Documentation\CodeComments.xml" path="/comments/comment[@id='IXAudio2::GetDeviceCount']/*" />	
        /// <unmanaged>HRESULT IXAudio2::GetDeviceCount([Out] unsigned int* pCount)</unmanaged>	
        /// <unmanaged-short>IXAudio2::GetDeviceCount</unmanaged-short>	
        private unsafe void GetDeviceCount(out int countRef)
        {
	        Result result;
	        fixed (void* ptr = &countRef)
	        {
		        result = LocalInterop.CalliGetDeviceCount(this._nativePointer, ptr, *(*(void***)this._nativePointer + 3));
	        }
	        result.CheckError();
        }

        internal unsafe void CreateMasteringVoice27(MasteringVoice masteringVoiceOut, int inputChannels, int inputSampleRate, int flags, int deviceIndex, EffectChain? effectChainRef)
        {
	        IntPtr zero = IntPtr.Zero;
	        EffectChain value;
	        if (effectChainRef.HasValue)
	        {
		        value = effectChainRef.Value;
	        }
	        Result result = LocalInterop.CalliCreateMasteringVoice(this._nativePointer, (void*)&zero, inputChannels, inputSampleRate, flags, deviceIndex, effectChainRef.HasValue ? ((void*)(&value)) : ((void*)IntPtr.Zero), *(*(void***)this._nativePointer + 10));
	        masteringVoiceOut.NativePointer = zero;
	        result.CheckError();
        }

        /// <summary>	
        /// Returns information about an audio output device.	
        /// </summary>	
        /// <param name="index">[in]  Index of the device to be queried. This value must be less than the count returned by <see cref="SharpDX.XAudio2.XAudio2.GetDeviceCount"/>. </param>
        /// <returns>On success, pointer to an <see cref="SharpDX.XAudio2.DeviceDetails"/> structure that is returned. </returns>
        /// <unmanaged>HRESULT IXAudio2::GetDeviceDetails([None] UINT32 Index,[Out] XAUDIO2_DEVICE_DETAILS* pDeviceDetails)</unmanaged>
        public SharpDX.XAudio2.DeviceDetails GetDeviceDetails(int index)
        {
            CheckVersion27();

            DeviceDetails details;
            GetDeviceDetails(index, out details);
            return details;
        }

        private unsafe void GetDeviceDetails(int index, out DeviceDetails deviceDetailsRef)
        {
	        DeviceDetails.__Native _Native = default(DeviceDetails.__Native);
	        Result result = LocalInterop.CalliGetDeviceDetails(this._nativePointer, index, &_Native, *(*(void***)this._nativePointer + 4));
	        deviceDetailsRef = default(DeviceDetails);
	        deviceDetailsRef.__MarshalFrom(ref _Native);
	        result.CheckError();
        }

        private unsafe void Initialize(int flags, ProcessorSpecifier xAudio2Processor)
        {
	        var result = (Result)LocalInterop.CalliInitialize(this._nativePointer, (int)flags, (int)xAudio2Processor, *(*(void***)this._nativePointer + 5));
            result.CheckError();
        }

        // ---------------------------------------------------------------------------------
        // End handling 2.7 requestedVersion here
        // ---------------------------------------------------------------------------------

        /// <summary>
        /// Calculate a decibel from a volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <returns>a dB value</returns>
        public static float AmplitudeRatioToDecibels(float volume)
        {
            if (volume == 0f)
                return float.MinValue;
            return (float)(Math.Log10(volume) * 20);
        }

        /// <summary>
        /// Calculate radians from a cutoffs frequency.
        /// </summary>
        /// <param name="cutoffFrequency">The cutoff frequency.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <returns>radian</returns>
        public static float CutoffFrequencyToRadians(float cutoffFrequency, int sampleRate)
        {
            if (((int)cutoffFrequency * 6.0) >= sampleRate)
                return 1f;
            return (float)(Math.Sin(cutoffFrequency*Math.PI/sampleRate)*2);
        }

        /// <summary>
        /// Calculate a cutoff frequency from a radian.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <returns>cutoff frequency</returns>
        public static float RadiansToCutoffFrequency(float radians, float sampleRate)
        {
            return (float)((Math.Asin(radians * 0.5) * sampleRate) / Math.PI);
        }

        /// <summary>
        /// Calculate a volume from a decibel
        /// </summary>
        /// <param name="decibels">a dB value</param>
        /// <returns>an amplitude value</returns>
        public static float DecibelsToAmplitudeRatio(float decibels)
        {
            return (float)Math.Pow(10, decibels / 20);
        }


        /// <summary>
        /// Calculate semitones from a Frequency ratio
        /// </summary>
        /// <param name="frequencyRatio">The frequency ratio.</param>
        /// <returns>semitones</returns>
        public static float FrequencyRatioToSemitones(float frequencyRatio)
        {
            return (float)(Math.Log10(frequencyRatio) * 12 * Math.PI);
        }

        /// <summary>
        /// Calculate frequency from semitones.
        /// </summary>
        /// <param name="semitones">The semitones.</param>
        /// <returns>the frequency</returns>
        public static float SemitonesToFrequencyRatio(float semitones)
        {
            return (float)Math.Pow(2, semitones / 12);
        }

        /// <summary>	
        /// Atomically applies a set of operations for all pending operations.
        /// </summary>	
        /// <unmanaged>HRESULT IXAudio2::CommitChanges([None] UINT32 OperationSet)</unmanaged>
        public void CommitChanges()
        {
            this.CommitChanges(0);
        }

        protected override void Dispose(bool disposing)
        {
            if (engineCallbackImpl != null)
                UnregisterForCallbacks(engineCallbackImpl);

            if (disposing)
            {
                if (engineCallbackImpl != null)
                    engineCallbackImpl.Dispose();
            }

            Version = XAudio2Version.Default;

            base.Dispose(disposing);
        }

        private class EngineCallbackImpl : CallbackBase, EngineCallback
        {
            XAudio2 XAudio2 { get; set; }

            public EngineCallbackImpl(XAudio2 xAudio2)
            {
                XAudio2 = xAudio2;
            }

            public void OnProcessingPassStart()
            {
                EventHandler handler = XAudio2.ProcessingPassStart;
                if (handler != null) handler(this, EventArgs.Empty);
            }

            public void OnProcessingPassEnd()
            {
                EventHandler handler = XAudio2.ProcessingPassEnd;
                if (handler != null) handler(this, EventArgs.Empty);
            }

            public void OnCriticalError(Result error)
            {
                EventHandler<ErrorEventArgs> handler = XAudio2.CriticalError;
                if (handler != null) handler(this, new ErrorEventArgs(error));
            }

            IDisposable ICallbackable.Shadow { get; set; }
        }
    }
}