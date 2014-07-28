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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.DirectSound
{
    public partial class DirectSound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSound"/> class.
        /// </summary>
        public DirectSound() : base(IntPtr.Zero)
        {
            DSound.Create8(null, this, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSound"/> class.
        /// </summary>
        /// <param name="driverGuid">The driver GUID.</param>
        public DirectSound(Guid driverGuid) : base(IntPtr.Zero)
        {
            DSound.Create8(driverGuid, this, null);
        }

        /// <summary>
        /// Verifies the certification.
        /// </summary>
        /// <returns>Return true if the driver is certified</returns>
        public bool IsCertified
        {
            get
            {
                int verify;
                this.VerifyCertification(out verify);
                return verify == 0;
            }
        }

        /// <summary>
        /// Retrieves the speaker configuration of the device.
        /// </summary>
        /// <param name="speakerSet" />
        /// <param name="geometry" />
        public void GetSpeakerConfiguration(out SpeakerConfiguration speakerSet, out SpeakerGeometry geometry)
        {
            int speakerConfig;
            GetSpeakerConfiguration(out speakerConfig);
            speakerSet = (SpeakerConfiguration)(speakerConfig & 0xFFFF);
            geometry = (SpeakerGeometry)(speakerConfig >> 16);
        }

        /// <summary>
        /// Sets the speaker configuration of the device.
        /// </summary>
        /// <param name="speakerSet" />
        /// <param name="geometry" />
        public void SetSpeakerConfiguration(SpeakerConfiguration speakerSet, SpeakerGeometry geometry)
        {
            SetSpeakerConfiguration(((int)speakerSet) | (((int)geometry) << 16));
        }

        /// <summary>
        /// Enumerates the DirectSound devices installed in the system.
        /// </summary>
        /// <returns>A collection of the devices found.</returns>
        public static List<DeviceInformation> GetDevices()
        {
            var callback = new EnumDelegateCallback();
            DSound.EnumerateW(callback.NativePointer, IntPtr.Zero);
            return callback.Informations;
        }

        /// <summary>
        /// Duplicates the sound buffer.
        /// </summary>
        /// <param name="sourceBuffer">The source buffer.</param>
        /// <returns>A duplicate of this soundBuffer.</returns>
        /// <unmanaged>HRESULT IDirectSound::DuplicateSoundBuffer([In] IDirectSoundBuffer* pDSBufferOriginal,[Out] void** ppDSBufferDuplicate)</unmanaged>
        ///   <unmanaged-short>IDirectSound::DuplicateSoundBuffer</unmanaged-short>
        public SoundBuffer DuplicateSoundBuffer(SoundBuffer sourceBuffer)
        {
            IntPtr soundBufferPtr;
            var result = DuplicateSoundBuffer(sourceBuffer, out soundBufferPtr);
            SoundBuffer soundBuffer = null;

            if (result.Success && soundBufferPtr != IntPtr.Zero)
            {
                soundBuffer = QueryInterfaceOrNull<PrimarySoundBuffer>(soundBufferPtr) ?? (SoundBuffer)QueryInterfaceOrNull<SecondarySoundBuffer>(soundBufferPtr);
            }

            if (soundBuffer != null)
            {
                Marshal.Release(soundBufferPtr);
            }

            return soundBuffer;
        }
    }
}