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

namespace SharpDX.DirectSound
{
    public partial class SecondarySoundBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecondarySoundBuffer"/> class.
        /// </summary>
        /// <param name="dSound">an instance of the <see cref="DirectSound"/></param>
        /// <param name="bufferDescription">The buffer description.</param>
        public SecondarySoundBuffer(DirectSound dSound, SoundBufferDescription bufferDescription)
            : base(IntPtr.Zero)
        {
            IntPtr temp;
            dSound.CreateSoundBuffer(bufferDescription, out temp, null);
            NativePointer = temp;
            QueryInterfaceFrom(this);
            Marshal.Release(temp);
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
        /// Enables effects on a buffer.	
        /// </summary>	
        /// <param name="effects">Effects guids</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IDirectSoundBuffer8::SetFX([None] int dwEffectsCount,[In, Buffer, Optional] LPDSEFFECTDESC pDSFXDesc,[Out, Buffer, Optional] int* pdwResultCodes)</unmanaged>
        public SoundEffectResult[] SetEffect(Guid[] effects)
        {
            if (effects == null || effects.Length == 0)
            {
                SetEffect(0, null, new SoundEffectResult[1]);
                return new SoundEffectResult[0];
            }

            var effectDescriptions = new SoundBufferEffectDescription[effects.Length];
            for(int i = 0; i < effectDescriptions.Length; i++)
            {
                effectDescriptions[i] = new SoundBufferEffectDescription();
                effectDescriptions[i].Flags = 0;
                effectDescriptions[i].idDSFXClass = effects[i];
            }

            SoundEffectResult[] resultCode = new SoundEffectResult[effects.Length];

            SetEffect(effects.Length, effectDescriptions, resultCode);

            return resultCode;
        }
    }
}