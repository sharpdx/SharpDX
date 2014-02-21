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

namespace SharpDX.Toolkit.Audio
{
    using System;
    using Multimedia;
    using X3DAudio;
    using XAudio2;

    /// <summary>
    /// Provides a single playing, paused, or stopped instance of a <see cref="SoundEffect"/> sound.
    /// </summary>
    public sealed class SoundEffectInstance : IDisposable
    {
        private DspSettings dspSettings;
        private Emitter emitter;
        private bool isReverbSubmixEnabled;
        private Listener listener;
        private float[] outputMatrix;
        private float pan;
        private bool paused;
        private float pitch;
        private float[] reverbLevels;
        private SourceVoice voice;
        private float volume;

        /// <summary>
        /// Creates a new instance of the <see cref="SoundEffectInstance"/> class.
        /// </summary>
        /// <param name="soundEffect">The source effect whose instance needs to be created.</param>
        /// <param name="sourceVoice">The source voice to play the created instance.</param>
        /// <param name="isFireAndForget">A value indicating whether this instance is not monitored after it is being send to playback.</param>
        internal SoundEffectInstance(SoundEffect soundEffect, SourceVoice sourceVoice, bool isFireAndForget)
        {
            Effect = soundEffect;
            voice = sourceVoice;
            IsFireAndForget = isFireAndForget;
            paused = false;
            IsLooped = false;
            volume = 1.0f;
            pan = 0.0f;
            pitch = 0.0f;
            outputMatrix = null;
        }

        /// <summary>
        /// Gets the base sound effect.
        /// </summary>
        public SoundEffect Effect { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is diposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is looped.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        /// Gets or sets the pan value of the sound effect.
        /// </summary>
        /// <remarks>The value is clamped to (-1f, 1f) range.</remarks>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public float Pan
        {
            get
            {
                return pan;
            }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (MathUtil.NearEqual(pan, value))
                    return;

                pan = MathUtil.Clamp(value, -1.0f, 1.0f);

                SetPanOutputMatrix();
            }
        }

        /// <summary>
        /// Gets or sets the pitch value of the sound effect.
        /// </summary>
        /// <remarks>The value is clamped to (-1f, 1f) range.</remarks>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (MathUtil.NearEqual(pitch, value))
                    return;

                pitch = MathUtil.Clamp(value, -1.0f, 1.0f);

                voice.SetFrequencyRatio(XAudio2.SemitonesToFrequencyRatio(pitch));
            }
        }

        /// <summary>
        /// Gets the state of the current sound effect instance.
        /// </summary>
        public SoundState State
        {
            get
            {
                if (voice == null || voice.State.BuffersQueued == 0)
                    return SoundState.Stopped;

                if (paused)
                    return SoundState.Paused;

                return SoundState.Playing;
            }
        }

        /// <summary>
        /// Gets or sets the volume of the current sound effect instance.
        /// </summary>
        /// <remarks>The value is clamped to (0f, 1f) range.</remarks>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (MathUtil.NearEqual(volume, value))
                    return;

                volume = MathUtil.Clamp(value, 0.0f, 1.0f);

                voice.SetVolume(volume);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is not monitored after submitting it for playback.
        /// </summary>
        internal bool IsFireAndForget { get; private set; }

        /// <summary>
        /// Gets the current audio buffer.
        /// </summary>
        private AudioBuffer CurrentAudioBuffer
        {
            get
            {
                if (Effect == null || Effect.AudioBuffer == null)
                    return null;

                return IsLooped ? Effect.LoopedAudioBuffer : Effect.AudioBuffer;
            }
        }

        /// <summary>
        /// Applies the 3D effect to the current sound effect instance.
        /// </summary>
        /// <param name="listener">The listener position.</param>
        /// <param name="listenerVelocity">The listener velocity.</param>
        /// <param name="emitter">The emitter position.</param>
        /// <param name="emitterVelocity">The emitter velocity.</param>
        public void Apply2D(Vector2 listener, Vector2 listenerVelocity, Vector2 emitter, Vector2 emitterVelocity)
        {
            Apply3D(Vector3.ForwardLH, Vector3.Up, new Vector3(listener, 0), new Vector3(listenerVelocity, 0), Vector3.ForwardLH, Vector3.Up, new Vector3(emitter, 0), new Vector3(emitterVelocity, 0));
        }

        
        // TODO: X3DAudio uses a left-handed Cartesian coordinate system. may need overloads for lh/rh.  seems to work with right hand matricies without it though.
        /// <summary>
        /// Applies the 3D effect to the current sound effect instance.
        /// </summary>
        /// <param name="listenerWorld">The listener world matrix.</param>
        /// <param name="listenerVelocity">The listener velocity.</param>
        /// <param name="emitterWorld">The emitter world matrix.</param>
        /// <param name="emitterVelocity">The emitter velocity.</param>
        public void Apply3D(Matrix listenerWorld, Vector3 listenerVelocity, Matrix emitterWorld, Vector3 emitterVelocity)
        {
            Apply3D(listenerWorld.Forward, listenerWorld.Up, listenerWorld.TranslationVector, listenerVelocity, emitterWorld.Forward, emitterWorld.Up, emitterWorld.TranslationVector, emitterVelocity);
        }

        /// <summary>
        /// Disposes the current instance and releases all associated unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Pauses the playback of the current instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public void Pause()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            voice.Stop();
            paused = true;
        }

        /// <summary>
        /// Plays the current instance. If it is already playing - the call is ignored.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public void Play()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (State == SoundState.Playing)
                return;

            if (voice.State.BuffersQueued > 0)
            {
                voice.Stop();
                voice.FlushSourceBuffers();
            }

            voice.SubmitSourceBuffer(CurrentAudioBuffer, Effect.DecodedPacketsInfo);
            voice.Start();

            paused = false;
        }

        /// <summary>
        /// Resets the current instance.
        /// </summary>
        public void Reset()
        {
            Volume = 1.0f;
            Pitch = 0.0f;
            Pan = 0.0f;
            IsLooped = false;
        }

        /// <summary>
        /// Resumes playback of the current instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public void Resume()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (!IsLooped)
            {
                if (voice.State.BuffersQueued == 0)
                {
                    voice.Stop();
                    voice.FlushSourceBuffers();
                    voice.SubmitSourceBuffer(CurrentAudioBuffer, Effect.DecodedPacketsInfo);
                }
            }

            voice.Start();
            paused = false;
        }

        /// <summary>
        /// Stops the playback of the current instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public void Stop()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            voice.Stop(0);
            voice.FlushSourceBuffers();

            paused = false;
        }

        /// <summary>
        /// Stops the playback of the current instance indicating whether the stop should occur immediately of at the end of the sound.
        /// </summary>
        /// <param name="immediate">A value indicating whether the playback should be stopped immediately or at the end of the sound.</param>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public void Stop(bool immediate)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (immediate)
            {
                voice.Stop(0);
            }
            else if (IsLooped)
            {
                voice.ExitLoop();
            }
            else
            {
                voice.Stop((int)PlayFlags.Tails);
            }

            paused = false;
        }

        /// <summary>
        /// Handles the event of disposal of the parent <see cref="SoundEffect"/>.
        /// </summary>
        internal void ParentDisposed()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                ReleaseSourceVoice();
                Effect = null;
                outputMatrix = null;
            }
        }

        /// <summary>
        /// Resets the current instance to be reused in an instance pool.
        /// </summary>
        /// <param name="soundEffect">The new parent sound effect.</param>
        /// <param name="sourceVoice">The new source voice.</param>
        /// <param name="isFireAndForget">The new <see cref="IsFireAndForget"/> value.</param>
        internal void Reset(SoundEffect soundEffect, SourceVoice sourceVoice, bool isFireAndForget)
        {
            Effect = soundEffect;
            voice = sourceVoice;
            IsFireAndForget = isFireAndForget;

            if (soundEffect != null && sourceVoice != null)
                Reset();
        }

        private void Apply3D(Vector3 listenerForward, Vector3 listenerUp, Vector3 listenerPosition, Vector3 listenerVelocity, Vector3 emitterForward, Vector3 emitterUp, Vector3 emitterPosition, Vector3 emitterVelocity)
        {
            if (!Effect.AudioManager.IsSpatialAudioEnabled)
                throw new InvalidOperationException("Spatial audio must be enabled first.");

            if (emitter == null)
                emitter = new Emitter();

            emitter.OrientFront = emitterForward;
            emitter.OrientTop = emitterUp;
            emitter.Position = emitterPosition;
            emitter.Velocity = emitterVelocity;
            emitter.DopplerScaler = SoundEffect.DopplerScale;
            emitter.CurveDistanceScaler = SoundEffect.DistanceScale;
            emitter.ChannelCount = Effect.Format.Channels;

            //TODO: work out what ChannelAzimuths is supposed to be.
            if (emitter.ChannelCount > 1)
                emitter.ChannelAzimuths = new float[emitter.ChannelCount];

            if (listener == null)
                listener = new Listener();

            listener.OrientFront = listenerForward;
            listener.OrientTop = listenerUp;
            listener.Position = listenerPosition;
            listener.Velocity = listenerVelocity;

            if (dspSettings == null)
                dspSettings = new DspSettings(Effect.Format.Channels, Effect.AudioManager.MasteringVoice.VoiceDetails.InputChannelCount);

            CalculateFlags flags = CalculateFlags.Matrix | CalculateFlags.Doppler | CalculateFlags.LpfDirect;

            if ((Effect.AudioManager.Speakers & Speakers.LowFrequency) > 0)
            {
                // On devices with an LFE channel, allow the mono source data to be routed to the LFE destination channel.
                flags |= CalculateFlags.RedirectToLfe;
            }

            if (Effect.AudioManager.IsReverbEffectEnabled)
            {
                flags |= CalculateFlags.Reverb | CalculateFlags.LpfReverb;

                if (!isReverbSubmixEnabled)
                {
                    VoiceSendFlags sendFlags = Effect.AudioManager.IsReverbFilterEnabled ? VoiceSendFlags.UseFilter : VoiceSendFlags.None;
                    VoiceSendDescriptor[] outputVoices = new VoiceSendDescriptor[]
                    {
                        new VoiceSendDescriptor { OutputVoice = Effect.AudioManager.MasteringVoice, Flags = sendFlags },
                        new VoiceSendDescriptor { OutputVoice = Effect.AudioManager.ReverbVoice, Flags = sendFlags }
                    };

                    voice.SetOutputVoices(outputVoices);
                    isReverbSubmixEnabled = true;
                }
            }

            Effect.AudioManager.Calculate3D(listener, emitter, flags, dspSettings);

            voice.SetFrequencyRatio(dspSettings.DopplerFactor);
            voice.SetOutputMatrix(Effect.AudioManager.MasteringVoice, dspSettings.SourceChannelCount, dspSettings.DestinationChannelCount, dspSettings.MatrixCoefficients);

            if (Effect.AudioManager.IsReverbEffectEnabled)
            {
                if (reverbLevels == null || reverbLevels.Length != Effect.Format.Channels)
                    reverbLevels = new float[Effect.Format.Channels];

                for (int i = 0; i < reverbLevels.Length; i++)
                {
                    reverbLevels[i] = dspSettings.ReverbLevel;
                }

                voice.SetOutputMatrix(Effect.AudioManager.ReverbVoice, Effect.Format.Channels, 1, reverbLevels);
            }

            if (Effect.AudioManager.IsReverbFilterEnabled)
            {
                FilterParameters filterDirect = new FilterParameters
                {
                    Type = FilterType.LowPassFilter,
                    // see XAudio2CutoffFrequencyToRadians() in XAudio2.h for more information on the formula used here
                    Frequency = 2.0f * (float)Math.Sin(X3DAudio.PI / 6.0f * dspSettings.LpfDirectCoefficient),
                    OneOverQ = 1.0f
                };

                voice.SetOutputFilterParameters(Effect.AudioManager.MasteringVoice, filterDirect);

                if (Effect.AudioManager.IsReverbEffectEnabled)
                {
                    FilterParameters filterReverb = new FilterParameters
                    {
                        Type = FilterType.LowPassFilter,
                        // see XAudio2CutoffFrequencyToRadians() in XAudio2.h for more information on the formula used here
                        Frequency = 2.0f * (float)Math.Sin(X3DAudio.PI / 6.0f * dspSettings.LpfReverbCoefficient),
                        OneOverQ = 1.0f
                    };

                    voice.SetOutputFilterParameters(Effect.AudioManager.ReverbVoice, filterReverb);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                ReleaseSourceVoice();
                Effect.ChildDisposed(this);
                Effect = null;
                outputMatrix = null;
            }
        }

        private void InitializeOutputMatrix(out int destinationChannels, out int sourceChannels)
        {
            destinationChannels = Effect.AudioManager.MasteringVoice.VoiceDetails.InputChannelCount;
            sourceChannels = Effect.Format.Channels;

            var outputMatrixSize = destinationChannels * sourceChannels;

            if (outputMatrix == null || outputMatrix.Length != outputMatrixSize)
                outputMatrix = new float[outputMatrixSize];

            // Default to full volume for all channels/destinations
            for (var i = 0; i < outputMatrix.Length; i++)
                outputMatrix[i] = 1.0f;
        }

        private void ReleaseSourceVoice()
        {
            if (voice != null && !voice.IsDisposed)
            {
                voice.Stop(0);
                voice.FlushSourceBuffers();
                if (isReverbSubmixEnabled)
                {
                    voice.SetOutputVoices((VoiceSendDescriptor[])null);
                    isReverbSubmixEnabled = false;
                }

                if (Effect.VoicePool.IsDisposed)
                {
                    voice.DestroyVoice();
                    voice.Dispose();
                }
                else
                {
                    Effect.VoicePool.Return(voice);
                }
            }
            voice = null;
        }

        private void SetPanOutputMatrix()
        {
            int destinationChannels;
            int sourceChannels;
            InitializeOutputMatrix(out destinationChannels, out sourceChannels);

            if (pan != 0.0f)
            {
                var panLeft = 1.0f - pan;
                var panRight = 1.0f + pan;

                //The level sent from source channel S to destination channel D
                //is specified in the form pLevelMatrix[SourceChannels × D + S]
                for (int S = 0; S < sourceChannels; S++)
                {
                    switch (Effect.AudioManager.Speakers)
                    {
                        case Speakers.Stereo:
                        case Speakers.TwoPointOne:
                        case Speakers.Surround:
                            outputMatrix[(sourceChannels * 0) + S] = panLeft;
                            outputMatrix[(sourceChannels * 1) + S] = panRight;
                            break;

                        case Speakers.Quad:
                            outputMatrix[(sourceChannels * 0) + S] = outputMatrix[(sourceChannels * 2) + S] = panLeft;
                            outputMatrix[(sourceChannels * 1) + S] = outputMatrix[(sourceChannels * 3) + S] = panRight;
                            break;

                        case Speakers.FourPointOne:
                            outputMatrix[(sourceChannels * 0) + S] = outputMatrix[(sourceChannels * 3) + S] = panLeft;
                            outputMatrix[(sourceChannels * 1) + S] = outputMatrix[(sourceChannels * 4) + S] = panRight;
                            break;

                        case Speakers.FivePointOne:
                        case Speakers.SevenPointOne:
                        case Speakers.FivePointOneSurround:
                            outputMatrix[(sourceChannels * 0) + S] = outputMatrix[(sourceChannels * 4) + S] = panLeft;
                            outputMatrix[(sourceChannels * 1) + S] = outputMatrix[(sourceChannels * 5) + S] = panRight;
                            break;

                        case Speakers.SevenPointOneSurround:
                            outputMatrix[(sourceChannels * 0) + S] = outputMatrix[(sourceChannels * 4) + S] = outputMatrix[(sourceChannels * 6) + S] = panLeft;
                            outputMatrix[(sourceChannels * 1) + S] = outputMatrix[(sourceChannels * 5) + S] = outputMatrix[(sourceChannels * 7) + S] = panRight;
                            break;

                        case Speakers.Mono:
                        default:
                            // don't do any panning here
                            break;
                    }
                }
            }

            voice.SetOutputMatrix(sourceChannels, destinationChannels, outputMatrix);
        }

        /// <summary>
        /// Returns this SoundEffectInstance to the SoundEffect InstancePool.
        /// You should not continue to call other functions on this object.
        /// </summary>
        public void Return()
        {
            ReleaseSourceVoice();
            Effect.AudioManager.InstancePool.Return(this);
        }
    }
}