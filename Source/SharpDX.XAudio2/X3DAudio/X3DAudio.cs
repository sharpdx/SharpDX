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

using SharpDX.Multimedia;

namespace SharpDX.X3DAudio
{
    public partial class X3DAudio
    {
        private X3DAudioHandle handle;
        private readonly X3DAudioVersion version;
        private readonly X3DAudioCalculateDelegate calculateDelegate;

        private delegate void X3DAudioCalculateDelegate(
            ref SharpDX.X3DAudio.X3DAudioHandle instance,
            SharpDX.X3DAudio.Listener listenerRef,
            SharpDX.X3DAudio.Emitter emitterRef,
            SharpDX.X3DAudio.CalculateFlags flags,
            System.IntPtr dSPSettingsRef);

        /// <summary>
        /// Speed of sound in the air.
        /// </summary>
        public const float SpeedOfSound = 343.5f;

        /// <summary>
        /// Initializes a new instance of the <see cref="X3DAudio" /> class.
        /// </summary>
        /// <param name="speakers">The speakers config.</param>
        /// <msdn-id>microsoft.directx_sdk.x3daudio.x3daudioinitialize</msdn-id>
        /// <unmanaged>void X3DAudioInitialize([In] SPEAKER_FLAGS SpeakerChannelMask,[In] float SpeedOfSound,[Out] X3DAUDIOHANDLE* Instance)</unmanaged>
        /// <unmanaged-short>X3DAudioInitialize</unmanaged-short>
        public X3DAudio(Speakers speakers)
            : this(speakers, X3DAudioVersion.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X3DAudio" /> class.
        /// </summary>
        /// <param name="speakers">The speakers config.</param>
        /// <param name="requestVersion">The requestVersion.</param>
        /// <msdn-id>microsoft.directx_sdk.x3daudio.x3daudioinitialize</msdn-id>
        /// <unmanaged>void X3DAudioInitialize([In] SPEAKER_FLAGS SpeakerChannelMask,[In] float SpeedOfSound,[Out] X3DAUDIOHANDLE* Instance)</unmanaged>
        /// <unmanaged-short>X3DAudioInitialize</unmanaged-short>
        public X3DAudio(Speakers speakers, X3DAudioVersion requestVersion)
            : this(speakers, SpeedOfSound, requestVersion)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X3DAudio"/> class.
        /// </summary>
        /// <param name="speakers">The speakers config.</param>
        /// <param name="speedOfSound">The speed of sound.</param>
        /// <param name="requestVersion">The request requestVersion.</param>
        /// 
        /// <msdn-id>microsoft.directx_sdk.x3daudio.x3daudioinitialize</msdn-id>
        /// 
        /// <unmanaged>void X3DAudioInitialize([In] SPEAKER_FLAGS SpeakerChannelMask,[In] float SpeedOfSound,[Out] X3DAUDIOHANDLE* Instance)</unmanaged>
        /// 
        /// <unmanaged-short>X3DAudioInitialize</unmanaged-short>
        public X3DAudio(Speakers speakers, float speedOfSound, X3DAudioVersion requestVersion = X3DAudioVersion.Default)
        {
            var tryVersions = requestVersion == X3DAudioVersion.Default
                ? new [] {X3DAudioVersion.Version29, X3DAudioVersion.Version28, X3DAudioVersion.Version17}
                : new[] {requestVersion};

            foreach(var tryVersion in tryVersions)
            {
                switch(tryVersion)
                {
#if !WINDOWS_UWP
                    case X3DAudioVersion.Version17:
                        try
                        {
                            X3DAudio17.X3DAudioInitialize(speakers, speedOfSound, out handle);
                            version = X3DAudioVersion.Version17;
                            calculateDelegate = X3DAudio17.X3DAudioCalculate;
                        }
                        catch (DllNotFoundException) {}
                    break;
#endif
                    case X3DAudioVersion.Version28:
                        try
                        {
                            X3DAudio28.X3DAudioInitialize(speakers, speedOfSound, out handle);
                            version = X3DAudioVersion.Version28;
                            calculateDelegate = X3DAudio28.X3DAudioCalculate;
                        }
                        catch (DllNotFoundException) { }
                    break;
#if WINDOWS_UWP
                    case X3DAudioVersion.Version29:
                        try
                        {
                            X3DAudio29.X3DAudioInitialize(speakers, speedOfSound, out handle);
                            version = X3DAudioVersion.Version29;
                            calculateDelegate = X3DAudio29.X3DAudioCalculate;
                        }
                        catch (DllNotFoundException) { }
                    break;
#endif
                }

                if (version != X3DAudioVersion.Default)
                {
                    break;
                }
            }

            if(version == X3DAudioVersion.Default)
            {
                throw new DllNotFoundException(string.Format("Unable to find X3DAudio dlls for the following requested version [{0}]", string.Join(",", tryVersions)));
            }
        }

        /// <summary>
        /// Gets the requestVersion of X3DAudio used.
        /// </summary>
        /// <value>The requestVersion.</value>
        public X3DAudioVersion Version
        {
            get { return version; }
        }

        /// <summary>
        /// Calculates DSP settings for the specified listener and emitter.
        /// </summary>
        /// <param name="listener">The listener.</param>
        /// <param name="emitter">The emitter.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="sourceChannelCount">The source channel count.</param>
        /// <param name="destinationChannelCount">The destination channel count.</param>
        /// <returns>DSP settings</returns>
        /// <msdn-id>ee419052</msdn-id>	
        /// <unmanaged>void X3DAudioCalculate([In] const X3DAUDIOHANDLE* Instance,[In] const X3DAUDIO_LISTENER* pListener,[In] const X3DAUDIO_EMITTER* pEmitter,[In] X3DAudioCalculateFlags Flags,[In] void* pDSPSettings)</unmanaged>	
        /// <unmanaged-short>X3DAudioCalculate</unmanaged-short>	
        public DspSettings Calculate(Listener listener, Emitter emitter, CalculateFlags flags, int sourceChannelCount, int destinationChannelCount)
        {
            var settings = new DspSettings(sourceChannelCount, destinationChannelCount);
            Calculate(listener, emitter, flags, settings);
            return settings;
        }

        /// <summary>
        /// Calculates DSP settings for the specified listener and emitter. See remarks.
        /// </summary>
        /// <param name="listener">The listener.</param>
        /// <param name="emitter">The emitter.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="settings">The settings.</param>
        /// <remarks>The source and destination channel count must be set on <see cref="DspSettings" /> before calling this method.</remarks>
        /// <msdn-id>ee419052</msdn-id>	
        /// <unmanaged>void X3DAudioCalculate([In] const X3DAUDIOHANDLE* Instance,[In] const X3DAUDIO_LISTENER* pListener,[In] const X3DAUDIO_EMITTER* pEmitter,[In] X3DAudioCalculateFlags Flags,[In] void* pDSPSettings)</unmanaged>	
        /// <unmanaged-short>X3DAudioCalculate</unmanaged-short>	
        public unsafe void Calculate(Listener listener, Emitter emitter, CalculateFlags flags, DspSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");

            DspSettings.__Native settingsNative;
            settingsNative.SrcChannelCount = settings.SourceChannelCount;
            settingsNative.DstChannelCount = settings.DestinationChannelCount;

            fixed (void* pMatrix = settings.MatrixCoefficients)
            fixed (void* pDelays = settings.DelayTimes)
            {
                settingsNative.MatrixCoefficientsPointer = (IntPtr)pMatrix;
                settingsNative.DelayTimesPointer = (IntPtr)pDelays;
                
                calculateDelegate(ref handle, listener, emitter, flags, new IntPtr(&settingsNative));
            }

            settings.__MarshalFrom(ref settingsNative);
        }
    }
}