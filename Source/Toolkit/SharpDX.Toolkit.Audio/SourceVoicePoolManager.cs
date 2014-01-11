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
    
    internal sealed class SourceVoicePoolManager:IDisposable
    {
        private Dictionary<uint, SourceVoicePool> pools;

        internal AudioManager AudioManager { get; private set; }

        public SourceVoicePoolManager(AudioManager audioManager)
        {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            AudioManager = audioManager;

            pools = new Dictionary<uint, SourceVoicePool>();

        }

        public SourceVoicePool GetSourceVoicePool(WaveFormat waveFormat)
        {
            lock (pools)
            {
                uint waveKey = MakeWaveFormatKey(waveFormat);

                if (waveKey == 0)
                {
                    return new SourceVoicePool(this, waveFormat, false);
                }
                else
                {
                    SourceVoicePool pool;

                    if (!pools.TryGetValue(waveKey, out pool))
                        pools[waveKey] = pool = new SourceVoicePool(this, waveFormat, true);

                    return pool;
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

        public bool IsDisposed { get; private set; }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                lock (pools)
                {
                    foreach (var pool in pools.Values)
                    {
                        pool.Clear();
                    }

                    pools.Clear();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
