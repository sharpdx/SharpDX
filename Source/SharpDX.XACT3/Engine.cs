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
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using SharpDX.Win32;

namespace SharpDX.XACT3
{
    internal delegate void ManagedNotificationCallback(Notification notification);

    public partial class Engine
    {
        private const int DefaultLookAhead = 250;
        private const string DebugEngineRegistryKey = "Software\\Microsoft\\XACT";
        private const string DebugEngineRegistryValue = "DebugEngine";

        private readonly NotificationCallbackDelegate unmanagedDelegate;
        private readonly IntPtr unmanagedDelegatePointer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        public Engine() : this(CreationFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="settingsFile">The settings file.</param>
        public Engine(Stream settingsFile) : this(CreationFlags.None, settingsFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="creationFlags">The creation flags.</param>
        /// <param name="settingsFile"></param>
        public Engine(CreationFlags creationFlags, Stream settingsFile = null)
        {
            bool debug = (creationFlags == CreationFlags.DebugMode);
            bool audition = (creationFlags == CreationFlags.AuditionMode);

            var debugRegistryKey = Registry.LocalMachine.OpenSubKey(DebugEngineRegistryKey);

            // If neither the debug nor audition flags are set, see if the debug registry key is set
            if (!debug && !audition && debugRegistryKey != null)
            {
                var value = debugRegistryKey.GetValue(DebugEngineRegistryValue);

                if (value is Int32 && ((int) value) != 0)
                    debug = true;

                debugRegistryKey.Close();
            }            
            
            var selectedEngineCLSID = (debug) ? DebugEngineGuid : (audition) ? AuditionEngineGuid : EngineGuid;

            IntPtr temp;
            var result = Utilities.CoCreateInstance(selectedEngineCLSID, IntPtr.Zero, Utilities.CLSCTX.ClsctxInprocServer, typeof(Engine).GUID, out temp);
            result.CheckError();
            NativePointer = temp;

            unsafe
            {
                unmanagedDelegate = new NotificationCallbackDelegate(NotificationCallbackDelegateImpl);
                unmanagedDelegatePointer = Marshal.GetFunctionPointerForDelegate(unmanagedDelegate);
            }

            // Initialize the engine
            Initialize(settingsFile, null);
        }

        /// <summary>
        /// Initializes this instance from a settings file and a renderer index.
        /// </summary>
        /// <param name="settingsFile">The settings file.</param>
        /// <param name="rendererIndex">Index of the renderer.</param>
        /// <unmanaged>IXACT3Engine::Initialize</unmanaged>
		private unsafe void Initialize(Stream settingsFile, short? rendererIndex)
		{
            var runtimeParameters = new RuntimeParameters
                                        {
                                            LookAheadTime = DefaultLookAhead,
                                            FnNotificationCallback = unmanagedDelegatePointer,
                                        };

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

            // Init with a RendererIndex
            if (rendererIndex.HasValue)
            {
                var rendererDetails = GetRendererDetails(rendererIndex.Value);
                runtimeParameters.RendererId = rendererDetails.RendererId;
            }

            // Final init
            Initialize(ref runtimeParameters);
		}

        /// <summary>
        /// Occurs when an engine event occurs.
        /// </summary>
        public event EventHandler<Notification> OnNotification;


        void OnNotificationDelegate(Notification notification)
        {
            // Dispatch the event
            if (OnNotification != null)
                OnNotification(this, notification);            
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate void NotificationCallbackDelegate(RawNotification* notification);
        private static unsafe void NotificationCallbackDelegateImpl(RawNotification* rawNotification)
        {
            var callback = (ManagedNotificationCallback)Marshal.GetDelegateForFunctionPointer(rawNotification->ContextPointer, typeof(ManagedNotificationCallback));

            Notification notification = null;

            switch (rawNotification->Type)
            {
                case NotificationType.Cuedestroyed:
                case NotificationType.Cueplay:
                case NotificationType.Cueprepared:
                case NotificationType.Cuestop:
                    notification = new CueNotification(rawNotification);
                    break;
            }

            // Notify client
            callback(notification);
        }
    }
}

