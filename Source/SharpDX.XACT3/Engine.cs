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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SharpDX.XACT3
{
    public partial class Engine
    {
        private const string DebugEngineRegistryKey = "Software\\Microsoft\\XACT";
        private const string DebugEngineRegistryValue = "DebugEngine";

        public Engine() : this(CreationFlags.None)
        {
        }

        public Engine(CreationFlags creationFlags)
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
        }

        private Dictionary<IntPtr, Cue> mapCues;
        private Dictionary<IntPtr, SoundBank> mapSoundBanks;

        Cue FindRegisteredCue(IntPtr cuePointer)
        {
            Cue temp;
            mapCues.TryGetValue(cuePointer, out temp);
            return temp;
        }

        SoundBank FindRegisteredSoundBank(IntPtr soundBankPointer)
        {
            SoundBank temp;
            mapSoundBanks.TryGetValue(soundBankPointer, out temp);
            return temp;
        }

        private unsafe static void InitializeEventArgsBase(EngineEvent args, RawNotification* notification)
        {
            args.Type = notification->Type;
            args.Context = Marshal.GetObjectForIUnknown(notification->ContextPointer);
            args.TimeStamp = notification->TimeStamp;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate void NotificationCallbackDelegate(RawNotification* notification);
        private unsafe void NotificationCallbackDelegateImpl(RawNotification* notification)
        {
            // Don't do anything if there is no OnEngineEvent
            if (OnEngineEvent == null)
                return;

            
            EngineEvent engineEventArgs = null;

            switch (notification->Type)
            {
                case NotificationType.Cuedestroyed:
                case NotificationType.Cueplay:
                case NotificationType.Cueprepared:
                case NotificationType.Cuestop:
                    engineEventArgs = new CueEvent
                                       {
                                           CueIndex = notification->Data.Cue.CueIndex,
                                           Cue = FindRegisteredCue(notification->Data.Cue.CuePointer),
                                           SoundBank = FindRegisteredSoundBank(notification->Data.Cue.SoundBankPointer)
                                       };
                    break;
            }

            InitializeEventArgsBase(engineEventArgs, notification);

            // Dispatch the event
            if (OnEngineEvent != null)
                OnEngineEvent(this, engineEventArgs);
        }

        /// <summary>
        /// Occurs when an engine event occurs.
        /// </summary>
        public event EventHandler<EngineEvent> OnEngineEvent;
    }
}

