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

namespace SharpDX.Toolkit.Audio
{
    using SharpDX.Multimedia;
    using SharpDX.XAudio2;

    /// <summary>
    /// Pool of <see cref="SourceVoice"/>.
    /// </summary>
    internal sealed class SourceVoicePool : Pool<SourceVoice>, IDisposable
    {
        private WaveFormat format;
        private SoundEffectInstancePool instancePool;
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

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Release()
        {
            if (!isShared)
            {
                instancePool.RemoveUnshared(this);
                Dispose();
            }
        }

        protected override void ClearItem(SourceVoice item)
        {
            if (!item.IsDisposed && instancePool.AudioManager.Device != null)
            {
                item.DestroyVoice();
                item.Dispose();
            }
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

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                this.Clear();
            }
        }
    }
}