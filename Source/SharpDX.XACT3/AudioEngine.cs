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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SharpDX.XACT3
{
    internal delegate void ManagedNotificationCallback(Notification notification);

    public partial class AudioEngine
    {
        private const string DebugEngineRegistryKey = "Software\\Microsoft\\XACT";
        private const string DebugEngineRegistryValue = "DebugEngine";

        private readonly NotificationCallbackDelegate unmanagedDelegate;
        private readonly IntPtr unmanagedDelegatePointer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEngine"/> class.
        /// </summary>
        public AudioEngine() : this(CreationFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEngine"/> class.
        /// </summary>
        /// <param name="settingsFile">The settings file.</param>
        public AudioEngine(Stream settingsFile) : this(CreationFlags.None, settingsFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEngine"/> class.
        /// </summary>
        /// <param name="creationFlags">The creation flags.</param>
        /// <param name="settingsFile"></param>
        public AudioEngine(CreationFlags creationFlags, Stream settingsFile = null) : this(creationFlags, new AudioEngineSettings(settingsFile))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEngine"/> class.
        /// </summary>
        /// <param name="creationFlags">The creation flags.</param>
        /// <param name="settings">Settings for this audio engine</param>
        public AudioEngine(CreationFlags creationFlags, AudioEngineSettings settings)
        {
            bool debug = (creationFlags == CreationFlags.DebugMode);
            bool audition = (creationFlags == CreationFlags.AuditionMode);

            var debugRegistryKey = Registry.LocalMachine.OpenSubKey(DebugEngineRegistryKey);

            // If neither the debug nor audition flags are set, see if the debug registry key is set
            if (!debug && !audition && debugRegistryKey != null)
            {
                var value = debugRegistryKey.GetValue(DebugEngineRegistryValue);

                if (value is Int32 && ((int)value) != 0)
                    debug = true;

                debugRegistryKey.Close();
            }

            var selectedEngineCLSID = (debug) ? DebugEngineGuid : (audition) ? AuditionEngineGuid : EngineGuid;

            Utilities.CreateComInstance(selectedEngineCLSID, Utilities.CLSCTX.ClsctxInprocServer, Utilities.GetGuidFromType(typeof(AudioEngine)), this);

            unsafe
            {
                unmanagedDelegate = new NotificationCallbackDelegate(NotificationCallbackDelegateImpl);
                unmanagedDelegatePointer = Marshal.GetFunctionPointerForDelegate(unmanagedDelegate);
            }

            // Initialize the engine
            PreInitialize(settings);
            Initialize(settings);
        }

        /// <summary>
        /// Initializes this instance from a settings file and a renderer index.
        /// </summary>
        /// <unmanaged>IXACT3Engine::Initialize</unmanaged>
		private unsafe void PreInitialize(AudioEngineSettings runtimeParameters)
		{
            if (runtimeParameters.LookAheadTime == 0)
                runtimeParameters.LookAheadTime = AudioEngineSettings.DefaultLookAhead;

            runtimeParameters.FnNotificationCallback = unmanagedDelegatePointer;

            var settingsFile = runtimeParameters.Settings;

            // Init from a settings file
            if (settingsFile != null)
            {
                settingsFile.Position = 0;
                var settingsData = new byte[settingsFile.Length];
                settingsFile.Read(settingsData, 0, settingsData.Length);
                runtimeParameters.GlobalSettingsBufferPointer = Marshal.AllocCoTaskMem(settingsData.Length);
                runtimeParameters.GlobalSettingsBufferSize = settingsData.Length;
                runtimeParameters.GlobalSettingsFlags = 1;
                Utilities.Write(runtimeParameters.GlobalSettingsBufferPointer, settingsData, 0, settingsData.Length);
            }
		}

        /// <summary>
        /// Occurs when a AudioEngine event occurs.
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
            var notificationDescription = VerifyRegister(notificationType, typeof(AudioEngine),
                                                                     OnNotificationDelegate);
            notificationDescription.SoundBankPointer = NativePointer;
            RegisterNotification(ref notificationDescription);
        }

        /// <summary>
        /// Unregisters this instance to notify for a type of notification.
        /// </summary>
        /// <param name="notificationType">Type of the notification.</param>
        public void UnregisterNotification(NotificationType notificationType)
        {
            var notificationDescription = VerifyRegister(notificationType, typeof(AudioEngine),
                                                                     OnNotificationDelegate);
            notificationDescription.SoundBankPointer = NativePointer;
            UnRegisterNotification(ref notificationDescription);
        }

        /// <summary>
        /// Internal unmanaged delegate
        /// </summary>
        /// <param name="notification">The notification.</param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate void NotificationCallbackDelegate(RawNotification* notification);
        private static unsafe void NotificationCallbackDelegateImpl(RawNotification* rawNotification)
        {
            if (rawNotification == (void*)0 || rawNotification->ContextPointer == IntPtr.Zero)
                return;

            var callback = (ManagedNotificationCallback)Marshal.GetDelegateForFunctionPointer(rawNotification->ContextPointer, typeof(ManagedNotificationCallback));

            switch (rawNotification->Type)
            {
                case NotificationType.CueDestroyed:
                case NotificationType.CuePlay:
                case NotificationType.CuePrepared:
                case NotificationType.CueStop:
                    callback(new CueNotification(rawNotification));
                    break;
                case NotificationType.WaveBankDestroyed:
                case NotificationType.WaveBankPrepared:
                case NotificationType.WaveBankStreamingInvalidContent:
                    callback(new WaveBankNotification(rawNotification));
                    break;
                case NotificationType.WaveDestroyed:
                case NotificationType.WaveLooped:
                case NotificationType.WavePlay:
                case NotificationType.WavePrepared:
                case NotificationType.WaveStop:
                    callback(new WaveNotification(rawNotification));
                    break;
                case NotificationType.Marker:
                    callback(new MarkerNotification(rawNotification));
                    break;
                case NotificationType.GuiConnected:
                case NotificationType.GuiDisconnected:
                    callback(new GuiNotification(rawNotification));
                    break;
                case NotificationType.GlobalVariableChanged:
                case NotificationType.LocalVariableChanged:
                    callback(new VariableNotification(rawNotification));
                    break;
                case NotificationType.SoundBankDestroyed:
                    callback(new SoundBankNotification(rawNotification));
                    break;
            }
        }

        /// <summary>
        /// Internal dictionary to verify allowed notifications.
        /// </summary>
        private static readonly Dictionary<NotificationType, List<Type>> AllowedNotifications =
            new Dictionary<NotificationType, List<Type>>()
                {
{ NotificationType.CuePrepared, new List<Type>() {typeof(AudioEngine), typeof(SoundBank), /*typeof(SoundBank) and cue index,*/ typeof(Cue)}},
{ NotificationType.CuePlay, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue)}},
{ NotificationType.CueStop, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue)}},
{ NotificationType.CueDestroyed, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue)}},
{ NotificationType.Marker, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue)}},
{ NotificationType.SoundBankDestroyed, new List<Type>() {typeof(AudioEngine), typeof(SoundBank)}},
{ NotificationType.WaveBankDestroyed, new List<Type>() {typeof(AudioEngine), typeof(WaveBank)}},
{ NotificationType.LocalVariableChanged, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue)}},
{ NotificationType.GlobalVariableChanged, new List<Type>() {typeof(AudioEngine)}},
{ NotificationType.GuiConnected, new List<Type>() {typeof(AudioEngine)}},
{ NotificationType.GuiDisconnected, new List<Type>() {typeof(AudioEngine)}},
{ NotificationType.WavePrepared, new List<Type>() {typeof(AudioEngine),  /*typeof(WaveBank) and wave index,*/ typeof(Wave)}},
{ NotificationType.WavePlay, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue), typeof(WaveBank)}},
{ NotificationType.WaveStop, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue), typeof(WaveBank)}},
{ NotificationType.WaveLooped, new List<Type>() {typeof(AudioEngine), typeof(SoundBank),  /*typeof(SoundBank) and cue index,*/ typeof(Cue), typeof(WaveBank), typeof(Wave)}},
{ NotificationType.WaveDestroyed, new List<Type>() {typeof(AudioEngine),  /*typeof(WaveBank) and wave index,*/ typeof(Wave)}},
{ NotificationType.WaveBankPrepared, new List<Type>() {typeof(AudioEngine), typeof(WaveBank)}},
{ NotificationType.WaveBankStreamingInvalidContent, new List<Type>() {typeof(AudioEngine), typeof(WaveBank) }},
                };

        /// <summary>
        /// Verifies a notification registration for a particular type.
        /// </summary>
        /// <param name="notificationType">Type of the notification.</param>
        /// <param name="type">The type.</param>
        /// <param name="notificationCallback">The notification callback.</param>
        /// <exception cref="InvalidOperationException">If this registration is invalid</exception>
        /// <returns></returns>
        internal static RawNotificationDescription VerifyRegister(NotificationType notificationType, Type type, ManagedNotificationCallback notificationCallback)
        {
            if (!AllowedNotifications[notificationType].Contains(type))
                throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Register to notification [{0}] not supported for type [{1}]", notificationType, type));
            return new RawNotificationDescription()
            {
                Type = notificationType,
                ContextPointer = Marshal.GetFunctionPointerForDelegate(notificationCallback),
                Flags = 1,
                CueIndex = -1,
                WaveIndex = -1,
            };
        }
    }
}

