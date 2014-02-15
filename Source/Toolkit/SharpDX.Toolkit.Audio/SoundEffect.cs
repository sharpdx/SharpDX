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
    using SharpDX.X3DAudio;
    using System.IO;

    /// <summary>
    /// Provides a loaded sound resource.
    /// </summary>
    public sealed class SoundEffect : IDisposable
    {       
        
        private List<WeakReference> children;

        internal SoundEffect(AudioManager audioManager, string name, WaveFormat waveFormat, DataStream buffer, uint[] decodedPacketsInfo)
        {
            AudioManager = audioManager;
            Name = name;
            Format = waveFormat;
            AudioBuffer = new AudioBuffer
            {
                Stream = buffer,
                AudioBytes = (int)buffer.Length,
                Flags = BufferFlags.EndOfStream,
            };
            LoopedAudioBuffer = new AudioBuffer
            {
                Stream = buffer,
                AudioBytes = (int)buffer.Length,
                Flags = BufferFlags.EndOfStream,
                LoopCount = AudioBuffer.LoopInfinite,
            };

            DecodedPacketsInfo = decodedPacketsInfo;

            Duration = Format.SampleRate > 0 ? TimeSpan.FromMilliseconds(GetSamplesDuration() * 1000 / Format.SampleRate) : TimeSpan.Zero;

            children = new List<WeakReference>();
            VoicePool = AudioManager.InstancePool.GetVoicePool(Format);
        }

        static float distanceScale = 1.0f;
        public static float DistanceScale
        {
            get
            {
                return distanceScale;
            }
            set
            {
                if (value <= 0f)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                distanceScale = value;
            }
        }

        static float dopplerScale = 1f;
        public static float DopplerScale
        {
            get
            {
                return dopplerScale;
            }
            set
            {
                if (value < 0f)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                dopplerScale = value;
            }
        }


        public TimeSpan Duration { get; private set; }
        public string Name { get; private set; }
        public AudioManager AudioManager { get; private set; }
        internal SourceVoicePool VoicePool { get; private set; }
        internal WaveFormat Format { get; private set; }
        internal AudioBuffer AudioBuffer { get; private set; }
        internal AudioBuffer LoopedAudioBuffer { get; private set; }
        internal uint[] DecodedPacketsInfo { get; private set; }


        public static SoundEffect FromStream(AudioManager audioManager, Stream stream, string name = null)
        {
            if (audioManager == null)
                throw new ArgumentNullException("audioManager");

            if (stream == null)
                throw new ArgumentNullException("stream");

            var sound = new SoundStream(stream);
            var format = sound.Format;
            var decodedPacketsInfo = sound.DecodedPacketsInfo;
            var buffer = sound.ToDataStream();

#if WIN8METRO
            sound.Dispose();
#else
            //sound.Close();
            sound.Dispose();
#endif            
            return audioManager.ToDisposeAudioAsset( new SoundEffect(audioManager, name, format, buffer, decodedPacketsInfo));
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

            SoundEffectInstance instance = null;
            if(AudioManager.InstancePool.TryAcquire(this, true, out instance))
            {
                instance.Volume = volume;
                instance.Pitch = pitch;
                instance.Pan = pan;
                instance.Play();
                return true;
            }

            return false;
        }


        public bool Play()
        {
            return Play(1.0f, 0.0f, 0.0f);
        }

        public SoundEffectInstance Create()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);    

            SoundEffectInstance instance = null;
            if (AudioManager.InstancePool.TryAcquire(this, false, out instance))
            {                
                AddChild(instance);
                return instance;
            }

            throw new InvalidOperationException("Unable to create SoundEffectInstance, insufficient source voices available.");
        }


        private void AddChild(SoundEffectInstance instance)
        {
            lock (children)
            {
                this.children.Add(new WeakReference(instance));
            }
        }


        private long GetSamplesDuration()
        {           
            
            switch (Format.Encoding)
            {
                case WaveFormatEncoding.Adpcm:
                    var adpcmFormat = Format as WaveFormatAdpcm;
                    long duration = (AudioBuffer.AudioBytes / adpcmFormat.BlockAlign) * adpcmFormat.SamplesPerBlock;
                    long partial = AudioBuffer.AudioBytes % adpcmFormat.BlockAlign;
                    if (partial > 0)
                    {
                        if (partial >= (7 * adpcmFormat.Channels))
                            duration += (partial * 2) / (adpcmFormat.Channels - 12);
                    }

                    return duration;
                case WaveFormatEncoding.Wmaudio2:
                case WaveFormatEncoding.Wmaudio3:
                    if (DecodedPacketsInfo != null)
                    {
                        return DecodedPacketsInfo[DecodedPacketsInfo.Length - 1] / Format.Channels;
                    }
                    break;

                case WaveFormatEncoding.Pcm:
                    if (Format.BitsPerSample > 0)
                    {
                        return ((long)AudioBuffer.AudioBytes) * 8 / (Format.BitsPerSample * Format.Channels);
                    }
                    break;
                default:
                    break;
            }

            return 0;
        }

        
        public bool IsDisposed { get; private set; }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                lock (children)
                {
                    foreach (var item in children)
                    {
                        SoundEffectInstance soundEffectInstance = item.Target as SoundEffectInstance;
                        if (soundEffectInstance != null)
                        {
                            soundEffectInstance.ParentDisposed();
                        }
                    }
                    children.Clear();
                    VoicePool.Release();
                    VoicePool = null;
                }

                AudioBuffer.Stream.Dispose();
                AudioBuffer = null;
                LoopedAudioBuffer = null;
            }
        }


        internal void ChildDisposed(SoundEffectInstance child)
        {
            lock (this.children)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    WeakReference weakReference = children[i];
                    SoundEffectInstance soundEffectInstance = weakReference.Target as SoundEffectInstance;
                    if (!weakReference.IsAlive || soundEffectInstance == null || soundEffectInstance == child)
                    {
                        children.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

    }
}
