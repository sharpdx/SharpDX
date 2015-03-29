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
    [Shadow(typeof(SampleGrabberSinkCallback2Shadow))]
    public partial interface SampleGrabberSinkCallback2 : SampleGrabberSinkCallback
    {
        /// <summary>	
        /// Called when the sample-grabber sink receives a new media sample.
        /// </summary>	
        /// <param name="guidMajorMediaType"><para>The major type that specifies the format of the data. For a list of possible values, see Major Media Types.</para></param>	
        /// <param name="dwSampleFlags"><para>Reserved.</para></param>	
        /// <param name="llSampleTime"><para>The presentation time for this sample, in 100-nanosecond units. If the sample does not have a presentation time, the value of this parameter is _I64_MAX.</para></param>	
        /// <param name="llSampleDuration"><para>The duration of the sample, in 100-nanosecond units. If the sample does not have a duration, the value of this parameter is _I64_MAX.</para></param>	
        /// <param name="sampleBufferRef"><para>A reference to a buffer that contains the sample data.</para></param>	
        /// <param name="dwSampleSize"><para>Size of the pSampleBuffer buffer, in bytes.</para></param>	
        /// <param name="attributesRef"><para>A reference to the MediaAttributes interface. Use this interface to get the attributes for this sample (if any). For a list of sample attributes, see Sample Attributes.</para></param>	
        /// <remarks>	
        /// If you use the sample-grabber sink in a playback topology, this method should return quickly, or it might interfere with playback. Do not block the thread, wait on events, or perform other lengthy operations inside this method.
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFSampleGrabberSinkCallback2::OnProcessSampleEx']/*"/>	
        /// <unmanaged>HRESULT IMFSampleGrabberSinkCallback2::OnProcessSampleEx([In] const GUID& guidMajorMediaType,[In] unsigned int dwSampleFlags,[In] longlong llSampleTime,[In] longlong llSampleDuration,[In, Buffer] const unsigned char* pSampleBuffer,[In] unsigned int dwSampleSize,[In] IMFAttributes* pAttributes)</unmanaged>
        void OnProcessSampleEx(Guid guidMajorMediaType, int dwSampleFlags, long llSampleTime, long llSampleDuration, IntPtr pSampleBuffer, int dwSampleSize, MediaAttributes attributesRef);
    }
}