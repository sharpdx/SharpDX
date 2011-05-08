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
using System.Windows.Forms;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using SharpDX.XAudio2.Fx;

namespace PlaySoundCustomXAPO
{
    public partial class PlayForm : Form
    {
        private XAudio2 xaudio2;
        private MasteringVoice masteringVoice;
        private SourceVoice sourceVoice;
        private AudioBuffer audioBuffer;
        private EffectDescriptor modulatorDescriptor;
        private Reverb reverb;
        private EffectDescriptor effectDescriptor;

        public PlayForm()
        {
            InitializeComponent();

            // Initalize XAudio2
            xaudio2 = new XAudio2(XAudio2Flags.DebugEngine, WindowsProcessorSpecifier.DefaultProcessor);
            masteringVoice = new MasteringVoice(xaudio2);

            var waveFormat = new WaveFormat(44100, 32, 2);
             sourceVoice = new SourceVoice(xaudio2, waveFormat);

            int bufferSize = waveFormat.ConvertLatencyToByteSize(60000);
            DataStream dataStream = new DataStream(bufferSize, true, true);

            // Prepare the initial sound to modulate
            int numberOfSamples = bufferSize / waveFormat.BlockAlign;
            for (int i = 0; i < numberOfSamples; i++)
            {
                float value = (float)(Math.Cos(2 * Math.PI * 220.0 * i / waveFormat.SampleRate) * 0.5);
                dataStream.Write(value);
                dataStream.Write(value);
            }
            dataStream.Position = 0;

            audioBuffer = new AudioBuffer
                              {
                                  Stream = dataStream,
                                  Flags = BufferFlags.EndOfStream,
                                  AudioBytes = bufferSize,
                                  LoopBegin = 0,
                                  LoopLength = numberOfSamples,
                                  LoopCount = AudioBuffer.LoopInfinite
                              };

            // Set the effect on the source
            ModulatorEffect = new ModulatorEffect();
            modulatorDescriptor = new EffectDescriptor(ModulatorEffect);
            reverb = new Reverb();
            effectDescriptor = new EffectDescriptor(reverb);
            //sourceVoice.SetEffectChain(modulatorDescriptor, effectDescriptor);
            sourceVoice.SetEffectChain(modulatorDescriptor);
            //sourceVoice.EnableEffect(0);

            this.Closed += new EventHandler(PlayForm_Closed);
        }

        void PlayForm_Closed(object sender, EventArgs e)
        {
            sourceVoice.Stop(PlayFlags.None, 0);
            xaudio2.StopEngine();
        }

        public ModulatorEffect ModulatorEffect { get; set; }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            ModulatorEffect.Intensity = trackBar1.Value / 100.0f;
        }

        private bool isStarted;
        private void buttonPlayStop_Click(object sender, EventArgs e)
        {
            if (!isStarted)
            {
                // Play a buffer
                sourceVoice.SubmitSourceBuffer(ref audioBuffer, null);
                sourceVoice.Start();
                isStarted = true;
                buttonPlayStop.Text = "Stop";
            }
            else
            {
                // Stop a buffer
                sourceVoice.Stop(PlayFlags.None, 0);
                isStarted = false;
                buttonPlayStop.Text = "Play";
            }
        }
    }
}
