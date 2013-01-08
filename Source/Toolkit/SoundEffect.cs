using System;
using System.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Provides a loaded sound resource.
    /// </summary>
    class SoundEffect : IDisposable
    {
        private TimeSpan duration;

        public AudioBuffer Buffer { get; private set; }
        public SoundStream Stream { get; private set; }
        public WaveFormat Format { get; private set; }
         
        /// <summary>
        /// Gets the duration of the <see cref="SoundEffect"/>.
        /// </summary>
        public TimeSpan Duration { get { return this.duration; } }

        /// <summary>
        /// Returns the speed of sound: 343.5 meters per second.
        /// </summary>
        public static readonly float SpeedOfSound = 343.5f;

        /// <summary>
        /// Closed constructor.
        /// </summary>
        private SoundEffect()
        {
            
        }
         
        /// <summary>
        /// Load sound from specified file on disk.
        /// </summary>
        /// <param name="filename">Full path to file.</param>
        /// <returns>A new <see cref="SoundEffect"/> created from sound file.</returns>
        public static SoundEffect FromFile(string filename)
        {
            var sound = new SoundEffect(); 
            sound.Stream = new SoundStream(File.OpenRead(filename));
            sound.Format = sound.Stream.Format;
            sound.Buffer = new AudioBuffer
            {
                Stream = sound.Stream.ToDataStream(),
                AudioBytes = (int)sound.Stream.Length,
                Flags = BufferFlags.EndOfStream
            };
            sound.Stream.Close();

            sound.duration = AudioHelper.GetSampleDuration((int) sound.Stream.Length, sound.Format.SampleRate,
                                                           sound.Format.Channels);

            return sound;
        }

        /// <summary>
        /// Creates a new <see cref="SoundEffectInstance"/> for this SoundEffect.
        /// </summary>
        /// <returns>A new <see cref="SoundEffectInstance"/> for this SoundEffect.</returns>
        public SoundEffectInstance CreateInstance()
        {
            return new SoundEffectInstance(this);
        }

        /// <summary>
        /// Plays a sound based on specified volume, pitch, and panning.
        /// </summary>
        /// <param name="volume">Volume, ranging from 0.0f (silence) to 1.0f (full volume). 1.0f is full volume relative to SoundEffect.MasterVolume.</param>
        /// <param name="pitch">Pitch adjustment, ranging from -1.0f (down one octave) to 1.0f (up one octave). 0.0f is unity (normal) pitch.</param>
        /// <param name="pan">Panning, ranging from -1.0f (full left) to 1.0f (full right). 0.0f is centered.</param>
        public void Play(float volume,float pitch,float pan)
        {
            var instance = new SoundEffectInstance(this);
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.Pan = pan;
            instance.Play();
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        public void Play()
        {
            var instance = new SoundEffectInstance(this); 
            instance.Play();
        }

        /// <summary>
        /// Releases the resources used by the <see cref="SoundEffect"/>.
        /// </summary>
        public void Dispose()
        {
             if (Buffer != null)
             { 
                 Buffer.Stream.Dispose();
             }
        }
    }
}
