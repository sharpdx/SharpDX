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
    using Multimedia;
    using XAudio2;

    /// <summary>
    /// Pool of <see cref="SoundEffectInstance"/> used to maintain fire and forget instances.
    /// </summary>
    internal class SoundEffectInstancePool : IDisposable
    {
        private readonly InstancePool instancePool;
        private readonly Dictionary<uint, SourceVoicePool> sharedVoicePools;
        private readonly List<SourceVoicePool> unsharedVoicePools;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffectInstancePool"/> class.
        /// </summary>
        /// <param name="audioManager">The associated audio manager instance.</param>
        public SoundEffectInstancePool(AudioManager audioManager)
        {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            AudioManager = audioManager;

            sharedVoicePools = new Dictionary<uint, SourceVoicePool>();
            unsharedVoicePools = new List<SourceVoicePool>();
            instancePool = new InstancePool();
        }

        /// <summary>
        /// Gets a value indicating whether this instance was already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the associated audio manager intance.
        /// </summary>
        internal AudioManager AudioManager { get; private set; }

        /// <summary>
        /// Disposes the current instance and releases all associated unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Gets the <see cref="SourceVoicePool"/> for the specified wave format.
        /// </summary>
        /// <param name="waveFormat">The wave format of the requested source voice pool.</param>
        /// <returns>The source voice pool for the provided wave format.</returns>
        public SourceVoicePool GetVoicePool(WaveFormat waveFormat)
        {
            lock (sharedVoicePools)
            {
                var waveKey = MakeWaveFormatKey(waveFormat);
                SourceVoicePool pool;

                if (waveKey == 0)
                {
                    pool = new SourceVoicePool(this, waveFormat, false);
                    unsharedVoicePools.Add(pool);
                }
                else
                {
                    if (!sharedVoicePools.TryGetValue(waveKey, out pool))
                        sharedVoicePools[waveKey] = pool = new SourceVoicePool(this, waveFormat, true);
                }

                return pool;
            }
        }

        /// <summary>
        /// Tries to acquire an existing or to create a new instance of the <see cref="SoundEffectInstance"/> class.
        /// </summary>
        /// <param name="soundEffect">The parenet sound effect.</param>
        /// <param name="isFireAndForget">A value indicating whether the instance doesn't need to be monitored for playback.</param>
        /// <param name="instance">The acquired instance.</param>
        /// <returns>true if operation succeeded, false - otherwise.</returns>
        public bool TryAcquire(SoundEffect soundEffect, bool isFireAndForget, out SoundEffectInstance instance)
        {
            SourceVoice voice = null;
            if (soundEffect.VoicePool.TryAcquire(isFireAndForget, out voice))
            {
                if (instancePool.TryAcquire(isFireAndForget, out instance))
                {
                    instance.Reset(soundEffect, voice, isFireAndForget);
                    return true;
                }
            }

            instance = null;
            return false;
        }

        /// <summary>
        /// Removes the specified pool from "unshared" list.
        /// </summary>
        /// <param name="pool">The pool to remove.</param>
        internal void RemoveUnshared(SourceVoicePool pool)
        {
            lock (sharedVoicePools)
            {
                unsharedVoicePools.Remove(pool);
            }
        }

        /// <summary>
        /// Returns the speicfied SoundEffectInstance to the instance pool
        /// </summary>
        /// <param name="item">SFXInstance to return to instance pool</param>
        internal void Return(SoundEffectInstance item)
        {
            instancePool.Return(item);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                lock (sharedVoicePools)
                {
                    foreach (var pool in sharedVoicePools.Values)
                    {
                        pool.Dispose();
                    }

                    foreach (var pool in unsharedVoicePools)
                    {
                        pool.Dispose();
                    }

                    sharedVoicePools.Clear();
                    unsharedVoicePools.Clear();
                    instancePool.Clear();
                }
            }
        }

        /// <summary>
        /// Creates a key based on wave format.
        /// </summary>
        /// <param name="waveFormat">The wave format from which the key should be created.</param>
        /// <returns>The created key.</returns>
        private uint MakeWaveFormatKey(WaveFormat waveFormat)
        {
            uint key = 0;
            uint extra = 0;

            BitField.Set((uint)waveFormat.Encoding, 9, 0, ref key);
            BitField.Set((uint)waveFormat.Channels, 7, 9, ref key);

            switch (waveFormat.Encoding)
            {
                case WaveFormatEncoding.Pcm:
                    extra = (uint)waveFormat.BitsPerSample;
                    break;

                case WaveFormatEncoding.IeeeFloat:
                    if (waveFormat.BitsPerSample != 32) return 0;
                    extra = (uint)waveFormat.BitsPerSample;
                    break;

                case WaveFormatEncoding.Adpcm:
                    extra = (uint)((WaveFormatAdpcm)waveFormat).SamplesPerBlock;
                    break;

                default:
                    return 0;
            }

            BitField.Set(extra, 16, 16, ref key);

            return key;
        }

        private class InstancePool : Pool<SoundEffectInstance>
        {
            protected override void ClearItem(SoundEffectInstance item)
            {
                item.Dispose();
            }

            protected override bool IsActive(SoundEffectInstance item)
            {
                return item.State != SoundState.Stopped;
            }

            protected override bool TryCreate(bool trackItem, out SoundEffectInstance item)
            {
                item = new SoundEffectInstance(null, null, true);
                return true;
            }

            protected override bool TryReset(bool trackItem, SoundEffectInstance item)
            {
                item.Reset(null, null, true);
                return true;
            }
        }
    }
}