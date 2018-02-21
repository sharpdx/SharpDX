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

namespace SharpDX.MediaFoundation
{
    public partial class MediaEventGenerator
    {
        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Retrieves the next event in the queue. This method is synchronous.</p>	
        /// </summary>	
        /// <param name="isBlocking"><c>true</c> if the method blocks until the event generator queues an event, <c>false</c> otherwise.</param>
        /// <returns>a reference to the <strong><see cref="SharpDX.MediaFoundation.MediaEvent"/></strong> interface. The caller must release the interface.</returns>	
        /// <remarks>	
        /// <p>This method executes synchronously.</p><p>If the queue already contains an event, the method returns <see cref="SharpDX.Result.Ok"/> immediately. If the queue does not contain an event, the behavior depends on the value of <em>dwFlags</em>:</p><ul> <li> <p>If <em>dwFlags</em> is 0, the method blocks indefinitely until a new event is queued, or until the event generator is shut down.</p> </li> <li> <p>If <em>dwFlags</em> is MF_EVENT_FLAG_NO_WAIT, the method fails immediately with the return code <see cref="SharpDX.MediaFoundation.ResultCode.NoEventsAvailable"/>.</p> </li> </ul><p>This method returns <see cref="SharpDX.MediaFoundation.ResultCode.MultipleSubScribers"/> if you previously called <strong><see cref="SharpDX.MediaFoundation.MediaEventGenerator.BeginGetEvent_"/></strong> and have not yet called <strong><see cref="SharpDX.MediaFoundation.MediaEventGenerator.EndGetEvent"/></strong>.</p>	
        /// </remarks>	
        /// <msdn-id>ms704754</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaEventGenerator::GetEvent([In] unsigned int dwFlags,[Out] IMFMediaEvent** ppEvent)</unmanaged>	
        /// <unmanaged-short>IMFMediaEventGenerator::GetEvent</unmanaged-short>	
        public MediaEvent GetEvent(bool isBlocking)
        {
            MediaEvent mediaEvent;
            GetEvent(isBlocking ? 0 : 1, out mediaEvent);
            return mediaEvent;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Begins an asynchronous request for the next event in the queue.</p>	
        /// </summary>	
        /// <param name="callback"><dd> <p>Pointer to the <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback"/></strong> interface of a callback object. The client must implement this interface.</p> </dd></param>	
        /// <param name="stateObject">A reference to a state object, defined by the caller. This parameter can be <strong><c>null</c></strong>. You can use this object to hold state information. The object is returned to the caller when the callback is invoked.</param>	
        /// <remarks>	
        /// <p>When a new event is available, the event generator calls the <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback.Invoke"/></strong> method. The <strong>Invoke</strong> method should call <strong><see cref="SharpDX.MediaFoundation.MediaEventGenerator.EndGetEvent"/></strong> to get a reference to the <strong><see cref="SharpDX.MediaFoundation.MediaEvent"/></strong> interface, and use that interface to examine the event.</p><p>Do not call <strong>BeginGetEvent</strong> a second time before calling <strong>EndGetEvent</strong>. While the first call is still pending, additional calls to the same object will fail. Also, the <strong><see cref="SharpDX.MediaFoundation.MediaEventGenerator.GetEvent"/></strong> method fails if an asynchronous request is still pending.</p>	
        /// </remarks>	
        /// <msdn-id>ms701637</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaEventGenerator::BeginGetEvent([In] IMFAsyncCallback* pCallback,[In] void* punkState)</unmanaged>	
        /// <unmanaged-short>IMFMediaEventGenerator::BeginGetEvent</unmanaged-short>	
        public void BeginGetEvent(IAsyncCallback callback, object stateObject)
        {
            BeginGetEvent(callback, stateObject == null ? IntPtr.Zero : Marshal.GetIUnknownForObject(stateObject));
        }
    }
}