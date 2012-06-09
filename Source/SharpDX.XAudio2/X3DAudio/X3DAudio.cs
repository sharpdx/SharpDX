// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using SharpDX.Multimedia;

namespace SharpDX.X3DAudio
{
    public partial class X3DAudio
    {
        private X3DAudioHandle handle;

        /// <summary>
        /// Speed of sound in the air.
        /// </summary>
        public const float SpeedOfSound = 343.5f;

        /// <summary>
        /// Initializes a new instance of the <see cref="X3DAudio"/> class.
        /// </summary>
        /// <param name="speakers">The speakers config.</param>
        public X3DAudio(Speakers speakers) : this(speakers, SpeedOfSound)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X3DAudio"/> class.
        /// </summary>
        /// <param name="speakers">The speakers config.</param>
        /// <param name="speedOfSound">The speed of sound.</param>
        public X3DAudio(Speakers speakers, float speedOfSound)
        {
            X3DAudioInitialize(speakers, speedOfSound, out handle);
        }

        /// <summary>
        /// Calculates dsp settings for the specified listener and emitter.
        /// </summary>
        /// <param name="listener">The listener.</param>
        /// <param name="emitter">The emitter.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="sourceChannelCount">The source channel count.</param>
        /// <param name="destinationChannelCount">The destination channel count.</param>
        /// <returns>Dsp settings</returns>
        public DspSettings Calculate(Listener listener, Emitter emitter, CalculateFlags flags, int sourceChannelCount, int destinationChannelCount)
        {
            var settings = new DspSettings
            {
                SourceChannelCount = sourceChannelCount,
                DestinationChannelCount = destinationChannelCount
            };

            X3DAudioCalculate(ref handle, listener, emitter, flags, settings);

            return settings;
        }
    }
}