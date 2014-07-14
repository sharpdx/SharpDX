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

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Resource characteristics returned by <see cref="MediaEngineEx.ResourceCharacteristics"/>.
    /// </summary>
    /// <msdn-id>hh447939</msdn-id>	
    /// <unmanaged>HRESULT IMFMediaEngineEx::GetResourceCharacteristics([Out] unsigned int* pCharacteristics)</unmanaged>	
    /// <unmanaged-short>IMFMediaEngineEx::GetResourceCharacteristics</unmanaged-short>	
    [Flags]
    public enum ResourceCharacteristics
    {
        /// <summary>
        /// None flag.
        /// </summary>
        None = 0,

        /// <summary>
        /// The media resource represents a live data source, such as a video camera. If playback is stopped and then restarted, there will be a gap in the content.
        /// </summary>
        LiveSource = 0x00000001,

        /// <summary>
        /// The media resource supports seeking. To get the seekable range, call IMFMediaEngine::GetSeekable.
        /// </summary>
        CanSeek = 0x00000002,

        /// <summary>
        /// The media resource can be paused.
        /// </summary>
        CanPause = 0x00000003,

        /// <summary>
        /// Seeking this resource can take a long time. For example, it might download through HTTP.
        /// </summary>
        LongTimeToSeek = 0x00000004
    }
}