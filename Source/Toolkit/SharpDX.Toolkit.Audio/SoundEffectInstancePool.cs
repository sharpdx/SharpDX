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
    internal class SoundEffectInstancePool : Pool<SoundEffectInstance>
    {
        private readonly SoundEffect effect;

        public SoundEffectInstancePool(SoundEffect soundEffect):base()
        {
            effect = soundEffect;
        }

        protected override bool IsActive(SoundEffectInstance item)
        {
            return item.State != SoundState.Stopped;
        }

        protected override SoundEffectInstance Create()
        {
            return new SoundEffectInstance(effect, true);
        }

        protected override void Reset(SoundEffectInstance item)
        {
            item.Reset();
            item.IsFireAndForget = true;
        }

        protected override void ClearItem(SoundEffectInstance item)
        {
            item.ParentDisposed();
        }
    }
}
