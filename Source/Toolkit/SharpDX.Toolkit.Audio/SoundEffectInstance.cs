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


namespace SharpDX.Toolkit.Audio
{
    using SharpDX.Multimedia;
    using SharpDX.XAudio2;
    using SharpDX.X3DAudio;

    /// <summary>
    /// Provides a single playing, paused, or stopped instance of a <see cref="SoundEffect"/> sound.
    /// </summary>
    public sealed class SoundEffectInstance: IDisposable
    {        
        private SourceVoice voice;
        private bool paused;
        private float volume;
        private float pan;
        private float pitch;
        private float[] outputMatrix;
        private Emitter emitter;
        private Listener listener;
        private DspSettings dspSettings;

        internal SoundEffectInstance(SoundEffect soundEffect, SourceVoice sourceVoice, bool isFireAndForget)
        {           
            if (soundEffect == null)
                throw new ArgumentNullException("soundEffect");

            if (sourceVoice == null)
                throw new ArgumentNullException("sourceVoice");

            voice = sourceVoice;  
            Effect = soundEffect;
            IsFireAndForget = isFireAndForget;
            paused = false;
            IsLooped = false;
            volume = 1.0f;
            pan = 0.0f;
            pitch = 0.0f;
            outputMatrix = null;
        }


        public SoundEffect Effect { get; private set; }

        internal bool IsFireAndForget { get; set; }

        private AudioBuffer CurrentAudioBuffer
        {
            get
            {
                if(Effect == null || Effect.AudioBuffer == null)
                    return null;

                return IsLooped ? Effect.LoopedAudioBuffer : Effect.AudioBuffer;
            }
        }

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

                if (value == volume)
                    return;

                volume = MathUtil.Clamp(value, 0.0f, 1.0f);

                voice.SetVolume(volume);
            }
        }
        

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

                if (value == pitch)
                    return;

                pitch = MathUtil.Clamp(value, -1.0f, 1.0f);

                voice.SetFrequencyRatio(XAudio2.SemitonesToFrequencyRatio(pitch));
            }
        }


        public float Pan
        {
            get
            {
                return pan;
            }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (value == pan)
                    return;
                
                pan = MathUtil.Clamp(value, -1.0f, 1.0f);

                SetPanOutputMatrix();
            }
        }


        public bool IsLooped { get; set; }


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


        public void Pause()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            voice.Stop();
            paused = true;
        }

        
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


        public void Stop()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            voice.Stop(0);
            voice.FlushSourceBuffers();

            paused = false;
        }

        
        public void Stop(bool immediate)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if(immediate)
            {
                voice.Stop(0);
            }
            else if(IsLooped)
            {
                voice.ExitLoop();
            }
            else
            {
                voice.Stop((int)PlayFlags.Tails);
            }

            paused = false;
        }


        public void Reset()
        {
            Volume = 1.0f;
            Pitch = 0.0f;
            Pan = 0.0f;
            IsLooped = false;
        }

        internal void Reset(SourceVoice sourceVoice)
        {
            voice = sourceVoice;
            Reset();
        }

        public void Apply3D(Matrix listener, Vector3 listenerVelocity, Matrix emitter, Vector3 emitterVelocity)
        {
            Apply3D(listener.Forward, listener.Up, listener.TranslationVector, listenerVelocity, emitter.Forward, emitter.Up, emitter.TranslationVector, emitterVelocity);         
            
        }


        // TODO: X3DAudio uses a left-handed Cartesian coordinate system. may need overloads for lh/rh.  seems to work with right hand matricies without it though.
        //public void Apply3DRH(Matrix listenerTransform, Vector3 listenerVelocity, Matrix emitterTransform, Vector3 emitterVelocity)
        //{
        //    //  X3DAudio uses a left-handed Cartesian coordinate system, needs to be converted

        //    var listenerForward = listenerTransform.Forward;
        //    var listenerUp = listenerTransform.Up;
        //    var listenerPosition = listenerTransform.TranslationVector;
            
            
        //    var emitterForward = emitterTransform.Forward;
        //    var emitterUp = emitterTransform.Up;
        //    var emitterPosition = emitterTransform.TranslationVector;
            
            
        //    Apply3D(listenerForward, listenerUp, listenerPosition, listenerVelocity, emitterForward, emitterUp, emitterPosition, emitterVelocity);

        //}


        public void Apply2D(Vector2 listener, Vector2 listenerVelocity, Vector2 emitter, Vector2 emitterVelocity)
        {
            Apply3D(Vector3.ForwardLH, Vector3.Up, new Vector3(listener, 0), new Vector3(listenerVelocity, 0), Vector3.ForwardLH, Vector3.Up, new Vector3(emitter, 0), new Vector3(emitterVelocity, 0));

        }


        private void Apply3D(Vector3 listenerForward, Vector3 listenerUp, Vector3 listenerPosition, Vector3 listenerVelocity, Vector3 emitterForward, Vector3 emitterUp, Vector3 emitterPosition, Vector3 emitterVelocity)
        {
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

            Effect.AudioManager.Calculate3D(listener, emitter, CalculateFlags.Matrix | CalculateFlags.Doppler, dspSettings);

            voice.SetOutputMatrix(dspSettings.SourceChannelCount, dspSettings.DestinationChannelCount, dspSettings.MatrixCoefficients);
            voice.SetFrequencyRatio(dspSettings.DopplerFactor);
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

        private void InitializeOutputMatrix(out int destinationChannels, out int sourceChannels)
        {
            destinationChannels = Effect.AudioManager.MasteringVoice.VoiceDetails.InputChannelCount;
            sourceChannels = Effect.Format.Channels;

            var outputMatrixSize = destinationChannels * sourceChannels;

            if (outputMatrix == null || outputMatrix.Length < outputMatrixSize)
                outputMatrix = new float[outputMatrixSize];

            // Default to full volume for all channels/destinations   
            for (var i = 0; i < outputMatrix.Length; i++)
                outputMatrix[i] = 1.0f;
        }


        public bool IsDisposed { get; private set; }


        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Effect.ChildDisposed(this);
                DestroyVoice();
                Effect = null;
                outputMatrix = null;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }


        internal void ParentDisposed()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                DestroyVoice();
                Effect = null; 
                outputMatrix = null;
            }
        }


        private void DestroyVoice()
        {            
            if (voice != null)
            {
                if (!IsFireAndForget)
                {
                    if (Effect.AudioManager.Device != null)
                    {
                        voice.DestroyVoice();
                    }
                    voice.Dispose();
                }
                voice = null;
            }
        }

    }
}
