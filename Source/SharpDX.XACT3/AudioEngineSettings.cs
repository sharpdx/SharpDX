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
using System.IO;

namespace SharpDX.XACT3
{
    /// <summary>
    /// Settings for the <see cref="AudioEngine"/>.
    /// </summary>
    public partial class AudioEngineSettings
    {
        /// <summary>
        /// Default value for <see cref="LookAheadTime"/> property (250ms).
        /// </summary>
        public const int DefaultLookAhead = 250;

        private XAudio2.XAudio2 xAudio2;
        private XAudio2.MasteringVoice masteringVoice;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEngineSettings"/> class.
        /// </summary>
        public AudioEngineSettings()
        {
            LookAheadTime = DefaultLookAhead;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEngineSettings"/> struct.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public AudioEngineSettings(Stream settings) : this()
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets or sets the settings file.
        /// </summary>
        /// <value>
        /// The settings file.
        /// </value>
        public Stream Settings { get; set; }

        /// <summary>
        /// Gets or sets the XAudio2 engine to use with XACT3 engine.
        /// </summary>
        /// <value>
        /// The XAudio2 engine.
        /// </value>
        public XAudio2.XAudio2 XAudio2
        {
            get
            {
                if (xAudio2 == null)
                {
                    if (XAudio2Pointer != IntPtr.Zero)
                        xAudio2 = new XAudio2.XAudio2(XAudio2Pointer);
                }
                return xAudio2;
            }
            set
            {
                xAudio2 = value;
                XAudio2Pointer = xAudio2 != null ? xAudio2.NativePointer : IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets or sets the MasteringVoice to use with XACT3 engine.
        /// </summary>
        /// <value>
        /// The MasteringVoice.
        /// </value>
        /// <remarks>
        /// <see cref="XAudio2"/> field must also be set to a valid XAudio2 instance.
        /// </remarks>
        public XAudio2.MasteringVoice MasteringVoice
        {
            get
            {
                if (masteringVoice == null)
                {
                    if (MasteringVoicePointer != IntPtr.Zero)
                        masteringVoice = new XAudio2.MasteringVoice(MasteringVoicePointer);
                }
                return masteringVoice;
            }
            set
            {
                masteringVoice = value;
                MasteringVoicePointer = masteringVoice != null ? masteringVoice.NativePointer : IntPtr.Zero;
            }
        }
    }
}