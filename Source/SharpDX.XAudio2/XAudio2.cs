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

namespace SharpDX.XAudio2
{
    public partial class XAudio2
    {
        private EngineCallbackImpl engineCallbackImpl;
        private IntPtr engineShadowPtr;

        ///// <summary>Constant None.</summary>
        //internal static Guid CLSID_XAudio2 = new Guid("5a508685-a254-4fba-9b82-9a24b00306af");
        
        ///// <summary>Constant None.</summary>
        //internal static Guid CLSID_XAudio2_Debug = new Guid("db05ea35-0329-4d4b-a53a-6dead03d3852");

        ///// <summary>Constant None.</summary>
        //internal static Guid IID_IXAudio2 = new Guid("8bcf1f58-9fe7-4583-8ac6-e2adc465c8bb");

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

        /// <summary>
        /// Initializes a new instance of the <see cref="XAudio2"/> class.
        /// </summary>
        public XAudio2()
            : this(XAudio2Flags.None, ProcessorSpecifier.DefaultProcessor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAudio2"/> class.
        /// </summary>
        /// <param name="flags">Specify a Debug or Normal XAudio2 instance.</param>
        /// <param name="processorSpecifier">The processor specifier.</param>
        public XAudio2(XAudio2Flags flags, ProcessorSpecifier processorSpecifier)
            : base(IntPtr.Zero)
        {
#if !DIRECTX11_1
            Guid clsid = (flags == XAudio2Flags.DebugEngine) ? CLSID_XAudio2_Debug : CLSID_XAudio2;

            // Initialize for multithreaded
            //var result = Utilities.CoInitializeEx(IntPtr.Zero, Utilities.CoInit.MultiThreaded);
            //result.CheckError();

            Utilities.CreateComInstance(clsid, Utilities.CLSCTX.ClsctxInprocServer, Utilities.GetGuidFromType(typeof(XAudio2)), this);

            // Initialize XAudio2
            Initialize(0, processorSpecifier);
#else

            XAudio2Functions.XAudio2Create(this, 0, (int)processorSpecifier);

#endif
            // Register engine callback

            engineCallbackImpl = new EngineCallbackImpl(this);
            engineShadowPtr = EngineShadow.ToIntPtr(engineCallbackImpl);
            RegisterForCallbacks_(engineShadowPtr);
        }

#if !DIRECTX11_1
        /// <summary>	
        /// Returns information about an audio output device.	
        /// </summary>	
        /// <param name="index">[in]  Index of the device to be queried. This value must be less than the count returned by <see cref="SharpDX.XAudio2.XAudio2.GetDeviceCount"/>. </param>
        /// <returns>On success, pointer to an <see cref="SharpDX.XAudio2.DeviceDetails"/> structure that is returned. </returns>
        /// <unmanaged>HRESULT IXAudio2::GetDeviceDetails([None] UINT32 Index,[Out] XAUDIO2_DEVICE_DETAILS* pDeviceDetails)</unmanaged>
        public SharpDX.XAudio2.DeviceDetails GetDeviceDetails(int index)
        {
            DeviceDetails details;
            GetDeviceDetails(index, out details);
            return details;
        }
#endif

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
            if (engineShadowPtr != IntPtr.Zero)
                UnregisterForCallbacks_(engineShadowPtr);

            if (disposing)
            {
                if (engineCallbackImpl != null)
                    engineCallbackImpl.Dispose();
            }
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