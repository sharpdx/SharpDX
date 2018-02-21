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

using System.Runtime.InteropServices;

using SharpDX.Multimedia;

namespace SharpDX.XAudio2
{
    public partial class SourceVoice
    {
        private VoiceCallbackImpl voiceCallbackImpl;

        /// <summary>	
        /// Creates and configures a source voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat"/> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat"/> have a <see cref="SharpDX.Multimedia.WaveFormat"/> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat"/> structure and use it as the value for pSourceFormat. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat)
            : this(device, sourceFormat, true)
        {
        }

        /// <summary>	
        /// Creates and configures a source voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat"/> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat"/> have a <see cref="SharpDX.Multimedia.WaveFormat"/> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat"/> structure and use it as the value for pSourceFormat. </param>
        /// <param name="enableCallbackEvents">True to enable delegate callbacks on this instance. Default is false</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, bool enableCallbackEvents)
            : this(device, sourceFormat, VoiceFlags.None, 1.0f, enableCallbackEvents)
        {
        }

        /// <summary>	
        /// Creates and configures a source voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat"/> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat"/> have a <see cref="SharpDX.Multimedia.WaveFormat"/> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat"/> structure and use it as the value for pSourceFormat. </param>
        /// <param name="flags">[in]  Flags that specify the behavior of the source voice. A flag can be 0 or a combination of one or more of the following: ValueDescriptionXAUDIO2_VOICE_NOPITCHNo pitch control is available on the voice.?XAUDIO2_VOICE_NOSRCNo sample rate conversion is available on the voice, the voice's  outputs must have the same sample rate.Note The XAUDIO2_VOICE_NOSRC flag causes the voice to behave as though the XAUDIO2_VOICE_NOPITCH flag also is specified. ?XAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.?XAUDIO2_VOICE_MUSICThe voice is used to play background music. The system automatically  can replace the voice with music selected by the user.? </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags)
            : this(device, sourceFormat, flags, true)
        {
        }

        /// <summary>	
        /// Creates and configures a source voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat"/> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat"/> have a <see cref="SharpDX.Multimedia.WaveFormat"/> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat"/> structure and use it as the value for pSourceFormat. </param>
        /// <param name="flags">[in]  Flags that specify the behavior of the source voice. A flag can be 0 or a combination of one or more of the following: ValueDescriptionXAUDIO2_VOICE_NOPITCHNo pitch control is available on the voice.?XAUDIO2_VOICE_NOSRCNo sample rate conversion is available on the voice, the voice's  outputs must have the same sample rate.Note The XAUDIO2_VOICE_NOSRC flag causes the voice to behave as though the XAUDIO2_VOICE_NOPITCH flag also is specified. ?XAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.?XAUDIO2_VOICE_MUSICThe voice is used to play background music. The system automatically  can replace the voice with music selected by the user.? </param>
        /// <param name="enableCallbackEvents">True to enable delegate callbacks on this instance. Default is false</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags, bool enableCallbackEvents)
            : this(device, sourceFormat, flags, 1.0f, enableCallbackEvents)
        {
        }


        /// <summary>	
        /// Creates and configures a source voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat"/> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat"/> have a <see cref="SharpDX.Multimedia.WaveFormat"/> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat"/> structure and use it as the value for pSourceFormat. </param>
        /// <param name="flags">[in]  Flags that specify the behavior of the source voice. A flag can be 0 or a combination of one or more of the following: ValueDescriptionXAUDIO2_VOICE_NOPITCHNo pitch control is available on the voice.?XAUDIO2_VOICE_NOSRCNo sample rate conversion is available on the voice, the voice's  outputs must have the same sample rate.Note The XAUDIO2_VOICE_NOSRC flag causes the voice to behave as though the XAUDIO2_VOICE_NOPITCH flag also is specified. ?XAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.?XAUDIO2_VOICE_MUSICThe voice is used to play background music. The system automatically  can replace the voice with music selected by the user.? </param>
        /// <param name="maxFrequencyRatio">[in]  Highest allowable frequency ratio that can be set on this voice. The value for this argument must be between XAUDIO2_MIN_FREQ_RATIO and XAUDIO2_MAX_FREQ_RATIO. Subsequent calls to <see cref="SharpDX.XAudio2.SourceVoice.SetFrequencyRatio"/> are clamped between XAUDIO2_MIN_FREQ_RATIO and MaxFrequencyRatio. The maximum value for this argument is defined as XAUDIO2_MAX_FREQ_RATIO, which allows pitch to be raised by up to 10 octaves. If MaxFrequencyRatio is less than 1.0, the voice will use that ratio immediately after being created (rather than the default of 1.0). Xbox 360  For XMA voices there is an additional restriction on the MaxFrequencyRatio argument and the voice's sample rate. The product of these two numbers cannot exceed XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MONO for one-channel voices or XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MULTICHANNEL for voices with any other number of channels. If the value specified for MaxFrequencyRatio is too high for the specified format, the call to CreateSourceVoice fails and produces a debug message.  Note XAudio2's memory usage can be reduced by using the lowest possible MaxFrequencyRatio value. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags, float maxFrequencyRatio)
            : this(device, sourceFormat, flags, maxFrequencyRatio, true)
        {
        }

        /// <summary>	
        /// Creates and configures a source voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat"/> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat"/> have a <see cref="SharpDX.Multimedia.WaveFormat"/> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat"/> structure and use it as the value for pSourceFormat. </param>
        /// <param name="flags">[in]  Flags that specify the behavior of the source voice. A flag can be 0 or a combination of one or more of the following: ValueDescriptionXAUDIO2_VOICE_NOPITCHNo pitch control is available on the voice.?XAUDIO2_VOICE_NOSRCNo sample rate conversion is available on the voice, the voice's  outputs must have the same sample rate.Note The XAUDIO2_VOICE_NOSRC flag causes the voice to behave as though the XAUDIO2_VOICE_NOPITCH flag also is specified. ?XAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.?XAUDIO2_VOICE_MUSICThe voice is used to play background music. The system automatically  can replace the voice with music selected by the user.? </param>
        /// <param name="maxFrequencyRatio">[in]  Highest allowable frequency ratio that can be set on this voice. The value for this argument must be between XAUDIO2_MIN_FREQ_RATIO and XAUDIO2_MAX_FREQ_RATIO. Subsequent calls to <see cref="SharpDX.XAudio2.SourceVoice.SetFrequencyRatio"/> are clamped between XAUDIO2_MIN_FREQ_RATIO and MaxFrequencyRatio. The maximum value for this argument is defined as XAUDIO2_MAX_FREQ_RATIO, which allows pitch to be raised by up to 10 octaves. If MaxFrequencyRatio is less than 1.0, the voice will use that ratio immediately after being created (rather than the default of 1.0). Xbox 360  For XMA voices there is an additional restriction on the MaxFrequencyRatio argument and the voice's sample rate. The product of these two numbers cannot exceed XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MONO for one-channel voices or XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MULTICHANNEL for voices with any other number of channels. If the value specified for MaxFrequencyRatio is too high for the specified format, the call to CreateSourceVoice fails and produces a debug message.  Note XAudio2's memory usage can be reduced by using the lowest possible MaxFrequencyRatio value. </param>
        /// <param name="callback">[in, optional]  Pointer to a client-provided callback interface, <see cref="SharpDX.XAudio2.VoiceCallback"/>. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags, float maxFrequencyRatio, VoiceCallback callback)
            : this(device, sourceFormat, flags, maxFrequencyRatio, callback, null)
        {
        }

        /// <summary>
        /// Creates and configures a source voice with callback through delegates.
        /// </summary>
        /// <param name="device">an instance of <see cref="SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat" /> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat" /> have a <see cref="SharpDX.Multimedia.WaveFormat" /> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat" /> structure and use it as the value for pSourceFormat.</param>
        /// <param name="flags">[in]  Flags that specify the behavior of the source voice. A flag can be 0 or a combination of one or more of the following: ValueDescriptionXAUDIO2_VOICE_NOPITCHNo pitch control is available on the voice.?XAUDIO2_VOICE_NOSRCNo sample rate conversion is available on the voice, the voice's  outputs must have the same sample rate.Note The XAUDIO2_VOICE_NOSRC flag causes the voice to behave as though the XAUDIO2_VOICE_NOPITCH flag also is specified. ?XAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.?XAUDIO2_VOICE_MUSICThe voice is used to play background music. The system automatically  can replace the voice with music selected by the user.?</param>
        /// <param name="maxFrequencyRatio">[in]  Highest allowable frequency ratio that can be set on this voice. The value for this argument must be between XAUDIO2_MIN_FREQ_RATIO and XAUDIO2_MAX_FREQ_RATIO. Subsequent calls to <see cref="SharpDX.XAudio2.SourceVoice.SetFrequencyRatio" /> are clamped between XAUDIO2_MIN_FREQ_RATIO and MaxFrequencyRatio. The maximum value for this argument is defined as XAUDIO2_MAX_FREQ_RATIO, which allows pitch to be raised by up to 10 octaves. If MaxFrequencyRatio is less than 1.0, the voice will use that ratio immediately after being created (rather than the default of 1.0). Xbox 360  For XMA voices there is an additional restriction on the MaxFrequencyRatio argument and the voice's sample rate. The product of these two numbers cannot exceed XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MONO for one-channel voices or XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MULTICHANNEL for voices with any other number of channels. If the value specified for MaxFrequencyRatio is too high for the specified format, the call to CreateSourceVoice fails and produces a debug message.  Note XAudio2's memory usage can be reduced by using the lowest possible MaxFrequencyRatio value.</param>
        /// <param name="enableCallbackEvents">if set to <c>true</c> [enable callback events].</param>
        /// <returns>No enableCallbackEvents.</returns>
        ///   <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags, float maxFrequencyRatio, bool enableCallbackEvents)
            : this(device, sourceFormat, flags, maxFrequencyRatio, enableCallbackEvents, null)
        {
        }

        /// <summary>	
        /// Creates and configures a source voice.	
        /// </summary>	
        /// <param name="device">an instance of <see cref = "SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat"/> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat"/> have a <see cref="SharpDX.Multimedia.WaveFormat"/> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat"/> structure and use it as the value for pSourceFormat. </param>
        /// <param name="flags">[in]  Flags that specify the behavior of the source voice. A flag can be 0 or a combination of one or more of the following: ValueDescriptionXAUDIO2_VOICE_NOPITCHNo pitch control is available on the voice.?XAUDIO2_VOICE_NOSRCNo sample rate conversion is available on the voice, the voice's  outputs must have the same sample rate.Note The XAUDIO2_VOICE_NOSRC flag causes the voice to behave as though the XAUDIO2_VOICE_NOPITCH flag also is specified. ?XAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.?XAUDIO2_VOICE_MUSICThe voice is used to play background music. The system automatically  can replace the voice with music selected by the user.? </param>
        /// <param name="maxFrequencyRatio">[in]  Highest allowable frequency ratio that can be set on this voice. The value for this argument must be between XAUDIO2_MIN_FREQ_RATIO and XAUDIO2_MAX_FREQ_RATIO. Subsequent calls to <see cref="SharpDX.XAudio2.SourceVoice.SetFrequencyRatio"/> are clamped between XAUDIO2_MIN_FREQ_RATIO and MaxFrequencyRatio. The maximum value for this argument is defined as XAUDIO2_MAX_FREQ_RATIO, which allows pitch to be raised by up to 10 octaves. If MaxFrequencyRatio is less than 1.0, the voice will use that ratio immediately after being created (rather than the default of 1.0). Xbox 360  For XMA voices there is an additional restriction on the MaxFrequencyRatio argument and the voice's sample rate. The product of these two numbers cannot exceed XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MONO for one-channel voices or XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MULTICHANNEL for voices with any other number of channels. If the value specified for MaxFrequencyRatio is too high for the specified format, the call to CreateSourceVoice fails and produces a debug message.  Note XAudio2's memory usage can be reduced by using the lowest possible MaxFrequencyRatio value. </param>
        /// <param name="callback">[in, optional]  Pointer to a client-provided callback interface, <see cref="SharpDX.XAudio2.VoiceCallback"/>. </param>
        /// <param name="effectDescriptors">[in, optional] Pointer to a list of XAUDIO2_EFFECT_CHAIN structures that describe an effect chain to use in the source voice.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags, float maxFrequencyRatio, VoiceCallback callback, EffectDescriptor[] effectDescriptors)
            : base(device)
        {
            CreateSourceVoice(device, sourceFormat, flags, maxFrequencyRatio, callback, effectDescriptors);
        }

        /// <summary>
        /// Creates and configures a source voice with callback through delegates.
        /// </summary>
        /// <param name="device">an instance of <see cref="SharpDX.XAudio2.XAudio2" /></param>
        /// <param name="sourceFormat">[in]  Pointer to a <see cref="SharpDX.Multimedia.WaveFormat" /> structure. This structure contains the expected format for all audio buffers submitted to the source voice. XAudio2 supports voice types of PCM, xWMA, ADPCM (Windows only), and XMA (Xbox 360 only). XAudio2 supports the following PCM formats.   8-bit (unsigned) integer PCM   16-bit integer PCM (Optimal format for XAudio2)   20-bit integer PCM (either in 24 or 32 bit containers)   24-bit integer PCM (either in 24 or 32 bit containers)   32-bit integer PCM   32-bit float PCM (Preferred format after 16-bit integer)   The number of channels in a source voice must be less than or equal to XAUDIO2_MAX_AUDIO_CHANNELS. The sample rate of a source voice must be between XAUDIO2_MIN_SAMPLE_RATE and XAUDIO2_MAX_SAMPLE_RATE. Note Data formats such as XMA, {{ADPCM}}, and {{xWMA}} that require more information than provided by <see cref="SharpDX.Multimedia.WaveFormat" /> have a <see cref="SharpDX.Multimedia.WaveFormat" /> structure as the first member in their format structure. When creating a source voice with one of those formats cast the format's structure as a <see cref="SharpDX.Multimedia.WaveFormat" /> structure and use it as the value for pSourceFormat.</param>
        /// <param name="flags">[in]  Flags that specify the behavior of the source voice. A flag can be 0 or a combination of one or more of the following: ValueDescriptionXAUDIO2_VOICE_NOPITCHNo pitch control is available on the voice.?XAUDIO2_VOICE_NOSRCNo sample rate conversion is available on the voice, the voice's  outputs must have the same sample rate.Note The XAUDIO2_VOICE_NOSRC flag causes the voice to behave as though the XAUDIO2_VOICE_NOPITCH flag also is specified. ?XAUDIO2_VOICE_USEFILTERThe filter effect should be available on this voice.?XAUDIO2_VOICE_MUSICThe voice is used to play background music. The system automatically  can replace the voice with music selected by the user.?</param>
        /// <param name="maxFrequencyRatio">[in]  Highest allowable frequency ratio that can be set on this voice. The value for this argument must be between XAUDIO2_MIN_FREQ_RATIO and XAUDIO2_MAX_FREQ_RATIO. Subsequent calls to <see cref="SharpDX.XAudio2.SourceVoice.SetFrequencyRatio" /> are clamped between XAUDIO2_MIN_FREQ_RATIO and MaxFrequencyRatio. The maximum value for this argument is defined as XAUDIO2_MAX_FREQ_RATIO, which allows pitch to be raised by up to 10 octaves. If MaxFrequencyRatio is less than 1.0, the voice will use that ratio immediately after being created (rather than the default of 1.0). Xbox 360  For XMA voices there is an additional restriction on the MaxFrequencyRatio argument and the voice's sample rate. The product of these two numbers cannot exceed XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MONO for one-channel voices or XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MULTICHANNEL for voices with any other number of channels. If the value specified for MaxFrequencyRatio is too high for the specified format, the call to CreateSourceVoice fails and produces a debug message.  Note XAudio2's memory usage can be reduced by using the lowest possible MaxFrequencyRatio value.</param>
        /// <param name="enableCallbackEvents">if set to <c>true</c> [enable callback events].</param>
        /// <param name="effectDescriptors">[in, optional] Pointer to a list of XAUDIO2_EFFECT_CHAIN structures that describe an effect chain to use in the source voice.</param>
        /// <returns>No enableCallbackEvents.</returns>
        ///   <unmanaged>HRESULT IXAudio2::CreateSourceVoice([Out] IXAudio2SourceVoice** ppSourceVoice,[In] const WAVEFORMATEX* pSourceFormat,[None] UINT32 Flags,[None] float MaxFrequencyRatio,[In, Optional] IXAudio2VoiceCallback* pCallback,[In, Optional] const XAUDIO2_VOICE_SENDS* pSendList,[In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public SourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags, float maxFrequencyRatio, bool enableCallbackEvents, EffectDescriptor[] effectDescriptors)
            : base(device)
        {
            CreateSourceVoice(device, sourceFormat, flags, maxFrequencyRatio, enableCallbackEvents ? (voiceCallbackImpl = new VoiceCallbackImpl(this)) : null, effectDescriptors);
        }

        private void CreateSourceVoice(XAudio2 device, SharpDX.Multimedia.WaveFormat sourceFormat, SharpDX.XAudio2.VoiceFlags flags, float maxFrequencyRatio, VoiceCallback callback, EffectDescriptor[] effectDescriptors)
        {
            var waveformatPtr = WaveFormat.MarshalToPtr(sourceFormat);
            try
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
                            device.CreateSourceVoice(this, waveformatPtr, flags, maxFrequencyRatio, callback, null, tempSendDescriptor);
                        }
                    }
                }
                else
                {
                    device.CreateSourceVoice(this, waveformatPtr, flags, maxFrequencyRatio, callback, null, null);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(waveformatPtr);
            }
        }

        /// <summary>	
        /// Starts consumption and processing of audio by the voice. Delivers the result to any connected submix or mastering voices, or to the output device, with CommitNow changes.
        /// </summary>	
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2SourceVoice::Start([None] UINT32 Flags,[None] UINT32 OperationSet)</unmanaged>
        public void Start()
        {
            this.Start(0, 0);
        }

#if DIRECTX11_1
        /// <summary>
        /// Gets the state.
        /// </summary>
        public VoiceState State
        {
            get
            {
                return GetState(0);
            }
        }
#endif

        /// <summary>
        /// Sets the frequency ratio.
        /// </summary>
        /// <param name="ratio">The ratio.</param>
        /// <returns></returns>
        public void SetFrequencyRatio(float ratio)
        {
            SetFrequencyRatio(ratio, 0);
        }

        /// <summary>	
        /// Starts consumption and processing of audio by the voice. Delivers the result to any connected submix or mastering voices, or to the output device.	
        /// </summary>	
        /// <param name="operationSet">[in]  Identifies this call as part of a deferred batch. See the {{XAudio2 Operation Sets}} overview for more information. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2SourceVoice::Start([None] UINT32 Flags,[None] UINT32 OperationSet)</unmanaged>
        public void Start(int operationSet)
        {
            this.Start(0, operationSet);
        }

        public void Stop()
        {
            this.Stop(PlayFlags.None, 0);
        }

        public void Stop(int operationSet)
        {
            this.Stop(PlayFlags.None, operationSet);
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="bufferRef">No documentation.</param>	
        /// <param name="decodedXMWAPacketInfo">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IXAudio2SourceVoice::SubmitSourceBuffer']/*"/>	
        /// <unmanaged>HRESULT IXAudio2SourceVoice::SubmitSourceBuffer([In] const XAUDIO2_BUFFER* pBuffer,[In, Optional] const XAUDIO2_BUFFER_WMA* pBufferWMA)</unmanaged>	
        public void SubmitSourceBuffer(SharpDX.XAudio2.AudioBuffer bufferRef, uint[] decodedXMWAPacketInfo)
        {
            unsafe
            {
                if (decodedXMWAPacketInfo != null)
                {
                    fixed (void* pBuffer = decodedXMWAPacketInfo)
                    {
                        SharpDX.XAudio2.BufferWma bufferWmaRef;
                        bufferWmaRef.PacketCount = decodedXMWAPacketInfo.Length;
                        bufferWmaRef.DecodedPacketCumulativeBytesPointer = (IntPtr)pBuffer;
                        SubmitSourceBuffer(bufferRef, bufferWmaRef);
                    }
                }
                else
                {
                    SubmitSourceBuffer(bufferRef, (SharpDX.XAudio2.BufferWma?)null);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Utilities.Dispose(ref voiceCallbackImpl);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Occurs just before the processing pass for the voice begins.
        /// </summary>
        /// <remarks>
        /// In order to use this delegate, this instance must have been initialized with events delegate support.
        /// </remarks>
        public event Action<int> ProcessingPassStart;

        public delegate void VoidAction();

        /// <summary>
        /// Occurs just after the processing pass for the voice ends.
        /// </summary>
        /// <remarks>
        /// In order to use this delegate, this instance must have been initialized with events delegate support.
        /// </remarks>
        public event VoidAction ProcessingPassEnd;

        /// <summary>
        /// Occurs when the voice has just finished playing a contiguous audio stream.
        /// </summary>
        /// <remarks>
        /// In order to use this delegate, this instance must have been initialized with events delegate support.
        /// </remarks>
        public event VoidAction StreamEnd;

        /// <summary>
        /// Occurs when the voice is about to start processing a new audio buffer.
        /// </summary>
        /// <remarks>
        /// In order to use this delegate, this instance must have been initialized with events delegate support.
        /// </remarks>
        public event Action<IntPtr> BufferStart;

        /// <summary>
        /// Occurs when the voice finishes processing a buffer.
        /// </summary>
        /// <remarks>
        /// In order to use this delegate, this instance must have been initialized with events delegate support.
        /// </remarks>
        public event Action<IntPtr> BufferEnd;

        /// <summary>
        /// Occurs when a critical error occurs during voice processing.
        /// </summary>
        /// <remarks>
        /// In order to use this delegate, this instance must have been initialized with events delegate support.
        /// </remarks>
        public event Action<IntPtr> LoopEnd;


        public struct VoiceErrorArgs
        {
            public VoiceErrorArgs(IntPtr pointer, Result result)
            {
                this.Pointer = pointer;
                this.Result = result;
            }

            public IntPtr Pointer;
            public Result Result;
        }

        /// <summary>
        /// Occurs when [voice error].
        /// </summary>
        /// <remarks>
        /// In order to use this delegate, this instance must have been initialized with events delegate support.
        /// </remarks>
        public event Action<VoiceErrorArgs> VoiceError;

        private class VoiceCallbackImpl : CallbackBase, VoiceCallback
        {
            private SourceVoice Voice { get; set; }

            public VoiceCallbackImpl(SourceVoice voice)
            {
                Voice = voice;
            }

            void VoiceCallback.OnVoiceProcessingPassStart(int bytesRequired)
            {
                if (Voice.ProcessingPassStart != null) Voice.ProcessingPassStart(bytesRequired);
            }

            void VoiceCallback.OnVoiceProcessingPassEnd()
            {
                if (Voice.ProcessingPassEnd != null) Voice.ProcessingPassEnd();
            }

            void VoiceCallback.OnStreamEnd()
            {
                if (Voice.StreamEnd != null) Voice.StreamEnd();
            }

            void VoiceCallback.OnBufferStart(IntPtr context)
            {
                if (Voice.BufferStart != null) Voice.BufferStart(context);
            }

            void VoiceCallback.OnBufferEnd(IntPtr context)
            {
                if (Voice.BufferEnd != null) Voice.BufferEnd(context);
            }

            void VoiceCallback.OnLoopEnd(IntPtr context)
            {
                if (Voice.LoopEnd != null) Voice.LoopEnd(context);
            }

            void VoiceCallback.OnVoiceError(IntPtr context, Result error)
            {
                if (Voice.VoiceError != null) Voice.VoiceError(new VoiceErrorArgs(context, error));
            }
        }
    }
}