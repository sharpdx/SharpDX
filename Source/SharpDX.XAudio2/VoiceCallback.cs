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
    [Shadow(typeof(VoiceShadow))]
    public partial interface VoiceCallback
    {
        /// <summary>	
        /// Called during each processing pass for each voice, just before XAudio2 reads data from the voice's buffer queue.	
        /// </summary>	
        /// <param name="bytesRequired"> The number of bytes that must be submitted immediately to avoid starvation. This allows the implementation of just-in-time streaming scenarios; the client can keep the absolute minimum data queued on the voice at all times, and pass it fresh data just before the data is required. This model provides the lowest possible latency attainable with XAudio2. For xWMA and XMA data BytesRequired will always be zero, since the concept of a frame of xWMA or XMA data is meaningless. Note In a situation where there is always plenty of data available on the source voice, BytesRequired should always report zero, because it doesn't need any samples immediately to avoid glitching. </param>
        /// <unmanaged>void IXAudio2VoiceCallback::OnVoiceProcessingPassStart([None] UINT32 BytesRequired)</unmanaged>
        void OnVoiceProcessingPassStart(int bytesRequired);


        /// <summary>	
        /// Called just after the processing pass for the voice ends.	
        /// </summary>	
        /// <unmanaged>void IXAudio2VoiceCallback::OnVoiceProcessingPassEnd()</unmanaged>
        void OnVoiceProcessingPassEnd();


        /// <summary>	
        /// Called when the voice has just finished playing a contiguous audio stream.	
        /// </summary>	
        /// <unmanaged>void IXAudio2VoiceCallback::OnStreamEnd()</unmanaged>
        void OnStreamEnd();


        /// <summary>	
        /// Called when the voice is about to start processing a new audio buffer.	
        /// </summary>	
        /// <param name="context"> Context pointer that was assigned to the pContext member of the <see cref="AudioBuffer"/> structure when the buffer was submitted. </param>
        /// <unmanaged>void IXAudio2VoiceCallback::OnBufferStart([None] void* pBufferContext)</unmanaged>
        void OnBufferStart(IntPtr context);


        /// <summary>	
        /// Called when the voice finishes processing a buffer.	
        /// </summary>	
        /// <param name="context"> Context pointer assigned to the pContext member of the <see cref="AudioBuffer"/> structure when the buffer was submitted. </param>
        /// <unmanaged>void IXAudio2VoiceCallback::OnBufferEnd([None] void* pBufferContext)</unmanaged>
        void OnBufferEnd(IntPtr context);


        /// <summary>	
        /// Called when the voice reaches the end position of a loop.	
        /// </summary>	
        /// <param name="context"> Context pointer that was assigned to the pContext member of the <see cref="AudioBuffer"/> structure when the buffer was submitted. </param>
        /// <unmanaged>void IXAudio2VoiceCallback::OnLoopEnd([None] void* pBufferContext)</unmanaged>
        void OnLoopEnd(IntPtr context);


        /// <summary>	
        /// Called when a critical error occurs during voice processing.	
        /// </summary>	
        /// <param name="context"> Context pointer that was assigned to the pContext member of the <see cref="AudioBuffer"/> structure when the buffer was submitted. </param>
        /// <param name="error"> The HRESULT code of the error encountered. </param>
        /// <unmanaged>void IXAudio2VoiceCallback::OnVoiceError([None] void* pBufferContext,[None] HRESULT Error)</unmanaged>
        void OnVoiceError(IntPtr context, Result error);
    }
}