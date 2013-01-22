// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Threading;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using SharpDX.XAudio2.Fx;
using BufferFlags = SharpDX.XAudio2.BufferFlags;

namespace PlayDynamicSound
{
    class Program
    {
        /// <summary>
        /// SharpDX XAudio2 sample. Plays a generated sound with some reverb.
        /// </summary>
        static void Main(string[] args)
        {
            var xaudio2 = new XAudio2();
            var masteringVoice = new MasteringVoice(xaudio2);

            var waveFormat = new WaveFormat(44100, 32, 2);
            var sourceVoice = new SourceVoice(xaudio2, waveFormat);

            int bufferSize = waveFormat.ConvertLatencyToByteSize(60000);
            var dataStream = new DataStream(bufferSize, true, true);

            int numberOfSamples = bufferSize/waveFormat.BlockAlign;
            for (int i = 0; i < numberOfSamples; i++)
            {
                double vibrato = Math.Cos(2 * Math.PI * 10.0 * i / waveFormat.SampleRate);
                float value = (float) (Math.Cos(2*Math.PI*(220.0 + 4.0*vibrato)*i/waveFormat.SampleRate)*0.5); 
                dataStream.Write(value);
                dataStream.Write(value);
            }
            dataStream.Position = 0;

            var audioBuffer = new AudioBuffer {Stream = dataStream, Flags = BufferFlags.EndOfStream, AudioBytes = bufferSize};

            var reverb = new Reverb();
            var effectDescriptor = new EffectDescriptor(reverb);
            sourceVoice.SetEffectChain(effectDescriptor);
            sourceVoice.EnableEffect(0);

            sourceVoice.SubmitSourceBuffer(audioBuffer, null);

            sourceVoice.Start();

            Console.WriteLine("Play sound");
            for(int i = 0; i < 60; i++)
            {
                Console.Write(".");
                Console.Out.Flush();
                Thread.Sleep(1000);
            }
        }
    }
}
