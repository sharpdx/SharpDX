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
using System;
using System.IO;
using System.Threading;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace PLaySound
{
    class Program
    {
        /// <summary>
        /// SharpDX XAudio2 sample. Plays wav/xwma/adpcm files from the disk.
        /// </summary>
        static void Main(string[] args)
        {
            var xaudio2 = new XAudio2();
            var masteringVoice = new MasteringVoice(xaudio2);

            PLaySoundFile(xaudio2, "1) Playing a standard WAV file", "ergon.wav");
            PLaySoundFile(xaudio2, "2) Playing a XWMA file", "ergon.xwma");
            PLaySoundFile(xaudio2, "3) Playing an ADPCM file", "ergon.adpcm.wav");

            masteringVoice.Dispose();
            xaudio2.Dispose();
        }

        /// <summary>
        /// Play a sound file. Supported format are Wav(pcm+adpcm) and XWMA
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="text">Text to display</param>
        /// <param name="fileName">Name of the file.</param>
        static void PLaySoundFile(XAudio2 device, string text, string fileName)
        {
            Console.WriteLine("{0} => {1} (Press esc to skip)", text, fileName);
            var stream = new SoundStream(File.OpenRead(fileName));
            var waveFormat = stream.Format;
            var buffer = new AudioBuffer
                             {
                                 Stream = stream.ToDataStream(),
                                 AudioBytes = (int) stream.Length,
                                 Flags = BufferFlags.EndOfStream
                             };
            stream.Close();
            
            var sourceVoice = new SourceVoice(device, waveFormat, true);
            // Adds a sample callback to check that they are working on source voices
            sourceVoice.BufferEnd += (context) => Console.WriteLine(" => event received: end of buffer");
            sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
            sourceVoice.Start();

            int count = 0;
            while (sourceVoice.State.BuffersQueued > 0 && !IsKeyPressed(ConsoleKey.Escape))
            {
                if (count == 50)
                {
                    Console.Write(".");
                    Console.Out.Flush();
                    count = 0;
                }
                Thread.Sleep(10);
                count++;
            }
            Console.WriteLine();

            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();
            buffer.Stream.Dispose();
        }

        /// <summary>
        /// Determines whether the specified key is pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is pressed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyPressed(ConsoleKey key)
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == key;
        }
    }
}
