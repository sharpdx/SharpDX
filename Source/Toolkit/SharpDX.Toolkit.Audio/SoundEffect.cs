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
    using System.Collections.Generic;
    using IO;
    using Multimedia;
    using XAudio2;
    using System.IO;
    using Content;

    /// <summary>
    /// Represents a loaded sound resource.
    /// </summary>
    [ContentReader(typeof(SoundEffectContentReader))]
    public sealed class SoundEffect : IDisposable
    {
        private static float distanceScale = 1f;
        private static float dopplerScale = 1f;
        private readonly List<WeakReference> children;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffect"/> class.
        /// </summary>
        /// <param name="audioManager">The associated audio manager instance.</param>
        /// <param name="name">The name of the current instance.</param>
        /// <param name="waveFormat">The format of the current instance.</param>
        /// <param name="buffer">The buffer containing audio data.</param>
        /// <param name="decodedPacketsInfo">The information regaring decoded packets.</param>
        internal SoundEffect(AudioManager audioManager, string name, WaveFormat waveFormat, DataStream buffer, uint[] decodedPacketsInfo)
        {
            AudioManager = audioManager;
            Name = name;
            Format = waveFormat;
            AudioBuffer = new AudioBuffer
            {
                Stream = buffer,
                AudioBytes = (int)buffer.Length,
                Flags = BufferFlags.EndOfStream,
            };
            LoopedAudioBuffer = new AudioBuffer
            {
                Stream = buffer,
                AudioBytes = (int)buffer.Length,
                Flags = BufferFlags.EndOfStream,
                LoopCount = AudioBuffer.LoopInfinite,
            };

            DecodedPacketsInfo = decodedPacketsInfo;

            Duration = Format.SampleRate > 0 ? TimeSpan.FromMilliseconds(GetSamplesDuration() * 1000 / Format.SampleRate) : TimeSpan.Zero;

            children = new List<WeakReference>();
            VoicePool = AudioManager.InstancePool.GetVoicePool(Format);
        }

        /// <summary>
        /// Gets or sets the distance scaling ratio. Default is 1f.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when there is an attempt to set the value less than zero.</exception>
        public static float DistanceScale
        {
            get
            {
                return distanceScale;
            }
            set
            {
                if (value <= 0f)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                distanceScale = value;
            }
        }

        /// <summary>
        /// Gets or sets the Doppler effect scale ratio. Default is 1f.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when there is an attempt to set the value less than zero.</exception>
        public static float DopplerScale
        {
            get
            {
                return dopplerScale;
            }
            set
            {
                if (value < 0f)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                dopplerScale = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="AudioManager"/> instance associated to this effect.
        /// </summary>
        public AudioManager AudioManager { get; private set; }

        /// <summary>
        /// Gets the duration of this effect.
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the name of the current instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the audio buffer of this instance.
        /// </summary>
        internal AudioBuffer AudioBuffer { get; private set; }

        /// <summary>
        /// Gets the information about decoded packets of this instance.
        /// </summary>
        internal uint[] DecodedPacketsInfo { get; private set; }

        /// <summary>
        /// Gets the wave format of this instance.
        /// </summary>
        internal WaveFormat Format { get; private set; }

        /// <summary>
        /// Gets the audio buffer of the looped version of this instance.
        /// </summary>
        internal AudioBuffer LoopedAudioBuffer { get; private set; }

        /// <summary>
        /// Gets the voice pool used by this instance.
        /// </summary>
        internal SourceVoicePool VoicePool { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SoundEffect"/> class from the specified file path.
        /// </summary>
        /// <param name="audioManager">The audio manager associated to the created instance.</param>
        /// <param name="filePath">The path to the file from which to create the effect.</param>
        /// <returns>The created effect.</returns>
        public static SoundEffect FromFile(AudioManager audioManager, string filePath)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return FromStream(audioManager, stream, Path.GetFileNameWithoutExtension(filePath));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SoundEffect"/> class from the spefified data stream.
        /// </summary>
        /// <param name="audioManager">The audio manager associated to the created instance.</param>
        /// <param name="stream">The stream containing the data from which to create the effect.</param>
        /// <param name="name">The name of the effect (optional).</param>
        /// <returns>The created effect.</returns>
        public static SoundEffect FromStream(AudioManager audioManager, Stream stream, string name = null)
        {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            if (stream == null)
                throw new ArgumentNullException("stream");

            var sound = new SoundStream(stream);
            var format = sound.Format;
            var decodedPacketsInfo = sound.DecodedPacketsInfo;
            var buffer = sound.ToDataStream();

            sound.Dispose();

            return audioManager.ToDisposeAudioAsset(new SoundEffect(audioManager, name, format, buffer, decodedPacketsInfo));
        }

        /// <summary>
        /// Creates a <see cref="SoundEffectInstance"/> from the current sound effect.
        /// </summary>
        /// <returns></returns>
        public SoundEffectInstance Create()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            SoundEffectInstance instance = null;
            if (AudioManager.InstancePool.TryAcquire(this, false, out instance))
            {
                AddChild(instance);
                return instance;
            }

            throw new InvalidOperationException("Unable to create SoundEffectInstance, insufficient source voices available.");
        }

        /// <summary>
        /// Releases all unmanaged resources used by the current instance.
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                lock (children)
                {
                    foreach (var item in children)
                    {
                        SoundEffectInstance soundEffectInstance = item.Target as SoundEffectInstance;
                        if (soundEffectInstance != null)
                        {
                            soundEffectInstance.ParentDisposed();
                        }
                    }
                    children.Clear();
                    VoicePool.Release();
                    VoicePool = null;
                }

                AudioBuffer.Stream.Dispose();
                AudioBuffer = null;
                LoopedAudioBuffer = null;
            }
        }

        /// <summary>
        /// Plays the current sound effect instance.
        /// </summary>
        /// <param name="volume">The volume of the sound.</param>
        /// <param name="pitch">The pitch of the sound.</param>
        /// <param name="pan">The pan of the sound.</param>
        /// <returns>true if the effect was successfuly queued for playback, false - otherwise.</returns>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        public bool Play(float volume, float pitch, float pan)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            SoundEffectInstance instance = null;
            if (AudioManager.InstancePool.TryAcquire(this, true, out instance))
            {
                instance.Volume = volume;
                instance.Pitch = pitch;
                instance.Pan = pan;
                instance.Play();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Plays the current sound effect instance with default volume, pitch and pan parameters.
        /// </summary>
        /// <returns></returns>
        public bool Play()
        {
            return Play(1.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Handles the disposal event of a child <see cref="SoundEffectInstance"/>.
        /// </summary>
        /// <param name="child">The child instance that is being disposed.</param>
        internal void ChildDisposed(SoundEffectInstance child)
        {
            lock (children)
            {
                for (var i = 0; i < children.Count; i++)
                {
                    var weakReference = children[i];
                    var soundEffectInstance = weakReference.Target as SoundEffectInstance;
                    if (!weakReference.IsAlive || soundEffectInstance == null || soundEffectInstance == child)
                    {
                        children.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified instance to the children list.
        /// </summary>
        /// <param name="instance">The instance to add to children list.</param>
        private void AddChild(SoundEffectInstance instance)
        {
            lock (children)
            {
                children.Add(new WeakReference(instance));
            }
        }

        /// <summary>
        /// Gets the wave samples duration.
        /// </summary>
        /// <returns>Wave samples duration or 0 (zero) if the format encoding is not known.</returns>
        private long GetSamplesDuration()
        {
            switch (Format.Encoding)
            {
                case WaveFormatEncoding.Adpcm:
                    var adpcmFormat = Format as WaveFormatAdpcm;
                    long duration = (AudioBuffer.AudioBytes / adpcmFormat.BlockAlign) * adpcmFormat.SamplesPerBlock;
                    long partial = AudioBuffer.AudioBytes % adpcmFormat.BlockAlign;
                    if (partial > 0)
                    {
                        if (partial >= (7 * adpcmFormat.Channels))
                            duration += (partial * 2) / (adpcmFormat.Channels - 12);
                    }

                    return duration;

                case WaveFormatEncoding.Wmaudio2:
                case WaveFormatEncoding.Wmaudio3:
                    if (DecodedPacketsInfo != null)
                    {
                        return DecodedPacketsInfo[DecodedPacketsInfo.Length - 1] / Format.Channels;
                    }
                    break;

                case WaveFormatEncoding.Pcm:
                    if (Format.BitsPerSample > 0)
                    {
                        return ((long)AudioBuffer.AudioBytes) * 8 / (Format.BitsPerSample * Format.Channels);
                    }
                    break;
            }

            return 0;
        }
    }
}