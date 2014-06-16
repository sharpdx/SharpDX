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

namespace SharpDX.DirectSound
{
    public partial class I3DL2Reverb
    {
        /// <summary>
        /// Default ratio of the decay time at high frequencies to the decay time at low frequencies.
        /// </summary>
        public const float DecayHFRatioDefault = 0.83f;
        /// <summary>
        /// Maximum ratio of the decay time at high frequencies to the decay time at low frequencies.
        /// </summary>
        public const float DecayHFRatioMax = 2f;
        /// <summary>
        /// Minimum ratio of the decay time at high frequencies to the decay time at low frequencies.
        /// </summary>
        public const float DecayHFRatioMin = 0.1f;
        /// <summary>
        /// Default decay time, in seconds.
        /// </summary>
        public const float DecayTimeDefault = 1.49f;
        /// <summary>
        /// Maximum decay time, in seconds.
        /// </summary>
        public const float DecayTimeMax = 20f;
        /// <summary>
        /// Minimum decay time, in seconds.
        /// </summary>
        public const float DecayTimeMin = 0.1f;
        /// <summary>
        /// Default modal density in the late reverberation decay, in percent.
        /// </summary>
        public const float DensityDefault = 100f;
        /// <summary>
        /// Maximum modal density in the late reverberation decay, in percent.
        /// </summary>
        public const float DensityMax = 100f;
        /// <summary>
        /// Minimum modal density in the late reverberation decay, in percent.
        /// </summary>
        public const float DensityMin = 0f;
        /// <summary>
        /// Default echo density in the late reverberation decay, in percent.
        /// </summary>
        public const float DiffusionDefault = 100f;
        /// <summary>
        /// Maximum echo density in the late reverberation decay, in percent.
        /// </summary>
        public const float DiffusionMax = 100f;
        /// <summary>
        /// Minimum echo density in the late reverberation decay, in percent.
        /// </summary>
        public const float DiffusionMin = 0f;
        /// <summary>
        /// Default reference high frequency, in hertz.
        /// </summary>
        public const float HFReferenceDefault = 5000f;
        /// <summary>
        /// Maximum reference high frequency, in hertz.
        /// </summary>
        public const float HFReferenceMax = 20000f;
        /// <summary>
        /// Minimum reference high frequency, in hertz.
        /// </summary>
        public const float HFReferenceMin = 20f;
        /// <summary>
        /// Default attenuation of early reflections relative to Room, in mB.
        /// </summary>
        public const int ReflectionsDefault = -2602;
        /// <summary>
        /// Default delay time of the first reflection relative to the direct path, in seconds.
        /// </summary>
        public const float ReflectionsDelayDefault = 0.007f;
        /// <summary>
        /// Maximum delay time of the first reflection relative to the direct path, in seconds.
        /// </summary>
        public const float ReflectionsDelayMax = 0.3f;
        /// <summary>
        /// Minimum delay time of the first reflection relative to the direct path, in seconds.
        /// </summary>
        public const float ReflectionsDelayMin = 0f;
        /// <summary>
        /// Maximum attenuation of early reflections relative to Room, in mB.
        /// </summary>
        public const int ReflectionsMax = 0x3e8;
        /// <summary>
        /// Minimum attenuation of early reflections relative to Room, in mB.
        /// </summary>
        public const int ReflectionsMin = -10000;
        /// <summary>
        /// Default attenuation of late reverberation relative to Room, in mB.
        /// </summary>
        public const int ReverbDefault = 200;
        /// <summary>
        /// Default time limit between the early reflections and the late reverberation relative to the time of the first reflection, in seconds.
        /// </summary>
        public const float ReverbDelayDefault = 0.011f;
        /// <summary>
        /// Maximum time limit between the early reflections and the late reverberation relative to the time of the first reflection, in seconds.
        /// </summary>
        public const float ReverbDelayMax = 0.1f;
        /// <summary>
        /// Minimum time limit between the early reflections and the late reverberation relative to the time of the first reflection, in seconds.
        /// </summary>
        public const float ReverbDelayMin = 0f;
        /// <summary>
        /// Maximum attenuation of late reverberation relative to Room, in mB.
        /// </summary>
        public const int ReverbMax = 0x7d0;
        /// <summary>
        /// Minimum attenuation of late reverberation relative to Room, in mB.
        /// </summary>
        public const int ReverbMin = -10000;
        /// <summary>
        /// Default attenuation of the room effect, in millibels (mB).
        /// </summary>
        public const int RoomDefault = -1000;
        /// <summary>
        /// Default attenuation of the room high-frequency effect, in mB.
        /// </summary>
        public const int RoomHFDefault = -100;
        /// <summary>
        /// Maximum attenuation of the room high-frequency effect, in mB.
        /// </summary>
        public const int RoomHFMax = 0;
        /// <summary>
        /// Minimum attenuation of the room high-frequency effect, in mB.
        /// </summary>
        public const int RoomHFMin = -10000;
        /// <summary>
        /// Maximum attenuation of the room effect, in millibels (mB).
        /// </summary>
        public const int RoomMax = 0;
        /// <summary>
        /// Minimum attenuation of the room effect, in millibels (mB).
        /// </summary>
        public const int RoomMin = -10000;
        /// <summary>
        /// Default rolloff factor for the reflected signals. The rolloff factor for the direct path is controlled by the DirectSound listener.
        /// </summary>
        public const float RoomRolloffFactorDefault = 0f;
        /// <summary>
        /// Maximum rolloff factor for the reflected signals. The rolloff factor for the direct path is controlled by the DirectSound listener.
        /// </summary>
        public const float RoomRolloffFactorMax = 10f;
        /// <summary>
        /// Minimum rolloff factor for the reflected signals. The rolloff factor for the direct path is controlled by the DirectSound listener.
        /// </summary>
        public const float RoomRolloffFactorMin = 0f;        
    }
}