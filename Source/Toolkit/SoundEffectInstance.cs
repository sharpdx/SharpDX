// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
//
// SoundEffectInstance originally created by Yuri Roubinsky for ShaprDX.

using System;
using SharpDX.XAudio2;

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Provides a single playing, paused, or stopped instance of a <see cref="SoundEffect"/> sound.
    /// </summary>
    class SoundEffectInstance
    {
        private int position;

        /// <summary>
        /// Get or sets the play position for the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public int Position 
        { 
            set
            { 
                position = value;
            } 
            get { return position; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether looping is enabled for the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        /// Gets or sets the volume of the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// Gets or sets the pitch adjustment for the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// Gets or sets the panning for the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public float Pan { get; set; }

        /// <summary>
        /// Gets source <see cref="SoundEffect"/>.
        /// </summary>
        public SoundEffect Origin { get; private set; }

        /// <summary>
        /// Gets the current state (playing, paused, or stopped) of the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public SoundState State { get; private set; }

        private SourceVoice sourceVoice;


        public SoundEffectInstance(SoundEffect soundEffect)
        {
            this.Origin = soundEffect;
            this.Volume = 1.0f;
            this.Pitch = 1.0f;
            this.Pan = 0.0f;
        } 

        /// <summary>
        /// Plays or resumes a <see cref="SoundEffectInstance"/>.
        /// </summary>
        public void Play()
        {
            if (sourceVoice == null)
            {
                sourceVoice = new SourceVoice(AudioEngine.Engine, Origin.Format, true);

                sourceVoice.BufferEnd += SourceVoiceOnBufferEnd;
                sourceVoice.BufferStart += sourceVoice_BufferStart;
                sourceVoice.SetVolume(Volume);
                sourceVoice.SetFrequencyRatio(Pitch);

                float left = 0.5f - (Pan/2.0f);
                float right = 0.5f + (Pan/2.0f);

                var outputMatrix = new float[2];
                outputMatrix[0] = left;
                outputMatrix[1] = right;
                sourceVoice.SetOutputMatrix(1, 2, outputMatrix);

                var buffer = new AudioBuffer
                {
                    Stream = Origin.Buffer.Stream,
                    AudioBytes = (int)Origin.Buffer.Stream.Length-Position,
                    Flags = BufferFlags.EndOfStream
                };

                buffer.AudioDataPointer = new IntPtr(buffer.AudioDataPointer.ToInt32() + Position);
                

                if (IsLooped) 
                {
                    buffer.LoopCount = 255; // infinity
                }
                
                sourceVoice.SubmitSourceBuffer(buffer, Origin.Stream.DecodedPacketsInfo);
                sourceVoice.Start();
            }
            else
            {
                sourceVoice.Start();
            }
        } 

        /// <summary>
        /// Resumes playback for a <see cref="SoundEffectInstance"/>.
        /// </summary>
        public void Resume()
        {
            sourceVoice.Start();
            State = SoundState.Playing;
        }

        /// <summary>
        /// Pauses a <see cref="SoundEffectInstance"/>.
        /// </summary>
        public void Pause()
        {
            sourceVoice.Stop(PlayFlags.None);
            State = SoundState.Paused;
        }
         
        /// <summary>
        /// Immediately stops playing a <see cref="SoundEffectInstance"/>.
        /// </summary>
        public void Stop()
        {
            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();
            sourceVoice = null;
        }

        private void sourceVoice_BufferStart(IntPtr ptr)
        { 
            AudioEngine.SoundEffectInstanceCount++;
            State = SoundState.Playing;
        }

        private void SourceVoiceOnBufferEnd(IntPtr ptr)
        {
            AudioEngine.SoundEffectInstanceCount--;
            State = SoundState.Stopped;
        }
    }
}
