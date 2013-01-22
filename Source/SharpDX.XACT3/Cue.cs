// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.XACT3
{
    public partial class Cue
    {
        private AudioEngine audioEngine;
        internal bool IsAudioEngineReadonly;

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
                if (IsAudioEngineReadonly)
                    throw new InvalidOperationException("Cannot change an initialized AudioEngine with this instance");
                audioEngine = value;
            }
        }

        /// <summary>
        /// Occurs when a cue event occurs.
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

            var notificationDescription = AudioEngine.VerifyRegister(notificationType, typeof (Cue),
                                                                     OnNotificationDelegate);
            notificationDescription.CuePointer = NativePointer;
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

            var notificationDescription = AudioEngine.VerifyRegister(notificationType, typeof(Cue),
                                                                     OnNotificationDelegate);
            notificationDescription.CuePointer = NativePointer;
            AudioEngine.UnRegisterNotification(ref notificationDescription);
        }
    }
}