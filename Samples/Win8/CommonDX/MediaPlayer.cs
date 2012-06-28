// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.MediaFoundation;
using Windows.Storage.Streams;

namespace CommonDX
{
    /// <summary>
    /// A Media Player. This is a port of the C++ "Media engine native C++ video playback sample".
    /// </summary>
    public class MediaPlayer
    {
        private DXGIDeviceManager dxgiDeviceManager;
        private MediaEngine mediaEngine;
        private MediaEngineEx mediaEngineEx;
        private SharpDX.DXGI.Output dxgiOutput;
        private CoreWindowTarget targetBase;

        private bool isEndOfStream;
        private bool isTimerStopped;
        private readonly object lockObject = new object();

        public MediaPlayer()
        {
            BackgroundColor = Colors.Transparent;
            isTimerStopped = true;
        }

        /// <summary>
        /// Gets whether this media player is playing a video or audio.
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Gets or sets the background color used to display the video.
        /// </summary>
        public Color4 BackgroundColor { get; set; }

        /// <summary>
        /// Event fired when a new video frame has been transfered to <see cref="VideoTextureDestination"/>.
        /// </summary>
        public event Action OnNewVideoFrameAvailable;

        /// <summary>
        /// Gets or sets the url used to play the stream.
        /// </summary>
        public string Url { get; set; }

        private ByteStream byteStream;
        private ComObject streamCom;
        
        
        public virtual void Initialize(SharpDX.Direct3D11.Device device, CoreWindowTarget targetBase)
        {
            lock (lockObject)
            {
                this.targetBase = targetBase;

                // Startup MediaManager
                MediaManager.Startup();

                // Create a DXGI Device Manager
                dxgiDeviceManager = new DXGIDeviceManager();
                dxgiDeviceManager.ResetDevice(device);

                // Setup Media Engine attributes
                var attributes = new MediaEngineAttributes
                                     {
                                         DxgiManager = dxgiDeviceManager,
                                         VideoOutputFormat = (int) SharpDX.DXGI.Format.B8G8R8A8_UNorm
                                     };

                using (var factory = new MediaEngineClassFactory())
                    mediaEngine = new MediaEngine(factory, attributes, MediaEngineCreateflags.WaitforstableState, OnMediaEngineEvent);                
                mediaEngineEx = mediaEngine.QueryInterface<MediaEngineEx>();
            }
        }

        public void Shutdown()
        {
            lock (lockObject)
            {
                StopTimer();

                if (mediaEngineEx != null)
                    mediaEngineEx.Shutdown();
            }
        }

        public void SetBytestream(IRandomAccessStream streamHandle)
        {
            streamCom = new ComObject(streamHandle);
            byteStream = new ByteStream(streamCom);
            mediaEngineEx.SetSourceFromByteStream(byteStream, Url);
        }

        /// <summary>
        /// Plays the audio/video.
        /// </summary>
        public void Play()
        {
            if (mediaEngineEx != null)
            {
                if (mediaEngineEx.HasVideo() && isTimerStopped)
                {
                    // Start the Timer thread
                    StartTimer();
                }

                if (isEndOfStream)
                {
                    PlaybackPosition = 0;
                    IsPlaying = true;
                }
                else
                {
                    mediaEngineEx.Play();
                }

                isEndOfStream = false;
            }
        }

        /// <summary>
        /// Pauses the audio/video.
        /// </summary>
        public void Pause()
        {    
            if (mediaEngineEx != null)
                mediaEngineEx.Pause();
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        public double Volume
        {
            get
            {
                if (mediaEngineEx != null)
                    return mediaEngineEx.Volume;
                return 0.0;
            }
            set
            {
                if (mediaEngineEx != null)
                    mediaEngineEx.Volume = value;
            }
        }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        public double Balance
        {
            get
            {
                if (mediaEngineEx != null)
                    return mediaEngineEx.Balance;
                return 0.0;
            }
            set
            {
                if (mediaEngineEx != null)
                    mediaEngineEx.Balance = value;
            }
        }

        /// <summary>
        /// Gets or sets muted mode.
        /// </summary>
        public bool Mute
        {
            get
            {
                if (mediaEngineEx != null)
                    return mediaEngineEx.Muted;
                return false;
            }
            set
            {
                if (mediaEngineEx != null)
                    mediaEngineEx.Muted = value;
            }
        }

        /// <summary>
        /// Steps forward or backward one frame.
        /// </summary>
        public void FrameStep(bool forward)
        {
            if (mediaEngineEx != null)
                mediaEngineEx.FrameStep(forward);
        }

        /// <summary>
        /// Gets the duration of the audio/video.
        /// </summary>
        public double Duration
        {
            get
            {
                double duration = 0.0;
                if (mediaEngineEx != null)
                {
                    duration = mediaEngineEx.Duration;
                    if (double.IsNaN(duration))
                        duration = 0.0;
                }
                return duration;
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether the audio/video is seekable.
        /// </summary>
        public bool CanSeek
        {
            get
            {
                if (mediaEngineEx != null)
                    return (mediaEngineEx.ResourceCharacteristics & ResourceCharacteristics.CanSeek) != 0 && Duration != 0.0;
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the playback position.
        /// </summary>
        public double PlaybackPosition
        {
            get
            {
                if (mediaEngineEx != null)
                    return mediaEngineEx.CurrentTime;
                return 0.0;
            }
            set
            {
                if (mediaEngineEx != null)
                    mediaEngineEx.CurrentTime = value;
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether the audio/video is seeking.
        /// </summary>
        public bool IsSeeking
        {
            get
            {
                if (mediaEngineEx != null)
                    return mediaEngineEx.IsSeeking;
                return false;
            }
        }

        /// <summary>
        /// Enables video effect.
        /// </summary>
        /// <param name="enable"></param>
        public void EnableVideoEffect(bool enable)
        {
            if (mediaEngineEx != null)
            {
                mediaEngineEx.RemoveAllEffects();
                if (enable)
                {
                    mediaEngineEx.InsertVideoEffect(new Activate(Windows.Media.VideoEffects.VideoStabilization), false);
                }
            }
        }

        private void StartTimer()
        {
            var factory = new SharpDX.DXGI.Factory1();
            dxgiOutput = factory.Adapters[0].Outputs[0];
            isTimerStopped = false;

            Task.Run(delegate
                         {
                             while (true)
                             {
                                 if (isTimerStopped)
                                 {
                                     break;
                                 }

                                 try
                                 {
                                     dxgiOutput.WaitForVerticalBlank();
                                     OnTimer();
                                 }
                                 catch (Exception ex)
                                 {
                                     break;
                                 }
                             }
                         });
        }

        private void StopTimer()
        {
            isTimerStopped = true;
            IsPlaying = false;
        }

        private void OnTimer()
        {
            lock(lockObject)
            {
                if (mediaEngineEx != null)
                {
                    long pts;
                    if (mediaEngineEx.OnVideoStreamTick(out pts))
                    {
                        var backBuffer = targetBase.SwapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);

                        var desc = backBuffer.Description;
                        var region = new Rectangle(0, 0, desc.Width, desc.Height);

                        mediaEngineEx.TransferVideoFrame(backBuffer, null, region, (ColorBgra)BackgroundColor);

                        targetBase.Present();

                        var onNewVideoFrameAvailable = OnNewVideoFrameAvailable;
                        if (onNewVideoFrameAvailable != null) onNewVideoFrameAvailable();
                    }
                }
            }
        }

        protected virtual void OnMediaEngineEvent(MediaEngineEvent mediaEvent, long param1, int param2)
        {
            switch (mediaEvent)
            {
                case MediaEngineEvent.Notifystablestate:
                    SetEvent(new IntPtr(param1));
                    break;
                case MediaEngineEvent.Loadedmetadata:
                    isEndOfStream = false;
                    break;
                case MediaEngineEvent.Canplay:
                    // Start the Playback
                    Play();
                    break;
                case MediaEngineEvent.Play:
                    IsPlaying = true;
                    break;
                case MediaEngineEvent.Pause:
                    IsPlaying = false;
                    break;
                case MediaEngineEvent.Ended:
                    if (mediaEngineEx.HasVideo())
                    {
                        StopTimer();
                    }
                    isEndOfStream = true;
                    break;
                case MediaEngineEvent.Timeupdate:
                    break;
                case MediaEngineEvent.Error:
                    break;
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "SetEvent")]
        private static extern bool SetEvent(IntPtr hEvent);
    }
}