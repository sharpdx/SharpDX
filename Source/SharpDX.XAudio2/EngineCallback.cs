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
namespace SharpDX.XAudio2
{
    /// <summary>	
    /// The IXAudio2EngineCallback interface contains methods that notify the client when certain events happen in the <see cref="SharpDX.XAudio2.XAudio2"/> engine. This interface should be implemented by the XAudio2 client. XAudio2 calls these methods via an interface pointer provided by the client, using either the {{XAudio2Create}} or <see cref="SharpDX.XAudio2.XAudio2.Initialize"/> method. Methods in this interface return void, rather than an HRESULT.	
    /// </summary>	
    /// <unmanaged>IXAudio2EngineCallback</unmanaged>
    [Shadow(typeof(EngineShadow))]
    internal partial interface EngineCallback
    {
        /// <summary>	
        /// Called by XAudio2 just before an audio processing pass begins.	
        /// </summary>	
        /// <unmanaged>void IXAudio2EngineCallback::OnProcessingPassStart()</unmanaged>
        void OnProcessingPassStart();

        /// <summary>	
        /// Called by XAudio2 just after an audio processing pass ends.	
        /// </summary>	
        /// <unmanaged>void IXAudio2EngineCallback::OnProcessingPassEnd()</unmanaged>
        void OnProcessingPassEnd();

        /// <summary>	
        /// Called if a critical system error occurs that requires XAudio2 to be closed down and restarted.	
        /// </summary>	
        /// <param name="error"> Error code returned by XAudio2. </param>
        /// <unmanaged>void IXAudio2EngineCallback::OnCriticalError([None] HRESULT Error)</unmanaged>
        void OnCriticalError(Result error);
    }
}