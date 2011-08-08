// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using SharpDX.XACT3;

namespace PLaySound
{
    class Program
    {
        /// <summary>
        /// SharpDX XACT3 sample. Plays a sound.
        /// </summary>
        static void Main(string[] args)
        {
            // Loading settings
            var settings = File.OpenRead("TestWithSharpDX.xgs");
            var xact3 = new AudioEngine(CreationFlags.DebugMode, settings);

            // Loads WaveBank
            var waveBankStream = File.OpenRead("Ergon.xwb");
            var waveBank = new WaveBank(xact3, waveBankStream);

            // Loads SoundBank
            var soundBankStream = File.OpenRead("Ergon.xsb");
            var soundBank = new SoundBank(xact3, soundBankStream);
            
            // Prepare a cue
            var cue = soundBank.Prepare(soundBank.GetCueIndex("ergon"));

            // Register Notification for testing purpose
            cue.OnNotification += ((from,notif) => Console.WriteLine("Received event {0}", notif.Type) );
            cue.RegisterNotification(NotificationType.CueStop);

            // Plays the cue
            cue.Play();

            // Log a message
            Console.WriteLine("Playing ergon cue");

            // Wait until its done
            int count = 0;
            while (cue.State != CueState.Stopped && !IsKeyPressed(ConsoleKey.Escape))
            {
                xact3.DoWork();
                Thread.Sleep(10);
                if (count == 50)
                {
                    Console.Write(".");
                    Console.Out.Flush();
                    count = 0;
                }
                Thread.Sleep(10);
                count++;
            }

            Thread.Sleep(2000);

            xact3.Dispose();
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
