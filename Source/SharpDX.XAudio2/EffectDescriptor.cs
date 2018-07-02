// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using SharpDX.XAPO;

namespace SharpDX.XAudio2
{
    public partial class EffectDescriptor
    {
        private AudioProcessor _effect;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectDescriptor"/> class with a Stereo Effect.
        /// </summary>
        /// <param name="effect">The effect.</param>
        public EffectDescriptor(AudioProcessor effect) : this(effect,2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectDescriptor"/> class.
        /// </summary>
        /// <param name="effect">The effect.</param>
        /// <param name="outputChannelCount">The output channel count.</param>
        public EffectDescriptor(AudioProcessor effect, int outputChannelCount)
        {
            EffectPointer = null;
            Effect = effect;
            InitialState = true;
            OutputChannelCount = outputChannelCount;
        }

        /// <summary>
        /// Gets or sets the AudioProcessor. The AudioProcessor cannot be set more than one.
        /// </summary>
        /// <value>The effect.</value>
        public AudioProcessor Effect
        {
            get
            {
                return _effect;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Effect cannot be set to null");
                if (_effect != null)
                    throw new ArgumentException("Cannot set Effect twice on the same EffectDescriptor");

                _effect = value;
                EffectPointer = _effect;
            }
        }        
    }
}