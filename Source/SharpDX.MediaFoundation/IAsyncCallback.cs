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

namespace SharpDX.MediaFoundation
{
    [Shadow(typeof(AsyncCallbackShadow))]
    public partial interface IAsyncCallback
    {
        /// <summary>
        /// Gets a flag indicating the behavior of the callback object's <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback.Invoke" /></strong> method. Default behavior should be <see cref="AsyncCallbackFlags.None"/>.
        /// </summary>
        /// <value>The a flag indicating the behavior of the callback object's <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback.Invoke" /></strong> method.</value>
        ///   <msdn-id>bb970381</msdn-id>
        ///   <unmanaged>HRESULT IMFAsyncCallback::GetParameters([Out] MFASYNC_CALLBACK_FLAGS* pdwFlags,[Out] unsigned int* pdwQueue)</unmanaged>
        ///   <unmanaged-short>IMFAsyncCallback::GetParameters</unmanaged-short>
        SharpDX.MediaFoundation.AsyncCallbackFlags Flags { get; }

        /// <summary>
        /// Gets the identifier of the work queue on which the callback is dispatched. See remarks.
        /// </summary>
        /// <value>The work queue identifier.</value>
        /// <remarks>
        /// <p>This value can specify one of the standard Media Foundation work queues, or a work queue created by the application. For list of standard Media Foundation work queues, see <strong>Work Queue Identifiers</strong>. To create a new work queue, call <strong><see cref="SharpDX.MediaFoundation.MediaFactory.AllocateWorkQueue" /></strong>. The default value is <strong><see cref="SharpDX.MediaFoundation.WorkQueueType.Standard" /></strong>.</p> <p>If the work queue is not compatible with the value returned in <em>pdwFlags</em>, the Media Foundation platform returns <strong><see cref="SharpDX.MediaFoundation.ResultCode.InvalidWorkqueue" /></strong> when it tries to dispatch the callback. (See <strong><see cref="SharpDX.MediaFoundation.MediaFactory.PutWorkItem" /></strong>.)</p>
        /// </remarks>
        ///   <msdn-id>bb970381</msdn-id>
        ///   <unmanaged>HRESULT IMFAsyncCallback::GetParameters([Out] MFASYNC_CALLBACK_FLAGS* pdwFlags,[Out] unsigned int* pdwQueue)</unmanaged>
        ///   <unmanaged-short>IMFAsyncCallback::GetParameters</unmanaged-short>
        WorkQueueId WorkQueueId { get; }
        
        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Called when an asynchronous operation is completed.</p>	
        /// </summary>	
        /// <param name="asyncResultRef"><dd> <p>Pointer to the <strong><see cref="SharpDX.MediaFoundation.AsyncResult"/></strong> interface. Pass this reference to the asynchronous <strong>End...</strong> method to complete the asynchronous call.</p> </dd></param>	
        /// <returns><p>The method returns an <strong><see cref="SharpDX.Result"/></strong>. Possible values include, but are not limited to, those in the following table.</p><table> <tr><th>Return code</th><th>Description</th></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.Result.Ok"/></strong></dt> </dl> </td><td> <p>The method succeeded.</p> </td></tr> </table><p>?</p></returns>	
        /// <remarks>	
        /// <p>Within your implementation of <strong>Invoke</strong>, call the corresponding <strong>End...</strong> method.</p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFAsyncCallback::Invoke']/*"/>	
        /// <msdn-id>bb970360</msdn-id>	
        /// <unmanaged>HRESULT IMFAsyncCallback::Invoke([In, Optional] IMFAsyncResult* pAsyncResult)</unmanaged>	
        /// <unmanaged-short>IMFAsyncCallback::Invoke</unmanaged-short>	
        void Invoke(SharpDX.MediaFoundation.AsyncResult asyncResultRef);
    }
}