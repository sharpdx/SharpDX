// Copyright (c) 2010-2013 SharpDX - SharpDX Team
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
using System.Collections.Generic;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Audio
{
    using SharpDX.XAudio2;
    using SharpDX.Multimedia;
    using SharpDX.X3DAudio;
    using SharpDX.XAPO.Fx;
    using SharpDX.XAudio2.Fx;
    using ReverbParameters = SharpDX.XAudio2.Fx.ReverbParameters;
    using Reverb = SharpDX.XAudio2.Fx.Reverb;
    /// <summary>
    /// This manages the XAudio2 audio graph, device, and mastering voice.  This manager also allows loading of <see cref="SoundEffect"/> using
    /// the <see cref="IContentManager"/>
    /// </summary>
    public class AudioManager : GameSystem, IContentReader, IContentReaderFactory 
    {
        private ContentManager contentManager;
        private float masterVolume;
        private X3DAudio x3DAudio;
        private bool isMasteringLimiterEnabled;
        private MasteringLimiterParameters masteringLimiterParameters;
        private MasteringLimiter masteringLimiter;        
        private ReverbParameters reverbParameters;
        private Reverb reverb;

        private static ReverbI3DL2Parameters[] reverbPresets = new ReverbI3DL2Parameters[]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManager" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public AudioManager(Game game)
            : base(game)
        {
            Services.AddService(this);
            masterVolume = 1.0f;
            Speakers = Multimedia.Speakers.None;
            masteringLimiterParameters = new MasteringLimiterParameters { Loudness = MasteringLimiter.DefaultLoudness, Release = MasteringLimiter.DefaultRelease };
            reverbParameters = (ReverbParameters)ReverbI3DL2Parameters.Presets.Default;

            InstancePool = new SoundEffectInstancePool(this);

            // register the audio manager as game system
            game.GameSystems.Add(this);
        }


        /// <summary>
        /// Initializes XAudio2 and MasteringVoice.  And registers itself as an <see cref="IContentReaderFactory"/>
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            contentManager = Content as ContentManager;
            if (contentManager == null)
            {
                throw new InvalidOperationException("Unable to initialize AudioManager. Expecting IContentManager to be an instance of ContentManager");
            }

#if !WIN8METRO && !WP8 && DEBUG
            try
            {
                Device = new XAudio2(XAudio2Flags.DebugEngine, ProcessorSpecifier.DefaultProcessor);
                Device.StartEngine();
            }
            catch (Exception)
#endif
            {
                Device = new XAudio2(XAudio2Flags.None, ProcessorSpecifier.DefaultProcessor);
                Device.StartEngine();                
            }  

           

#if WIN8METRO || WP8
            string deviceId = null;
#else
            const int deviceId = 0;
#endif
            MasteringVoice = new MasteringVoice(Device, XAudio2.DefaultChannels, XAudio2.DefaultSampleRate, deviceId);
            MasteringVoice.SetVolume(masterVolume);
            


#if WIN8METRO || WP8
            Speakers = (Speakers)MasteringVoice.ChannelMask;
#else
            var deviceDetails = Device.GetDeviceDetails(deviceId);
            Speakers = deviceDetails.OutputFormat.ChannelMask;
#endif
            x3DAudio = new X3DAudio(Speakers, SoundEffect.SpeedOfSound);
            

            if(isMasteringLimiterEnabled)
            {
                CreateMasteringLimitier();
            }

            if(IsReverbEffectEnabled)
            {
                CreateReverbSubmixVoice();
            }

            contentManager.ReaderFactories.Add(this);
        }

        
        /// <summary>
        /// Sets and gets the volume of the Masteing voice.
        /// </summary>
        public float MasterVolume
        {
            get
            {
                return masterVolume;
            }
            set
            {

                if (IsDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);
            
                if (value == masterVolume)
                    return;

                masterVolume = MathUtil.Clamp(value, 0.0f, 1.0f);

                if (MasteringVoice != null)
                    MasteringVoice.SetVolume(masterVolume);
            }
        }


        internal Speakers Speakers { get; private set; }
        internal XAudio2 Device { get; private set; }
        internal MasteringVoice MasteringVoice  { get; private set; }
        internal SubmixVoice ReverbVoice { get; private set; }
        internal SoundEffectInstancePool InstancePool { get; private set; }
        public bool IsReverbEffectEnabled { get; private set; }
        public bool IsReverbFilterEnabled { get; private set; }

        public void EnableMasterVolumeLimiter()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);            
            
            if (isMasteringLimiterEnabled)
                return;

            if (MasteringVoice != null)
            {
                if (masteringLimiter == null)
                {
                    CreateMasteringLimitier();
                }
                else
                {
                    MasteringVoice.EnableEffect(0);
                }
            }            

            isMasteringLimiterEnabled = true;
        }


        public void DisableMasterVolumeLimiter()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            if (!isMasteringLimiterEnabled)
                return;            

            if (MasteringVoice != null && masteringLimiter != null)
            {
                MasteringVoice.DisableEffect(0);
            }

            isMasteringLimiterEnabled = false;
        }

        private void CreateMasteringLimitier()
        {
            masteringLimiter = new MasteringLimiter();
            masteringLimiter.Parameter = masteringLimiterParameters;
            MasteringVoice.SetEffectChain(new EffectDescriptor(masteringLimiter));
        }

        public void SetMasteringLimit(int release, int loudness)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            if (release < MasteringLimiter.MinimumRelease || release > MasteringLimiter.MaximumRelease)
                throw new ArgumentOutOfRangeException("release");

            if (loudness < MasteringLimiter.MinimumLoudness || loudness > MasteringLimiter.MaximumLoudness)
                throw new ArgumentOutOfRangeException("loudness");
            
            masteringLimiterParameters = new MasteringLimiterParameters { Loudness = loudness, Release = release };

            if(MasteringVoice != null && masteringLimiter != null)
            {
                MasteringVoice.SetEffectParameters(0, masteringLimiterParameters);
            }
        }

        public void EnableReverbEffect()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            if (IsReverbEffectEnabled)
                return;

            if (MasteringVoice != null)
            {
                if (ReverbVoice == null)
                {
                    CreateReverbSubmixVoice();
                }
                else
                {
                    ReverbVoice.EnableEffect(0);
                }
            }

            IsReverbEffectEnabled = true;
        }

        public void DisableReverbEffect()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            if (!IsReverbEffectEnabled)
                return;

            if (ReverbVoice != null && reverb != null)
            {
                ReverbVoice.DisableEffect(0);
            }

            IsReverbEffectEnabled = false;
        }

        public void SetReverbEffectParameters(ReverbParameters parameters)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            reverbParameters = parameters;

            if (ReverbVoice != null && reverb != null)
            {
                ReverbVoice.SetEffectParameters(0, parameters);
            }
        }

        public void SetReverbEffectParameters(ReverbPresets preset)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            SetReverbEffectParameters((ReverbParameters)reverbPresets[(int)preset]);
        }

        public void EnableReverbFilter()
        {
            IsReverbFilterEnabled = true;
        }
        
        
        private void CreateReverbSubmixVoice()
        {
            VoiceDetails masterDetails = MasteringVoice.VoiceDetails;
            VoiceSendFlags sendFlags = IsReverbFilterEnabled ? VoiceSendFlags.UseFilter : VoiceSendFlags.None;
            ReverbVoice = new SubmixVoice(Device, 1, masterDetails.InputSampleRate, sendFlags, 0);
            reverb = new Reverb();
            ReverbVoice.SetEffectChain(new EffectDescriptor(reverb, 1));
            ReverbVoice.SetEffectParameters(0, reverbParameters);
        }

        internal void Calculate3D(Listener listener, Emitter emitter, CalculateFlags flags, DspSettings dspSettings)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            x3DAudio.Calculate(listener, emitter, flags, dspSettings);            
        }


        IContentReader IContentReaderFactory.TryCreate(Type type)
        {
            if (type == typeof(SoundEffect) || type == typeof(WaveBank))
                return this;

            return null;
        }


        object IContentReader.ReadContent(IContentManager contentManager, ref ContentReaderParameters parameters)
        {
            if (parameters.AssetType == typeof(SoundEffect))
                return SoundEffect.FromStream(this, parameters.Stream, parameters.AssetName);

            if (parameters.AssetType == typeof(WaveBank))
                return WaveBank.FromStream(this, parameters.Stream);

            return null;
        }

        internal SoundEffect ToDisposeSoundEffect(SoundEffect soundEffect)
        {
            return ToDispose(soundEffect);
        }


        protected override void Dispose(bool disposeManagedResources)
        {            
            if (disposeManagedResources)
            {
                InstancePool.Dispose();
                base.Dispose(disposeManagedResources);
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

                if(masteringLimiter != null)
                {
                    masteringLimiter.Dispose();
                    masteringLimiter = null;
                }

                isMasteringLimiterEnabled = false;

                if (Device != null)
                {
                    Device.StopEngine();
                    Device.Dispose();
                    Device = null;
                }
            }

        }  
    }
}
