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
    public partial class AsyncResult
    {
        private object state;
        private bool isStateVerified;

        /// <summary>
        /// Gets the state object specified by the caller in the asynchronous <strong>Begin</strong> method. If the value is not <strong><c>null</c></strong>, the caller must dispose.
        /// </summary>
        /// <value>The state.</value>
        /// <remarks>	
        /// <p>The caller of the asynchronous method specifies the state object, and can use it for any caller-defined purpose. The state object can be <strong><c>null</c></strong>. If the state object is <strong><c>null</c></strong>, <strong>GetState</strong> returns <strong>E_POINTER</strong>.</p><p>If you are implementing an asynchronous method, set the state object on the through the <em>punkState</em> parameter of the <strong><see cref="SharpDX.MediaFoundation.MediaFactory.CreateAsyncResult"/></strong> function.</p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>bb970576</msdn-id>	
        /// <unmanaged>HRESULT IMFAsyncResult::GetState([Out] IUnknown** ppunkState)</unmanaged>	
        /// <unmanaged-short>IMFAsyncResult::GetState</unmanaged-short>	
        public object State
        {
            get
            {
                if (!isStateVerified)
                {
                    IntPtr statePtr;
                    GetState(out statePtr);
                    if (statePtr != IntPtr.Zero)
                    {
                        state = Marshal.GetObjectForIUnknown(statePtr);
                        Marshal.Release(statePtr);
                    }
                    isStateVerified = true;
                }
                return state;
            }
        }

        /// <summary>	
        /// <p>Get or sets the status of the asynchronous operation.</p>	
        /// </summary>	
        /// <value><p>The method returns an <strong><see cref="SharpDX.Result"/></strong>. Possible values include, but are not limited to, those in the following table.</p><table> <tr><th>Return code</th><th>Description</th></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.Result.Ok"/></strong></dt> </dl> </td><td> <p>The operation completed successfully.</p> </td></tr> </table><p>?</p></value>	
        /// <remarks>	
        /// <p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms702095</msdn-id>	
        /// <unmanaged>HRESULT IMFAsyncResult::GetStatus()</unmanaged>	
        /// <unmanaged-short>IMFAsyncResult::GetStatus</unmanaged-short>	
        public SharpDX.Result Status
        {
            get { return GetStatus(); }
            set { SetStatus(value); }
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Returns an object associated with the asynchronous operation. The type of object, if any, depends on the asynchronous method that was called.</p>	
        /// </summary>	
        /// <value><dd> <p>Receives a reference to the object's <strong><see cref="SharpDX.ComObject"/></strong> interface. If no object is associated with the operation, this parameter receives the value <strong><c>null</c></strong>. If the value is not <strong><c>null</c></strong>, the caller must release the interface.</p> </dd></value>	
        /// <remarks>	
        /// <p>Typically, this object is used by the component that implements the asynchronous method. It provides a way for the function that invokes the callback to pass information to the asynchronous <strong>End...</strong> method that completes the operation.</p><p>If you are implementing an asynchronous method, you can set the object through the <em>punkObject</em> parameter of the <strong><see cref="SharpDX.MediaFoundation.MediaFactory.CreateAsyncResult"/></strong> function.</p><p>If the asynchronous result object's internal <strong><see cref="SharpDX.ComObject"/></strong> reference is <strong><c>null</c></strong>, the method returns <strong>E_POINTER</strong>.</p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>bb970500</msdn-id>	
        /// <unmanaged>HRESULT IMFAsyncResult::GetObjectW([Out] IUnknown** ppObject)</unmanaged>	
        /// <unmanaged-short>IMFAsyncResult::GetObjectW</unmanaged-short>	
        public IUnknown PrivateObject
        {
            get
            {
                GetObject(out IUnknown privateObject);
                return privateObject;
            }
        }
    }
}