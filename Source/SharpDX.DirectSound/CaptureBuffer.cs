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
    public partial class CaptureBuffer
    {
        /// <summary>	
        /// Creates a buffer for capturing waveform audio.	
        /// </summary>
        /// <param name="capture">a reference to an instance of <see cref="DirectSoundCapture"/></param>
        /// <param name="description">a <see cref="SharpDX.DirectSound.CaptureBufferDescription"/> structure containing values for the capture buffer being created. </param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IDirectSoundCapture::CreateCaptureBuffer([In] LPCDSCBUFFERDESC pcDSCBufferDesc,[Out] LPDIRECTSOUNDCAPTUREBUFFER* ppDSCBuffer,[None] IUnknown* pUnkOuter)</unmanaged>
        public CaptureBuffer(DirectSoundCapture capture, CaptureBufferDescription description) : base(IntPtr.Zero)
        {

            CaptureBufferBase captureBuffer;
            capture.CreateCaptureBuffer(description, out captureBuffer, null);
            using (captureBuffer)
            {
                NativePointer = captureBuffer.NativePointer;
                QueryInterfaceFrom(this);
            }
        }

        /// <summary>	
        /// Retrieves an interface to an effect object associated with the buffer.	
        /// </summary>	
        /// <param name="index">Index of the object within objects of that class in the path. See Remarks.  </param>
        /// <returns>an effect object associated with the buffer</returns>
        /// <unmanaged>HRESULT IDirectSoundCaptureBuffer8::GetObjectInPath([In] GUID* rguidObject,[None] int dwIndex,[In] GUID* rguidInterface,[Out] void** ppObject)</unmanaged>
        public T GetEffect<T>(int index) where T : ComObject
        {
            IntPtr effectPtr;
            GetEffect(DSound.AllObjects, index, Utilities.GetGuidFromType(typeof(T)), out effectPtr);
            return FromPointer<T>(effectPtr);
        }

        /// <summary>
        /// Retrieves the status of capture effects.
        /// </summary>
        /// <param name="effectCount" />
        /// <returns />
        public CaptureEffectResult[] GetEffectStatus(int effectCount)
        {
            var result = new CaptureEffectResult[effectCount];
            GetEffectStatus(effectCount, result);
            return result;
        }

        /// <summary>
        /// Sets the notification positions.
        /// </summary>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public void SetNotificationPositions(NotificationPosition[] positions)
        {
            using (var notifier = QueryInterface<SoundBufferNotifier>())
            {
                notifier.SetNotificationPositions(positions.Length, positions);
            }
        }
    }
}