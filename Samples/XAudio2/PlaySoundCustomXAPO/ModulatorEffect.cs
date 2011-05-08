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
using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.XAPO;

namespace PlaySoundCustomXAPO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ModulatorParam
    {
    }

    /// <summary>
    /// A simple Ring Modulator Effect
    /// </summary>
    public class ModulatorEffect : AudioProcessorBase<ModulatorParam>
    {
        private Stopwatch timer;

        public ModulatorEffect()
        {
            RegistrationProperties = new RegistrationProperties()
                                         {
                                             Clsid = typeof(ModulatorEffect).GUID,
                                             CopyrightInfo = "Copyright",
                                             FriendlyName = "Modulator",
                                             MaxInputBufferCount = 1,
                                             MaxOutputBufferCount = 1,
                                             MinInputBufferCount = 1,
                                             MinOutputBufferCount = 1,
                                             Flags = PropertyFlags.Default
                                         };
            timer = new Stopwatch();
            timer.Start();
        }

        private int _counter;
        public override void Process(BufferParameters[] inputProcessParameters, BufferParameters[] outputProcessParameters, bool isEnabled)
        {
            int frameCount = inputProcessParameters[0].ValidFrameCount;
            DataStream input = new DataStream(inputProcessParameters[0].Buffer, frameCount*InputFormatLocked.BlockAlign, true, true);
            DataStream output = new DataStream(inputProcessParameters[0].Buffer, frameCount * InputFormatLocked.BlockAlign, true, true);

            //Console.WriteLine("Process is called every: " + timer.ElapsedMilliseconds);
            timer.Reset(); timer.Start();

            // Use a linear ramp on intensity in order to avoir too much glitches
            float nextIntensity = Intensity;
            for (int i = 0; i < frameCount; i++, _counter++)
            {
                float left = input.Read<float>();
                float right = input.Read<float>();
                float intensity = (nextIntensity - lastIntensity) * (float)i/frameCount + lastIntensity;
                double vibrato = Math.Cos(2 * Math.PI * intensity * 400  * _counter / InputFormatLocked.SampleRate);
                output.Write((float) vibrato*left);
                output.Write((float)vibrato * right);
            }
            lastIntensity = nextIntensity;
        }

        private float lastIntensity = 0;

        public float Intensity { get; set; }
    }
}