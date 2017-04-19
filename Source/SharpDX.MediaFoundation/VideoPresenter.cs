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

#if DESKTOP_APP

using System;

namespace SharpDX.MediaFoundation
{
    [Shadow(typeof(VideoPresenterShadow))]
    public partial interface VideoPresenter : ClockStateSink
    {
        /// <summary>	
        /// Retrieves the presenter's media type.
        /// </summary>	
        /// <param name="ppMediaType"><para>Receives a pointer to the IMFVideoMediaType interface. The caller must release the interface.</para></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFVideoPresenter::GetCurrentMediaType']/*"/>	
        /// <unmanaged>HRESULT IMFVideoPresenter::GetCurrentMediaType([out] IMFVideoMediaType **ppMediaType</unmanaged>
        VideoMediaType CurrentMediaType { get; }

        /// <summary>	
        /// Sends a message to the video presenter. Messages are used to signal the presenter that it must perform some action, or that some event has occurred.
        /// </summary>	
        /// <param name="eMessage"><para>Specifies the message as a member of the VpMessageType enumeration.</para></param>	
        /// <param name="ulParam"><para>Message parameter. The meaning of this parameter depends on the message type.</para></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFVideoPresenter::ProcessMessage']/*"/>	
        /// <unmanaged>HRESULT IMFVideoPresenter::ProcessMessage([In] MFVP_MESSAGE_TYPE eMessage,[In] ULONG_PTR ulParam)</unmanaged>
        void ProcessMessage(VpMessageType eMessage, IntPtr ulParam);
    }
}

#endif