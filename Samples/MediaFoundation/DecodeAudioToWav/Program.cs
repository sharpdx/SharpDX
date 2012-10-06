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
using System.Windows.Forms;
using SharpDX.MediaFoundation;
using SharpDX.Multimedia;

namespace DecodeAudioToWav
{
    /// <summary>
    /// This samples demonstrates how to use <see cref="AudioDecoder"/> to decode any audio file (wma, mp3,... or even video files)
    /// to WAV format. This sample is using also the <see cref="WavWriter"/>
    /// </summary>
    public class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string fileName;
            if (args.Length == 1)
            {
                fileName = args[0];
            }
            else
            {
                var dialog = new OpenFileDialog {Title = "Select an audio file (wma, mp3, ...etc.) or video file..."};
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                fileName = dialog.FileName;
            }

            var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var audioDecoder = new AudioDecoder(stream);

            var outputWavStream = new FileStream("output.wav", FileMode.Create, FileAccess.Write);

            var wavWriter = new WavWriter(outputWavStream);

            // Write the WAV file
            wavWriter.Begin(audioDecoder.WaveFormat);

            // Decode the samples from the input file and output PCM raw data to the WAV stream.
            wavWriter.AppendData(audioDecoder.GetSamples());

            // Close the wav writer.
            wavWriter.End();

            audioDecoder.Dispose();
            outputWavStream.Close();
            stream.Close();
        }
    }
}
