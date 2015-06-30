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

namespace SharpDX.X3DAudio
{
    /// <summary>
    /// An enum to select the XAudio version to load.
    /// </summary>
    public enum X3DAudioVersion
    {
        /// <summary>
        /// The default version (X3DAudio1_7.dll if it is installed, otherwise XAudio2_8.dll or from XAudio2_9.dll)
        /// </summary>
        Default,

        /// <summary>
        /// The X3DAudio1.7 version (X3DAudio1_7.dll).
        /// </summary>
        Version17,

        /// <summary>
        /// From the XAudio2.8 version (XAudio2_8.dll).
        /// </summary>
        Version28,

        /// <summary>
        /// From the XAudio2.9 version (XAudio2_9.dll).
        /// </summary>
        Version29,
    }
}