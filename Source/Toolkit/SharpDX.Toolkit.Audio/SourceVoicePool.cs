﻿// Copyright (c) 2010-2013 SharpDX - SharpDX Team
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
    /// Pool of <see cref="SourceVoice"/>.
    /// </summary>
    internal sealed class SourceVoicePool : Pool<SourceVoice>, IDisposable
    {
        private SoundEffectInstancePool instancePool;
        private WaveFormat format;
        private bool isShared;

        public SourceVoicePool(SoundEffectInstancePool soundEffectInstancePool, WaveFormat waveFormat, bool isShared)
        {
            if (soundEffectInstancePool == null)
                throw new ArgumentNullException("soundEffectInstancePool");

            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");

            instancePool = soundEffectInstancePool;
            format = waveFormat;
            this.isShared = isShared;
        }
        
        protected override bool IsActive(SourceVoice item)
        {
            return item.State.BuffersQueued > 0;
        }

        protected override bool TryCreate(bool trackItem, out SourceVoice item)
        {
            item = new SourceVoice(instancePool.AudioManager.Device, format, VoiceFlags.None, XAudio2.MaximumFrequencyRatio);
            return true;
        }

        protected override void ClearItem(SourceVoice item)
        {
            if (!item.IsDisposed && instancePool.AudioManager.Device != null)
            {
                item.DestroyVoice();
                item.Dispose();
            }
        }

        internal void ReleaseSourceVoice(SourceVoice voice)
        {
            if (voice != null && !voice.IsDisposed)
            {
                if (IsDisposed)
                {
                    voice.DestroyVoice();
                    voice.Dispose();
                }
                else
                {
                    voice.Stop(0);
                    voice.FlushSourceBuffers();
                    Return(voice);
                }
            }

        }

        public void Release()
        {
            if (!isShared)
            {
                instancePool.RemoveUnshared(this);
                Dispose();
            }
        }

        public bool IsDisposed { get; private set; }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                this.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
