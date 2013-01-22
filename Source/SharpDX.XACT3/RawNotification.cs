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

using System.Runtime.InteropServices;

namespace SharpDX.XACT3
{
    /// <summary>	
    /// Hand written version of XACT_NOTIFICATION in order to smoothly support compatible x86/x64 inner anonymous union.
    /// </summary>	
    /// <unmanaged>XACT_NOTIFICATION</unmanaged>	
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal partial struct RawNotification
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>XACT_NOTIFICATION_TYPE type</unmanaged>	
        public SharpDX.XACT3.NotificationType Type;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int timeStamp</unmanaged>	
        public int TimeStamp;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>void* pvContext</unmanaged>	
        public System.IntPtr ContextPointer;

        /// <summary>
        /// Notification data specific to the <see cref="Type"/>.
        /// </summary>
        public SubData Data;

        [StructLayout(LayoutKind.Explicit)]
        public struct SubData
        {
            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>XACT_NOTIFICATION_CUE cue</unmanaged>	
            [FieldOffset(0)] 
            public SharpDX.XACT3.RawNotificationCue Cue;

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>XACT_NOTIFICATION_MARKER marker</unmanaged>	
            [FieldOffset(0)] 
            public SharpDX.XACT3.RawNotificationMarker Marker;

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>XACT_NOTIFICATION_SOUNDBANK soundBank</unmanaged>	
            [FieldOffset(0)]
            public SharpDX.XACT3.RawNotificationSoundbank SoundBank;

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>XACT_NOTIFICATION_WAVEBANK waveBank</unmanaged>	
            [FieldOffset(0)]
            public SharpDX.XACT3.RawNotificationWavebank WaveBank;

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>XACT_NOTIFICATION_VARIABLE variable</unmanaged>	
            [FieldOffset(0)]
            public SharpDX.XACT3.RawNotificationVariable Variable;

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>XACT_NOTIFICATION_GUI gui</unmanaged>	
            [FieldOffset(0)] 
            public SharpDX.XACT3.RawNotificationGui Gui;

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>XACT_NOTIFICATION_WAVE wave</unmanaged>	
            [FieldOffset(0)] 
            public SharpDX.XACT3.RawNotificationWave Wave;
        }
    }
}

