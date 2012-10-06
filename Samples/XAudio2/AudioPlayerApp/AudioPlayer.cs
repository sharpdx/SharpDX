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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.MediaFoundation;
using SharpDX.XAudio2;

namespace AudioPlayerApp
{
    /// <summary>
    /// An audio player able to play audio from several audio and video formats (mp3, wma, avi, mp4... etc.) using XAudio2 and MediaFoundation.
    /// </summary>
    public class AudioPlayer : Component
    {
        private const int WaitPrecision = 1;
        private XAudio2 xaudio2;
        private AudioDecoder audioDecoder;
        private SourceVoice sourceVoice;
        private AudioBuffer[] audioBuffersRing;
        private DataPointer[] memBuffers;
        private Stopwatch clock;
        private ManualResetEvent playEvent;
        private ManualResetEvent waitForPlayToOutput;
        private AutoResetEvent bufferEndEvent;
        private TimeSpan playPosition;
        private TimeSpan nextPlayPosition;
        private TimeSpan playPositionStart;
        private Task playingTask;
        private int playCounter;
        private float localVolume;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPlayer" /> class.
        /// </summary>
        /// <param name="xaudio2">The xaudio2 engine.</param>
        /// <param name="audioStream">The input audio stream.</param>
        public AudioPlayer(XAudio2 xaudio2, Stream audioStream)
        {
            this.xaudio2 = xaudio2;
            audioDecoder = new AudioDecoder(audioStream);
            sourceVoice = new SourceVoice(xaudio2, audioDecoder.WaveFormat);
            localVolume = 1.0f;

            sourceVoice.BufferEnd += sourceVoice_BufferEnd;
            sourceVoice.Start();

            bufferEndEvent = new AutoResetEvent(false);
            playEvent = new ManualResetEvent(false);
            waitForPlayToOutput = new ManualResetEvent(false);

            clock = new Stopwatch();

            // Pre-allocate buffers
            audioBuffersRing = new AudioBuffer[3];
            memBuffers = new DataPointer[audioBuffersRing.Length];
            for (int i = 0; i < audioBuffersRing.Length; i++)
            {
                audioBuffersRing[i] = new AudioBuffer();
                memBuffers[i].Size = 32 * 1024; // default size 32Kb
                memBuffers[i].Pointer = Utilities.AllocateMemory(memBuffers[i].Size);
            }

            // Initialize to stopped
            State = AudioPlayerState.Stopped;

            // Starts the playing thread
            playingTask = Task.Factory.StartNew(PlayAsync, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Gets the XAudio2 <see cref="SourceVoice"/> created by this decoder.
        /// </summary>
        /// <value>The source voice.</value>
        public SourceVoice SourceVoice { get { return sourceVoice; } }

        /// <summary>
        /// Gets the state of this instance.
        /// </summary>
        /// <value>The state.</value>
        public AudioPlayerState State { get; private set; }

        /// <summary>
        /// Gets the duration in seconds of the current sound.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration
        {
            get { return audioDecoder.Duration; }
        }

        /// <summary>
        /// Gets or sets the position in seconds.
        /// </summary>
        /// <value>The position.</value>
        public TimeSpan Position
        {
            get { return playPosition; }
            set
            {
                playPosition = value;
                nextPlayPosition = value;
                playPositionStart = value;
                clock.Restart();
                playCounter++;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to the sound is looping when the end of the buffer is reached.
        /// </summary>
        /// <value><c>true</c> if to loop the sound; otherwise, <c>false</c>.</value>
        public bool IsRepeating { get; set; }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        public void Play()
        {
            if (State != AudioPlayerState.Playing)
            {
                bool waitForFirstPlay = false;
                if (State == AudioPlayerState.Stopped)
                {
                    playCounter++;
                    waitForPlayToOutput.Reset();
                    waitForFirstPlay = true;
                }
                else
                {
                    // The song was paused
                    clock.Start();
                }

                State = AudioPlayerState.Playing;
                playEvent.Set();

                if (waitForFirstPlay)
                {
                    waitForPlayToOutput.WaitOne();
                }
            }
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public float Volume
        {
            get { return localVolume; }
            set
            {
                localVolume = value;
                sourceVoice.SetVolume(value);
            }
        }

        /// <summary>
        /// Pauses the sound.
        /// </summary>
        public void Pause()
        {
            if (State == AudioPlayerState.Playing)
            {
                clock.Stop();
                State = AudioPlayerState.Paused;
                playEvent.Reset();
            }
        }

        /// <summary>
        /// Stops the sound.
        /// </summary>
        public void Stop()
        {
            if (State != AudioPlayerState.Stopped)
            {
                playPosition = TimeSpan.Zero;
                nextPlayPosition = TimeSpan.Zero;
                playPositionStart = TimeSpan.Zero;
                clock.Stop();
                playCounter++;
                State = AudioPlayerState.Stopped;
                playEvent.Reset();
            }
        }

        /// <summary>
        /// Close this audio player.
        /// </summary>
        /// <remarks>
        /// This is similar to call Stop(), Dispose(), Wait().
        /// </remarks>
        public void Close()
        {
            Stop();
            Dispose();
            Wait();
        }

        /// <summary>
        /// Wait that the player is finished.
        /// </summary>
        public void Wait()
        {
            playingTask.Wait();
        }

        /// <summary>
        /// Internal method to play the sound.
        /// </summary>
        private void PlayAsync()
        {
            int currentPlayCounter = 0;
            int nextBuffer = 0;

            try
            {
                while (true)
                {
                    // Check that this instanced is not disposed
                    while (!IsDisposed)
                    {
                        if (playEvent.WaitOne(WaitPrecision))
                            break;
                    }

                    if (IsDisposed)
                        break;

                    clock.Restart();
                    playPositionStart = nextPlayPosition;
                    playPosition = playPositionStart;
                    currentPlayCounter = playCounter;

                    // Get the decoded samples from the specified starting position.
                    var sampleIterator = audioDecoder.GetSamples(playPositionStart).GetEnumerator();

                    bool isFirstTime = true;

                    bool endOfSong = false;

                    // Playing all the samples
                    while (true)
                    {
                        // If the player is stopped or disposed, then break of this loop
                        while (!IsDisposed && State != AudioPlayerState.Stopped)
                        {
                            if (playEvent.WaitOne(WaitPrecision))
                                break;
                        }

                        // If the player is stopped or disposed, then break of this loop
                        if (IsDisposed || State == AudioPlayerState.Stopped)
                        {
                            nextPlayPosition = TimeSpan.Zero;
                            break;
                        }

                        // If there was a change in the play position, restart the sample iterator.
                        if (currentPlayCounter != playCounter)
                            break;

                        // If ring buffer queued is full, wait for the end of a buffer.
                        while (sourceVoice.State.BuffersQueued == audioBuffersRing.Length && !IsDisposed && State != AudioPlayerState.Stopped)
                            bufferEndEvent.WaitOne(WaitPrecision);

                        // If the player is stopped or disposed, then break of this loop
                        if (IsDisposed || State == AudioPlayerState.Stopped)
                        {
                            nextPlayPosition = TimeSpan.Zero;
                            break;
                        }

                        // Check that there is a next sample
                        if (!sampleIterator.MoveNext())
                        {
                            endOfSong = true;
                            break;
                        }

                        // Retrieve a pointer to the sample data
                        var bufferPointer = sampleIterator.Current;

                        // If there was a change in the play position, restart the sample iterator.
                        if (currentPlayCounter != playCounter)
                            break;

                        // Check that our ring buffer has enough space to store the audio buffer.
                        if (bufferPointer.Size > memBuffers[nextBuffer].Size)
                        {
                            if (memBuffers[nextBuffer].Pointer != IntPtr.Zero)
                                Utilities.FreeMemory(memBuffers[nextBuffer].Pointer);

                            memBuffers[nextBuffer].Pointer = Utilities.AllocateMemory(bufferPointer.Size);
                            memBuffers[nextBuffer].Size = bufferPointer.Size;
                        }

                        // Copy the memory from MediaFoundation AudioDecoder to the buffer that is going to be played.
                        Utilities.CopyMemory(memBuffers[nextBuffer].Pointer, bufferPointer.Pointer, bufferPointer.Size);

                        // Set the pointer to the data.
                        audioBuffersRing[nextBuffer].AudioDataPointer = memBuffers[nextBuffer].Pointer;
                        audioBuffersRing[nextBuffer].AudioBytes = bufferPointer.Size;

                        // If this is a first play, restart the clock and notify play method.
                        if (isFirstTime)
                        {
                            clock.Restart();
                            isFirstTime = false;

                            waitForPlayToOutput.Set();
                        }

                        // Update the current position used for sync
                        playPosition = new TimeSpan(playPositionStart.Ticks + clock.Elapsed.Ticks);

                        // Submit the audio buffer to xaudio2
                        sourceVoice.SubmitSourceBuffer(audioBuffersRing[nextBuffer], null);

                        // Go to next entry in the ringg audio buffer
                        nextBuffer = ++nextBuffer%audioBuffersRing.Length;
                    }

                    // If the song is not looping (by default), then stop the audio player.
                    if (endOfSong && !IsRepeating && State == AudioPlayerState.Playing)
                    {
                        Stop();
                    }
                }
            } finally
            {
                DisposePlayer();
            }
        }

        private void DisposePlayer()
        {
            audioDecoder.Dispose();
            audioDecoder = null;

            sourceVoice.Dispose();
            sourceVoice = null;

            for (int i = 0; i < audioBuffersRing.Length; i++)
            {
                Utilities.FreeMemory(memBuffers[i].Pointer);
                memBuffers[i].Pointer = IntPtr.Zero;
            }
        }

        void sourceVoice_BufferEnd(IntPtr obj)
        {
            bufferEndEvent.Set();
        }
    }
}