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

using SharpDX.Multimedia;

namespace SharpDX.XAudio2
{
    public partial class MasteringVoice
    {
        /// <summary>	
        /// Creates and configures a mastering voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="inputChannels">[in]  Number of channels the mastering voice expects in its input audio. InputChannels must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. InputChannels can be set to XAUDIO2_DEFAULT_CHANNELS, with the default being determined by the current platform. Windows  Attempts to detect the system speaker configuration setup.  Xbox 360  Defaults to 5.1 surround.  </param>
        /// <param name="inputSampleRate">[in]  Sample rate of the input audio data of the mastering voice. This rate must be a multiple of XAUDIO2_QUANTUM_DENOMINATOR. InputSampleRate must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. InputSampleRate can be set to XAUDIO2_DEFAULT_SAMPLERATE, with the default being determined by the current platform. Windows  Windows XP defaults to 44100. Windows Vista and Windows 7 default to the setting specified in the Sound Control Panel. The default for this setting is 44100 (or 48000 if required by the driver).  Xbox 360  Defaults to 48000.  </param>
        /// <param name="deviceId">[in]  Index of the output device that will be sent input by the mastering voice. Specifying the default value of 0 causes XAudio2 to select the global default audio device. </param>
        /// <unmanaged>HRESULT IXAudio2::CreateMasteringVoice([Out] IXAudio2MasteringVoice** ppMasteringVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 DeviceIndex,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public MasteringVoice(XAudio2 device, int inputChannels = 2, int inputSampleRate = 44100)
            : base(device)
        {
            if (device.Version == XAudio2Version.Version27)
            {
                device.CreateMasteringVoice27(this, inputChannels, inputSampleRate, 0, 0, null);
            }
            else
            {
                device.CreateMasteringVoice(this, inputChannels, inputSampleRate, 0, null, null, AudioStreamCategory.GameEffects);
            }
        }

        /// <summary>	
        /// Creates and configures a mastering voice (Valid only for XAudio2.8)
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="inputChannels">[in]  Number of channels the mastering voice expects in its input audio. InputChannels must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. InputChannels can be set to XAUDIO2_DEFAULT_CHANNELS, with the default being determined by the current platform. Windows  Attempts to detect the system speaker configuration setup.  Xbox 360  Defaults to 5.1 surround.  </param>
        /// <param name="inputSampleRate">[in]  Sample rate of the input audio data of the mastering voice. This rate must be a multiple of XAUDIO2_QUANTUM_DENOMINATOR. InputSampleRate must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. InputSampleRate can be set to XAUDIO2_DEFAULT_SAMPLERATE, with the default being determined by the current platform. Windows  Windows XP defaults to 44100. Windows Vista and Windows 7 default to the setting specified in the Sound Control Panel. The default for this setting is 44100 (or 48000 if required by the driver).  Xbox 360  Defaults to 48000.  </param>
        /// <param name="deviceId">[in]  Index of the output device that will be sent input by the mastering voice. Specifying the default value of 0 causes XAudio2 to select the global default audio device. </param>
        /// <unmanaged>HRESULT IXAudio2::CreateMasteringVoice([Out] IXAudio2MasteringVoice** ppMasteringVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 DeviceIndex,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public MasteringVoice(XAudio2 device, int inputChannels, int inputSampleRate, string deviceId)
            : base(device)
        {
            if (device.Version == XAudio2Version.Version27)
            {
                throw new InvalidOperationException("This method is only valid on XAudio 2.8 or XAudio 2.9 version");
            }

            device.CreateMasteringVoice(this, inputChannels, inputSampleRate, 0, deviceId, null, AudioStreamCategory.GameEffects);
        }

        /// <summary>
        /// Creates and configures a mastering voice (Valid only for XAudio2.7)
        /// </summary>
        /// <param name="device">an instance of <see cref="SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="inputChannels">[in]  Number of channels the mastering voice expects in its input audio. InputChannels must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. InputChannels can be set to XAUDIO2_DEFAULT_CHANNELS, with the default being determined by the current platform. Windows  Attempts to detect the system speaker configuration setup.  Xbox 360  Defaults to 5.1 surround.</param>
        /// <param name="inputSampleRate">[in]  Sample rate of the input audio data of the mastering voice. This rate must be a multiple of XAUDIO2_QUANTUM_DENOMINATOR. InputSampleRate must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. InputSampleRate can be set to XAUDIO2_DEFAULT_SAMPLERATE, with the default being determined by the current platform. Windows  Windows XP defaults to 44100. Windows Vista and Windows 7 default to the setting specified in the Sound Control Panel. The default for this setting is 44100 (or 48000 if required by the driver).  Xbox 360  Defaults to 48000.</param>
        /// <param name="deviceIndex">Index of the device.</param>
        /// <unmanaged>HRESULT IXAudio2::CreateMasteringVoice([Out] IXAudio2MasteringVoice** ppMasteringVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 DeviceIndex,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public MasteringVoice(XAudio2 device, int inputChannels, int inputSampleRate, int deviceIndex)
            : base(device)
        {
            device.CheckVersion27();
            device.CreateMasteringVoice27(this, inputChannels, inputSampleRate, 0, deviceIndex, null);
        }

        /// <summary>	
        /// <p>Returns the channel mask for this voice. (Only valid for XAudio 2.8, returns 0 otherwise)</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>The <em>pChannelMask</em> argument is a bit-mask of the various channels in the speaker geometry reported by the audio system. This information is needed for the <strong><see cref="SharpDX.X3DAudio.X3DAudio.X3DAudioInitialize"/></strong> <em>SpeakerChannelMask</em> parameter. </p><p>The X3DAUDIO.H header declares a number of <strong>SPEAKER_</strong> positional defines to decode these channels masks. </p><p>Examples include: </p><pre><see cref="SharpDX.Multimedia.Speakers.Stereo"/> // <see cref="SharpDX.Multimedia.Speakers.FrontLeft"/> (0x1) | <see cref="SharpDX.Multimedia.Speakers.FrontRight"/> (0x2)  <see cref="SharpDX.Multimedia.Speakers.FivePointOne"/> // <see cref="SharpDX.Multimedia.Speakers.FrontLeft"/> (0x1) | <see cref="SharpDX.Multimedia.Speakers.FrontRight"/> (0x2) // | <see cref="SharpDX.Multimedia.Speakers.FrontCenter"/> (0x4) // | <see cref="SharpDX.Multimedia.Speakers.LowFrequency"/> (0x8) // | <see cref="SharpDX.Multimedia.Speakers.BackLeft"/> (0x10) | <see cref="SharpDX.Multimedia.Speakers.BackRight"/> (0x20)</pre><p><strong>Note</strong>??For the DirectX SDK versions of XAUDIO, the channel mask for the output device was obtained via the <strong>IXAudio2::GetDeviceDetails</strong> method, which doesn't exist in Windows?8 and later.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IXAudio2MasteringVoice::GetChannelMask']/*"/>	
        /// <msdn-id>microsoft.directx_sdk.ixaudio2masteringvoice.ixaudio2masteringvoice.getchannelmask</msdn-id>	
        /// <unmanaged>GetChannelMask</unmanaged>	
        /// <unmanaged-short>GetChannelMask</unmanaged-short>	
        /// <unmanaged>HRESULT IXAudio2MasteringVoice::GetChannelMask([Out] unsigned int* pChannelmask)</unmanaged>
        public int ChannelMask
        {
            get
            {
                if(device.Version == XAudio2Version.Version27)
                {
                    return 0;
                }

                int __output__; 
                GetChannelMask(out __output__); 
                return __output__;
            }
        }
    }
}