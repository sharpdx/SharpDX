using SharpDX.Toolkit.Content;

// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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

namespace SharpDX.Toolkit.Audio
{
    using System;
    using Multimedia;
    using X3DAudio;
    using XAPO.Fx;
    using XAudio2;
    using XAudio2.Fx;
    using Reverb = XAudio2.Fx.Reverb;
    using ReverbParameters = XAudio2.Fx.ReverbParameters;

    /// <summary>
    /// This manages the XAudio2 audio graph, device, and mastering voice.  This manager also allows loading of <see cref="SoundEffect"/> using
    /// the <see cref="IContentManager"/>
    /// </summary>
    public class AudioManager : GameSystem
    {
        private const float FLT_MIN = 1.175494351e-38F;

        /// <summary>
        /// Result of a device not found.
        /// </summary>
        /// <unmanaged>ERROR_NOT_FOUND</unmanaged>
        private static readonly ResultDescriptor NotFound = new ResultDescriptor(unchecked((int)0x80070490), "Windows Portable Devices", "ERROR_NOT_FOUND", "NotFound");

        private static readonly ReverbI3DL2Parameters[] reverbPresets =
        {
            ReverbI3DL2Parameters.Presets.Default,
            ReverbI3DL2Parameters.Presets.Generic,
            ReverbI3DL2Parameters.Presets.PaddedCell,
            ReverbI3DL2Parameters.Presets.Room,
            ReverbI3DL2Parameters.Presets.BathRoom,
            ReverbI3DL2Parameters.Presets.LivingRoom,
            ReverbI3DL2Parameters.Presets.StoneRoom,
            ReverbI3DL2Parameters.Presets.Auditorium,
            ReverbI3DL2Parameters.Presets.ConcertHall,
            ReverbI3DL2Parameters.Presets.Cave,
            ReverbI3DL2Parameters.Presets.Arena,
            ReverbI3DL2Parameters.Presets.Hangar,
            ReverbI3DL2Parameters.Presets.CarpetedHallway,
            ReverbI3DL2Parameters.Presets.Hallway,
            ReverbI3DL2Parameters.Presets.StoneCorridor,
            ReverbI3DL2Parameters.Presets.Alley,
            ReverbI3DL2Parameters.Presets.Forest,
            ReverbI3DL2Parameters.Presets.City,
            ReverbI3DL2Parameters.Presets.Mountains,
            ReverbI3DL2Parameters.Presets.Quarry,
            ReverbI3DL2Parameters.Presets.Plain,
            ReverbI3DL2Parameters.Presets.ParkingLot,
            ReverbI3DL2Parameters.Presets.SewerPipe,
            ReverbI3DL2Parameters.Presets.UnderWater,
            ReverbI3DL2Parameters.Presets.SmallRoom,
            ReverbI3DL2Parameters.Presets.MediumRoom,
            ReverbI3DL2Parameters.Presets.LargeRoom,
            ReverbI3DL2Parameters.Presets.MediumHall,
            ReverbI3DL2Parameters.Presets.LargeHall,
            ReverbI3DL2Parameters.Presets.Plate,
        };

        private ContentManager contentManager;
        private MasteringLimiter masteringLimiter;
        private MasteringLimiterParameters masteringLimiterParameters;
        private float masterVolume;
        private Reverb reverb;
        private ReverbParameters reverbParameters;
        private float speedOfSound;
        private X3DAudio x3DAudio;
        private bool isMasteringLimiterEnabled;
        private bool isReverbEffectEnabled;
        private bool isReverbFilterEnabled;
        private bool isSpatialAudioEnabled;
        private XAudio2 device;
        private SoundEffectInstancePool instancePool;
        private MasteringVoice masteringVoice;
        private SubmixVoice reverbVoice;
        private Speakers speakers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManager" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public AudioManager(Game game)
            : base(game)
        {
            Services.AddService(this);
            masterVolume = 1.0f;
            Speakers = Speakers.None;
            masteringLimiterParameters = new MasteringLimiterParameters { Loudness = MasteringLimiter.DefaultLoudness, Release = MasteringLimiter.DefaultRelease };
            reverbParameters = (ReverbParameters)ReverbI3DL2Parameters.Presets.Default;

            InstancePool = new SoundEffectInstancePool(this);

            // register the audio manager as game system
            game.GameSystems.Add(this);
        }

        /// <summary>
        /// Gets a value indicating whether mastering limiter is enabled.
        /// </summary>
        public bool IsMasteringLimiterEnabled
        {
            get
            {
                DisposeGuard();
                return isMasteringLimiterEnabled;
            }
            private set
            {
                DisposeGuard();
                isMasteringLimiterEnabled = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether reverb effect is enabled.
        /// </summary>
        public bool IsReverbEffectEnabled
        {
            get
            {
                DisposeGuard();
                return isReverbEffectEnabled;
            }
            private set
            {
                DisposeGuard();
                isReverbEffectEnabled = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether reverb filter is enabled.
        /// </summary>
        public bool IsReverbFilterEnabled
        {
            get
            {
                DisposeGuard();
                return isReverbFilterEnabled;
            }
            private set
            {
                DisposeGuard();
                isReverbFilterEnabled = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether spatial audio is enabled.
        /// </summary>
        public bool IsSpatialAudioEnabled
        {
            get
            {
                DisposeGuard();
                return isSpatialAudioEnabled;
            }
            private set
            {
                DisposeGuard();
                isSpatialAudioEnabled = value;
            }
        }

        /// <summary>
        /// Sets and gets the volume of the Mastering voice.
        /// </summary>
        public float MasterVolume
        {
            get
            {
                DisposeGuard();
                return masterVolume;
            }
            set
            {
                DisposeGuard();

                if (MathUtil.NearEqual(value, masterVolume))
                    return;

                masterVolume = MathUtil.Clamp(value, 0.0f, 1.0f);

                if (MasteringVoice != null)
                    MasteringVoice.SetVolume(masterVolume);
            }
        }

        /// <summary>
        /// Gets the <see cref="XAudio2"/> device associated with the current <see cref="AudioManager"/> instance.
        /// </summary>
        internal XAudio2 Device
        {
            get
            {
                DisposeGuard();
                return device;
            }
            private set
            {
                DisposeGuard();
                device = value;
            }
        }

        /// <summary>
        /// Gets the pool of <see cref="SoundEffectInstance"/> associated with the current <see cref="AudioManager"/> instance.
        /// </summary>
        internal SoundEffectInstancePool InstancePool
        {
            get
            {
                DisposeGuard();
                return instancePool;
            }
            private set
            {
                DisposeGuard();
                instancePool = value;
            }
        }

        /// <summary>
        /// Gets a reference to the <see cref="MasteringVoice"/> associated with the current <see cref="AudioManager"/> instance.
        /// </summary>
        internal MasteringVoice MasteringVoice
        {
            get
            {
                DisposeGuard();
                return masteringVoice;
            }
            private set
            {
                DisposeGuard();
                masteringVoice = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="SubmixVoice"/> associated with the current <see cref="AudioManager"/> instance (for reverb effect).
        /// </summary>
        internal SubmixVoice ReverbVoice
        {
            get
            {
                DisposeGuard();
                return reverbVoice;
            }
            private set
            {
                DisposeGuard();
                reverbVoice = value;
            }
        }

        /// <summary>
        /// Gets the speaker configuration.
        /// </summary>
        internal Speakers Speakers
        {
            get
            {
                DisposeGuard();
                return speakers;
            }
            private set
            {
                DisposeGuard();
                speakers = value;
            }
        }

        /// <summary>
        /// Disables the master volume limiter.
        /// </summary>
        public void DisableMasterVolumeLimiter()
        {
            DisposeGuard();

            if (!IsMasteringLimiterEnabled)
                return;

            if(MasteringVoice != null && masteringLimiter != null)
                MasteringVoice.DisableEffect(0);

            IsMasteringLimiterEnabled = false;
        }

        /// <summary>
        /// Disables the reverb effect.
        /// </summary>
        public void DisableReverbEffect()
        {
            DisposeGuard();

            if (!IsReverbEffectEnabled)
                return;

            if(ReverbVoice != null && reverb != null)
                ReverbVoice.DisableEffect(0);

            IsReverbEffectEnabled = false;
        }

        /// <summary>
        /// Enables the master volume limiter.
        /// </summary>
        public void EnableMasterVolumeLimiter()
        {
            DisposeGuard();

            if (IsMasteringLimiterEnabled)
                return;

            if (MasteringVoice != null)
            {
                if(masteringLimiter == null)
                    CreateMasteringLimitier();
                else
                    MasteringVoice.EnableEffect(0);
            }

            IsMasteringLimiterEnabled = true;
        }

        /// <summary>
        /// Enables the reverb effect.
        /// </summary>
        public void EnableReverbEffect()
        {
            DisposeGuard();

            if (IsReverbEffectEnabled)
                return;

            if (MasteringVoice != null)
            {
                if(ReverbVoice == null)
                    CreateReverbSubmixVoice();
                else
                    ReverbVoice.EnableEffect(0);
            }

            IsReverbEffectEnabled = true;
        }

        /// <summary>
        /// Enables the reverb filter.
        /// </summary>
        public void EnableReverbFilter()
        {
            DisposeGuard();

            IsReverbFilterEnabled = true;
        }

        /// <summary>
        /// Enables the spatial audio effect.
        /// </summary>
        /// <param name="speedOfSound">The speed of the sound in the medium. Should be greater than or equal to 1.175494351e-38F.</param>
        public void EnableSpatialAudio(float speedOfSound)
        {
            DisposeGuard();

            if (speedOfSound < FLT_MIN)
                throw new ArgumentOutOfRangeException("speedOfSound", "Speed of sound must be greater than or equal to FLT_MIN (1.175494351e-38F).");

            IsSpatialAudioEnabled = true;
            this.speedOfSound = speedOfSound;

            if (MasteringVoice != null)
            {
                x3DAudio = new X3DAudio(Speakers, speedOfSound);
            }
        }

        /// <summary>
        /// Enables the spatial audio effect with the default speed of sound equal to <see cref="X3DAudio.SpeedOfSound"/>.
        /// </summary>
        public void EnableSpatialAudio()
        {
            EnableSpatialAudio(X3DAudio.SpeedOfSound);
        }

        /// <summary>
        /// Initializes XAudio2 and MasteringVoice.  And registers itself as an <see cref="IContentReaderFactory"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown when the IContentManager is not an instance of <see cref="ContentManager"/>.</exception>
        /// <exception cref="AudioException">Is thrown when the <see cref="AudioManager"/> instance could not be initialized (either due to unsupported features or missing audio-device).</exception>
        public override void Initialize()
        {
            base.Initialize();
            contentManager = Content as ContentManager;
            if (contentManager == null)
            {
                throw new InvalidOperationException("Unable to initialize AudioManager. Expecting IContentManager to be an instance of ContentManager");
            }
            try
            {
#if DEBUG && !WIN8METRO && !WP8 && !DIRECTX11_1
                try
                {
                    // "XAudio2Flags.DebugEngine" is supported only in XAudio 2.7, but not in newer versions
                    // msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2create(v=vs.85).aspx
                    Device = new XAudio2(XAudio2Flags.DebugEngine, ProcessorSpecifier.DefaultProcessor);
                    Device.StartEngine();
                }
                catch (Exception)
#endif
                {
                    Device = new XAudio2(XAudio2Flags.None, ProcessorSpecifier.DefaultProcessor);
                    Device.StartEngine();
                }
            }
            catch (SharpDXException ex)
            {
                DisposeCore();
                throw new AudioException("Error creating XAudio device.", ex);
            }

#if !W8CORE && !DIRECTX11_1
            if (Device.DeviceCount == 0)
            {
                DisposeCore();
                throw new AudioException("No default audio devices detected.");
            }
#endif

#if W8CORE || DIRECTX11_1
            string deviceId = null;
#else
            const int deviceId = 0;
#endif
            try
            {
                MasteringVoice = new MasteringVoice(Device, XAudio2.DefaultChannels, XAudio2.DefaultSampleRate, deviceId);
            }
            catch (SharpDXException ex)
            {
                DisposeCore();
#if W8CORE
                if (ex.ResultCode == AudioManager.NotFound)
                {
                    throw new AudioException("No default audio devices detected.");
                }
                else
#endif
                {
                    throw new AudioException("Error creating mastering voice.", ex);
                }
            }

            MasteringVoice.SetVolume(masterVolume);

#if W8CORE || DIRECTX11_1
            Speakers = (Speakers)MasteringVoice.ChannelMask;
#else
            var deviceDetails = Device.GetDeviceDetails(deviceId);
            Speakers = deviceDetails.OutputFormat.ChannelMask;
#endif

            if (IsMasteringLimiterEnabled)
            {
                try
                {
                    CreateMasteringLimitier();
                }
                catch (Exception)
                {
                    DisposeCore();
                    throw;
                }
            }

            if (IsSpatialAudioEnabled)
            {
                try
                {
                    x3DAudio = new X3DAudio(Speakers, speedOfSound);
                }
                catch (Exception)
                {
                    DisposeCore();
                    throw;
                }
            }

            if (IsReverbEffectEnabled)
            {
                try
                {
                    CreateReverbSubmixVoice();
                }
                catch (Exception)
                {
                    DisposeCore();
                    throw;
                }
            }

            contentManager.ReaderFactories.Add(new AudioContentReaderFactory());
        }

        /// <summary>
        /// Sets the mastering limiter parameters.
        /// </summary>
        /// <param name="release">Speed at which the limiter stops affecting audio once it drops below the limiter's threshold.</param>
        /// <param name="loudness">Threshold of the limiter.</param>
        /// <exception cref="ObjectDisposedException">Is thrown when this instance was already disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when either <paramref name="release"/> or <paramref name="loudness"/> are outside of allowed ranges
        /// (<see cref="MasteringLimiter.MinimumRelease"/>/<see cref="MasteringLimiter.MaximumRelease"/> 
        /// and <see cref="MasteringLimiter.MinimumLoudness"/>/<see cref="MasteringLimiter.MaximumLoudness"/> respectively).</exception>
        public void SetMasteringLimit(int release, int loudness)
        {
            DisposeGuard();

            if (release < MasteringLimiter.MinimumRelease || release > MasteringLimiter.MaximumRelease)
                throw new ArgumentOutOfRangeException("release");

            if (loudness < MasteringLimiter.MinimumLoudness || loudness > MasteringLimiter.MaximumLoudness)
                throw new ArgumentOutOfRangeException("loudness");

            masteringLimiterParameters = new MasteringLimiterParameters { Loudness = loudness, Release = release };

            if (MasteringVoice != null && masteringLimiter != null)
            {
                MasteringVoice.SetEffectParameters(0, masteringLimiterParameters);
            }
        }

        /// <summary>
        /// Sets the Reverb effect parameters.
        /// </summary>
        /// <param name="parameters">The reverb effect parameters.</param>
        /// /// <exception cref="ObjectDisposedException">Is thrown when this instance was already disposed.</exception>
        public void SetReverbEffectParameters(ReverbParameters parameters)
        {
            DisposeGuard();

            reverbParameters = parameters;

            if (ReverbVoice != null && reverb != null)
            {
                ReverbVoice.SetEffectParameters(0, parameters);
            }
        }

        /// <summary>
        /// Sets the Reverb effect parameters from an existing preset.
        /// </summary>
        /// <param name="preset">The existing Reverb preset.</param>
        public void SetReverbEffectParameters(ReverbPresets preset)
        {
            DisposeGuard();

            SetReverbEffectParameters((ReverbParameters)reverbPresets[(int)preset]);
        }

        /// <summary>
        /// Calculate 3D Audio parameters.
        /// </summary>
        /// <param name="listener">The 3D audio listener definition.</param>
        /// <param name="emitter">The 3D audio emitter definition.</param>
        /// <param name="flags">The 3D audio calculate flags.</param>
        /// <param name="dspSettings">The DSP settings.</param>
        internal void Calculate3D(Listener listener, Emitter emitter, CalculateFlags flags, DspSettings dspSettings)
        {
            DisposeGuard();

            x3DAudio.Calculate(listener, emitter, flags, dspSettings);
        }

        /// <summary>
        /// Adds a disposable audio asset to the list of the objects to dispose.
        /// </summary>
        /// <param name="audioAsset">To dispose.</param>
        internal T ToDisposeAudioAsset<T>(T audioAsset) where T : IDisposable
        {
            return ToDispose(audioAsset);
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                InstancePool.Dispose();
                base.Dispose(true);
                DisposeCore();
            }
        }

        private void CreateMasteringLimitier()
        {
            try
            {
                masteringLimiter = new MasteringLimiter();
                masteringLimiter.Parameter = masteringLimiterParameters;
                MasteringVoice.SetEffectChain(new EffectDescriptor(masteringLimiter));
            }
            catch (SharpDXException ex)
            {
                throw new AudioException("Error creating mastering limiter.", ex);
            }
        }

        private void CreateReverbSubmixVoice()
        {
            try
            {
                VoiceDetails masterDetails = MasteringVoice.VoiceDetails;
                SubmixVoiceFlags sendFlags = IsReverbFilterEnabled ? SubmixVoiceFlags.UseFilter : SubmixVoiceFlags.None;
                ReverbVoice = new SubmixVoice(Device, 1, masterDetails.InputSampleRate, sendFlags, 0);
            }
            catch (SharpDXException ex)
            {
                throw new AudioException("Error creating reverb submix voice.", ex);
            }

            try
            {
                reverb = new Reverb();
                ReverbVoice.SetEffectChain(new EffectDescriptor(reverb, 1));
                ReverbVoice.SetEffectParameters(0, reverbParameters);
            }
            catch (SharpDXException ex)
            {
                throw new AudioException("Error setting reverb effect.", ex);
            }
        }

        private void DisposeCore()
        {
            if (x3DAudio != null)
            {
                x3DAudio = null;
            }

            if (ReverbVoice != null)
            {
                ReverbVoice.DestroyVoice();
                ReverbVoice.Dispose();
                ReverbVoice = null;
                reverb.Dispose();
                reverb = null;
            }

            IsReverbEffectEnabled = false;

            if (MasteringVoice != null)
            {
                MasteringVoice.DestroyVoice();
                MasteringVoice.Dispose();
                MasteringVoice = null;
            }

            if (masteringLimiter != null)
            {
                masteringLimiter.Dispose();
                masteringLimiter = null;
            }

            IsMasteringLimiterEnabled = false;

            if (Device != null)
            {
                Device.StopEngine();
                Device.Dispose();
                Device = null;
            }
        }

        private void DisposeGuard()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }
    }
}