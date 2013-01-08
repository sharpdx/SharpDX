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
//
// SoundState originally created by Microsoft for XNA.

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Current state (playing, paused, or stopped) of a <see cref="SoundEffectInstance"/>.
    /// </summary>
    public enum SoundState
    {
        /// <summary>
        /// The <see cref="SoundEffectInstance"/> is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The <see cref="SoundEffectInstance"/> is playing.
        /// </summary>
        Playing,

        /// <summary>
        /// The <see cref="SoundEffectInstance"/> is stopped.
        /// </summary>
        Stopped
    }
}
