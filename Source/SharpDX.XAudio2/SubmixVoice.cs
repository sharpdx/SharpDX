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

namespace SharpDX.XAudio2
{
    public partial class SubmixVoice
    {
        /// <summary>	
        /// Creates and configures a submix voice on the default audio device, with stereo channels at 44100Hz.
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <unmanaged>HRESULT IXAudio2::CreateSubmixVoice([Out] IXAudio2SubmixVoice** ppSubmixVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 DeviceIndex,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SubmixVoice(XAudio2 device)
            : this(device, 2)
        {
        }

        /// <summary>	
        /// Creates and configures a submix voice on the default audio device and 44100Hz.
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="inputChannels">[in]  Number of channels the mastering voice expects in its input audio. InputChannels must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. InputChannels can be set to XAUDIO2_DEFAULT_CHANNELS, with the default being determined by the current platform. Windows  Attempts to detect the system speaker configuration setup.  Xbox 360  Defaults to 5.1 surround.  </param>
        /// <unmanaged>HRESULT IXAudio2::CreateSubmixVoice([Out] IXAudio2SubmixVoice** ppSubmixVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 DeviceIndex,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SubmixVoice(XAudio2 device, int inputChannels)
            : this(device, inputChannels, 44100)
        {
        }

        /// <summary>	
        /// Creates and configures a submix voice on the default audio device.
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="inputChannels">[in]  Number of channels the mastering voice expects in its input audio. InputChannels must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. InputChannels can be set to XAUDIO2_DEFAULT_CHANNELS, with the default being determined by the current platform. Windows  Attempts to detect the system speaker configuration setup.  Xbox 360  Defaults to 5.1 surround.  </param>
        /// <param name="inputSampleRate">[in]  Sample rate of the input audio data of the mastering voice. This rate must be a multiple of XAUDIO2_QUANTUM_DENOMINATOR. InputSampleRate must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. InputSampleRate can be set to XAUDIO2_DEFAULT_SAMPLERATE, with the default being determined by the current platform. Windows  Windows XP defaults to 44100. Windows Vista and Windows 7 default to the setting specified in the Sound Control Panel. The default for this setting is 44100 (or 48000 if required by the driver).  Xbox 360  Defaults to 48000.  </param>
        /// <unmanaged>HRESULT IXAudio2::CreateSubmixVoice([Out] IXAudio2SubmixVoice** ppSubmixVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 DeviceIndex,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SubmixVoice(XAudio2 device, int inputChannels, int inputSampleRate)
            : this(device, inputChannels, inputSampleRate, SubmixVoiceFlags.None, 0)
        {
        }

        /// <summary>	
        /// Creates and configures a submix voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="inputChannels">[in]  Number of channels in the input audio data of the submix voice. InputChannels must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. </param>
        /// <param name="inputSampleRate">[in]  Sample rate of the input audio data of submix voice. This rate must be a multiple of  XAUDIO2_QUANTUM_DENOMINATOR.  InputSampleRate must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. </param>
        /// <param name="flags">[in]  Flags that specify the behavior of the submix voice. Can be 0 or the following: ValueDescriptionXAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.? </param>
        /// <param name="processingStage">[in]  An arbitrary number that specifies when this voice is processed with respect to other submix  voices, if the XAudio2 engine is running other submix voices. The voice is processed after all other  voices that include a smaller ProcessingStage value, and before all other voices  that include a larger ProcessingStage value. Voices that include the same  ProcessingStage value are processed in any order. A submix voice cannot send to  another submix voice with a lower or equal ProcessingStage value; this prevents  audio being lost due to a submix cycle. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSubmixVoice([Out] IXAudio2SubmixVoice** ppSubmixVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 ProcessingStage,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SubmixVoice(XAudio2 device, int inputChannels, int inputSampleRate, SubmixVoiceFlags flags, int processingStage)
            : this(device, inputChannels, inputSampleRate, flags, processingStage, null)
        {
        }

        /// <summary>	
        /// Creates and configures a submix voice with an effect chain.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="inputChannels">[in]  Number of channels in the input audio data of the submix voice. InputChannels must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. </param>
        /// <param name="inputSampleRate">[in]  Sample rate of the input audio data of submix voice. This rate must be a multiple of  XAUDIO2_QUANTUM_DENOMINATOR.  InputSampleRate must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. </param>
        /// <param name="flags">[in]  Flags that specify the behavior of the submix voice. Can be 0 or the following: ValueDescriptionXAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.? </param>
        /// <param name="processingStage">[in]  An arbitrary number that specifies when this voice is processed with respect to other submix  voices, if the XAudio2 engine is running other submix voices. The voice is processed after all other  voices that include a smaller ProcessingStage value, and before all other voices  that include a larger ProcessingStage value. Voices that include the same  ProcessingStage value are processed in any order. A submix voice cannot send to  another submix voice with a lower or equal ProcessingStage value; this prevents  audio being lost due to a submix cycle. </param>
        /// <param name="effectDescriptors">[in, optional] Pointer to a list of XAUDIO2_EFFECT_CHAIN structures that describe an effect chain to use in the submix voice.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSubmixVoice([Out] IXAudio2SubmixVoice** ppSubmixVoice,[None] UINT32 InputChannels,[None] UINT32 InputSampleRate,[None] UINT32 Flags,[None] UINT32 ProcessingStage,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SubmixVoice(XAudio2 device, int inputChannels, int inputSampleRate, SubmixVoiceFlags flags, int processingStage, EffectDescriptor[] effectDescriptors)
            : base(device)
        {

            if (effectDescriptors != null)
            {
                unsafe
                {
                    var tempSendDescriptor = new EffectChain();
                    var effectDescriptorNatives = new EffectDescriptor.__Native[effectDescriptors.Length];
                    for (int i = 0; i < effectDescriptorNatives.Length; i++)
                        effectDescriptors[i].__MarshalTo(ref effectDescriptorNatives[i]);
                    tempSendDescriptor.EffectCount = effectDescriptorNatives.Length;
                    fixed (void* pEffectDescriptors = &effectDescriptorNatives[0])
                    {
                        tempSendDescriptor.EffectDescriptorPointer = (IntPtr)pEffectDescriptors;
                        device.CreateSubmixVoice(this, inputChannels, inputSampleRate, unchecked((int)flags), processingStage, null, tempSendDescriptor);
                    }
                }
            }
            else
            {
                device.CreateSubmixVoice(this, inputChannels, inputSampleRate, unchecked((int)flags), processingStage, null, null);
            }
        }
    }
}
