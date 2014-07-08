// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpDX.XACT3
{
    public partial class SoundBank
    {
        private DataStream soundBankSourceStream;
        private AudioEngine audioEngine;
        private readonly bool isAudioEngineReadonly;

        private ManagedNotificationCallback callback;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundBank"/> class from a soundbank stream.
        /// </summary>
        /// <param name="audioEngine">The engine.</param>
        /// <param name="stream">The soundbank stream stream.</param>
        /// <unmanaged>HRESULT IXACT3Engine::CreateSoundBank([In] const void* pvBuffer,[In] unsigned int dwSize,[In] unsigned int dwFlags,[In] unsigned int dwAllocAttributes,[Out, Fast] IXACT3SoundBank** ppSoundBank)</unmanaged>
        public SoundBank(AudioEngine audioEngine, Stream stream)
        {
            this.audioEngine = audioEngine;
            isAudioEngineReadonly = true;
            soundBankSourceStream = stream as DataStream ?? DataStream.Create(Utilities.ReadStream(stream), true, true);
            audioEngine.CreateSoundBank(soundBankSourceStream.PositionPointer, (int)(soundBankSourceStream.Length - soundBankSourceStream.Position), 0, 0, this);
            callback = OnNotificationDelegate;
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


        public SharpDX.XACT3.Cue Play(short cueIndex)
        {
            return Play(cueIndex, 0);
        }

        public SharpDX.XACT3.Cue Play(short cueIndex, int timeOffset)
        {
            var cue = Play(cueIndex, 0, timeOffset);
            cue.AudioEngine = AudioEngine;
            cue.IsAudioEngineReadonly = true;
            return cue;
        }

        public SharpDX.XACT3.Cue Prepare(short cueIndex)
        {
            return Prepare(cueIndex, 0);
        }

        public SharpDX.XACT3.Cue Prepare(short cueIndex, int timeOffset)
        {
            var cue = Prepare(cueIndex, 0, timeOffset);
            cue.AudioEngine = AudioEngine;
            cue.IsAudioEngineReadonly = true;
            return cue;
        }

        /// <summary>
        /// Occurs when a Soundbank event occurs.
        /// </summary>
        /// <remarks>
        /// Use <see cref="RegisterNotification"/> to register types.
        /// </remarks>
        public event EventHandler<Notification> OnNotification;

        /// <summary>
        /// Called when an internal notification occurred.
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

            var notificationDescription = AudioEngine.VerifyRegister(notificationType, typeof(SoundBank), callback ?? (callback = OnNotificationDelegate));
            notificationDescription.SoundBankPointer = NativePointer;
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

            var notificationDescription = AudioEngine.VerifyRegister(notificationType, typeof(SoundBank), callback ?? (callback = OnNotificationDelegate));
            notificationDescription.SoundBankPointer = NativePointer;
            AudioEngine.UnRegisterNotification(ref notificationDescription);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (soundBankSourceStream != null)
                {
                    soundBankSourceStream.Dispose();
                    soundBankSourceStream = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is referenced by at least one valid cue instance or other client. For example, the game itself might reference the sound bank. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in use; otherwise, <c>false</c>.
        /// </value>
        public bool IsInUse
        {
            get
            {
                int state;
                GetState(out state);
                return state != 0;
            }
        }        
    }
}