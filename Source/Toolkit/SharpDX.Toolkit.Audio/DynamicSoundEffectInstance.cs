using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Multimedia;
using SharpDX.XAPO;
using SharpDX.XAudio2;

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Provides properties, methods, and events for playback of the audio buffer.
    /// </summary>
    public sealed class DynamicSoundEffectInstance : SoundEffectInstance
    {
        static int instNum = 0;

        /// <summary>
        /// Gets the sample rate of the <see cref="DynamicSoundEffectInstance" />.
        /// </summary>
        public int           SampleRate
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the audio channels the <see cref="DynamicSoundEffectInstance" /> has.
        /// </summary>
        public AudioChannels Channels
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the number of audio buffers in the queue awaiting playback.
        /// </summary>
        public int PendingBufferCount
        {
            get
            {
                return voice.State.BuffersQueued - 1;
            }
        }

        /// <summary>
        /// Event that occurs when the number of audio capture buffers awaiting playback is less than or equal to two.
        /// </summary>
        public event Action<DynamicSoundEffectInstance> BufferNeeded;

        /// <summary>
        /// Initializes a new instance of this class, which creates a dynamic sound effect based on the specified sample rate and audio channel.
        /// </summary>
        /// <param name="manager">The <see cref="AudioManager" /> that manages the sound effect.</param>
        /// <param name="sampleRate">Sample rate, in Hertz (Hz), of audio content.</param>
        /// <param name="channels">Number of channels in the audio data.</param>
        /// <param name="isFireAndForget">A value indicating whether this instance is not monitored after it is being send to playback.</param>
        public DynamicSoundEffectInstance(AudioManager manager, int sampleRate, AudioChannels channels, bool isFireAndForget)
            : base(null, GetVoice(manager, sampleRate, channels), isFireAndForget)
        {
            Effect = new SoundEffect(manager, "SharpDX.Toolkit.Audio.DSEI.__INST_" + instNum,
                new WaveFormat(sampleRate, sizeof(ushort) * 8, (int)channels), DataStream.Create(new byte[8192], true, true), null);

            voice.BufferEnd += p =>
            {
                if (PendingBufferCount < 3 && BufferNeeded != null)
                    BufferNeeded(this);
            };
        }
        /// <summary>
        /// Initializes a new instance of this class, which creates a dynamic sound effect based on the specified sample rate and audio channel.
        /// </summary>
        /// <param name="manager">The <see cref="AudioManager" /> that manages the sound effect.</param>
        /// <param name="sampleRate">Sample rate, in Hertz (Hz), of audio content.</param>
        /// <param name="channels">Number of channels in the audio data.</param>
        public DynamicSoundEffectInstance(AudioManager manager, int sampleRate, AudioChannels channels)
            : this(manager, sampleRate, channels, true)
        {

        }

        /// <summary>
        /// Submits an audio buffer for playback. Playback starts at the beginning, and the buffer is played in its entirety.
        /// </summary>
        /// <param name="buffer">Buffer that contains the audio data. The audio format must be PCM wave data.</param>
        public void SubmitBuffer(byte[] buffer)
        {
            SubmitBuffer(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Submits an audio buffer for playback. Playback begins at the specifed offset, and the byte count determines the size of the sample played.
        /// </summary>
        /// <param name="buffer">Buffer that contains the audio data. The audio format must be PCM wave data.</param>
        /// <param name="offset">Offset, in bytes, to the starting position of the data.</param>
        /// <param name="count">Amount, in bytes, of data sent.</param>
        public void SubmitBuffer(byte[] buffer, int offset, int count)
        {
            if (PendingBufferCount >= 63)
                throw new InvalidOperationException("There cannot be more than 63 buffers pending in the DynamicSoundEffectInstance.");

            if (CurrentAudioBuffer.Stream.RemainingLength < count)
            {
                long newSize = CurrentAudioBuffer.Stream.Length * 2;
                while (newSize < count)
                    newSize *= 2;

                DataStream stream = new DataStream((int)newSize, true, true);

                CurrentAudioBuffer.Stream.CopyTo(stream);

                CurrentAudioBuffer.Stream = stream;
            }

            CurrentAudioBuffer.Stream.Write(buffer, offset, count);
        }

        static SourceVoice GetVoice(AudioManager manager, int sampleRate, AudioChannels channels)
        {
            SourceVoicePool pool = manager.InstancePool.GetVoicePool(new WaveFormat(sampleRate, sizeof(ushort) * 8, (int)channels));

            SourceVoice ret = null;
            if (pool.TryAcquire(true, out ret))
                return ret;
            else
                throw new InvalidOperationException("Couldn't get a SourceVoice for the DynamicSoundEffectInstance.");
        }

        protected internal override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                ReleaseSourceVoice();
                //Effect.ChildDisposed(this);
                //Effect = null;
                outputMatrix = null;
            }

            SampleRate = 0;
            Channels   = 0;
        }

        internal override void Reset(SoundEffect soundEffect, SourceVoice sourceVoice, bool isFireAndForget)
        {
            base.Reset(soundEffect, sourceVoice, isFireAndForget);

            SampleRate = 0;
            Channels   = 0;
        }

        /// <summary>
        /// Returns this <see cref="SoundEffectInstance" /> to the <see cref="SoundEffect" /> InstancePool.
        /// You should not continue to call other functions on this object.
        /// </summary>
        public override void Return()
        {
            base.Return();

            SampleRate = 0;
            Channels   = 0;
        }
    }
}
