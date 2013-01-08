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
// AudioEngine originally created by Yuri Roubinsky for ShaprDX.

using SharpDX.XAudio2;

namespace SharpDX.Toolkit.Audio
{ 
    internal static class AudioEngine
    {
        public static SharpDX.XAudio2.XAudio2 Engine;
        public static MasteringVoice MasterVoice;
        public static int SoundEffectInstanceCount;

        static AudioEngine()
        {
           Engine = new SharpDX.XAudio2.XAudio2(XAudio2Flags.None, ProcessorSpecifier.AnyProcessor);
           MasterVoice = new MasteringVoice(Engine);
           SoundEffectInstanceCount = 0;
        }

        public static void Dispose()
        {
            Engine.Dispose();
            MasterVoice.Dispose();
        }
    }
}
