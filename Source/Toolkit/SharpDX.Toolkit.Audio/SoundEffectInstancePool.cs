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
using System.Collections.Generic;

namespace SharpDX.Toolkit.Audio
{
    using SharpDX.Multimedia;
    using SharpDX.XAudio2;

    /// <summary>
    /// Pool of <see cref="SoundEffectInstance"/> used to maintain fire and forget instances.
    /// </summary>
    internal class SoundEffectInstancePool : IDisposable
    {
        private InstancePool instancePool;
        private Dictionary<uint, SourceVoicePool> sharedVoicePools;
        private List<SourceVoicePool> unsharedVoicePools;

        public SoundEffectInstancePool(AudioManager audioManager)
        {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            AudioManager = audioManager;

            sharedVoicePools = new Dictionary<uint, SourceVoicePool>();
            unsharedVoicePools = new List<SourceVoicePool>();
            instancePool = new InstancePool();
        }

        public bool IsDisposed { get; private set; }

        internal AudioManager AudioManager { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public SourceVoicePool GetVoicePool(WaveFormat waveFormat)
        {
            lock (sharedVoicePools)
            {
                uint waveKey = MakeWaveFormatKey(waveFormat);
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

        internal void RemoveUnshared(SourceVoicePool pool)
        {
            lock (sharedVoicePools)
            {
                unsharedVoicePools.Remove(pool);
            }
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
        /// Creates a key based on wave format
        /// </summary>
        /// <param name="waveFormat"></param>
        /// <returns></returns>
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