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
using System.Threading;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.X3DAudio;
using SharpDX.XAudio2;

namespace BipBounceApp
{
    class Program
    {
        /// <summary>
        /// SharpDX X3DAudio sample. Plays a generated sound rotating around the listener.
        /// </summary>
        static void Main(string[] args)
        {
            var xaudio2 = new XAudio2();
            using (var masteringVoice = new MasteringVoice(xaudio2))
            {
                // Instantiate X3DAudio
                var deviceFormat = xaudio2.GetDeviceDetails(0).OutputFormat;
                var x3dAudio = new X3DAudio(deviceFormat.ChannelMask);

                var emitter = new Emitter
                                  {
                                      ChannelCount = 1,
                                      CurveDistanceScaler = float.MinValue,
                                      OrientFront = new Vector3(0, 0, 1),
                                      OrientTop = new Vector3(0, 1, 0),
                                      Position = new Vector3(0, 0, 0),
                                      Velocity = new Vector3(0, 0, 0)
                                  };

                var listener = new Listener
                                   {
                                       OrientFront = new Vector3(0, 0, 1),
                                       OrientTop = new Vector3(0, 1, 0),
                                       Position = new Vector3(0, 0, 0),
                                       Velocity = new Vector3(0, 0, 0)
                                   };

                var waveFormat = new WaveFormat(44100, 32, 1);
                var sourceVoice = new SourceVoice(xaudio2, waveFormat);

                int bufferSize = waveFormat.ConvertLatencyToByteSize(60000);
                var dataStream = new DataStream(bufferSize, true, true);

                int numberOfSamples = bufferSize/waveFormat.BlockAlign;
                for (int i = 0; i < numberOfSamples; i++)
                {
                    float value = (float) (Math.Cos(2*Math.PI*220.0*i/waveFormat.SampleRate)*0.5);
                    dataStream.Write(value);
                }
                dataStream.Position = 0;

                var audioBuffer = new AudioBuffer
                                      {Stream = dataStream, Flags = BufferFlags.EndOfStream, AudioBytes = bufferSize};

                //var reverb = new Reverb();
                //var effectDescriptor = new EffectDescriptor(reverb);
                //sourceVoice.SetEffectChain(effectDescriptor);
                //sourceVoice.EnableEffect(0);

                sourceVoice.SubmitSourceBuffer(audioBuffer, null);

                sourceVoice.Start();

                Console.WriteLine("Play a sound rotating around the listener");
                for (int i = 0; i < 1200; i++)
                {
                    // Rotates the emitter
                    var rotateEmitter = Matrix.RotationY(i/5.0f);
                    var newPosition = Vector3.Transform(new Vector3(0, 0, 1000), rotateEmitter);
                    var newPositionVector3 = new Vector3(newPosition.X, newPosition.Y, newPosition.Z);
                    emitter.Velocity = (newPositionVector3 - emitter.Position)/0.05f;
                    emitter.Position = newPositionVector3;

                    // Calculate X3DAudio settings
                    var dspSettings = x3dAudio.Calculate(listener, emitter, CalculateFlags.Matrix | CalculateFlags.Doppler , 1, deviceFormat.Channels);

                    // Modify XAudio2 source voice settings
                    sourceVoice.SetOutputMatrix(1, deviceFormat.Channels, dspSettings.MatrixCoefficients);
                    sourceVoice.SetFrequencyRatio(dspSettings.DopplerFactor);

                    // Wait for 50ms
                    Thread.Sleep(50);
                }
            }
        }
    }
}
