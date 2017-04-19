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
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='XAUDIO2_SUBMIX_VOICE_FLAGS']/*"/>
    [Flags]
    public enum SubmixVoiceFlags : int
    {

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='XAUDIO2_VOICE_USEFILTER']/*"/>	
        /// <unmanaged>XAUDIO2_VOICE_USEFILTER</unmanaged>	
        /// <unmanaged-short>XAUDIO2_VOICE_USEFILTER</unmanaged-short>	
        UseFilter = VoiceFlags.UseFilter,

        /// <summary>	
        /// None.	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='']/*"/>	
        /// <unmanaged>None</unmanaged>	
        /// <unmanaged-short>None</unmanaged-short>	
        None = VoiceFlags.None,
    }
}
