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

namespace SharpDX.Toolkit.Audio
{
    using System;
    using Multimedia;
    using XAudio2;

    /// <summary>
    /// Pool of <see cref="SourceVoice"/>.
    /// </summary>
    internal sealed class SourceVoicePool : Pool<SourceVoice>, IDisposable
    {
        private readonly WaveFormat format;
        private readonly SoundEffectInstancePool instancePool;
        private readonly bool isShared;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceVoicePool"/> class.
        /// </summary>
        /// <param name="soundEffectInstancePool">The associated sound effect instance pool.</param>
        /// <param name="waveFormat">The wave format of this pool.</param>
        /// <param name="isShared">A value indicating whether the initialized instance is shared or not.</param>
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

        /// <summary>
        /// Gets a value indicating whether the current instance was already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes the current instance and releases all associated unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Removes from instance pool and disposes the current instance if it is not shared.
        /// </summary>
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