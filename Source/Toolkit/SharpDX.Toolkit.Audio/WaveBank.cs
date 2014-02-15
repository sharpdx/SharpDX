// Copyright (c) 2010-2013 SharpDX - SharpDX Team
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
using SharpDX.IO;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpDX.Toolkit.Audio
{
    public sealed class WaveBank:IDisposable
    {
        private AudioManager audioManager;
        private SoundEffect[] effects;
        private Dictionary<string, SoundEffect> effectsByName;


        private WaveBank(AudioManager audioManager, Stream stream)
	    {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            if (stream == null)
                throw new ArgumentNullException("stream");
            
            this.audioManager = audioManager;

            using (var reader = new WaveBankReader(stream))
            {               
                
                this.effects = new SoundEffect[reader.Count];
                this.effectsByName = new Dictionary<string, SoundEffect>();

                for (uint i = 0; i < reader.Count; i++)
                {
                    var format = reader.GetWaveFormat(i);
                    var metadata = reader.GetMetadata(i);
                    var name = reader.GetName(i);
                    var data = reader.GetWaveData(i);
                    uint[] decodedPacketsInfo = null;

                    if (format.Encoding == Multimedia.WaveFormatEncoding.Wmaudio2 || format.Encoding == Multimedia.WaveFormatEncoding.Wmaudio3)
                    {
                        Multimedia.WaveFormatEncoding tag;
                        decodedPacketsInfo = reader.GetSeekTable(i, out tag);
                    }

                    var buffer = DataStream.Create<byte>(data,true,false);

                    var effect = this.effects[i] = new SoundEffect(audioManager, name, format, buffer, decodedPacketsInfo);

                    if (!string.IsNullOrEmpty(name))
                        this.effectsByName.Add(name, effect);
                }
            }

            
	    }

        public int Count { get { return effects == null ? 0 : effects.Length;}}

        public static WaveBank FromStream(AudioManager audioManager, Stream stream)
        {
            return audioManager.ToDisposeAudioAsset( new WaveBank(audioManager, stream));
        }


        public static WaveBank FromFile(AudioManager audioManager, string filePath)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return new WaveBank(audioManager, stream);
        }


        public bool Play(int index, float volume, float pitch, float pan)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (index < 0 || index >= effects.Length)
                return false;

            return effects[index].Play(volume,pitch,pan);
        }


        public bool Play(int index)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (index < 0 || index >= effects.Length)
                return false;

            return effects[index].Play();
        }


        public bool Play(string name, float volume, float pitch, float pan)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            SoundEffect soundEffect = null;
            if (effectsByName.TryGetValue(name, out soundEffect))
            {
                return soundEffect.Play(volume,pitch,pan);
            }

            return false;
            //throw new ArgumentOutOfRangeException("name", string.Format("No wave with name '{0}' exists.", name));
        }


        public bool Play(string name)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The name cannot be null or empty.");
            
            SoundEffect soundEffect = null;
            if (effectsByName.TryGetValue(name, out soundEffect))
            {
                return soundEffect.Play();
            }

            return false;
            //throw new ArgumentOutOfRangeException("name", string.Format("No wave with name '{0}' exists.", name));
        }


        public SoundEffectInstance Create(int index)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (index < 0 || index >= effects.Length)
                throw new ArgumentOutOfRangeException("index", string.Format("No wave at index '{0}' exists.", index));

            return effects[index].Create();
        }


        public SoundEffectInstance Create(string name)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The name cannot be null or empty.");

            SoundEffect soundEffect = null;
            if (effectsByName.TryGetValue(name, out soundEffect))
            {
                return soundEffect.Create();
            }

            throw new ArgumentOutOfRangeException("name", string.Format("No wave with name '{0}' exists.", name));
        }


        public bool IsDisposed { get; private set; }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                lock (effectsByName)
                {
                    for (int i = 0; i < effects.Length; i++)
                    {
                        effects[i].Dispose();
                    }

                    effects = null;

                    effectsByName.Clear();
                }
            }
        }
    }
}
