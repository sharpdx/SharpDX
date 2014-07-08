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

namespace SharpDX.Direct3D10
{
    public partial class InfoQueue
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="messageIndex">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D10InfoQueue::GetMessageW']/*"/>	
        /// <unmanaged>HRESULT ID3D10InfoQueue::GetMessageW([In] unsigned longlong MessageIndex,[Out, Buffer, Optional] D3D10_MESSAGE* pMessage,[InOut] SIZE_T* pMessageByteLength)</unmanaged>	
        public unsafe Message GetMessage(long messageIndex)
        {
            PointerSize messageSize = 0;
            GetMessage(messageIndex, IntPtr.Zero, ref messageSize);

            var messagePtr = stackalloc byte[(int) messageSize];
            GetMessage(messageIndex, new IntPtr(messagePtr), ref messageSize);

            var message = new Message();
            message.__MarshalFrom(ref *(Message.__Native*)messagePtr);
            return message;
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D10InfoQueue::GetStorageFilter']/*"/>	
        /// <unmanaged>HRESULT ID3D10InfoQueue::GetStorageFilter([Out, Buffer, Optional] D3D10_INFO_QUEUE_FILTER* pFilter,[InOut] SIZE_T* pFilterByteLength)</unmanaged>	
        public SharpDX.Direct3D10.InfoQueueFilter GetStorageFilter()
        {
            throw new NotImplementedException();
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D10InfoQueue::GetRetrievalFilter']/*"/>	
        /// <unmanaged>HRESULT ID3D10InfoQueue::GetRetrievalFilter([Out, Buffer, Optional] D3D10_INFO_QUEUE_FILTER* pFilter,[InOut] SIZE_T* pFilterByteLength)</unmanaged>	
        public SharpDX.Direct3D10.InfoQueueFilter GetRetrievalFilter()
        {
            throw new NotImplementedException();
        }
    }
}

