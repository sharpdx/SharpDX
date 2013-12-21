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

    public sealed class SoundEffectInstance
    {
        private AudioBuffer audioBuffer;
        private SourceVoice voice;
        private bool paused;
        private float volume;
        private float pan;
        private float pitch;
        private float[] panOutputMatrix;


        internal SoundEffectInstance(SoundEffect soundEffect, bool isFireAndForget)
        {           
            if (soundEffect == null)
                throw new ArgumentNullException("effect");

            this.voice = new SourceVoice(soundEffect.Manager.Device, soundEffect.Format, VoiceFlags.None, XAudio2.MaximumFrequencyRatio);
            this.audioBuffer = new AudioBuffer
            {
                Stream = soundEffect.AudioBuffer,
                AudioBytes = (int)soundEffect.AudioBuffer.Length,
                Flags = BufferFlags.EndOfStream,
            };

            this.Effect = soundEffect;
            this.IsFireAndForget = isFireAndForget;
            this.volume = 1.0f;
            this.pan = 0.0f;
            this.pitch = 0.0f;
            this.panOutputMatrix = null;
        }


        public SoundEffect Effect { get; internal set; }

        internal bool IsFireAndForget { get; set; }


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

                if (value < -1f || value > 1f)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                pan = MathUtil.Clamp(value, -1.0f, 1.0f);

                SetPanOutputMatrix();
            }
        }


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

            if (this.State == SoundState.Playing)
                return;

            if (voice.State.BuffersQueued > 0)
            {
                voice.Stop();
                voice.FlushSourceBuffers();
            }

            voice.SubmitSourceBuffer(audioBuffer, Effect.DecodedPacketsInfo);
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

            if (voice.State.BuffersQueued == 0)
            {
                voice.Stop();
                voice.FlushSourceBuffers();
                voice.SubmitSourceBuffer(audioBuffer, Effect.DecodedPacketsInfo);
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

            voice.Stop(immediate ? 0 : (int)PlayFlags.Tails);

            paused = false;
        }


        private void SetPanOutputMatrix()
        {
            var destinationChannels = this.Effect.Manager.MasteringVoice.VoiceDetails.InputChannelCount;
            var sourceChannels = this.Effect.Format.Channels;

            var outputMatrixSize = destinationChannels * sourceChannels;

            if (panOutputMatrix == null || panOutputMatrix.Length < outputMatrixSize)
                panOutputMatrix = new float[outputMatrixSize];

            // Default to full volume for all channels/destinations   
            for (var i = 0; i < panOutputMatrix.Length; i++)
                panOutputMatrix[i] = 1.0f;

            if (pan != 0.0f)
            {

                var panLeft = 1.0f - pan;
                var panRight = 1.0f + pan;


                //The level sent from source channel S to destination channel D 
                //is specified in the form pLevelMatrix[SourceChannels × D + S]
                for (int S = 0; S < sourceChannels; S++)
                {
                    switch (this.Effect.Manager.Speakers)
                    {
                        case Speakers.Stereo:
                        case Speakers.TwoPointOne:
                        case Speakers.Surround:
                            panOutputMatrix[(sourceChannels * 0) + S] = panLeft;
                            panOutputMatrix[(sourceChannels * 1) + S] = panRight;
                            break;

                        case Speakers.Quad:
                            panOutputMatrix[(sourceChannels * 0) + S] = panOutputMatrix[(sourceChannels * 2) + S] = panLeft;
                            panOutputMatrix[(sourceChannels * 1) + S] = panOutputMatrix[(sourceChannels * 3) + S] = panRight;
                            break;

                        case Speakers.FourPointOne:
                            panOutputMatrix[(sourceChannels * 0) + S] = panOutputMatrix[(sourceChannels * 3) + S] = panLeft;
                            panOutputMatrix[(sourceChannels * 1) + S] = panOutputMatrix[(sourceChannels * 4) + S] = panRight;
                            break;

                        case Speakers.FivePointOne:
                        case Speakers.SevenPointOne:
                        case Speakers.FivePointOneSurround:
                            panOutputMatrix[(sourceChannels * 0) + S] = panOutputMatrix[(sourceChannels * 4) + S] = panLeft;
                            panOutputMatrix[(sourceChannels * 1) + S] = panOutputMatrix[(sourceChannels * 5) + S] = panRight;
                            break;

                        case Speakers.SevenPointOneSurround:
                            panOutputMatrix[(sourceChannels * 0) + S] = panOutputMatrix[(sourceChannels * 4) + S] = panOutputMatrix[(sourceChannels * 6) + S] = panLeft;
                            panOutputMatrix[(sourceChannels * 1) + S] = panOutputMatrix[(sourceChannels * 5) + S] = panOutputMatrix[(sourceChannels * 7) + S] = panRight;
                            break;

                        case Speakers.Mono:
                        default:
                            // don't do any panning here   
                            break;
                    }
                }
            }

            voice.SetOutputMatrix(sourceChannels, destinationChannels, panOutputMatrix);
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
                panOutputMatrix = null;
                audioBuffer = null;
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
                panOutputMatrix = null;

                audioBuffer = null;
            }
        }


        private void DestroyVoice()
        {
            if (voice != null)
            {
                if (Effect.Manager.Device != null)
                {
                    voice.DestroyVoice();
                }
                voice.Dispose();
                voice = null;
            }
        }

    }
}
