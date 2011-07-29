// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.IO;

namespace SharpDX.XACT3
{
    public partial class SoundBank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundBank"/> class from a soundbank stream.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="stream">The soundbank stream stream.</param>
        /// <unmanaged>HRESULT IXACT3Engine::CreateSoundBank([In] const void* pvBuffer,[In] unsigned int dwSize,[In] unsigned int dwFlags,[In] unsigned int dwAllocAttributes,[Out, Fast] IXACT3SoundBank** ppSoundBank)</unmanaged>
        public SoundBank(Engine engine, Stream stream)
        {
            if (stream is DataStream)
            {
                engine.CreateSoundBank(((DataStream)stream).DataPointer, (int)stream.Length, 0, 0, this);
            }
            else
            {
                var data = Utilities.ReadStream(stream);
                unsafe
                {
                    fixed (void* pData = data)
                        engine.CreateSoundBank((IntPtr) pData, data.Length, 0, 0, this);
                }
            }
        }

        public SharpDX.XACT3.Cue Play(short cueIndex, int timeOffset)
        {
            return Play(cueIndex, 0, timeOffset);
        }

        public SharpDX.XACT3.Cue Prepare(short cueIndex, int timeOffset)
        {
            return Prepare(cueIndex, 0, timeOffset);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is referenced by at least one valid cue instance or other client. For example, the game itself might reference the sound bank. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in use; otherwise, <c>false</c>.
        /// </value>
        public bool IsInUse
        {
            get
            {
                int state;
                GetState(out state);
                return state != 0;
            }
        }        
    }
}