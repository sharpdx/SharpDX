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
    public partial class Voice
    {
        protected readonly XAudio2 device;

        protected Voice(XAudio2 device) : base(IntPtr.Zero)
        {
            this.device = device;
        }

        /// <summary>	
        /// <p>Gets or Sets the overall volume level for the voice.</p>	
        /// </summary>	
        public float Volume
        {
            get
            {
                GetVolume(out var value);
                return value;
            }
            set
            {
                SetVolume(value);
            }
        }

        /// <summary>	
        /// <p>Returns information about the creation flags, input channels, and sample rate of a voice.</p>	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IXAudio2Voice::GetVoiceDetails']/*"/>	
        /// <msdn-id>microsoft.directx_sdk.ixaudio2voice.ixaudio2voice.getvoicedetails</msdn-id>	
        /// <unmanaged>GetVoiceDetails</unmanaged>	
        /// <unmanaged-short>GetVoiceDetails</unmanaged-short>	
        /// <unmanaged>void IXAudio2Voice::GetVoiceDetails([Out] XAUDIO2_VOICE_DETAILS* pVoiceDetails)</unmanaged>
        public SharpDX.XAudio2.VoiceDetails VoiceDetails
        {
            get
            {
                SharpDX.XAudio2.VoiceDetails __output__; 
                GetVoiceDetails(out __output__);

                // Handle 2.7 version changes here
                if (device.Version == XAudio2Version.Version27)
                {
                    __output__.InputSampleRate = __output__.InputChannelCount;
                    __output__.InputChannelCount = __output__.ActiveFlags;
                    __output__.ActiveFlags = 0;
                }

                return __output__;
            }
        }

        /// <summary>	
        /// Enables the effect at a given position in the effect chain of the voice.	
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect in the effect chain of the voice. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::EnableEffect([None] UINT32 EffectIndex,[None] UINT32 OperationSet)</unmanaged>
        public void EnableEffect(int effectIndex)
        {
            EnableEffect(effectIndex, 0);
        }

        /// <summary>	
        /// Disables the effect at a given position in the effect chain of the voice.	
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect in the effect chain of the voice. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::DisableEffect([None] UINT32 EffectIndex,[None] UINT32 OperationSet)</unmanaged>
        public void DisableEffect(int effectIndex)
        {
            DisableEffect(effectIndex, 0);
        }

        /// <summary>	
        /// Sets parameters for a given effect in the voice's effect chain.
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect within the voice's effect chain. </param>
        /// <returns>Returns the current values of the effect-specific parameters.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetEffectParameters([None] UINT32 EffectIndex,[In, Buffer] const void* pParameters,[None] UINT32 ParametersByteSize,[None] UINT32 OperationSet)</unmanaged>
        public T GetEffectParameters<T>(int effectIndex) where T : struct
        {
            unsafe
            {
                var effectParameter = default(T);
                byte* pEffectParameter = stackalloc byte[Utilities.SizeOf<T>()];
                GetEffectParameters(effectIndex, (IntPtr)pEffectParameter, Utilities.SizeOf<T>());
                Utilities.Read((IntPtr)pEffectParameter, ref effectParameter);
                return effectParameter;
            }
        }

        /// <summary>	
        /// Returns the current effect-specific parameters of a given effect in the voice's effect chain.	
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect within the voice's effect chain. </param>
        /// <param name="effectParameters">[out]  Returns the current values of the effect-specific parameters. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::GetEffectParameters([None] UINT32 EffectIndex,[Out, Buffer] void* pParameters,[None] UINT32 ParametersByteSize)</unmanaged>
        public void GetEffectParameters(int effectIndex, byte[] effectParameters)
        {
            unsafe
            {
                fixed (void* pEffectParameter = &effectParameters[0])
                    GetEffectParameters(effectIndex, (IntPtr)pEffectParameter, effectParameters.Length);
            }
        }

        /// <summary>	
        /// Sets parameters for a given effect in the voice's effect chain.	
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect within the voice's effect chain. </param>
        /// <param name="effectParameter">[in]  Returns the current values of the effect-specific parameters. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetEffectParameters([None] UINT32 EffectIndex,[In, Buffer] const void* pParameters,[None] UINT32 ParametersByteSize,[None] UINT32 OperationSet)</unmanaged>
        public void SetEffectParameters(int effectIndex, byte[] effectParameter)
        {
            SetEffectParameters(effectIndex, effectParameter, 0);
        }

        /// <summary>	
        /// Sets parameters for a given effect in the voice's effect chain.	
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect within the voice's effect chain. </param>
        /// <param name="effectParameter">[in]  Returns the current values of the effect-specific parameters. </param>
        /// <param name="operationSet">[in]  Identifies this call as part of a deferred batch. See the {{XAudio2 Operation Sets}} overview  for more information. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetEffectParameters([None] UINT32 EffectIndex,[In, Buffer] const void* pParameters,[None] UINT32 ParametersByteSize,[None] UINT32 OperationSet)</unmanaged>
        public void SetEffectParameters(int effectIndex, byte[] effectParameter, int operationSet)
        {
            unsafe
            {
                fixed (void* pEffectParameter = &effectParameter[0])
                    SetEffectParameters(effectIndex, (IntPtr)pEffectParameter, effectParameter.Length, operationSet);
            }
        }

        /// <summary>	
        /// Sets parameters for a given effect in the voice's effect chain.
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect within the voice's effect chain. </param>
        /// <param name="effectParameter">[in]  Returns the current values of the effect-specific parameters. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetEffectParameters([None] UINT32 EffectIndex,[In, Buffer] const void* pParameters,[None] UINT32 ParametersByteSize,[None] UINT32 OperationSet)</unmanaged>
        public void SetEffectParameters<T>(int effectIndex, T effectParameter) where T : struct
        {
            this.SetEffectParameters<T>(effectIndex, effectParameter, 0);
        }

        /// <summary>	
        /// Sets parameters for a given effect in the voice's effect chain.	
        /// </summary>	
        /// <param name="effectIndex">[in]  Zero-based index of an effect within the voice's effect chain. </param>
        /// <param name="effectParameter">[in]  Returns the current values of the effect-specific parameters. </param>
        /// <param name="operationSet">[in]  Identifies this call as part of a deferred batch. See the {{XAudio2 Operation Sets}} overview  for more information. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetEffectParameters([None] UINT32 EffectIndex,[In, Buffer] const void* pParameters,[None] UINT32 ParametersByteSize,[None] UINT32 OperationSet)</unmanaged>
        public void SetEffectParameters<T>(int effectIndex, T effectParameter, int operationSet) where T : struct
        {
            unsafe
            {
                byte* pEffectParameter = stackalloc byte[Utilities.SizeOf<T>()];
                Utilities.Write((IntPtr)pEffectParameter, ref effectParameter);
                SetEffectParameters(effectIndex, (IntPtr) pEffectParameter, Utilities.SizeOf<T>(), operationSet);
            }
        }

        /// <summary>	
        /// Replaces the effect chain of the voice.	
        /// </summary>	
        /// <param name="effectDescriptors">[in, optional]  an array of <see cref="SharpDX.XAudio2.EffectDescriptor"/> structure that describes the new effect chain to use. If NULL is passed, the current effect chain is removed. If array is non null, its length must be at least of 1. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetEffectChain([In, Optional] const XAUDIO2_EFFECT_CHAIN* pEffectChain)</unmanaged>
        public void SetEffectChain(params EffectDescriptor[] effectDescriptors)
        {
            unsafe
            {
                if (effectDescriptors != null)
                {
                    var tempSendDescriptor = new EffectChain();
                    var effectDescriptorNatives = new EffectDescriptor.__Native[effectDescriptors.Length];
                    for (int i = 0; i < effectDescriptorNatives.Length; i++)
                        effectDescriptors[i].__MarshalTo(ref effectDescriptorNatives[i]);
                    tempSendDescriptor.EffectCount = effectDescriptorNatives.Length;
                    fixed (void* pEffectDescriptors = &effectDescriptorNatives[0])
                    {
                        tempSendDescriptor.EffectDescriptorPointer = (IntPtr)pEffectDescriptors;
                        SetEffectChain(tempSendDescriptor);
                    }
                }
                else
                {
                    SetEffectChain((EffectChain?) null);
                }
            }

        }

        /// <summary>	
        /// Designates a new set of submix or mastering voices to receive the output of the voice.	
        /// </summary>	
        /// <param name="outputVoices">[in]  Array of <see cref="VoiceSendDescriptor"/> structure pointers to destination voices. If outputVoices is NULL, the voice will send its output to the current mastering voice. To set the voice to not send its output anywhere set an array of length 0. All of the voices in a send list must have the same input sample rate, see {{XAudio2 Sample Rate Conversions}} for additional information. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetOutputVoices([In, Optional] const XAUDIO2_VOICE_SENDS* pSendList)</unmanaged>
        public void SetOutputVoices(params VoiceSendDescriptor[] outputVoices)
        {
            unsafe
            {
                if (outputVoices != null)
                {
                    var tempSendDescriptor = new VoiceSendDescriptors {SendCount = outputVoices.Length};

                    if(outputVoices.Length > 0)
                    {
                        var nativeDescriptors = new VoiceSendDescriptor.__Native[outputVoices.Length];

                        for (int i = 0; i < outputVoices.Length; i++)
                        {
                            outputVoices[i].__MarshalTo(ref nativeDescriptors[i]);
                        }

                        fixed(void* pVoiceSendDescriptors = &nativeDescriptors[0])
                        {
                            tempSendDescriptor.SendPointer = (IntPtr)pVoiceSendDescriptors;
                            SetOutputVoices(tempSendDescriptor);
                        }
                    }
                    else
                    {
                        tempSendDescriptor.SendPointer = IntPtr.Zero;
                    }
                }
                else
                {
                    SetOutputVoices((VoiceSendDescriptors?) null);
                }
            }
        }

        /// <summary>	
        /// Sets the volume level of each channel of the final output for the voice. These channels are mapped to the input channels of a specified destination voice.	
        /// </summary>	
        /// <param name="sourceChannels">[in]  Confirms the output channel count of the voice. This is the number of channels that are produced by the last effect in the chain. </param>
        /// <param name="destinationChannels">[in]  Confirms the input channel count of the destination voice. </param>
        /// <param name="levelMatrixRef">[in]  Array of [SourceChannels ? DestinationChannels] volume levels sent to the destination voice. The level sent from source channel S to destination channel D is specified in the form pLevelMatrix[SourceChannels ? D + S]. For example, when rendering two-channel stereo input into 5.1 output that is weighted toward the front channels?but is absent from the center and low-frequency channels?the matrix might have the values shown in the following table.  OutputLeft InputRight Input Left1.00.0 Right0.01.0 Front Center0.00.0 LFE0.00.0 Rear Left0.80.0 Rear Right0.00.8  Note that the left and right input are fully mapped to the output left and right channels; 80 percent of the left and right input is mapped to the rear left and right channels. See Remarks for more information on volume levels. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetOutputMatrix([In, Optional] IXAudio2Voice* pDestinationVoice,[None] UINT32 SourceChannels,[None] UINT32 DestinationChannels,[In, Buffer] const float* pLevelMatrix,[None] UINT32 OperationSet)</unmanaged>
        public void SetOutputMatrix(int sourceChannels, int destinationChannels, float[] levelMatrixRef)
        {
            this.SetOutputMatrix(sourceChannels, destinationChannels, levelMatrixRef, 0);
        }


        /// <summary>	
        /// Sets the volume level of each channel of the final output for the voice. These channels are mapped to the input channels of a specified destination voice.	
        /// </summary>	
        /// <param name="destinationVoiceRef">[in]  Pointer to a destination <see cref="SharpDX.XAudio2.Voice"/> for which to set volume levels. Note If the voice sends to a single target voice then specifying NULL will cause SetOutputMatrix to operate on that target voice. </param>
        /// <param name="sourceChannels">[in]  Confirms the output channel count of the voice. This is the number of channels that are produced by the last effect in the chain. </param>
        /// <param name="destinationChannels">[in]  Confirms the input channel count of the destination voice. </param>
        /// <param name="levelMatrixRef">[in]  Array of [SourceChannels ? DestinationChannels] volume levels sent to the destination voice. The level sent from source channel S to destination channel D is specified in the form pLevelMatrix[SourceChannels ? D + S]. For example, when rendering two-channel stereo input into 5.1 output that is weighted toward the front channels?but is absent from the center and low-frequency channels?the matrix might have the values shown in the following table.  OutputLeft InputRight Input Left1.00.0 Right0.01.0 Front Center0.00.0 LFE0.00.0 Rear Left0.80.0 Rear Right0.00.8  Note that the left and right input are fully mapped to the output left and right channels; 80 percent of the left and right input is mapped to the rear left and right channels. See Remarks for more information on volume levels. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetOutputMatrix([In, Optional] IXAudio2Voice* pDestinationVoice,[None] UINT32 SourceChannels,[None] UINT32 DestinationChannels,[In, Buffer] const float* pLevelMatrix,[None] UINT32 OperationSet)</unmanaged>
        public void SetOutputMatrix(SharpDX.XAudio2.Voice destinationVoiceRef, int sourceChannels, int destinationChannels, float[] levelMatrixRef)
        {
            this.SetOutputMatrix(destinationVoiceRef, sourceChannels, destinationChannels, levelMatrixRef, 0);
        }

        /// <summary>	
        /// Sets the volume level of each channel of the final output for the voice. These channels are mapped to the input channels of a specified destination voice.	
        /// </summary>	
        /// <param name="sourceChannels">[in]  Confirms the output channel count of the voice. This is the number of channels that are produced by the last effect in the chain. </param>
        /// <param name="destinationChannels">[in]  Confirms the input channel count of the destination voice. </param>
        /// <param name="levelMatrixRef">[in]  Array of [SourceChannels ? DestinationChannels] volume levels sent to the destination voice. The level sent from source channel S to destination channel D is specified in the form pLevelMatrix[SourceChannels ? D + S]. For example, when rendering two-channel stereo input into 5.1 output that is weighted toward the front channels?but is absent from the center and low-frequency channels?the matrix might have the values shown in the following table.  OutputLeft InputRight Input Left1.00.0 Right0.01.0 Front Center0.00.0 LFE0.00.0 Rear Left0.80.0 Rear Right0.00.8  Note that the left and right input are fully mapped to the output left and right channels; 80 percent of the left and right input is mapped to the rear left and right channels. See Remarks for more information on volume levels. </param>
        /// <param name="operationSet">[in]  Identifies this call as part of a deferred batch. See the {{XAudio2 Operation Sets}} overview for more information. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IXAudio2Voice::SetOutputMatrix([In, Optional] IXAudio2Voice* pDestinationVoice,[None] UINT32 SourceChannels,[None] UINT32 DestinationChannels,[In, Buffer] const float* pLevelMatrix,[None] UINT32 OperationSet)</unmanaged>
        public void SetOutputMatrix(int sourceChannels, int destinationChannels, float[] levelMatrixRef, int operationSet)
        {
            this.SetOutputMatrix(null, sourceChannels, destinationChannels, levelMatrixRef, operationSet);
        }
    }
}