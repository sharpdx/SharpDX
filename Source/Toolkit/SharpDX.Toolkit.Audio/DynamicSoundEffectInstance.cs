using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Provides properties, methods, and events for playback of the audio buffer.
    /// </summary>
    public sealed class DynamicSoundEffectInstance : SoundEffectInstance
    {
        readonly static string
            BUFFER      = "buffer"    ,
            MANAGER     = "manager"   ,
            SAMPLE_RATE = "sampleRate",
            CHANNELS    = "channels"  ,

            INVALID_RANGE    = "The buffer does not contain the given range of [offset, offset + count[.",
            NO_EFFECT        = "A DynamicSoundEffectInstance doesn't have a SoundEffect object.",
            NO_SOURCE_VOICE  = "Couldn't get a SourceVoice for the DynamicSoundEffectInstance.",
            BUFFERS_DISABLED = "Submitting audio buffers is disabled because Discontinuity() is called on the source voice.",
            TOO_MANY_BUFFERS = "Cannot queue more than 63 audio buffers for a DynamicSoundEffectInstance.";

        //static int instNum = 0;

        SoundState state = SoundState.Stopped;
        bool disabled    = false;

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
                return voice.State.BuffersQueued;// - 1;
            }
        }

        /// <summary>
        /// Gets the base sound effect.
        /// </summary>
        public override SoundEffect Effect
        {
            get
            {
                throw new InvalidOperationException(NO_EFFECT);
            }
            protected internal set
            {
                //throw new InvalidOperationException(NO_EFFECT);
            }
        }

        /// <summary>
        /// Gets the state of the current sound effect instance.
        /// </summary>
        public override SoundState State
        {
            get
            {
                return state;
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
            : base(//new SoundEffect(manager, "SharpDX.Toolkit.Audio.DSEI.__INST_" + instNum++,
            //       new WaveFormat(sampleRate, sizeof(ushort) * 8, (int)channels), DataStream.Create(new byte[8192], true, true), null)
                  null, GetVoice(manager, sampleRate, channels), isFireAndForget)
        {
            SampleRate = sampleRate;
            Channels   = channels  ;

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
        public        void SubmitBuffer(byte[] buffer)
        {
            SubmitBuffer(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Submits an audio buffer for playback. Playback begins at the specifed offset, and the byte count determines the size of the sample played.
        /// </summary>
        /// <param name="buffer">Buffer that contains the audio data. The audio format must be PCM wave data.</param>
        /// <param name="offset">Offset, in bytes, to the starting position of the data.</param>
        /// <param name="count">Amount, in bytes, of data sent.</param>
        public unsafe void SubmitBuffer(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(BUFFER);
            if (buffer.Length < offset + count)
                throw new ArgumentException(INVALID_RANGE, BUFFER);
            if (disabled)
                throw new InvalidOperationException(BUFFERS_DISABLED);
            if (PendingBufferCount >= 63)
                throw new InvalidOperationException(TOO_MANY_BUFFERS);

            fixed (byte* ptr = buffer)
            {
                voice.SubmitSourceBuffer(new AudioBuffer(new DataPointer(ptr + offset, count)), null);
            }
        }

        /// <summary>
        /// Removes all pending buffers from the <see cref="DynamicSoundEffectInstance" />.
        /// </summary>
        public void FlushPendingBuffers    ()
        {
            voice.FlushSourceBuffers();
        }
        /// <summary>
        /// Disables the usage of <see cref="SubmitBuffer(byte[], int, int)" /> and notifies the XAudio2 voice that no more buffers will be enqueued.
        /// </summary>
        public void DisableBufferSubmission()
        {
            voice.Discontinuity();

            disabled = true;
        }

        /// <summary>
        /// Plays the current instance. If it is already playing - the call is ignored.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        /// <exception cref="InvalidOperationException">There are no buffers queued. <see cref="BufferNeeded" /> will then be invoked before throwing the exception.</exception>
        public override void Play  ()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (state == SoundState.Playing)
                return;

            if (voice.State.BuffersQueued <= 0)
            {
                if (BufferNeeded != null)
                    BufferNeeded(this);

                if (voice.State.BuffersQueued <= 0)
                    throw new InvalidOperationException("Cannot play a DynamicSoundEffectInstance with no buffers.");
            }

            voice.Start();

            state  = SoundState.Playing;
            paused = false;
        }
        /// <summary>
        /// Resumes playback of the current instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public override void Resume()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (state == SoundState.Playing)
                return;
            if (state == SoundState.Stopped)
            {
                Play();
                return;
            }

            if (!IsLooped && voice.State.BuffersQueued == 0)
                voice.Stop();

            voice.Start();

            state  = SoundState.Playing;
            paused = false;
        }

        /// <summary>
        /// Pauses the playback of the current instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public override void Pause()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            voice.Stop();

            state  = SoundState.Paused;
            paused = true;
        }

        /// <summary>
        /// Stops the playback of the current instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public override void Stop()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (state == SoundState.Stopped)
                return;

            voice.Stop(0);
            voice.FlushSourceBuffers();

            state  = SoundState.Stopped;
            paused = false;
        }
        /// <summary>
        /// Stops the playback of the current instance indicating whether the stop should occur immediately of at the end of the sound.
        /// </summary>
        /// <param name="immediate">A value indicating whether the playback should be stopped immediately or at the end of the sound.</param>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public override void Stop(bool immediate)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (state == SoundState.Stopped)
                return;

            if (immediate)
                voice.Stop(0);
            else if (IsLooped)
                voice.ExitLoop();
            else
                voice.Stop((int)PlayFlags.Tails);

            state  = SoundState.Stopped;
            paused = false;
        }

        static SourceVoice GetVoice(AudioManager manager, int sampleRate, AudioChannels channels)
        {
            if (manager == null)
                throw new ArgumentNullException(MANAGER);
            if (sampleRate < 8000 || sampleRate > 48000)
                throw new ArgumentOutOfRangeException(SAMPLE_RATE);
            if (channels < AudioChannels.Mono)
                throw new ArgumentOutOfRangeException(CHANNELS);

            SourceVoicePool pool = manager.InstancePool.GetVoicePool(new WaveFormat(sampleRate, sizeof(ushort) * 8, (int)channels));

            SourceVoice ret = null;
            if (pool.TryAcquire(true, out ret))
                return ret;
            else
                throw new InvalidOperationException(NO_SOURCE_VOICE);
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
