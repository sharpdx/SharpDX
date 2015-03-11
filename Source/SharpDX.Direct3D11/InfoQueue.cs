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

namespace SharpDX.Direct3D11
{
    public partial class InfoQueue
    {
        /// <summary>	
        /// <p>Get a message from the message queue.</p>	
        /// </summary>	
        /// <param name="messageIndex"><dd>  <p>Index into message queue after an optional retrieval filter has been applied. This can be between 0 and the number of messages in the message queue that pass through the retrieval filter (which can be obtained with <strong><see cref="SharpDX.Direct3D11.InfoQueue.GetNumStoredMessagesAllowedByRetrievalFilter"/></strong>). 0 is the message at the front of the message queue.</p> </dd></param>	
        /// <returns>Returned message (see <strong><see cref="SharpDX.Direct3D11.Message"/></strong>)</returns>	
        /// <msdn-id>ff476549</msdn-id>	
        /// <unmanaged>HRESULT ID3D11InfoQueue::GetMessageW([In] unsigned longlong MessageIndex,[In] void* pMessage,[InOut] SIZE_T* pMessageByteLength)</unmanaged>	
        /// <unmanaged-short>ID3D11InfoQueue::GetMessageW</unmanaged-short>	
        public unsafe Message GetMessage(long messageIndex)
        {
            PointerSize messageSize = 0;
            GetMessage(messageIndex, IntPtr.Zero, ref messageSize);

            if(messageSize == 0)
            {
                return new Message();
            }

            var messagePtr = stackalloc byte[(int)messageSize];
            GetMessage(messageIndex, new IntPtr(messagePtr), ref messageSize);

            var message = new Message();
            message.__MarshalFrom(ref *(Message.__Native*)messagePtr);
            return message;
        }

        /// <summary>	
        /// <p>Get the storage filter at the top of the storage-filter stack.</p>	
        /// </summary>	
        /// <returns>The storage filter at the top of the storage-filter stack.</returns>	
        /// <msdn-id>ff476560</msdn-id>	
        /// <unmanaged>HRESULT ID3D11InfoQueue::GetStorageFilter([In] void* pFilter,[InOut] SIZE_T* pFilterByteLength)</unmanaged>	
        /// <unmanaged-short>ID3D11InfoQueue::GetStorageFilter</unmanaged-short>	
        public unsafe SharpDX.Direct3D11.InfoQueueFilter GetStorageFilter()
        {
            var sizeFilter = PointerSize.Zero;
            GetStorageFilter(IntPtr.Zero, ref sizeFilter);

            if(sizeFilter == 0)
            {
                return null;
            }
            var filter = stackalloc byte[(int)sizeFilter];
            GetStorageFilter((IntPtr)filter, ref sizeFilter);

            var queueNative = new InfoQueueFilter();
            queueNative.__MarshalFrom(ref *(InfoQueueFilter.__Native*)filter);

            return queueNative;
        }

        /// <summary>	
        /// <p>Get the retrieval filter at the top of the retrieval-filter stack.</p>	
        /// </summary>	
        /// <returns>The retrieval filter at the top of the retrieval-filter stack.</returns>	
        /// <msdn-id>ff476558</msdn-id>	
        /// <unmanaged>HRESULT ID3D11InfoQueue::GetRetrievalFilter([In] void* pFilter,[InOut] SIZE_T* pFilterByteLength)</unmanaged>	
        /// <unmanaged-short>ID3D11InfoQueue::GetRetrievalFilter</unmanaged-short>	
        public unsafe SharpDX.Direct3D11.InfoQueueFilter GetRetrievalFilter()
        {
            var sizeFilter = PointerSize.Zero;
            GetRetrievalFilter(IntPtr.Zero, ref sizeFilter);

            if (sizeFilter == 0)
            {
                return null;
            }
            var filter = stackalloc byte[(int)sizeFilter];
            GetRetrievalFilter((IntPtr)filter, ref sizeFilter);

            var queueNative = new InfoQueueFilter();
            queueNative.__MarshalFrom(ref *(InfoQueueFilter.__Native*)filter);

            return queueNative;
        }
    }
}
