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

namespace SharpDX.Multimedia
{

    /// <summary>
    /// This enumeration defines constants that indicate the role that the system has assigned to an audio endpoint device.
    /// </summary>
    /// <msdn-id>dd370842</msdn-id>	
    /// <unmanaged>ERole</unmanaged>	
    /// <unmanaged-short>ERole</unmanaged-short>	
    public enum AudioEndpointRole
    {
        /// <summary>
        /// Games, system notification sounds, and voice commands
        /// </summary>
        Console,

        /// <summary>
        /// Music, movies, narration, and live music recording.
        /// </summary>
        Multimedia,

        /// <summary>
        /// Voice communications (talking to another person).
        /// </summary>
        Communications,
    }
}