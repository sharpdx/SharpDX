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
    using IO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Content;

    /// <summary>
    /// Represents a wave bank that were read from a stream.
    /// </summary>
    [ContentReader(typeof(WaveBankReader))]
    public sealed class WaveBank : IDisposable
    {
        private readonly Dictionary<string, SoundEffect> effectsByName;
        private SoundEffect[] effects;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveBank"/> class and loads the wave data from the provided stream.
        /// </summary>
        /// <param name="audioManager">The associated audio manager.</param>
        /// <param name="stream">The stream from which to read the wave data.</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="audioManager"/> or <paramref name="stream"/> are null.</exception>
        private WaveBank(AudioManager audioManager, Stream stream)
        {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            if (stream == null)
                throw new ArgumentNullException("stream");

            using (var reader = new WaveBankReader(stream))
            {
                effects = new SoundEffect[reader.Count];
                effectsByName = new Dictionary<string, SoundEffect>();

                for (uint i = 0; i < reader.Count; i++)
                {
                    var format = reader.GetWaveFormat(i);
                    var metadata = reader.GetMetadata(i); // why it is not used?
                    var name = reader.GetName(i);
                    var data = reader.GetWaveData(i);
                    uint[] decodedPacketsInfo = null;

                    if (format.Encoding == Multimedia.WaveFormatEncoding.Wmaudio2 || format.Encoding == Multimedia.WaveFormatEncoding.Wmaudio3)
                    {
                        Multimedia.WaveFormatEncoding tag;
                        decodedPacketsInfo = reader.GetSeekTable(i, out tag);
                    }

                    var buffer = DataStream.Create<byte>(data, true, false);

                    var effect = effects[i] = new SoundEffect(audioManager, name, format, buffer, decodedPacketsInfo);

                    if (!string.IsNullOrEmpty(name))
                        effectsByName.Add(name, effect);
                }
            }
        }

        /// <summary>
        /// Gets the number of sound effects stored in the current instance.
        /// </summary>
        public int Count { get { return effects == null ? 0 : effects.Length; } }

        /// <summary>
        /// Gets a value indicating whether the current instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Initializes a new wave bank from the file located at the provided file path.
        /// </summary>
        /// <param name="audioManager">The associated audio manager.</param>
        /// <param name="filePath">The path to the wave bank file.</param>
        /// <returns>The wave bank initialized from provided file.</returns>
        public static WaveBank FromFile(AudioManager audioManager, string filePath)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return new WaveBank(audioManager, stream);
        }

        /// <summary>
        /// Initializes a new wave bank from the provided stream.
        /// </summary>
        /// <param name="audioManager">The associated audio manager.</param>
        /// <param name="stream">The stream containing wave bank data.</param>
        /// <returns>The wave bank initialized from provided stream.</returns>
        public static WaveBank FromStream(AudioManager audioManager, Stream stream)
        {
            return audioManager.ToDisposeAudioAsset(new WaveBank(audioManager, stream));
        }

        /// <summary>
        /// Creates a new sound effect instance from the sound effect at the specified index in this wave bank instance.
        /// </summary>
        /// <param name="index">The index of the sound effect in the current instance.</param>
        /// <returns>The sound effect instance initialized from the sound effect at specified index.</returns>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="index"/> is lesser than zero or greather than or equal to the effects count.</exception>
        public SoundEffectInstance Create(int index)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (index < 0 || index >= effects.Length)
                throw new ArgumentOutOfRangeException("index", string.Format("No wave at index '{0}' exists.", index));

            return effects[index].Create();
        }

        /// <summary>
        /// Creates a new sound effect instance from the sound effect with the specified name in this wave bank instance.
        /// </summary>
        /// <param name="name">The name of the sound effect in the current instance.</param>
        /// <returns>The sound effect instance initialized from sound effect with the specified name.</returns>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="name"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when the sound effect with the specified <paramref name="name"/> is not found.</exception>
        public SoundEffectInstance Create(string name)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The name cannot be null or empty.");

            SoundEffect soundEffect = null;
            if (effectsByName.TryGetValue(name, out soundEffect))
            {
                return soundEffect.Create();
            }

            throw new ArgumentOutOfRangeException("name", string.Format("No wave with name '{0}' exists.", name));
        }

        /// <summary>
        /// Disposes the current instance and releases all associated unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                lock (effectsByName)
                {
                    for (int i = 0; i < effects.Length; i++)
                    {
                        effects[i].Dispose();
                    }

                    effects = null;

                    effectsByName.Clear();
                }
            }
        }

        /// <summary>
        /// Plays the sound effect at specified index with provided parameters.
        /// </summary>
        /// <param name="index">The sound effect index.</param>
        /// <param name="volume">The volume of the sound effect instance.</param>
        /// <param name="pitch">The pitch of the sound effect instance.</param>
        /// <param name="pan">The pan of the sound effect instance.</param>
        /// <returns>true if the sound effect instance was scheduled successfuly for playback, false - otherwise.</returns>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="index"/> is lesser than zero or greather than or equal to the effects count.</exception>
        public bool Play(int index, float volume, float pitch, float pan)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (index < 0 || index >= effects.Length)
                throw new ArgumentOutOfRangeException("index", string.Format("No wave at index '{0}' exists.", index));

            return effects[index].Play(volume, pitch, pan);
        }

        /// <summary>
        /// Plays the sound effect at specified index.
        /// </summary>
        /// <param name="index">The sound effect index.</param>
        /// <returns>true if the sound effect instance was scheduled successfuly for playback, false - otherwise.</returns>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="index"/> is lesser than zero or greather than or equal to the effects count.</exception>
        public bool Play(int index)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (index < 0 || index >= effects.Length)
                throw new ArgumentOutOfRangeException("index", string.Format("No wave at index '{0}' exists.", index));

            return effects[index].Play();
        }

        /// <summary>
        /// Plays the sound effect with the specified name with provided parameters.
        /// </summary>
        /// <param name="name">The sound effect name.</param>
        /// <param name="volume">The volume of the sound effect instance.</param>
        /// <param name="pitch">The pitch of the sound effect instance.</param>
        /// <param name="pan">The pan of the sound effect instance.</param>
        /// <returns>true if the sound effect instance was scheduled successfuly for playback, false - otherwise.</returns>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when the sound effect with the specified <paramref name="name"/> is not found.</exception>
        public bool Play(string name, float volume, float pitch, float pan)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            SoundEffect soundEffect;
            if (effectsByName.TryGetValue(name, out soundEffect))
                return soundEffect.Play(volume, pitch, pan);

            throw new ArgumentOutOfRangeException("name", string.Format("No wave with name '{0}' exists.", name));
        }

        /// <summary>
        /// Plays the sound effect with the specified name.
        /// </summary>
        /// <param name="name">The sound effect name.</param>
        /// <returns>true if the sound effect instance was scheduled successfuly for playback, false - otherwise.</returns>
        /// <exception cref="ObjectDisposedException">Is thrown if the current instance was already disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when the sound effect with the specified <paramref name="name"/> is not found.</exception>
        public bool Play(string name)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The name cannot be null or empty.");

            SoundEffect soundEffect;
            if (effectsByName.TryGetValue(name, out soundEffect))
                return soundEffect.Play();

            throw new ArgumentOutOfRangeException("name", string.Format("No wave with name '{0}' exists.", name));
        }
    }
}