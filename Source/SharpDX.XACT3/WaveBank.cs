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
using Microsoft.Win32.SafeHandles;

using SharpDX.IO;
using SharpDX.Win32;

namespace SharpDX.XACT3
{
    public partial class WaveBank
    {
        private AudioEngine audioEngine;
        private readonly bool isAudioEngineReadonly;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveBank"/> class from a wave bank stream.
        /// </summary>
        /// <param name="audioEngine">The engine.</param>
        /// <param name="stream">The wave bank stream.</param>
        /// <unmanaged>HRESULT IXACT3Engine::CreateInMemoryWaveBank([In] const void* pvBuffer,[In] unsigned int dwSize,[In] unsigned int dwFlags,[In] unsigned int dwAllocAttributes,[Out, Fast] IXACT3WaveBank** ppWaveBank)</unmanaged>
        public WaveBank(AudioEngine audioEngine, Stream stream)
        {
            this.audioEngine = audioEngine;
            isAudioEngineReadonly = true;

            if (stream is DataStream)
            {
                audioEngine.CreateInMemoryWaveBank(((DataStream) stream).DataPointer, (int) stream.Length, 0, 0, this);
                return;
            }

            var data = Utilities.ReadStream(stream);
            unsafe
            {
                fixed (void* pData = data)
                    audioEngine.CreateInMemoryWaveBank((IntPtr)pData, data.Length, 0, 0, this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveBank"/> class from a file for async reading.
        /// </summary>
        /// <param name="audioEngine">The engine.</param>
        /// <param name="fileName">Name of the file to load the wavebank from.</param>
        /// <param name="offset">The offset into the stream.</param>
        /// <param name="packetSize">Packet size used to load the stream.</param>
        public WaveBank(AudioEngine audioEngine, string fileName, int offset, short packetSize)
        {
            this.audioEngine = audioEngine;
            isAudioEngineReadonly = true;

            var handle = NativeFile.Create(fileName, NativeFileAccess.Read, NativeFileShare.Read | NativeFileShare.Write,
                                  IntPtr.Zero, NativeFileMode.Open,
                                  NativeFileOptions.Normal | NativeFileOptions.NoBuffering |
                                  NativeFileOptions.Overlapped | NativeFileOptions.SequentialScan, IntPtr.Zero);


            if (handle == IntPtr.Zero || handle.ToInt32() == -1)
                throw new FileNotFoundException("Unable to open the specified file.", fileName);


            var streamingParameters = new StreamingParameters {File = handle, Flags = 0, Offset = offset, PacketSize = packetSize};

            audioEngine.CreateStreamingWaveBank(streamingParameters, this);
            FileStreamHandle = new SafeFileHandle(handle, true);
        }

        /// <summary>
        /// Gets or sets the audio engine.
        /// </summary>
        /// <value>
        /// The audio engine.
        /// </value>
        public AudioEngine AudioEngine
        {
            get { return audioEngine; }
            set
            {
                if (isAudioEngineReadonly)
                    throw new InvalidOperationException("Cannot change an initialized AudioEngine with this instance");

                audioEngine = value;
            }
        }

        /// <summary>
        /// Occurs when a WaveBank event occurs.
        /// </summary>
        /// <remarks>
        /// Use <see cref="RegisterNotification"/> to register types.
        /// </remarks>
        public event EventHandler<Notification> OnNotification;

        /// <summary>
        /// Called when an internal notification occured.
        /// </summary>
        /// <param name="notification">The notification.</param>
        void OnNotificationDelegate(Notification notification)
        {
            // Dispatch the event
            if (OnNotification != null)
                OnNotification(this, notification);
        }

        /// <summary>
        /// Registers this instance to notify for a type of notification.
        /// </summary>
        /// <param name="notificationType">Type of the notification.</param>
        public void RegisterNotification(NotificationType notificationType)
        {
            if (AudioEngine == null)
                throw new InvalidOperationException("AudioEngine attached to this instance cannot be null");

            var notificationDescription = AudioEngine.VerifyRegister(notificationType, typeof(WaveBank),
                                                                     OnNotificationDelegate);
            notificationDescription.WaveBankPointer = NativePointer;
            AudioEngine.RegisterNotification(ref notificationDescription);
        }

        /// <summary>
        /// Unregisters this instance to notify for a type of notification.
        /// </summary>
        /// <param name="notificationType">Type of the notification.</param>
        public void UnregisterNotification(NotificationType notificationType)
        {
            if (AudioEngine == null)
                throw new InvalidOperationException("AudioEngine attached to this instance cannot be null");

            var notificationDescription = AudioEngine.VerifyRegister(notificationType, typeof(WaveBank),
                                                                     OnNotificationDelegate);
            notificationDescription.WaveBankPointer = NativePointer;
            AudioEngine.UnRegisterNotification(ref notificationDescription);
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="waveIndex">No documentation.</param>	
        /// <param name="flags">No documentation.</param>	
        /// <param name="playOffset">No documentation.</param>	
        /// <param name="loopCount">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IXACT3WaveBank::Prepare']/*"/>	
        /// <unmanaged>HRESULT IXACT3WaveBank::Prepare([In] unsigned short nWaveIndex,[In] XACT_CONTENT_PREPARATION_FLAGS dwFlags,[In] unsigned int dwPlayOffset,[In] unsigned char nLoopCount,[Out] IXACT3Wave** ppWave)</unmanaged>	
        public SharpDX.XACT3.Wave Prepare(short waveIndex, SharpDX.XACT3.ContentPreparationFlags flags, int playOffset, byte loopCount)
        {
            var wave = Prepare(waveIndex, flags, playOffset, loopCount);
            wave.AudioEngine = audioEngine;
            wave.IsAudioEngineReadonly = true;
            return wave;
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="waveIndex">No documentation.</param>	
        /// <param name="flags">No documentation.</param>	
        /// <param name="playOffset">No documentation.</param>	
        /// <param name="loopCount">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IXACT3WaveBank::Play']/*"/>	
        /// <unmanaged>HRESULT IXACT3WaveBank::Play([In] unsigned short nWaveIndex,[In] XACT_CONTENT_PREPARATION_FLAGS dwFlags,[In] unsigned int dwPlayOffset,[In] unsigned char nLoopCount,[Out] IXACT3Wave** ppWave)</unmanaged>	
        public SharpDX.XACT3.Wave Play(short waveIndex, SharpDX.XACT3.ContentPreparationFlags flags, int playOffset, byte loopCount)
        {
            var wave = Play(waveIndex, flags, playOffset, loopCount);
            wave.AudioEngine = audioEngine;
            wave.IsAudioEngineReadonly = true;
            return wave;
        }

        protected override void Dispose(bool disposing)
        {
            if (FileStreamHandle != null && !FileStreamHandle.IsInvalid)
            {
                FileStreamHandle.Dispose();
                FileStreamHandle = null;
            }
            base.Dispose(disposing);
        }

        internal SafeFileHandle FileStreamHandle { get; set; }
    }
}

