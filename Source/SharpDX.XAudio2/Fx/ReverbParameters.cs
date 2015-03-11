// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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

namespace SharpDX.XAudio2.Fx
{
    public partial struct ReverbParameters
    {
        /// <summary>
        /// Defines an explicit conversion of a <see cref="SharpDX.XAudio2.Fx.ReverbI3DL2Parameters"/> to <see cref="SharpDX.XAudio2.Fx.ReverbParameters"/>.
        /// </summary>
        /// <param name="I3DL2Parameters">The value to convert.</param>
        /// <returns>A <see cref="SharpDX.XAudio2.Fx.ReverbParameters"/> that represents the converted <see cref="SharpDX.XAudio2.Fx.ReverbI3DL2Parameters"/>.</returns>
        /// <remarks>
        /// The <see cref="SharpDX.XAudio2.Fx.ReverbI3DL2Parameters"/> is converted to <see cref="SharpDX.XAudio2.Fx.ReverbParameters"/>
        /// using the same logic as the inline function "ReverbConvertI3DL2ToNative" from "XAudio2fx.h".
        /// </remarks>
        public static explicit operator ReverbParameters(ReverbI3DL2Parameters I3DL2Parameters)
        {
            ReverbParameters native = new ReverbParameters();

            float reflectionsDelay;
            float reverbDelay;

            // RoomRolloffFactor is ignored

            // These parameters have no equivalent in I3DL2
            native.RearDelay = Reverb.DefaultRearDelay; // 5
            native.PositionLeft = Reverb.DefaultPosition; // 6
            native.PositionRight = Reverb.DefaultPosition; // 6
            native.PositionMatrixLeft = Reverb.DefaultPositionMatrix; // 27
            native.PositionMatrixRight = Reverb.DefaultPositionMatrix; // 27
            native.RoomSize = Reverb.DefaultRoomSize; // 100
            native.LowEQCutoff = 4;
            native.HighEQCutoff = 6;

            // The rest of the I3DL2 parameters map to the native property set
            native.RoomFilterMain = (float)I3DL2Parameters.Room / 100.0f;
            native.RoomFilterHF = (float)I3DL2Parameters.RoomHF / 100.0f;

            if (I3DL2Parameters.DecayHFRatio >= 1.0f)
            {
                int index = (int)(-4.0 * Math.Log10(I3DL2Parameters.DecayHFRatio));
                if (index < -8) index = -8;
                native.LowEQGain = (byte)((index < 0) ? index + 8 : 8);
                native.HighEQGain = 8;
                native.DecayTime = I3DL2Parameters.DecayTime * I3DL2Parameters.DecayHFRatio;
            }
            else
            {
                int index = (int)(4.0 * Math.Log10(I3DL2Parameters.DecayHFRatio));
                if (index < -8) index = -8;
                native.LowEQGain = 8;
                native.HighEQGain = (byte)((index < 0) ? index + 8 : 8);
                native.DecayTime = I3DL2Parameters.DecayTime;
            }

            reflectionsDelay = I3DL2Parameters.ReflectionsDelay * 1000.0f;
            if (reflectionsDelay >= Reverb.MaxReflectionsDelay) // 300
            {
                reflectionsDelay = (float)(Reverb.MaxReflectionsDelay - 1);
            }
            else if (reflectionsDelay <= 1)
            {
                reflectionsDelay = 1;
            }
            native.ReflectionsDelay = (int)reflectionsDelay;

            reverbDelay = I3DL2Parameters.ReverbDelay * 1000.0f;
            if (reverbDelay >= Reverb.MaxReverbDelay) // 85
            {
                reverbDelay = (float)(Reverb.MaxReverbDelay - 1);
            }
            native.ReverbDelay = (byte)reverbDelay;

            native.ReflectionsGain = I3DL2Parameters.Reflections / 100.0f;
            native.ReverbGain = I3DL2Parameters.Reverb / 100.0f;
            native.EarlyDiffusion = (byte)(15.0f * I3DL2Parameters.Diffusion / 100.0f);
            native.LateDiffusion = native.EarlyDiffusion;
            native.Density = I3DL2Parameters.Density;
            native.RoomFilterFreq = I3DL2Parameters.HFReference;

            native.WetDryMix = I3DL2Parameters.WetDryMix;
            return native;
        }
    }
}