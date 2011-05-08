/*
* Copyright (c) 2007-2009 SharpDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Windows.Forms;
using SharpDX;
using SharpDX.DirectSound;
using SharpDX.Multimedia;

namespace PlaySoundCustomXAPO
{
    /// <summary>
    /// SharpDX DirectSound sample. Plays a generated sound.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DirectSound directSound = new DirectSound();

            var form = new Form();
            form.Text = "SharpDX - DirectSound Demo";

            // Set Cooperative Level to PRIORITY (priority level can call the SetFormat and Compact methods)
            //
            directSound.SetCooperativeLevel(form.Handle, CooperativeLevel.Priority);

            // Create PrimarySoundBuffer
            var primaryBufferDesc = new SoundBufferDescription();
            primaryBufferDesc.Flags = BufferFlags.PrimaryBuffer;
            primaryBufferDesc.AlgorithmFor3D = Guid.Empty;

            var primarySoundBuffer = new PrimarySoundBuffer(directSound, primaryBufferDesc);

            // Play the PrimarySound Buffer
            primarySoundBuffer.Play(0, PlayFlags.Looping);

            // Default WaveFormat Stereo 44100 16 bit
            WaveFormat waveFormat = new WaveFormat();

            // Create SecondarySoundBuffer
            var secondaryBufferDesc = new SoundBufferDescription();
            secondaryBufferDesc.BufferBytes = waveFormat.ConvertLatencyToByteSize(60000);
            secondaryBufferDesc.Format = waveFormat;
            secondaryBufferDesc.Flags = BufferFlags.GetCurrentPosition2 | BufferFlags.ControlPositionNotify | BufferFlags.GlobalFocus |
                                        BufferFlags.ControlVolume | BufferFlags.StickyFocus;
            secondaryBufferDesc.AlgorithmFor3D = Guid.Empty;
            var secondarySoundBuffer = new SecondarySoundBuffer(directSound, secondaryBufferDesc);

            // Get Capabilties from secondary sound buffer
            var capabilities = secondarySoundBuffer.Capabilities;

            // Lock the buffer
            DataStream dataPart2;
            var dataPart1 =secondarySoundBuffer.Lock(0, capabilities.BufferBytes,  LockFlags.EntireBuffer, out dataPart2);

            // Fill the buffer with some sound
            int numberOfSamples = capabilities.BufferBytes/waveFormat.BlockAlign;
            for (int i = 0; i < numberOfSamples; i++)
            {
                double vibrato = Math.Cos(2 * Math.PI * 10.0 * i /waveFormat.SampleRate);
                short value = (short) (Math.Cos(2*Math.PI*(220.0 + 4.0 * vibrato)*i/waveFormat.SampleRate)*16384); // Not too loud
                dataPart1.Write(value);
                dataPart1.Write(value);
            }

            // Unlock the buffer
            secondarySoundBuffer.Unlock(dataPart1, dataPart2);

            // Play the song
            secondarySoundBuffer.Play(0, PlayFlags.Looping);
           
            Application.Run(form);
        }
    }
}
