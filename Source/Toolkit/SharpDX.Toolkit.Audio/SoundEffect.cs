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
using System;
using System.Collections.Generic;

namespace SharpDX.Toolkit.Audio
{
    using SharpDX.IO;
    using SharpDX.Multimedia;
    using SharpDX.XAudio2;
    using System.IO;

    public sealed class SoundEffect : IDisposable
    {
        private AudioBuffer audioBuffer;
        private uint[] decodedPacketsInfo;
        private List<WeakReference> children;
        private SoundEffectInstancePool instancePool;


        internal SoundEffect(AudioManager audioManager, string name, WaveFormat format,AudioBuffer buffer, uint[] decodedPacketsInfo)
        {
            this.Manager = audioManager;
            this.Name = name;
            this.Format = format;
            this.audioBuffer = buffer;
            this.decodedPacketsInfo = decodedPacketsInfo;

            var sampleCount = (float)this.audioBuffer.PlayLength;
            var avgBPS = (float)this.Format.AverageBytesPerSecond;
            this.Duration = TimeSpan.FromSeconds(sampleCount / avgBPS);

            this.children = new List<WeakReference>();
            this.instancePool = new SoundEffectInstancePool(this);
        }

        public TimeSpan Duration { get; private set; }
        public string Name { get; private set; }
        internal AudioManager Manager { get; private set; }
        internal WaveFormat Format { get; private set; }


        public static SoundEffect FromStream(AudioManager audioManager, Stream stream, string name = null)
        {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            if (stream == null)
                throw new ArgumentNullException("stream");

            var sound = new SoundStream(stream);
            var format = sound.Format;
            var decodedPacketsInfo = sound.DecodedPacketsInfo;
            var buffer = new AudioBuffer()
            {
                Stream = sound.ToDataStream(),
                AudioBytes = (int)sound.Length,
                Flags = BufferFlags.EndOfStream,
            };

            sound.Close();

            return new SoundEffect(audioManager, name, format, buffer, decodedPacketsInfo);
        }

        public static SoundEffect FromFile(AudioManager audioManager, string filePath)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return FromStream(audioManager, stream, Path.GetFileNameWithoutExtension(filePath));
        }

        public bool Play(float volume, float pitch, float pan)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            SoundEffectInstance instance = instancePool.Acquire(true);
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.Pan = pan;
            instance.Play();
            AddChild(instance);
            return true;
        }


        public bool Play()
        {
            return Play(1.0f, 0.0f, 0.0f);
        }


        public SoundEffectInstance Create()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            
            SoundEffectInstance instance = instancePool.Acquire(false);
            instance.IsFireAndForget = false;
            AddChild(instance);
            return instance;
        }


        private void AddChild(SoundEffectInstance instance)
        {
            lock (this.children)
            {
                this.children.Add(new WeakReference(instance));
            }
        }


        internal void SubmitAudioBuffer(SourceVoice voice)
        {
            voice.SubmitSourceBuffer(audioBuffer, decodedPacketsInfo);
        }


        public bool IsDisposed { get; private set; }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                lock (this.children)
                {
                    foreach (var item in this.children)
                    {
                        SoundEffectInstance soundEffectInstance = item.Target as SoundEffectInstance;
                        if (soundEffectInstance != null)
                        {
                            soundEffectInstance.ParentDisposed();
                        }
                    }
                    this.children.Clear();
                    this.instancePool.Clear();
                }

                audioBuffer.Stream.Dispose();
                audioBuffer = null;
            }
        }


        internal void ChildDisposed(SoundEffectInstance child)
        {
            lock (this.children)
            {
                for (int i = 0; i < this.children.Count; i++)
                {
                    WeakReference weakReference = this.children[i];
                    SoundEffectInstance soundEffectInstance = weakReference.Target as SoundEffectInstance;
                    if (!weakReference.IsAlive || soundEffectInstance == null || soundEffectInstance == child)
                    {
                        this.children.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        
    }
}
